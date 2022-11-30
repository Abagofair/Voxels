using Game.Debug;
using Game.Editor;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Game
{
    internal class Game : GameWindow
    {
        private DirectionalLight _directionalLight;

        private Shader? _shader;
        private FreeLookCamera? _camera;

        private Shader? _gridShader;
        private Thing? _gridThing;

        private Shader? _skyBoxShader;
        private Model? _skyBoxCube;

        private Shader? _stencilShader;

        private InputManager _inputManager;
        private Time _time;

        private Material _basicDiffuse;

        private Material _gridMaterial;

        private CubeMap _cubeMap;

        private ImGuiController _controller;
        private ImGuiMainWindow _imguiEditor;

        private Renderer _renderer;
        private Scene _scene;

        private Shader _renderTargetShader;
        private Texture2D _renderTargetTexture;
        private Model _renderTargetQuad;

        public Game(
            GameWindowSettings gameWindowSettings, 
            NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _time = new Time();
            _inputManager = new InputManager(_time);
            _imguiEditor = new ImGuiMainWindow(this, _time);
        }

        public Scene ActiveScene => _scene;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);

            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);

            _cubeMap = AssetManager.CreateCubeMap("SkyCubeMap");
            _skyBoxShader = AssetManager.CreateOrGetShader("skybox", ShaderType.Skybox, true);
            var skyboxMaterial = new SkyboxMaterial(_skyBoxShader, _cubeMap);

            _renderer = new Renderer(ClientSize);
            _scene = new Scene(this, _renderer, _inputManager, new Skybox(99, skyboxMaterial));

            _renderTargetShader = AssetManager.CreateOrGetShader("renderTargetPresent", ShaderType.RenderTargetPresent, true);
            _renderTargetQuad = Model.CreateQuadNDC();

            _shader = AssetManager.CreateOrGetShader("quad", ShaderType.MaterialShader, true);
            _basicDiffuse = new BasicMaterial(_shader, new MaterialProperties()
            {
                ambient = new Vector3(1.0f, 0.5f, 0.31f),
                diffuse = AssetManager.CreateTextureFromPng("container_diffuse", PixelInternalFormat.Srgb),
                specular = AssetManager.CreateTextureFromPng("container_specular", PixelInternalFormat.Rgb),
                shininess = 32.0f
            });

            var block0 = GameEntityManager.Create<GameEntity>("block0", new Transform());
            GameEntityManager.AddAsDynamicRenderable(block0, new Renderable(1, _basicDiffuse, Model.CreateUnitCube()));
            var block0Node = new SceneTree.Node(block0);
            _scene.SceneTree.Root.Children.Add(block0Node);

            var block1 = GameEntityManager.Create<GameEntity>("block1", new Transform());
            GameEntityManager.AddAsDynamicRenderable(block1, new Renderable(2, _basicDiffuse, Model.CreateUnitCube()));
            block0Node.Children.Add(new SceneTree.Node(block1));

            _gridShader = AssetManager.CreateOrGetShader("grid", ShaderType.Grid, true);
            _gridMaterial = new GridMaterial(_gridShader);
            var grid = GameEntityManager.Create<GameEntity>("editorGrid", new Transform());
            GameEntityManager.AddAsDynamicRenderable(grid, new Renderable(5, _gridMaterial, Model.CreateQuadNDC())
            {
                RenderOrder = 99
            });
            _scene.SceneTree.Root.Children.Add(new SceneTree.Node(grid));

            _stencilShader = AssetManager.CreateOrGetShader("stencil", ShaderType.ObjectSelect, true);

            //TAG LIGE NOGLE FUCKING NOTER SÅ DU IKKE GLEMMER ALT DET HER FRA MODEL TO PROJECTION DEPTH BUFFER LORT

            _camera = new FreeLookCamera(MathF.PI / 4.0f, (float)ClientSize.X / (float)ClientSize.Y, 0.01f, 1000.0f);

            _directionalLight = new DirectionalLight(2, "SunLight", new Transform())
            {
                direction = new Vector3(0.0f, -1.0f, 0.0f),
                lightProperties = new LightProperties()
                {
                    ambient = new Vector3(0.2f, 0.2f, 0.2f),
                    diffuse = new Vector3(0.5f, 0.5f, 0.5f),
                    specular = new Vector3(1.0f, 1.0f, 1.0f)
                }
            };
            _scene.SceneTree.Root.Children.Add(new SceneTree.Node(_directionalLight));

            _scene.Initialize();

            //_inputManager.AddMouseAction(MouseAxis.MouseX, _camera.MouseX);
            //_inputManager.AddMouseAction(MouseAxis.MouseY, _camera.MouseY);

            _inputManager.AddKeyAction(Keys.Escape, (_) =>
            {
                if (CursorState == CursorState.Grabbed)
                    CursorState = CursorState.Normal;
                else
                    CursorState = CursorState.Grabbed;
            });

            _inputManager.AddKeyAction(Keys.T, (_) =>
            {
                //GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer);
                var pixels = new byte[ClientSize.X * ClientSize.Y * 4];
                GL.ReadPixels(
                    0,
                    0,
                    ClientSize.X,
                    ClientSize.Y,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    pixels);

                using (var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(pixels, ClientSize.X, ClientSize.Y))
                {
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                    image.SaveAsPng($"screenshot_{DateTimeOffset.UtcNow.Ticks}.png");
                }
                //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                //i.SaveAsPng("test");
            });

            _time.Start();

            CursorState = CursorState.Normal;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            if (_camera!= null)
            {
                _camera.AspectRatio = (float)e.Width / e.Height;
                _controller.WindowResized(e.Width, e.Height);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            _time.Update();

            _inputManager.UpdateMouse(MouseState.Position, MouseState.Delta);
            _inputManager.HandleActions(KeyboardState);

            //_controller.Update(this, _time.DeltaTimeF);

            _scene.Update(_time);
        }
        
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _scene.Render();

            ////Write first draw to stencil buffer
            //GL.StencilOp(StencilOp.Keep, StencilOp.Replace, StencilOp.Replace);
            ////write 1's in the stencil buffer for the fragments where cube1 is drawn
            //GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            //GL.StencilMask(0xFF);

            //_shader.Use();
            //_cube1.Scale(1.0f);
            //_cube1.PrepareForDraw();
            //_cube1.Draw();

            //GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
            //GL.StencilMask(0x00);
            //GL.Disable(EnableCap.DepthTest);

            //_cube1.Scale(1.06f);
            //_stencilShader.SetMatrix4f(5, ref _cube1.Transform.Matrix);
            //_stencilShader.SetMatrix4f(6, ref _camera.View);
            //_stencilShader.SetMatrix4f(7, ref _camera.Perspective);
            //_stencilShader.Use();
            //_cube1.PrepareForDraw();
            //_cube1.DrawWithoutMaterial();

            ////POST
            //GL.StencilMask(0xFF);
            //GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
            //GL.Enable(EnableCap.DepthTest);

            /*_gridThing.PrepareForDraw();
            _gridShader.Use();
            _gridThing.Draw();*/

            //GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            _controller.Update(this, _time.DeltaTimeF);
            _imguiEditor.Show();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
        }

        private void PresentRenderTarget(RenderTarget renderTarget)
        {
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            ////var renderTargetTexture = renderTarget.ColorTextureId;

            ////_renderTargetShader.Use();
            ////GL.ActiveTexture(TextureUnit.Texture0);
            ////GL.BindTexture(TextureTarget.Texture2D, renderTargetTexture);
            ////_renderTargetQuad.Draw();

            //_imguiEditor.Show();
            //_controller.Render();
            //ImGuiController.CheckGLError("End of frame");

            //SwapBuffers();
        }
    }
}
