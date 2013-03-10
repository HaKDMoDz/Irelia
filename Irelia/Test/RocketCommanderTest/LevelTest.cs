using RocketCommander;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RocketCommanderTest
{
    [TestClass()]
    public class LevelTest
    {
        [TestMethod()]
        public void Mission_Construct_Test()
        {
            foreach (string levelFile in new string[] 
                { @"RocketCommander/Levels/EasyFlight.level",
                  @"RocketCommander/Levels/LostCivilization.level",
                  @"RocketCommander/Levels/TheRevenge.level",
                  @"RocketCommander/Levels/ValleyofDeath.level"})
            {
                var game = new TestGame();
                var level = new Level(game.Framework.AssetManager.GetFullPath(levelFile));
                Assert.AreEqual(Path.GetFileNameWithoutExtension(levelFile), level.Name);
                Assert.IsTrue(level.Width > 0);
                Assert.IsTrue(level.Length > 0);
                Assert.IsTrue(level.Items.Count > 0);
                Assert.AreEqual(level.Length, level.SunColors.Length);
            }
        }
    }
}
