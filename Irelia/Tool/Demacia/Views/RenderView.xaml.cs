using System.Windows;
using System.Windows.Controls;
using Demacia.ViewModels;
using Irelia.Render;
using System.Windows.Input;
using Size = Irelia.Render.Size;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for RenderView.xaml
    /// </summary>
    public partial class RenderView : UserControl
    {
        #region Fields
        private bool firstLoad = true;
        #endregion

        #region Properties
        private RenderViewModel ViewModel { get { return (DataContext as RenderViewModel); } }
        #endregion

        public RenderView()
        {
            InitializeComponent();

            Loaded += ((o, e) =>
            {
                if (!firstLoad)
                    return;

                if (ViewModel != null)
                    ViewModel.SetD3DImage(this.d3dImage, new Size((float)ActualWidth, (float)ActualHeight));

                firstLoad = false;
            });
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (ViewModel != null)
                ViewModel.SetRenderSize(new Size((float)sizeInfo.NewSize.Width, (float)sizeInfo.NewSize.Height));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var pos = new Vector2((float)e.GetPosition(this.image).X, (float)e.GetPosition(this.image).Y);
            if (ViewModel != null)
                ViewModel.OnMouseMove(pos, e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (ViewModel != null)
                ViewModel.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            var pos = new Vector2((float)e.GetPosition(this.image).X, (float)e.GetPosition(this.image).Y);
            if (ViewModel != null)
                ViewModel.OnMouseDown(pos, e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            var pos = new Vector2((float)e.GetPosition(this.image).X, (float)e.GetPosition(this.image).Y);
            if (ViewModel != null)
                ViewModel.OnMouseUp(pos, e);
        }
    }
}
