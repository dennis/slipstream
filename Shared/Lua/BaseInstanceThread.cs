#nullable enable

using Serilog;
using System;
using System.Threading;

namespace Slipstream.Shared.Lua
{
    public abstract class BaseInstanceThread : ILuaInstanceThread
    {
        protected readonly string InstanceId;
        protected readonly ILogger Logger;
        private Thread ServiceThread;
        protected volatile bool Stopping = false;
        protected volatile bool AutoStart = false;

        protected BaseInstanceThread(string instanceId, ILogger logger)
        {
            InstanceId = instanceId;
            Logger = logger;
            SetupThread();
        }

        private void SetupThread()
        {
            ServiceThread = new Thread(new ThreadStart(ThreadMain))
            {
                Name = GetType().Name + " " + InstanceId,
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

            if (AutoStart)
            {
                AutoStart = false;
                Stopping = false;
                ThreadMain();
            }
        }

        protected abstract void Main();

        public void Start()
        {
            if (ServiceThread?.ThreadState == ThreadState.Unstarted)
            {
                ServiceThread.Start();
            }
            else if (ServiceThread?.ThreadState == ThreadState.Stopped)
            {
                // We need to recreate it
                SetupThread();
                ServiceThread.Start();
            }
        }

        public void Stop()
        {
            if (ServiceThread?.IsAlive == true)
                Stopping = true;
        }

        public void Restart()
        {
            if (ServiceThread?.IsAlive == true)
            {
                Stop();
                AutoStart = true;
            }
            else
            {
                Start();
            }
        }

        public void Join()
        {
            if (ServiceThread?.IsAlive == true)
                ServiceThread.Join();
        }

        public void Dispose()
        {
            Stopping = true;
            if (ServiceThread?.IsAlive == true)
                ServiceThread?.Join();
        }
    }
}