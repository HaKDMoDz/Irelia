using Irelia.Render;
using System.IO;
using System;
using D3D = SlimDX.Direct3D9;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    internal class TestHelpers
    {
        internal static Device device;

        internal static Device GetDevice()
        {
            if (device == null)
            {
                Window window = new Window("TestHelpers.GetDevice", 640, 480);
                device = new Device(window.Handle, window.Width, window.Height);
            }
            return device;
        }

        internal static string TestMediaPath
        {
            get { return Path.Combine(RenderSettings.MediaPath, "Test"); }
        }

        internal static string SampleShaderPath
        {
            get { return Path.Combine(TestMediaPath, "Simple.fx"); }
        }

        internal static string SampleTexturePath
        {
            get { return Path.Combine(TestMediaPath, "Default_color.dds"); }
        }

        internal static string SampleCubeTexturePath
        {
            get { return Path.Combine(TestMediaPath, "SpaceSkyCubeMap.dds"); }
        }

        internal static string SampleDirectXMeshPath
        {
            get { return Path.Combine(TestMediaPath, "tiger.x"); }
        }

        public static string GetAssetFullPath(string path)
        {
            return Path.Combine(TestMediaPath, path);
        }

        // IRenderable mock
        internal class RenderableMock : IRenderable
        {
            public event EventHandler RenderCalled = delegate { };

            bool IRenderable.Render(Camera camera, Light light)
            {
                RenderCalled(this, EventArgs.Empty);
                return true;
            }

            public Texture Texture { get; set; }
            public Shader Shader { get; set; }
            public Matrix4 WorldMatrix { get { return Matrix4.Identity; } }
        };

        internal static RenderableMock CreateRenderableMock(Device device)
        {
            return new RenderableMock()
            {
                Shader = TestHelpers.CreateSimpleShader(device)
            };
        }

        // ISprite mock
        internal class SpriteMock : ISprite
        {
            public event EventHandler RenderCalled = delegate { };
            private readonly Texture texture;

            public SpriteMock(Device device)
            {
                this.texture = new Texture(device, SampleTexturePath, "");
            }

            bool ISprite.Render(SpriteRenderer renderer)
            {
                renderer.RawSprite.Draw(texture.RawTexture, new SlimDX.Color4(1.0f, 0.0f, 1.0f, 0.0f));
                RenderCalled(this, EventArgs.Empty);
                return true;
            }
        }

        internal static SpriteMock CreateSpriteMock(Device device)
        {
            return new SpriteMock(device);
        }

        internal static Shader CreateSimpleShader(Device device)
        {
            return new Shader(device, TestHelpers.SampleShaderPath, TestHelpers.SampleShaderPath);
        }

        internal static bool CatchException(Type targetException, Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (e.GetType() == targetException)
                    return true;
            }

            return false;
        }

        internal static void QuaternionAreNearEqual(Quaternion lhs, Quaternion rhs)
        {
            Assert.IsTrue(MathUtils.IsNearZero((lhs - rhs).Length));
        }

        internal static void Matrix3AreNearEqual(Matrix3 lhs, Matrix3 rhs)
        {
            var diff = lhs - rhs;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    Assert.IsTrue(MathUtils.IsNearZero(diff[i, j]));
                }
            }
        }

        internal static void AreEqual(Vector3 lhs, Vector3 rhs)
        {
            var diff = lhs - rhs;
            Assert.IsTrue(MathUtils.IsNearZero(diff.x));
            Assert.IsTrue(MathUtils.IsNearZero(diff.y));
            Assert.IsTrue(MathUtils.IsNearZero(diff.z));
        }
    }
}
