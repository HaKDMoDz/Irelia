using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    [Serializable]
    public class MeshData
    {
        public string Material { get; set; }
        public MeshVertex[] Vertices { get; set; }
        public short[] Indices { get; set; }
        public MeshAttribute Attribute { get; set; }

        public MeshData(Mesh mesh)
        {
            Material = mesh.Material.Name;
            Vertices = mesh.VertexBuffer.Vertices.ToArray();
            Indices = mesh.IndexBuffer.Indices.ToArray();
            Attribute = mesh.Attribute;
        }
    }

    #region Struct, Attribute
    [Serializable]
    public struct MeshAttribute
    {
        public int Id { get; set; }
        public int VertexStart { get; set; }
        public int VertexCount { get; set; }
        public int FaceStart { get; set; }
        public int FaceCount { get; set; }
    }
    #endregion

    public sealed class Mesh : Resource<MeshData>
    {

        #region Private fields
        private readonly Device device;
        #endregion

        #region Properties
        public string Name { get; private set; }
        public Vector3 BoundingCenter { get; private set; }
        public float BoundingRadius { get; private set; }
        public Material Material { get; private set; }
        public ReadOnlyCollection<MeshVertex> Vertices { get { return VertexBuffer.Vertices; } }
        public VertexBuffer<MeshVertex> VertexBuffer;
        public IndexBuffer IndexBuffer;
        public MeshAttribute Attribute;
        #endregion

        #region Constructors
        private Mesh(Device device, string name, D3D.Mesh d3dMesh, AssetManager assetManager, string mediaDirectory)
            : this(name)
        {
            this.device = device;
            ReadD3DMeshData(assetManager, d3dMesh, mediaDirectory);
        }

        private Mesh(Device device, string name, MeshData data, AssetManager assetManager)
            : this(name)
        {
            this.device = device;
            ReadMeshData(data, assetManager);
        }

        private Mesh(string name)
        {
            Name = name;
        }
        #endregion

        #region Public methods
        public static Mesh CreateSphere(Device device, AssetManager assetManager, Material material, float radius, int slices, int stacks)
        {
            // Prepare D3D mesh
            D3D.Mesh d3dMesh = null;
            using (var untexMesh = D3D.Mesh.CreateSphere(device.RawDevice, radius, slices, stacks))
            {
                d3dMesh = untexMesh.Clone(device.RawDevice, 0, device.MeshVertexDecl.RawDecl.Elements);
            }

            var vertices = d3dMesh.LockVertexBuffer(D3D.LockFlags.None);
            while (vertices.Position < vertices.Length)
            {
                var oldPos = vertices.Position;
                var vertex = vertices.Read<MeshVertex>();

                vertices.Position = oldPos;
                var newVertex = new MeshVertex()
                {
                    Position = vertex.Position,
                    Normal = vertex.Normal,
                    UV = new Vector2()
                    {
                        x = MathUtils.Asin(new Radian(vertex.Normal.x)) / MathUtils.PI + 0.5f,
                        y = 1 - (MathUtils.Asin(new Radian(vertex.Normal.y)) / MathUtils.PI + 0.5f)
                    }
                };
                vertices.Write(newVertex);
            }
            d3dMesh.UnlockVertexBuffer();

            return new Mesh(device, "Sphere", d3dMesh, assetManager, null)
            {
                Material = material
            };
        }

        public static Mesh CreateFromXFile(Device device, AssetManager assetManager, string xfile, string name)
        {
            var d3dMesh = D3D.Mesh.FromFile(device.RawDevice, xfile, 0);
            return new Mesh(device, name, d3dMesh, assetManager, Path.GetDirectoryName(xfile));
        }

        public void FlipTextureV()
        {
            var vertices = VertexBuffer.Vertices.Select(v =>
                {
                    var vertex = v.Clone();
                    vertex.UV = new Vector2(vertex.UV.x, 1 - vertex.UV.y);
                    return vertex;
                }).ToArray();
            VertexBuffer.OverWrite(vertices);
        }
        #endregion

        #region Private methods
        private void ReadD3DMeshData(AssetManager assetManager, D3D.Mesh mesh, string mediaDirectory)
        {
            // Read materials
            var d3dMaterials = mesh.GetMaterials();

            if (d3dMaterials == null || d3dMaterials.Length == 0)
            {
                Material = assetManager.DefaultMaterial;
            }
            else if (d3dMaterials.Length > 0)
            {
                if (d3dMaterials.Length > 1)
                    Log.Msg(TraceLevel.Warning, "Only one material per mesh supported. The first material will be accepted as a material");
                 
                string texFile = d3dMaterials[0].TextureFileName;
                Texture texture = null;
                if (string.IsNullOrWhiteSpace(texFile) == false)
                    texture = assetManager.Load(Path.Combine(mediaDirectory, texFile)) as Texture;

                string matName = Path.ChangeExtension(Name, ".matb");
                Material = new Material(matName)
                {
                    Shader = assetManager.Load(@"Engine\UberShader.fx") as Shader,
                    DiffuseTexture = texture,
                    NormalTexture = null,
                    AmbientColor = d3dMaterials[0].MaterialD3D.Ambient.ToIreliaColor(),
                    DiffuseColor = d3dMaterials[0].MaterialD3D.Diffuse.ToIreliaColor(),
                    EmissiveColor = d3dMaterials[0].MaterialD3D.Emissive.ToIreliaColor(),
                    SpecularPower = d3dMaterials[0].MaterialD3D.Power,
                    SpecularColor = d3dMaterials[0].MaterialD3D.Specular.ToIreliaColor()
                };
            }

            bool computeNormals = ((mesh.VertexFormat & D3D.VertexFormat.Normal) == 0);

            // Clone mesh with our mesh vertex format
            var cloneMesh = mesh.Clone(this.device.RawDevice, mesh.CreationOptions, this.device.MeshVertexDecl.RawDecl.Elements);
            if (computeNormals)
            {
                var success = cloneMesh.ComputeNormals();
                if (success.IsFailure)
                    Log.Msg(TraceLevel.Error, this, "Failed compute mesh normals" + success.Description);
            }

            // Read vertex buffer
            VertexBuffer = new VertexBuffer<MeshVertex>(this.device);
            using (var stream = cloneMesh.LockVertexBuffer(D3D.LockFlags.ReadOnly))
            {
                var vertices = stream.ReadRange<MeshVertex>(cloneMesh.VertexCount);
                VertexBuffer.Write(vertices);
                cloneMesh.UnlockVertexBuffer();
            }

            // Read index buffer
            IndexBuffer = new IndexBuffer(this.device);
            using (var stream = cloneMesh.LockIndexBuffer(D3D.LockFlags.ReadOnly))
            {
                var indices = stream.ReadRange<short>(cloneMesh.FaceCount * 3);
                IndexBuffer.Write(indices);
                cloneMesh.UnlockIndexBuffer();
            }

            // Attribute (subset)
            var attributes = cloneMesh.GetAttributeTable();
            if (attributes != null && attributes.Length > 0)
            {
                Attribute = new MeshAttribute()
                {
                    Id = attributes[0].AttribId,
                    VertexStart = attributes[0].VertexStart,
                    VertexCount = attributes[0].VertexCount,
                    FaceStart = attributes[0].FaceStart,
                    FaceCount = attributes[0].FaceCount
                };
            }
            else
            {
                Attribute = new MeshAttribute()
                {
                    Id = 0,
                    VertexStart = 0,
                    VertexCount = VertexBuffer.Count,
                    FaceStart = 0,
                    FaceCount = IndexBuffer.Count / 3,
                };
            }

            BuildTangentVectors();
            ComputeBoundingSphereFromD3DMesh(cloneMesh);
        }

        private void ReadMeshData(MeshData meshData, AssetManager assetManager)
        {
            Attribute = meshData.Attribute;
            Material = assetManager.Load(meshData.Material) as Material;
            VertexBuffer = new VertexBuffer<MeshVertex>(this.device);
            VertexBuffer.Write(meshData.Vertices);
            IndexBuffer = new IndexBuffer(this.device);
            IndexBuffer.Write(meshData.Indices);

            var points = VertexBuffer.Vertices.Select((v) => v.Position.ToD3DVector3()).ToArray();
            ComputeBoundingSphere(points);
        }

        private void ComputeBoundingSphereFromD3DMesh(D3D.Mesh mesh)
        {
            var vertices = new List<SlimDX.Vector3>(mesh.VertexCount);
            
            var stream = mesh.LockVertexBuffer(D3D.LockFlags.ReadOnly);
            while (stream.Position < stream.Length)
            {
                var oldPos = stream.Position;
                vertices.Add(stream.Read<SlimDX.Vector3>());
                stream.Position = oldPos + mesh.BytesPerVertex;
            }
            mesh.UnlockVertexBuffer();

            ComputeBoundingSphere(vertices.ToArray());
        }

        private void BuildTangentVectors()
        {
            var tangentCalc = new TangentSpaceCalculator();
            var result = tangentCalc.Build(VertexBuffer.Vertices, IndexBuffer.Indices);
            VertexBuffer.OverWrite(result.ToArray());
        }

        private void ComputeBoundingSphere(SlimDX.Vector3[] points)
        {
            var sphere = SlimDX.BoundingSphere.FromPoints(points);
            BoundingCenter = sphere.Center.ToVector3();
            BoundingRadius = sphere.Radius;
        }
        #endregion

        #region Overrides Resource<T>
        protected override void OnDispose()
        {
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
            if (IndexBuffer != null)
                IndexBuffer.Dispose();
        }
        #endregion

        #region Save & Load
        public void Save(string fileName)
        {
            Save(fileName, new MeshData(this));
        }

        public static Mesh Load(Device device, string fileName, AssetManager assetManager, string name)
        {
            MeshData meshData = Load(fileName);
            return new Mesh(device, name, meshData, assetManager);
        }
        #endregion
    }

    public sealed class MeshFactory : IAssetFactory
    {
        public string[] FileExtensions
        {
            get { return new string[] { ".meshs", ".meshb" }; }
        }

        public Type AssetType { get { return typeof(Mesh); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            return Mesh.Load(device, filePath, assetManager, name);
        }
    }
}
