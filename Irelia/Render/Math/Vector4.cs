using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        private static readonly Vector4 zeroVector = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        public static Vector4 Zero
        {
            get { return zeroVector; }
        }

        public Vector4(float x, float y, float z, float w) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector3 v3, float w) : this()
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
            this.w = w;
        }

        public static bool operator ==(Vector4 lhs, Vector4 rhs)
        {
            return (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w);
        }

        public static bool operator !=(Vector4 lhs, Vector4 rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z || lhs.w == rhs.w);
        }

        bool IEquatable<Vector4>.Equals(Vector4 other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            return (other is Vector4 && this == (Vector4)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        public static Vector4 operator *(Vector4 v, float scalar)
        {
            return new Vector4(v.x * scalar, v.y * scalar, v.z * scalar, v.w * scalar);
        }

        public static Vector4 operator *(float scalar, Vector4 v)
        {
            return v * scalar;
        }

        public static float Dot(Vector4 lhs, Vector4 rhs)
        {
            return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w);
        }

        public override string ToString()
        {
            return "Vector4(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ", " + w.ToString() + ")";
        }
    }
}
