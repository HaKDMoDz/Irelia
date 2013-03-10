
namespace Irelia.Render
{
    public struct Rectangle
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Vector2 Location
        {
            get { return new Vector2(Left, Top); }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        public float Right
        {
            get { return Left + Width; }
        }

        public float Bottom
        {
            get { return Top + Height; }
        }

        public Rectangle(float left, float top, float width, float height)
            : this()
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2 location, Size size)
            : this()
        {
            Left = location.x;
            Top = location.y;
            Width = size.Width;
            Height = size.Height;
        }

        public override string ToString()
        {
            return string.Format("Rectangle({0}, {1}, {2}, {3})", Left.ToString(), Top.ToString(), Width.ToString(), Height.ToString());
        }

        public static Rectangle Parse(string s)
        {
            var words = s.Split(new char[] { '(', ',', ')' });
            return new Rectangle()
            {
                Left = float.Parse(words[1]),
                Top = float.Parse(words[2]),
                Width = float.Parse(words[3]),
                Height = float.Parse(words[4])
            };
        }
    }
}
