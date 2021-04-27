#nullable enable

using Serilog;
using System;
using System.Threading;

namespace Slipstream.Shared.Lua
{
    public abstract class BaseInstanceThread : ILuaInstanceThread
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
                Name = GetType().Name + " " + instanceId,
            };
        }

        private void ThreadMain()
        {
            Logger.Debug($"Starting {GetType().Name } {InstanceId}");
            try
            {
                Main();
            }
            catch (Exception e)
            {
                Logger.Error($"Exception in {GetType().Name} {InstanceId}: {e.Message}");
            }
            Logger.Debug($"Stopping {GetType().Name } {InstanceId}");
        }

        protected abstract void Main();

        public void Start()
        {
            ServiceThread.Start();
        }

        public void Stop()
        {
            Stopping = true;
        }

        public void Join()
        {
            ServiceThread.Join();
        }

        public void Dispose()
        {
            Stopping = true;
            ServiceThread?.Join();
        }
    }
}