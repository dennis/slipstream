#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
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
        private readonly BlockingCollection<string> PendingMessages = new BlockingCollection<string>();
        private readonly IDictionary<string, ToolStripMenuItem> MenuPluginItems = new Dictionary<string, ToolStripMenuItem>();

        public MainWindow(IEventBus eventBus)
        {
            EventBus = eventBus;

            InitializeComponent();

            Load += MainWindow_Load;
            FormClosing += MainWindow_FormClosing;

            LogAreaTextBox.Text += "Hello world\r\n";
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

            // Tell backend that we're ready
            EventBus.PublishEvent(new FrontendReady());
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
                    case PluginStateChanged ev:
                        OnPluginStateChanged(ev);
                        break;
                }

                PendingMessages.Add($"Got Event: {e}");
            }
        }

        private void OnPluginStateChanged(PluginStateChanged e)
        {
            switch (e.PluginStatus)
            {
                case PluginStatus.Registered:
                    {
                        var item = new ToolStripMenuItem
                        {
                            Checked = true,
                            CheckState = System.Windows.Forms.CheckState.Unchecked,
                            Name = e.PluginName,
                            Size = new System.Drawing.Size(180, 22),
                            Text = e.PluginName
                        };
                        item.Click += PluginMenuItem_Click;
                        MenuPluginItems.Add(e.PluginName, item);

                        ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { item }));
                    }
                    break;
                case PluginStatus.Unregistered:
                    {
                        var item = MenuPluginItems[e.PluginName];
                        MenuPluginItems.Remove(e.PluginName);

                        ExecuteSecure(() => PluginsToolStripMenuItem.DropDownItems.Remove(item));
                    }
                    break;
                case PluginStatus.Enabled:
                    {
                        var item = MenuPluginItems[e.PluginName];

                        ExecuteSecure(() => item.CheckState = CheckState.Checked);
                    }
                    break;
                case PluginStatus.Disabled:
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
                EventBus.PublishEvent(new PluginDisable() { PluginName = item.Name });
            }
            else
            {
                EventBus.PublishEvent(new PluginEnable() { PluginName = item.Name });
            }
        }
        #endregion

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
