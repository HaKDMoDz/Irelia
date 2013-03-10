
namespace Irelia.Render
{
    public class AnimateSprite : Sprite
    {
        public Range<int> FrameRange { get; private set; }
        public int NumFramePerRow { get; private set; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public int CurrentFrame
        {
            get { return (int)this.currentFrame; }
            set 
            {
                if (this.currentFrame == value)
                    return;

                this.currentFrame = value;

                UpdateSourceRect();
            }
        }

        private float currentFrame;

        public AnimateSprite(Texture texture, Range<int> frameRange, int startFrame, int numFramePerRow, int frameWidth, int frameHeight)
            : base(texture)
        {
            FrameRange = frameRange;
            this.currentFrame = (int)startFrame;
            NumFramePerRow = numFramePerRow;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            UpdateSourceRect();
        }

        protected override void OnUpdate(float delta)
        {
 	        base.OnUpdate(delta);

            this.currentFrame += delta;

            if ((int)this.currentFrame > FrameRange.Max)
                this.currentFrame = (float)FrameRange.Min;

            UpdateSourceRect();
        }

        private void UpdateSourceRect()
        {
            SourceRect = new Rectangle()
            {
                Left = (((int)CurrentFrame % NumFramePerRow) * FrameWidth),
                Top = (((int)CurrentFrame / NumFramePerRow) * FrameHeight),
                Width = FrameWidth,
                Height = FrameHeight
            };
        }
    }
}
