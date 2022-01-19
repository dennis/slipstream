#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal class DependencyEventFilter : IEventFilter
    {
        private readonly string SelectedNodeInstanceId;
        private readonly string SelectedNodeDependency;

        public DependencyEventFilter(string instanceId, string dependency)
        {
            SelectedNodeInstanceId = instanceId;
            SelectedNodeDependency = dependency;
        }

        public bool Accept(IEvent e)
        {
            return e.Envelope.ContainsRecipient(SelectedNodeInstanceId) && e.Envelope.Sender == SelectedNodeDependency;
        }
    }
}