using Input;
using Irelia.Gui;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RocketCommanderTest
{
    [TestClass]
    public class GUITest
    {
        [TestMethod]
        public void Layout_Test()
        {
            var game = new TestGame();
            Layout layout = null;
            game.Updated += (o, e) =>
                {
                    game.DebugText += "\n\nPress key 1~ to watch each layout";

                    string[] layoutNames = 
                    {
                        @"RocketCommander\GUI\MainMenu.layout",
                        @"RocketCommander\GUI\Help.layout",
                        @"RocketCommander\GUI\MissionSelection.layout"
                    };
                    for (int i = 0; i < layoutNames.Length; ++i)
                    {
                        if (game.Keyboard.IsKeyDown(Key.D1 + i) == false)
                            continue;

                        if (layout != null)
                            game.RemoveLayout(layout);

                        layout = game.Framework.AssetManager.Load(layoutNames[i]) as Layout;
                        game.AddLayout(layout);
                    }
                };
            game.Run();
        }
    }
}
