using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LibNoise;
using LibNoise.Models;
using D3D = SlimDX.Direct3D9;
using System.Linq;

namespace Irelia.Render
{
    internal sealed class HeightMap
    {
        public int Width { get; private set; }
        public int Length { get; private set; }
        public int MaxHeight { get; private set; }

        private readonly float[] heights;

        public HeightMap(int width, int length, int maxHeight)
        {
            this.heights = new float[width * length];

            // Init perlin noise library
            Perlin perlin = new Perlin();
            perlin.Seed = (new Random()).Next() % 1000;
            perlin.Frequency = 0.03f;
            perlin.OctaveCount = 5;
            perlin.Persistence = 0.05f;

            // Create the plane model
            Plane planeModel = new Plane(perlin);

            // Fill every point in the noise map with the output values from the model
            for (int z = 0; z < length; ++z)
            {
                for (int x = 0; x < width; ++x)
                {
                    float value = (float)planeModel.GetValue(x, z) * maxHeight;
                    heights[z * width + x] = System.Math.Max(0.0f, value);
                }
            }

            Width = width;
            Length = length;
            MaxHeight = maxHeight;
        }

        public float GetHeight(int x, int z)
        {
            if (x < 0 || x >= Width || z < 0 || z >= Length)
                throw new ArgumentOutOfRangeException();

            return this.heights[x + z * Width];
        }
    }

    internal sealed class Patch : DisposableObject
    {
        private readonly D3D.Mesh mesh;
        private readonly D3D.VertexFormat terrainFvf = D3D.VertexFormat.Position | D3D.VertexFormat.Normal | D3D.VertexFormat.Texture1;

        [StructLayout(LayoutKind.Sequential)]
        private struct TerrainVertex
        {
            private readonly Vector3 position;
            private readonly Vector3 normal;
            private readonly Vector2 uv;

            public TerrainVertex(Vector3 pos, Vector2 uv)
            {
                this.position = pos;
                this.normal = Vector3.Zero; // will be calculated by ComputeNormals()
                this.uv = uv;
            }
        }

        public Patch(Device device, HeightMap heightMap, int left, int top, int right, int bottom)
        {
            int width = right - left;
            int height = bottom - top;
            int numFace = (int)(width * height * 2);
            int numVertex = (int)((width + 1) * (height + 1));
            this.mesh = new D3D.Mesh(device.RawDevice, numFace, numVertex, 0, this.terrainFvf);

            // Create terrain vertices
            var vertices = this.mesh.LockVertexBuffer(D3D.LockFlags.None);
            for (int z = top, z0 = 0; z <= bottom; ++z, ++z0)
            {
                for (int x = left, x0 = 0; x <= right; ++x, ++x0)
                {
                    var pos = new Vector3(x, heightMap.GetHeight(x, z), z);
                    var uv = new Vector2(x * 0.2f, z * 0.2f);
                    vertices.Write(new TerrainVertex(pos, uv));
                }
            }
            this.mesh.UnlockVertexBuffer();

            // Calculate terrain indices
            var indices = this.mesh.LockIndexBuffer(D3D.LockFlags.None);
            for (int z = top, z0 = 0; z < bottom; ++z, ++z0)
            {
                for (int x = left, x0 = 0; x < right; ++x, ++x0)
                {
                    // Triangle 1
                    indices.Write((short)(z0 * (width + 1) + x0));
                    indices.Write((short)((z0 + 1) * (width + 1) + x0));
                    indices.Write((short)(z0 * (width + 1) + x0 + 1));

                    // Triangle 2
                    indices.Write((short)(z0 * (width + 1) + x0 + 1));
                    indices.Write((short)((z0 + 1) * (width + 1) + x0));
                    indices.Write((short)((z0 + 1) * (width + 1) + x0 + 1));
                }
            }
            this.mesh.UnlockIndexBuffer();

            // Set attributes
            var attributes = this.mesh.LockAttributeBuffer(D3D.LockFlags.None);
            for (int z = top; z < bottom; ++z)
            {
                for (int x = left; x < right; ++x)
                {
                    // Calculate vertices based on height
                    int subset;
                    if (heightMap.GetHeight(x, z) == 0.0f)
                        subset = 0;
                    else if (heightMap.GetHeight(x, z) <= heightMap.MaxHeight * 0.6f)
                        subset = 1;
                    else
                        subset = 2;

                    attributes.Write(subset);
                    attributes.Write(subset);
                }
            }
            this.mesh.UnlockAttributeBuffer();

            // Compute normal for the terrain
            this.mesh.ComputeNormals();
        }

        public bool Render(int subset)
        {
            return (this.mesh.DrawSubset(subset).IsSuccess);
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.mesh != null)
                    this.mesh.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }

    public sealed class Terrain : IRenderable
    {
        #region Public properties
        public int Width { get { return this.heightMap.Width; } }
        public int Length { get { return this.heightMap.Length; } }
        public int MaxHeight { get { return this.heightMap.MaxHeight; } }
        #endregion

        #region Private fields
        private readonly HeightMap heightMap;
        private readonly int numPatches = 3;
        private readonly IList<Patch> patches = new List<Patch>();
        private readonly Device device;
        private readonly List<Material> materials;
        #endregion

        public Terrain(Device device, AssetManager assetManager,  int width, int length, int maxHeight, string[] textures)
        {
            this.heightMap = new HeightMap(width, length, maxHeight);

            CreatePatches(device);

            this.materials = textures.Select((texture, i) =>
                new Material("Terrain" + i)
                {
                    Shader = assetManager.Load(@"Engine\UberShader.fx") as Shader,
                    DiffuseTexture = assetManager.Load(texture) as Texture
                }).ToList();
            this.device = device;
        }

        public float GetHeight(int x, int z)
        {
            return this.heightMap.GetHeight(x, z);
        }

        private void CreatePatches(Device device)
        {
            Debug.Assert(this.heightMap != null);

            for (int y = 0; y < this.numPatches; ++y)
            {
                for (int x = 0; x < this.numPatches; ++x)
                {
                    int left = x * (Width - 1) / this.numPatches;
                    int top = y * (Length - 1) / this.numPatches;
                    int right = (x + 1) * (Width - 1) / this.numPatches;
                    int bottom = (y + 1) * (Length - 1) / this.numPatches;

                    Patch p = new Patch(device, this.heightMap, left, top, right, bottom);
                    this.patches.Add(p);
                }
            }
        }

        #region Implements IRenderable
        bool IRenderable.Render(Camera camera, Light light)
        {
            for (int i = 0; i < this.materials.Count; ++i)
            {
                var material = this.materials[i];
                bool ret = material.Shader.Render(material, light, camera, Matrix4.Identity,
                    (pass) =>
                    {
                        foreach (var patch in this.patches)
                        {
                            if (patch.Render(i) == false)
                                return false;
                        }
                        return true;
                    });
                if (ret == false)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
