using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class TrueTypeFont : DisposableObject
    {
        private readonly D3D.Font font;

        public TrueTypeFont(Device device, int size)
        {
            this.font = new D3D.Font(device.RawDevice, size, 0, D3D.FontWeight.Normal, 0, false, 
                D3D.CharacterSet.Default, D3D.Precision.Outline, D3D.FontQuality.Default, 
                D3D.PitchAndFamily.Default, "");
        }

        public bool DrawString(int x, int y, string text, int color)
        {
            return (this.font.DrawString(null, text, x, y, color) != 0);
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.font != null)
                    this.font.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
