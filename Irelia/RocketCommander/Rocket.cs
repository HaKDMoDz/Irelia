using Irelia.Render;

namespace RocketCommander
{
    public sealed class Rocket
    {
        public Mesh Mesh { get; private set; }
        public MeshNode MeshNode { get; private set; }

        private readonly float scale = 0.0075f;

        public Rocket(Framework framework)
        {
            Mesh = framework.AssetManager.Load(@"RocketCommander\rocket.meshb") as Mesh;
            
            MeshNode = new MeshNode(framework.Device, this.Mesh)
            {
                Scale = new Vector3(scale, scale, scale)
            };
        }
    }
}
