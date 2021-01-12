using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Slipstream;
using Slipstream.Shared;

#nullable enable

namespace Slipstream
{
    public class SlipstreamConsoleSink : ILogEventSink
    {
        private readonly object ThreadLock = new object();
        public IEventBus? EventBus { get; set; }
        public IEventFactory? EventFactory { get; set; }

        public void Emit(LogEvent logEvent)
        {
            lock (ThreadLock)
            {
                var message = logEvent.RenderMessage();

                EventBus?.PublishEvent(EventFactory?.CreateUICommandWriteToConsole(message));
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
