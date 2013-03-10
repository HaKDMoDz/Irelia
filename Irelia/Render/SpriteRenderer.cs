using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public class SpriteRenderer
    {
        public D3D.Sprite RawSprite { get; private set; }

        public SpriteRenderer(Device device)
        {
            RawSprite = new D3D.Sprite(device.RawDevice);
        }
    }
}
