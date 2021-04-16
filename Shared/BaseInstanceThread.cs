#nullable enable

using Serilog;
using System;
using System.Threading;

namespace Slipstream.Shared
{
    public abstract class BaseInstanceThread : IDisposable
    {
        private readonly Thread ServiceThread;
        protected readonly string InstanceId;
        protected readonly ILogger Logger;
        protected volatile bool Stopping = false;

        protected BaseInstanceThread(string instanceId, ILogger logger)
        {
            InstanceId = instanceId;
            Logger = logger;
            ServiceThread = new Thread(new ThreadStart(ThreadMain))
            {
                Name = GetType().Name + " " + instanceId
            };
        }

        private void ThreadMain()
        {
            Logger.Debug($"Starting {GetType().Name } {InstanceId}");
            Main();
            Logger.Debug($"Stopping {GetType().Name } {InstanceId}");
        }

        protected abstract void Main();

        public void Start()
        {
            ServiceThread.Start();
        }

        public void Dispose()
        {
            Stopping = true;
            ServiceThread?.Join();
        }
    }
}