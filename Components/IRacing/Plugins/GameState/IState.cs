#nullable enable

using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    public interface IState
    {
        public double SessionTime { get; }
        public int SessionNum { get; }
        public long DriverCarIdx { get; }
        public int DriverIncidentCount { get; }
        public Car[] Cars { get; }
        public float FuelLevel { get; }
        public float LFtempCL { get; }
        public float LFtempCM { get; }
        public float LFtempCR { get; }
        public float RFtempCL { get; }
        public float RFtempCM { get; }
        public float RFtempCR { get; }
        public float LRtempCL { get; }
        public float LRtempCM { get; }
        public float LRtempCR { get; }
        public float RRtempCL { get; }
        public float RRtempCM { get; }
        public float RRtempCR { get; }
        public float LFwearL { get; }
        public float LFwearM { get; }
        public float LFwearR { get; }
        public float RFwearL { get; }
        public float RFwearM { get; }
        public float RFwearR { get; }
        public float LRwearL { get; }
        public float LRwearM { get; }
        public float LRwearR { get; }
        public float RRwearL { get; }
        public float RRwearM { get; }
        public float RRwearR { get; }
        public long TrackId { get; }
        public string TrackLength { get; }
        public string TrackDisplayName { get; }
        public string TrackCity { get; }
        public string TrackCountry { get; }
        public string TrackDisplayShortName { get; }
        public string TrackConfigName { get; }
        public string TrackType { get; }
        public Skies Skies { get; }
        public float TrackTempCrew { get; }
        public float AirTemp { get; }
        public float AirPressure { get; }
        public float RelativeHumidity { get; }
        public float FogLevel { get; }
        public IRacingCategoryEnum RaceCategory { get; }
        public SessionFlags SessionFlags { get; }
        public IRacingSessionStateEnum SessionState { get; }
        public IRacingSessionTypeEnum SessionType { get; }
        public ISession[] Sessions { get; }
        public ISession CurrentSession { get; }
    }
}