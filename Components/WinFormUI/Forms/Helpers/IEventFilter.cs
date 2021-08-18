#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Forms.Helpers
{
    internal interface IEventFilter
    {
        bool Accept(IEvent e);
    }
}