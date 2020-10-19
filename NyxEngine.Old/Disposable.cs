using System;

namespace NyxEngine.Old
{
    public abstract class Disposable : IDisposable
    {
        protected bool Disposed;

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // Clean up resources which implement IDisposable
            }

            Disposed = true;
        }
    }
}