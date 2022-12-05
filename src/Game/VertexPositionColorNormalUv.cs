using OpenTK.Mathematics;

namespace Game
{
    internal struct VertexPositionColorNormalUv
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Uv;
        public Vector4 Color;

        public VertexPositionColorNormalUv(
            Vector3 position,
            Vector3 normal,
            Vector2 uv,
            Vector4? color = null)
        {
            Position = position;
            Normal = normal;
            Uv = uv;
            Color = color ?? Vector4.One;
        }

        public static int Size => 48;
    }
}
