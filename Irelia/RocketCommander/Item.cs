using System;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class Item
    {
        public enum Type
        {
            Fuel = 0,
            Health,
            ExtraLife,
            Speed,
            Bomb
        };

        public Type ItemType { get; private set; }
        public MeshNode MeshNode { get; private set; }
        private Mesh Mesh { get; set; }

        public Item(Framework framework, SceneManager sceneManager, Type type)
        {
            ItemType = type;

            string meshFile = @"RocketCommander\" + Enum.GetName(typeof(Type), ItemType) + ".meshb";
            Mesh = framework.AssetManager.Load(meshFile) as Mesh;

            float[] scalings = new float[] { 0.006876214f, 0.009066273f, 0.0125944568f, 0.0146259107f, 0.0168181341f };
            float scale = scalings[(int)type];

            MeshNode = new MeshNode(framework.Device, Mesh)
            {
                Scale = new Vector3(scale, scale, scale)
            };

            sceneManager.AddRenderable(MeshNode);
        }
    }
}
