using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class MathUtilsTest
    {
        [TestMethod()]
        public void MathUtils_IsNearZero_Test()
        {
            Assert.IsTrue(MathUtils.IsNearZero(0.0f));
            Assert.IsFalse(MathUtils.IsNearZero(0.1f));
        }

        [TestMethod()]
        public void MathUtils_Sqrt_Test()
        {
            Assert.AreEqual(2, MathUtils.Sqrt(4));
        }

        [TestMethod()]
        public void MathUtils_Abs_Test()
        {
            Assert.AreEqual(4, MathUtils.Abs(-4));
        }

        [TestMethod()]
        public void MathUtils_Clamp_Test()
        {
            Assert.AreEqual(0.0f, MathUtils.Clamp(-1.0f, 0.0f, 2.0f));
            Assert.AreEqual(2.0f, MathUtils.Clamp(3.0f, 0.0f, 2.0f));
        }
    }
}
