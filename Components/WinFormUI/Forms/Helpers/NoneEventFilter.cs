#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal class NoneEventFilter : IEventFilter
    {
        public bool Accept(IEvent e)
        {
            return false;
        }
    }
}