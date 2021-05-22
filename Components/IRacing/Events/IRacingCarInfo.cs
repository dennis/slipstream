#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCarInfo : IEvent
    {
        public string EventType => "IRacingCarInfo";
        public ulong Uptime { get; set; }
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
        public string CurrentDriverLicense { get; set; } = string.Empty;
        public bool LocalUser { get; set; }
        public bool Spectator { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IRacingCarInfo info &&
                   EventType == info.EventType &&
                   SessionTime == info.SessionTime &&
                   CarIdx == info.CarIdx &&
                   CarNumber == info.CarNumber &&
                   CurrentDriverUserID == info.CurrentDriverUserID &&
                   CurrentDriverName == info.CurrentDriverName &&
                   TeamID == info.TeamID &&
                   TeamName == info.TeamName &&
                   CarName == info.CarName &&
                   CarNameShort == info.CarNameShort &&
                   CurrentDriverIRating == info.CurrentDriverIRating &&
                   CurrentDriverLicense == info.CurrentDriverLicense &&
                   LocalUser == info.LocalUser &&
                   Spectator == info.Spectator;
        }

        public override int GetHashCode()
        {
            int hashCode = 854757270;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CarNumber);
            hashCode = hashCode * -1521134295 + CurrentDriverUserID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrentDriverName);
            hashCode = hashCode * -1521134295 + TeamID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TeamName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CarName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CarNameShort);
            hashCode = hashCode * -1521134295 + CurrentDriverIRating.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrentDriverLicense);
            hashCode = hashCode * -1521134295 + LocalUser.GetHashCode();
            hashCode = hashCode * -1521134295 + Spectator.GetHashCode();
            return hashCode;
        }

        public bool SameAs(IRacingCarInfo other)
        {
            return
                CarIdx.Equals(other.CarIdx) &&
                CarNumber.Equals(other.CarNumber) &&
                CurrentDriverUserID.Equals(other.CurrentDriverUserID) &&
                CurrentDriverName.Equals(other.CurrentDriverName) &&
                CurrentDriverIRating.Equals(other.CurrentDriverIRating) &&
                CurrentDriverLicense.Equals(other.CurrentDriverLicense) &&
                TeamID.Equals(other.TeamID) &&
                TeamName.Equals(other.TeamName) &&
                CarName.Equals(other.CarName) &&
                CarNameShort.Equals(other.CarNameShort) &&
                LocalUser.Equals(other.LocalUser) &&
                Spectator.Equals(other.Spectator);
        }
    }
}