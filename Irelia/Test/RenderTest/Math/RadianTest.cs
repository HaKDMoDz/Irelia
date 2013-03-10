using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class RadianTest
    {
        [TestMethod()]
        public void Radian_Constructor_Test()
        {
            var r = new Radian();
            Assert.AreEqual(0.0f, r.Value);

            r = new Radian(3.14f);
            Assert.AreEqual(3.14f, r.Value);

            var r2 = new Radian(r);
            Assert.AreEqual(r.Value, r2.Value);

            var r3 = r;
            Assert.AreEqual(r.Value, r3.Value);
        }

        [TestMethod()]
        public void Radian_Comparision_Test()
        {
            Assert.IsTrue(new Radian(1.0f) < new Radian(2.0f));
            Assert.IsTrue(new Radian(1.0f) <= new Radian(1.0f));
            Assert.IsTrue(new Radian(1.0f) <= new Radian(2.0f));
            Assert.IsTrue(new Radian(1.0f) == new Radian(1.0f));
            Assert.IsTrue(new Radian(1.0f) != new Radian(2.0f));
            Assert.IsTrue(new Radian(1.0f) >= new Radian(1.0f));
            Assert.IsTrue(new Radian(2.0f) >= new Radian(1.0f));
            Assert.IsTrue(new Radian(2.0f) > new Radian(1.0f));
        }

        [TestMethod()]
        public void Radian_Operators_Test()
        {
            Assert.AreEqual(new Radian(1.0f),  +new Radian(1.0f));
            Assert.AreEqual(new Radian(3.0f),  new Radian(1.0f) + new Radian(2.0f));
            Assert.AreEqual(new Radian(-1.0f), -new Radian(1.0f));
            Assert.AreEqual(new Radian(-1.0f), new Radian(1.0f) - new Radian(2.0f));
            Assert.AreEqual(new Radian(3.0f),  new Radian(1.0f) * 3);
            Assert.AreEqual(new Radian(3.0f),  3 * new Radian(1.0f));
            Assert.AreEqual(new Radian(3.0f),  new Radian(1.0f) * new Radian(3.0f));
            Assert.AreEqual(new Radian(1.5f),  new Radian(3.0f) / 2);

            Radian r1 = new Radian(1.0f);
            Assert.AreEqual(new Radian(3.0f), r1 += new Radian(2.0f));
            Assert.AreEqual(new Radian(2.0f), r1 -= new Radian(1.0f));
            Assert.AreEqual(new Radian(4.0f), r1 *= 2);
            Assert.AreEqual(new Radian(2.0f), r1 /= 2);
        }

        [TestMethod()]
        public void Radian_Wrap_Test()
        {
            var radian = new Radian(MathUtils.PI);
            var wrapAngle = new Radian(MathUtils.PI);
            Assert.AreEqual(new Radian(0.0f), radian.Wrap(wrapAngle));

            radian = new Radian(MathUtils.PI / 4);
            Assert.AreEqual(new Radian(MathUtils.PI / 4), radian.Wrap(wrapAngle));

            radian = new Radian(-MathUtils.PI / 4);
            Assert.AreEqual(new Radian(MathUtils.PI * 3 / 4), radian.Wrap(wrapAngle));
        }

        [TestMethod()]
        public void Radian_ToString_Test()
        {
            Assert.AreEqual("Radian(-1.2)", (new Radian(-1.2f)).ToString());
        }

        [TestMethod()]
        public void Radian_AngleBetween_Test()
        {
            Assert.AreEqual(new Radian(MathUtils.PI / 2), Radian.AngleBetween(Vector3.UnitX, Vector3.UnitY));
        }
    }
}
