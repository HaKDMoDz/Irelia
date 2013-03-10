using System;
using System.Collections.Generic;
using SlimDX;
using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class IndexBuffer : DisposableObject
    {
        #region Private fields
        private readonly Device device;
        private int bufferSize;
        private readonly int initialBufferSize = 32;
        #endregion

        #region Properties
        public List<short> Indices { get; private set; }

        public D3D.IndexBuffer RawBuffer { get; private set; }

        public int Count { get { return this.Indices.Count; } }
        #endregion

        public IndexBuffer(Device device)
        {
            this.device = device;
            Indices = new List<short>();
        }

        public bool Write(short[] src)
        {
            this.Indices.AddRange(src);

            if (this.Indices.Count > this.bufferSize)
            {
                this.bufferSize = (this.bufferSize == 0? initialBufferSize : this.bufferSize * 2);
                this.bufferSize = Math.Max(this.bufferSize, this.Indices.Count);

                RawBuffer = new D3D.IndexBuffer(device.RawDevice, sizeof(short) * this.bufferSize, D3D.Usage.None, D3D.Pool.Default, true);
            }

            DataStream stream = this.RawBuffer.Lock(0, 0, D3D.LockFlags.None);
            try
            {
                stream.WriteRange(this.Indices.ToArray());
            }
            catch
            {
                return false;
            }
            finally
            {
                this.RawBuffer.Unlock();
            }
            return true;
        }
    }
}
