using System;

namespace Irelia.Render
{
    public class Camera
    {
        #region Private fields
        private bool viewMatIsDirty = true;
        private Vector3 eyePos = Vector3.Zero;
        private Vector3 upVec = Vector3.UnitY;
        private Matrix4 viewMatrix = Matrix4.Identity;

        private bool projMatIsDirty = true;
        private Radian fovY = new Radian(MathUtils.PI / 4.0f);
        private float aspectRatio = 1.0f / 3;
        private float nearPlaneDistance = 1.0f;
        private float farPlaneDistance = 100000.0f;
        private Matrix4 projectionMatrix = Matrix4.Identity;
        #endregion

        #region Public properties
        public Vector3 EyePos
        {
            get { return this.eyePos; }
            set
            {
                if (this.eyePos == value)
                    return;

                this.eyePos = value;
                this.viewMatIsDirty = true;
            }
        }

        public Vector3 LookAt
        {
            set
            {
                Direction = value - EyePos;
                this.viewMatIsDirty = true;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return (Vector3.UnitZ * Orientation);
            }
            set
            {
                if (value == Vector3.Zero)
                    return;

                var newDir = value.Normalize();

                if (YawFixed)
                {
                    Vector3 xVec = Vector3.Cross(Vector3.UnitY, newDir).Normalize();
                    Vector3 yVec = Vector3.Cross(newDir, xVec).Normalize();
                    Orientation = Quaternion.CreateFromAxes(xVec, yVec, newDir);
                }
                else
                {
                    Vector3 xAxis, yAxis, zAxis;
                    Orientation.ToAxes(out xAxis, out yAxis, out zAxis);

                    Quaternion quat;
                    if (MathUtils.IsNearZero((zAxis + newDir).Length))
                    {
                        // Oops, a 180 degree turn. Default to yaw.
                        quat = Quaternion.CreateFromAxisAngle(yAxis, new Radian(MathUtils.PI));
                    }
                    else
                    {
                        // Shortes arc to new direction
                        quat = Quaternion.CreateFromVectorRotation(zAxis, newDir);
                    }
                    Orientation = quat * Orientation;
                }

                this.viewMatIsDirty = true;
            }
        }

        public Vector3 UpVec
        {
            get { return this.upVec; }
            set
            {
                if (this.upVec == value)
                    return;

                this.upVec = value;
                this.viewMatIsDirty = true;
            }
        }

        public Matrix4 ViewMatrix
        {
            get
            {
                if (this.viewMatIsDirty)
                {
                    var zaxis = Direction;
                    var xaxis = Vector3.Cross(UpVec, zaxis).Normalize();
                    var yaxis = Vector3.Cross(zaxis, xaxis);

                    this.viewMatrix.m00 = xaxis.x;
                    this.viewMatrix.m10 = xaxis.y;
                    this.viewMatrix.m20 = xaxis.z;
                    this.viewMatrix.m30 = -Vector3.Dot(xaxis, EyePos);

                    this.viewMatrix.m01 = yaxis.x;
                    this.viewMatrix.m11 = yaxis.y;
                    this.viewMatrix.m21 = yaxis.z;
                    this.viewMatrix.m31 = -Vector3.Dot(yaxis, EyePos);

                    this.viewMatrix.m02 = zaxis.x;
                    this.viewMatrix.m12 = zaxis.y;
                    this.viewMatrix.m22 = zaxis.z;
                    this.viewMatrix.m32 = -Vector3.Dot(zaxis, EyePos);

                    this.viewMatrix.m03 = 0;
                    this.viewMatrix.m13 = 0;
                    this.viewMatrix.m23 = 0;
                    this.viewMatrix.m33 = 1;

                    this.viewMatIsDirty = false;
                }
                return this.viewMatrix;
            }
        }

        public Radian FovY
        {
            get { return this.fovY; }
            set
            {
                if (this.fovY == value)
                    return;

                if (value.Value < 0.0f || value.Value > MathUtils.PI)
                    throw new ArgumentOutOfRangeException("value", value.ToString() + " must be in range from 0 to PI");

                this.fovY = value;
                this.projMatIsDirty = true;
            }
        }

        public float AspectRatio
        {
            get { return this.aspectRatio; }
            set
            {
                if (this.aspectRatio == value)
                    return;

                if (value <= 0.0f)
                    throw new ArgumentOutOfRangeException("value", value.ToString() + " must be a positive value"); 

                this.aspectRatio = value;
                this.projMatIsDirty = true;
            }
        }

        public float NearPlaneDistance
        {
            get { return this.nearPlaneDistance; }
        }

        public float FarPlaneDistance
        {
            get { return this.farPlaneDistance; }
        }

        public Matrix4 ProjectionMatrix
        {
            get
            {
                if (this.projMatIsDirty)
                {
                    var dxMatrix = SlimDX.Matrix.PerspectiveFovLH(FovY.Value, AspectRatio, NearPlaneDistance, FarPlaneDistance);
                    this.projectionMatrix = dxMatrix.ToMatrix4();
                    this.projMatIsDirty = false;
                }
                return this.projectionMatrix;
            }
        }

        public Quaternion Orientation
        {
            get;
            set;
        }

        public bool YawFixed
        {
            get;
            set;
        }
        #endregion

        public Camera()
        {
            Orientation = Quaternion.Identity;
            YawFixed = true;
        }

        #region Public methods
        public void SetPlaneDistance(float near, float far)
        {
            if (NearPlaneDistance == near &&
                FarPlaneDistance == far)
                return;

            if (near >= far)
                throw new ArgumentException(
                    "near(" + near.ToString() + ") should be smaller than far(" + far.ToString() + ")", 
                    "near & far");

            this.nearPlaneDistance = near;
            this.farPlaneDistance = far;
            this.projMatIsDirty = true;
        }

        public void Yaw(Radian angle)
        {
            Vector3 yAxis;
            if (YawFixed)
            {
                // Rotate around fixed yaw axis
                yAxis = Vector3.UnitY;
            }
            else
            {
                // Rotate around local Y axis
                yAxis = Vector3.UnitY * Orientation;
            }
            Rotate(yAxis, angle);
        }

        public void Pitch(Radian angle)
        {
            // Rotate around local Y axis
            var xAxis = Vector3.UnitX * Orientation;
            Rotate(xAxis, angle);
        }

        public void Roll(Radian angle)
        {
            Vector3 zAxis = Vector3.UnitZ * Orientation;
            Rotate(zAxis, angle);
        }

        public void Move(Vector3 vec)
        {
            EyePos += vec;
            this.viewMatIsDirty = true;
        }

        public void MoveRelative(Vector3 vec)
        {
            var moveVec = vec * Orientation;
            EyePos += moveVec;
            this.viewMatIsDirty = true;
        }

        public void LookMeshNode(MeshNode meshNode)
        {
            EyePos = meshNode.BoundingCenter + new Vector3(0, 0, meshNode.BoundingRadius * -3.0f);
            LookAt = meshNode.BoundingCenter;
        }
        #endregion

        #region Private methods
        private void Rotate(Vector3 axis, Radian angle)
        {
            var q = Quaternion.CreateFromAxisAngle(axis, angle).Normalize();
            Orientation = q * Orientation;
            this.viewMatIsDirty = true;
        }
        #endregion
    }
}
