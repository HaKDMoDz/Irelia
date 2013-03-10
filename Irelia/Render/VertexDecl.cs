using SlimDX.Direct3D9;

namespace Irelia.Render
{
    public class VertexDecl : DisposableObject
    {
        public VertexDecl(Device device, VertexElement[] elements)
        {
            if (elements == null)
                return;

            if (elements[elements.Length - 1] != VertexElement.VertexDeclarationEnd)
                return;

            d3d9VertexDecl = new VertexDeclaration(device.RawDevice, elements);
        }

        private readonly VertexDeclaration d3d9VertexDecl;

        public VertexDeclaration RawDecl
        {
            get { return this.d3d9VertexDecl; }
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.d3d9VertexDecl != null)
                    this.d3d9VertexDecl.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
