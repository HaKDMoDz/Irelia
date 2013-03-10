using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketCommander;

namespace RocketCommanderTest
{
    [TestClass()]
    public class RocketTest
    {
        [TestMethod()]
        public void Rocket_Render_Test()
        {
            var game = new TestGame();
            game.Initialized += (o, e) =>
                {
                    var rocket = new Rocket(game.Framework);
                    rocket.MeshNode.Orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(MathUtils.PI * -0.5f));
                    game.SceneManager.AddRenderable(rocket.MeshNode);

                    game.SceneManager.Camera.LookMeshNode(rocket.MeshNode);
                    game.SceneManager.Light.Position = new Vector3(0.0f, 0.0f, rocket.MeshNode.BoundingRadius * -5.0f);
                };

            game.Run();
        }

        [TestMethod()]
        public void Rocket_InFrontOfCamera_Test()
        {
            var game = new TestGame();

            Rocket rocket = null;
            game.Initialized += (o, e) =>
                {
                    rocket = new Rocket(game.Framework);
                    game.SceneManager.AddRenderable(rocket.MeshNode);
                    
                    game.Camera = new RoamingCamera(game.Mouse, game.Keyboard, game);
                    game.Camera.FovY = new Radian(MathUtils.PI / 1.8f);
                };
            game.Updated += (o, e) =>
                {
                    var inFrontOfCameraPos = new Vector3(0, -1.33f, 2.5f);
                    rocket.MeshNode.Position = inFrontOfCameraPos * game.Camera.ViewMatrix.Invert();

                    rocket.MeshNode.Orientation = game.Camera.Orientation *
                        Quaternion.CreateFromAxisAngle(Vector3.UnitZ, new Radian(game.TotalGameTime / 8500.0f));
                };
            game.Run();
        }
    }
}
