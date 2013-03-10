using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class DeviceTest
    {
        private Window Window { get; set; }

        [TestInitialize()]
        public void CreateWindow()
        {
            Window = new Window("DeviceTest", 640, 480);
        }
        
        [TestMethod()]
        public void Device_Constructor_Test()
        {
            Device device = new Device(Window.Handle, Window.Width, Window.Height);
            Assert.IsNotNull(device.RawDevice);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Device_ConstructorFail_Test()
        {
            Device device = new Device(IntPtr.Zero, 0, 0);
        }

        [TestMethod()]
        public void Device_GetVertexDeclarations_Test()
        {
            Device device = new Device(Window.Handle, Window.Width, Window.Height);
            Assert.IsNotNull(device.TransformedColoredVertexDecl);
            Assert.IsNotNull(device.MeshVertexDecl);
            Assert.IsNotNull(device.TextureVertexDecl);
            Assert.IsNotNull(device.ScreenVertexDecl);
        }

        [TestMethod()]
        public void Device_GetVertexBuffers_Test()
        {
            Device device = new Device(Window.Handle, Window.Width, Window.Height);
            Assert.IsNotNull(device.ScreenVertexBuffer);
        }

        [TestMethod()]
        public void Device_RenderTarget_Test()
        {
            var device = new Device(Window.Handle, Window.Width, Window.Height);
            var renderTarget = new TextureRenderTarget(device, new Size(100, 100));

            Assert.AreEqual(true, device.SetRenderTarget(renderTarget));
            Assert.AreEqual(renderTarget, device.RenderTarget);
        }
    }
}
