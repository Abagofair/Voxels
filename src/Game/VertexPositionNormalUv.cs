using OpenTK.Mathematics;

namespace Game
{
    internal struct VertexPositionNormalUv
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Uv;

        public VertexPositionNormalUv(
            Vector3 position,
            Vector3 normal,
            Vector2 uv)
        {
            Position = position;
            Normal = normal;
            Uv = uv;
        }

        public static int Size => 32;
    }
}
