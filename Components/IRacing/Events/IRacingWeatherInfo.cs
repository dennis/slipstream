#nullable enable

using Slipstream.Shared;
using System;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingWeatherInfo : IEvent
    {
        public string EventType => "IRacingWeatherInfo";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public string Skies { get; set; } = string.Empty;
        public float SurfaceTemp { get; set; }
        public float AirTemp { get; set; }
        public float AirPressure { get; set; }
        public float RelativeHumidity { get; set; }
        public float FogLevel { get; set; }

        public bool DifferentTo(IRacingWeatherInfo other)
        {
            return !(
                other.Skies.Equals(Skies) &&
                Math.Abs(other.SurfaceTemp - SurfaceTemp) < 0.1 &&
                Math.Abs(other.AirTemp - AirTemp) < 0.1 &&
                Math.Abs(other.AirPressure - AirPressure) < 0.1 &&
                Math.Abs(other.RelativeHumidity - RelativeHumidity) < 0.1 &&
                Math.Abs(other.FogLevel - FogLevel) < 0.1
            );
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
            hashCode = hashCode * -1521134295 + SurfaceTemp.GetHashCode();
            hashCode = hashCode * -1521134295 + AirTemp.GetHashCode();
            hashCode = hashCode * -1521134295 + AirPressure.GetHashCode();
            hashCode = hashCode * -1521134295 + RelativeHumidity.GetHashCode();
            hashCode = hashCode * -1521134295 + FogLevel.GetHashCode();
            return hashCode;
        }
    }
}