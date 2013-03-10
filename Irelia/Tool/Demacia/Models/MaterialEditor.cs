using System.ComponentModel;
using Demacia.Views;
using Irelia.Render;
using WM = System.Windows.Media;

namespace Demacia.Models
{
    public sealed class MaterialEditor
    {
        private readonly Material material;

        public MaterialEditor(Material material)
        {
            this.material = material;
        }

        [Category("Information")]
        [Description("Material Name")]
        public string Name
        {
            get { return this.material.Name; }
        }

        [Category("Shader")]
        [Editor(typeof(ShaderTypeView), typeof(ShaderTypeView))]
        public Shader Shader
        {
            get { return this.material.Shader; }
            set { this.material.Shader = value; }
        }

        [Category("Shader")]
        public string Technique
        {
            get { return this.material.Technique; }
            set { this.material.Technique = value; }
        }

        [Category("Diffuse")]
        [DisplayName("Diffuse Texture")]
        [Editor(typeof(TextureTypeView), typeof(TextureTypeView))]
        public Texture DiffuseTexture
        {
            get { return this.material.DiffuseTexture; }
            set { this.material.DiffuseTexture = value; }
        }

        [Category("Diffuse")]
        [DisplayName("Diffuse Color")]
        public WM.Color DiffuseColor
        {
            get { return this.material.DiffuseColor.ToWindowsColor(); }
            set { this.material.DiffuseColor = value.ToIreliaColor(); }
        }
        
        [Category("Normal")]
        [DisplayName("Use Normal Mapping")]
        public bool UseNormalMap
        {
            get { return this.material.UseNormalMap; }
            set { this.material.UseNormalMap = value; }
        }

        [Category("Normal")]
        [DisplayName("Normal Texture")]
        [Editor(typeof(TextureTypeView), typeof(TextureTypeView))]
        public Texture NormalTexture
        {
            get { return this.material.NormalTexture; }
            set { this.material.NormalTexture = value; }
        }

        [Category("Specular")]
        [DisplayName("Specular Power")]
        public float SpecularPower
        {
            get { return this.material.SpecularPower; }
            set { this.material.SpecularPower = value; }
        }

        [Category("Specular")]
        [DisplayName("Specular Color")]
        public WM.Color SpecularColor
        {
            get { return this.material.SpecularColor.ToWindowsColor(); }
            set { this.material.SpecularColor = value.ToIreliaColor(); }
        }

        [Category("Ambient")]
        [DisplayName("Ambient Color")]
        public WM.Color AmbientColor
        {
            get { return this.material.AmbientColor.ToWindowsColor(); }
            set { this.material.AmbientColor = value.ToIreliaColor(); }
        }

        [Category("Emissive")]
        [DisplayName("Emissive Color")]
        public WM.Color EmissiveColor
        {
            get { return this.material.EmissiveColor.ToWindowsColor(); }
            set { this.material.EmissiveColor = value.ToIreliaColor(); }
        }

        [Category("Offset Mapping")]
        [DisplayName("Use Offset Mapping")]
        public bool UseOffsetMapping
        {
            get { return this.material.UseOffsetMapping; }
            set { this.material.UseOffsetMapping = value; }
        }

        [Category("Offset Mapping")]
        [Editor(typeof(TextureTypeView), typeof(TextureTypeView))]
        [DisplayName("Height Map Texture")]
        public Texture HeightMapTexture
        {
            get { return this.material.HeightMapTexture; }
            set { this.material.HeightMapTexture = value; }
        }

        [Category("Offset Mapping")]
        [DisplayName("Offset Height")]
        public float OffsetHeight
        {
            get { return this.material.OffsetHeight; }
            set { this.material.OffsetHeight = value; }
        }
    }
}
