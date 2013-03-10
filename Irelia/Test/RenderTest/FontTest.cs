using System.IO;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using D3D = SlimDX.Direct3D9;

namespace RenderTest
{
    [TestClass()]
    public class FontTest
    {
        private Device Device { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            SpriteRenderer = new SpriteRenderer(Device);
            AssetManager = new AssetManager(TestHelpers.GetDevice(), RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void Font_SaveLoad_Test()
        {
            string name = "Font0";
            var texture = AssetManager.Load(@"Engine/system12.tga") as Texture;
            var dataFile = Path.Combine(RenderSettings.MediaPath, "Engine/system12.dat");
            var font = new Font(name, texture, dataFile);
            font.Save("Font.font");

            var font2 = Font.Load(Device, "Font.font", name, AssetManager);
            Assert.AreEqual(name, font2.Name);
        }

        [TestMethod()]
        public void Font_Print_Test()
        {
            var texture = new Texture(Device, Path.Combine(RenderSettings.MediaPath, "Engine/system12.tga"), "");
            string fontData = Path.Combine(RenderSettings.MediaPath, "Engine/system12.dat");
            var font = new Font("system12", texture, fontData);

            Device.RawDevice.BeginScene();
            SpriteRenderer.RawSprite.Begin(D3D.SpriteFlags.AlphaBlend);
            Assert.IsTrue(font.Print("text", Vector2.Zero, Color.White, SpriteRenderer));
            SpriteRenderer.RawSprite.End();
            Device.RawDevice.EndScene();
        }

        [TestMethod()]
        public void FontFactory_Test()
        {
            Assert.AreEqual(typeof(Font), AssetManager.GetAssetType("sample.font"));

            string path = @"Engine\system12.font";
            var font = AssetManager.Load(path) as Font;
            Assert.IsNotNull(font);
            Assert.AreEqual(path, font.Name);
        }
    }
}
