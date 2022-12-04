using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Formats.Asn1.AsnWriter;

namespace Game
{
    internal class EditorCamera
    {
        private float _fov;
        private float _near;
        private float _far;
        private float _aspectRatio;

        const float MaxPitch = (89.0f * MathF.PI) / 180.0f;

        private float _yaw = MathF.PI / 2.0f;
        private float _pitch = 0.0f;

        private Vector3 _worldPosition = Vector3.Zero;
        private Vector3 _worldUp = Vector3.UnitY;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _forward = Vector3.UnitZ;
        private Vector3 _right = Vector3.UnitX;

        private Matrix4 _perspective;
        private Matrix4 _view;
        private Matrix4 _viewWithoutTranslation;

        public event Action? ViewChanged;
        public event Action? PerspectiveChanged;

        public EditorCamera(
            float fov,
            float aspectRatio,
            float near,
            float far)
        {
            _fov = fov;
            _aspectRatio = aspectRatio;
            _near = near;
            _far = far;

            _worldPosition = new Vector3(0.0f, 10.0f, -35.0f);

            UpdatePerspective();
            UpdateView();
        }

        public ref Vector3 Forward => ref _forward;
        public ref Vector3 Position => ref _worldPosition;
        public ref Matrix4 Projection => ref _perspective;
        public ref Matrix4 View => ref _view;
        public ref Matrix4 ViewWithoutTranslation => ref _viewWithoutTranslation;

        public void MoveLeft(Time time)
        {
            _worldPosition -= _right * 50.0f * time.DeltaTimeF;

            UpdateView();
        }

        public void MoveRight(Time time)
        {
            _worldPosition += _right * 50.0f * time.DeltaTimeF;

            UpdateView();
        }

        public void MoveForward(Time time)
        {
            _worldPosition += _forward * 50.0f * time.DeltaTimeF;

            UpdateView();
        }

        public void MoveBackward(Time time)
        {
            _worldPosition -= _forward * 50.0f * time.DeltaTimeF;

            UpdateView();
        }

        public void Rotate(MouseState mouseState, Time time)
        {
            _yaw += 0.25f * mouseState.Delta.X * time.DeltaTimeF;
            _pitch -= 0.25f * mouseState.Delta.Y * time.DeltaTimeF;

            if (_pitch > MaxPitch)
            {
                _pitch = MaxPitch;
            }
            else if (_pitch < -MaxPitch)
            {
                _pitch = -MaxPitch;
            }

            UpdateView();
        }

        public void VerticalHorizontal(MouseState mouseState, Time time)
        {
            _worldPosition += _up * mouseState.Delta.Y * time.DeltaTimeF;
            _worldPosition -= _right * mouseState.Delta.X * time.DeltaTimeF;
            UpdateView();
        }

        public void Dolly(float scale, Time time)
        {
            _worldPosition += 125.0f * _forward * scale * time.DeltaTimeF;
            UpdateView();
        }

        public float AspectRatio
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                UpdatePerspective();
            }
        }

        private void UpdatePerspective()
        {
            _perspective = MathHelpers.CreatePerspectiveFieldOfViewLH(_fov, _aspectRatio, _near, _far);

            PerspectiveChanged?.Invoke();
        }

        private void UpdateView()
        {
            _forward = Vector3.Normalize(
                new Vector3(
                    -MathF.Cos(_yaw) * MathF.Cos(_pitch),
                    MathF.Sin(_pitch),
                    MathF.Sin(_yaw) * MathF.Cos(_pitch)));

            _right = Vector3.Normalize(Vector3.Cross(_worldUp, _forward));
            _up = Vector3.Cross(_forward, _right);

            //ensure the world moves opposite to the camera
            var tran = Matrix4.CreateTranslation(-_worldPosition);
            //ensure the world rotates opposite the camera by creating a transposed camera basis (row to column)
            var view = new Matrix4()
            {
                Row0 = new Vector4(_right.X, _up.X, _forward.X, 0.0f),
                Row1 = new Vector4(_right.Y, _up.Y, _forward.Y, 0.0f),
                Row2 = new Vector4(_right.Z, _up.Z, _forward.Z, 0.0f),
                Row3 = Vector4.UnitW
            };

            _view = tran * view;

            _viewWithoutTranslation = view;

            ViewChanged?.Invoke();
        }
    }
}
