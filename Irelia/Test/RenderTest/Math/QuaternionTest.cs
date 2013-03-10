using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class QuaternionTest
    {
        [TestMethod()]
        public void Quaternion_Constructor_Test()
        {
            var q = new Quaternion();
            Assert.AreEqual(Quaternion.Zero, q);

            q = new Quaternion(0.1f, 0.22f, 0.333f, 0.4444f);
            Assert.AreEqual(0.1f, q.w);
            Assert.AreEqual(0.22f, q.x);
            Assert.AreEqual(0.333f, q.y);
            Assert.AreEqual(0.4444f, q.z);
        }

        [TestMethod()]
        public void Quaternion_Equality_Test()
        {
            var a = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            var b = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            Assert.AreEqual(b, a);

            var c = new Quaternion(1.0f, 2.0f, 0.0f, -1.0f);
            Assert.AreNotEqual(c, a);
        }

        [TestMethod()]
        public void Quaternion_CreateFromAxisAngle_Test()
        {
            var actual = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(MathUtils.PI / 2.0f));
            var expected = SlimDX.Quaternion.RotationAxis(SlimDX.Vector3.UnitX, MathUtils.PI / 2.0f).ToQuaternion();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Quaternion_Negation_Test()
        {
            var actual = -(new Quaternion(1.0f, 2.0f, 3.0f, 4.0f));
            var expected = new Quaternion(-1.0f, -2.0f, -3.0f, -4.0f);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Quaternion_Conjugate_Test()
        {
            var actual = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f).Conjugate();
            var expected = new Quaternion(1.0f, -2.0f, -3.0f, -4.0f);
            Assert.AreEqual(expected, actual);

            var expected2 = SlimDX.Quaternion.Conjugate(new SlimDX.Quaternion(2.0f, 3.0f, 4.0f, 1.0f)).ToQuaternion();
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod()]
        public void Quaternion_Length_Test()
        {
            var q = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            var expectedLengthSquared = q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z;
            Assert.AreEqual(expectedLengthSquared, q.LengthSquared);
            Assert.AreEqual(MathUtils.Sqrt(expectedLengthSquared), q.Length);

            Assert.AreEqual(new SlimDX.Quaternion(2.0f, 3.0f, 4.0f, 1.0f).Length(), q.Length);
        }

        [TestMethod()]
        public void Quaternion_Inverse_Test()
        {
            var q = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            var expected = q.Conjugate() * (1 / q.Length);
            Assert.AreEqual(expected, q.Inverse());

            Assert.AreEqual(Quaternion.Zero, Quaternion.Zero.Inverse());

            var q2 = SlimDX.Quaternion.Invert(new SlimDX.Quaternion(2.0f, 3.0f, 4.0f, 1.0f));
            q2.Normalize();
            Assert.AreEqual(q2.ToQuaternion(), q.Inverse());
        }

        [TestMethod()]
        public void Quaternion_Multiply_Scalar_Test()
        {
            var q = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            var scalar = 0.12f;
            var expected = new Quaternion(q.w * scalar, q.x * scalar, q.y * scalar, q.z * scalar);
            Assert.AreEqual(expected, q * scalar);
        }

        [TestMethod()]
        public void Quaternion_Multiply_Quaternion_Test()
        {
            var a = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(MathUtils.PI / 4));
            var b = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(MathUtils.PI / 4));
            var expected = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(MathUtils.PI / 2));
            TestHelpers.QuaternionAreNearEqual(a, b);

            // Not commutative
            var c = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 4));
            Assert.AreNotEqual(c * a, a * c);
        }

        [TestMethod()]
        public void Quaternion_Multiply_Vector_Test()
        {
            var q = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2));
            TestHelpers.AreEqual(-Vector3.UnitZ, Vector3.UnitX * q);
        }

        [TestMethod()]
        public void Quaternion_Subtract_Test()
        {
            var a = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            var b = new Quaternion(0.0f, -1.0f, -2.0f, -3.0f);
            var expected = new Quaternion(a.w - b.w, a.x - b.x, a.y - b.y, a.z - b.z);
            Assert.AreEqual(expected, a - b);
        }

        [TestMethod()]
        public void Quaternion_Normalize_Test()
        {
            var q = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
            Assert.AreEqual(1.0f, q.Normalize().Length);
        }

        [TestMethod()]
        public void Quaternion_PredefinedQuaternions_Test()
        {
            Assert.AreEqual(new Quaternion(1.0f, 0.0f, 0.0f, 0.0f), Quaternion.Identity);
            Assert.AreEqual(new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), Quaternion.Zero);
        }

        [TestMethod()]
        public void Quaternion_CreateFromRotationMatrix_Test()
        {
            var matrix = Matrix3.CreateFromYawPitchRoll(new Radian(MathUtils.PI / 2.0f), new Radian(0.0f), new Radian(0.0f));
            var actual = Quaternion.CreateFromRotationMatrix(matrix);
            var expected = SlimDX.Quaternion.RotationMatrix(matrix.ToD3DMatrix()).ToQuaternion();
            TestHelpers.QuaternionAreNearEqual(expected, actual);
        }

        [TestMethod()]
        public void Quaternion_ToRotationMatrix_Test()
        {
            var q = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2.0f));
            var actual = q.ToRotationMatrix();
            var expected = SlimDX.Matrix.RotationAxis(SlimDX.Vector3.UnitY, MathUtils.PI / 2.0f).ToMatrix4();
            TestHelpers.Matrix3AreNearEqual(actual, expected.Matrix3);
        }

        [TestMethod()]
        public void Quaternion_CreateFromAxes_Test()
        {
            var actual = Quaternion.CreateFromAxes(-Vector3.UnitZ, Vector3.UnitY, Vector3.UnitX);
            var expected = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Quaternion_ToAxes_Test()
        {
            var q = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2));
            
            Vector3 xAxis, yAxis, zAxis;
            q.ToAxes(out xAxis, out yAxis, out zAxis);

            TestHelpers.AreEqual(-Vector3.UnitZ, xAxis);
            TestHelpers.AreEqual(Vector3.UnitY, yAxis);
            TestHelpers.AreEqual(Vector3.UnitX, zAxis);
        }

        [TestMethod()]
        public void Quaternion_CreateFromVectorRotation()
        {
            var actual = Quaternion.CreateFromVectorRotation(Vector3.UnitX, -Vector3.UnitZ);
            var expected = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2));
            TestHelpers.QuaternionAreNearEqual(expected, actual);
        }

        [TestMethod()]
        public void Quaternion_ToString_Test()
        {
            var q = new Quaternion(0.1f, 0.22f, 0.333f, 0.4444f);
            Assert.AreEqual("Quaternion(0.1, 0.22, 0.333, 0.4444)", q.ToString());
        }
    }
}
