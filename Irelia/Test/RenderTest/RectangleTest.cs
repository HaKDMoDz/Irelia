using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class RectangleTest
    {
        [TestMethod()]
        public void Rectangle_Constructor_Test()
        {
            var rect = new Rectangle(0, 1, 2, 3);
            Assert.AreEqual(0, rect.Left);
            Assert.AreEqual(1, rect.Top);
            Assert.AreEqual(2, rect.Width);
            Assert.AreEqual(3, rect.Height);

            Assert.AreEqual(new Vector2(0, 1), rect.Location);
            Assert.AreEqual(new Size(2, 3), rect.Size);
            Assert.AreEqual(rect.Left + rect.Width, rect.Right);
            Assert.AreEqual(rect.Top + rect.Height, rect.Bottom);
        }

        [TestMethod()]
        public void Rectangle_ToString_Test()
        {
            var rect = new Rectangle(1.0f, 2.2f, 3.33f, 4.444f);
            Assert.AreEqual("Rectangle(1, 2.2, 3.33, 4.444)", rect.ToString());
        }

        [TestMethod()]
        public void Rectangle_Parse_Test()
        {
            var expected = new Rectangle(1.0f, 2.2f, 3.33f, 4.444f);
            var actual = Rectangle.Parse(expected.ToString());
            Assert.AreEqual(expected, actual);
        }
    }
}
