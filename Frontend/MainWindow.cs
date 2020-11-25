﻿#nullable enable

using Slipstream.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Slipstream.Frontend
{
    public partial class MainWindow : Form
    {
        private Thread? EventHandlerThread;
        private readonly IEventBus EventBus;
        private IEventBusSubscription? EventBusSubscription;
        private readonly string ScriptsPath;
        private readonly string AudioPath;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, ToolStripMenuItem> MenuPluginItems = new Dictionary<string, ToolStripMenuItem>();

        public MainWindow(IEventBus eventBus)
        {
            EventBus = eventBus;

            InitializeComponent();

            ScriptsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\Slipstream\Scripts\";
            System.IO.Directory.CreateDirectory(ScriptsPath);

            AudioPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\Slipstream\Audio\";
            System.IO.Directory.CreateDirectory(AudioPath);

            Load += MainWindow_Load;
            FormClosing += MainWindow_FormClosing;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventBusSubscription?.Dispose();
            EventBusSubscription = null;
            EventHandlerThread?.Abort(); // abit harsh?
            EventHandlerThread?.Join();
            EventHandlerThread = null;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            EventBusSubscription = EventBus.RegisterListener();
            EventHandlerThread = new Thread(new ThreadStart(this.EventListenerMain));
            EventHandlerThread.Start();

            // Plugins..
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { Id = "DebugOutputPlugin", PluginName = "DebugOutputPlugin", Enabled = true });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { Id = "FileMonitorPlugin", PluginName = "FileMonitorPlugin", Enabled = true, Settings = new Shared.Events.Setting.FileMonitorSettings { Paths = new string[] { ScriptsPath } } });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { Id = "FileTriggerPlugin", PluginName = "FileTriggerPlugin", Enabled = true });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { Id = "AudioPlugin", PluginName = "AudioPlugin", Enabled = true, Settings = new Shared.Events.Setting.AudioSettings { Path = AudioPath } });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { Id = "IRacingPlugin", PluginName = "IRacingPlugin", Enabled = true });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginRegister() { Id = "TwitchPlugin", PluginName = "TwitchPlugin", Enabled = true, Settings = GetTwitchSettings() });

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

            EventHandler.OnInternalPluginStateChanged += (s, e) => EventHandler_OnInternalPluginStateChanged(e.Event);
            EventHandler.OnUtilityWriteToConsole += (s, e) => PendingMessages.Add($"{DateTime.Now:s} {e.Event.Message}");

            while (true)
            {
                EventHandler.HandleEvent(EventBusSubscription?.NextEvent());
            }
        }

        private void EventHandler_OnInternalPluginStateChanged(Shared.Events.Internal.PluginStateChanged e)
        {
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
                        if (MenuPluginItems.ContainsKey(e.Id))
                        {
                            var item = MenuPluginItems[e.Id];
                            MenuPluginItems.Remove(e.Id);

                            ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.Remove(item));
                        }
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
                EventBus.PublishEvent(new Shared.Events.Internal.PluginDisable() { Id = item.Name });
            }
            else
            {
                EventBus.PublishEvent(new Shared.Events.Internal.PluginEnable() { Id = item.Name });
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

        private void OpenAudioDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(AudioPath);
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = new SettingsForm().ShowDialog(this);

            // Just spam settings to everyone that wants it
            EventBus.PublishEvent(GetTwitchSettings());
        }

        private Shared.Events.Setting.TwitchSettings GetTwitchSettings()
        {
            var settings = Properties.Settings.Default;

            return new Shared.Events.Setting.TwitchSettings { TwitchUsername = settings.TwitchUsername, TwitchToken = settings.TwitchToken };
        }
    }
}
