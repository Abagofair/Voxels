using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game
{
    internal class Model : IDisposable, IDrawable
    {
        private readonly int _vbo;
        private readonly int _vao;
        private readonly int _ebo;

        private readonly int _indexCount;

        public Model(
            VertexPositionNormalUv[] vertices,
            ushort[] indices)
        {
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();
            _vao = GL.GenVertexArray();

            _indexCount = indices.Length;

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * VertexPositionNormalUv.Size,
                vertices,
                BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(ushort),
                indices,
                BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(BeginMode.Triangles, _indexCount, DrawElementsType.UnsignedShort, 0);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_ebo);
            GL.DeleteBuffer(_vbo);
        }

        public static Model CreateQuadNDC()
        {
            return new Model(
                new VertexPositionNormalUv[]
                {
                    new VertexPositionNormalUv(new Vector3(1.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalUv(new Vector3(1.0f, -1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionNormalUv(new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalUv(new Vector3(-1.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f))
                },
                new ushort[] { 0, 1, 3, 1, 2, 3 });
        }

        public static Model CreateQuad()
        {
            return new Model(
                new VertexPositionNormalUv[]
                {
                    new VertexPositionNormalUv(new Vector3(5.0f, 5.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalUv(new Vector3(5.0f, -5.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionNormalUv(new Vector3(-5.0f, -5.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalUv(new Vector3(-5.0f, 5.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f))
                },
                new ushort[] { 0, 1, 3, 1, 2, 3 });
        }

        public static Model CreateVoxelMesh(Block[] blocks)
        {
            return null;
        }

        public static VertexPositionColorNormalUv[] Cube() //CW
        {
            return new VertexPositionColorNormalUv[]
                {
                    //LEFT (-X)
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    //RIGHT (+X)
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    //FRONT (-Z)
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 0.0f)),
                    //BACK (+Z)
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)),
                    //BOTTOM (-Y)
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f)),
                    //TOP (+Y)
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                    new VertexPositionColorNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f))
                };
        }

        public static Model CreateUnitCube()
        {
            //https://github.com/0Camus0/T800-Project/commit/858f82da9f1e78df4e0a760ab0e5a3b27a7321d2
            //swapped v0 -> v2 so it's clockwise winding order
            return new Model(
                new VertexPositionNormalUv[]
                {
                    //+Y side
                    new VertexPositionNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f)), // 0
                    new VertexPositionNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)), // 1
                    new VertexPositionNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)), // 2
                    new VertexPositionNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)), // 3

                    //-Y side
                    new VertexPositionNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 0.0f)), // 0
                    new VertexPositionNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 0.0f)), // 1
                    new VertexPositionNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f)), // 2
                    new VertexPositionNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f)), // 3

                    //+X side
                    new VertexPositionNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)), // 0
                    new VertexPositionNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)), // 1
                    new VertexPositionNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)), // 2
                    new VertexPositionNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)), // 3

                    //-X side
                    new VertexPositionNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)), // 0
                    new VertexPositionNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)), // 1
                    new VertexPositionNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)), // 2
                    new VertexPositionNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)), // 3

                    //+Z side - world is left-handed
                    new VertexPositionNormalUv(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 1.0f)), // 0
                    new VertexPositionNormalUv(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 1.0f)), // 1
                    new VertexPositionNormalUv(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 0.0f)), // 2
                    new VertexPositionNormalUv(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 0.0f)), // 3

                    //-Z side
                    new VertexPositionNormalUv(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)), // 0
                    new VertexPositionNormalUv(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)), // 1
                    new VertexPositionNormalUv(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)), // 2
                    new VertexPositionNormalUv(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f)), // 3
                },
                new ushort[]
                {
                    //+X
                    8, 9, 10, 9, 11, 10,

                    //-X
                    14, 13, 12, 14, 15, 13,

                    //+Y
                    1, 2, 0, 3, 2, 1,

                    //-Y
                    4, 6, 5, 5, 6, 7,

                    //+Z
                    17, 18, 16, 19, 18, 17,

                    //-Z
                    20, 22, 21, 21, 22, 23
                });
        }
    }
}
