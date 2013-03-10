using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RenderTest
{
    [TestClass()]
    public class RenderSystemTest
    {
        private Device Device { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            SpriteRenderer = new SpriteRenderer(Device);
        }

        [TestMethod()]
        public void RenderSystem_Constructor_Test()
        {
            var rs = new RenderSystem(Device, SpriteRenderer);
            Assert.IsNotNull(rs.PrimaryRenderTarget);
        }

        [TestMethod()]
        public void RenderSystem_ConstructorFail_Test()
        {
            Assert.IsTrue(
                TestHelpers.CatchException(typeof(ArgumentNullException), 
                () => new RenderSystem(null, SpriteRenderer)));
            Assert.IsTrue(
                TestHelpers.CatchException(typeof(ArgumentNullException), 
                () => new RenderSystem(Device, null)));
        }

        [TestMethod()]
        public void RenderSystem_Render_Test()
        {
            var rs = new RenderSystem(Device, SpriteRenderer);

            var renderable = TestHelpers.CreateRenderableMock(Device);
            int numRenderCalled = 0;
            renderable.RenderCalled += ((o, e) => ++numRenderCalled);
            var renderables = new List<IRenderable>() { renderable };

            var sprite = TestHelpers.CreateSpriteMock(Device);
            sprite.RenderCalled += ((o, e) => ++numRenderCalled);
            var sprites = new List<ISprite>() { sprite };

            Assert.IsTrue(rs.Render(rs.PrimaryRenderTarget, renderables, sprites, new Camera(), new Light(), false));
            Assert.AreEqual(2, numRenderCalled);
            Assert.AreEqual(0.0f, rs.PrimaryRenderTarget.LastFps);
        }

        [TestMethod()]
        public void RenderSystem_RenderFail_Test()
        {
            var rs = new RenderSystem(Device, SpriteRenderer);

            Assert.IsTrue(
                TestHelpers.CatchException(typeof(ArgumentNullException),
                () => rs.Render(rs.PrimaryRenderTarget, null, null, null, new Light(), false)));
            Assert.IsTrue(
                TestHelpers.CatchException(typeof(ArgumentNullException),
                () => rs.Render(rs.PrimaryRenderTarget, null, null, new Camera(), null, false)));
        }

        class RenderTargetMock : RenderTarget
        {
            public int NumRenderCalled { get; set; }

            protected override void OnRender()
            {
                ++NumRenderCalled;
            }
        }
    }
}
