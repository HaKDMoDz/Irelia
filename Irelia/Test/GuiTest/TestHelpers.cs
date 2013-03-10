using System;
using System.Xml;
using Irelia.Gui;
using Irelia.Render;
using D3D = SlimDX.Direct3D9;

namespace GuiTest
{
    internal static class TestHelpers
    {
        internal static Device device;

        internal static Device GetDevice()
        {
            if (device == null)
            {
                Window window = new Window("TestHelpers.GetDevice", 640, 480);
                device = new Device(window.Handle, window.Width, window.Height);
            }
            return device;
        }

        // UI RootElement
        internal class RootElement : IElement
        {
            public Rectangle AbsRect
            {
                get { return this.absoluteRect; }
                set { this.absoluteRect = value; }
            }
            public event EventHandler PositionChanged = delegate { };
            public event EventHandler SizeChanged = delegate { };
            public void SaveXml(XmlWriter writer) { }
            public void LoadXml(XmlReader reader) { }

            private Rectangle absoluteRect = new Rectangle(0.0f, 0.0f, 100.0f, 100.0f);
        }

        internal static void RenderSprite(this Device device, Action<SpriteRenderer> action)
        {
            var spriteRenderer = new SpriteRenderer(device);

            try
            {
                device.RawDevice.BeginScene();
                spriteRenderer.RawSprite.Begin(D3D.SpriteFlags.AlphaBlend);
            
                action(spriteRenderer);
            }
            finally
            {
                spriteRenderer.RawSprite.End();
                device.RawDevice.EndScene();
            }
        }
    }
}
