#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal class AllEventFilter : IEventFilter
    {
        public bool Accept(IEvent e)
        {
            return true;
        }
    }
}