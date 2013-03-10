using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class CameraTest
    {
        [TestMethod()]
        public void Camera_EyePos_Test()
        {
            Camera camera = new Camera();
            Assert.AreEqual(Vector3.Zero, camera.EyePos);

            var newPos = new Vector3(1.0f, 2.0f, 3.0f);
            camera.EyePos = newPos;
            Assert.AreEqual(newPos, camera.EyePos);
        }

        [TestMethod()]
        public void Camera_Direction_Test()
        {
            var camera = new Camera();
            Assert.AreEqual(Vector3.UnitZ, camera.Direction);
            
            var dir = new Vector3(1.0f, 2.0f, 3.0f);
            camera.Direction = dir;
            TestHelpers.AreEqual(dir.Normalize(), camera.Direction);

            var eyePos = new Vector3(1.0f, 2.0f, 3.0f);
            camera.EyePos = eyePos;
            var lookAt = new Vector3(4.0f, 5.0f, 6.0f);
            camera.LookAt = lookAt;
            TestHelpers.AreEqual((lookAt - eyePos).Normalize(), camera.Direction);

            // Set zero vector has no effect
            var oldDir = camera.Direction;
            camera.Direction = Vector3.Zero;
            Assert.AreEqual(oldDir, camera.Direction);
        }

        [TestMethod()]
        public void Camera_UpVec_Test()
        {
            Camera camera = new Camera();
            Assert.AreEqual(Vector3.UnitY, camera.UpVec);

            var upVec = new Vector3(1.0f, 2.0f, 3.0f);
            camera.UpVec = upVec;
            Assert.AreEqual(upVec, camera.UpVec);
        }

        [TestMethod()]
        public void Camera_ViewMatrix_Test()
        {
            Camera camera = new Camera();
            camera.EyePos = new Vector3(1.0f, 2.0f, 3.0f);
            camera.LookAt = new Vector3(0.0f, 0.0f, 0.0f);
            camera.UpVec = new Vector3(2.0f, 3.0f, 4.0f);

            var zaxis = camera.Direction;
            var xaxis = Vector3.Cross(camera.UpVec, zaxis).Normalize();
            var yaxis = Vector3.Cross(zaxis, xaxis);
            var col0 = new Vector4(xaxis, -Vector3.Dot(xaxis, camera.EyePos));
            var col1 = new Vector4(yaxis, -Vector3.Dot(yaxis, camera.EyePos));
            var col2 = new Vector4(zaxis, -Vector3.Dot(zaxis, camera.EyePos));
            var expected = new Matrix4(col0, col1, col2, new Vector4(0, 0, 0, 1));

            Assert.AreEqual(expected, camera.ViewMatrix);
        }

        [TestMethod()]
        public void Camera_FovY_Test()
        {
            var camera = new Camera();
	        Assert.AreEqual(new Radian(MathUtils.PI / 4.0f), camera.FovY);

            var expectedFovY = new Radian(2.0f);
	        camera.FovY = expectedFovY;
	        Assert.AreEqual(expectedFovY, camera.FovY);

	        // FieldOfView is between 0 and pi (180 degrees)
	        camera.FovY = new Radian(0.0f);
	        camera.FovY = new Radian(MathUtils.PI);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Camera_FovY_Invalid_Under0_Test()
        {
            var camera = new Camera();
            camera.FovY = new Radian(-0.1f);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Camera_FovY_Invalid_OverPI_Test()
        {
            var camera = new Camera();
            camera.FovY = new Radian(MathUtils.PI * 1.1f);
        }

        [TestMethod()]
        public void Camera_AspectRatio_Test()
        {
            var camera = new Camera();
            Assert.AreEqual(1.0f / 3, camera.AspectRatio);

            var expected = 800.0f / 600.0f;
            camera.AspectRatio = expected;
            Assert.AreEqual(expected, camera.AspectRatio);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Camera_AspectRatio_Invalid_ZeroValue_Test()
        {
            var camera = new Camera();
            camera.AspectRatio = 0.0f;
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Camera_AspectRatio_Invalid_NegativeValue_Test()
        {
            var camera = new Camera();
            camera.AspectRatio = -1.0f;
        }

        [TestMethod()]
        public void Camera_PlaneDistance_Test()
        {
            var camera = new Camera();
            Assert.AreEqual(1.0f, camera.NearPlaneDistance);
            Assert.AreEqual(100000.0f, camera.FarPlaneDistance);

            var expectedNear = 200.0f;
            var expectedFar = 50000.0f;
            camera.SetPlaneDistance(expectedNear, expectedFar);
            Assert.AreEqual(expectedNear, camera.NearPlaneDistance);
            Assert.AreEqual(expectedFar, camera.FarPlaneDistance);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentException))]
        public void Camera_PlaneDistance_FarShouldBeLargerThanNear_Test()
        {
            var camera = new Camera();
            camera.SetPlaneDistance(100.0f, 100.0f);
        }

        [TestMethod()]
        public void Camera_ProjectionMatrix_Test()
        {
            var camera = new Camera();
            var projMat = camera.ProjectionMatrix;

            // FovY changes
            var oldFovY = camera.FovY;
            camera.FovY = oldFovY / 2;
            Assert.AreNotEqual(projMat, camera.ProjectionMatrix);
            camera.FovY = oldFovY;  // restore
            Assert.AreEqual(projMat, camera.ProjectionMatrix);

            // Aspect ratio changes
            var oldAspect = camera.AspectRatio;
            camera.AspectRatio = oldAspect / 2;
            Assert.AreNotEqual(projMat, camera.ProjectionMatrix);
            camera.AspectRatio = oldAspect;
            Assert.AreEqual(projMat, camera.ProjectionMatrix);

            // Plane distance changes
            var oldNear = camera.NearPlaneDistance;
            var oldFar = camera.FarPlaneDistance;
            camera.SetPlaneDistance(oldNear / 2, oldFar / 2);
            Assert.AreNotEqual(projMat, camera.ProjectionMatrix);
            camera.SetPlaneDistance(oldNear, oldFar);
            Assert.AreEqual(projMat, camera.ProjectionMatrix);
        }

        [TestMethod()]
        public void Camera_Orientation_Test()
        {
            var camera = new Camera();
            Assert.AreEqual(Quaternion.Identity, camera.Orientation);

            var newOrientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(MathUtils.PI / 2.0f));
            camera.Orientation = newOrientation;
            Assert.AreEqual(newOrientation, camera.Orientation);
        }

        [TestMethod()]
        public void Camera_Yaw_Test()
        {
            var camera = new Camera();
            var angle = new Radian(MathUtils.PI / 2); 
            
            camera.Yaw(angle);
            var expected = Quaternion.CreateFromAxisAngle(Vector3.UnitY, angle);
            TestHelpers.QuaternionAreNearEqual(expected, camera.Orientation);
        }

        [TestMethod()]
        public void Camera_Pitch_Test()
        {
            var camera = new Camera();
            var angle = new Radian(MathUtils.PI / 2);

            camera.Pitch(angle);
            var expected = Quaternion.CreateFromAxisAngle(Vector3.UnitX, angle);
            TestHelpers.QuaternionAreNearEqual(expected, camera.Orientation);
        }

        [TestMethod()]
        public void Camera_Roll_Test()
        {
            var camera = new Camera();
            var angle = new Radian(MathUtils.PI / 2);

            camera.Roll(angle);
            var expected = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle);
            TestHelpers.QuaternionAreNearEqual(expected, camera.Orientation);
        }

        [TestMethod()]
        public void Camera_Move_Test()
        {
            var camera = new Camera();
            camera.Direction = Vector3.UnitY;
            
            var moveVec = new Vector3(1.0f, 1.0f, 0.0f);
            camera.Move(moveVec);
            
            Assert.AreEqual(moveVec, camera.EyePos);
        }

        [TestMethod()]
        public void Camera_MoveRelative_Test()
        {
            var camera = new Camera()
            {
                EyePos = Vector3.UnitX,
                Direction = Vector3.UnitZ
            };

            var moveVec = new Vector3(1.0f, 1.0f, 0.0f);

            camera.MoveRelative(moveVec);
            TestHelpers.AreEqual(Vector3.UnitX + moveVec, camera.EyePos);
        }
    }
}
