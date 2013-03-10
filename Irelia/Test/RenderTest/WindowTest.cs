using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class WindowTest
    {
        [TestMethod()]
        public void Window_ConstructorTest()
        {
            string title = "Window Test";
            int width = 800, height = 600;

            Window window = new Window(title, width, height);
            Assert.AreEqual(true, window.Created);
            Assert.AreEqual(title, window.Title);
            Assert.AreEqual(width, window.Width);
            Assert.AreEqual(height, window.Height);

            Assert.AreNotEqual(null, window.Handle);
            Assert.AreNotEqual(null, window.Form);
        }

        [TestMethod()]
        public void Window_CreationFailTest()
        {
            Window window = new Window("Invalid width", -100, 300);
            Assert.AreEqual(false, window.Created);

            window = new Window("Invalid height", 100, -300);
            Assert.AreEqual(false, window.Created);
        }
    }
}
