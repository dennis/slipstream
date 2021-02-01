using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingRaceFlags : IEvent
    {
        public string EventType => "IRacingRaceFlags";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public bool Black { get; set; }
        public bool Blue { get; set; }
        public bool Caution { get; set; }
        public bool CautionWaving { get; set; }
        public bool Checkered { get; set; }
        public bool Crossed { get; set; }
        public bool Debris { get; set; }
        public bool Disqualify { get; set; }
        public bool FiveToGo { get; set; }
        public bool Furled { get; set; }
        public bool Green { get; set; }
        public bool GreenHeld { get; set; }
        public bool OneLapToGreen { get; set; }
        public bool RandomWaving { get; set; }
        public bool Red { get; set; }
        public bool Repair { get; set; }
        public bool Servicible { get; set; }
        public bool StartGo { get; set; }
        public bool StartHidden { get; set; }
        public bool StartReady { get; set; }
        public bool StartSet { get; set; }
        public bool TenToGo { get; set; }
        public bool White { get; set; }
        public bool Yellow { get; set; }
        public bool YellowWaving { get; set; }

        public bool DifferentTo(IRacingRaceFlags other)
        {
            return
               Black == other.Black &&
               Blue == other.Blue &&
               Caution == other.Caution &&
               CautionWaving == other.CautionWaving &&
               Checkered == other.Checkered &&
               Crossed == other.Crossed &&
               Debris == other.Debris &&
               Disqualify == other.Disqualify &&
               FiveToGo == other.FiveToGo &&
               Furled == other.Furled &&
               Green == other.Green &&
               GreenHeld == other.GreenHeld &&
               OneLapToGreen == other.OneLapToGreen &&
               RandomWaving == other.RandomWaving &&
               Red == other.Red &&
               Repair == other.Repair &&
               Servicible == other.Servicible &&
               StartGo == other.StartGo &&
               StartHidden == other.StartHidden &&
               StartReady == other.StartReady &&
               StartSet == other.StartSet &&
               TenToGo == other.TenToGo &&
               White == other.White &&
               Yellow == other.Yellow &&
               YellowWaving == other.YellowWaving;
        }

        public override bool Equals(object obj)
        {
            return obj is IRacingRaceFlags flags &&
                   EventType == flags.EventType &&
                   ExcludeFromTxrx == flags.ExcludeFromTxrx &&
                   SessionTime == flags.SessionTime &&
                   Black == flags.Black &&
                   Blue == flags.Blue &&
                   Caution == flags.Caution &&
                   CautionWaving == flags.CautionWaving &&
                   Checkered == flags.Checkered &&
                   Crossed == flags.Crossed &&
                   Debris == flags.Debris &&
                   Disqualify == flags.Disqualify &&
                   FiveToGo == flags.FiveToGo &&
                   Furled == flags.Furled &&
                   Green == flags.Green &&
                   GreenHeld == flags.GreenHeld &&
                   OneLapToGreen == flags.OneLapToGreen &&
                   RandomWaving == flags.RandomWaving &&
                   Red == flags.Red &&
                   Repair == flags.Repair &&
                   Servicible == flags.Servicible &&
                   StartGo == flags.StartGo &&
                   StartHidden == flags.StartHidden &&
                   StartReady == flags.StartReady &&
                   StartSet == flags.StartSet &&
                   TenToGo == flags.TenToGo &&
                   White == flags.White &&
                   Yellow == flags.Yellow &&
                   YellowWaving == flags.YellowWaving;
        }

        public override int GetHashCode()
        {
            int hashCode = -1865722245;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + Black.GetHashCode();
            hashCode = hashCode * -1521134295 + Blue.GetHashCode();
            hashCode = hashCode * -1521134295 + Caution.GetHashCode();
            hashCode = hashCode * -1521134295 + CautionWaving.GetHashCode();
            hashCode = hashCode * -1521134295 + Checkered.GetHashCode();
            hashCode = hashCode * -1521134295 + Crossed.GetHashCode();
            hashCode = hashCode * -1521134295 + Debris.GetHashCode();
            hashCode = hashCode * -1521134295 + Disqualify.GetHashCode();
            hashCode = hashCode * -1521134295 + FiveToGo.GetHashCode();
            hashCode = hashCode * -1521134295 + Furled.GetHashCode();
            hashCode = hashCode * -1521134295 + Green.GetHashCode();
            hashCode = hashCode * -1521134295 + GreenHeld.GetHashCode();
            hashCode = hashCode * -1521134295 + OneLapToGreen.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomWaving.GetHashCode();
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            hashCode = hashCode * -1521134295 + Repair.GetHashCode();
            hashCode = hashCode * -1521134295 + Servicible.GetHashCode();
            hashCode = hashCode * -1521134295 + StartGo.GetHashCode();
            hashCode = hashCode * -1521134295 + StartHidden.GetHashCode();
            hashCode = hashCode * -1521134295 + StartReady.GetHashCode();
            hashCode = hashCode * -1521134295 + StartSet.GetHashCode();
            hashCode = hashCode * -1521134295 + TenToGo.GetHashCode();
            hashCode = hashCode * -1521134295 + White.GetHashCode();
            hashCode = hashCode * -1521134295 + Yellow.GetHashCode();
            hashCode = hashCode * -1521134295 + YellowWaving.GetHashCode();
            return hashCode;
        }
    }
}