using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class ManualRenderableTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        private class ManualRenderableMock : ManualRenderable<TransformedColoredVertex>
        {
            private readonly Func<bool> renderDelegate;

            public ManualRenderableMock(Device device, Func<bool> renderDelegate)
                : base (device, device.TransformedColoredVertexDecl)
            {
                this.renderDelegate = renderDelegate;
            }

            protected override bool Render(Camera camera, Irelia.Render.Light light)
            {
                return this.renderDelegate.Invoke();
            }
        };

        [TestMethod()]
        public void ManualRenderable_Render_Test()
        {
            var manual = new ManualRenderableMock(Device, () => true);

            var vertices = new[] {
                new TransformedColoredVertex() { Color = Color.Red.ToArgb(), Position = new Vector4(400.0f, 100.0f, 0.5f, 1.0f) },
                new TransformedColoredVertex() { Color = Color.Green.ToArgb(), Position = new Vector4(650.0f, 500.0f, 0.5f, 1.0f) },
                new TransformedColoredVertex() { Color = Color.Blue.ToArgb(), Position = new Vector4(150.0f, 500.0f, 0.5f, 1.0f) }};
            manual.AddVertex(vertices);

            var indices = new short[] { 0 };
            manual.AddIndex(indices);

            Device.RawDevice.BeginScene();
            Assert.IsTrue((manual as IRenderable).Render(new Camera(), new Light()));
            Device.RawDevice.EndScene();
        }

        [TestMethod()]
        public void ManualRenderable_RenderFail_Test()
        {
            var manual = new ManualRenderableMock(Device, () => false);

            Device.RawDevice.BeginScene();
            Assert.IsFalse((manual as IRenderable).Render(new Camera(), new Light()));
            Device.RawDevice.EndScene();
        }
    }
}
