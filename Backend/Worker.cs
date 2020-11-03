using System;
using System.Threading;

namespace Slipstream.Backend
{
    abstract class Worker : IDisposable
    {
        private readonly Thread WorkerThread;
        private readonly object StopLock = new object();
        private bool stop;
        protected bool Stopped
        {
            get
            {
                lock (StopLock)
                {
                    return stop;
                }
            }
        }

        protected Worker()
        {
            WorkerThread = new Thread(new ThreadStart(this.Main));
        }

        public void Start()
        {
            WorkerThread.Start();
        }

        public void Stop()
        {
            if (!stop)
            {
                lock (StopLock)
                {
                    stop = true;
                }
                WorkerThread.Join();
            }
        }

        abstract protected void Main();

        public void Dispose()
        {
            Stop();
        }
    }
}
