
namespace Irelia.Render
{
    public static class D3DMappings
    {
        public static SlimDX.Matrix ToD3DMatrix(this Matrix3 mat)
        {
            return new SlimDX.Matrix()
            {
                M11 = mat.m00,
                M12 = mat.m01,
                M13 = mat.m02,
                M14 = 0,
                M21 = mat.m10,
                M22 = mat.m11,
                M23 = mat.m12,
                M24 = 0,
                M31 = mat.m20,
                M32 = mat.m21,
                M33 = mat.m22,
                M34 = 0,
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
        }

        public static SlimDX.Matrix ToD3DMatrix(this Matrix4 mat)
        {
            return new SlimDX.Matrix()
            {
                M11 = mat.m00,
                M12 = mat.m01,
                M13 = mat.m02,
                M14 = mat.m03,
                M21 = mat.m10,
                M22 = mat.m11,
                M23 = mat.m12,
                M24 = mat.m13,
                M31 = mat.m20,
                M32 = mat.m21,
                M33 = mat.m22,
                M34 = mat.m23,
                M41 = mat.m30,
                M42 = mat.m31,
                M43 = mat.m32,
                M44 = mat.m33
            };
        }

        public static Matrix4 ToMatrix4(this SlimDX.Matrix mat)
        {
            return new Matrix4(mat.M11, mat.M12, mat.M13, mat.M14,
                               mat.M21, mat.M22, mat.M23, mat.M24,
                               mat.M31, mat.M32, mat.M33, mat.M34,
                               mat.M41, mat.M42, mat.M43, mat.M44);
        }

        public static SlimDX.Vector2 ToD3DVector2(this Vector2 v2)
        {
            return new SlimDX.Vector2()
            {
                X = v2.x,
                Y = v2.y
            };
        }

        public static SlimDX.Vector3 ToD3DVector3(this Vector3 v3)
        {
            return new SlimDX.Vector3(v3.x, v3.y, v3.z);
        }

        public static Vector3 ToVector3(this SlimDX.Vector3 v3)
        {
            return new Vector3(v3.X, v3.Y, v3.Z);
        }

        public static SlimDX.Vector4 ToD3DVector4(this Vector4 v4)
        {
            return new SlimDX.Vector4()
            {
                X = v4.x,
                Y = v4.y,
                Z = v4.z,
                W = v4.w
            };
        }

        public static Quaternion ToQuaternion(this SlimDX.Quaternion q)
        {
            return new Quaternion(q.W, q.X, q.Y, q.Z);
        }

        #region Color
        public static SlimDX.Color4 ToD3DColor4(this Color color)
        {
            return new SlimDX.Color4()
            {
                Alpha = color.a,
                Red = color.r,
                Green = color.g,
                Blue = color.b
            };
        }

        public static SlimDX.Vector4 ToD3DVector4(this Color color)
        {
            return new SlimDX.Vector4()
            {
                X = color.r,
                Y = color.g,
                Z = color.b,
                W = color.a
            };
        }

        public static Color ToIreliaColor(this SlimDX.Color4 color)
        {
            return new Color()
            {
                a = color.Alpha,
                r = color.Red,
                g = color.Green,
                b = color.Blue
            };
        }
        #endregion

        public static System.Drawing.Rectangle ToD3DRectangle(this Rectangle rect)
        {
            return new System.Drawing.Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
        }
    }
}
