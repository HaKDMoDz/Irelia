using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Demacia.Command;
using Demacia.Models;
using Demacia.Services;
using Irelia.Render;

namespace Demacia.ViewModels
{
    class ImportMeshViewModel : ViewModelBase
    {
        #region Events
        public event Action RequestAccept = delegate { };
        public event Action RequestCancel = delegate { };
        #endregion

        #region Commands
        public DelegateCommand OkCommand { get; private set;  }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BrowseCommand { get; private set; }
        public DelegateCommand FlipVCommand { get; private set; }
        #endregion

        #region Fields
        private string saveName = "";
        private string meshSavePath;
        private string materialSavePath;
        private string saveFolder = "";
        private readonly Framework framework;
        #endregion

        #region Properties
        public Mesh Mesh { get; private set; }

        public RenderViewModel RenderViewModel { get; private set; }
        
        public string SaveFolder
        {
            get { return this.saveFolder; }
            set
            {
                this.saveFolder = value;
                OnPropertyChanged("SaveFolder");

                UpdateSaveFileList();
            }
        }

        public string SaveName
        {
            get { return this.saveName; }
            set
            {
                this.saveName = value;
                OnPropertyChanged("SaveName");
                
                UpdateSaveFileList();
            }
        }

        public string FileList { get; private set; }

        public MaterialEditor MaterialEditor { get; private set; }
        #endregion


        public ImportMeshViewModel(Framework framework, Mesh mesh, string initialSaveFolder)
        {
            this.framework = framework;

            // Actually, import already done. See argument "mesh" :)
            Mesh = mesh;

            var sceneManager = new SceneManager(framework.Device, framework.AssetManager);
            sceneManager.LocateCameraLookingMesh(mesh);
            sceneManager.AddRenderable(new MeshNode(framework.Device, Mesh));
            
            RenderViewModel = new RenderViewModel(framework, sceneManager);

            MaterialEditor = new MaterialEditor(mesh.Material);
            SaveName = Path.GetFileNameWithoutExtension(Mesh.Name);
            SaveFolder = initialSaveFolder;

            OkCommand = new DelegateCommand(ExecuteOk, CanExecuteOk);
            CancelCommand = new DelegateCommand(() => { RequestCancel(); });
            BrowseCommand = new DelegateCommand(ExecuteBrowse);
            FlipVCommand = new DelegateCommand(() => Mesh.FlipTextureV());
        }

        private void ExecuteOk()
        {
            // NOTE: Should rename material before saving mesh.
            Mesh.Material.Name = framework.AssetManager.GetName(this.materialSavePath);
            Mesh.Material.Save(this.materialSavePath);
            Mesh.Save(this.meshSavePath);
            RequestAccept();
        }

        private bool CanExecuteOk()
        {
            if (String.IsNullOrWhiteSpace(SaveFolder))
                return false;

            if (String.IsNullOrWhiteSpace(SaveName))
                return false;

            return true;
        }

        private void ExecuteBrowse()
        {
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = SaveFolder,
                ShowNewFolderButton = true
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            SaveFolder = dlg.SelectedPath;
        }

        private void UpdateSaveFileList()
        {
            const string meshExt = ".meshb";
            const string materialExt = ".matb";

            this.meshSavePath = Path.Combine(SaveFolder, SaveName + meshExt);
            this.materialSavePath = Path.Combine(SaveFolder, SaveName + materialExt);

            FileList = this.meshSavePath;
            FileList += "\n" + this.materialSavePath;

            OnPropertyChanged("FileList");
        }
    }
}
