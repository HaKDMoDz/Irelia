using System.Runtime.InteropServices;
using SlimDX.Direct3D9;

namespace Irelia.Render
{
    public class TransformedColoredVertexDecl : VertexDecl
    {
        public TransformedColoredVertexDecl(Device device)
            : base(device, new[] 
            {
        		new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
        		new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
				VertexElement.VertexDeclarationEnd
        	})
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TransformedColoredVertex
    {
        public Vector4 Position { get; set; }
        public int Color { get; set; }
    }
}
