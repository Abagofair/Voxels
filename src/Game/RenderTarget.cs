using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game
{
    internal class RenderTarget : IDisposable
    {
        private readonly int _frameBufferId;
        private readonly int _colorTextureId;
        private readonly int _depthStencilRboId;

        public RenderTarget(Vector2i viewportSize)
        {
            ViewportSize = viewportSize;

            _frameBufferId = GL.GenFramebuffer();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferId);

            _colorTextureId = GL.GenTexture();

            //create color texture
            GL.BindTexture(TextureTarget.Texture2D, _colorTextureId);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgb,
                viewportSize.X,
                viewportSize.Y,
                0,
                PixelFormat.Rgb,
                PixelType.UnsignedByte,
                Array.Empty<byte>());
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            //attach color texture to framebuffer
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D,
                _colorTextureId,
                0);

            _depthStencilRboId = GL.GenRenderbuffer();
            //create depthstencil rbo
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _depthStencilRboId);
            GL.RenderbufferStorage(
                RenderbufferTarget.Renderbuffer,
                RenderbufferStorage.Depth24Stencil8,
                viewportSize.X,
                viewportSize.Y);

            //attach depthstencil to framebuffer
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthStencilAttachment,
                RenderbufferTarget.Renderbuffer, _depthStencilRboId);

            var code = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (code != FramebufferErrorCode.FramebufferComplete)
                throw new Exception(code.ToString());

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public int ColorTextureId => _colorTextureId;

        public Vector2i ViewportSize { get; set; }

        public void Bind()
        {
            GL.Viewport(0, 0, ViewportSize.X, ViewportSize.Y);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferId);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(_frameBufferId);
            GL.DeleteRenderbuffer(_colorTextureId);
            GL.DeleteTexture(_depthStencilRboId);
        }
    }
}
