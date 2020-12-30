#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingWeatherInfo : IEvent
    {
        public string EventType => "IRacingWeatherInfo";
        public bool ExcludeFromTxrx => false;
        public double SessionTime { get; set; }
        public string Skies { get; set; } = string.Empty;
        public string SurfaceTemp { get; set; } = string.Empty;
        public string AirTemp { get; set; } = string.Empty;
        public string AirPressure { get; set; } = string.Empty;
        public string RelativeHumidity { get; set; } = string.Empty;
        public string FogLevel { get; set; } = string.Empty;

        public bool DifferentTo(IRacingWeatherInfo other)
        {
            return
                other.Skies.Equals(Skies) &&
                other.SurfaceTemp.Equals(SurfaceTemp) &&
                other.AirTemp.Equals(AirTemp) &&
                other.AirPressure.Equals(AirPressure) &&
                other.RelativeHumidity.Equals(RelativeHumidity) &&
                other.FogLevel.Equals(FogLevel);
        }
    }
}