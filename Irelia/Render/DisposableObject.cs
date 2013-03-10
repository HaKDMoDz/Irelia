using System;

namespace Irelia.Render
{
    public abstract class DisposableObject : IDisposable
    {
        public bool IsDisposed { get; set; }

        protected DisposableObject()
        {
            IsDisposed = false;
        }

        ~DisposableObject()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (IsDisposed)
                return;
            
            if (disposeManagedResources)
            {
                // Dispose managed resources
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
