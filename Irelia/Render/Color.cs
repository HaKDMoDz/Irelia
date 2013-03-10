using System;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }
        public float a { get; set; }

        public int IntA
        {
            get { return (int)(a * 255.0f); }
        }

        public int IntR
        {
            get { return (int)(r * 255.0f); }
        }

        public int IntG
        {
            get { return (int)(g * 255.0f); }
        }

        public int IntB
        {
            get { return (int)(b * 255.0f); }
        }

        public Color(float a, float r, float g, float b) : this()
        {
            Create(a, r, g, b);
        }

        private void Create(float a, float r, float g, float b)
        {
            if (a < 0.0f || a > 1.0f)
                throw new ArgumentOutOfRangeException("a", a, "must be in range 0.0f~1.0f");
            if (r < 0.0f || r > 1.0f)
                throw new ArgumentOutOfRangeException("r", r, "must be in range 0.0f~1.0f");
            if (g < 0.0f || g > 1.0f)
                throw new ArgumentOutOfRangeException("g", g, "must be in range 0.0f~1.0f");
            if (b < 0.0f || b > 1.0f)
                throw new ArgumentOutOfRangeException("b", b, "must be in range 0.0f~1.0f");

            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color(int a, int r, int g, int b) : this()
        {
            Create(a / 255.0f, r / 255.0f, g / 255.0f, b / 255.0f);
        }

        public int ToArgb()
        {
            int result = 0;
            result += ((int)(a * 255.0f)) << 24;
            result += ((int)(r * 255.0f)) << 16;
            result += ((int)(g * 255.0f)) << 8;
            result += ((int)(b * 255.0f));
            return result;
        }

        public override string ToString()
        {
            return "Color(" + a.ToString() + ", " + r.ToString() + ", " + g.ToString() + ", " + b.ToString() + ")";
        }

        public static Color Parse(string s)
        {
            var words = s.Split(new char[] { '(', ',', ')' });
            return new Color()
            {
                a = float.Parse(words[1]),
                r = float.Parse(words[2]),
                g = float.Parse(words[3]),
                b = float.Parse(words[4])
            };
        }

        public static bool operator ==(Color lhs, Color rhs)
        {
            return (lhs.a == rhs.a && lhs.r == rhs.r && lhs.g == rhs.g && lhs.b == rhs.b);
        }

        public static bool operator !=(Color lhs, Color rhs)
        {
            return (lhs.a != rhs.a || lhs.r != rhs.r || lhs.g != rhs.g || lhs.b != rhs.b);
        }

        public override bool Equals(object other)
        {
            return (other is Color && this == (Color)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region Predefined colors
        public static Color Transparent { get { return new Color(0.0f, 1.0f, 1.0f, 1.0f); } }
        public static Color Black { get { return new Color(1.0f, 0.0f, 0.0f, 0.0f); } }
        public static Color White { get { return new Color(1.0f, 1.0f, 1.0f, 1.0f); } }
        public static Color Red { get { return new Color(1.0f, 1.0f, 0.0f, 0.0f); } }
        public static Color Green { get { return new Color(1.0f, 0.0f, 0.5019608f, 0.0f); } }
        public static Color Blue { get { return new Color(1.0f, 0.0f, 0.0f, 1.0f); } }
        public static Color Gray { get { return new Color(1.0f, 0.5019608f, 0.5019608f, 0.5019608f); } }
        #endregion
    }
}
