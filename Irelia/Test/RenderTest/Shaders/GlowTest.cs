using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class GlowTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void Glow_Render_Test()
        {
            var glow = new Glow(Device, AssetManager);

            Device.SetRenderTarget(new TextureRenderTarget(Device, new Size(100, 100)));
            Device.RawDevice.BeginScene();
            Assert.IsTrue((glow as IRenderable).Render(new Camera(), new Light()));
            Device.RawDevice.EndScene();
        }
    }
}
