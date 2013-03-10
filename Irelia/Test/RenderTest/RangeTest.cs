using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class RangeTest
    {
        [TestMethod()]
        public void Range_Int_Test()
        {
            var range = new Range<int>(-5, 10);
            Assert.AreEqual(-5, range.Min);
            Assert.AreEqual(10, range.Max);
        }

        [TestMethod()]
        public void Range_Struct_Test()
        {
            var min = new Vector2(-1.0f, -2.0f);
            var max = Vector2.Zero;
            var range = new Range<Vector2>(min, max);
            Assert.AreEqual(min, range.Min);
            Assert.AreEqual(max, range.Max);
        }

        [TestMethod()]
        public void Range_Equality_Test()
        {
            var actual = new Range<Color>(Color.Black, Color.White);
            var expected = new Range<Color>(Color.Black, Color.White);
            Assert.AreEqual(expected, actual);
        }
    }
}
