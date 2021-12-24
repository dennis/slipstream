#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.WinFormUI.Events;
using Slipstream.Components.WinFormUI.Forms.Helpers;
using Slipstream.Components.WinFormUI.Lua;
using Slipstream.Shared;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using EventHandler = Slipstream.Shared.EventHandlerController;

namespace Slipstream.Components.WinFormUI.Forms
{
    public partial class MainWindow : Form
    {
        private Thread? EventHandlerThread;
        private readonly IEventBus EventBus;
        private readonly string InstanceId;
        private readonly WinFormUIInstanceThread Instance;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IWinFormUIEventFactory UIEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private IEventBusSubscription? EventBusSubscription;
        private readonly BlockingCollection<WinFormUICommandWriteToConsole> PendingMessages = new BlockingCollection<WinFormUICommandWriteToConsole>();
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
        private readonly bool DeepView = false;

        private DataGridViewCellEventArgs? EventViewerMouseLocation;
        private readonly TreeNode? DeepViewInstanceNode;
        private readonly Dictionary<string, ToolStripMenuItem> EndpointMenuItems = new Dictionary<string, ToolStripMenuItem>();

        public MainWindow(
            string instanceId,
            WinFormUIInstanceThread instance,
            IInternalEventFactory internalEventFactory,
            IWinFormUIEventFactory uiEventFactory,
            IPlaybackEventFactory playbackEventFactory,
            IEventBus eventBus,
            IApplicationVersionService applicationVersionService,
            IEventHandlerController eventHandlerController,
            IEventSerdeService eventSerdeService,
            IEventBusSubscription subscription,
            bool deepView
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
            DeepView = deepView;

            EventBusSubscription = subscription;

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

            if (DeepView)
            {
                InsideView.Nodes.Add("Instances");
                DeepViewInstanceNode = InsideView.Nodes[^1];
            }

            InsideView.EndUpdate();

            ResizeConsoleViewColumns();

            EventsCollected.Selected(LuaScriptTreeNode);
            EventsTabControl.SelectedIndex = 1;

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
            EventHandlerThreadCancellationToken = EventHandlerThreadCts.Token;
            EventHandlerThread = new Thread(new ThreadStart(EventListenerMain))
            {
                Name = "EventListenerMain"
            };
            EventHandlerThread.Start();
        }

        private void AppendMessages(WinFormUICommandWriteToConsole msg)
        {
            ExecuteSecure(() =>
            {
                var now = DateTime.Now.ToString("T", DateTimeFormatInfo.InvariantInfo);
                var color = msg.Error ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                var line = new ListViewItem(new string[] { now, msg.Message }, 0, color, System.Drawing.Color.Transparent, ConsoleListView.Font);
                ConsoleListView.BeginUpdate();
                ConsoleListView.Items.Add(line);
                ConsoleListView.EnsureVisible(ConsoleListView.Items.Count - 1);
                ConsoleListView.EndUpdate();
            });
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
            while (PendingMessages.TryTake(out WinFormUICommandWriteToConsole? msg))
            {
                if (msg != null)
                {
                    AppendMessages(msg);
                }
            }

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
            var webWidgetEventHandler = EventHandler.Get<WebWidget.EventHandler.WebWidgetEventHandler>();
            var webEventHandler = EventHandler.Get<WebServer.EventHandler.WebServerEventHandler>();

            uiEventHandler.OnWinFormUICommandWriteToConsole += (_, e) => PendingMessages.Add(e);
            uiEventHandler.OnWinFormUICommandCreateButton += (_, e) => EventHandler_OnWinFormUICommandCreateButton(e);
            uiEventHandler.OnWinFormUICommandDeleteButton += (_, e) => EventHandler_OnWinFormUICommandDeleteButton(e);

            internalEventHandler.OnInternalDependencyAdded += (_, e) => EventHandler_OnInternaDependencyAdded_Envelope(e.InstanceId, e.DependsOn);
            internalEventHandler.OnInternalDependencyRemoved += (_, e) => EventHandler_OnInternalDependencyRemoved_Envelope(e.InstanceId, e.DependsOn);

            internalEventHandler.OnInternalInstanceAdded += (_, e) => EventHandler_OnInternalInstanceAdded_InsideView(e.LuaLibrary, e.InstanceId);
            internalEventHandler.OnInternalInstanceRemoved += (_, e) => EventHandler_OnInternalInstanceRemoved_InsideView(e.LuaLibrary, e.InstanceId);
            internalEventHandler.OnInternalDependencyAdded += (_, e) => EventHandler_OnInternaDependencyAdded_InsideView(e.LuaLibrary, e.InstanceId, e.DependsOn);
            internalEventHandler.OnInternalDependencyRemoved += (_, e) => EventHandler_OnInternalDependencyRemoved_InsideView(e.LuaLibrary, e.InstanceId, e.DependsOn);

            internalEventHandler.OnInternalInstanceAdded += (_, e) => EventHandler_OnInternalInstanceAdded_DeepView(e.InstanceId);
            internalEventHandler.OnInternalInstanceRemoved += (_, e) => EventHandler_OnInternalInstanceRemoved_DeepView(e.InstanceId);
            internalEventHandler.OnInternalDependencyAdded += (_, e) => EventHandler_OnInternaDependencyAdded_DeepView(e.InstanceId, e.DependsOn);
            internalEventHandler.OnInternalDependencyRemoved += (_, e) => EventHandler_OnInternalDependencyRemoved_DeepView(e.InstanceId, e.DependsOn);

            webWidgetEventHandler.OnWebWidgetEndpointAdded += (_, e) => EventHandler_OnEndpointAdded(e.Endpoint);
            webWidgetEventHandler.OnWebWidgetEndpointRemoved += (_, e) => EventHandler_OnEndpointRemoved(e.Endpoint);
            webEventHandler.OnWebServerEndpointAdded += (_, e) => EventHandler_OnEndpointAdded(e.Url);
            webEventHandler.OnWebServerEndpointRemoved += (_, e) => EventHandler_OnEndpointRemoved(e.Url);

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

        private void EventHandler_OnEndpointRemoved(string endpoint)
        {
            ExecuteSecure(() =>
            {
                if (EndpointMenuItems.ContainsKey(endpoint))
                {
                    EndpointsToolStripMenuItem.DropDownItems.Remove(EndpointMenuItems[endpoint]);
                    EndpointsToolStripMenuItem.Enabled = EndpointsToolStripMenuItem.DropDownItems.Count != 0;
                    EndpointMenuItems.Remove(endpoint);
                }
            });
        }

        private void EventHandler_OnEndpointAdded(string endpoint)
        {
            // Make sure that endpoints does not contain http://*, but an actual useable URL
            var concreteEndpoint = endpoint.Replace("http://*", "http://127.0.0.1").Replace("ws://*", "ws://128.0.0.1");
            ExecuteSecure(() =>
            {
                if (EndpointMenuItems.ContainsKey(endpoint))
                    return;

                EndpointsToolStripMenuItem.Enabled = true;

                var openItem = new ToolStripMenuItem("Open in browser");
                openItem.Click += (_, e) => { Process.Start(new ProcessStartInfo(concreteEndpoint) { UseShellExecute = true }); };
                var copyItem = new ToolStripMenuItem("Copy to clipboard");
                copyItem.Click += (_, e) => CopyToClipBoard(concreteEndpoint);

                var item = new ToolStripMenuItem(endpoint);
                item.DropDownItems.AddRange(new ToolStripItem[] { openItem, copyItem });

                if (!endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
                {
                    openItem.Enabled = false;
                }
                EndpointMenuItems.Add(endpoint, item);
                EndpointsToolStripMenuItem.DropDownItems.Add(item);
            });
        }

        private void EventHandler_OnInternalDependencyRemoved_Envelope(string instanceId, string dependsOn)
        {
            // something stopped begin dependant on us, so remove it from our Envelope
            if (dependsOn == InstanceId)
                Envelope = Envelope.Remove(instanceId);
        }

        private void EventHandler_OnInternalDependencyRemoved_InsideView(string luaLibraryName, string instanceId, string dependsOn)
        {
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

        private void EventHandler_OnInternalDependencyRemoved_DeepView(string instanceId, string dependsOn)
        {
            if (DeepViewInstanceNode == null)
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                foreach (TreeNode instanceNode in DeepViewInstanceNode.Nodes)
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

        private void EventHandler_OnInternaDependencyAdded_Envelope(string instanceId, string dependsOn)
        {
            // something is depending on us, so add it to the receivers of our events
            if (dependsOn == InstanceId)
                Envelope = Envelope.Add(instanceId);
        }

        private void EventHandler_OnInternaDependencyAdded_InsideView(string luaLibraryName, string instanceId, string dependsOn)
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
                        instanceNode.Nodes.Add(InsideViewNodeTag.DependencyNode(dependsOn));
                        break;
                    }
                }

                InsideView.EndUpdate();
            });
        }

        private void EventHandler_OnInternaDependencyAdded_DeepView(string instanceId, string dependsOn)
        {
            if (DeepViewInstanceNode == null)
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                foreach (TreeNode instanceNode in DeepViewInstanceNode.Nodes)
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

        private void EventHandler_OnInternalInstanceAdded_InsideView(string luaLibraryName, string instanceId)
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

        private void EventHandler_OnInternalInstanceAdded_DeepView(string instanceId)
        {
            if (DeepViewInstanceNode == null)
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                DeepViewInstanceNode.Nodes.Add(InsideViewNodeTag.InstanceNode(instanceId));

                InsideView.EndUpdate();
            });
        }

        private void EventHandler_OnInternalInstanceRemoved_InsideView(string luaLibraryName, string instanceId)
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

        private void EventHandler_OnInternalInstanceRemoved_DeepView(string instanceId)
        {
            if (DeepViewInstanceNode == null)
                return;

            ExecuteSecure(() =>
            {
                InsideView.BeginUpdate();

                foreach (TreeNode instanceNode in DeepViewInstanceNode.Nodes)
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

            if (!(rows.Cells[1].ToolTipText is string eventType))
                return;

            var code = $@"
addEventHandler(""{eventType}"", function(event)
    -- Example event: {json}
    -- your code here
end)
";
            CopyToClipBoard(code);
        }

        private void EventsTabControl_Resize(object sender, EventArgs e)
        {
            ResizeConsoleViewColumns();
        }

        private void ResizeConsoleViewColumns()
        {
            // the 20px, is the width needed to make sure that the ConsoleViewTextColumn doesn't create a
            // horizontal scrollback
            ConsoleViewTextColumnHeader.Width = EventsTabControl.Width - ConsoleViewTimestampColumnHeader.Width - 20;
        }

        private void ClearEventsMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear events?", "Clear events", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                EventsCollected.Clear();
            }
        }

        private void SaveEventsMenuItem_Click(object sender, EventArgs e)
        {
            // https://stackoverflow.com/questions/17762037/current-thread-must-be-set-to-single-thread-apartment-sta-error-in-copy-stri
            Thread thread = new Thread(() =>
            {
                var result = SaveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    EventsCollected.SaveToFile(SaveFileDialog.FileName);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}