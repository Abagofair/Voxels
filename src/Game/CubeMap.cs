using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class CubeMap
    {
        private readonly int _handle;

        public CubeMap(
            IList<Image> faces)
        {
            _handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, _handle);

            for (int i = 0; i < faces.Count; ++i)
            {
                var img = faces[i];
                GL.TexImage2D(
                    TextureTarget.TextureCubeMapPositiveX + i,
                    0,
                    PixelInternalFormat.Srgb,
                    img.Width,
                    img.Height,
                    0,
                    PixelFormat.Rgb,
                    PixelType.UnsignedByte,
                    img.Bytes);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        }

        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, _handle);
        }
    }
}
