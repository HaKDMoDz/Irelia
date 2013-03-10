
namespace Irelia.Render
{
    public struct Size
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Size(float width, float height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return "Size(Width=" + Width + ", Height=" + Height + ")";
        }
    }
}
