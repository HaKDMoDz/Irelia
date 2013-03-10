using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irelia.Render
{
    public class TangentSpaceCalculator
    {
        private class VertexInfo
        {
            public Vector3 Pos { get; set; }
            public Vector3 Norm { get; set; }
            public Vector2 UV { get; set; }
            public Vector3 Tangent { get; set; }
            public Vector3 Binormal { get; set; }
            // Which way the tangent space is oriented (+1 / -1) (set on first time found)
            public int Parity { get; set; }

            public VertexInfo()
            {
                Tangent = Vector3.Zero;
                Binormal = Vector3.Zero;
                Parity = 0;
            }
        }

        private VertexInfo[] vertexArray;

        public List<MeshVertex> Build(IList<MeshVertex> vertices, IList<short> indices)
        {
            // Pull out all the vertex components we'll need
            PopulateVertexArray(vertices);

            // Now process the faces and calculate / add their contributions
            ProcessFaces(indices);

            // Now normalize & orthogonalize
            NormalizeVertices();

            var result = this.vertexArray.Select(v =>
                {
                    return new MeshVertex()
                    {
                        Position = v.Pos,
                        Normal = v.Norm,
                        UV = v.UV,
                        Tangent = v.Tangent,
                        Binormal = v.Binormal
                    };
                }).ToList();
            return result;
        }

        private void PopulateVertexArray(IList<MeshVertex> vertices)
        {
            this.vertexArray = vertices.Select(v => 
                {
                    return new VertexInfo()
                    {
                        Pos = v.Position,
                        Norm = v.Normal,
                        UV = v.UV
                    };
                }).ToArray();
        }

        private void ProcessFaces(IList<short> indices)
        {
            // Lopp through all faces to calculate the tangents and normals
            int i = 0;
            for (var f = 0; f < indices.Count / 3; ++f)
            {
                // Current triangle
                short[] vertInd = new short[3]
                {
                    indices[i++], indices[i++], indices[i++]
                };
       
                // For each triangle
                //  Calculate tangent & binormal per triangle
                //  Note these are not normalized, are weighted by UV area
                Vector3 faceTsU, faceTsV, faceNorm;
                CalculateFaceTangentSpace(vertInd, out faceTsU, out faceTsV, out faceNorm);

                //// Skil invalid UV space triangles
                if (faceTsU.IsZeroLength || faceTsV.IsZeroLength)
                    continue;

                AddFaceTangentSpaceToVertices(f, vertInd, faceTsU, faceTsV, faceNorm);
            }
        }

        private void CalculateFaceTangentSpace(short[] vertInd, out Vector3 tsU, out Vector3 tsV, out Vector3 tsN)
        {
            VertexInfo v0 = this.vertexArray[vertInd[0]];
            VertexInfo v1 = this.vertexArray[vertInd[1]];
            VertexInfo v2 = this.vertexArray[vertInd[2]];
            Vector2 deltaUV1 = v1.UV - v0.UV;
            Vector2 deltaUV2 = v2.UV - v0.UV;
            Vector3 deltaPos1 = v1.Pos - v0.Pos;
            Vector3 deltaPos2 = v2.Pos - v0.Pos;

            // Face normal
            tsN = Vector3.Cross(deltaPos1, deltaPos2).Normalize();

            float uvarea = Vector2.Cross(deltaUV1, deltaUV2) * 0.5f;
            if (MathUtils.IsNearZero(uvarea, 0.0f))
            {
                // No tangent, null uv area
                tsU = tsV = Vector3.Zero;
            }
            else
            {
                // Normalize by uvarea
                float a = deltaUV2.y / uvarea;
                float b = -deltaUV1.y / uvarea;
                float c = -deltaUV2.x / uvarea;
                float d = deltaUV1.x / uvarea;

                tsU = ((deltaPos1 * a) + (deltaPos2 * b)).Normalize();
                tsV = ((deltaPos1 * c) + (deltaPos2 * d)).Normalize();

                float abs_uvarea = MathUtils.Abs(uvarea);
                tsU *= abs_uvarea;
                tsV *= abs_uvarea;

                // Tangent (tsU) and binormal (tsV) are now weighted by uv area
            }
        }

        private void AddFaceTangentSpaceToVertices(int faceIndex, short[] vertInd, Vector3 faceTsU, Vector3 faceTsV, Vector3 faceNorm)
        {
            // Calculate parity for this triangle
            int faceParity = CalculateParity(faceTsU, faceTsV, faceNorm);

            // Now add these to each vertex referenced by the face
            for (int v = 0; v < 3; ++v)
            {
                // Index 0 is vertex we're calculating, 1 and 2 are the others

                // We want too re-weight these by the angle the face makes with the vertex
                // in order to obtain tesselation-independent results
                float angleWeight = CalculateAngleWeight(vertInd[v], vertInd[(v + 1) % 3], vertInd[(v + 2) % 3]).Value;

                VertexInfo vertex = this.vertexArray[vertInd[v]];
                if (vertex.Parity == 0)
                    vertex.Parity = faceParity;

                // Add weighted tangent & binormal
                vertex.Tangent += (faceTsU * angleWeight);
                vertex.Binormal += (faceTsV * angleWeight);
            }
        }

        private int CalculateParity(Vector3 u, Vector3 v, Vector3 n)
        {
            if (Vector3.Dot(Vector3.Cross(u, v), n) >= 0.0f)
                return -1;
            else
                return 1;
        }

        private Radian CalculateAngleWeight(int vidx0, int vidx1, int vidx2)
        {
            VertexInfo v0 = this.vertexArray[vidx0];
            VertexInfo v1 = this.vertexArray[vidx1];
            VertexInfo v2 = this.vertexArray[vidx2];

            Vector3 diff0 = v1.Pos - v0.Pos;
            Vector3 diff1 = v2.Pos - v1.Pos;

            // Weight is just the angle - larget == better
            return Radian.AngleBetween(diff0, diff1);
        }

        private void NormalizeVertices()
        {
            foreach (var vertex in this.vertexArray)
            {
                vertex.Tangent = vertex.Tangent.Normalize();
                vertex.Binormal = vertex.Binormal.Normalize();

                // Orthogonalise with the vertex normal since it's currently
                // orthogonal with the face normals, but will be close to ortho
                // Apply Gram-Schmidt orthogonalise
                Vector3 temp = vertex.Tangent;
                vertex.Tangent = temp - (vertex.Norm * Vector3.Dot(vertex.Norm, temp));

                temp = vertex.Binormal;
                vertex.Binormal = temp - (vertex.Norm * Vector3.Dot(vertex.Norm, temp));

                // Renormalize
                vertex.Tangent = vertex.Tangent.Normalize();
                vertex.Binormal = vertex.Binormal.Normalize();
            }
        }
    }
}
