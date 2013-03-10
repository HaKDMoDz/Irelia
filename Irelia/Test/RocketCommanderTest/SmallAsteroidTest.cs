using Input;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketCommander;

namespace RocketCommanderTest
{
    [TestClass()]
    public class SmallAsteroidTest
    {
        [TestMethod()]
        public void SmallAsteroid_Render_Test()
        {
            var game = new TestGame();
            SmallAsteroid smallAsteroid = null;
            game.Updated += (o, e) =>
            {
                game.DebugText += "\n\nPress key 1~3 to watch each small asteroid";

                Key[] keys = new Key[] { Key.D1, Key.D2, Key.D3 };
                foreach (Key key in keys)
                {
                    if (game.Keyboard.IsKeyDown(key) == false)
                        continue;

                    if (smallAsteroid != null)
                        game.SceneManager.RemoveRenderable(smallAsteroid.MeshNode);

                    int type = key - keys[0];
                    smallAsteroid = new SmallAsteroid(game.Framework, game.SceneManager, type);

                    game.SceneManager.Camera.LookMeshNode(smallAsteroid.MeshNode);
                    game.SceneManager.Light.Position = new Vector3(0.0f, 0.0f, smallAsteroid.MeshNode.BoundingRadius * -5.0f);
                }
            };
            game.Run();
        }
    }
}
