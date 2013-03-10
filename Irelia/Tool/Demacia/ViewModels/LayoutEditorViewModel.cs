using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using System.Xml;
using Demacia.Command;
using Demacia.Models;
using ICSharpCode.AvalonEdit.Document;
using Irelia.Gui;
using Irelia.Render;
using SysInput = System.Windows.Input;
using System.Collections.Specialized;
using System.Collections.Generic;
using Demacia.Services;

namespace Demacia.ViewModels
{
    public sealed class LayoutEditorViewModel : ViewModelBase
    {
        #region Properties
        public RenderViewModel RenderViewModel { get; private set; }
        public SceneManager SceneManager { get; private set; }
        public ObservableCollection<UIElementEditor> Elements { get; private set; }
        public UIElementEditor SelectedElement
        {
            get { return this.selectedElement; }
            set
            {
                this.selectedElement = value;
                OnPropertyChanged("SelectedElement");
            }
        }
        
        public TextDocument Document { get; set; }
        
        public bool IsDesignMode
        {
            get { return this.isDesignMode; }
            set { SetDesignMode(value); }
        }

        public bool IsXmlMode
        {
            get { return this.isXmlMode; }
            set 
            { 
                SetDesignMode(!value);
                if (IsXmlMode)
                    this.xmlUpdateTimer.Start();
                else
                    this.xmlUpdateTimer.Stop();
            }
        }

        public Vector2 MousePos { get; private set; }
        #endregion

        #region Fields
        private readonly LayoutAsset layoutAsset;
        private UIElementEditor selectedElement;
        private bool isDesignMode;
        private bool isXmlMode;
        private readonly DispatcherTimer xmlUpdateTimer = new DispatcherTimer();
        private List<ISprite> visualTree;
        #endregion

        #region Commands
        public DelegateCommand SaveCommand { get; private set; }
        #endregion

        #region Constructor
        public LayoutEditorViewModel(LayoutAsset layoutAsset, Framework framework)
        {
            this.layoutAsset = layoutAsset;

            SceneManager = new SceneManager(framework.Device, framework.AssetManager);

            UpdateVisualTree();

            RenderViewModel = new RenderViewModel(framework, SceneManager);
            RenderViewModel.SizeChanged += ((o, e) => 
                {
                    layoutAsset.Layout.Size = new Size(RenderViewModel.Width, RenderViewModel.Height);
                });

            Elements = new ObservableCollection<UIElementEditor>() { UIElementEditor.Create(null, layoutAsset.Layout, framework.AssetManager) };
            Elements.First().TreeChanged += (o, e) => UpdateVisualTree();

            SaveCommand = new DelegateCommand(() =>
                {
                    layoutAsset.Layout.Save(layoutAsset.FullPath);
                    StatusBarService.StatusText = "Layout saved: " + layoutAsset.ShortName;
                });
            Document = new TextDocument();
            IsDesignMode = true;

            this.xmlUpdateTimer.Interval = TimeSpan.FromMilliseconds(500);
            this.xmlUpdateTimer.Tick += UpdateXmlDocument;
            UpdateXmlDocument(this, EventArgs.Empty);

            RenderViewModel.MouseMove += RenderViewModel_MouseMove;
            RenderViewModel.MouseDown += RenderViewModel_MouseDown;
            RenderViewModel.MouseUp += RenderViewModel_MouseUp;
        }
        #endregion

        #region Private Methods
        private void RenderViewModel_MouseMove(object sender, RenderViewModel.MouseMoveEventArgs e)
        {
            this.layoutAsset.Layout.InjectMouseMoveEvent(e.Pos.x, e.Pos.y);

            MousePos = e.Pos;
            OnPropertyChanged("MousePos");
        }

        private void RenderViewModel_MouseDown(object sender, RenderViewModel.MouseButtonEventArgs e)
        {
            this.layoutAsset.Layout.InjectMouseDownEvent(e.Pos.x, e.Pos.y, e.Args.ChangedButton.ToIreliaButton());
        }

        private void RenderViewModel_MouseUp(object sender, RenderViewModel.MouseButtonEventArgs e)
        {
            this.layoutAsset.Layout.InjectMouseUpEvent(e.Pos.x, e.Pos.y, e.Args.ChangedButton.ToIreliaButton());
        }

        private void UpdateVisualTree()
        {
            if (this.visualTree != null)
                this.visualTree.ForEach(element => SceneManager.RemoveSprite(element));

            this.visualTree = layoutAsset.Layout.GetVisualTree();
            this.visualTree.ForEach(e => SceneManager.AddSprite(e));
        }

        private void SetDesignMode(bool enable)
        {
            this.isDesignMode = enable;
            this.isXmlMode = !enable;
            OnPropertyChanged("IsDesignMode");
            OnPropertyChanged("IsXmlMode");
        }

        private void UpdateXmlDocument(object sender, EventArgs args)
        {
            using (var sw = new StringWriter())
            {
                using (var xmlWriter = new XmlTextWriter(sw) { Formatting = Formatting.Indented })
                {
                    (this.layoutAsset.Layout as Element).WriteXml(xmlWriter);
                    Document.Text = sw.ToString();
                }
            }
        }
        #endregion
    }
}
