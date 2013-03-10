using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {
        #region Member variables
        private static readonly Vector3 zeroVector = new Vector3(0.0f, 0.0f, 0.0f);
        private static readonly Vector3 unitXVector = new Vector3(1.0f, 0.0f, 0.0f);
        private static readonly Vector3 unitYVector = new Vector3(0.0f, 1.0f, 0.0f);
        private static readonly Vector3 unitZVector = new Vector3(0.0f, 0.0f, 1.0f);
        #endregion

        #region Properties
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public float Length
        {
            get { return (float)System.Math.Sqrt(SquaredLength); }
        }

        public float SquaredLength
        {
            get { return (x * x + y * y + z * z); }
        }

        public bool IsZeroLength
        {
            get { return SquaredLength < (1e-06 * 1e-06); }
        }

        public static Vector3 Zero
        {
            get { return zeroVector; }
        }

        public static Vector3 UnitX
        {
            get { return unitXVector; }
        }

        public static Vector3 UnitY
        {
            get { return unitYVector; }
        }

        public static Vector3 UnitZ
        {
            get { return unitZVector; }
        }
        #endregion

        public Vector3(float x, float y, float z) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z);
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z);
        }

        bool IEquatable<Vector3>.Equals(Vector3 other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            return (other is Vector3 && this == (Vector3)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static Vector3 operator *(Vector3 v, float scalar)
        {
            return new Vector3(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static Vector3 operator *(float scalar, Vector3 v)
        {
            return v * scalar;
        }

        public static Vector3 operator /(Vector3 v, float scalar)
        {
            Debug.Assert(scalar != 0.0f);
            return (v * (1.0f / scalar));
        }

        public Vector3 Normalize()
        {
            if (Length <= 0.0f)
                return Vector3.Zero;

            float inv = 1.0f / Length;
            return new Vector3(x * inv, y * inv, z * inv);
        }

        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z);
        }

        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.x * rhs.y - lhs.y * rhs.x);
        }

        public override string ToString()
        {
            return "Vector3(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }
    }
}
