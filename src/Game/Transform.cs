using OpenTK.Mathematics;

namespace Game
{
    internal class Transform
    {
        private Matrix4 _matrix = Matrix4.Identity;

        public Matrix4 Parent = Matrix4.Identity;
        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;

        public float Yaw = 0.0f;
        public float Pitch = 0.0f;
        public float Roll = 0.0f;

        public Vector3 Scale = Vector3.One;

        public ref Matrix4 Matrix
        {
            get
            {
                _matrix =
                    Matrix4.CreateScale(Scale) *
                    Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(Pitch, Yaw, Roll)) *
                    Matrix4.CreateTranslation(Position) *
                    Parent;

                return ref _matrix;
            }
        }
    }
}