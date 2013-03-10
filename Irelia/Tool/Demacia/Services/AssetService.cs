using System.Collections.Generic;
using Demacia.Models;
using Irelia.Render;

namespace Demacia.Services
{
    public static class AssetService
    {
        public static List<TextureAsset> TextureAssets { get; private set; }
        public static List<ShaderAsset> ShaderAssets { get; private set; }
        public static List<FontAsset> FontAssets { get; private set; }

        public static void SetAssetFolders(List<AssetFolder> assetFolders)
        {
            TextureAssets = new List<TextureAsset>();
            ShaderAssets = new List<ShaderAsset>();
            FontAssets = new List<FontAsset>();
            assetFolders.ForEach((af) => CategorizeAssets(af));
        }

        private static void CategorizeAssets(AssetFolder assetFolder)
        {
            assetFolder.Assets.ForEach((asset) =>
                {
                    if (asset as TextureAsset != null)
                        TextureAssets.Add(asset as TextureAsset);
                    else if (asset as ShaderAsset != null)
                        ShaderAssets.Add(asset as ShaderAsset);
                    else if (asset as FontAsset != null)
                        FontAssets.Add(asset as FontAsset);
                });
            assetFolder.ChildFolders.ForEach((af) => CategorizeAssets(af));
        }
    }
}
