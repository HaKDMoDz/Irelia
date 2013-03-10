using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class FrameworkTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void Framework_Constructor_Test()
        {
            var window = new Window("FrameworkTest", 640, 480);
            Framework fw = new Framework(window.Handle, window.Width, window.Height, RenderSettings.MediaPath);

            Assert.IsNotNull(fw.Device);
            Assert.IsNotNull(fw.AssetManager);
            Assert.IsNotNull(fw.Renderer);
            Assert.IsNotNull(fw.Light);
        }
    }
}
