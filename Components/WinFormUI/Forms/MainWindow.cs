#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.WinFormUI.Events;
using Slipstream.Components.WinFormUI.Lua;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

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

        public MainWindow(
            string instanceId,
            WinFormUIInstanceThread instance,
            IInternalEventFactory internalEventFactory,
            IWinFormUIEventFactory uiEventFactory,
            IPlaybackEventFactory playbackEventFactory,
            IEventBus eventBus,
            IApplicationVersionService applicationVersionService,
            IEventHandlerController eventHandlerController
            )
        {
            InstanceId = instanceId;
            Instance = instance;
            InternalEventFactory = internalEventFactory;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;
            EventHandler = eventHandlerController;
            EventBus = eventBus;
            Envelope = new EventEnvelope(instanceId);
            BroadcastEnvelope = new EventEnvelope(instanceId);

            InitializeComponent();

            AboutTextBox.Text = "Slipstream version v" + applicationVersionService.Version;

            // Find Root Nodes of InsideView:
            foreach (TreeNode node in InsideView.Nodes)
            {
                if (node.Name == "LuaScripts")
                {
                    LuaScriptTreeNode = node;
                }
            }

            Debug.Assert(LuaScriptTreeNode != null);

            InsideView.BeginUpdate();
            LuaScriptTreeNode.Expand();
            InsideView.EndUpdate();

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
            EventBusSubscription = EventBus.RegisterListener(InstanceId, fromBeginning: true);
            EventHandlerThreadCancellationToken = EventHandlerThreadCts.Token;
            EventHandlerThread = new Thread(new ThreadStart(this.EventListenerMain))
            {
                Name = "EventListenerMain"
            };
            EventHandlerThread.Start();
        }

        private void AppendMessages(string msg)
        {
            ExecuteSecure(() => this.LogAreaTextBox.AppendText(msg));
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
                EventHandler.HandleEvent(EventBusSubscription?.NextEvent(250));
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
                        instanceNode.Nodes.Add(new TreeNode(dependsOn + " [dependency]") { Name = dependsOn });
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

                LuaScriptTreeNode.Nodes.Add(new TreeNode(instanceId) { Name = instanceId });

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
    }
}