using System;

namespace Irelia.Render
{
    public sealed class Framework : DisposableObject
    {
        #region Properties
        public Device Device { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public AssetManager AssetManager { get; private set; }
        public RenderSystem Renderer { get; private set; }
        public Light Light { get; private set; }
        #endregion

        #region Constructor
        public Framework(IntPtr handle, int width, int height, string mediaRootPath)
        {
            Device = new Device(handle, width, height);
            AssetManager = new AssetManager(Device, mediaRootPath);
            Light = new Light();
            SpriteRenderer = new SpriteRenderer(Device);
            Renderer = new RenderSystem(Device, SpriteRenderer);
        }
        #endregion

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (Device != null)
                    Device.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
