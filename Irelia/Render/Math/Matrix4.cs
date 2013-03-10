using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Irelia.Render
{
    // Use "row vector convention"
    //   v' = (((vA)B)C)

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4
    {
        #region Member variables
        public float m00, m01, m02, m03;
        public float m10, m11, m12, m13;
        public float m20, m21, m22, m23;
        public float m30, m31, m32, m33;

        private static readonly Matrix4 zeroMatrix = new Matrix4(
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0);

        private static readonly Matrix4 identityMatrix = new Matrix4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
        #endregion

        #region Properties
        public static Matrix4 Zero
        {
            get { return zeroMatrix; }
        }

        public static Matrix4 Identity
        {
            get { return identityMatrix; }
        }

        public float this[int row, int col]
        {
            get
            {
                Debug.Assert(row >= 0 && row < 4 && col >= 0 && col < 4);
                unsafe
                {
                    fixed (float* ptr = &m00)
                        return *(ptr + ((row * 4) + col));
                }
            }
            set
            {
                Debug.Assert(row >= 0 && row < 4 && col >= 0 && col < 4);
                unsafe
                {
                    fixed (float* ptr = &m00)
                        *(ptr + ((row * 4) + col)) = value;
                }
            }
        }

        public Vector3 Translation
        {
            get { return new Vector3(m30, m31, m32); }
            set { m30 = value.x; m31 = value.y; m32 = value.z; }
        }

        public Quaternion Quaternion
        {
            get
            {
                var m3x3 = new Matrix3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
                return Quaternion.CreateFromRotationMatrix(m3x3);
            }
        }

        public Vector3 Scale
        {
            get { return new Vector3(m00, m11, m22); }
            set { m00 = value.x; m11 = value.y; m22 = value.z; }
        }

        public Matrix3 Matrix3
        {
            get { return new Matrix3(m00, m01, m02, m10, m11, m12, m20, m21, m22); }
            set
            {
                this.m00 = value.m00;
                this.m01 = value.m01;
                this.m02 = value.m02;
                this.m10 = value.m10;
                this.m11 = value.m11;
                this.m12 = value.m12;
                this.m20 = value.m20;
                this.m21 = value.m21;
                this.m22 = value.m22;
            }
        }
        #endregion

        #region Constructors
        public Matrix4(float m00, float m01, float m02, float m03,
                       float m10, float m11, float m12, float m13,
                       float m20, float m21, float m22, float m23,
                       float m30, float m31, float m32, float m33)
        {

            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public Matrix4(Vector4 col0, Vector4 col1, Vector4 col2, Vector4 col3)
        {
            this.m00 = col0.x;
            this.m10 = col0.y;
            this.m20 = col0.z;
            this.m30 = col0.w;
            this.m01 = col1.x;
            this.m11 = col1.y;
            this.m21 = col1.z;
            this.m31 = col1.w;
            this.m02 = col2.x;
            this.m12 = col2.y;
            this.m22 = col2.z;
            this.m32 = col2.w;
            this.m03 = col3.x;
            this.m13 = col3.y;
            this.m23 = col3.z;
            this.m33 = col3.w;
        }

        public Matrix4(Matrix3 m3x3)
        {
            this.m00 = m3x3.m00;
            this.m01 = m3x3.m01;
            this.m02 = m3x3.m02;
            this.m03 = 0.0f;
            this.m10 = m3x3.m10;
            this.m11 = m3x3.m11;
            this.m12 = m3x3.m12;
            this.m13 = 0.0f;
            this.m20 = m3x3.m20;
            this.m21 = m3x3.m21;
            this.m22 = m3x3.m22;
            this.m23 = 0.0f;
            this.m30 = 0.0f;
            this.m31 = 0.0f;
            this.m32 = 0.0f;
            this.m33 = 1.0f;
        }
        #endregion

        #region Public Methods
        public static Matrix4 CreateTranslation(Vector3 transDelta)
        {
            Matrix4 result = Matrix4.Identity;
            result.Translation = transDelta;
            return result;
        }

        public static Matrix4 CreateFromQuaternion(Quaternion q)
        {
            return new Matrix4(q.ToRotationMatrix());
        }

        public static Matrix4 CreateScale(Vector3 scale)
        {
            Matrix4 result = Matrix4.Identity;
            result.Scale = scale;
            return result;
        }

        public static bool operator ==(Matrix4 lhs, Matrix4 rhs)
        {
            if (lhs.m00 != rhs.m00 || lhs.m01 != rhs.m01 || lhs.m02 != rhs.m02 || lhs.m03 != rhs.m03 ||
                lhs.m10 != rhs.m10 || lhs.m11 != rhs.m11 || lhs.m12 != rhs.m12 || lhs.m13 != rhs.m13 ||
                lhs.m20 != rhs.m20 || lhs.m21 != rhs.m21 || lhs.m22 != rhs.m22 || lhs.m23 != rhs.m23 ||
                lhs.m30 != rhs.m30 || lhs.m31 != rhs.m31 || lhs.m32 != rhs.m32 || lhs.m33 != rhs.m33)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(Matrix4 lhs, Matrix4 rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            return (other is Matrix4) && (this == (Matrix4)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Matrix4 operator *(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 result = new Matrix4();

            result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;

            result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;

            result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;

            result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;

            return result;
        }

        public Matrix4 Invert()
        {
			float v0 = m20 * m31 - m21 * m30;
			float v1 = m20 * m32 - m22 * m30;
			float v2 = m20 * m33 - m23 * m30;
			float v3 = m21 * m32 - m22 * m31;
			float v4 = m21 * m33 - m23 * m31;
			float v5 = m22 * m33 - m23 * m32;

			float t00 = + (v5 * m11 - v4 * m12 + v3 * m13);
			float t10 = - (v5 * m10 - v2 * m12 + v1 * m13);
			float t20 = + (v4 * m10 - v2 * m11 + v0 * m13);
			float t30 = - (v3 * m10 - v1 * m11 + v0 * m12);

			float det = t00 * m00 + t10 * m01 + t20 * m02 + t30 * m03;

			if (MathUtils.IsNearZero(det, 1e-20f))
				return Identity;

			float invDet = 1 / det;

			float d00 = t00 * invDet;
			float d10 = t10 * invDet;
			float d20 = t20 * invDet;
			float d30 = t30 * invDet;

			float d01 = - (v5 * m01 - v4 * m02 + v3 * m03) * invDet;
			float d11 = + (v5 * m00 - v2 * m02 + v1 * m03) * invDet;
			float d21 = - (v4 * m00 - v2 * m01 + v0 * m03) * invDet;
			float d31 = + (v3 * m00 - v1 * m01 + v0 * m02) * invDet;

			v0 = m10 * m31 - m11 * m30;
			v1 = m10 * m32 - m12 * m30;
			v2 = m10 * m33 - m13 * m30;
			v3 = m11 * m32 - m12 * m31;
			v4 = m11 * m33 - m13 * m31;
			v5 = m12 * m33 - m13 * m32;

			float d02 = + (v5 * m01 - v4 * m02 + v3 * m03) * invDet;
			float d12 = - (v5 * m00 - v2 * m02 + v1 * m03) * invDet;
			float d22 = + (v4 * m00 - v2 * m01 + v0 * m03) * invDet;
			float d32 = - (v3 * m00 - v1 * m01 + v0 * m02) * invDet;

			v0 = m21 * m10 - m20 * m11;
			v1 = m22 * m10 - m20 * m12;
			v2 = m23 * m10 - m20 * m13;
			v3 = m22 * m11 - m21 * m12;
			v4 = m23 * m11 - m21 * m13;
			v5 = m23 * m12 - m22 * m13;

			float d03 = - (v5 * m01 - v4 * m02 + v3 * m03) * invDet;
			float d13 = + (v5 * m00 - v2 * m02 + v1 * m03) * invDet;
			float d23 = - (v4 * m00 - v2 * m01 + v0 * m03) * invDet;
			float d33 = + (v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            Matrix4 result = new Matrix4();
			result.m00 = d00; result.m01 = d01; result.m02 = d02; result.m03 = d03;
			result.m10 = d10; result.m11 = d11; result.m12 = d12; result.m13 = d13;
			result.m20 = d20; result.m21 = d21; result.m22 = d22; result.m23 = d23;
			result.m30 = d30; result.m31 = d31; result.m32 = d32; result.m33 = d33;

            return result;
        }

        public Matrix4 Transpose()
        {
            Matrix4 result = new Matrix4();
            result.m00 = this.m00; result.m01 = this.m10; result.m02 = this.m20; result.m03 = this.m30;
            result.m10 = this.m01; result.m11 = this.m11; result.m12 = this.m21; result.m13 = this.m31;
            result.m20 = this.m02; result.m21 = this.m12; result.m22 = this.m22; result.m23 = this.m32;
            result.m30 = this.m03; result.m31 = this.m13; result.m32 = this.m23; result.m33 = this.m33;
            return result;
        }

        public static Vector3 operator *(Vector3 v3, Matrix4 m)
        {
            var result = new Vector3();
            float inverseW = 1.0f / (v3.x * m.m03 + v3.y * m.m13 + v3.z * m.m23 + m.m33);
            result.x = (v3.x * m.m00 + v3.y * m.m10 + v3.z * m.m20 + m.m30) * inverseW;
            result.y = (v3.x * m.m01 + v3.y * m.m11 + v3.z * m.m21 + m.m31) * inverseW;
            result.z = (v3.x * m.m02 + v3.y * m.m12 + v3.z * m.m22 + m.m32) * inverseW;
            return result;
        }

        public override string ToString()
        {
            string result = "Matrix4(";
            for (int row = 0; row < 4; ++row)
            {
                result += " row" + row + "{";
                for (int col = 0; col < 4; ++col)
                {
                    result += this[row, col] + " ";
                }
                result += "}";
            }
            result += ")";

            return result;
        }
        #endregion
    }
}
