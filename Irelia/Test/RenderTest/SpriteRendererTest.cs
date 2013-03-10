using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class SpriteRendererTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void SpriteRendererConstructor_Test()
        {
            var renderer = new SpriteRenderer(Device);
            Assert.IsNotNull(renderer.RawSprite);
        }
    }
}
