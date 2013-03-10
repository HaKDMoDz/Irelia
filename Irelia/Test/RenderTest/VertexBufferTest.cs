using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class VertexBufferTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void VertexBuffer_ConstructorTest()
        {
            var vb = new VertexBuffer<TransformedColoredVertex>(Device);
            Assert.IsNull(vb.RawBuffer);
            Assert.AreEqual(20, vb.ElementSize);
            Assert.AreEqual(0, vb.Count);
        }

        [TestMethod()]
        public void VertexBuffer_WriteTest()
        {
            var vb = new VertexBuffer<TransformedColoredVertex>(Device);

            vb.Write(new[] {
                new TransformedColoredVertex() { Position = Vector4.Zero },
                new TransformedColoredVertex() { Position = new Vector4(0, 1, 2, 1) } 
            });
            Assert.IsNotNull(vb.RawBuffer);
            Assert.AreEqual(2, vb.Count);

            vb.Write(new[] { new TransformedColoredVertex() { Position = Vector4.Zero } });
            Assert.AreEqual(3, vb.Count);
        }

        [TestMethod()]
        public void VertexBuffer_OverWrite_Test()
        {
            var vb = new VertexBuffer<TransformedColoredVertex>(Device);
            vb.Write(new[] {
                new TransformedColoredVertex() { Position = Vector4.Zero },
                new TransformedColoredVertex() { Position = new Vector4(0, 1, 2, 1) } 
            });

            vb.OverWrite(new[] { new TransformedColoredVertex() { Position = Vector4.Zero } });
            Assert.AreEqual(1, vb.Count);
        }
    }
}
