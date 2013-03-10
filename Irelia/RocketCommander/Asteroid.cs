using System;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class Asteroid
    {
        public MeshNode MeshNode { get; private set; }
        private Mesh Mesh { get; set; }

        public static int NumTypes { get { return 5; } }

        private readonly int minScale = 32;
        private readonly int maxScale = 62;

        public Asteroid(Framework framework, SceneManager sceneManager, int type)
        {
            string meshFile = @"RocketCommander\asteroid" + (type + 1).ToString() + ".meshb";
            Mesh = framework.AssetManager.Load(meshFile) as Mesh;

            var random = new Random();
            float[] scalings = new float[] {0.0249557421f, 0.01308855f, 0.0136641478f, 0.0136658037f, 0.008148187f};
            float scale = random.Next(minScale, maxScale) * scalings[type];

            MeshNode = new MeshNode(framework.Device, Mesh)
            {
                Scale = new Vector3(scale, scale, scale)
            };

            sceneManager.AddRenderable(MeshNode);
        }
    }
}
