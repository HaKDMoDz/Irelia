using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class Vector3Test
    {
        [TestMethod()]
        public void Vector3_ConstructorTest()
        {
            Vector3 a = new Vector3();
            Assert.AreEqual(0.0f, a.x);
            Assert.AreEqual(0.0f, a.y);

            Vector3 b = new Vector3(1.0f, 2.0f, 3.0f);
            Assert.AreEqual(1.0f, b.x);
            Assert.AreEqual(2.0f, b.y);
            Assert.AreEqual(3.0f, b.z);
        }

        [TestMethod()]
        public void Vector3_EqualityTest()
        {
            Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);
            Vector3 b = new Vector3(1.0f, 2.0f, 3.0f);
            Assert.AreEqual(a, b);

            Vector3 c = new Vector3(1.0f, 2.0f, 0.0f);
            Assert.AreNotEqual(a, c);
        }

        [TestMethod()]
        public void Vector3_UnaryTest()
        {
            Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);
            Assert.AreEqual(-1.0f, (-a).x);
            Assert.AreEqual(-2.0f, (-a).y);
            Assert.AreEqual(-3.0f, (-a).z);
        }

        [TestMethod()]
        public void Vector3_AdditionTest()
        {
            Vector3 a = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 b = new Vector3(1.0f, 2.0f, 3.0f);

            // operator +
            Vector3 c = a + b;
            Assert.AreEqual(2.0f, c.x);
            Assert.AreEqual(3.0f, c.y);
            Assert.AreEqual(4.0f, c.z);

            Vector3 c2 = a + b + c;
            Assert.AreEqual(4.0f, c2.x);
            Assert.AreEqual(6.0f, c2.y);
            Assert.AreEqual(8.0f, c2.z);

            // operator +=
            Vector3 d = new Vector3(1.0f, 3.0f, 5.0f);
            d += a;
            Assert.AreEqual(2.0f, d.x);
            Assert.AreEqual(4.0f, d.y);
            Assert.AreEqual(6.0f, d.z);

            d = (c += b);
            Assert.AreEqual(c, d);
        }

        [TestMethod()]
        public void Vector3_SubtractionTest()
        {
            Vector3 a = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 b = new Vector3(1.0f, 2.0f, 3.0f);

            // operator -
            Vector3 c = a - b;
            Assert.AreEqual(0.0f, c.x);
            Assert.AreEqual(-1.0f, c.y);
            Assert.AreEqual(-2.0f, c.z);

            Vector3 c2 = a - b - c;
            Assert.AreEqual(0.0f, c2.x);
            Assert.AreEqual(0.0f, c2.y);
            Assert.AreEqual(0.0f, c2.z);

            // operator -=
            Vector3 d = new Vector3(1.0f, 3.0f, 5.0f);
            d -= a;
            Assert.AreEqual(0.0f, d.x);
            Assert.AreEqual(2.0f, d.y);
            Assert.AreEqual(4.0f, d.z);

            d = (c -= b);
            Assert.AreEqual(c, d);
        }

        [TestMethod()]
        public void Vector3_MultiplicationByScalarTest()
        {
            Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);

            // operator *
            Vector3 b = a * 2.0f;
            Assert.AreEqual(2.0f, b.x);
            Assert.AreEqual(4.0f, b.y);
            Assert.AreEqual(6.0f, b.z);

            b = 2.0f * a;
            Assert.AreEqual(2.0f, b.x);
            Assert.AreEqual(4.0f, b.y);
            Assert.AreEqual(6.0f, b.z);

            Vector3 b2 = a * 2.0f * 3.0f;
            Assert.AreEqual(6.0f, b2.x);
            Assert.AreEqual(12.0f, b2.y);
            Assert.AreEqual(18.0f, b2.z);

            // operator *=
            Vector3 c = new Vector3(1.0f, 3.0f, 5.0f);
            c *= 2;
            Assert.AreEqual(2.0f, c.x);
            Assert.AreEqual(6.0f, c.y);
            Assert.AreEqual(10.0f, c.z);

            c = (b *= 2);
            Assert.AreEqual(b, c);
        }

        [TestMethod()]
        public void Vector3_DivisionTest()
        {
            Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);

            // operator /
            Vector3 b = a / 2.0f;
            Assert.AreEqual(0.5f, b.x);
            Assert.AreEqual(1.0f, b.y);
            Assert.AreEqual(1.5f, b.z);

            Vector3 b2 = a / 2.0f / 0.5f;
            Assert.AreEqual(1.0f, b2.x);
            Assert.AreEqual(2.0f, b2.y);
            Assert.AreEqual(3.0f, b2.z);

            // operator /=
            Vector3 c = new Vector3(1.0f, 3.0f, 5.0f);
            c /= 2.0f;
            Assert.AreEqual(0.5f, c.x);
            Assert.AreEqual(1.5f, c.y);
            Assert.AreEqual(2.5f, c.z);

            // TODO: Division by zero
        }

        [TestMethod()]
        public void Vector3_MagnitudeTest()
        {
            Vector3 a = new Vector3(3.0f, 4.0f, 5.0f);

            float len = (float)(System.Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z));
            Assert.AreEqual(len, a.Length);
            Assert.AreEqual(len * len, a.SquaredLength);

            Vector3 b = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(0.0f, b.Length);
            Assert.AreEqual(0.0f, b.SquaredLength);
        }

        [TestMethod()]
        public void Vector3_Normalize_Test()
        {
            Vector3 a = new Vector3(3.0f, 4.0f, 5.0f);
            Assert.AreEqual(1.0f, a.Normalize().Length);

            Vector3 b = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(Vector3.Zero, b.Normalize());
        }

        [TestMethod()]
        public void Vector3_DotProductTest()
        {
            Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);
            Vector3 b = new Vector3(3.0f, 4.0f, 5.0f);

            Assert.AreEqual(26.0f, Vector3.Dot(a, b));

            // Commutative
            Assert.AreEqual(Vector3.Dot(a, b), Vector3.Dot(b, a));

            // Distributive over addition
            Vector3 c = new Vector3(-1.0f, 1.0f, 3.0f);
            Assert.AreEqual(Vector3.Dot(c, a + b),
                      Vector3.Dot(c, a) + Vector3.Dot(c, b));

            // Combines with scalar multiplication
            float s = 2.0f;
            Assert.AreEqual(Vector3.Dot(s * a, b), Vector3.Dot(a, s * b));
            Assert.AreEqual(Vector3.Dot(s * a, b), s * Vector3.Dot(a, b));
        }

        [TestMethod()]
        public void Vector3_CrossProductTest()
        {
            Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);
            Vector3 b = new Vector3(3.0f, 4.0f, 5.0f);

            Vector3 c = Vector3.Cross(a, b);
            Assert.AreEqual(a.y * b.z - a.z * b.y, c.x);
            Assert.AreEqual(a.z * b.x - a.x * b.z, c.y);
            Assert.AreEqual(a.x * b.y - a.y * b.x, c.z);

            // Anticommutative: a x b = -b x a
            Assert.AreEqual(Vector3.Cross(-b, a), Vector3.Cross(a, b));

            // Distributive over addition: a x (b + c) = (a x b) + (a x c)
            Assert.AreEqual(Vector3.Cross(a, b) + Vector3.Cross(a, c), Vector3.Cross(a, b + c));

            // Scalar multiplication: (ra) x b = a x (rb) = r(a x b)
            float mult = 1.5f;
            Assert.AreEqual(Vector3.Cross(a, mult * b), Vector3.Cross(mult * a, b));
            Assert.AreEqual(mult * Vector3.Cross(a, b), Vector3.Cross(mult * a, b));

            // Axis...
            Assert.AreEqual(Vector3.UnitX, Vector3.Cross(Vector3.UnitY, Vector3.UnitZ));
            Assert.AreEqual(Vector3.UnitY, Vector3.Cross(Vector3.UnitZ, Vector3.UnitX));
            Assert.AreEqual(Vector3.UnitZ, Vector3.Cross(Vector3.UnitX, Vector3.UnitY));
        }

        [TestMethod()]
        public void Vector3_ToStringTest()
        {
            Vector3 v = new Vector3(1.0f, 2.2f, 3.33f);
            Assert.AreEqual("Vector3(1, 2.2, 3.33)", v.ToString());
        }

        [TestMethod()]
        public void Vector3_SpecialVectorsTest()
        {
            Vector3 zero = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(zero, Vector3.Zero);

            Vector3 unitx = new Vector3(1.0f, 0.0f, 0.0f);
            Assert.AreEqual(unitx, Vector3.UnitX);

            Vector3 unity = new Vector3(0.0f, 1.0f, 0.0f);
            Assert.AreEqual(unity, Vector3.UnitY);

            Vector3 unitz = new Vector3(0.0f, 0.0f, 1.0f);
            Assert.AreEqual(unitz, Vector3.UnitZ);
        }

        [TestMethod()]
        public void Vector3_IsZeroLength_Test()
        {
            Assert.IsTrue(Vector3.Zero.IsZeroLength);
            Assert.IsFalse(Vector3.UnitX.IsZeroLength);
        }
    }
}
