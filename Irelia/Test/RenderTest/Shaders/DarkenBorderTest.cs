using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class DarkenBorderTest
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
        public void DarkenBorder_Render_Test()
        {
            var darkenBorder = new DarkenBorder(Device, AssetManager)
            {
                BorderTexture = new Texture(Device, AssetManager.GetFullPath(@"Engine\ScreenBorderFadeout.dds"), "")
            };

            Device.SetRenderTarget(new TextureRenderTarget(Device, new Size(100, 100)));
            Device.RawDevice.BeginScene();
            Assert.IsTrue((darkenBorder as IRenderable).Render(new Camera(), new Light()));
            Device.RawDevice.EndScene();
        }
    }
}
