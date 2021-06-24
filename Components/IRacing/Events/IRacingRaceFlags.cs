using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingRaceFlags : IEvent
    {
        public string EventType => nameof(IRacingRaceFlags);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
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
    }
}