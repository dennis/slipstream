using System;
using System.Diagnostics;
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
                Debug.WriteLine($"[Worker-{Name}] Starting worker group");
                WorkerThread.Start();
                started = true;
            }
        }

        public void Stop()
        {
            if (!stop)
            {
                Debug.WriteLine($"[Worker-{Name}] Stopping worker group");
                lock (StopLock)
                {
                    stop = true;
                }
                Debug.WriteLine($"[Worker-{Name}] Waiting for it to stop");
                WorkerThread.Join();
                Debug.WriteLine($"[Worker-{Name}] Stopped");
            }
        }

        abstract protected void Main();

        public void Dispose()
        {
            Stop();
        }
    }
}
