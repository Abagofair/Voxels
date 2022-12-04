using Game.Editor;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Game
{
    internal class Game : GameWindow
    {
        private DirectionalLight _directionalLight;

        private Shader? _shader;

        private Shader? _gridShader;

        private Shader? _skyBoxShader;

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

            _time.Start();

            CursorState = CursorState.Normal;

            _imguiEditor.Initialize();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);

            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _controller.MouseScroll(e.Offset);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            ActiveScene.Camera.AspectRatio = (float)e.Width / e.Height;
            _controller.WindowResized(e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            _time.Update();

            ActiveScene.UpdateInput(KeyboardState, MouseState);
            ActiveScene.Update(_time);
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
