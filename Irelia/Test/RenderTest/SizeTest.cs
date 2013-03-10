using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class SizeTest
    {
        [TestMethod()]
        public void Size_Constructor_Test()
        {
            var size = new Size(100, 200);
            Assert.AreEqual(100, size.Width);
            Assert.AreEqual(200, size.Height);
        }

        [TestMethod()]
        public void Size_Equality_Test()
        {
            var actual = new Size(100, 200);
            Assert.AreEqual(new Size(100, 200), actual);

            Assert.AreNotEqual(new Size(0, 100), actual);
        }

        [TestMethod()]
        public void Size_ToString_Test()
        {
            var size = new Size(1.1f, 2.22f);
            Assert.AreEqual("Size(Width=1.1, Height=2.22)", size.ToString());
        }
    }
}
