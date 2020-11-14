#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCarInfo : IEvent
    {
        public string EventType => "IRacingCarInfo";
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public string CarNumber { get; set; } = string.Empty;
        public long CurrentDriverUserID { get; set; }
        public string CurrentDriverName { get; set; } = string.Empty;
        public long TeamID { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string CarName { get; set; } = string.Empty;
        public string CarNameShort { get; set; } = string.Empty;
        public long CurrentDriverIRating { get; set; }
        public bool LocalUser { get; set; }
        public long Spectator { get; set; }

        public bool SameAs(IRacingCarInfo other)
        {
            return
                CarIdx.Equals(other.CarIdx) &&
                CarNumber.Equals(other.CarNumber) &&
                CurrentDriverUserID.Equals(other.CurrentDriverUserID) &&
                CurrentDriverName.Equals(other.CurrentDriverName) &&
                CurrentDriverIRating.Equals(other.CurrentDriverIRating) &&
                TeamID.Equals(other.TeamID) &&
                TeamName.Equals(other.TeamName) &&
                CarName.Equals(other.CarName) &&
                CarNameShort.Equals(other.CarNameShort) &&
                LocalUser.Equals(other.LocalUser) &&
                Spectator.Equals(other.Spectator);
        }
    }
}