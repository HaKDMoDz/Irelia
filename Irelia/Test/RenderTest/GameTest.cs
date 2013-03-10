using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class GameTest
    {
        [TestMethod()]
        public void Game_Constructor_Test()
        {
            var game = new Game("Test", 640, 480);
            Assert.AreEqual(640, game.Width);
            Assert.AreEqual(480, game.Height);
            Assert.AreEqual(1000 / 60, game.TargetElapsedTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Game_SetInvalidTargetElapsedTime_Test()
        {
            var game = new Game("Test", 640, 480);
            game.TargetElapsedTime = 0;
        }

        internal class TestGame : Game
        {
            public event EventHandler<EventArgs> Initialized = delegate { };
            public event EventHandler<EventArgs> Updated = delegate { };
            public event EventHandler<EventArgs> Drew = delegate { };

            public TestGame() : base("TestGame", 640, 480) { }

            protected override void Initialize()
            {
                base.Initialize();
                Initialized(this, EventArgs.Empty);
            }

            protected override void Update(int elapsed)
            {
                base.Update(elapsed);
                Updated(this, EventArgs.Empty);
                Exit();
            }

            protected override void Draw(float elapsed)
            {
                base.Draw(elapsed);
                Drew(this, EventArgs.Empty);
            }
        }

        [TestMethod()]
        public void Game_Run_Test()
        {
            var game = new TestGame();

            bool initializedFired = false, updateFired = false, drawFired = false, exitingFired = false;
            game.Initialized += ((o, e) => initializedFired = true);
            game.Updated += ((o, e) => updateFired = true);
            game.Exiting += ((o, e) => exitingFired = true);
            game.Drew += ((o, e) => drawFired = true);
            game.Run();

            Assert.IsTrue(initializedFired);
            Assert.IsTrue(updateFired);
            Assert.IsTrue(exitingFired);
            Assert.IsTrue(drawFired);
        }
    }
}
