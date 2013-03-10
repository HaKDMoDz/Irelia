using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Irelia.Render;
using Color = Irelia.Render.Color;
using System.Collections.Generic;
using System.Windows.Input;
using SysInput = System.Windows.Input;
using Size = Irelia.Render.Size;

namespace Demacia.ViewModels
{
    public class RenderViewModel : ViewModelBase
    {
        #region Events
        public event EventHandler SizeChanged = delegate { };
        #endregion

        #region Fields
        private readonly Framework framework;
        private D3DImage d3dImage;
        private TextureRenderTarget renderTarget;
        private Size pendingSize;
        private Timer timer = new Timer();
        private static readonly int resizePeriod = 100;
        private readonly SceneManager sceneManager;
        #endregion

        #region Properties
        public float Width 
        { 
            get { return (this.renderTarget != null)? this.renderTarget.Size.Width : 0; }
        }

        public float Height
        {
            get { return (this.renderTarget != null) ? this.renderTarget.Size.Height : 0; }
        }
        #endregion

        #region Constructor
        public RenderViewModel(Framework fw, SceneManager sceneManager)
        {
            this.framework = fw;
            this.sceneManager = sceneManager;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        ~RenderViewModel()
        {
            //CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }
        #endregion

        #region Public Methods
        public void SetD3DImage(D3DImage d3dImage, Size size)
        {
            this.d3dImage = d3dImage;

            CreateRenderTarget(size);
        }

        public void SetRenderSize(Size size)
        {
            this.pendingSize = size;
            this.timer.Start(true);
        }

        public void OnMouseMove(Vector2 pos, MouseEventArgs e)
        {
            MouseMove(this, new MouseMoveEventArgs() { Pos = pos, Args = e });
        }

        public void OnMouseWheel(SysInput.MouseWheelEventArgs e)
        {
            MouseWheel(this, new MouseWheelEventArgs() { Args = e });
        }

        public void OnMouseDown(Vector2 pos, SysInput.MouseButtonEventArgs e)
        {
            MouseDown(this, new MouseButtonEventArgs() { Pos = pos, Args = e });
        }

        public void OnMouseUp(Vector2 pos, SysInput.MouseButtonEventArgs e)
        {
            MouseUp(this, new MouseButtonEventArgs() { Pos = pos, Args = e });
        }
        #endregion

        #region Private Methods
        private void CreateRenderTarget(Size size)
        {
            //if (this.renderTarget != null)
            //    this.renderTarget.Dispose();

            size.Width = Math.Max(1, size.Width);
            size.Height = Math.Max(1, size.Height);

            this.renderTarget = new TextureRenderTarget(this.framework.Device, size)
            {
                ClearBackGround = true,
                ClearColor = Color.Black
            };

            this.d3dImage.Lock();
            this.d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, this.renderTarget.TargetSurface.ComPointer);
            this.d3dImage.Unlock();

            this.sceneManager.Camera.AspectRatio = (float)this.renderTarget.Size.Width / (float)this.renderTarget.Size.Height;

            SizeChanged(this, EventArgs.Empty);
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (this.renderTarget == null)
                return;

            if (this.timer.Elapsed > resizePeriod)
            {
                CreateRenderTarget(this.pendingSize);
                this.timer.Stop(true);
            }

            this.d3dImage.Lock();

            this.sceneManager.Render(this.framework.Renderer, this.renderTarget, false);

            this.d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, this.renderTarget.TargetSurface.ComPointer);
            this.d3dImage.AddDirtyRect(new Int32Rect(0, 0, this.d3dImage.PixelWidth, this.d3dImage.PixelHeight));
            this.d3dImage.Unlock();
        }
        #endregion

        #region Events
        public class MouseMoveEventArgs : EventArgs
        {
            public Vector2 Pos { get; set; }
            public MouseEventArgs Args { get; set; }
        }
        public event EventHandler<MouseMoveEventArgs> MouseMove = delegate { };

        public class MouseWheelEventArgs : EventArgs
        {
            public System.Windows.Input.MouseWheelEventArgs Args { get; set; }
        }
        public event EventHandler<MouseWheelEventArgs> MouseWheel = delegate { };

        public class MouseButtonEventArgs : EventArgs
        {
            public Vector2 Pos { get; set; }
            public System.Windows.Input.MouseButtonEventArgs Args { get; set; }
        }
        public event EventHandler<MouseButtonEventArgs> MouseDown = delegate { };
        public event EventHandler<MouseButtonEventArgs> MouseUp = delegate { };
        #endregion
    }
}
