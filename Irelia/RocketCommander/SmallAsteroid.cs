using System;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class SmallAsteroid
    {
        public static int NumTypes { get { return 3; } }

        public MeshNode MeshNode { get; private set; }
        private Mesh Mesh { get; set; }

        private readonly float smallAsteroidSize = 129.0f;

        public SmallAsteroid(Framework framework, SceneManager sceneManager, int type)
        {
            string meshFile = @"RocketCommander\SmallAsteroid" + (type + 1).ToString() + ".meshb";
            Mesh = framework.AssetManager.Load(meshFile) as Mesh;

            var random = new Random();
            float[] scalings = new float[] {0.00243701926f, 0.00213851035f, 0.00200786744f};
            float scale = (smallAsteroidSize + RandomHelper.GetRandomFloat(-2.5f, 5.0f)) * scalings[type];

            MeshNode = new MeshNode(framework.Device, this.Mesh)
            {
                Scale = new Vector3(scale, scale, scale)
            };

            sceneManager.AddRenderable(MeshNode);
        }
    }
}
