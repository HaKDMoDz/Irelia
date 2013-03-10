using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class Vector2Test
    {
        [TestMethod()]
        public void Vector2_ConstructTest()
        {
            Vector2 a = new Vector2();
            Assert.AreEqual(0.0f, a.x);
            Assert.AreEqual(0.0f, a.y);

            Vector2 b = new Vector2(1.0f, 1.0f);
            Assert.AreEqual(1.0f, b.x);
            Assert.AreEqual(1.0f, b.y);
        }

        [TestMethod()]
        public void Vector2_EquialityTest()
        {
            Vector2 a = new Vector2(1.0f, 1.0f);
            Vector2 b = new Vector2(1.0f, 1.0f);
            Assert.AreEqual(a, b);

            Vector2 c = new Vector2(0.0f, 0.0f);
            Assert.AreNotEqual(a, c);
        }

        [TestMethod()]
        public void Vector2_UnaryTest()
        {
            Vector2 a = new Vector2(1.0f, 2.0f);
            Assert.AreEqual(-1.0f, (-a).x);
            Assert.AreEqual(-2.0f, (-a).y);
        }

        [TestMethod()]
        public void Vector2_AdditionTest()
        {
            Vector2 a = new Vector2(1.0f, 1.0f);
            Vector2 b = new Vector2(1.0f, 2.0f);

            // operator +
            Vector2 c = a + b;
            Assert.AreEqual(2.0f, c.x);
            Assert.AreEqual(3.0f, c.y);

            Vector2 c2 = a + b + c;
            Assert.AreEqual(4.0f, c2.x);
            Assert.AreEqual(6.0f, c2.y);

            // operator +=
            Vector2 d = new Vector2(1.0f, 3.0f);
            d += a;
            Assert.AreEqual(2.0f, d.x);
            Assert.AreEqual(4.0f, d.y);

            d = (c += b);
            Assert.AreEqual(c, d);
        }

        [TestMethod()]
        public void Vector2_SubtractionTest()
        {
            Vector2 a = new Vector2(1.0f, 1.0f);
            Vector2 b = new Vector2(1.0f, 2.0f);

            // operator -
            Vector2 c = a - b;
            Assert.AreEqual(0.0f, c.x);
            Assert.AreEqual(-1.0f, c.y);

            Vector2 c2 = a - b - c;
            Assert.AreEqual(0.0f, c2.x);
            Assert.AreEqual(0.0f, c2.y);

            // operator -=
            Vector2 d = new Vector2(1.0f, 3.0f);
            d -= a;
            Assert.AreEqual(0.0f, d.x);
            Assert.AreEqual(2.0f, d.y);

            d = (c -= b);
            Assert.AreEqual(c, d);
        }

        [TestMethod()]
        public void Vector2_MultiplicationByScalarTest()
        {
            Vector2 a = new Vector2(1.0f, 2.0f);

            // operator *
            Vector2 b = a * 2.0f;
            Assert.AreEqual(2.0f, b.x);
            Assert.AreEqual(4.0f, b.y);

            b = 2.0f * a;
            Assert.AreEqual(2.0f, b.x);
            Assert.AreEqual(4.0f, b.y);

            Vector2 b2 = a * 2.0f * 3.0f;
            Assert.AreEqual(6.0f, b2.x);
            Assert.AreEqual(12.0f, b2.y);

            // operator *=
            Vector2 c = new Vector2(1.0f, 3.0f);
            c *= 2;
            Assert.AreEqual(2.0f, c.x);
            Assert.AreEqual(6.0f, c.y);

            c = (b *= 2);
            Assert.AreEqual(b, c);
        }

        [TestMethod()]
        public void Vector2_DivisionTest()
        {
            Vector2 a = new Vector2(1.0f, 2.0f);

            // operator /
            Vector2 b = a / 2.0f;
            Assert.AreEqual(0.5f, b.x);
            Assert.AreEqual(1.0f, b.y);

            Vector2 b2 = a / 2.0f / 0.5f;
            Assert.AreEqual(1.0f, b2.x);
            Assert.AreEqual(2.0f, b2.y);

            // operator /=
            Vector2 c = new Vector2(1.0f, 3.0f);
            c /= 2.0f;
            Assert.AreEqual(0.5f, c.x);
            Assert.AreEqual(1.5f, c.y);

            // TODO: Division by zero
        }

        [TestMethod()]
        public void Vector2_MagnitudeTest()
        {
            Vector2 a = new Vector2(3.0f, 4.0f);
            Assert.AreEqual(5.0f, a.Length);
            Assert.AreEqual(25.0f, a.SquaredLength);

            Vector2 b = new Vector2(0.0f, 0.0f);
            Assert.AreEqual(0.0f, b.Length);
            Assert.AreEqual(0.0f, b.SquaredLength);
        }

        [TestMethod()]
        public void Vector2_NormalizationTest()
        {
            Vector2 a = new Vector2(3.0f, 4.0f);
            a.Normalize();
            Assert.AreEqual(1.0f, a.Length);

            Vector2 b = new Vector2(0.0f, 0.0f);
            b.Normalize();
            Assert.AreEqual(0.0f, b.Length);
        }

        [TestMethod()]
        public void Vector2_DotProductTest()
        {
            Vector2 a = new Vector2(1.0f, 2.0f);
            Vector2 b = new Vector2(3.0f, 4.0f);

            Assert.AreEqual(11.0f, Vector2.Dot(a, b));

            // Commutative
            Assert.AreEqual(Vector2.Dot(a, b), Vector2.Dot(b, a));

            // Distributive over addition
            Vector2 c = new Vector2(-1.0f, 1.0f);
            Assert.AreEqual(Vector2.Dot(c, a + b),
                      Vector2.Dot(c, a) + Vector2.Dot(c, b));

            // Combines with scalar multiplication
            float s = 2.0f;
            Assert.AreEqual(Vector2.Dot(s * a, b), Vector2.Dot(a, s * b));
            Assert.AreEqual(Vector2.Dot(s * a, b), s * Vector2.Dot(a, b));
        }

        [TestMethod()]
        public void Vector2_ToStringTest()
        {
            Vector2 v = new Vector2(1.0f, 2.2f);
            Assert.AreEqual("Vector2(1, 2.2)", v.ToString());
        }

        [TestMethod()]
        public void Vector2_SpecialVectors_Test()
        {
            Assert.AreEqual(new Vector2(0.0f, 0.0f), Vector2.Zero);
            Assert.AreEqual(new Vector2(0.0f, 1.0f), Vector2.UnitY);
            Assert.AreEqual(new Vector2(1.0f, 0.0f), Vector2.UnitX);
        }

        [TestMethod()]
        public void Vector2_CrossProduct_Test()
        {
            Vector2 a = new Vector2(1.0f, 2.0f);
            Vector2 b = new Vector2(3.0f, 4.0f);

            Assert.AreEqual(-2, Vector2.Cross(a, b));
        }

        [TestMethod()]
        public void Vector2_Parse_Test()
        {
            var expected = new Vector2(1.0f, 2.2f);
            var actual = Vector2.Parse(expected.ToString());
            Assert.AreEqual(expected, actual);
        }
    }
}
