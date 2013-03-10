using System;
using System.Windows.Media;
using SlimDX;
using SlimDX.Direct3D9;
using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class Device : DisposableObject
    {
        #region Member fields
        private Direct3DEx direct3dEx;
        private Direct3D direct3d;
        private DeviceEx deviceEx;
        private D3D.Device device;

        // Device settings
        private Format adapterFormat = Format.X8R8G8B8;
        private Format backbufferFormat = Format.A8R8G8B8;
        private Format depthStencilFormat = Format.D16;
        private CreateFlags createFlags = CreateFlags.Multithreaded | CreateFlags.FpuPreserve;
        #endregion

        #region Private Properties
        private bool UseDeviceEx { get; set; }

        private Direct3D Direct3D
        {
            get
            {
                if (UseDeviceEx)
                    return this.direct3dEx;
                else
                    return this.direct3d;
            }
        }
        #endregion

        #region Public Properties
        public D3D.Device RawDevice
        {
            get
            {
                if (UseDeviceEx)
                    return this.deviceEx;
                else
                    return this.device;
            }
        }

        public PresentParameters PresentParams
        {
            get;
            private set;
        }

        public IRenderTarget RenderTarget
        {
            get;
            private set;
        }

        // Vertex declarations
        public VertexDecl TransformedColoredVertexDecl { get; private set; }
        public VertexDecl MeshVertexDecl { get; private set; }
        public VertexDecl TextureVertexDecl { get; private set; }
        public VertexDecl ScreenVertexDecl { get; private set; }
        
        // Vertex buffer
        public VertexBuffer<ScreenVertex> ScreenVertexBuffer { get; private set; }
        #endregion

        #region Constructors
        public Device(IntPtr handle, int width, int height)
        {
            if (handle == IntPtr.Zero)
                throw new ArgumentException("Value must be a valid window handle", "handle");

            if (GetSystemMetrics(SM_REMOTESESSION) != 0)
                throw new Exception("We can't run at all under terminal services");

            int renderingTier = (RenderCapability.Tier >> 16);
            if (renderingTier < 2)
                throw new Exception("Render capability check failed, low tier=" + renderingTier);

            // Create D3D
            try
            {
                this.direct3dEx = new Direct3DEx();
                UseDeviceEx = true;
            }
            catch
            {
                this.direct3d = new Direct3D();
                UseDeviceEx = false;
            }

            // Create device
            Result result;
            if (!Direct3D.CheckDeviceType(0, DeviceType.Hardware, this.adapterFormat, this.backbufferFormat, true, out result))
                throw new Exception("CheckDeviceType failed: " + result.ToString());

            if (!Direct3D.CheckDepthStencilMatch(0, DeviceType.Hardware, this.adapterFormat, this.backbufferFormat, this.depthStencilFormat, out result))
                throw new Exception("CheckDepthStencilMatch failed: " + result.ToString());

            Capabilities deviceCaps = Direct3D.GetDeviceCaps(0, DeviceType.Hardware);
            if ((deviceCaps.DeviceCaps & DeviceCaps.HWTransformAndLight) != 0)
                this.createFlags |= CreateFlags.HardwareVertexProcessing;
            else
                this.createFlags |= CreateFlags.SoftwareVertexProcessing;

            PresentParams = new PresentParameters()
            {
                BackBufferFormat = this.backbufferFormat,
                BackBufferCount = 1,
                BackBufferWidth = width,
                BackBufferHeight = height,
                Multisample = MultisampleType.None,
                SwapEffect = SwapEffect.Discard,
                EnableAutoDepthStencil = true,
                AutoDepthStencilFormat = this.depthStencilFormat,
                PresentFlags = PresentFlags.DiscardDepthStencil,
                PresentationInterval = PresentInterval.Immediate,
                Windowed = true,
                DeviceWindowHandle = handle,
            };

            if (UseDeviceEx)
            {
                this.deviceEx = new DeviceEx(this.direct3dEx,
                        0,
                        DeviceType.Hardware,
                        handle,
                        this.createFlags,
                        PresentParams);
            }
            else
            {
                this.device = new SlimDX.Direct3D9.Device(this.direct3d,
                    0,
                    DeviceType.Hardware,
                    handle,
                    this.createFlags,
                    PresentParams);
            }

            // Create vertex declarations
            TransformedColoredVertexDecl = new TransformedColoredVertexDecl(this);
            MeshVertexDecl = new MeshVertexDecl(this);
            TextureVertexDecl = new TexturedVertexDecl(this);
            ScreenVertexDecl = new ScreenVertexDecl(this);

            ScreenVertexBuffer = new VertexBuffer<ScreenVertex>(this);
            var vertices = new ScreenVertex[]
            {
                new ScreenVertex() { Position = new Vector3(-1.0f, -1.0f, 0.5f), UV = new Vector2(0.0f, 1.0f) },
                new ScreenVertex() { Position = new Vector3(-1.0f, 1.0f, 0.5f), UV = new Vector2(0.0f, 0.0f) },
                new ScreenVertex() { Position = new Vector3(1.0f, -1.0f, 0.5f), UV = new Vector2(1.0f, 1.0f) },
                new ScreenVertex() { Position = new Vector3(1.0f, 1.0f, 0.5f), UV = new Vector2(1.0f, 0.0f) },                  
            };
            ScreenVertexBuffer.Write(vertices);
        }
        #endregion

        #region Public Methods
        public bool SetRenderTarget(IRenderTarget renderTarget)
        {
            if (RawDevice.SetRenderTarget(0, renderTarget.TargetSurface).IsFailure)
                return false;

            RawDevice.DepthStencilSurface = renderTarget.DepthStencilSurface;
            RenderTarget = renderTarget;
            return true;
        }
        #endregion

        #region DLL imports
        // can't figure out how to access remote session status through .NET
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int GetSystemMetrics(int smIndex);
        private const int SM_REMOTESESSION = 0x1000;
        #endregion

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.device != null)
                    this.device.Dispose();
                if (this.deviceEx != null)
                    this.deviceEx.Dispose();
                if (this.direct3d != null)
                    this.direct3d.Dispose();
                if (this.direct3dEx != null)
                    this.direct3dEx.Dispose();
                if (MeshVertexDecl != null)
                    MeshVertexDecl.Dispose();
                if (TextureVertexDecl != null)
                    TextureVertexDecl.Dispose();
                if (TransformedColoredVertexDecl != null)
                    TransformedColoredVertexDecl.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
