using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class CubeMap : IDisposable
    {
        private readonly int _handle;

        public CubeMap(CubeMap.Images images)
        {
            if (images == null) throw new ArgumentNullException(nameof(images));

            _handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, _handle);

            CreateSide(images.Top, TextureTarget.TextureCubeMapPositiveX);
            CreateSide(images.Left, TextureTarget.TextureCubeMapNegativeX);
            CreateSide(images.Front, TextureTarget.TextureCubeMapPositiveY);
            CreateSide(images.Right, TextureTarget.TextureCubeMapNegativeY);
            CreateSide(images.Back, TextureTarget.TextureCubeMapPositiveZ);
            CreateSide(images.Bottom, TextureTarget.TextureCubeMapNegativeZ);

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

        public void Dispose()
        {
            GL.DeleteTexture(_handle);
        }

        private void CreateSide(Image image, TextureTarget textureTarget)
        {
            GL.TexImage2D(
            textureTarget,
            0,
            PixelInternalFormat.Srgb,
            image.Width,
            image.Height,
            0,
            PixelFormat.Rgb,
            PixelType.UnsignedByte,
            image.Bytes);
        }

        public class Images
        {
            public Images(
                Image top,
                Image left,
                Image front,
                Image right,
                Image back,
                Image bottom)
            {
                Top = top ?? throw new ArgumentNullException(nameof(top));
                Left = left ?? throw new ArgumentNullException(nameof(left));
                Front = front ?? throw new ArgumentNullException(nameof(front));
                Right = right ?? throw new ArgumentNullException(nameof(right));
                Back = back ?? throw new ArgumentNullException(nameof(back));
                Bottom = bottom ?? throw new ArgumentNullException(nameof(bottom));
            }

            public Image Top { get; }
            public Image Left { get; }
            public Image Front { get; }
            public Image Right { get; }
            public Image Back { get; }
            public Image Bottom { get; }
        }
    }
}
