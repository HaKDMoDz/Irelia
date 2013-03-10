using System.ComponentModel;
using Demacia.Views;
using Irelia.Render;

namespace Demacia.Models
{
    public class WorldEditor
    {
        private readonly World world;
        private readonly AssetManager assetManager;

        public WorldEditor(World world, AssetManager assetManager)
        {
            this.world = world;
            this.assetManager = assetManager;
        }

        [Category("Sky")]
        [DisplayName("Skybox Texture")]
        [Editor(typeof(TextureTypeView), typeof(TextureTypeView))]
        public Texture SkyboxTexture
        {
            get 
            {
                if (this.world.Skybox.SkyTexture == null)
                    return null;

                string texName = this.world.Skybox.SkyTexture.Name;
                return this.assetManager.Load(texName) as Texture;
            }
            set 
            {
                if (value == null)
                    this.world.Skybox.SkyTexture = null;

                var args = new AssetLoadArguments();
                args.Add("cube", true);
                this.world.Skybox.SkyTexture = this.assetManager.Load(value.Name, args) as CubeTexture; 
            }
        }

        [Category("Post Process")]
        [DisplayName("Enable Darken Border")]
        public bool EnableDarkenBorder
        {
            get { return this.world.DarkenBorder.IsEnabled; }
            set { this.world.DarkenBorder.IsEnabled = value; }
        }

        [Category("Post Process")]
        [DisplayName("Darken Border Texture")]
        [Editor(typeof(TextureTypeView), typeof(TextureTypeView))]
        public Texture DarkenBorderTexture
        {
            get { return this.world.DarkenBorder.BorderTexture; }
            set { this.world.DarkenBorder.BorderTexture = value; }
        }

        [Category("Post Process")]
        [DisplayName("Enable Glow")]
        public bool EnableGlow
        {
            get { return this.world.Glow.IsEnabled; }
            set { this.world.Glow.IsEnabled = value; }
        }

        [Category("Post Process")]
        [DisplayName("Glow Radial Blur Scale Factor")]
        [Editor(typeof(FloatTypeView), typeof(FloatTypeView))]
        public float GlowRadialBlurScaleFactor
        {
            get { return this.world.Glow.RadialBlurScaleFactor; }
            set { this.world.Glow.RadialBlurScaleFactor = value; }
        }

        [Category("Post Process")]
        [DisplayName("Glow Blur Width")]
        [Editor(typeof(FloatTypeView), typeof(FloatTypeView))]
        public float GlowBlurWidth
        {
            get { return this.world.Glow.BlurWidth; }
            set { this.world.Glow.BlurWidth = value; }
        }

        [Category("Post Process")]
        [DisplayName("Glow Glow Intensity")]
        [Editor(typeof(FloatTypeView), typeof(FloatTypeView))]
        public float GlowGlowIntensity
        {
            get { return this.world.Glow.GlowIntensity; }
            set { this.world.Glow.GlowIntensity = value; }
        }

        [Category("Post Process")]
        [DisplayName("Glow Highlight Intensity")]
        [Editor(typeof(FloatTypeView), typeof(FloatTypeView))]
        public float GlowHighlightIntensity
        {
            get { return this.world.Glow.HighlightIntensity; }
            set { this.world.Glow.HighlightIntensity = value; }
        }
    }
}
