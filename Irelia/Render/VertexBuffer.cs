using System.Collections.Generic;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;
using D3D = SlimDX.Direct3D9;
using System.Collections.ObjectModel;

namespace Irelia.Render
{
    public sealed class VertexBuffer<T> : DisposableObject where T: struct 
    {
        #region Fields
        private readonly Device device;
        private D3D.VertexBuffer rawBuffer;
        private int bufferSize;
        private readonly int initialBufferSize = 32;
        private readonly List<T> vertices = new List<T>();
        #endregion

        #region Properties
        public D3D.VertexBuffer RawBuffer
        {
            get { return this.rawBuffer; }
        }

        public int ElementSize
        {
            get 
            {
                if (typeof(T).IsGenericType)
                {
                    var t = default(T);
                    return Marshal.SizeOf(t);
                }
                else
                {
                    return Marshal.SizeOf(typeof(T));
                }
            }
        }

        public int Count
        {
            get { return this.vertices.Count; }
        }

        public ReadOnlyCollection<T> Vertices { get { return new ReadOnlyCollection<T>(this.vertices); } }
        #endregion

        #region Constructors
        public VertexBuffer(Device device)
        {
            this.device = device;
        }
        #endregion

        public void Write(T[] src)
        {
            this.vertices.AddRange(src);

            if (this.vertices.Count > this.bufferSize)
            {
                this.bufferSize = (this.bufferSize == 0) ? initialBufferSize : this.bufferSize * 2;
                this.bufferSize = System.Math.Max(this.bufferSize, this.vertices.Count);

                this.rawBuffer = new D3D.VertexBuffer(
                                device.RawDevice,
                                this.bufferSize * ElementSize,
                                Usage.WriteOnly,
                                VertexFormat.None,
                                Pool.Default);
            }

            DataStream stream = this.rawBuffer.Lock(0, 0, LockFlags.None);
            try
            {
                stream.WriteRange(this.vertices.ToArray());
            }
            finally
            {
                this.rawBuffer.Unlock();
            }
        }

        public void OverWrite(T[] src)
        {
            this.vertices.Clear();
            Write(src);
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.rawBuffer != null)
                    this.rawBuffer.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
