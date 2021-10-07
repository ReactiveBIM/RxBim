namespace RxBim.Di.Tests
{
    using System;

    public abstract class ServiceBase : IDisposable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}