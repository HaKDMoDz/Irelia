using System;

namespace Irelia.Render
{
    public class Shader
    {
        public Effect.Parameters Params { get { return this.effect.Params; } }
        public string Name { get; private set; }
        public string EffectFile { get; private set; }

        private readonly Device device;
        private Effect effect;

        public Shader(Device device, string effectFile, string name)
        {
            this.device = device;
            EffectFile = effectFile;
            Name = name;

            this.effect = new Effect(this.device, EffectFile);
        }

        public bool Reload()
        {
            try
            {
                var newEffect = new Effect(this.device, EffectFile);
                this.effect = newEffect;
                return true;
            }
            catch (Exception e)
            {
                Log.Msg(System.Diagnostics.TraceLevel.Warning, e.ToString());
                return false;
            }
        }

        public bool RenderTechnique(Func<int, bool> renderDelegate, string technique)
        {
            if (this.effect.SetTechnique(technique) == false)
                return false;

            uint numPassFailed = 0;
            var numPass = this.effect.Begin();
            for (var pass = 0; pass < numPass; ++pass)
            {
                if (RenderPass(pass, renderDelegate) == false)
                    numPassFailed++;
            }
            this.effect.End();

            return (numPassFailed == 0);
        }

        public bool Render(Material material, Light light, Camera camera, Matrix4 worldMatrix, Func<int, bool> renderDelegate)
        {
            Params["m_World"].SetValue(worldMatrix);
            Params["m_WorldIT"].SetValue(worldMatrix.Invert().Transpose());
            Params["m_WVP"].SetValue(worldMatrix * camera.ViewMatrix * camera.ProjectionMatrix);
            Params["g_vLightPos"].SetValue(light.Position);
            Params["m_ViewInv"].SetValue(camera.ViewMatrix.Invert());
            Params["DiffuseTexture"].SetValue(material.DiffuseTexture);
            Params["useNormalMap"].SetValue(material.UseNormalMap);
            Params["NormalTexture"].SetValue(material.NormalTexture);
            Params["specularPower"].SetValue(material.SpecularPower);
            Params["specularColor"].SetValue(material.SpecularColor);
            Params["useOffsetMapping"].SetValue(material.UseOffsetMapping);
            Params["HeightMapTexture"].SetValue(material.HeightMapTexture);
            Params["g_fOffsetBias"].SetValue(material.OffsetHeight);

            return RenderTechnique(renderDelegate, material.Technique);
        }

        private bool RenderPass(int pass, Func<int, bool> renderDelegate)
        {
            if (this.effect.BeginPass(pass) == false)
                return false;

            if (renderDelegate.Invoke(pass) == false)
            {
                this.effect.EndPass();
                return false;
            }

            return this.effect.EndPass();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class ShaderFactory : IAssetFactory
    {
        public string[] FileExtensions
        {
            get { return new string[] { ".fx" }; }
        }

        public Type AssetType { get { return typeof(Shader); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            return new Shader(device, filePath, name);
        }
    }
}
