using System;
using System.Threading;

namespace Slipstream.Backend
{
    abstract class Worker : IDisposable
    {
        public string Name { get; }
        private readonly Thread WorkerThread;
        private readonly object StopLock = new object();
        private bool stop;
        private bool started;
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

        protected Worker(string name)
        {
            Name = name;
            WorkerThread = new Thread(new ThreadStart(this.Main));
        }

        public void Start()
        {
            if (!started)
            {
                WorkerThread.Start();
                started = true;
            }
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
