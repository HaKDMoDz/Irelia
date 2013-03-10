using System;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Radian : IEquatable<Radian>
    {
        public float Value { get; private set; }

        public Radian(float value) : this()
        {
            Value = value;
        }

        public Radian(Radian other) : this()
        {
            Value = other.Value;
        }

        public static bool operator <(Radian lhs, Radian rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator <=(Radian lhs, Radian rhs)
        {
            return lhs.Value <= rhs.Value;
        }

        public static bool operator >(Radian lhs, Radian rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator >=(Radian lhs, Radian rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        public static bool operator ==(Radian lhs, Radian rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Radian lhs, Radian rhs)
        {
            return lhs.Value != rhs.Value;
        }

        bool IEquatable<Radian>.Equals(Radian other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            return (obj is Radian && this == (Radian)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Radian operator +(Radian r)
        {
            return r;
        }

        public static Radian operator -(Radian r)
        {
            return new Radian(-r.Value);
        }

        public static Radian operator +(Radian lhs, Radian rhs)
        {
            return new Radian(lhs.Value + rhs.Value);
        }

        public static Radian operator -(Radian lhs, Radian rhs)
        {
            return new Radian(lhs.Value - rhs.Value);
        }

        public static Radian operator *(Radian r, float scalar)
        {
            return new Radian(scalar * r.Value);
        }

        public static Radian operator *(float scalar, Radian r)
        {
            return r * scalar;
        }

        public static Radian operator *(Radian lhs, Radian rhs)
        {
            return new Radian(lhs.Value * rhs.Value);
        }

        public static Radian operator /(Radian r, float scalar)
        {
            return new Radian(r.Value / scalar);
        }

        public Radian Wrap(Radian wrapAngle)
        {
            float result = Value % wrapAngle.Value;
            if (result < 0)
                result += wrapAngle.Value;
            return new Radian(result);
        }

        public override string ToString()
        {
            return "Radian(" + Value.ToString() + ")";
        }

        public static Radian AngleBetween(Vector3 lhs, Vector3 rhs)
        {
            float lenProduct = lhs.Length * rhs.Length;
            if (lenProduct < 1e-6f)
                lenProduct = 1e-6f;

            float f = Vector3.Dot(lhs, rhs) / lenProduct;
            f = MathUtils.Clamp(f, -1.0f, 1.0f);
            return MathUtils.Acos(f);
        }
    }
}
