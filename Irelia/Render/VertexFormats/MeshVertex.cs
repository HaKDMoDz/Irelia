using System.Runtime.InteropServices;
using SlimDX.Direct3D9;
using System;

namespace Irelia.Render
{
    public class MeshVertexDecl : VertexDecl
    {
        public MeshVertexDecl(Device device)
            : base(device, new[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
        		new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
				new VertexElement(0, 20, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
				new VertexElement(0, 32, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Binormal, 0),
				new VertexElement(0, 44, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
				new VertexElement(0, 56, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            })
        {
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 UV { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Binormal { get; set; }
        public Vector3 Tangent { get; set; }
        public Color Color { get; set; }

        public MeshVertex Clone()
        {
            return new MeshVertex()
            {
                Position = Position,
                UV = UV,
                Normal = Normal,
                Tangent = Tangent,
                Binormal = Binormal,
                Color = Color
            };
        }
    }
}
