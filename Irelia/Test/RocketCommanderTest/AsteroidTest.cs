using Input;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketCommander;

namespace RocketCommanderTest
{
    [TestClass()]
    public class AsteroidTest
    {
        [TestMethod()]
        public void Asteroid_Render_Test()
        {
            var game = new TestGame();
            Asteroid asteroid = null;
            game.Updated += (o, e) =>
            {
                game.DebugText += "\n\nPress key 1~5 to watch each asteroid";

                Key[] keys = new Key[] { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5 };
                foreach (Key key in keys)
                {
                    if (game.Keyboard.IsKeyDown(key) == false)
                        continue;
                            
                    if (asteroid != null)
                        game.SceneManager.RemoveRenderable(asteroid.MeshNode);

                    int type = key - keys[0];
                    asteroid = new Asteroid(game.Framework, game.SceneManager, type);

                    game.SceneManager.Camera.LookMeshNode(asteroid.MeshNode);
                    game.SceneManager.Light.Position = new Vector3(0.0f, 0.0f, asteroid.MeshNode.BoundingRadius * -5.0f);
                }
            };
            game.Run();
        }
    }
}
