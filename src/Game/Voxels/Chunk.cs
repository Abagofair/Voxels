using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game.Voxels
{
    internal class Chunk : IDisposable, IDrawable
    {
        private readonly int _vbo;
        private readonly int _vao;

        private readonly int _vertexCount;

        public Chunk(MagicaVoxelImporter.Chunk chunk)
        {
            _vbo = GL.GenBuffer();
            _vao = GL.GenVertexArray();

            GL.BindVertexArray(_vao);

            var vertices = Model.Cube();
            _vertexCount = chunk.Blocks.Length * vertices.Length;
            var voxelVertices = new VertexPositionColorNormalUv[_vertexCount];
            for (int i = 0; i < chunk.Blocks.Length; i++)
            {
                for (int j = 0; j < vertices.Length; j++)
                {
                    var vertex = vertices[j];
                    voxelVertices[i * vertices.Length + j] = new VertexPositionColorNormalUv(
                        vertex.Position + new OpenTK.Mathematics.Vector3(chunk.Blocks[i].Position.X, chunk.Blocks[i].Position.Y, chunk.Blocks[i].Position.Z),
                        vertex.Normal,
                        vertex.Uv,
                        new OpenTK.Mathematics.Vector4(chunk.Blocks[i].Color.X, chunk.Blocks[i].Color.Y, chunk.Blocks[i].Color.Z, chunk.Blocks[i].Color.W));
                }
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                _vertexCount * VertexPositionColorNormalUv.Size,
                voxelVertices,
                BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 12 * sizeof(float), 8 * sizeof(float));

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
        }
    }
}
