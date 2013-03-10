
namespace Irelia.Render
{
    public sealed class Light
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Color AmbientColor { get; set; }

        public Light()
        {
            Position = new Vector3(0.0f, 100.0f, 0.0f);
            Color = Color.White;
            AmbientColor = new Color(1.0f, 0.07f, 0.07f, 0.07f);
        }
    }
}
