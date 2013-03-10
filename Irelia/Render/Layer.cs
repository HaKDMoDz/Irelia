
namespace Irelia.Render
{
    public sealed class Layer
    {
        #region Properties
        public Size BufferSize { get; private set; }
        public Size WindowSize { get; private set; }
        public TextureRenderTarget RenderTarget { get; private set; }
        public Sprite Sprite { get; private set; }

        public float ScrollX
        {
            get { return this.scrollX; }
            set 
            {
                if (this.scrollX == value)
                    return;

                this.scrollX = System.Math.Max(ScrollRangeX.Min, value);
                this.scrollX = System.Math.Min(ScrollRangeX.Max, this.scrollX);

                UpdateSpriteSourceRect();
            }
        }

        public float ScrollY
        {
            get { return this.scrollY; }
            set 
            {
                if (this.scrollY == value)
                    return;
            
                this.scrollY = System.Math.Max(ScrollRangeY.Min, value);
                this.scrollY = System.Math.Min(ScrollRangeY.Max, this.scrollY);

                UpdateSpriteSourceRect();
            }
        }

        public Range<float> ScrollRangeX { get; private set; }
        public Range<float> ScrollRangeY { get; private set; }
        #endregion

        #region Private fields
        private float scrollX;
        private float scrollY;
        #endregion

        public Layer(Device device, Size bufferSize, Size windowSize)
        {
            BufferSize = bufferSize;
            WindowSize = windowSize;
            RenderTarget = new TextureRenderTarget(device, BufferSize);
            Sprite = new Sprite(RenderTarget.Texture);
            ScrollRangeX = new Range<float>(0.0f, bufferSize.Width - windowSize.Width - 1);
            ScrollRangeY = new Range<float>(0.0f, bufferSize.Height - windowSize.Height - 1);
        }

        public void Scroll(float deltaX, float deltaY)
        {
            ScrollX += deltaX;
            ScrollY += deltaY;
        }

        private void UpdateSpriteSourceRect()
        {
            Sprite.SourceRect = new Rectangle()
            {
                Left = ScrollX,
                Top = ScrollY,
                Width = WindowSize.Width,
                Height = WindowSize.Height
            };
        }
    }
}
