using Irelia.Gui;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Irelia.Render;
using System.IO;
using System.Xml;

namespace GuiTest
{
    [TestClass()]
    public class ButtonTest
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
        public void Button_Constructor_Test()
        {
            var button = new Button(new TestHelpers.RootElement(), AssetManager);
        }

        [TestMethod()]
        public void Button_SetTemplate_Test()
        {
            var button = new Button(new TestHelpers.RootElement(), AssetManager);
            var block = new TextBlock(button, AssetManager) { Name = "Normal TextBlock" };
            
            Assert.IsTrue(button.SetTemplate(ButtonState.Normal, block.Name));
            Assert.IsFalse(block.IsLogical);
            Assert.AreEqual(block, button.GetTemplate(ButtonState.Normal));

            Assert.IsFalse(button.SetTemplate(ButtonState.Normal, "NonExistingChildName"));

            Assert.IsTrue(button.SetTemplate(ButtonState.Normal, null));
            Assert.IsTrue(block.IsLogical);
        }

        [TestMethod()]
        public void Button_State_Test()
        {
            var button = new Button(new TestHelpers.RootElement(), AssetManager);
            var block = new TextBlock(button, AssetManager) { Name = "Normal TextBlock" };
            var img = new Image(button, AssetManager) { Name = "Hover Image" };

            button.SetTemplate(ButtonState.Normal, block.Name);
            button.SetTemplate(ButtonState.Hover, img.Name);

            Assert.AreEqual(ButtonState.Normal, button.State);
            Assert.IsTrue(block.IsVisible);
            Assert.IsFalse(img.IsVisible);

            button.State = ButtonState.Hover;
            Assert.IsFalse(block.IsVisible);
            Assert.IsTrue(img.IsVisible);
        }

        [TestMethod()]
        public void Button_Render_Test()
        {
            var button = new Button(new TestHelpers.RootElement(), AssetManager);
            TestHelpers.RenderSprite(Device, (s) => Assert.IsFalse((button as ISprite).Render(s)));

            var block = new TextBlock(button, AssetManager) { Name = "Normal TextBlock" };
            button.SetTemplate(ButtonState.Normal, block.Name);
            TestHelpers.RenderSprite(Device, s => Assert.IsTrue((button as ISprite).Render(s)));
        }

        [TestMethod()]
        public void Button_XmlSerialize_Test()
        {
            var parent = new TestHelpers.RootElement();
            var expected = new Button(parent, AssetManager);
            var block = new TextBlock(expected, AssetManager) { Name = "Normal TextBlock" };
            expected.SetTemplate(ButtonState.Normal, block.Name);

            var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream))
            {
                expected.WriteXml(xmlWriter);
            }

            stream.Position = 0;
            var actual = new Button(parent, AssetManager);
            using (var xmlReader = XmlReader.Create(stream))
            {
                actual.ReadXml(xmlReader);
            }

            Assert.AreEqual(block.Name, actual.GetTemplate(ButtonState.Normal).Name);
            Assert.IsNull(actual.GetTemplate(ButtonState.Hover));
        }

        [TestMethod()]
        public void Button_StateChangesOnMouseEvent_Test()
        {
            var button = new Button(new TestHelpers.RootElement(), AssetManager)
            {
                DestRect = new Rectangle(0.0f, 0.0f, 0.5f, 0.5f)
            };
            
            // We need to check that "template" does not consume our event.
            var block = new TextBlock(button, AssetManager) { Name = "Normal TextBlock" };
            button.SetTemplate(ButtonState.Normal, block.Name);
            
            button.InjectMouseMoveEvent(0.0f, 0.0f);
            Assert.AreEqual(ButtonState.Hover, button.State);

            button.InjectMouseDownEvent(0.0f, 0.0f, MouseButton.Left);
            Assert.AreEqual(ButtonState.Pushed, button.State);

            button.InjectMouseUpEvent(0.0f, 0.0f, MouseButton.Left);
            Assert.AreEqual(ButtonState.Hover, button.State);

            button.InjectMouseDownEvent(0.0f, 0.0f, MouseButton.Right);
            Assert.AreEqual(ButtonState.Hover, button.State);

            button.InjectMouseMoveEvent(button.AbsRect.Left - 1, button.AbsRect.Top - 1);
            Assert.AreEqual(ButtonState.Normal, button.State);
        }
    }
}
