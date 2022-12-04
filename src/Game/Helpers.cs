using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal static class Helpers
    {
        public static void SaveScreenshot(RenderTarget renderTarget, string tag = "")
        {
            renderTarget.Bind();

            var pixels = new byte[renderTarget.ViewportSize.X * renderTarget.ViewportSize.Y * 4];
            GL.ReadPixels(
                0,
                0,
                renderTarget.ViewportSize.X,
                renderTarget.ViewportSize.Y,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                pixels);

            using (var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(pixels, 
                renderTarget.ViewportSize.X, renderTarget.ViewportSize.Y))
            {
                image.Mutate(x => x.Flip(FlipMode.Vertical));
                image.SaveAsPng($"screenshot_{tag}_{DateTimeOffset.UtcNow.Ticks}.png");
            }

            RenderTarget.Unbind();
        }
    }
}
