using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace Irelia.Render
{
    [Serializable]
    public sealed class WorldData
    {
        public string SkyboxTexture { get; set; }
        public bool DarkenBorderEnabled { get; set; }
        public string DarkenBorderTexture { get; set; }
        public bool GlowEnabled { get; set; }
        public float GlowRadialBlurScaleFactor { get; set; }
        public float GlowBlurWidth { get; set; }
        public float GlowGlowIntensity { get; set; }
        public float GlowHighlightIntensity { get; set; }

        public WorldData(World world)
        {
            if (world.Skybox.SkyTexture != null)
                SkyboxTexture = world.Skybox.SkyTexture.Name;
            DarkenBorderEnabled = world.DarkenBorder.IsEnabled;
            if (world.DarkenBorder.BorderTexture != null)
                DarkenBorderTexture = world.DarkenBorder.BorderTexture.Name;
            GlowEnabled = world.Glow.IsEnabled;
            GlowRadialBlurScaleFactor = world.Glow.RadialBlurScaleFactor;
            GlowBlurWidth = world.Glow.BlurWidth;
            GlowGlowIntensity = world.Glow.GlowIntensity;
            GlowHighlightIntensity = world.Glow.HighlightIntensity;
        }
    }

    public sealed class World : Resource<WorldData>
    {
        #region Properties
        public string Name { get; set; }
        public Skybox Skybox { get; private set; }
        public DarkenBorder DarkenBorder { get; private set; }
        public Glow Glow { get; private set; }
        #endregion

        public World(Device device, string name, AssetManager assetManager)
        {
            Name = name;
            Skybox = new Skybox(device, assetManager);
            DarkenBorder = new DarkenBorder(device, assetManager);
            Glow = new Glow(device, assetManager);
        }

        private World(Device device, string name, WorldData worldData, AssetManager assetManager)
            : this(device, name, assetManager)
        {
            if (string.IsNullOrWhiteSpace(worldData.SkyboxTexture) == false)
            {
                AssetLoadArguments args = new AssetLoadArguments();
                args.Add("cube", true);
                Skybox.SkyTexture = assetManager.Load(worldData.SkyboxTexture, args) as CubeTexture;
            }

            DarkenBorder.IsEnabled = worldData.DarkenBorderEnabled;
            if (string.IsNullOrWhiteSpace(worldData.DarkenBorderTexture) == false)
            {
                DarkenBorder.BorderTexture = assetManager.Load(worldData.DarkenBorderTexture) as Texture;
            }

            Glow.IsEnabled = worldData.GlowEnabled;
            Glow.RadialBlurScaleFactor = worldData.GlowRadialBlurScaleFactor;
            Glow.BlurWidth = worldData.GlowBlurWidth;
            Glow.GlowIntensity = worldData.GlowGlowIntensity;
            Glow.HighlightIntensity = worldData.GlowHighlightIntensity;
        }

        public override string ToString()
        {
            return Name;
        }

        #region Save & Load
        public void Save(string fileName)
        {
            Save(fileName, new WorldData(this));
        }

        public static World Load(Device device, string fileName, string name, AssetManager assetManager)
        {
            WorldData worldData = Load(fileName);
            return new World(device, name, worldData, assetManager);
        }
        #endregion
    }

    public sealed class WorldFactory : IAssetFactory
    {
        public string[] FileExtensions 
        { 
            get { return new string[] {".worlds", ".worldb"}; } 
        }
        
        public Type AssetType { get { return typeof(World); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            return World.Load(device, filePath, name, assetManager);
        }
    }
}
