using System;
using D3D = SlimDX.Direct3D9;
using System.Collections.Generic;
using System.Diagnostics;

namespace Irelia.Render
{
    public sealed class Effect : DisposableObject
    {
        #region Private Fields
        private readonly D3D.Effect d3dEffect;
        private string techniqueName;
        #endregion

        #region Properties
        public string Technique
        {
            get
            {
                return this.techniqueName;
            }
        }

        public Parameters Params { get; private set; }
        #endregion

        public Effect(Device device, string filePath)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            this.d3dEffect = D3D.Effect.FromFile(device.RawDevice, filePath, D3D.ShaderFlags.EnableBackwardsCompatibility);
            Params = new Parameters(this);
        }

        #region Public Methods
        public int Begin()
        {
            return this.d3dEffect.Begin();
        }

        public bool End()
        {
            return this.d3dEffect.End().IsSuccess;
        }

        public bool BeginPass(int pass)
        {
            return this.d3dEffect.BeginPass(pass).IsSuccess;
        }

        public bool EndPass()
        {
            return this.d3dEffect.EndPass().IsSuccess;
        }

        public bool SetTechnique(string name)
        {
            if (this.techniqueName == name)
                return true;

            var handle = new D3D.EffectHandle(name);
            if (this.d3dEffect.ValidateTechnique(handle) == false)
                return false;

            this.techniqueName = name;
            this.d3dEffect.Technique = handle;
            return true;
        }
        #endregion

        #region Effect Parameter
        public class Param
        {
            private readonly Effect effect;
            private readonly D3D.EffectHandle handle;

            public Param(Effect effect, D3D.EffectHandle handle)
            {
                this.effect = effect;
                this.handle = handle;
            }

            public bool SetValue(bool value)
            {
                return SetValue<bool>(value);
            }

            public bool SetValue(float value)
            {
                return SetValue<float>(value);
            }

            public bool SetValue(Matrix4 mat)
            {
                return SetValue(mat.ToD3DMatrix());
            }

            public bool SetValue(Vector2 v2)
            {
                return SetValue(v2.ToD3DVector2());
            }

            public bool SetValue(Vector3 v3)
            {
                return SetValue(v3.ToD3DVector3());
            }

            public bool SetValue(Vector4 v4)
            {
                return SetValue(v4.ToD3DVector4());
            }

            public bool SetValue(Texture texture)
            {
                return SetValue((texture != null)? texture.RawTexture : null, 
                                (texture != null)? texture.ToString() : "null");
            }

            public bool SetValue(CubeTexture texture)
            {
                return SetValue((texture != null)? texture.RawCubeTexture : null,
                                (texture != null) ? texture.ToString() : "null");
            }

            public bool SetValue(Color color)
            {
                return SetValue(color.ToD3DVector4());
            }

            private bool SetValue<T>(T value) where T : struct
            {
                if (this.handle == null)
                    return false;

                try
                {
                    var result = this.effect.d3dEffect.SetValue(this.handle, value);
                    return result.IsSuccess;
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Effect.SetValue failed", value.ToString(), e);
                }
            }

            private bool SetValue(D3D.BaseTexture baseTexture, string name)
            {
                if (this.handle == null)
                    return false;

                try
                {
                    var result = this.effect.d3dEffect.SetTexture(this.handle, baseTexture);
                    return result.IsSuccess;
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Effect.SetTexture failed", name, e);
                }
            }
        }

        public class Parameters
        {
            private readonly Effect effect;

            public Parameters(Effect effect)
            {
                this.effect = effect;
            }

            public Param this[string name]
            {
                get
                {
                    Param param;
                    if (this.effectParams.TryGetValue(name, out param))
                        return param;

                    var paramHandle = this.effect.d3dEffect.GetParameter(null, name);
                    if (paramHandle == null)
                    {
                        Log.Msg(TraceLevel.Warning, this, "Failed to get effect parameter " + name);
                    }

                    this.effectParams.Add(name, new Param(this.effect, paramHandle));
                    return effectParams[name];
                }
            }

            private readonly IDictionary<string, Param> effectParams = new Dictionary<string, Param>();
        }
        #endregion

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.d3dEffect != null)
                    this.d3dEffect.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
