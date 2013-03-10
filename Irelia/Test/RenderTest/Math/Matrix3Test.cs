using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class Matrix3Test
    {
        [TestMethod()]
        public void Matrix3_Constructor_Test()
        {
            var m = new Matrix3();
            Assert.AreEqual(Matrix3.Zero, m);

            m = new Matrix3(0, 3, 6,
                            1, 4, 7,
                            2, 5, 8);
            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 3; ++col)
                {
                    Assert.AreEqual(col * 3 + row, m[row, col]);
                }
            }
        }

        [TestMethod()]
        public void Matrix3_CreateFromVectors_Test()
        {
            var v1 = new Vector3(0, 1, 2);
            var v2 = new Vector3(3, 4, 5);
            var v3 = new Vector3(6, 7, 8);

            var actual = new Matrix3(v1, v2, v3);
            var expected = new Matrix3(v1.x, v2.x, v3.x,
                                       v1.y, v2.y, v3.y,
                                       v1.z, v2.z, v3.z);
        }

        [TestMethod()]
        public void Matrix3_Equality_Test()
        {
            var actual = new Matrix3(0, 3, 6,
                                     1, 4, 7,
                                     2, 5, 8);
            var expected = new Matrix3(0, 3, 6,
                                       1, 4, 7,
                                       2, 5, 8);
            Assert.AreEqual(expected, actual);

            Assert.AreNotEqual(Matrix3.Zero, actual);
        }

        [TestMethod()]
        public void Matrix3_Multiplication_Test()
        {
            var m1 = new Matrix3(0, 3, 6,
                                 1, 4, 7,
                                 2, 5, 8);
            var m2 = new Matrix3(9, 12, 15,
                                 10, 13, 16,
                                 11, 14, 17);
            var res = m1 * m2;
            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 3; ++col)
                {
                    var rowv = new Vector3(m1[row, 0], m1[row, 1], m1[row, 2]);
                    var colv = new Vector3(m2[0, col], m2[1, col], m2[2, col]);
                    Assert.AreEqual(Vector3.Dot(rowv, colv), res[row, col]);
                }
            }

            // Not commutative (AB != BA)
            Assert.AreNotEqual(m1 * m2, m2 * m1);
        }

        [TestMethod()]
        public void Matrix3_SpecialMatrices_Test()
        {
            // Zero matrix
            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 3; ++col)
                {
                    Assert.AreEqual(0, Matrix3.Zero[row, col]);
                }
            }

            // Identity matrix
            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 3; ++col)
                {
                    if (row == col)
                        Assert.AreEqual(1, Matrix3.Identity[row, col]);
                    else
                        Assert.AreEqual(0, Matrix3.Identity[row, col]);
                }
            }
        }

        [TestMethod()]
        public void Matrix3_CreateFromYawPitchRoll_Test()
        {
            Radian yaw = new Radian(MathUtils.PI / 6), 
                   pitch = new Radian(MathUtils.PI / 5),
                   roll = new Radian(MathUtils.PI / 4);
            var actual = Matrix3.CreateFromYawPitchRoll(yaw, pitch, roll);

            float cos = MathUtils.Cos(new Radian(yaw));
            float sin = MathUtils.Sin(new Radian(yaw));
            var yMat = new Matrix3(cos, 0, -sin, 0, 1, 0, sin, 0, cos);

            cos = MathUtils.Cos(new Radian(pitch));
            sin = MathUtils.Sin(new Radian(pitch));
            var xMat = new Matrix3(1, 0, 0, 0, cos, sin, 0, -sin, cos);

            cos = MathUtils.Cos(new Radian(roll));
            sin = MathUtils.Sin(new Radian(roll));
            var zMat = new Matrix3(cos, sin, 0, -sin, cos, 0, 0, 0, 1);

            Assert.AreEqual(zMat * xMat * yMat, actual);
        }

        [TestMethod()]
        public void Matrix3_Subtract_Test()
        {
            var m = new Matrix3(0, 3, 6,
                                1, 4, 7,
                                2, 5, 8);
            var actual = m - m;
            Assert.AreEqual(Matrix3.Zero, actual);
        }

        [TestMethod()]
        public void Matrix3_ToStringTest()
        {
            Assert.AreEqual("Matrix3( row0{1 0 0 } row1{0 1 0 } row2{0 0 1 })",
                Matrix3.Identity.ToString());
        }
    }
}
