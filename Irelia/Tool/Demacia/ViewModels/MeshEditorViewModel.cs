using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Demacia.Services;
using Irelia.Render;

namespace Demacia.ViewModels
{
    public class MeshEditorViewModel : ViewModelBase
    {
        #region Inner classes
        private class DebugModel : ManualRenderable<MeshVertex>
        {
            private readonly Material material;
            private readonly List<MeshVertex> lineVerticesToAdd = new List<MeshVertex>();

            public Matrix4 WorldMatrix { get; set; }

            public DebugModel(Device device, AssetManager assetManager)
                : base(device, device.MeshVertexDecl)
            {
                this.material = new Material("XZGridPlaneMaterial")
                {
                    Shader = assetManager.Load(@"Engine\UberShader.fx") as Shader,
                    Technique = "VertexColor"
                };

                WorldMatrix = Matrix4.Identity;
            }

            protected override bool Render(Camera camera, Light light)
            {
                return this.material.Shader.Render(this.material, light, camera, WorldMatrix,
                    (pass) => RenderVertices(OperationType.LineList));
            }

            public void BeginAddLine()
            {
                Debug.Assert(this.lineVerticesToAdd.Count == 0);
            }

            public void EndAddLine()
            {
                AddVertex(this.lineVerticesToAdd.ToArray());
                this.lineVerticesToAdd.Clear();
            }

            public void AddLine(Vector3 startPos, Vector3 endPos, Color startColor, Color endColor)
            {
                var vertices = new MeshVertex[2]
                {
                    new MeshVertex() { Position = startPos, Color = startColor },
                    new MeshVertex() { Position = endPos, Color = endColor }
                };
                this.lineVerticesToAdd.AddRange(vertices);
            }
        }

        private class XZGridPlane : DebugModel
        {
            public XZGridPlane(Device device, AssetManager assetManager, float halfWidth)
                : base(device, assetManager)
            {
                float halfHeight = halfWidth;

                BeginAddLine();

                // Grid
                for (float x = -halfWidth; x <= halfWidth; x += (halfWidth / 10.0f))
                {
                    AddLine(new Vector3(x, 0.0f, halfHeight), new Vector3(x, 0.0f, -halfHeight), Color.Gray, Color.Gray);
                }

                for (float z = -halfHeight; z <= halfHeight; z += (halfHeight / 10.0f))
                {
                    AddLine(new Vector3(-halfWidth, 0.0f, z), new Vector3(halfWidth, 0.0f, z), Color.Gray, Color.Gray);
                }

                // X, Z Axis
                AddLine(new Vector3(-halfWidth, 0.0f, 0.0f), new Vector3(halfWidth, 0.0f, 0.0f), Color.Gray, Color.Red);
                AddLine(new Vector3(0.0f, 0.0f, -halfHeight), new Vector3(0.0f, 0.0f, halfHeight), Color.Gray, Color.Blue);

                EndAddLine();

                WorldMatrix = Matrix4.Identity;
            }
        }
        #endregion

        #region Properties
        public RenderViewModel RenderViewModel { get; private set; }
        public bool IsNormalsShown
        {
            get { return this.isNormalsShown; }
            set
            {
                if (this.isNormalsShown == value)
                    return;

                if (value)
                    this.sceneManager.AddRenderable(normals);
                else
                    this.sceneManager.RemoveRenderable(normals);
             
                this.isNormalsShown = value;
                OnPropertyChanged("IsNormalsShown");
            }
        }

        public bool IsTangentsShown
        {
            get { return this.isTangentsShown; }
            set
            {
                if (this.isTangentsShown == value)
                    return;

                if (value)
                    this.sceneManager.AddRenderable(tangents);
                else
                    this.sceneManager.RemoveRenderable(tangents);

                this.isTangentsShown = value;
                OnPropertyChanged("IsTangentsShown");
            }
        }

        public bool IsBinormalsShown
        {
            get { return this.isBinormalsShown; }
            set
            {
                if (this.isBinormalsShown == value)
                    return;

                if (value)
                    this.sceneManager.AddRenderable(binormals);
                else
                    this.sceneManager.RemoveRenderable(binormals);

                this.isBinormalsShown = value;
                OnPropertyChanged("IsBinormalsShown");
            }
        }

        public bool IsCameraLocked
        {
            get { return this.isCameraLocked; }
            set
            {
                this.isCameraLocked = value;
                OnPropertyChanged("IsCameraLocked");
            }
        }

        public bool IsGridPlaneShown
        {
            get { return this.isGridPlaneShown; }
            set
            {
                this.isGridPlaneShown = value;
                if (value)
                    this.sceneManager.AddRenderable(this.xzPlane);
                else
                    this.sceneManager.RemoveRenderable(this.xzPlane);
                OnPropertyChanged("IsGridPlaneShown");
            }
        }
        #endregion

        #region Fields
        private readonly Device device;
        private readonly Mesh mesh;
        private readonly MeshNode meshNode;
        private readonly AssetManager assetManager;
        private bool isNormalsShown;
        private bool isTangentsShown;
        private bool isBinormalsShown;
        private Vector2 mousePos;
        private bool isCameraLocked;
        private readonly XZGridPlane xzPlane;
        private DebugModel normals;
        private DebugModel binormals;
        private DebugModel tangents;
        private DebugModel localAxes;
        private bool isGridPlaneShown;
        private readonly SceneManager sceneManager;
        #endregion

        public MeshEditorViewModel(Mesh mesh, Framework framework)
        {
            this.device = framework.Device;
            this.mesh = mesh;
            this.meshNode = new MeshNode(framework.Device, this.mesh);
            this.assetManager = framework.AssetManager;

            // Build scene
            this.sceneManager = new SceneManager(framework.Device, framework.AssetManager);
            this.sceneManager.LocateCameraLookingMesh(mesh);
            this.sceneManager.AddRenderable(meshNode);

            sceneManager.Camera.EyePos = mesh.BoundingCenter + new Vector3(0, mesh.BoundingRadius * 2.0f, mesh.BoundingRadius * -2.0f);
            sceneManager.Camera.LookAt = mesh.BoundingCenter;
            sceneManager.Light.Position = new Vector3(0.0f, mesh.BoundingRadius * 5.0f, 0.0f);
            
            // RenderView
            RenderViewModel = new RenderViewModel(framework, this.sceneManager);

            // Grid plane
            this.xzPlane = new XZGridPlane(framework.Device, framework.AssetManager, mesh.BoundingRadius * 2);

            // Local Axes
            this.localAxes = new DebugModel(this.device, this.assetManager);
            this.localAxes.BeginAddLine();
            float lineLength = mesh.BoundingRadius;
            this.localAxes.AddLine(Vector3.Zero, Vector3.UnitX * lineLength, Color.Red, Color.Red);
            this.localAxes.AddLine(Vector3.Zero, Vector3.UnitY * lineLength, Color.Green, Color.Green);
            this.localAxes.AddLine(Vector3.Zero, Vector3.UnitZ * lineLength, Color.Blue, Color.Blue);
            this.localAxes.EndAddLine();
            //RenderViewModel.AddRenderable(this.localAxes);

            CreateTangentSpaceDebugModels(mesh);

            IsCameraLocked = true;
            IsGridPlaneShown = false;

            RenderViewModel.MouseMove += RenderViewModel_MouseMove;
            RenderViewModel.MouseWheel += RenderViewModel_MouseWheel;
        }

        private void CreateTangentSpaceDebugModels(Mesh mesh)
        {
            this.normals = new DebugModel(this.device, this.assetManager);
            this.binormals = new DebugModel(this.device, this.assetManager);
            this.tangents = new DebugModel(this.device, this.assetManager);

            this.normals.BeginAddLine();
            this.binormals.BeginAddLine();
            this.tangents.BeginAddLine();

            foreach (var vertex in mesh.Vertices)
            {
                float lineLength = mesh.BoundingRadius / 20.0f;
                this.normals.AddLine(vertex.Position, vertex.Position + vertex.Normal * lineLength, Color.Green, Color.Green);
                this.binormals.AddLine(vertex.Position, vertex.Position + vertex.Binormal * lineLength, Color.Blue, Color.Blue);
                this.tangents.AddLine(vertex.Position, vertex.Position + vertex.Tangent * lineLength, Color.Red, Color.Red);
            }

            this.normals.EndAddLine();
            this.binormals.EndAddLine();
            this.tangents.EndAddLine();
        }

        private void RenderViewModel_MouseWheel(object sender, RenderViewModel.MouseWheelEventArgs e)
        {
            float moveScale = this.mesh.BoundingRadius * 0.001f;
            this.sceneManager.Camera.MoveRelative(Vector3.UnitZ * e.Args.Delta * moveScale);
        }

        private void RenderViewModel_MouseMove(object sender, RenderViewModel.MouseMoveEventArgs e)
        {
            if (e.Args.RightButton == MouseButtonState.Pressed)
            {
                var delta = e.Pos - this.mousePos;
                float scale = 0.001f;

                if (IsCameraLocked)
                {
                    this.meshNode.Orientation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(-delta.x * 0.01f));
                    this.meshNode.Orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(-delta.y * 0.01f)) * this.meshNode.Orientation;

                    this.normals.WorldMatrix = this.meshNode.WorldMatrix;
                    this.binormals.WorldMatrix = this.meshNode.WorldMatrix;
                    this.tangents.WorldMatrix = this.meshNode.WorldMatrix;
                    this.localAxes.WorldMatrix = this.meshNode.WorldMatrix;
                }
                else
                {
                    this.sceneManager.Camera.Yaw(new Radian(-delta.x * scale));
                    this.sceneManager.Camera.Pitch(new Radian(-delta.y * scale));
                }
            }

            this.mousePos = e.Pos;
        }
    }
}
