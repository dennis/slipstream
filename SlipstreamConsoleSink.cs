using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Slipstream.Components.UI;
using Slipstream.Shared;

#nullable enable

namespace Slipstream
{
    public class SlipstreamConsoleSink : ILogEventSink
    {
        private readonly object ThreadLock = new object();
        public IEventBus? EventBus { get; set; }
        public IUIEventFactory? EventFactory { get; set; }
        public IEventEnvelope Envelope = new EventEnvelope("SlipstreamConsoleSink");

        public void Emit(LogEvent logEvent)
        {
            if (EventBus == null || EventFactory == null)
                return;

            lock (ThreadLock)
            {
                var message = logEvent.RenderMessage();

                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole(Envelope, message));
            }
        }
    }

    public static class SlipstreamConsoleSinkExtension
    {
        public static LoggerConfiguration SlipstreamConsoleSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  out SlipstreamConsoleSink myself)
        {
            myself = new SlipstreamConsoleSink();
            return loggerConfiguration.Sink(myself);
        }
    }
}