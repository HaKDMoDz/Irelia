using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using D3D = SlimDX.Direct3D9;

namespace RenderTest
{
    [TestClass()]
    public class AnimateSpriteTest
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
        public void AnimateSprite_Constructor_Test()
        {
            var texture = new Texture(Device, TestHelpers.GetAssetFullPath("explosion_30_128.tga"), "");
            int frameRangeMin = 0, frameRangeMax = 29;
            int startFrame = 6;
            int numColumn = 6;
            int frameWidth = 128, frameHeight = 128;

            var sprite = new AnimateSprite(texture,
                                    new Range<int>(frameRangeMin, frameRangeMax),
                                    startFrame,
                                    numColumn,
                                    frameWidth, frameHeight);
            Assert.AreEqual(texture, sprite.Texture);
            Assert.AreEqual(new Range<int>(frameRangeMin, frameRangeMax), sprite.FrameRange);
            Assert.AreEqual(startFrame, sprite.CurrentFrame);
            Assert.AreEqual(frameWidth, sprite.FrameWidth);
            Assert.AreEqual(frameHeight, sprite.FrameHeight);
        }

        [TestMethod()]
        public void AnimateSprite_Animate_ByUpdate_Test()
        {
            var texture = new Texture(Device, TestHelpers.GetAssetFullPath("explosion_30_128.tga"), "");
            int startFrame = 6;
            var sprite = new AnimateSprite(texture, new Range<int>(0, 29), startFrame, 6, 128, 128);

            sprite.Update(1.0f);
            Assert.AreEqual(startFrame + 1, sprite.CurrentFrame);
        }

        [TestMethod()]
        public void AnimateSprite_Render_Test()
        {
            var texture = new Texture(Device, TestHelpers.GetAssetFullPath("explosion_30_128.tga"), "");
            var sprite = new AnimateSprite(texture, new Range<int>(0, 29), 0, 6, 128, 128);

            Device.RawDevice.BeginScene();
            SpriteRenderer.RawSprite.Begin(D3D.SpriteFlags.AlphaBlend);
            Assert.IsTrue((sprite as ISprite).Render(SpriteRenderer));
            SpriteRenderer.RawSprite.End();
            Device.RawDevice.EndScene();
        }
    }
}
