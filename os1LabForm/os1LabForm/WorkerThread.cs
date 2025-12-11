using System;

namespace os1LabForm
{
    public abstract class WorkerThread
    {
        public abstract bool IsAlive { get; }
        public abstract void Start();
        public abstract void Stop();
        public abstract string GetStatus();
    }
}