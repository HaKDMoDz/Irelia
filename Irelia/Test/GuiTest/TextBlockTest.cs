using Irelia.Gui;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Irelia.Render;
using System.IO;
using D3D = SlimDX.Direct3D9;
using System.Xml;

namespace GuiTest
{
    [TestClass()]
    public class TextBlockTest
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
        public void TextBlock_Constructor_Test()
        {
            var block = new TextBlock(new TestHelpers.RootElement(), AssetManager);
            Assert.IsNull(block.Font);
            Assert.AreEqual(Color.Black, block.Foreground);
            Assert.AreEqual("", block.Text);
            Assert.AreEqual(Vector2.Zero, block.Position);
        }

        [TestMethod()]
        public void TextBlock_Render_Test()
        {
            var text = "Text will be displayed";
            var font = AssetManager.Load(@"Engine\system12.font") as Font;
            var color = Color.White;
            var block = new TextBlock(new TestHelpers.RootElement(), AssetManager)
            {
                Font = font,
                Text = text,
                Foreground = color
            };

            Assert.AreEqual(text, block.Text);
            Assert.AreEqual(font, block.Font);
            Assert.AreEqual(color, block.Foreground);
            
            TestHelpers.RenderSprite(Device, s => Assert.IsTrue((block as ISprite).Render(s)));
        }

        [TestMethod()]
        public void TextBlock_XmlSerialize_Test()
        {
            var text = "Text will be displayed";
            var font = AssetManager.Load(@"Engine\system12.font") as Font;
            var color = Color.Gray;
            var position = new Vector2(1.0f, 2.0f);
            var parent = new TestHelpers.RootElement();
            var expected = new TextBlock(parent, AssetManager)
            {
                Font = font,
                Text = text,
                Foreground = color,
                Position = position
            };

            var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream))
            {
                expected.WriteXml(xmlWriter);
            }

            stream.Position = 0;
            var actual = new TextBlock(parent, AssetManager);
            using (var xmlReader = XmlReader.Create(stream))
            {
                actual.ReadXml(xmlReader);
            }

            Assert.AreEqual(expected.Font, actual.Font);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Foreground, actual.Foreground);
            Assert.AreEqual(expected.Position, actual.Position);
        }
    }
}
