using System.Collections.Generic;

namespace Irelia.Render
{
    public sealed class SceneManager
    {
        private readonly IList<IRenderable> renderables = new List<IRenderable>();
        public readonly IList<ISprite> sprites = new List<ISprite>();
        private readonly Device device;
        private readonly AssetManager assetManager;
        private World world;

        public Camera Camera { get; set; }
        public Light Light { get; private set; }
        public World World
        {
            get
            {
                return this.world;
            }
            set
            {
                if (this.world != null)
                {
                    RemoveRenderable(this.world.Skybox, false);
                    RemoveRenderable(this.world.Glow, false);
                    RemoveRenderable(this.world.DarkenBorder, false);
                }
                
                this.world = value;

                if (this.world != null)
                {
                    AddRenderable(this.world.Skybox, false, true);
                    AddRenderable(this.world.Glow, false);
                    AddRenderable(this.world.DarkenBorder, false);
                }
            }
        }

        public SceneManager(Device device, AssetManager assetManager)
        {
            this.device = device;
            this.assetManager = assetManager;

            // Create default camera and light.
            Camera = new Camera();
            Light = new Light();
        }

        public bool AddRenderable(IRenderable renderable, bool repositionWorld = true, bool toHead = false)
        {
            if (this.renderables.Contains(renderable))
                return false;

            if (toHead)
                this.renderables.Insert(0, renderable);
            else
                this.renderables.Add(renderable);
            
            if (repositionWorld)
                World = World;

            return true;
        }

        public bool RemoveRenderable(IRenderable renderable, bool repositionWorld = true)
        {
            if (this.renderables.Remove(renderable) == false)
                return false;

            if (repositionWorld)
                World = World;

            return true;
        }

        public bool AddSprite(ISprite sprite)
        {
            if (this.sprites.Contains(sprite))
                return false;

            this.sprites.Add(sprite);
            return true;
        }

        public bool RemoveSprite(ISprite sprite)
        {
            return this.sprites.Remove(sprite);
        }

        public bool Render(RenderSystem renderSystem, IRenderTarget renderTarget, bool present)
        {
            return renderSystem.Render(renderTarget, renderables, sprites, Camera, Light, present);
        }

        // TODO: Need this?
        public void LocateCameraLookingMesh(Mesh mesh)
        {
            Camera.EyePos = new Vector3(0, 0, mesh.BoundingRadius * -3.0f) + mesh.BoundingCenter;
            Camera.LookAt = mesh.BoundingCenter;
            Camera.AspectRatio = 1.0f;

            var pos = mesh.BoundingRadius * 10.0f;
            Light.Position = new Vector3(-pos, pos, -pos);
            Light.Color = Color.White;
            Light.AmbientColor = Color.White;
        }
    }
}
