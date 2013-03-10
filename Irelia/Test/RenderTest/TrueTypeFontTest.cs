using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class TrueTypeFontTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void TrueTypeFont_DrawString_Test()
        {
            var font = new TrueTypeFont(Device, 10);
            Assert.IsTrue(font.DrawString(0, 0, "String to display", Color.White.ToArgb()));
        }
    }
}
