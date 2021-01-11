#nullable enable

using Slipstream.Properties;
using Slipstream.Shared;
using Slipstream.Shared.Events.UI;
using Slipstream.Shared.Factories;
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
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IUIEventFactory UIEventFactory;
        private IEventBusSubscription? EventBusSubscription;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, ToolStripMenuItem> MenuPluginItems = new Dictionary<string, ToolStripMenuItem>();
        private readonly IDictionary<string, Button> LuaButtons = new Dictionary<string, Button>();
        private readonly ApplicationConfiguration ApplicationConfiguration;
        private readonly string CleanTitle;

        public MainWindow(IEventFactory eventFactory, IEventBus eventBus, IApplicationVersionService applicationVersionService, ApplicationConfiguration applicationConfiguration)
        {
            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            UIEventFactory = eventFactory.Get<IUIEventFactory>();

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
            Settings.Default.WindowLocation = Location;

            // Copy window size to app settings
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = Size;
            }
            else
            {
                Settings.Default.WindowSize = RestoreBounds.Size;
            }
            Settings.Default.Save();

            EventBusSubscription?.Dispose();
            EventBusSubscription = null;
            EventHandlerThread?.Abort(); // abit harsh?
            EventHandlerThread?.Join();
            EventHandlerThread = null;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Location = Settings.Default.WindowLocation;
            Size = Settings.Default.WindowSize;

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
            EventHandler.OnUICommandWriteToConsole += (s, e) => PendingMessages.Add($"{DateTime.Now:s} {e.Event.Message}");
            EventHandler.OnUICommandCreateButton += (s, e) => EventHandler_OnUICommandCreateButton(e.Event);
            EventHandler.OnUICommandDeleteButton += (s, e) => EventHandler_OnUICommandDeleteButton(e.Event);

            // Request full state of all known plugins, so we get any that might be started before "us"
            EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandPluginStates());

            while (true)
            {
                EventHandler.HandleEvent(EventBusSubscription?.NextEvent());
            }
        }

        private void EventHandler_OnUICommandCreateButton(UICommandCreateButton @event)
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
                        EventBus.PublishEvent(UIEventFactory.CreateUIButtonTriggered(b.Text));
                };

                LuaButtons.Add(@event.Text, b);

                ButtonFlowLayoutPanel.Controls.Add(b);
            });
        }

        private void EventHandler_OnUICommandDeleteButton(UICommandDeleteButton @event)
        {
            ExecuteSecure(() =>
            {
                if (!LuaButtons.ContainsKey(@event.Text))
                    return;

                ButtonFlowLayoutPanel.Controls.Remove(LuaButtons[@event.Text]);
                LuaButtons.Remove(@event.Text);
            });
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
                MenuPluginItems.Add(e.Id, item);

                ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { item }));
            }

            switch (e.PluginStatus)
            {
                case "Registered":
                    switch (e.Id)
                    {
                        case "TransmitterPlugin":
                            ExecuteSecure(() => Text = $"{CleanTitle} <<< transmitting to {ApplicationConfiguration.TxrxIpPort} >>>");
                            break;

                        case "ReceiverPlugin":
                            ExecuteSecure(() => Text = $"{CleanTitle} <<< receiving from {ApplicationConfiguration.TxrxIpPort} >>>");
                            break;
                    }
                    break;
                case "Unregistered":
                    {
                        if (MenuPluginItems.ContainsKey(e.Id))
                        {
                            var item = MenuPluginItems[e.Id];
                            MenuPluginItems.Remove(e.Id);

                            ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.Remove(item));
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
                    }
                    break;
            }
        }
        #endregion

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenScriptsDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ApplicationConfiguration.ScriptPath);
        }

        private void OpenAudioDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ApplicationConfiguration.AudioPath);
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(new SettingsForm().ShowDialog(this) == DialogResult.OK)
            {
                EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandReconfigure());
            }
        }
    }
}
