
namespace Irelia.Render
{
    public sealed class MathUtils
    {
        public const float PI = (float)System.Math.PI;
        public const float tolerance = 1e-6f;

        private MathUtils() { }

        public static float Sin(Radian angle)
        {
            return (float)System.Math.Sin(angle.Value);
        }

        public static float Cos(Radian angle)
        {
            return (float)System.Math.Cos(angle.Value);
        }

        public static bool IsNearZero(float val, float tolerance = tolerance)
        {
            return (System.Math.Abs(val) <= tolerance);
        }

        public static float Sqrt(float value)
        {
            return (float)System.Math.Sqrt(value);
        }

        public static float Asin(Radian angle)
        {
            return (float)System.Math.Asin(angle.Value);
        }

        public static float Abs(float value)
        {
            return System.Math.Abs(value);
        }

        public static float Clamp(float value, float min, float max)
        {
            return System.Math.Max(System.Math.Min(value, max), min);
        }

        public static Radian Acos(float value)
        {
            return new Radian((float)System.Math.Acos((float)value));
        }

        public static float Min(float val1, float val2)
        {
            return System.Math.Min(val1, val2);
        }

        public static float Max(float val1, float val2)
        {
            return System.Math.Max(val1, val2);
        }

        public static float Round(float val)
        {
            return (float)System.Math.Round(val);
        }
    }
}
