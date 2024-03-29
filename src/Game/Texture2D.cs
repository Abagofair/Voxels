﻿using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class Texture2D : IDisposable
    {
        private int _handle;

        private readonly int _width;
        private readonly int _height;

        public Texture2D(
            PixelInternalFormat internalFormat,
            int width, 
            int height, 
            byte[] bytes,
            bool generateMipMaps)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            _width = width;
            _height = height;

            CreateTexture(bytes, internalFormat, generateMipMaps);
        }

        private void CreateTexture(byte[] bytes, PixelInternalFormat internalFormat, bool generateMipMaps)
        {
            _handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                internalFormat,
                _width,
                _height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                bytes);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (generateMipMaps)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
        }

        public int Width => _width;
        public int Height => _height;
        public int Handle => _handle;
        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Dispose()
        {
            GL.DeleteTexture(_handle);
        }
    }
}
