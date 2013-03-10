using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using D3D = SlimDX.Direct3D9;

namespace RenderTest
{
    [TestClass()]
    public class LayerTest
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
        public void Layer_Constructor_Test()
        {
            var bufferSize = new Size(300, 300);
            var windowSize = new Size(100, 100);
            var layer = new Layer(Device, bufferSize, windowSize);

            Assert.AreEqual(bufferSize, layer.BufferSize);
            Assert.AreEqual(windowSize, layer.WindowSize);
            Assert.IsNotNull(layer.RenderTarget);
            Assert.AreEqual(0.0f, layer.ScrollX);
            Assert.AreEqual(0.0f, layer.ScrollY);
            Assert.AreEqual(new Range<float>(0.0f, 199.0f), layer.ScrollRangeX);
            Assert.AreEqual(new Range<float>(0.0f, 199.0f), layer.ScrollRangeY);
        }

        [TestMethod()]
        public void Layer_SetScrollXY_Test()
        {
            var bufferSize = new Size(300, 300);
            var windowSize = new Size(100, 100);
            var layer = new Layer(Device, bufferSize, windowSize);

            layer.ScrollX = -10.0f;
            Assert.AreEqual(0, layer.ScrollX);
            layer.ScrollY = -10.0f;
            Assert.AreEqual(0.0f, layer.ScrollY);

            layer.ScrollX = layer.ScrollRangeX.Max + 1;
            Assert.AreEqual(layer.ScrollRangeX.Max, layer.ScrollX);
            layer.ScrollY = layer.ScrollRangeY.Max + 1;
            Assert.AreEqual(layer.ScrollRangeY.Max, layer.ScrollY);
        }

        [TestMethod()]
        public void Layer_Scroll_Test()
        {
            var bufferSize = new Size(300, 300);
            var windowSize = new Size(100, 100);
            var layer = new Layer(Device, bufferSize, windowSize);

            layer.Scroll(100.0f, 150.0f);
            Assert.AreEqual(100.0f, layer.ScrollX);
            Assert.AreEqual(150.0f, layer.ScrollY);
        }
    }
}
