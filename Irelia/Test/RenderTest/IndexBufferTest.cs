using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class IndexBufferTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void IndexBuffer_Constructor_Test()
        {
            var ib = new IndexBuffer(Device);
        }

        [TestMethod()]
        public void IndexBuffer_Write_Test()
        {
            var ib = new IndexBuffer(Device);

            Assert.IsTrue(ib.Write(new short[] { 1, 2, 3 }));
            Assert.IsNotNull(ib.RawBuffer);
            Assert.AreEqual(3, ib.Count);

            Assert.IsTrue(ib.Write(new short[] { 2, 1 }));
            Assert.AreEqual(5, ib.Count);
        }
    }
}
