using System;
using System.Collections.Generic;
using System.Threading;

namespace Slipstream.Backend
{
    public class CircularBlockingQueue<T> : ICircularBlockingQueue<T>, IDisposable where T : class
    {
        private readonly List<T> Storage;
        private readonly int Size;
        private int FrontIdx = -1;
        private int RearIdx = -1;
        private readonly Semaphore Sema = new Semaphore(0, int.MaxValue);

        public CircularBlockingQueue(int size)
        {
            Size = size;
            Storage = new List<T>(Size);
        }

        public void Enqueue(T item)
        {
            lock (Storage)
                EnqueueNoLock(item);
        }

        private void EnqueueNoLock(T item)
        {
            if (Full)
            {
                DropFrontItemNoLock();
            }

            if (FrontIdx == -1)
                FrontIdx = 0;
            RearIdx = (RearIdx + 1) % Size;

            if (Storage.Count == RearIdx)
            {
                // Storage isn't fully populated, so we need to add it
                Storage.Add(item);
            }
            else
            {
                Storage[RearIdx] = item;
            }

            Sema.Release();
        }

        public T? Dequeue(int timeout)
        {
            lock (Storage)
                return DequeueNoLock(timeout);
        }

        private T? DequeueNoLock(int millisecondsTimeout)
        {
            if (Empty)
            {
                return null;
            }
            if (Sema.WaitOne(millisecondsTimeout))
            {
                var item = Storage[FrontIdx];
                DropFrontItemNoLock();
                return item;
            }
            return null;
        }

        private void DropFrontItemNoLock()
        {
            if (FrontIdx == RearIdx)
            {
                // Only one item left, so we're back to initial state
                FrontIdx = RearIdx = -1;
            }
            else
            {
                FrontIdx = (FrontIdx + 1) % Size;
            }
        }

        public void Dispose()
        {
            Sema.Close();
            Sema.Dispose();
        }

        public ICircularBlockingQueue<T> Clone()
        {
            lock (Storage)
            {
                var clone = new CircularBlockingQueue<T>(Size);

                int idx = FrontIdx;
                while (idx != RearIdx)
                {
                    clone.Enqueue(Storage[idx]);
                    idx = (idx + 1) % Size;
                }

                return clone;
            }
        }

        private bool Full { get => (FrontIdx == 0 && RearIdx == Size - 1) || (FrontIdx == RearIdx + 1); }
        private bool Empty { get => FrontIdx == -1; }
    }
}