using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float x { get; set; }
        public float y { get; set; }

        public static Vector2 Zero 
        { 
            get { return new Vector2(0.0f, 0.0f); } 
        }

        public static Vector2 UnitX
        {
            get { return new Vector2(1.0f, 0.0f); }
        }

        public static Vector2 UnitY
        {
            get { return new Vector2(0.0f, 1.0f); }
        }

        public Vector2(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return (lhs.x == rhs.x && lhs.y == rhs.y);
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y);
        }

        bool IEquatable<Vector2>.Equals(Vector2 other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            return (other is Vector2 && this == (Vector2)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.x, -v.y);
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vector2 operator *(Vector2 v, float scalar)
        {
            return new Vector2(v.x * scalar, v.y * scalar);
        }

        public static Vector2 operator *(float scalar, Vector2 v)
        {
            return v * scalar;
        }

        public static Vector2 operator /(Vector2 v, float scalar)
        {
            Debug.Assert(scalar != 0.0f);
            return (v * (1.0f / scalar));
        }

        public float Length
        {
            get { return (float)System.Math.Sqrt(SquaredLength); }
        }

        public float SquaredLength
        {
            get { return (x * x + y * y); }
        }

        public void Normalize()
        {
            if (Length <= 0.0f)
                return;

            float inv = 1.0f / Length;
            if (inv > 0.0f)
            {
                x *= inv;
                y *= inv;
            }
        }

        public static float Dot(Vector2 lhs, Vector2 rhs)
        {
            return (lhs.x * rhs.x +  lhs.y * rhs.y);
        }

        // Calculates the 2 dimensional cross-product of 2 vectors,
        // which results in a single floating point value which is 2 times the area of the triangle.
        public static float Cross(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }

        public override string ToString()
        {
            return "Vector2(" + x.ToString() + ", " + y.ToString() + ")";
        }

        public static Vector2 Parse(string s)
        {
            var words = s.Split(new char[] { '(', ',', ')' });
            return new Vector2()
            {
                x = float.Parse(words[1]),
                y = float.Parse(words[2])
            };
        }
    }
}
