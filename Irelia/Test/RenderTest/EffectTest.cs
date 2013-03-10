using System;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class EffectTest
    {
        #region Test settings & helpers
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        private Effect CreateSimpleEffect()
        {
            return new Effect(Device, TestHelpers.SampleShaderPath);
        }
        #endregion

        [TestMethod()]
        public void Effect_Constructor_Test()
        {
            Assert.IsNotNull(CreateSimpleEffect());
        }

        [TestMethod(), ExpectedException(typeof(ArgumentNullException))]
        public void Effect_Construct_Fail_Test()
        {
            Effect effect = new Effect(null, TestHelpers.SampleShaderPath);
        }

        [TestMethod()]
        public void Effect_SetTechnique_Test()
        {
            Effect effect = CreateSimpleEffect();
            
            string techniqueName = "SimpleEffect";
            Assert.IsTrue(effect.SetTechnique(techniqueName));
            Assert.AreEqual(techniqueName, effect.Technique);
            
            Assert.IsFalse(effect.SetTechnique("InvalidTechniqueName"));
        }

        [TestMethod()]
        public void Effect_Begin_Test()
        {
            Effect effect = CreateSimpleEffect();
            Assert.AreEqual(1, effect.Begin());
        }

        [TestMethod()]
        public void Effect_End_Test()
        {
            Effect effect = CreateSimpleEffect();
            effect.Begin();
            Assert.IsTrue(effect.End());
        }

        [TestMethod()]
        public void Effect_BeginPass_Test()
        {
            Effect effect = CreateSimpleEffect();
            var numPass = effect.Begin();
            for (var i = 0; i < numPass; ++i)
            {
                Assert.IsTrue(effect.BeginPass(i));
            }
        }

        [TestMethod()]
        public void Effect_EndPass_Test()
        {
            Effect effect = CreateSimpleEffect();
            effect.Begin();
            effect.BeginPass(0);
            Assert.IsTrue(effect.EndPass());
        }

        [TestMethod()]
        public void Effect_GetParameter_Test()
        {
            Effect effect = CreateSimpleEffect();
            var param = effect.Params["gWorldXf"];
            Assert.IsNotNull(param);
        }

        [Ignore]
        [TestMethod(), ExpectedException(typeof(ArgumentException))]
        public void Effect_GetInvalidParameter_Test()
        {
            Effect effect = CreateSimpleEffect();
            var param = effect.Params["nonExistingParam"];
        }

        [TestMethod()]
        public void Effect_SetValueToParameter_Test()
        {
            Effect effect = CreateSimpleEffect();
            
            // bool
            var param = effect.Params["gBool"];
            Assert.IsTrue(param.SetValue(true));

            // float
            param = effect.Params["gFloat"];
            Assert.IsTrue(param.SetValue(1.0f));

            // float4x4
            param = effect.Params["gWorldXf"];
            Assert.IsTrue(param.SetValue(Matrix4.Identity));

            // float4
            param = effect.Params["gLamp0Color"];
            Assert.IsTrue(param.SetValue(Vector4.Zero));

            // texture
            param = effect.Params["gTexture"];
            Assert.IsTrue(param.SetValue(new Texture(Device, TestHelpers.SampleTexturePath, "")));

            // float2
            param = effect.Params["gFloat2"];
            Assert.IsTrue(param.SetValue(new Vector2(0.0f, 1.0f)));
        }

        [TestMethod(), ExpectedException(typeof(ArgumentException))]
        public void Effect_SetValueToParameterFail_Test()
        {
            Effect effect = CreateSimpleEffect();
            var param = effect.Params["gWorldXf"];
            param.SetValue(Vector3.UnitX);
        }
    }
}
