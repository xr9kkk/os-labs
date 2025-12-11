using System;
using System.Collections.Generic;
using System.Threading;

namespace os1LabForm
{
    public class Buffer<T>
    {
        private readonly Queue<T> queue;
        private readonly int maxSize;
        private readonly object syncObject = new object();
        private bool isActive = true;

        public bool HasWriter { get; set; }
        public bool HasReader { get; set; }
        public bool IsCompletePair => HasWriter && HasReader;

        public Buffer(int size)
        {
            queue = new Queue<T>();
            maxSize = size;
        }

        public int Count
        {
            get
            {
                Monitor.Enter(syncObject);
                try { return queue.Count; }
                finally { Monitor.Exit(syncObject); }
            }
        }

        public int MaxSize => maxSize;
        public bool IsEmpty => Count == 0;
        public bool IsFull => Count >= maxSize;

        public bool IsActive
        {
            get
            {
                Monitor.Enter(syncObject);
                try { return isActive; }
                finally { Monitor.Exit(syncObject); }
            }
        }

        public void Deactivate()
        {
            Monitor.Enter(syncObject);
            try
            {
                isActive = false;
                Monitor.PulseAll(syncObject);
            }
            finally
            {
                Monitor.Exit(syncObject);
            }
        }

        public bool Put(T item)
        {
            Monitor.Enter(syncObject);
            try
            {
                if (!isActive) return false;
                if (IsFull) return false;

                queue.Enqueue(item);
                Monitor.PulseAll(syncObject);
                return true;
            }
            finally
            {
                Monitor.Exit(syncObject);
            }
        }

        public T Take()
        {
            Monitor.Enter(syncObject);
            try
            {
                while (IsEmpty && isActive)
                {
                    Monitor.Wait(syncObject);
                }

                if (!isActive || IsEmpty)
                    throw new InvalidOperationException("Buffer is deactivated or empty");

                T item = queue.Dequeue();
                Monitor.PulseAll(syncObject);
                return item;
            }
            finally
            {
                Monitor.Exit(syncObject);
            }
        }
    }
}