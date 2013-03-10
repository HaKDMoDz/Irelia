using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimDX.Direct3D9;

namespace RenderTest
{
    [TestClass()]
    public class VertexDeclTest
    {
        private Irelia.Render.Device device;

        [TestInitialize()]
        public void CreateDevice()
        {
            Window window = new Window("VertexDeclTest", 640, 480);
            this.device = new Irelia.Render.Device(window.Handle, window.Width, window.Height);
        }

        [TestMethod()]
        public void VertexDecl_ConstructorTest()
        {
            VertexElement[] vertexElems = new[] {
        		new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
        		new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
				VertexElement.VertexDeclarationEnd
        	};

            VertexDecl vdecl = new VertexDecl(device, vertexElems);
            Assert.IsNotNull(vdecl.RawDecl);
        }

        [TestMethod()]
        public void VertexDecl_CreateTest()
        {
            VertexDecl vdecl = new VertexDecl(device, null);
            Assert.IsNull(vdecl.RawDecl);

            // Vertex elements without VertexDeclarationEnd
            VertexElement[] vertexElems = new[] {
        		new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0)};
            vdecl = new VertexDecl(device, vertexElems);
            Assert.IsNull(vdecl.RawDecl);
        }
    }
}
