using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class Matrix4Test
    {
        [TestMethod()]
        public void Matrix4_Constructor_Test()
        {
            Matrix4 m = new Matrix4();
            Assert.AreEqual(Matrix4.Zero, m);

            m = new Matrix4(0, 4, 8, 12,
                            1, 5, 9, 13,
                            2, 6, 10, 14,
                            3, 7, 11, 15);
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    Assert.AreEqual(col * 4 + row, m[row, col]);
                }
            }
        }

        [TestMethod()]
        public void Matrix4_CreateFromVectors_Test()
        {
            Vector4 v1 = new Vector4(0, 1, 2, 3);
            Vector4 v2 = new Vector4(4, 5, 6, 7);
            Vector4 v3 = new Vector4(8, 9, 10, 11);
            Vector4 v4 = new Vector4(12, 13, 14, 15);

            Matrix4 m = new Matrix4(v1, v2, v3, v4);
            Matrix4 expected = new Matrix4(v1.x, v2.x, v3.x, v4.x,
                                           v1.y, v2.y, v3.y, v4.y,
                                           v1.z, v2.z, v3.z, v4.z,
                                           v1.w, v2.w, v3.w, v4.w);
            Assert.AreEqual(expected, m);
        }

        [TestMethod()]
        public void Matrix4_Equality_Test()
        {
            Matrix4 a = new Matrix4(0, 4, 8, 12,
                                    1, 5, 9, 13,
                                    2, 6, 10, 14,
                                    3, 7, 11, 15);
            Matrix4 b = new Matrix4(0, 4, 8, 12,
                                    1, 5, 9, 13,
                                    2, 6, 10, 14,
                                    3, 7, 11, 15);
            Assert.AreEqual(b, a);

            Matrix4 c = new Matrix4(0, 0, 0, 0,
                                    1, 1, 1, 1,
                                    2, 2, 2, 2,
                                    3, 3, 3, 3);
            Assert.AreNotEqual(c, a);
        }

        [TestMethod()]
        public void Matrix4_Multiplication_Test()
        {
            Matrix4 m1 = new Matrix4(0, 4, 8, 12,
                     1, 5, 9, 13,
                     2, 6, 10, 14,
                     3, 7, 11, 15);
            Matrix4 m2 = new Matrix4(16, 20, 24, 28,
                             17, 21, 25, 29,
                             18, 22, 26, 30,
                             19, 23, 27, 31);
            Matrix4 res = m1 * m2;
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    Vector4 rowv = new Vector4(m1[row, 0], m1[row, 1], m1[row, 2], m1[row, 3]);
                    Vector4 colv = new Vector4(m2[0, col], m2[1, col], m2[2, col], m2[3, col]);
                    Assert.AreEqual(Vector4.Dot(rowv, colv), res[row, col]);
                }
            }

            // Not commutative (AB != BA)
            Assert.AreNotEqual(m1 * m2, m2 * m1);
        }

        [TestMethod()]
        public void Matrix4_SpecialMatrices_Test()
        {
            // Zero matrix
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    Assert.AreEqual(0, Matrix4.Zero[row, col]);
                }
            }

            // Identity matrix
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    if (row == col)
                        Assert.AreEqual(1, Matrix4.Identity[row, col]);
                    else
                        Assert.AreEqual(0, Matrix4.Identity[row, col]);
                }
            }
        }

        [TestMethod()]
        public void Matrix4_InvertTest()
        {
            Matrix4 a = new Matrix4(1, 2, 2, 2,
                    2, 2, 1, 1,
                    1, 2, 1, 0,
                    1, 2, 2, 1);

            Matrix4 b = new Matrix4(-1, 1, -1, 1,
                             1, -0.5f, 1.5f, -1.5f,
                            -1, 0, -1, 2,
                             1, 0, 0, -1);

            Assert.AreEqual(b, a.Invert());

            Matrix4 ai = a.Invert();
            Assert.AreEqual(b, ai);

            // A(A-1) = (A-1)A = I
            Assert.AreEqual(Matrix4.Identity, a * a.Invert());
            Assert.AreEqual(Matrix4.Identity, a.Invert() * a);

            // (AB)-1 = B-1A-1
            Matrix4 c = new Matrix4(2, 3, 2, 2,
                            3, 3, 3, 3,
                            3, 2, 2, 3,
                            3, 2, 1, 2);
            // TODO: Fail in release mode.
            //Assert.AreEqual((a * c).Invert(), c.Invert() * a.Invert());

            // (M-1)-1 = M
            Assert.AreEqual(a, a.Invert().Invert());

            // I-1 = I
            Assert.AreEqual(Matrix4.Identity, Matrix4.Identity.Invert());

            // (MT)-1 = (M-1)T
            Assert.AreEqual(a.Transpose().Invert(), a.Invert().Transpose());
        }

        [TestMethod()]
        public void Matrix4_TransposeTest()
        {
            Matrix4 a = new Matrix4(0, 1, 2, 3,
                    4, 5, 6, 7,
                    8, 9, 10, 11,
                    12, 13, 14, 15);
            Assert.AreEqual(new Matrix4(0, 4, 8, 12,
                              1, 5, 9, 13,
                              2, 6, 10, 14,
                              3, 7, 11, 15), a.Transpose());

            Matrix4 at = a.Transpose();
            Assert.AreEqual(a.Transpose(), at);

            // (ABC)T = CT*BT*CT
            Matrix4 b = new Matrix4(16, 17, 18, 19,
                            20, 21, 22, 23,
                            24, 25, 26, 27,
                            28, 29, 30, 31);
            Matrix4 c = new Matrix4(32, 33, 34, 35,
                            36, 37, 38, 39,
                            40, 41, 42, 43,
                            44, 45, 46, 47);
            Assert.AreEqual((a * b * c).Transpose(),
                      c.Transpose() * b.Transpose() * a.Transpose());
        }

        [TestMethod()]
        public void Matrix4_ToStringTest()
        {
            Assert.AreEqual("Matrix4( row0{1 0 0 0 } row1{0 1 0 0 } row2{0 0 1 0 } row3{0 0 0 1 })",
                Matrix4.Identity.ToString());
        }

        [TestMethod()]
        public void Matrix4_Multiply_By_Vector3_Test()
        {
            var m = new Matrix4(0, 1, 2, 3,
                                4, 5, 6, 7,
                                8, 9, 10, 11,
                                12, 13, 14, 15);
            var v = new Vector3(1, 2, 3);

            float inverseW = 1.0f / (v.x * m[0, 3] + v.y * m[1, 3] + v.z * m[2, 3] + m[3, 3]);
            var expectedX = (v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0] + m[3, 0]) * inverseW;
            var expectedY = (v.x * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1] + m[3, 1]) * inverseW;
            var expectedZ = (v.x * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2] + m[3, 2]) * inverseW;
            var expected = new Vector3(expectedX, expectedY, expectedZ);
            Assert.AreEqual(expected, v * m);
        }

        [TestMethod()]
        public void Matrix4_Translation_Test()
        {
            var transDelta = new Vector3(1.0f, 2.0f, 3.0f);
            var m1 = Matrix4.CreateTranslation(transDelta);
            Assert.AreEqual(transDelta, m1.Translation);

            var m2 = Matrix4.Identity;
            m2.Translation = transDelta;
            Assert.AreEqual(m1, m2);

            var v3 = new Vector3(0.0f, 1.0f, 2.0f);
            Assert.AreEqual(v3 + transDelta, v3 * m1);
        }

        [TestMethod()]
        public void Matrix4_Quaternion_Test()
        {
            var rotate = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2.0f));
            var m = Matrix4.CreateFromQuaternion(rotate);
            TestHelpers.QuaternionAreNearEqual(rotate, m.Quaternion);
            TestHelpers.AreEqual(-Vector3.UnitZ, Vector3.UnitX * m);
        }

        [TestMethod()]
        public void Matrix4_Scale_Test()
        {
            var scale = new Vector3(1.0f, 2.0f, 3.0f);
            var m = Matrix4.CreateScale(scale);
            Assert.AreEqual(scale, m.Scale);

            scale = new Vector3(2.0f, 3.0f, 4.0f);
            m.Scale = scale;
            Assert.AreEqual(scale, m.Scale);

            var v = new Vector3(1.0f, 2.0f, 3.0f);
            var expected = new Vector3(v.x * scale.x, v.y * scale.y, v.z * scale.z);
            Assert.AreEqual(expected, v * m);
        }

        [TestMethod()]
        public void Matrix4_GetSetMatrix3_Test()
        {
            var m3x3 = new Matrix3(0, 3, 6,
                                   1, 4, 7,
                                   2, 5, 8);
            var m4x4 = new Matrix4(m3x3);
            Assert.AreEqual(m3x3, m4x4.Matrix3);

            m3x3 = new Matrix3(0, 1, 2,
                               3, 4, 5,
                               6, 7, 8);
            m4x4.Matrix3 = m3x3;
            Assert.AreEqual(m3x3, m4x4.Matrix3);
        }
    }
}
