using Input;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketCommander;

namespace RocketCommanderTest
{
    [TestClass()]
    public class ItemTest
    {
        [TestMethod()]
        public void Item_Render_Test()
        {
            var game = new TestGame();
            Item item = null;
            game.Updated += (o, e) =>
            {
                game.DebugText += "\n\nPress key 1~5 to watch each item";

                Key[] keys = new Key[] { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5 };
                foreach (Key key in keys)
                {
                    if (game.Keyboard.IsKeyDown(key) == false)
                        continue;

                    if (item != null)
                        game.SceneManager.RemoveRenderable(item.MeshNode);

                    Item.Type type = (Item.Type)(key - keys[0]);
                    item = new Item(game.Framework, game.SceneManager, type);

                    game.SceneManager.Camera.LookMeshNode(item.MeshNode);
                    game.SceneManager.Light.Position = new Vector3(0.0f, 0.0f, item.MeshNode.BoundingRadius * -5.0f);
                }
            };
            game.Run();
        }
    }
}
