namespace Slipstream.Components.IRacing.Models
{
    internal class DriverState
    {
        public int DriverIncidentCount { get; set; }
        public int TeamIncidentCount { get; set; }
        public int MyIncidentCount { get; set; }

        public void ClearState()
        {
            DriverIncidentCount = 0;
            TeamIncidentCount = 0;
            MyIncidentCount = 0;
        }
    }
}