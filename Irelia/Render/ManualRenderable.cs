using SlimDX.Direct3D9;
using System.Diagnostics;

namespace Irelia.Render
{
    public abstract class ManualRenderable<T> : DisposableObject, IRenderable where T : struct
    {
        // The rendering operation type to perform
        protected enum OperationType
        {
            LineList,
            TriangleStrip,
            TriangleList
        }

        private readonly Device device;
        private readonly VertexDecl vertexDecl;
        private readonly VertexBuffer<T> vertices;
        private readonly IndexBuffer indices;

        public ManualRenderable(Device device, VertexDecl vertexDecl)
        {
            this.device = device;
            this.vertexDecl = vertexDecl;
            this.vertices = new VertexBuffer<T>(device);
            this.indices = new IndexBuffer(device);
        }

        public void AddVertex(T[] vertices)
        {
            this.vertices.Write(vertices);
        }

        public void AddIndex(short[] indices)
        {
            this.indices.Write(indices);
        }

        bool IRenderable.Render(Camera camera, Light light)
        {
            return Render(camera, light);
        }

        protected bool RenderVertices(OperationType operationType)
        {
            this.device.RawDevice.VertexDeclaration = vertexDecl.RawDecl;
            this.device.RawDevice.SetStreamSource(0, this.vertices.RawBuffer, 0, this.vertices.ElementSize);

            PrimitiveType primType = PrimitiveType.TriangleList;
            int primCount = 0;
            switch (operationType)
            {
                case OperationType.LineList:
                    primType = PrimitiveType.LineList;
                    primCount = ((this.indices.Count > 0)? this.indices.Count : this.vertices.Count) / 2;
                    break;

                case OperationType.TriangleStrip:
                    primType = PrimitiveType.TriangleStrip;
                    primCount = ((this.indices.Count > 0)? this.indices.Count : this.vertices.Count) - 2;
                    break;

                case OperationType.TriangleList:
                    primType = PrimitiveType.TriangleList;
                    primCount = ((this.indices.Count > 0)? this.indices.Count : this.vertices.Count) / 3;
                    break;

                default:
                    Debug.Assert(false, "Invalid rendering operation type: " + operationType);
                    return false;
            }


            if (this.indices.Count > 0)
            {
                this.device.RawDevice.Indices = this.indices.RawBuffer;
                return this.device.RawDevice.DrawIndexedPrimitives(primType, 0, 0, this.vertices.Count, 0, primCount).IsSuccess;
            }
            else
            {
                return this.device.RawDevice.DrawPrimitives(primType, 0, primCount).IsSuccess;
            }
        }

        protected abstract bool Render(Camera camera, Light light);

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (vertices != null)
                    vertices.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
