using System;

namespace RocketCommander
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var game = new RocketCommanderGame(1024, 768);
            game.Run();
        }
    }
}
