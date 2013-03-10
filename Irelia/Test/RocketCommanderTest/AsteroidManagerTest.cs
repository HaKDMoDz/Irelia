using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketCommander;

namespace RocketCommanderTest
{
    [TestClass()]
    public class AsteroidManagerTest
    {
        [TestMethod()]
        public void AsteroidManager_Render_Test()
        {
            var game = new TestGame();
            AsteroidManager asteroidManager = null;
            game.Initialized += (o, e) =>
            {
                var level = new Level(game.Framework.AssetManager.GetFullPath(@"RocketCommander/Levels/EasyFlight.level"));
                asteroidManager = new AsteroidManager(level, game);
            };
            game.Updated += (o, e) =>
            {
                asteroidManager.Update();
                game.DebugText += "\n\n" + asteroidManager.GetVisibilityText();
            };
            game.Run();
        }
    }
}
