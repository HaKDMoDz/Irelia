using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class TextureRenderTargetTest
    {
        private Device Device { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            SpriteRenderer = new SpriteRenderer(Device);
        }

        [TestMethod()]
        public void TextureRenderTarget_Constructor_Test()
        {
            var rt = new TextureRenderTarget(Device, new Size(100, 100));
            Assert.IsNotNull(rt.Texture);
            Assert.IsNotNull(rt.TargetSurface);
            Assert.IsNotNull(rt.DepthStencilSurface);
        }
    }
}
