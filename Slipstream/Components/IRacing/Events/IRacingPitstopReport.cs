#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingPitstopReport : IEvent
    {
        public string EventType => nameof(IRacingPitstopReport);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }

        #region Temperature: Left Front

        public uint TempLFL { get; set; }
        public uint TempLFM { get; set; }
        public uint TempLFR { get; set; }

        #endregion Temperature: Left Front

        #region Temperature: Right Front

        public uint TempRFL { get; set; }
        public uint TempRFM { get; set; }
        public uint TempRFR { get; set; }

        #endregion Temperature: Right Front

        #region Temperature: Left Rear

        public uint TempLRL { get; set; }
        public uint TempLRM { get; set; }
        public uint TempLRR { get; set; }

        #endregion Temperature: Left Rear

        #region Temperature: Right Rear

        public uint TempRRL { get; set; }
        public uint TempRRM { get; set; }
        public uint TempRRR { get; set; }

        #endregion Temperature: Right Rear

        #region Wear: Left Front

        public uint WearLFL { get; set; }
        public uint WearLFM { get; set; }
        public uint WearLFR { get; set; }

        #endregion Wear: Left Front

        #region Wear: Right Front

        public uint WearRFL { get; set; }
        public uint WearRFM { get; set; }
        public uint WearRFR { get; set; }

        #endregion Wear: Right Front

        #region Wear: Left Rear

        public uint WearLRL { get; set; }
        public uint WearLRM { get; set; }
        public uint WearLRR { get; set; }

        #endregion Wear: Left Rear

        #region Wear: Right Rear

        public uint WearRRL { get; set; }
        public uint WearRRM { get; set; }
        public uint WearRRR { get; set; }

        #endregion Wear: Right Rear

        public long Laps { get; set; }
        public float FuelDelta { get; set; }
        public double Duration { get; set; }
    }
}