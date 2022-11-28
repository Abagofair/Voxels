using OpenTK.Mathematics;

namespace Game
{
    internal class Transform
    {
        private Matrix4 _matrix = Matrix4.Identity;

        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 Scale = Vector3.One;

        public ref Matrix4 Matrix
        {
            get
            {
                _matrix =
                    Matrix4.CreateScale(Scale) *
                    Matrix4.CreateFromQuaternion(Rotation) *
                    Matrix4.CreateTranslation(Position);

                return ref _matrix;
            }
        }

        public void AddTransform(Transform transform)
        {
            _matrix *= transform.Matrix;
        }
    }
}