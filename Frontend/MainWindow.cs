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
        private readonly IDictionary<string, ToolStripMenuItem> MenuPluginItems = new Dictionary<string, ToolStripMenuItem>();

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
            EventBus.PublishEvent(new Shared.Events.Internal.PluginLoad () { PluginName = "DebugOutputPlugin", Enabled = true });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginLoad() { PluginName = "FileMonitorPlugin", Enabled = true, Settings = new Shared.Events.Internal.FileMonitorSettings { Paths = new string[] { ScriptsPath } } });

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
        private void EventListenerMain()
        {
            Debug.Assert(EventBusSubscription != null);

            while (true)
            {
                IEvent? e = EventBusSubscription?.NextEvent();

                switch (e)
                {
                    case Shared.Events.Internal.PluginLoad ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.PluginName} enabled: {ev.Enabled}");
                        break;
                    case Shared.Events.Internal.PluginEnable ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.PluginName}");
                        break;
                    case Shared.Events.Internal.PluginDisable ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.PluginName}");
                        break;
                    case Shared.Events.Internal.PluginStateChanged ev:
                        OnPluginStateChanged(ev);
                        PendingMessages.Add($"{e.GetType().Name}: {ev.PluginName} -> {ev.PluginStatus}");
                        break;
                    case Shared.Events.Internal.FileMonitorFileCreated ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.FilePath}");
                        break;
                    case Shared.Events.Internal.FileMonitorFileChanged ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.FilePath}");
                        break;
                    case Shared.Events.Internal.FileMonitorFileDeleted ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.FilePath}");
                        break;
                    case Shared.Events.Internal.FileMonitorFileRenamed ev:
                        PendingMessages.Add($"{e.GetType().Name}: {ev.OldFilePath} -> {ev.FilePath}");
                        break;
                    case null:
                        // Ignore
                        break;
                    default:
                        PendingMessages.Add($"{e.GetType().Name}");
                        break;
                }
            }
        }

        private void OnPluginStateChanged(Shared.Events.Internal.PluginStateChanged e)
        {
            switch (e.PluginStatus)
            {
                case Shared.Events.Internal.PluginStatus.Registered:
                    {
                        var item = new ToolStripMenuItem
                        {
                            Checked = true,
                            CheckState = CheckState.Unchecked,
                            Name = e.PluginName,
                            Size = new System.Drawing.Size(180, 22),
                            Text = e.PluginName
                        };
                        item.Click += PluginMenuItem_Click;
                        MenuPluginItems.Add(e.PluginName, item);

                        ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { item }));
                    }
                    break;
                case Shared.Events.Internal.PluginStatus.Unregistered:
                    {
                        var item = MenuPluginItems[e.PluginName];
                        MenuPluginItems.Remove(e.PluginName);

                        ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.Remove(item));
                    }
                    break;
                case Shared.Events.Internal.PluginStatus.Enabled:
                    {
                        var item = MenuPluginItems[e.PluginName];

                        ExecuteSecure(() => item.CheckState = CheckState.Checked);
                    }
                    break;
                case Shared.Events.Internal.PluginStatus.Disabled:
                    {
                        var item = MenuPluginItems[e.PluginName];

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
                EventBus.PublishEvent(new Shared.Events.Internal.PluginDisable() { PluginName = item.Name });
            }
            else
            {
                EventBus.PublishEvent(new Shared.Events.Internal.PluginEnable() { PluginName = item.Name });
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
