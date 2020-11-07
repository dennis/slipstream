#nullable enable

using Slipstream.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Slipstream.Frontend
{
    public partial class MainWindow : Form, IEventListener
    {
        private Thread? EventHandlerThread;
        private readonly IEventBus EventBus;
        private IEventBusSubscription? EventBusSubscription;
        private readonly string ScriptsPath;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<Guid, ToolStripMenuItem> MenuPluginItems = new Dictionary<Guid, ToolStripMenuItem>();

        public MainWindow(IEventBus eventBus)
        {
            EventBus = eventBus;

            InitializeComponent();

            ScriptsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\Slipstream\Scripts\";
            System.IO.Directory.CreateDirectory(ScriptsPath);

            Load += MainWindow_Load;
            FormClosing += MainWindow_FormClosing;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventBus.UnregisterListener(this);
            EventHandlerThread?.Abort(); // abit harsh?
            EventHandlerThread?.Join();
            EventHandlerThread = null;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            EventBusSubscription = EventBus.RegisterListener(this);
            EventHandlerThread = new Thread(new ThreadStart(this.EventListenerMain));
            EventHandlerThread.Start();

            // Plugins..
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { PluginName = "DebugOutputPlugin", Enabled = true });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { PluginName = "FileMonitorPlugin", Enabled = true, Settings = new Shared.Events.Internal.FileMonitorSettings { Paths = new string[] { ScriptsPath } } });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { PluginName = "FileTriggerPlugin", Enabled = true });

            // Tell backend that we're ready
            EventBus.PublishEvent(new Shared.Events.Internal.FrontendReady());
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

            while (PendingMessages.TryTake(out string msg))
            {
                allMesssages += msg + "\r\n";
            }

            AppendMessages(allMesssages);
        }

        #region EventHandlerThread methods
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();
        private void EventListenerMain()
        {
            Debug.Assert(EventBusSubscription != null);

            EventHandler.OnInternalPluginRegister += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.Id} name: {e.Event.PluginName} enabled: {e.Event.Enabled}");
            EventHandler.OnInternalPluginEnable += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.Id}");
            EventHandler.OnInternalPluginDisable += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.Id}");
            EventHandler.OnInternalPluginStateChanged += (s, e) => EventHandler_OnInternalPluginStateChanged(e.Event);
            EventHandler.OnInternalFileMonitorFileCreated += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.FilePath}");
            EventHandler.OnInternalFileMonitorFileChanged += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.FilePath}");
            EventHandler.OnInternalFileMonitorFileDeleted += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.FilePath}");
            EventHandler.OnInternalFileMonitorFileRenamed += (s, e) => PendingMessages.Add($"{e.Event.GetType().Name}: {e.Event.FilePath}");

            while (true)
            {
                EventHandler.HandleEvent(EventBusSubscription?.NextEvent());
            }
        }

        private void EventHandler_OnInternalPluginStateChanged(Shared.Events.Internal.PluginStateChanged e)
        {
            PendingMessages.Add($"{e.GetType().Name}: {e.PluginName} -> {e.PluginStatus}");

            switch (e.PluginStatus)
            {
                case Shared.Events.Internal.PluginStatus.Registered:
                    {
                        var item = new ToolStripMenuItem
                        {
                            Checked = true,
                            CheckState = CheckState.Unchecked,
                            Name = e.Id.ToString(),
                            Size = new System.Drawing.Size(180, 22),
                            Text = e.DisplayName
                        };
                        item.Click += PluginMenuItem_Click;
                        MenuPluginItems.Add(e.Id, item);

                        ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { item }));
                    }
                    break;
                case Shared.Events.Internal.PluginStatus.Unregistered:
                    {
                        var item = MenuPluginItems[e.Id];
                        MenuPluginItems.Remove(e.Id);

                        ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.Remove(item));
                    }
                    break;
                case Shared.Events.Internal.PluginStatus.Enabled:
                    {
                        var item = MenuPluginItems[e.Id];

                        ExecuteSecure(() => item.CheckState = CheckState.Checked);
                    }
                    break;
                case Shared.Events.Internal.PluginStatus.Disabled:
                    {
                        var item = MenuPluginItems[e.Id];

                        ExecuteSecure(() => item.CheckState = CheckState.Unchecked);
                    }
                    break;
            }
        }

        private void PluginMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item))
                return;

            if (item.CheckState == CheckState.Checked)
            {
                EventBus.PublishEvent(new Shared.Events.Internal.PluginDisable() { Id = Guid.Parse(item.Name) });
            }
            else
            {
                EventBus.PublishEvent(new Shared.Events.Internal.PluginEnable() { Id = Guid.Parse(item.Name) });
            }
        }
        #endregion

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenScriptsDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ScriptsPath);
        }
    }
}
