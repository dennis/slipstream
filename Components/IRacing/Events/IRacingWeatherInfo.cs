#nullable enable

using Slipstream.Shared;
using System;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingWeatherInfo : IEvent
    {
        public string EventType => nameof(IRacingWeatherInfo);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
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
    }
}