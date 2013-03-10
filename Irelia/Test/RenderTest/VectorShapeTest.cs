using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class VectorShapeTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void VectorShape_AddLine_Test()
        {
            var shape = new VectorShape(Device);

            shape.AddLine(Vector2.Zero, new Vector2(1.0f, 2.0f), 3.0f, Color.White);

            Device.RawDevice.BeginScene();
            Assert.IsTrue((shape as ISprite).Render(null));
            Device.RawDevice.EndScene();
        }

        [TestMethod()]
        public void VectorShape_AddPoint_Test()
        {
            var shape = new VectorShape(Device);

            shape.AddPoint(Vector2.Zero, 5, Color.White);

            Device.RawDevice.BeginScene();
            Assert.IsTrue((shape as ISprite).Render(null));
            Device.RawDevice.EndScene();
        }

        [TestMethod()]
        public void VectorShape_AddRectangle_Test()
        {
            var shape = new VectorShape(Device);

            shape.AddRectangle(Vector2.Zero, new Vector2(3.0f, 3.0f), 5, Color.White);
            shape.AddFilledRectangle(Vector2.Zero, new Vector2(3.0f, 3.0f), Color.White);

            Device.RawDevice.BeginScene();
            Assert.IsTrue((shape as ISprite).Render(null));
            Device.RawDevice.EndScene();
        }
    }
}
