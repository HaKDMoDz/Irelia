using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using D3D = SlimDX.Direct3D9;

namespace RenderTest
{
    [TestClass()]
    public class ParticleEmitterTest
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
        public void ParticleEmitter_Constructor_Test()
        {
            var texture = new Texture(Device, TestHelpers.GetAssetFullPath("particle16.tga"), "");
            var emitter = new ParticleEmitter(texture);

            Assert.AreEqual(Vector2.Zero, emitter.Position);
            Assert.AreEqual(new Radian(0.0f), emitter.Direction);
            Assert.AreEqual(100, emitter.MaxSprite);
            Assert.AreEqual(200, emitter.Length);
            Assert.AreEqual(new Range<Color>(Color.Black, Color.White), emitter.ColorRange);
            Assert.AreEqual(10, emitter.Spread);
            Assert.AreEqual(1.0f, emitter.Velocity);
            Assert.AreEqual(2.0f, emitter.Scale);
        }

        [TestMethod()]
        public void ParticleEmitter_Render_Test()
        {
            var texture = new Texture(Device, TestHelpers.GetAssetFullPath("particle16.tga"), "");
            var emitter = new ParticleEmitter(texture);

            Device.RawDevice.BeginScene();
            SpriteRenderer.RawSprite.Begin(D3D.SpriteFlags.AlphaBlend);
            Assert.IsTrue((emitter as ISprite).Render(SpriteRenderer));
            SpriteRenderer.RawSprite.End();
            Device.RawDevice.EndScene();

            emitter.Update(1000.0f);

            Device.RawDevice.BeginScene();
            SpriteRenderer.RawSprite.Begin(D3D.SpriteFlags.AlphaBlend);
            Assert.IsTrue((emitter as ISprite).Render(SpriteRenderer));
            SpriteRenderer.RawSprite.End();
            Device.RawDevice.EndScene();
        }
    }
}
