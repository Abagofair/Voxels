using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game
{
    internal class Renderer
    {
        private readonly RenderTarget _defaultRenderTarget;

        public Renderer(
            Vector2i viewportSize)
        {
            _defaultRenderTarget = new RenderTarget(viewportSize);
        }

        public RenderTarget RenderTarget => _defaultRenderTarget;

        public void Render(IEnumerable<Renderable> renderables)
        {
            _defaultRenderTarget.Bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            /*
             * foreach rendercommand
             *      rendercommand.submit
             *          
             */

            foreach (var renderable in renderables.OrderBy(x => x.RenderOrder))
            {
                renderable.Render();
            }
        }
    }
}
