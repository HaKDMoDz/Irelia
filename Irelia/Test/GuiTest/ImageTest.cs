using System.IO;
using System.Xml;
using Irelia.Gui;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using D3D = SlimDX.Direct3D9;

namespace GuiTest
{
    [TestClass()]
    public class ImageTest
    {
        private Device Device { get; set; }
        private RenderSystem RenderSystem { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            RenderSystem = new RenderSystem(Device, new SpriteRenderer(Device));
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void Image_Constructor_Test()
        {
            var img = new Image(new TestHelpers.RootElement(), AssetManager);
            Assert.AreEqual(ElementType.Image, img.Type);
            Assert.IsNull(img.Texture);
            Assert.AreEqual(new Rectangle(0, 0, 0, 0), img.SourceRect);
        }

        [TestMethod()]
        public void Image_Render_Test()
        {
            var texture = AssetManager.Load(@"Test\Default_color.dds") as Texture;
            var img = new Image(new TestHelpers.RootElement(), AssetManager)
            {
                Texture = texture,
                DestRect = new Rectangle(0.1f, 0.2f, 0.4f, 0.5f),
                SourceRect = new Rectangle(0, 0, texture.Width, texture.Height)
            };

            TestHelpers.RenderSprite(Device, s => Assert.IsTrue((img as ISprite).Render(s)));
        }

        [TestMethod()]
        public void Image_XmlSerialize_Test()
        {
            var texture = AssetManager.Load(@"Test\Default_color.dds") as Texture;
            var parent = new TestHelpers.RootElement();
            var expected = new Image(parent, AssetManager)
            {
                Texture = texture,
                DestRect = new Rectangle(0.1f, 0.2f, 0.4f, 0.5f),
                SourceRect = new Rectangle(1, 2, texture.Width - 1, texture.Height - 2)
            };

            var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream))
            {
                expected.WriteXml(xmlWriter);
            }
            
            stream.Position = 0;
            var actual = new Image(parent, AssetManager);
            using (var xmlReader = XmlReader.Create(stream))
            {
                actual.ReadXml(xmlReader);
            }

            Assert.AreEqual(expected.Texture, texture);
            Assert.AreEqual(expected.DestRect, actual.DestRect);
            Assert.AreEqual(expected.AbsRect, actual.AbsRect);
            Assert.AreEqual(expected.SourceRect, actual.SourceRect);
        }

        [TestMethod()]
        public void Image_SomePropertiesChangeIfTextureChanged_Test()
        {
            var destRect = new Rectangle(0.1f, 0.2f, 0.4f, 0.5f);
            var sourceRect = new Rectangle(1, 2, 100, 100);

            var parent = new TestHelpers.RootElement();
            var img = new Image(parent, AssetManager)
            {
                DestRect = destRect,
                SourceRect = sourceRect
            };

            img.Texture = AssetManager.Load(@"Test\Default_color.dds") as Texture;
            Assert.AreEqual(destRect, img.DestRect);
            Assert.AreEqual(sourceRect, img.SourceRect);
        }
    }
}
