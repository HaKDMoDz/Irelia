using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Irelia.Render
{
    public class AssetLoadArguments
    {
        private readonly IDictionary<string, object> arguments = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public void Add(string key, object value)
        {
            this.arguments.Add(key, value);
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            object value;
            if (this.arguments.TryGetValue(key, out value) == false)
                return defaultValue;
            return (T)value;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            AssetLoadArguments otherArgs = other as AssetLoadArguments;
            return this.arguments.Keys.SequenceEqual<string>(otherArgs.arguments.Keys, StringComparer.OrdinalIgnoreCase) &&
                   this.arguments.Values.SequenceEqual<object>(otherArgs.arguments.Values);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public interface IAssetFactory
    {
        Type AssetType { get; }
        string[] FileExtensions { get; }
        object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager);
    }

    public class AssetManager
    {
        #region Public Properties
        public Texture DefaultTexture { get; private set; }
        public Material DefaultMaterial { get; private set; }
        public Font DefaultFont { get; private set; }

        public string RootPath { get; private set; }

        public Type[] AllAssetTypes
        {
            get { return this.assetFactories.Values.Distinct().Select(factory => factory.AssetType).ToArray(); }
        }
        #endregion

        private class ArgumentAssets : List<KeyValuePair<AssetLoadArguments, object>>
        {
        }

        #region Private fields
        private readonly Device device;
        private readonly Dictionary<string, ArgumentAssets> assets = new Dictionary<string, ArgumentAssets>(StringComparer.OrdinalIgnoreCase);
        private readonly AssetLoadArguments EmptyArguments = new AssetLoadArguments();
        private readonly IDictionary<string, IAssetFactory> assetFactories = new Dictionary<string, IAssetFactory>(StringComparer.OrdinalIgnoreCase);
        #endregion

        public AssetManager(Device device, string rootPath)
        {
            this.device = device;
            RootPath = rootPath;

            RegisterAssetFactory(new TextureFactory());
            RegisterAssetFactory(new ShaderFactory());
            RegisterAssetFactory(new MeshFactory());
            RegisterAssetFactory(new MaterialFactory());
            RegisterAssetFactory(new WorldFactory());
            RegisterAssetFactory(new DirectXMeshFactory());
            RegisterAssetFactory(new FontFactory());

            DefaultTexture = Load(@"Engine\water.bmp") as Texture;
            DefaultMaterial = new Material("Default Material") 
            { 
                Shader = Load(@"Engine\UberShader.fx") as Shader, 
                DiffuseTexture = DefaultTexture 
            };
            DefaultFont = Load(@"Engine\Consolas9.font") as Font;
        }

        #region Public Methods
        public string GetName(string fullPath)
        {
            return GetRelativePath(fullPath);
        }

        public string GetFullPath(string path)
        {
            return Path.GetFullPath(Path.Combine(RootPath, path));
        }

        public string GetShortName(string name)
        {
            return Path.GetFileName(name);
        }

        public object Load(string filePath, AssetLoadArguments args = null)
        {
            string fullPath = GetFullPath(filePath);
            string name = GetName(fullPath);

            if (args == null)
                args = EmptyArguments;

            object asset = GetCachedAsset(fullPath, args);
            if (asset != null)
                return asset;

            asset = LoadAsset(filePath, args, fullPath, name);

            if (asset != null)
            {
                if (!this.assets.ContainsKey(fullPath))
                    this.assets.Add(fullPath, new ArgumentAssets());

                this.assets[fullPath].Add(new KeyValuePair<AssetLoadArguments,object>(args, asset));
            }

            return asset;
        }

        public void RegisterAssetFactory(IAssetFactory factory)
        {
            foreach (var extension in factory.FileExtensions)
            {
                if (this.assetFactories.ContainsKey(extension))
                    throw new ArgumentException("Factory with same extension already registered");

                this.assetFactories.Add(extension, factory);
            }
        }

        public Type GetAssetType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (this.assetFactories.ContainsKey(extension) == false)
                return null;

            return this.assetFactories[extension].AssetType;
        }
        #endregion

        #region Private Methods
        private object GetCachedAsset(string fullPath, AssetLoadArguments args)
        {
            ArgumentAssets argAssets = null;
            if (this.assets.TryGetValue(fullPath, out argAssets))
            {
                foreach (var argAsset in argAssets)
                {
                    if (argAsset.Key.Equals(args))
                        return argAsset.Value;
                }
            }
            return null;
        }

        private string GetRelativePath(string targetPath)
        {
            Uri pathToUri;
            if (!Uri.TryCreate(RootPath + Path.DirectorySeparatorChar, UriKind.Absolute, out pathToUri))
                return targetPath;

            Uri targetPathUri;
            if (!Uri.TryCreate(targetPath, UriKind.Absolute, out targetPathUri))
                return targetPath;

            string relativePath = pathToUri.MakeRelativeUri(targetPathUri).ToString();
            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        private object LoadAsset(string filePath, AssetLoadArguments args, string fullPath, string name)
        {
            object asset = null;
            try
            {
                var extension = Path.GetExtension(fullPath);
                if (GetAssetType(fullPath) != null)
                {
                    asset = this.assetFactories[extension].Load(device, fullPath, args, name, this);
                }
                else
                {
                    throw new ArgumentException("Unknown asset type, " + filePath, "filePath");
                }
            }
            catch (Exception ex)
            {
                Log.Msg(TraceLevel.Warning, "Failed load asset: " + filePath + " (" + GetAssetType(fullPath) + ")\n" + ex.ToString());
                return null;
            }
            return asset;
        }
        #endregion
    }
}
