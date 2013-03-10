using RocketCommander;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Input;

namespace RocketCommanderTest
{
    [TestClass()]
    public class GameCameraTest
    {
        [TestMethod()]
        public void GameCamera_VariousCameras_Test()
        {
            var game = new TestGame();
            game.Updated += (o, e) =>
                {
                    if (game.Keyboard.IsKeyDown(Key.D1))
                        game.Camera = new FreeCamera(game.Mouse, game.Keyboard);
                    else if (game.Keyboard.IsKeyDown(Key.D2))
                        game.Camera = new RoamingCamera(game.Mouse, game.Keyboard, game);
                };

            game.Run();
        }
    }
}
