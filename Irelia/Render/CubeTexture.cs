using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class CubeTexture : DisposableObject
    {
        public D3D.CubeTexture RawCubeTexture { get; private set; }
        public string Name { get; private set; }

        public CubeTexture(Device device, string filePath, string name)
        {
            RawCubeTexture = D3D.CubeTexture.FromFile(device.RawDevice, filePath);
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
