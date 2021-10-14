#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Internal.Events;
using Slipstream.Shared;

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal class EventRepository
    {
        private readonly List<IEvent> Events = new List<IEvent>();
        private readonly DataGridView EventGridView;
        private readonly IEventSerdeService EventSerdeService;
        private readonly Label EventFilterDescriptionLabel;
        private readonly ContextMenuStrip ContextMenu;
        private readonly TabPage EventsTabPage;
        private IEventFilter SelectedFilter = new NoneEventFilter();

        private const int MaxEventsStored = 1000;

        public EventRepository(DataGridView eventGridView, Label eventFilterDescriptionLabel, ContextMenuStrip eventViewerContextMenuStrip, TabPage eventsTabPage, IEventSerdeService eventSerdeService)
        {
            EventGridView = eventGridView;
            EventSerdeService = eventSerdeService;
            EventFilterDescriptionLabel = eventFilterDescriptionLabel;
            ContextMenu = eventViewerContextMenuStrip;
            EventsTabPage = eventsTabPage;
        }

        public void Add(IEvent e)
        {
            while (Events.Count > MaxEventsStored)
            {
                Events.RemoveAt(0);
            }
            Events.Add(e);
            AddToControl(e);
        }

        private void AddToControl(IEvent e)
        {
            if (SelectedFilter.Accept(e))
            {
                string recipients = RecipientsAsString(e);
                var json = EventSerdeService.Serialize(e);

                // Default to showing the last item (newest event) if nothing is added, or if we're already viewing last item
                bool viewingLastRow = (EventGridView.FirstDisplayedScrollingRowIndex + EventGridView.DisplayedRowCount(true) == EventGridView.RowCount) || EventGridView.RowCount == 0;

                // Show Custom events in green
                if (e is InternalCustomEvent a)
                {
                    EventGridView.Rows.Add(e.Envelope.Uptime, a.Name, e.Envelope.Sender, recipients, json);
                    EventGridView.Rows[^1].DefaultCellStyle.ForeColor = Color.Green;
                    EventGridView.Rows[^1].Cells[1].ToolTipText = "InternalCustomEvent";
                }
                else if (e.EventType.StartsWith("Internal"))
                {
                    EventGridView.Rows.Add(e.Envelope.Uptime, e.EventType, e.Envelope.Sender, recipients, json);
                    EventGridView.Rows[^1].DefaultCellStyle.ForeColor = Color.Gray;
                }
                else
                {
                    EventGridView.Rows.Add(e.Envelope.Uptime, e.EventType, e.Envelope.Sender, recipients, json);
                }

                EventGridView.Rows[^1].ContextMenuStrip = ContextMenu;
                if (viewingLastRow)
                {
                    // Make sure we're still viewing last row
                    EventGridView.FirstDisplayedScrollingRowIndex = EventGridView.RowCount - 1;
                }
            }
        }

        private static string RecipientsAsString(IEvent e)
        {
            if (e.Envelope.Recipients == null || e.Envelope.Recipients.Length == 0)
            {
                return "*";
            }
            else
            {
                return string.Join(", ", e.Envelope.Recipients);
            }
        }

        internal void Selected(TreeNode node)
        {
            if (!(node.Tag is InsideViewNodeTag tag))
                return;

            switch (tag.NodeType)
            {
                case NodeTypeEnum.Dependency:
                    SelectedFilter = new DependencyEventFilter(node.Parent.Name, node.Name);
                    EventFilterDescriptionLabel.Text = $"Show events filtered by recipient '{node.Parent.Name}' and sender '{node.Name}'";
                    EventsTabPage.Text = $"Events between '{node.Parent.Name}' and '{node.Name}'";
                    break;

                case NodeTypeEnum.Instance:
                    SelectedFilter = new InstanceEventFilter(node.Name);
                    EventFilterDescriptionLabel.Text = $"Show events filtered by recipient '{node.Name}'";
                    EventsTabPage.Text = $"Events for '{node.Name}'";
                    break;

                case NodeTypeEnum.LuaScripts:
                    SelectedFilter = new AllEventFilter();
                    EventFilterDescriptionLabel.Text = "Showing all events";
                    EventsTabPage.Text = $"Events";
                    break;

                default:
                    SelectedFilter = new NoneEventFilter();
                    EventFilterDescriptionLabel.Text = "";
                    EventsTabPage.Text = $"Events";
                    break;
            }

            EventGridView.Rows.Clear();
            foreach (var e in Events)
            {
                AddToControl(e);
            }
        }
    }
}