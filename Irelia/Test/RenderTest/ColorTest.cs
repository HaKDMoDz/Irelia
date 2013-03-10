using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SysColor = System.Drawing.Color;

namespace RenderTest
{
    [TestClass()]
    public class ColorTest
    {
        [TestMethod()]
        public void Color_Constructor_Test()
        {
            var color = new Color();
            Assert.AreEqual(0.0f, color.a);
            Assert.AreEqual(0.0f, color.r);
            Assert.AreEqual(0.0f, color.g);
            Assert.AreEqual(0.0f, color.b);

            color = new Color(0.2f, 0.4f, 0.6f, 0.8f);
            Assert.AreEqual(0.2f, color.a);
            Assert.AreEqual(0.4f, color.r);
            Assert.AreEqual(0.6f, color.g);
            Assert.AreEqual(0.8f, color.b);

            color = new Color(0, 32, 64, 128);
            Assert.AreEqual(0.0f, color.a);
            Assert.AreEqual(32 / 255.0f, color.r);
            Assert.AreEqual(64 / 255.0f, color.g);
            Assert.AreEqual(128 / 255.0f, color.b);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Color_CreateWith_InvalidValue_Test()
        {
            var color = new Color(1.2f, 0.0f, 0.0f, 0.0f);
        }

        [TestMethod()]
        public void Color_Equality_Test()
        {
            var a = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            var b = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b);

            var c = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            Assert.AreNotEqual(a, c);
            Assert.IsFalse(a == c);
        }

        [TestMethod()]
        public void Color_ToString_Test()
        {
            var color = new Color(0.0f, 0.1f, 0.22f, 0.333f);
            Assert.AreEqual("Color(0, 0.1, 0.22, 0.333)", color.ToString());
        }

        [TestMethod()]
        public void Color_ToARGB_Predefined_Colors_Test()
        {
            Assert.AreEqual(SysColor.Black.ToArgb(), Color.Black.ToArgb());
            Assert.AreEqual(SysColor.White.ToArgb(), Color.White.ToArgb());
            Assert.AreEqual(SysColor.Red.ToArgb(), Color.Red.ToArgb());
            Assert.AreEqual(SysColor.Green.ToArgb(), Color.Green.ToArgb());
            Assert.AreEqual(SysColor.Blue.ToArgb(), Color.Blue.ToArgb());
            Assert.AreEqual(SysColor.Transparent.ToArgb(), Color.Transparent.ToArgb());
            Assert.AreEqual(SysColor.Gray.ToArgb(), Color.Gray.ToArgb());
        }

        [TestMethod()]
        public void Color_IntProperties_Test()
        {
            var color = new Color(0.2f, 0.3f, 0.4f, 0.5f);
            Assert.AreEqual((int)(color.a * 255.0f), color.IntA);
            Assert.AreEqual((int)(color.r * 255.0f), color.IntR);
            Assert.AreEqual((int)(color.g * 255.0f), color.IntG);
            Assert.AreEqual((int)(color.b * 255.0f), color.IntB);
        }

        [TestMethod()]
        public void Color_Parse_Test()
        {
            var expected = new Color(0.0f, 0.1f, 0.22f, 0.333f);
            var actual = Color.Parse(expected.ToString());
            Assert.AreEqual(expected, actual);
        }
    }
}
