#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.UI;
using Slipstream.Components.UI.Events;
using Slipstream.Components.WinFormUI.Lua;
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
        private readonly WinFormUIInstanceThread Instance;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IUIEventFactory UIEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private IEventBusSubscription? EventBusSubscription;
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, Button> LuaButtons = new Dictionary<string, Button>();
        private readonly IEventHandlerController EventHandler;
        private bool ShuttingDown = false;

        public MainWindow(WinFormUIInstanceThread instance, IInternalEventFactory internalEventFactory, IUIEventFactory uiEventFactory, IPlaybackEventFactory playbackEventFactory, IEventBus eventBus, IApplicationVersionService applicationVersionService, IEventHandlerController eventHandlerController)
        {
            Instance = instance;
            InternalEventFactory = internalEventFactory;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;

            EventHandler = eventHandlerController;

            EventBus = eventBus;

            InitializeComponent();

            Text += " v" + applicationVersionService.Version;

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

            var internalEventHandler = EventHandler.Get<Components.Internal.EventHandler.Internal>();
            var uiEventHandler = EventHandler.Get<Components.UI.EventHandler.UIEventHandler>();

            uiEventHandler.OnUICommandWriteToConsole += (_, e) => PendingMessages.Add($"{DateTime.Now:s} {e.Message}");
            uiEventHandler.OnUICommandCreateButton += (_, e) => EventHandler_OnUICommandCreateButton(e);
            uiEventHandler.OnUICommandDeleteButton += (_, e) => EventHandler_OnUICommandDeleteButton(e);

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

        private void TestEventsMenuItem_Click(object sender, EventArgs e)
        {
            EventTestWindow etw = new EventTestWindow(EventBus);
            etw.Show(this);
        }
    }
}