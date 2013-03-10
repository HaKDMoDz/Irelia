using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public class Sprite : ISprite
    {
        #region Properties
        public Texture Texture { get; private set; }
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Radian Rotation { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle SourceRect { get; set; }
        #endregion

        #region Constructors
        public Sprite(Texture texture)
        {
            Texture = texture;
            Color = Color.White;
            Position = Vector2.Zero;
            Scale = new Vector2(1.0f, 1.0f);
            Rotation = new Radian(0.0f);
            SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
        #endregion

        #region Public methods
        public void Update(float delta)
        {
            OnUpdate(delta);
        }
        #endregion

        protected virtual void OnUpdate(float delta)
        {
            Position += (Velocity * delta);
        }

        #region Implements ISprite
        bool ISprite.Render(SpriteRenderer spriteRenderer)
        {
            var pivot = new Vector2((SourceRect.Width * Scale.x) / 2, (SourceRect.Height * Scale.y) / 2);
            var center = pivot;

            var transformMatrix = SlimDX.Matrix.Transformation2D(Vector2.Zero.ToD3DVector2(), 0.0f, Scale.ToD3DVector2(),
                                                                 center.ToD3DVector2(), Rotation.Value,
                                                                 Position.ToD3DVector2());

            spriteRenderer.RawSprite.Transform = transformMatrix;

            var ret = spriteRenderer.RawSprite.Draw(Texture.RawTexture, SourceRect.ToD3DRectangle(), null, null, Color.ToD3DColor4());

            spriteRenderer.RawSprite.Transform = SlimDX.Matrix.Identity;
            
            return ret.IsSuccess;
        }
        #endregion
    }
}
