using System.Collections.Generic;
using System;

namespace Irelia.Render
{
    public sealed class ParticleEmitter : ISprite
    {
        private readonly Texture texture;
        private readonly Timer timer = new Timer();
        private readonly IList<Sprite> sprites = new List<Sprite>();
        private readonly Random random = new Random();

        public Vector2 Position { get; set; }
        public Radian Direction { get; set; }
        public int MaxSprite { get; set; }
        public int Length { get; set; }
        public Range<Color> ColorRange { get; set; }
        public int Spread { get; set; }
        public float Velocity { get; set; }
        public float Scale { get; set; }

        public ParticleEmitter(Texture texture)
        {
            this.texture = texture;
            MaxSprite = 100;
            Length = 200;
            ColorRange = new Range<Color>(Color.Black, Color.White);
            Spread = 10;
            Velocity = 1.0f;
            Scale = 2.0f;

            this.timer.Start();
        }

        public void Update(float elapsedTime)
        {
            if (this.sprites.Count < MaxSprite &&
                this.timer.Elapsed > 100)
            {
                AddSprite();

                this.timer.Start(true);
            }

            foreach (var sprite in this.sprites)
            {
                sprite.Update(elapsedTime / 1000.0f * 50.0f);

                // Is particle beyond the emitter's range?
                float distance = (sprite.Position - Position).Length;
                if (distance > Length)
                {
                    sprite.Position = Position;
                }
            }
        }

        private void AddSprite()
        {
            // Linear velocity
            float variation = (random.Next(Spread) - Spread / 2) / 100.0f;
            Radian dir = (Direction - new Radian(MathUtils.PI / 2)).Wrap(new Radian(MathUtils.PI * 2));
            Vector2 velocity = new Vector2(MathUtils.Cos(dir) + variation, MathUtils.Sin(dir) + variation);

            // Random color based on ranges
            int a = random.Next(ColorRange.Min.IntA, ColorRange.Max.IntA);
            int r = random.Next(ColorRange.Min.IntR, ColorRange.Max.IntR);
            int g = random.Next(ColorRange.Min.IntG, ColorRange.Max.IntG);
            int b = random.Next(ColorRange.Min.IntB, ColorRange.Max.IntB);
            Color color = new Color(a, r, g, b);

            Sprite sprite = new Sprite(this.texture)
            {
                Position = this.Position,
                Velocity = velocity * this.Velocity,
                Color = color,
                Scale = new Vector2(this.Scale, this.Scale)
            };

            this.sprites.Add(sprite);
        }

        #region Implements IRenderable
        bool ISprite.Render(SpriteRenderer spriteRenderer)
        {
            foreach (var sprite in this.sprites)
            {
                if ((sprite as ISprite).Render(spriteRenderer) == false)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
