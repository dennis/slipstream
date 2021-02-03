namespace Slipstream.Components.IRacing.Plugins.Models
{
    internal class DriverState
    {
        public int PlayerCarDriverIncidentCount { get; set; }

        public void ClearState()
        {
            PlayerCarDriverIncidentCount = 0;
        }
    }
}