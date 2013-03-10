using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace Irelia.Render
{
    [Serializable]
    public class MaterialData
    {
        public string Shader { get; set; }
        public string DiffuseTexture { get; set; }
        public bool UseNormalMap { get; set; }
        public string NormalTexture { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Color EmissiveColor { get; set; }
        public float SpecularPower { get; set; }
        public Color SpecularColor { get; set; }
        public string Technique { get; set; }
        public bool UseOffsetMapping { get; set; }
        public string HeightMapTexture { get; set; }
        public float OffsetHeight { get; set; }

        public MaterialData(Material mat)
        {
            Shader = mat.Shader.Name;
            if (mat.DiffuseTexture != null)
                DiffuseTexture = mat.DiffuseTexture.Name;
            UseNormalMap = mat.UseNormalMap;
            if (mat.NormalTexture != null)
                NormalTexture = mat.NormalTexture.Name;
            AmbientColor = mat.AmbientColor;
            DiffuseColor = mat.DiffuseColor;
            EmissiveColor = mat.EmissiveColor;
            SpecularPower = mat.SpecularPower;
            SpecularColor = mat.SpecularColor;
            Technique = mat.Technique;
            UseOffsetMapping = mat.UseOffsetMapping;
            if (mat.HeightMapTexture != null)
                HeightMapTexture = mat.HeightMapTexture.Name;
            OffsetHeight = mat.OffsetHeight;
        }
    }

    public class Material : Resource<MaterialData>
    {
        public string Name { get; set; }
        public Shader Shader { get; set; }
        public Texture DiffuseTexture { get; set; }
        public bool UseNormalMap { get; set; }
        public Texture NormalTexture { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Color EmissiveColor { get; set; }
        public float SpecularPower { get; set; }
        public Color SpecularColor { get; set; }
        public string Technique { get; set; }
        public bool UseOffsetMapping { get; set; }
        public Texture HeightMapTexture { get; set; }
        public float OffsetHeight { get; set; }

        public Material(string name)
        {
            Name = name;
            Technique = "SimpleEffect";
        }

        private Material(Device device, string name, MaterialData matData, AssetManager assetManager)
            : this(name)
        {
            Shader = assetManager.Load(matData.Shader) as Shader;
            if (string.IsNullOrWhiteSpace(matData.DiffuseTexture) == false)
                DiffuseTexture = assetManager.Load(matData.DiffuseTexture) as Texture;
            UseNormalMap = matData.UseNormalMap;
            if (string.IsNullOrWhiteSpace(matData.NormalTexture) == false)
                NormalTexture = assetManager.Load(matData.NormalTexture) as Texture;
            AmbientColor = matData.AmbientColor;
            DiffuseColor = matData.DiffuseColor;
            EmissiveColor = matData.EmissiveColor;
            SpecularPower = matData.SpecularPower;
            SpecularColor = matData.SpecularColor;
            Technique = matData.Technique;
            UseOffsetMapping = matData.UseOffsetMapping;
            if (string.IsNullOrWhiteSpace(matData.HeightMapTexture) == false)
                HeightMapTexture = assetManager.Load(matData.HeightMapTexture) as Texture;
            OffsetHeight = matData.OffsetHeight;
        }

        public override string ToString()
        {
            return Name;
        }

        #region Save & Load
        public void Save(string fileName)
        {
            Save(fileName, new MaterialData(this));
        }

        public static Material Load(Device device, string fileName, string name, AssetManager assetManager)
        {
            MaterialData matData = Load(fileName);
            return new Material(device, name, matData, assetManager);
        }
        #endregion
    }

    public sealed class MaterialFactory : IAssetFactory
    {
        public string[] FileExtensions
        {
            get { return new string[] { ".mats", ".matb" }; }
        }

        public Type AssetType { get { return typeof(Material); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            return Material.Load(device, filePath, name, assetManager);
        }
    }
}
