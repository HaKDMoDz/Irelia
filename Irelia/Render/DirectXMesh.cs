using System;

namespace Irelia.Render
{
    public sealed class DirectXMesh
    {
        public DirectXMesh()
        {
        }
    }

    public sealed class DirectXMeshFactory : IAssetFactory
    {
        public string[] FileExtensions
        {
            get { return new string[] { ".x" }; }
        }

        public Type AssetType { get { return typeof(DirectXMesh); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            bool loadHierarchy = args.GetValue<bool>("loadHierarchy", false);
            //if (loadHierarchy)
            //{
            //    return new BoneMesh(this.device, this, fullPath, name);
            //}
            //else
            {
                return Mesh.CreateFromXFile(device, assetManager, filePath, name);
            }
        }
    }
}
