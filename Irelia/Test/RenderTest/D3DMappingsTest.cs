using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SlimDX;

namespace RenderTest
{
    [TestClass()]
    public class D3DMappingsTest
    {
        [TestMethod()]
        public void ToD3DMatrix_Test()
        {
            Assert.AreEqual(Matrix.Identity, Matrix4.Identity.ToD3DMatrix());
            Assert.AreEqual(Matrix.Identity, Matrix3.Identity.ToD3DMatrix());
        }

        [TestMethod()]
        public void ToMatrix4_Test()
        {
            Assert.AreEqual(Matrix4.Identity, Matrix.Identity.ToMatrix4());
        }

        [TestMethod()]
        public void ToD3DVector2_Test()
        {
            var actual = new Irelia.Render.Vector2(0.1f, 0.22f);
            var expected = new SlimDX.Vector2(actual.x, actual.y);
            Assert.AreEqual(expected, actual.ToD3DVector2());
        }

        [TestMethod()]
        public void D3DMapping_Vector3_Test()
        {
            var actual = new Irelia.Render.Vector3(0.1f, 0.22f, 0.33f);
            var expected = new SlimDX.Vector3(actual.x, actual.y, actual.z);
            Assert.AreEqual(expected, actual.ToD3DVector3());
            Assert.AreEqual(actual, expected.ToVector3());
        }

        [TestMethod()]
        public void ToD3DVector4_Test()
        {
            var actual = new Irelia.Render.Vector4(0.1f, 0.22f, 0.33f, 0.444f);
            var expected = new SlimDX.Vector4(actual.x, actual.y, actual.z, actual.w);
            Assert.AreEqual(expected, actual.ToD3DVector4());
        }

        [TestMethod()]
        public void ToD3DColor4_Test()
        {
            var actual = new Color(0.1f, 0.22f, 0.333f, 0.4444f);
            var expected = new SlimDX.Color4(actual.a, actual.r, actual.g, actual.b);
            Assert.AreEqual(expected, actual.ToD3DColor4());
        }

        [TestMethod()]
        public void FromColor_ToD3DVector4_Test()
        {
            var actual = new Color(0.1f, 0.22f, 0.333f, 0.4444f);
            var expected = new SlimDX.Vector4(actual.r, actual.g, actual.b, actual.a);
            Assert.AreEqual(expected, actual.ToD3DVector4());
        }

        [TestMethod()]
        public void FromD3DColor4_ToIreliaColor_Test()
        {
            var actual = new SlimDX.Color4(0.1f, 0.22f, 0.333f, 0.4444f);
            var expected = new Color(actual.Alpha, actual.Red, actual.Green, actual.Blue);
            Assert.AreEqual(expected, actual.ToIreliaColor());
        }
    }
}
