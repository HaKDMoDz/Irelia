using System;
using Irelia.Render;

namespace RocketCommander
{
    public static class RandomHelper
    {
        private static readonly Random random = new Random();

        public static int GetRandomInt(int max)
        {
            return random.Next(max);
        }

        public static float GetRandomFloat(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        public static Vector3 GetRandomVector3(float min, float max)
        {
            return new Vector3()
            {
                x = GetRandomFloat(min, max),
                y = GetRandomFloat(min, max),
                z = GetRandomFloat(min, max)
            };
        }
    }
}
