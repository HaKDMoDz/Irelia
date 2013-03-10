using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class MeshNode : IRenderable
    {
        public Vector3 Position { get; set; }
        public Quaternion Orientation { get; set; }
        public Vector3 Scale { get; set; }

        public Matrix4 WorldMatrix
        {
            get
            {
                // SRT
                return Matrix4.CreateScale(Scale) *
                       Matrix4.CreateFromQuaternion(Orientation) *
                       Matrix4.CreateTranslation(Position);
            }
        }

        public float BoundingRadius
        {
            get { return this.mesh.BoundingRadius * HighestScale; }
        }

        public Vector3 BoundingCenter
        {
            get { return this.mesh.BoundingCenter * HighestScale; }
        }

        private float HighestScale
        {
            get
            {
                return MathUtils.Max(MathUtils.Max(Scale.x, Scale.y), Scale.z);
            }
        }

        private readonly Device device;
        private readonly Mesh mesh;

        public MeshNode(Device device, Mesh mesh)
        {
            Position = Vector3.Zero;
            Orientation = Quaternion.Identity;
            Scale = new Vector3(1.0f, 1.0f, 1.0f);

            this.device = device;
            this.mesh = mesh;
        }

        #region Implements IRenderable
        bool IRenderable.Render(Camera camera, Light light)
        {
            this.device.RawDevice.VertexDeclaration = this.device.MeshVertexDecl.RawDecl;

            if (this.device.RawDevice.SetStreamSource(0, this.mesh.VertexBuffer.RawBuffer, 0, this.mesh.VertexBuffer.ElementSize).IsFailure)
                return false;

            this.device.RawDevice.Indices = this.mesh.IndexBuffer.RawBuffer;

            return this.mesh.Material.Shader.Render(this.mesh.Material, light, camera, WorldMatrix,
                (pass) =>
                {
                    var drawResult = this.device.RawDevice.DrawIndexedPrimitives(D3D.PrimitiveType.TriangleList,
                                                                0, this.mesh.Attribute.VertexStart, this.mesh.Attribute.VertexCount,
                                                                this.mesh.Attribute.FaceStart * 3, this.mesh.Attribute.FaceCount);
                    return drawResult.IsSuccess;
                });
        }
        #endregion

    }
}
