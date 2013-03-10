using System.Runtime.InteropServices;
using SlimDX.Direct3D9;

namespace Irelia.Render
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScreenVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 UV { get; set; }
    };

    public class ScreenVertexDecl : VertexDecl
    {
        public ScreenVertexDecl(Device device)
            : base(device, new[] {
                    new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                    new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                    VertexElement.VertexDeclarationEnd
                })
        {
        }
    }
}
