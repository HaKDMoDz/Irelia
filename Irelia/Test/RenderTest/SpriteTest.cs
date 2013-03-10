using System.IO;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using D3D = SlimDX.Direct3D9;

namespace RenderTest
{
    [TestClass()]
    public class SpriteTest
    {
        private Device Device { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            SpriteRenderer = new SpriteRenderer(Device);
        }

        private Sprite CreateSprite(string textureFile = "Test/fatship256.tga")
        {
            var texture = new Texture(Device, Path.Combine(RenderSettings.MediaPath, textureFile), "");
            return new Sprite(texture);
        }

        [TestMethod()]
        public void Sprite_Constructor_Test()
        {
            var texture = new Texture(Device, Path.Combine(RenderSettings.MediaPath, "Test/fatship256.tga"), "");
            var sprite = new Sprite(texture);
            Assert.AreEqual(texture, sprite.Texture);
            Assert.AreEqual(Color.White, sprite.Color);
        }

        [TestMethod()]
        public void Sprite_Render_Test()
        {
            var sprite = CreateSprite();

            Device.RawDevice.BeginScene();
            SpriteRenderer.RawSprite.Begin(D3D.SpriteFlags.AlphaBlend);
            Assert.IsTrue((sprite as ISprite).Render(SpriteRenderer));
            SpriteRenderer.RawSprite.End();
            Device.RawDevice.EndScene();
        }

        [TestMethod()]
        public void Sprite_Transform_Test()
        {
            var sprite = CreateSprite();

            Assert.AreEqual(Vector2.Zero, sprite.Position);
            var newPos = new Vector2(1.0f, 2.0f);
            sprite.Position = newPos;
            Assert.AreEqual(newPos, sprite.Position);

            Assert.AreEqual(new Vector2(1.0f, 1.0f), sprite.Scale);
            var newScale = new Vector2(0.5f, 2.0f);
            sprite.Scale = newScale;
            Assert.AreEqual(newScale, sprite.Scale);

            Assert.AreEqual(new Radian(0.0f), sprite.Rotation);
            var newRotation = new Radian(MathUtils.PI);
            sprite.Rotation = newRotation;
            Assert.AreEqual(newRotation, sprite.Rotation);
        }

        [TestMethod()]
        public void Sprite_Velocity_Test()
        {
            var sprite = CreateSprite();
            Assert.AreEqual(Vector2.Zero, sprite.Velocity);

            var newVelocity = new Vector2(1.0f, -2.0f);
            sprite.Velocity = newVelocity;
            Assert.AreEqual(newVelocity, sprite.Velocity);

            float delta = 2.0f;
            sprite.Update(delta);
            Assert.AreEqual(newVelocity * delta, sprite.Position);
        }
    }
}
