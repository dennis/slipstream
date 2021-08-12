#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.WinFormUI.Events;
using Slipstream.Components.WinFormUI.Lua;
using Slipstream.Shared;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using EventHandler = Slipstream.Shared.EventHandlerController;

namespace Slipstream.Components.WinFormUI.Forms
{
    public partial class MainWindow : Form
    {
        private enum NodeTypeEnum
        {
            None,
            Generic,
            LuaScripts,
            Instance,
            Dependency
        }

        private class InsideViewNodeTag
        {
            public NodeTypeEnum NodeType { get; private set; }
            public bool EventFilter { get => NodeType == NodeTypeEnum.Instance || NodeType == NodeTypeEnum.Dependency || NodeType == NodeTypeEnum.LuaScripts; }

            public InsideViewNodeTag(NodeTypeEnum type)
            {
                NodeType = type;
            }

            public static TreeNode InstanceNode(string text)
            {
                return new TreeNode(text)
                {
                    Name = text,
                    Tag = new InsideViewNodeTag(NodeTypeEnum.Instance),
                };
            }

            public static TreeNode DependencyNode(string text)
            {
                return new TreeNode(text)
                {
                    Name = text,
                    Tag = new InsideViewNodeTag(NodeTypeEnum.Dependency),
                };
            }
        }

        private class EventRepository
        {
            private readonly List<IEvent> Events = new List<IEvent>();
            private readonly DataGridView EventGridView;
            private readonly IEventSerdeService EventSerdeService;
            private readonly Label EventFilterDescriptionLabel;
            private readonly ContextMenuStrip ContextMenu;
            private readonly TabPage EventsTabPage;
            private IEventFilter SelectedFilter = new NoneEventFilter();

            private const int MaxEventsStored = 1000;

            public EventRepository(DataGridView eventGridView, Label eventFilterDescriptionLabel, ContextMenuStrip eventViewerContextMenuStrip, TabPage eventsTabPage, IEventSerdeService eventSerdeService)
            {
                EventGridView = eventGridView;
                EventSerdeService = eventSerdeService;
                EventFilterDescriptionLabel = eventFilterDescriptionLabel;
                ContextMenu = eventViewerContextMenuStrip;
                EventsTabPage = eventsTabPage;
            }

            public void Add(IEvent e)
            {
                while (Events.Count > MaxEventsStored)
                {
                    Events.RemoveAt(0);
                }
                Events.Add(e);
                AddToControl(e);
            }

            private void AddToControl(IEvent e)
            {
                if (SelectedFilter.Accept(e))
                {
                    string recipients = RecipientsAsString(e);
                    var json = EventSerdeService.Serialize(e);

                    EventGridView.Rows.Add(e.Envelope.Uptime, e.EventType, e.Envelope.Sender, recipients, json);
                    EventGridView.Rows[^1].ContextMenuStrip = ContextMenu;
                }
            }

            private static string RecipientsAsString(IEvent e)
            {
                if (e.Envelope.Recipients == null || e.Envelope.Recipients.Length == 0)
                {
                    return "*";
                }
                else
                {
                    return string.Join(", ", e.Envelope.Recipients);
                }
            }

            private interface IEventFilter
            {
                bool Accept(IEvent e);
            }

            private class NoneEventFilter : IEventFilter
            {
                public bool Accept(IEvent e)
                {
                    return false;
                }
            }

            private class AllEventFilter : IEventFilter
            {
                public bool Accept(IEvent e)
                {
                    return true;
                }
            }

            private class DependencyEventFilter : IEventFilter
            {
                private readonly string SelectedNodeInstanceId;
                private readonly string SelectedNodeDependency;

                public DependencyEventFilter(string instanceId, string dependency)
                {
                    SelectedNodeInstanceId = instanceId;
                    SelectedNodeDependency = dependency;
                }

                public bool Accept(IEvent e)
                {
                    return e.Envelope.ContainsRecipient(SelectedNodeInstanceId) && e.Envelope.Sender == SelectedNodeDependency;
                }
            }

            private class InstanceEventFilter : IEventFilter
            {
                private readonly string SelectedNodeInstanceId;

                public InstanceEventFilter(string instanceId)
                {
                    SelectedNodeInstanceId = instanceId;
                }

                public bool Accept(IEvent e)
                {
                    return e.Envelope.ContainsRecipient(SelectedNodeInstanceId);
                }
            }

            internal void Selected(TreeNode node)
            {
                if (!(node.Tag is InsideViewNodeTag tag))
                    return;

                switch (tag.NodeType)
                {
                    case NodeTypeEnum.Dependency:
                        SelectedFilter = new DependencyEventFilter(node.Parent.Name, node.Name);
                        EventFilterDescriptionLabel.Text = $"Show events filtered by recipient '{node.Parent.Name}' and sender '{node.Name}'";
                        EventsTabPage.Text = $"Events between '{node.Parent.Name}' and '{node.Name}'";
                        break;

                    case NodeTypeEnum.Instance:
                        SelectedFilter = new InstanceEventFilter(node.Name);
                        EventFilterDescriptionLabel.Text = $"Show events filtered by recipient '{node.Name}'";
                        EventsTabPage.Text = $"Events for '{node.Name}'";
                        break;

                    case NodeTypeEnum.LuaScripts:
                        SelectedFilter = new AllEventFilter();
                        EventFilterDescriptionLabel.Text = "Showing all events";
                        EventsTabPage.Text = $"Events";
                        break;

                    default:
                        SelectedFilter = new NoneEventFilter();
                        EventFilterDescriptionLabel.Text = "";
                        EventsTabPage.Text = $"Events";
                        break;
                }

                EventGridView.Rows.Clear();
                foreach (var e in Events)
                {
                    AddToControl(e);
                }
            }
        }

        private Thread? EventHandlerThread;
        private readonly IEventBus EventBus;
        private readonly string InstanceId;
        private readonly WinFormUIInstanceThread Instance;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IWinFormUIEventFactory UIEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private IEventBusSubscription? EventBusSubscription;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, Button> LuaButtons = new Dictionary<string, Button>();
        private readonly IEventHandlerController EventHandler;
        private bool ShuttingDown = false;
        private IEventEnvelope Envelope;
        private readonly IEventEnvelope BroadcastEnvelope;
        private readonly CancellationTokenSource EventHandlerThreadCts = new CancellationTokenSource();
        private readonly TreeNode LuaScriptTreeNode;
        private CancellationToken? EventHandlerThreadCancellationToken;
        private readonly EventRepository EventsCollected;
        private readonly IEventSerdeService EventSerdeService;

        private DataGridViewCellEventArgs? EventViewerMouseLocation;

        public MainWindow(
            string instanceId,
            WinFormUIInstanceThread instance,
            IInternalEventFactory internalEventFactory,
            IWinFormUIEventFactory uiEventFactory,
            IPlaybackEventFactory playbackEventFactory,
            IEventBus eventBus,
            IApplicationVersionService applicationVersionService,
            IEventHandlerController eventHandlerController,
            IEventSerdeService eventSerdeService
            )
        {
            InstanceId = instanceId;
            Instance = instance;
            InternalEventFactory = internalEventFactory;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;
            EventHandler = eventHandlerController;
            EventBus = eventBus;
            EventSerdeService = eventSerdeService;
            Envelope = new EventEnvelope(instanceId);
            BroadcastEnvelope = new EventEnvelope(instanceId);

            InitializeComponent();

            EventGridView.Columns.Add("uptime", "Uptime");
            EventGridView.Columns.Add("eventname", "Event Name");
            EventGridView.Columns.Add("sender", "Sender");
            EventGridView.Columns.Add("recipients", "Recipients");
            EventGridView.Columns.Add("json", "JSON");
            EventGridView.Columns[^1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            EventsCollected = new EventRepository(EventGridView, EventFilterDescriptionLabel, EventViewerContextMenuStrip, EventsTabPage, eventSerdeService);

            AboutTextBox.Text = "Slipstream version v" + applicationVersionService.Version;

            // Find Root Nodes of InsideView:
            foreach (TreeNode node in InsideView.Nodes)
            {
                node.Tag = new InsideViewNodeTag(NodeTypeEnum.LuaScripts);

                if (node.Name == "LuaScripts")
                {
                    LuaScriptTreeNode = node;
                    node.Tag = new InsideViewNodeTag(NodeTypeEnum.LuaScripts);
                }
            }

            Debug.Assert(LuaScriptTreeNode != null);

            InsideView.BeginUpdate();
            LuaScriptTreeNode.Expand();
            InsideView.EndUpdate();

            EventsCollected.Selected(LuaScriptTreeNode);

            Text += " v" + applicationVersionService.Version;

            Load += MainWindow_Load;
            FormClosing += MainWindow_FormClosing;
        }

        private void MainWindow_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!ShuttingDown)
            {
                // Just request we want to shut down. We'll receive an InternalShutdown if we
                // actually will shut down

                EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandShutdown(BroadcastEnvelope));
                e.Cancel = true;

                return;
            }

            EventBusSubscription?.Dispose();
            EventBusSubscription = null;
            EventHandlerThreadCts.Cancel();
            EventHandlerThread?.Join();
            EventHandlerThread = null;
        }

        private void MainWindow_Load(object? sender, EventArgs e)
        {
            EventBusSubscription = EventBus.RegisterListener(InstanceId, fromBeginning: true, promiscuousMode: true);
            EventHandlerThreadCancellationToken = EventHandlerThreadCts.Token;
            EventHandlerThread = new Thread(new ThreadStart(EventListenerMain))
            {
                Name = "EventListenerMain"
            };
            EventHandlerThread.Start();
        }

        private void AppendMessages(string msg)
        {
            ExecuteSecure(() => LogAreaTextBox.AppendText(msg));
        }

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        private void LogMessageUpdate_Tick(object sender, EventArgs e)
        {
            string allMesssages = string.Empty;

            while (PendingMessages.TryTake(out string? msg))
            {
                allMesssages += msg + "\r\n";
            }

            AppendMessages(allMesssages);

            if (Instance.IsStopping())
            {
                ShuttingDown = true;
                ExecuteSecure(() => Application.Exit());
            }
        }

        #region EventHandlerThread methods

        private void EventListenerMain()
        {
            Debug.Assert(EventBusSubscription != null);
            Debug.Assert(EventHandlerThreadCancellationToken != null);

            var internalEventHandler = EventHandler.Get<Internal.EventHandler.Internal>();
            var uiEventHandler = EventHandler.Get<EventHandler.WinFormUIEventHandler>();

            uiEventHandler.OnWinFormUICommandWriteToConsole += (_, e) => PendingMessages.Add($"{DateTime.Now:s} {e.Message}");
            uiEventHandler.OnWinFormUICommandCreateButton += (_, e) => EventHandler_OnWinFormUICommandCreateButton(e);
            uiEventHandler.OnWinFormUICommandDeleteButton += (_, e) => EventHandler_OnWinFormUICommandDeleteButton(e);
            internalEventHandler.OnInternalInstanceAdded += (_, e) => EventHandler_OnInternalInstanceAdded(e.LuaLibrary, e.InstanceId);
            internalEventHandler.OnInternalInstanceRemoved += (_, e) => EventHandler_OnInternalInstanceRemoved(e.LuaLibrary, e.InstanceId);
            internalEventHandler.OnInternaDependencyAdded += (_, e) => EventHandler_OnInternaDependencyAdded(e.LuaLibrary, e.InstanceId, e.DependsOn);
            internalEventHandler.OnInternalDependencyRemoved += (_, e) => EventHandler_OnInternalDependencyRemoved(e.LuaLibrary, e.InstanceId, e.DependsOn);

            var token = (CancellationToken)EventHandlerThreadCancellationToken; // We got a Assert ensuring this isn't null

            while (!token.IsCancellationRequested)
            {
                var @event = EventBusSubscription?.NextEvent(250);
                if (@event != null)
                {
                    ExecuteSecure(() => EventsCollected.Add(@event));
                    EventHandler.HandleEvent(@event);
                }
            }
        }

        private void EventHandler_OnInternalDependencyRemoved(string luaLibraryName, string instanceId, string dependsOn)
        {
            // something stopped begin dependant on us, so remove it from our Envelope
            if (dependsOn == InstanceId)
                Envelope = Envelope.Remove(instanceId);

            // Populating "InsideView" TreeView
            if (luaLibraryName != "api/lua")
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                foreach (TreeNode instanceNode in LuaScriptTreeNode.Nodes)
                {
                    if (instanceNode.Name == instanceId)
                    {
                        foreach (TreeNode dependsOnNode in instanceNode.Nodes)
                        {
                            if (dependsOnNode.Name == dependsOn)
                            {
                                dependsOnNode.Remove();
                                break;
                            }
                        }
                        break;
                    }
                }

                InsideView.EndUpdate();
            });
        }

        private void EventHandler_OnInternaDependencyAdded(string luaLibraryName, string instanceId, string dependsOn)
        {
            // something is depending on us, so add it to the receivers of our events
            if (dependsOn == InstanceId)
                Envelope = Envelope.Add(instanceId);

            // Populating "InsideView" TreeView
            if (luaLibraryName != "api/lua")
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                foreach (TreeNode instanceNode in LuaScriptTreeNode.Nodes)
                {
                    if (instanceNode.Name == instanceId)
                    {
                        instanceNode.Nodes.Add(InsideViewNodeTag.DependencyNode(dependsOn));
                        break;
                    }
                }

                InsideView.EndUpdate();
            });
        }

        private void EventHandler_OnInternalInstanceAdded(string luaLibraryName, string instanceId)
        {
            if (luaLibraryName != "api/lua")
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                LuaScriptTreeNode.Nodes.Add(InsideViewNodeTag.InstanceNode(instanceId));

                InsideView.EndUpdate();
            });
        }

        private void EventHandler_OnInternalInstanceRemoved(string luaLibraryName, string instanceId)
        {
            if (luaLibraryName != "api/lua")
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                foreach (TreeNode instanceNode in LuaScriptTreeNode.Nodes)
                {
                    if (instanceNode.Name == instanceId)
                    {
                        instanceNode.Remove();
                        break;
                    }
                }

                InsideView.EndUpdate();
            });
        }

        private void EventHandler_OnWinFormUICommandCreateButton(WinFormUICommandCreateButton @event)
        {
            ExecuteSecure(() =>
            {
                if (LuaButtons.ContainsKey(@event.Text))
                    return;

                var b = new Button
                {
                    Text = @event.Text
                };

                b.Click += (s, e) =>
                {
                    if (s is Button b)
                        EventBus.PublishEvent(UIEventFactory.CreateWinFormUIButtonTriggered(Envelope, b.Text));
                };

                LuaButtons.Add(@event.Text, b);

                ButtonFlowLayoutPanel.Controls.Add(b);
            });
        }

        private void EventHandler_OnWinFormUICommandDeleteButton(WinFormUICommandDeleteButton @event)
        {
            ExecuteSecure(() =>
            {
                if (!LuaButtons.ContainsKey(@event.Text))
                    return;

                ButtonFlowLayoutPanel.Controls.Remove(LuaButtons[@event.Text]);
                LuaButtons.Remove(@event.Text);
            });
        }

        #endregion EventHandlerThread methods

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandShutdown(Envelope));
        }

        private void SaveEventsToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                EventBus.PublishEvent(PlaybackEventFactory.CreatePlaybackCommandSaveEvents(Envelope, SaveFileDialog.FileName));
            }
        }

        private void LoadEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                EventBus.PublishEvent(PlaybackEventFactory.CreatePlaybackCommandInjectEvents(Envelope, OpenFileDialog.FileName));
            }
        }

        private void OpenDataDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }

        private void TestEventsMenuItem_Click(object sender, EventArgs e)
        {
            EventTestWindow etw = new EventTestWindow(EventBus);
            etw.Show(this);
        }

        private void InsideView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown || e.Action == TreeViewAction.Collapse || e.Action == TreeViewAction.Expand)
                return;

            if (!(e.Node.Tag is InsideViewNodeTag nodeTag))
                return;

            if (nodeTag.EventFilter)
            {
                EventsCollected.Selected(e.Node);
            }
        }

        private void EventGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            EventViewerMouseLocation = location;
        }

        private void EventViewerResendMenuItem_Click(object sender, EventArgs e)
        {
            if (EventViewerMouseLocation == null)
                return;

            if (!(EventGridView
                .Rows[EventViewerMouseLocation.RowIndex]
                .Cells[EventGridView.Rows[EventViewerMouseLocation.RowIndex].Cells.Count - 1]
                .Value is string json))
                return;

            var @event = EventSerdeService.Deserialize(json);
            if (@event == null)
                return;
            EventBus.PublishEvent(@event);
        }

        private void EventViewerCopyJsonMenuItem_Click(object sender, EventArgs e)
        {
            if (EventViewerMouseLocation == null)
                return;

            if (!(EventGridView
                .Rows[EventViewerMouseLocation.RowIndex]
                .Cells[EventGridView.Rows[EventViewerMouseLocation.RowIndex].Cells.Count - 1]
                .Value is string json))
                return;

            CopyToClipBoard(json);
        }

        private static void CopyToClipBoard(string str)
        {
            // https://stackoverflow.com/questions/17762037/current-thread-must-be-set-to-single-thread-apartment-sta-error-in-copy-stri
            Thread thread = new Thread(() => Clipboard.SetText(str));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void CopyLuaHandlerCodeMenuItem_Click(object sender, EventArgs e)
        {
            if (EventViewerMouseLocation == null)
                return;

            var rows = EventGridView.Rows[EventViewerMouseLocation.RowIndex];

            if (!(rows
                .Cells[EventGridView.Rows[EventViewerMouseLocation.RowIndex].Cells.Count - 1]
                .Value is string json))
                return;

            if (!(rows.Cells[1].Value is string eventType))
                return;

            var code = $@"
addEventHandler(""{eventType}"", function(event)
    -- Example event: {json}
    -- your code here
end)
";
            CopyToClipBoard(code);
        }
    }
}