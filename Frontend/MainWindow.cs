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
    public partial class MainWindow : Form
    {
        private Thread? EventHandlerThread;
        private readonly IEventBus EventBus;
        private IEventBusSubscription? EventBusSubscription;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, ToolStripMenuItem> MenuPluginItems = new Dictionary<string, ToolStripMenuItem>();
        private readonly ApplicationConfiguration ApplicationConfiguration;
        private readonly string CleanTitle;

        public MainWindow(IEventBus eventBus, IApplicationVersionService applicationVersionService, ApplicationConfiguration applicationConfiguration)
        {
            EventBus = eventBus;
            ApplicationConfiguration = applicationConfiguration;

            InitializeComponent();

            Text += " v" + applicationVersionService.Version;
            CleanTitle = Text;

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
            EventBusSubscription = EventBus.RegisterListener(fromBeginning: true);
            EventHandlerThread = new Thread(new ThreadStart(this.EventListenerMain));
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

            EventHandler.OnInternalPluginState += (s, e) => EventHandler_OnInternalPluginState(e.Event);
            EventHandler.OnUICommandWriteToConsole += (s, e) =>
            {
                PendingMessages.Add($"{DateTime.Now:s} {e.Event.Message}");
            };

            // Request full state of all known plugins, so we get any that might be started before "us"
            EventBus.PublishEvent(new Shared.Events.Internal.InternalCommandPluginStates());

            while (true)
            {
                EventHandler.HandleEvent(EventBusSubscription?.NextEvent());
            }
        }

        private void EventHandler_OnInternalPluginState(Shared.Events.Internal.InternalPluginState e)
        {
            if (e.PluginStatus != "Unregistered" && !MenuPluginItems.ContainsKey(e.Id))
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

            switch (e.PluginStatus)
            {
                case "Registered":
                    break;
                case "Unregistered":
                    {
                        if (MenuPluginItems.ContainsKey(e.Id))
                        {
                            var item = MenuPluginItems[e.Id];
                            MenuPluginItems.Remove(e.Id);

                            ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.Remove(item));
                        }
                    }
                    break;
                case "Enabled":
                    {
                        switch (e.Id)
                        {
                            case "FileMonitorPlugin":
                                EventBus.PublishEvent(ApplicationConfiguration.GetFileMonitorSettingsEvent());
                                break;

                            case "AudioPlugin":
                                EventBus.PublishEvent(ApplicationConfiguration.GetAudioSettingsEvent());
                                break;

                            case "TwitchPlugin":
                                var settings = Properties.Settings.Default;
                                EventBus.PublishEvent(ApplicationConfiguration.GetTwitchSettingsEvent());
                                break;

                            case "TransmitterPlugin":
                                ExecuteSecure(() => Text += $" <<< transmitting to {ApplicationConfiguration.GetTxrxSettingsEvent().TxrxIpPort} >>>");
                                break;

                            case "ReceiverPlugin":
                                ExecuteSecure(() => Text += $" <<< receiving from {ApplicationConfiguration.GetTxrxSettingsEvent().TxrxIpPort} >>>");
                                break;
                        }

                        var item = MenuPluginItems[e.Id];

                        ExecuteSecure(() => item.CheckState = CheckState.Checked);
                        ExecuteSecure(() => item.Text = e.DisplayName);
                    }
                    break;
                case "Disabled":
                    {
                        var item = MenuPluginItems[e.Id];

                        ExecuteSecure(() => item.CheckState = CheckState.Unchecked);
                        ExecuteSecure(() => item.Text = e.DisplayName);
                    }
                    switch (e.Id)
                    {
                        case "TransmitterPlugin":
                            ExecuteSecure(() => Text = CleanTitle);
                            break;

                        case "ReceiverPlugin":
                            ExecuteSecure(() => Text = CleanTitle);
                            break;
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
                EventBus.PublishEvent(new Shared.Events.Internal.InternalCommandPluginDisable() { Id = item.Name });
            }
            else
            {
                EventBus.PublishEvent(new Shared.Events.Internal.InternalCommandPluginEnable() { Id = item.Name });
            }
        }
        #endregion

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenScriptsDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ApplicationConfiguration.GetScriptsPath());
        }

        private void OpenAudioDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ApplicationConfiguration.GetAudioPath());
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = new SettingsForm().ShowDialog(this);

            // Just spam settings to everyone that wants it
            EventBus.PublishEvent(ApplicationConfiguration.GetTwitchSettingsEvent());
            EventBus.PublishEvent(ApplicationConfiguration.GetTxrxSettingsEvent());
        }
    }
}
