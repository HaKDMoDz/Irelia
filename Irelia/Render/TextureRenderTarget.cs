using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public class TextureRenderTarget : RenderTarget
    {
        public Texture Texture { get; private set; }

        public TextureRenderTarget(Device device, Size size)
        {
            Texture = new Texture(device, "RenderTarget", size);
            TargetSurface = Texture.RawTexture.GetSurfaceLevel(0);
            DepthStencilSurface = D3D.Surface.CreateDepthStencil(device.RawDevice, (int)size.Width, (int)size.Height, D3D.Format.D16,
                D3D.MultisampleType.None, 0, false);
            Size = size;
        }
    }
}
