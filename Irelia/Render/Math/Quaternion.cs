using System;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public float w;
        public float x;
        public float y;
        public float z;

        public float LengthSquared
        {
            get { return w * w + x * x + y * y + z * z; }
        }

        public float Length
        {
            get { return MathUtils.Sqrt(LengthSquared); }
        }

        public static Quaternion Identity { get { return new Quaternion(1.0f, 0.0f, 0.0f, 0.0f); } }
        public static Quaternion Zero { get { return new Quaternion(0.0f, 0.0f, 0.0f, 0.0f); } }

        public Quaternion(float w, float x, float y, float z) : this()
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
            return (lhs.w == rhs.w && lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z);
        }

        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return (lhs.w != rhs.w || lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z);
        }

        bool IEquatable<Quaternion>.Equals(Quaternion other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            return (other is Quaternion && this == (Quaternion)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Quaternion(" + w + ", " + x + ", " + y + ", " + z + ")";
        }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, Radian angle)
        {
            Radian halfAngle = angle / 2;
            float sin = MathUtils.Sin(halfAngle);
            return new Quaternion()
            {
                w = MathUtils.Cos(halfAngle),
                x = sin * axis.x,
                y = sin * axis.y,
                z = sin * axis.z
            };
        }

        public static Quaternion operator -(Quaternion q)
        {
            return new Quaternion(-q.w, -q.x, -q.y, -q.z);
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(w, -x, -y, -z);
        }

        public static Quaternion operator *(Quaternion q, float scalar)
        {
            return new Quaternion(q.w * scalar, q.x * scalar, q.y * scalar, q.z * scalar);
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion()
            {
                w = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z,
                x = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                y = lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                z = lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x
            };
        }

        public static Vector3 operator *(Vector3 point, Quaternion q)
        {
            // nVidia SDK implementation
            var qvec = new Vector3(q.x, q.y, q.z);
            var uv = Vector3.Cross(qvec, point);
            var uuv = Vector3.Cross(qvec, uv);
            uv *= (2.0f * q.w);
            uuv *= 2.0f;

            return point + uv + uuv;
        }

        public Quaternion Inverse()
        {
            if (Length <= 0.0f)
                return Quaternion.Zero;

            float invLength = 1.0f / Length;
            return new Quaternion(w * invLength, -x * invLength, -y * invLength, -z * invLength);
        }

        public static Quaternion operator -(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion()
            {
                w = lhs.w - rhs.w,
                x = lhs.x - rhs.x,
                y = lhs.y - rhs.y,
                z = lhs.z - rhs.z
            };
        }

        public Quaternion Normalize()
        {
            if (Length <= 0.0f)
                return Quaternion.Zero;

            return (this * (1.0f / Length));
        }

        public static Quaternion CreateFromRotationMatrix(Matrix3 m)
        {
            // Algorithm in Ken Shoemake's article in 1987 SIGGRAPH course notes
            // article "Quaternion Calculus and Fast Animation".
            float trace = m.m00 + m.m11 + m.m22;

            if (trace > 0.0f)
            {
                // |w| > 1/2, may as well choose w > 1/2

                float root = MathUtils.Sqrt(trace + 1.0f);  // 2w
                float root2 = 0.5f / root;                  // 1/(4w)
                
                return new Quaternion()
                {
                    w = 0.5f * root,
                    x = (m.m12 - m.m21) * root2,
                    y = (m.m20 - m.m02) * root2,
                    z = (m.m01 - m.m10) * root2
                };
            }
            else
            {
                // |w| <= 1/2

                int i = 0;
                if (m.m11 > m.m00)
                    i = 1;
                if (m.m22 > m.m11)
                    i = 2;

		        int[] next = new int[ 3 ] { 1, 2, 0 };
                int j = next[i];
                int k = next[j];

                float root = MathUtils.Sqrt(m[i, i] - m[j, j] - m[k, k] + 1.0f);
                
                var result = new Quaternion()
                {
                    w = (m[j, k] - m[k, j]) * root
                };

                unsafe
                {
                    float* apkQuat = &result.x;

                    apkQuat[i] = 0.5f * root;
                    root = 0.5f / root;
                    apkQuat[j] = (m[i, j] + m[j, i]) * root;
                    apkQuat[k] = (m[i, k] + m[k, i]) * root;
                }
                return result;
            }
        }

        public Matrix3 ToRotationMatrix()
        {
            float tx = 2.0f * this.x;
            float ty = 2.0f * this.y;
            float tz = 2.0f * this.z;
            float twx = tx * this.w;
            float twy = ty * this.w;
            float twz = tz * this.w;
            float txx = tx * this.x;
            float txy = ty * this.x;
            float txz = tz * this.x;
            float tyy = ty * this.y;
            float tyz = tz * this.y;
            float tzz = tz * this.z;

            var m = new Matrix3();
            m.m00 = 1.0f - (tyy + tzz);
            m.m10 = txy - twz;
            m.m20 = txz + twy;
            m.m01 = txy + twz;
            m.m11 = 1.0f - (txx + tzz);
            m.m21 = tyz - twx;
            m.m02 = txz - twy;
            m.m12 = tyz + twx;
            m.m22 = 1.0f - (txx + tyy);
            return m;
        }

        public static Quaternion CreateFromAxes(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
        {
            Matrix3 m = new Matrix3();
            
            m.m00 = xAxis.x;
            m.m01 = xAxis.y;
            m.m02 = xAxis.z;

            m.m10 = yAxis.x;
            m.m11 = yAxis.y;
            m.m12 = yAxis.z;

            m.m20 = zAxis.x;
            m.m21 = zAxis.y;
            m.m22 = zAxis.z;

            return Quaternion.CreateFromRotationMatrix(m);
        }

        public void ToAxes(out Vector3 xAxis, out Vector3 yAxis, out Vector3 zAxis)
        {
            Matrix3 m = ToRotationMatrix();

            xAxis = new Vector3(m.m00, m.m01, m.m02);
            yAxis = new Vector3(m.m10, m.m11, m.m12);
            zAxis = new Vector3(m.m20, m.m21, m.m22);
        }

        public static Quaternion CreateFromVectorRotation(Vector3 src, Vector3 dest)
        {
            Vector3 v0 = src.Normalize();
            Vector3 v1 = dest.Normalize();

            float d = Vector3.Dot(v0, v1);
            if (d >= 1.0f)
            {
                return Quaternion.Identity;
            }
            else if (d < (1e-6f - 1.0f))
            {
                Vector3 axis = Vector3.Cross(src, Vector3.UnitX);
                if (axis.Length == 0.0f)
                    axis = Vector3.Cross(src, Vector3.UnitY);
                axis.Normalize();
                return Quaternion.CreateFromAxisAngle(axis, new Radian(MathUtils.PI));
            }
            else
            {
                float s = MathUtils.Sqrt((1 + d) * 2);
                float invs = 1 / s;

                Vector3 c = Vector3.Cross(v0, v1);

                var q = new Quaternion()
                {
                    w = s * 0.5f,
                    x = c.x * invs,
                    y = c.y * invs,
                    z = c.z * invs
                };
                return q.Normalize();
            }
        }
    }
}
