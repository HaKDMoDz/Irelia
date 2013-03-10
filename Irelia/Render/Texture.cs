using D3D = SlimDX.Direct3D9;
using System.IO;
using System;

namespace Irelia.Render
{
    public sealed class Texture : DisposableObject
    {
        #region Properties
        public D3D.Texture RawTexture { get; private set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        #endregion

        private readonly D3D.ImageFileFormat imageFileFormat;

        #region Constructors
        public Texture(Device device, string filePath, string name)
        {
            var imgInfo = D3D.ImageInformation.FromFile(filePath);

            RawTexture = D3D.Texture.FromFile(device.RawDevice, filePath,
                                              imgInfo.Width, imgInfo.Height,
                                              1,
                                              D3D.Usage.None,
                                              D3D.Format.Unknown,
                                              D3D.Pool.Default,
                                              D3D.Filter.Default,
                                              D3D.Filter.Default,
                                              new Color(1.0f, 0.0f, 1.0f, 0.0f).ToArgb());
            Name = name;
            Width = imgInfo.Width;
            Height = imgInfo.Height;
            this.imageFileFormat = imgInfo.ImageFileFormat;
        }

        public Texture(Device device, Stream stream, string name)
        {
            var imgInfo = D3D.ImageInformation.FromStream(stream);
            RawTexture = D3D.Texture.FromStream(device.RawDevice, stream, (int)(stream.Length - stream.Position),
                                                imgInfo.Width, imgInfo.Height,
                                                1,
                                                D3D.Usage.None,
                                                D3D.Format.Unknown,
                                                D3D.Pool.Default,
                                                D3D.Filter.Default,
                                                D3D.Filter.Default,
                                                new Color(1.0f, 0.0f, 1.0f, 0.0f).ToArgb());

            Name = name;
            Width = imgInfo.Width;
            Height = imgInfo.Height;
            this.imageFileFormat = imgInfo.ImageFileFormat;
        }

        public Texture(Device device, string name, Size size)
        {
            RawTexture = new D3D.Texture(device.RawDevice, (int)size.Width, (int)size.Height, 1, D3D.Usage.RenderTarget,
                D3D.Format.A8R8G8B8, D3D.Pool.Default);

            Name = name;
            Width = (int)size.Width;
            Height = (int)size.Height;
            this.imageFileFormat = D3D.ImageFileFormat.Bmp;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public bool SaveToFile(string filePath)
        {
            return D3D.Texture.ToFile(RawTexture, filePath, D3D.ImageFileFormat.Jpg).IsSuccess;
        }

        public Stream ToStream()
        {
            return D3D.Texture.ToStream(RawTexture, this.imageFileFormat);
        }

        public Stream ToJpgStream()
        {
            return D3D.Texture.ToStream(RawTexture, D3D.ImageFileFormat.Jpg);
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (RawTexture != null)
                    RawTexture.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }

    public sealed class TextureFactory : IAssetFactory
    {
        public string[] FileExtensions
        {
            get { return new string[] { ".jpg", ".bmp", ".dds", ".tga", ".png" }; }
        }

        public Type AssetType { get { return typeof(Texture); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            bool cube = args.GetValue<bool>("cube", false);
            if (cube)
            {
                return new CubeTexture(device, filePath, name);
            }
            else
            {
                return new Texture(device, filePath, name);
            }
        }
    }
}
