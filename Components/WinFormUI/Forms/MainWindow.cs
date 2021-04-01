#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Internal.Events;
using Slipstream.Components.Playback;
using Slipstream.Components.UI;
using Slipstream.Components.UI.Events;
using Slipstream.Properties;
using Slipstream.Shared;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using EventHandler = Slipstream.Shared.EventHandlerController;

namespace Slipstream.Components.WinFormUI.Forms
{
    public partial class MainWindow : Form
    {
        private Thread? EventHandlerThread;
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IUIEventFactory UIEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private IEventBusSubscription? EventBusSubscription;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, ToolStripMenuItem> MenuPluginItems = new Dictionary<string, ToolStripMenuItem>();
        private readonly IDictionary<string, Button> LuaButtons = new Dictionary<string, Button>();
        private readonly string CleanTitle;
        private readonly IEventHandlerController EventHandler;
        private bool ShuttingDown = false;

        public MainWindow(IInternalEventFactory internalEventFactory, IUIEventFactory uiEventFactory, IPlaybackEventFactory playbackEventFactory, IEventBus eventBus, IApplicationVersionService applicationVersionService, IEventHandlerController eventHandlerController)
        {
            InternalEventFactory = internalEventFactory;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;

            EventHandler = eventHandlerController;

            EventBus = eventBus;

            InitializeComponent();

            Text += " v" + applicationVersionService.Version;
            CleanTitle = Text;

            Load += MainWindow_Load;
            FormClosing += MainWindow_FormClosing;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ShuttingDown)
            {
                // Just request we want to shut down. We'll receive an InternalShutdown if we
                // actually will shut down

                EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandShutdown());
                e.Cancel = true;

                return;
            }

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

        private void EventListenerMain()
        {
            Debug.Assert(EventBusSubscription != null);

            var internalEventHandler = EventHandler.Get<Components.Internal.EventHandler.Internal>();
            var uiEventHandler = EventHandler.Get<Components.UI.EventHandler.UIEventHandler>();

            internalEventHandler.OnInternalPluginState += (_, e) => EventHandler_OnInternalPluginState(e);
            internalEventHandler.OnInternalShutdown += (_, e) => EventHandler_OnInteralShutdown(e);
            uiEventHandler.OnUICommandWriteToConsole += (_, e) => PendingMessages.Add($"{DateTime.Now:s} {e.Message}");
            uiEventHandler.OnUICommandCreateButton += (_, e) => EventHandler_OnUICommandCreateButton(e);
            uiEventHandler.OnUICommandDeleteButton += (_, e) => EventHandler_OnUICommandDeleteButton(e);

            // Request full state of all known plugins, so we get any that might be started before "us"
            EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandPluginStates());

            while (true)
            {
                EventHandler.HandleEvent(EventBusSubscription?.NextEvent());
            }
        }

        private void EventHandler_OnInteralShutdown(InternalShutdown _)
        {
            ShuttingDown = true;
            ExecuteSecure(() => Application.Exit());
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

        private void EventHandler_OnInternalPluginState(InternalPluginState e)
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
                            ExecuteSecure(() => Text = $"{CleanTitle} <<< transmitting >>>");
                            break;

                        case "ReceiverPlugin":
                            ExecuteSecure(() => Text = $"{CleanTitle} <<< receiving >>>");
                            break;

                        case "PlaybackPlugin":
                            ExecuteSecure(() =>
                            {
                                LoadEventsToolStripMenuItem.Visible = true;
                                SaveEventsToFileToolStripMenuItem.Visible = true;
                            });
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
                            case "ReceiverPlugin":
                                ExecuteSecure(() => Text = CleanTitle);
                                break;
                        }
                    }
                    break;
            }
        }

        #endregion EventHandlerThread methods

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandShutdown());
        }

        private void SaveEventsToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                EventBus.PublishEvent(PlaybackEventFactory.CreatePlaybackCommandSaveEvents(SaveFileDialog.FileName));
            }
        }

        private void LoadEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                EventBus.PublishEvent(PlaybackEventFactory.CreatePlaybackCommandInjectEvents(OpenFileDialog.FileName));
            }
        }

        private void OpenDataDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(".");
        }
    }
}