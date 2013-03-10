using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3
    {
        #region Member variables
        public float m00, m01, m02;
        public float m10, m11, m12;
        public float m20, m21, m22;

        private static readonly Matrix3 zeroMatrix = new Matrix3(
            0, 0, 0,
            0, 0, 0,
            0, 0, 0);
        
        private static readonly Matrix3 identityMatrix = new Matrix3(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1);
        #endregion

        #region Properties
        public static Matrix3 Zero
        {
            get { return zeroMatrix; }
        }

        public static Matrix3 Identity
        {
            get { return identityMatrix; }
        }

        public float this[int row, int col]
        {
            get
            {
                Debug.Assert(row >= 0 && row < 3 && col >= 0 && col < 3);
                unsafe
                {
                    fixed (float* ptr = &m00)
                        return *(ptr + ((row * 3) + col));
                }
            }
            set
            {
                Debug.Assert(row >= 0 && row < 3 && col >= 0 && col < 3);
                unsafe
                {
                    fixed (float* ptr = &m00)
                        *(ptr + ((row * 3) + col)) = value;
                }
            }
        }
        #endregion

        #region Constructors
        public Matrix3(float m00, float m01, float m02,
                       float m10, float m11, float m12,
                       float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        public Matrix3(Vector3 col0, Vector3 col1, Vector3 col2)
        {
            this.m00 = col0.x;
            this.m10 = col0.y;
            this.m20 = col0.z;
            this.m01 = col1.x;
            this.m11 = col1.y;
            this.m21 = col1.z;
            this.m02 = col2.x;
            this.m12 = col2.y;
            this.m22 = col2.z;
        }
        #endregion

        #region Public methods
        public static bool operator ==(Matrix3 lhs, Matrix3 rhs)
        {
            if (lhs.m00 != rhs.m00 || lhs.m01 != rhs.m01 || lhs.m02 != rhs.m02 ||
                lhs.m10 != rhs.m10 || lhs.m11 != rhs.m11 || lhs.m12 != rhs.m12 ||
                lhs.m20 != rhs.m20 || lhs.m21 != rhs.m21 || lhs.m22 != rhs.m22)
            {
                return false;
            }
            return true;
        }

        public static bool operator !=(Matrix3 lhs, Matrix3 rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            return (other is Matrix3) && (this == (Matrix3)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            var result = new Matrix3();

            result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20;
            result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21;
            result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22;

            result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20;
            result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21;
            result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22;

            result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20;
            result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21;
            result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22;

            return result;
        }

        public override string ToString()
        {
            string result = "Matrix3(";
            for (int row = 0; row < 3; ++row)
            {
                result += " row" + row + "{";
                for (int col = 0; col < 3; ++col)
                {
                    result += this[row, col] + " ";
                }
                result += "}";
            }
            result += ")";

            return result;
        }

        public static Matrix3 CreateFromYawPitchRoll(Radian yaw, Radian pitch, Radian roll)
        {
            float cos = MathUtils.Cos(new Radian(yaw));
            float sin = MathUtils.Sin(new Radian(yaw));
            var yMat = new Matrix3(cos, 0, -sin, 0, 1, 0, sin, 0, cos);

            cos = MathUtils.Cos(new Radian(pitch));
            sin = MathUtils.Sin(new Radian(pitch));
            var xMat = new Matrix3(1, 0, 0, 0, cos, sin, 0, -sin, cos);
             
            cos = MathUtils.Cos(new Radian(roll));
            sin = MathUtils.Sin(new Radian(roll));
            var zMat = new Matrix3(cos, sin, 0, -sin, cos, 0, 0, 0, 1);

            return zMat * xMat * yMat;
        }

        public static Matrix3 operator -(Matrix3 lhs, Matrix3 rhs)
        {
            var result = new Matrix3();

            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 3; ++col)
                {
                    result[row, col] = lhs[row, col] - rhs[row, col];
                }
            }
            return result;
        }
        #endregion
    }
}
