#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal class InstanceEventFilter : IEventFilter
    {
        private readonly string SelectedNodeInstanceId;

        public InstanceEventFilter(string instanceId)
        {
            SelectedNodeInstanceId = instanceId;
        }

        public bool Accept(IEvent e)
        {
            return e.Envelope.ContainsRecipient(SelectedNodeInstanceId);
        }
    }
}