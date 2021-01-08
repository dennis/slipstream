#nullable enable

using System.Collections.Generic;

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

        public override bool Equals(object? obj)
        {
            return obj is IRacingWeatherInfo info &&
                   EventType == info.EventType &&
                   ExcludeFromTxrx == info.ExcludeFromTxrx &&
                   SessionTime == info.SessionTime &&
                   Skies == info.Skies &&
                   SurfaceTemp == info.SurfaceTemp &&
                   AirTemp == info.AirTemp &&
                   AirPressure == info.AirPressure &&
                   RelativeHumidity == info.RelativeHumidity &&
                   FogLevel == info.FogLevel;
        }

        public override int GetHashCode()
        {
            int hashCode = 1373460440;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Skies);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SurfaceTemp);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AirTemp);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AirPressure);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelativeHumidity);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FogLevel);
            return hashCode;
        }
    }
}