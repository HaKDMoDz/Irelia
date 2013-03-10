using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Irelia.Gui;
using System.Collections.ObjectModel;

namespace GuiTest
{
    [TestClass()]
    public class LayoutTest
    {
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            AssetManager = new AssetManager(TestHelpers.GetDevice(), RenderSettings.MediaPath);
        }
        
        [TestMethod()]
        public void Layout_DefaultVales_Test()
        {
            var layout = new Layout("Layout0", AssetManager);
            Assert.AreEqual("Layout0", layout.Name);
        }

        [TestMethod()]
        public void Layout_SaveLoad_Test()
        {
            string name = "Layout0";
            var texture = AssetManager.Load(@"Test\Default_color.dds") as Texture;

            var layout1 = new Layout(name, AssetManager);
            new Image(layout1, AssetManager) 
                { 
                    Texture = texture, 
                    DestRect = new Rectangle(0.0f, 0.0f, 0.5f, 0.5f),
                    SourceRect = new Rectangle(0.0f, 0.0f, texture.Width / 2.0f, texture.Height / 2.0f) 
                };
            new Image(layout1, AssetManager) 
                { 
                    Texture = texture,
                    DestRect = new Rectangle(0.5f, 0.5f, 0.5f, 0.5f),
                    SourceRect = new Rectangle(0.0f, 0.0f, texture.Width, texture.Height) 
                };
            layout1.Save("Layout.layout");

            var layout2 = Layout.Load("Layout.layout", name, AssetManager);
            Assert.AreEqual(name, layout2.Name);
            Assert.AreEqual(2, layout2.Childs.Count);
        }

        [TestMethod()]
        public void LayoutFactory_Test()
        {
            AssetManager.RegisterAssetFactory(new LayoutFactory());
            Assert.AreEqual(typeof(Layout), AssetManager.GetAssetType("sample.layout"));

            var layout = AssetManager.Load(@"Test\Layout.layout") as Layout;
            Assert.AreEqual(@"Test\Layout.layout", layout.Name);
        }
    }
}
