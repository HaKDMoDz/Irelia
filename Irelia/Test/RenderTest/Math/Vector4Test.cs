using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class Vector4Test
    {
        [TestMethod()]
        public void Vector4_ConstructorTest()
        {
            Vector4 a = new Vector4();
            Assert.AreEqual(0.0f, a.x);
            Assert.AreEqual(0.0f, a.y);
            Assert.AreEqual(0.0f, a.z);
            Assert.AreEqual(0.0f, a.w);

            Vector4 b = new Vector4(1.0f, 2.0f, 3.0f, 4.0f);
            Assert.AreEqual(1.0f, b.x);
            Assert.AreEqual(2.0f, b.y);
            Assert.AreEqual(3.0f, b.z);
            Assert.AreEqual(4.0f, b.w);
        }

        [TestMethod()]
        public void Vector4_ConstructFromVector3Test()
        {
            Vector3 v3 = new Vector3(1.0f, 2.0f, 3.0f);
            Vector4 v4 = new Vector4(v3, 0.8f);
            Vector4 expected = new Vector4(v3.x, v3.y, v3.z, 0.8f);
            Assert.AreEqual(expected, v4);
        }

        [TestMethod()]
        public void Vector4_EqualityTest()
        {
            Vector4 a = new Vector4(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4 b = new Vector4(1.0f, 2.0f, 3.0f, 4.0f);
            Assert.AreEqual(b, a);

            Vector4 c = new Vector4(1.0f, 2.0f, 0.0f, -1.0f);
            Assert.AreNotEqual(c, a);
        }

        [TestMethod()]
        public void Vector4_AdditionTest()
        {
            Vector4 a = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            Vector4 b = new Vector4(1.0f, 2.0f, 3.0f, 4.0f);

            // operator +
            Vector4 c = a + b;
            Assert.AreEqual(2.0f, c.x);
            Assert.AreEqual(3.0f, c.y);
            Assert.AreEqual(4.0f, c.z);
            Assert.AreEqual(5.0f, c.w);
        }

        [TestMethod()]
        public void Vector4_DotProductTest()
        {
            Vector4 a = new Vector4(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4 b = new Vector4(5.0f, 6.0f, 7.0f, 8.0f);

            Assert.AreEqual(70.0f, Vector4.Dot(a, b));

            // Commutative
            Assert.AreEqual(Vector4.Dot(a, b), Vector4.Dot(b, a));

            // Distributive over addition
            Vector4 c = new Vector4(-1.0f, 1.0f, 3.0f, 5.0f);
            Assert.AreEqual(Vector4.Dot(c, a + b),
                      Vector4.Dot(c, a) + Vector4.Dot(c, b));

            // Combines with scalar multiplication
            const float s = 2.0f;
            Assert.AreEqual(Vector4.Dot(s * a, b), Vector4.Dot(a, s * b));
            Assert.AreEqual(Vector4.Dot(s * a, b), s * Vector4.Dot(a, b));
        }

        [TestMethod()]
        public void Vector4_ToStringTest()
        {
            Vector4 v = new Vector4(1.0f, 2.2f, 3.33f, 4.444f);
            Assert.AreEqual("Vector4(1, 2.2, 3.33, 4.444)", v.ToString());
        }

        [TestMethod()]
        public void Vector4_SpecialVectorsTest()
        {
            Vector4 zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            Assert.AreEqual(zero, Vector4.Zero);
        }
    }
}
