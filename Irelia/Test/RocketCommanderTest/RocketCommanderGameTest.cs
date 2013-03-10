using Input;
using Irelia.Gui;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketCommander;
using System.Collections.Generic;
using System;

namespace RocketCommanderTest
{
    [TestClass()]
    public class RocketCommanderGameTest
    {
        [TestMethod()]
        public void RocketCommanderGame_Run_Test()
        {
            var game = new RocketCommanderGame(1024, 768);
            game.Run();
        }

        [TestMethod()]
        public void RocketCommanderGame_Keyboard_Test()
        {
            var game = new TestGame();
            game.Updated += (o, e) =>
                {
                    if (game.Keyboard.IsKeyDown(Key.Escape))
                        game.Exit();
                };

            game.Run();
        }

        [TestMethod()]
        public void RocketCommanderGame_Mouse_Test()
        {
            var game = new TestGame();

            Layout layout = null;
            game.Initialized += (o, e) =>
                {
                    layout = new Layout("MouseState", game.Framework.AssetManager)
                    {
                        DestRect = new Rectangle(0.0f, 0.1f, 1.0f, 1.0f)
                    };

                    var mouseBlock = new TextBlock(layout, game.Framework.AssetManager)
                    {
                        Font = game.Framework.AssetManager.DefaultFont,
                        Foreground = Color.Red,
                        Name = "MouseBlock"
                    };

                    game.AddLayout(layout);
                };
            game.Mouse.MouseMove += (o, e) =>
                {
                    var mouseBlock = layout.GetElement("MouseBlock") as TextBlock;
                    mouseBlock.Text = game.Mouse.Position.ToString();
                };
            
            game.Run();
        }

        [TestMethod()]
        public void RocketCommanderGame_RoamingRocketScene_Test()
        {
            var game = new TestGame();
            game.Updated += (o, e) =>
                {
                    if (game.Keyboard.IsKeyDown(Key.D1))
                        game.EnableRoamingRocketScene();
                    else if (game.Keyboard.IsKeyDown(Key.D2))
                        game.DisableRoamingRocketScene();
                };
            game.Run();
        }
    }
}
