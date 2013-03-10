//using System.Collections.Generic;
//using D3D = SlimDX.Direct3D9;
//using System.Diagnostics;
//using System.IO;
//using System;

//namespace Irelia.Render
//{
//    public sealed class BoneMesh : IRenderable
//    {
//        #region Nested classes
//        // Class Frame inherits D3D.Frame
//        private sealed class Frame : D3D.Frame
//        {
//            public Matrix4 CombinedMatrix { get; set; }
//        }

//        // Class MeshContainer inherits D3D.MeshContainer
//        private sealed class MeshContainer : D3D.MeshContainer
//        {
//            public Texture[] Textures { get; set; }
//            public D3D.Material[] Materials { get; set; }
//            public D3D.Mesh SkinMesh { get; set; }
//            public Matrix4[] BoneOffsets { get; set; }
//        }

//        // Class AllocateHierarchy implements D3D.IAllocateHierarchy
//        private sealed class AllocateHierarchy : D3D.IAllocateHierarchy
//        {
//            private readonly Device device;
//            private readonly AssetManager assetManager;
//            private readonly string mediaDirectory;

//            public AllocateHierarchy(Device device, AssetManager assetManager, string mediaDirectory)
//            {
//                this.device = device;
//                this.assetManager = assetManager;
//                this.mediaDirectory = mediaDirectory;
//            }

//            public D3D.Frame CreateFrame(string name)
//            {
//                return new Frame()
//                {
//                    Name = name,
//                    TransformationMatrix = SlimDX.Matrix.Identity,
//                    CombinedMatrix = Matrix4.Identity,
//                    MeshContainer = null,
//                    Sibling = null,
//                    FirstChild = null
//                };
//            }

//            public void DestroyFrame(D3D.Frame frame)
//            {
//                frame.Dispose();
//            }

//            public D3D.MeshContainer CreateMeshContainer(string name, D3D.MeshData meshData, D3D.ExtendedMaterial[] materials, D3D.EffectInstance[] effectInstances, int[] adjacency, D3D.SkinInfo skinInfo)
//            {
//                if (meshData.Type != D3D.MeshDataType.Mesh)
//                    throw new ArgumentException("Not allowed mesh data type: " + meshData.Type, "meshData");

//                var meshContainer = new MeshContainer()
//                {
//                    Name = name,
//                    MeshData = meshData,
//                    SkinInfo = skinInfo
//                };
//                meshContainer.SetMaterials(materials);
//                meshContainer.SetAdjacency(adjacency);
//                meshContainer.SetEffects(effectInstances);

//                if (materials.Length > 0)
//                {
//                    // Load materials including textures
//                    meshContainer.Materials = new D3D.Material[materials.Length];
//                    meshContainer.Textures = new Texture[materials.Length];

//                    for (int i = 0; i < materials.Length; ++i)
//                    {
//                        meshContainer.Materials[i] = materials[i].MaterialD3D;
//                        if (string.IsNullOrWhiteSpace(materials[i].TextureFileName) == false)
//                        {
//                            meshContainer.Textures[i] = this.assetManager.Load(Path.Combine(this.mediaDirectory, materials[i].TextureFileName)) as Texture;
//                        }
//                    }
//                }
//                else
//                {
//                    // Make a defauilt material
//                    meshContainer.Materials = new D3D.Material[1];
//                    meshContainer.Materials[0].Diffuse = new SlimDX.Color4(0.5f, 0.5f, 0.5f);
//                    meshContainer.Materials[0].Specular = meshContainer.Materials[0].Diffuse;
//                    meshContainer.Textures = new Texture[1];
//                }

//                if (meshContainer.SkinInfo != null)
//                {
//                    // Save offset matrices
//                    meshContainer.BoneOffsets = new Matrix4[skinInfo.BoneCount];
//                    for (int i = 0; i < skinInfo.BoneCount; ++i)
//                    {
//                        meshContainer.BoneOffsets[i] = meshContainer.SkinInfo.GetBoneOffsetMatrix(i).ToMatrix4();
//                    }
//                }

//                if (meshContainer.GetEffects() != null && meshContainer.GetEffects().Length > 0)
//                {
//                    Log.Msg(TraceLevel.Warning, this, "Mesh references an effect file which will be ignored");
//                }

//                return meshContainer;
//            }

//            public void DestroyMeshContainer(D3D.MeshContainer container_)
//            {
//                MeshContainer container = container_ as MeshContainer;
                
//                foreach (var texture in container.Textures)
//                {
//                    if (texture != null)
//                        texture.Dispose();
//                }
//                if (container.SkinMesh != null)
//                {
//                    container.SkinMesh.Dispose();
//                }

//                container.Dispose();
//            }
//        }
//        #endregion

//        #region Properties
//        public string Name { get; private set; }
//        public int VertexCount { get; private set; }
//        public int FaceCount { get; private set; }
//        public IList<string> BoneNames { get; private set; }
//        public IList<string> AllBoneNames
//        {
//            get
//            {
//                IList<string> names = new List<string>();
//                BuildBoneNames(this.rootFrame, names);
//                return names;
//            }
//        }
//        public int AnimationSet
//        {
//            get { return this.animationSet; }
//            set
//            {
//                if (value == this.animationSet)
//                    return;

//                if (this.animController == null)
//                    throw new InvalidOperationException("Animation is not available");

//                if (0 > value || value >= AnimationSetCount)
//                    throw new ArgumentOutOfRangeException("AnimationSet", "value(" + value + ") must be in range 0~" + (AnimationSetCount - 1));

//                // Alternate tracks for transitions, assign to the correct track
//                var animSet = this.animController.GetAnimationSet(value);
//                var newTrack = (this.currentTrack == 0? 1 : 0);
//                this.animController.SetTrackAnimationSet(currentTrack, animSet);
//                animSet.Dispose();

//                this.animController.UnkeyAllTrackEvents(this.currentTrack);
//                this.animController.UnkeyAllTrackEvents(newTrack);
//                float transitionTime = 0.25f;
//                this.animController.KeyTrackEnable(this.currentTrack, false, this.currentTime + transitionTime);
//                this.animController.KeyTrackSpeed(this.currentTrack, 0.0f, this.currentTime, transitionTime, D3D.TransitionType.Linear);
//                this.animController.KeyTrackWeight(this.currentTrack, 0.0f, this.currentTime, transitionTime, D3D.TransitionType.Linear);

//                this.animController.EnableTrack(newTrack);
//                this.animController.KeyTrackSpeed(newTrack, 1.0f, this.currentTime, transitionTime, D3D.TransitionType.Linear);
//                this.animController.KeyTrackWeight(newTrack, 1.0f, this.currentTime, transitionTime, D3D.TransitionType.Linear);

//                this.currentTrack = newTrack;
//                this.animationSet = value;
//            }
//        }

//        public int AnimationSetCount
//        {
//            get
//            {
//                if (this.animController == null)
//                    return 0;

//                return this.animController.MaxAnimationSets;
//            }
//        }
//        #endregion

//        #region Private fields
//        private readonly Device device;
//        private readonly Frame rootFrame;
//        private readonly Shader shader;
//        private readonly D3D.AnimationController animController;
//        private readonly SlimDX.Matrix[] boneMatrices;
//        private readonly SlimDX.Matrix[] boneInvTransposedMatrices;
//        private int maxBones;
//        private MeshContainer firstMeshContainer;
//        private int animationSet;
//        private int currentTrack;
//        private float currentTime;
//        #endregion

//        #region Constructors
//        public BoneMesh(Device device, AssetManager assetManager, string xfile, string name)
//        {
//            this.device = device;
//            Name = name;
//            this.shader = assetManager.Load(@"Engine\UberShader.fx") as Shader;

//            var allocHierarchy = new AllocateHierarchy(device, assetManager, Path.GetDirectoryName(xfile));
//            this.rootFrame = D3D.Frame.LoadHierarchyFromX(device.RawDevice, xfile, 0, allocHierarchy, null, out this.animController) as Frame;
//            this.BoneNames = new List<string>();
            
//            // Bones for skinning exist
//            if (this.rootFrame != null)
//            {
//                CreateBoneMatrices(this.rootFrame);
//                this.boneMatrices = new SlimDX.Matrix[this.maxBones];
//                this.boneInvTransposedMatrices = new SlimDX.Matrix[this.maxBones];
//                for (int i = 0; i < this.boneInvTransposedMatrices.Length; ++i)
//                {
//                    this.boneInvTransposedMatrices[i] = SlimDX.Matrix.Identity;
//                }
//            }
//        }
//        #endregion

//        #region Public methods
//        public bool Animate(float elapsedTime)
//        {
//            this.currentTime += elapsedTime / 1000.0f;

//            if (this.animController != null &&
//                this.animController.AdvanceTime(elapsedTime / 1000.0f, null).IsFailure)
//                return false;

//            UpdateFrameMatrices(this.rootFrame, Matrix4.Identity);

//            // Update the vertices if thers'a skinned mesh
//            var meshContainer = this.firstMeshContainer;
//            if (meshContainer != null && meshContainer.SkinInfo != null)
//            {
//                for (int i = 0; i < meshContainer.SkinInfo.BoneCount; ++i)
//                {
//                    string boneName = meshContainer.SkinInfo.GetBoneName(i);
//                    Frame boneFrame = this.rootFrame.FindChild(boneName) as Frame;
//                    this.boneMatrices[i] = (meshContainer.BoneOffsets[i] * boneFrame.CombinedMatrix).ToD3DMatrix();
//                }

//                var source = meshContainer.MeshData.Mesh.LockVertexBuffer(D3D.LockFlags.ReadOnly);
//                var dest = meshContainer.SkinMesh.LockVertexBuffer(0);
//                var result = meshContainer.SkinInfo.UpdateSkinnedMesh(this.boneMatrices, this.boneMatrices, source, dest);
//                meshContainer.SkinMesh.UnlockVertexBuffer();
//                meshContainer.MeshData.Mesh.UnlockVertexBuffer();
//                if (result.IsFailure)
//                    return false;
//            }

//            return true;
//        }

//        public string GetAnimationSetName(int index)
//        {
//            if (this.animController == null)
//                throw new InvalidOperationException("Animation is not available");

//            if (index < 0 || index >= AnimationSetCount)
//                throw new ArgumentOutOfRangeException("index", "value(" + index + ") must be in range 0~" + (AnimationSetCount - 1));

//            var animSet = this.animController.GetAnimationSet(index);
//            return animSet.Name;
//        }
//        #endregion

//        #region Private methods
//        private void CreateBoneMatrices(Frame frame)
//        {
//            MeshContainer meshContainer = frame.MeshContainer as MeshContainer;

//            if (meshContainer != null)
//            {
//                VertexCount += meshContainer.MeshData.Mesh.VertexCount;
//                FaceCount += meshContainer.MeshData.Mesh.FaceCount;

//                // Save the first mesh in the hierarchy for animation purpose
//                if (this.firstMeshContainer == null)
//                    this.firstMeshContainer = meshContainer;

//                // Setup the bone matrices if skinning info exists
//                if (meshContainer.SkinInfo != null && meshContainer.MeshData.Mesh != null)
//                {
//                    var declaration = meshContainer.MeshData.Mesh.GetDeclaration();
//                    meshContainer.SkinMesh = meshContainer.MeshData.Mesh.Clone(this.device.RawDevice, 0, declaration);

//                    // Total bones determine size of bone matrix array
//                    this.maxBones = System.Math.Max(this.maxBones, meshContainer.SkinInfo.BoneCount);

//                    // Calculate its matrix for each bone
//                    for (int i = 0; i < meshContainer.SkinInfo.BoneCount; ++i)
//                    {
//                        string boneName = meshContainer.SkinInfo.GetBoneName(i);
//                        Frame boneFrame = this.rootFrame.FindChild(boneName) as Frame;

//                        BoneNames.Add(boneName);
//                    }
//                }
//            }

//            if (frame.Sibling != null)
//                CreateBoneMatrices(frame.Sibling as Frame);

//            if (frame.FirstChild != null)
//                CreateBoneMatrices(frame.FirstChild as Frame);
//        }

//        private void BuildBoneNames(D3D.Frame frame, IList<string> names)
//        {
//            if (frame.Name != null)
//                names.Add(frame.Name);
//            if (frame.Sibling != null)
//                BuildBoneNames(frame.Sibling, names);
//            if (frame.FirstChild != null)
//                BuildBoneNames(frame.FirstChild, names);
//        }

//        private bool RenderFrame(D3D.Frame frame, Camera camera, Light light)
//        {
//            D3D.MeshContainer meshContainer = frame.MeshContainer;
//            while (meshContainer != null)
//            {
//                if (RenderMeshContainer(meshContainer, frame, camera, light) == false)
//                    return false;

//                meshContainer = meshContainer.NextMeshContainer;
//            }

//            if (frame.Sibling != null && RenderFrame(frame.Sibling, camera, light) == false)
//                return false;

//            if (frame.FirstChild != null && RenderFrame(frame.FirstChild, camera, light) == false)
//                return false;

//            return true;
//        }

//        private bool RenderMeshContainer(D3D.MeshContainer meshContainerBase, D3D.Frame frameBase, Camera camera, Light light)
//        {
//            Frame frame = frameBase as Frame;
//            MeshContainer meshContainer = meshContainerBase as MeshContainer;
//            var shaderParams = this.shader.Params;

//            var worldMatrix = Matrix4.Identity;
//            shaderParams["m_World"].SetValue(worldMatrix);
//            shaderParams["m_WorldIT"].SetValue(worldMatrix.Invert().Transpose());
//            shaderParams["m_WVP"].SetValue(worldMatrix * camera.ViewMatrix * camera.ProjectionMatrix);
//            shaderParams["g_vLightPos"].SetValue(light.Position);

//            for (int i = 0; i < meshContainer.Materials.Length; ++i)
//            {
//                if (meshContainer.Textures[i] != null)
//                    shaderParams["DiffuseTexture"].SetValue(meshContainer.Textures[i]);

//                bool ret = this.shader.Render((pass) =>
//                {
//                    D3D.Mesh mesh = (meshContainer.SkinMesh != null) ? meshContainer.SkinMesh : meshContainer.MeshData.Mesh;
//                    return mesh.DrawSubset(i).IsSuccess;
//                });

//                if (ret == false)
//                    return false;
//            }

//            return true;
//        }

//        private void UpdateFrameMatrices(D3D.Frame frameBase, Matrix4 parentMatrix)
//        {
//            Frame frame = frameBase as Frame;

//            frame.CombinedMatrix = frame.TransformationMatrix.ToMatrix4() * parentMatrix;

//            if (frame.Sibling != null)
//                UpdateFrameMatrices(frame.Sibling, parentMatrix);

//            if (frame.FirstChild != null)
//                UpdateFrameMatrices(frame.FirstChild, frame.CombinedMatrix);
//        }
//        #endregion

//        #region Implements IRenderable
//        bool IRenderable.Render(Camera camera, Light light)
//        {
//            if (this.rootFrame == null)
//                return false;

//            return RenderFrame(this.rootFrame, camera, light);
//        }
//        #endregion
//    }
//}
