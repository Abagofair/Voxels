using Game.Debug;
using Game.Editor;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game
{
    internal class Game : GameWindow
    {
        private DirectionalLight _directionalLight;
        private PointLight _pointLight;
        private SpotLight _spotLight;

        private Thing? _cube1;
        private Thing? _cube2;
        private Thing? _cube3;

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

        public Game(
            GameWindowSettings gameWindowSettings, 
            NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _time = new Time();
            _inputManager = new InputManager(_time);
            _imguiEditor = new ImGuiMainWindow(this, _time);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);

            var top = AssetManager.LoadImageJpg("top");
            var left = AssetManager.LoadImageJpg("left");
            var front = AssetManager.LoadImageJpg("front");
            var right = AssetManager.LoadImageJpg("right");
            var back = AssetManager.LoadImageJpg("back");
            var bottom = AssetManager.LoadImageJpg("bottom");

            var list = new List<Image>()
            {
                right, left, top, bottom, front, back
            };

            _stencilShader = AssetManager.CreateShader("stencil");

            _cubeMap = new CubeMap(list);
            _skyBoxShader = AssetManager.CreateShader("skybox");
            _skyBoxCube = Model.CreateUnitCube();

            _shader = AssetManager.CreateShader("quad");
            _gridShader = AssetManager.CreateShader("grid");

            _basicDiffuse = new Material(_shader, new MaterialProperties()
            {
                ambient = new Vector3(1.0f, 0.5f, 0.31f),
                diffuse = AssetManager.CreateTextureFromPng("container_diffuse", PixelInternalFormat.Srgb),
                specular = AssetManager.CreateTextureFromPng("container_specular", PixelInternalFormat.Rgb),
                shininess = 32.0f
            });

            _gridMaterial = new Material(_gridShader, new MaterialProperties());

            //TAG LIGE NOGLE FUCKING NOTER SÅ DU IKKE GLEMMER ALT DET HER FRA MODEL TO PROJECTION DEPTH BUFFER LORT

            _gridThing = new Thing(_gridMaterial, Vector3.Zero);

            _camera = new FreeLookCamera(MathF.PI / 4.0f, (float)ClientSize.X / (float)ClientSize.Y, 0.01f, 1000.0f);

            _cube1 = new Thing(Model.CreateUnitCube(), _basicDiffuse, new Vector3(0.0f, 0.0f, 0.0f));
            _cube2 = new Thing(Model.CreateUnitCube(), _basicDiffuse, new Vector3(0.0f, 0.0f, 5.0f));
            _cube3 = new Thing(Model.CreateUnitCube(), _basicDiffuse, new Vector3(0.0f, 0.0f, -5.0f));

            _pointLight = new PointLight()
            {
                position = new Vector3(0.0f, 2.0f, -5.0f),
                lightProperties = new LightProperties()
                {
                    ambient = new Vector3(0.2f, 0.1f, 0.4f),
                    diffuse = new Vector3(0.5f, 0.5f, 0.5f),
                    specular = new Vector3(1.0f, 1.0f, 1.0f)
                }
            };
            _pointLight.Upload(_shader);

            _directionalLight = new DirectionalLight()
            {
                direction = new Vector3(0.0f, -1.0f, -0.2f),
                lightProperties = new LightProperties()
                {
                    ambient = new Vector3(0.2f, 0.2f, 0.2f),
                    diffuse = new Vector3(0.5f, 0.5f, 0.5f),
                    specular = new Vector3(1.0f, 1.0f, 1.0f)
                }
            };
            _directionalLight.Upload(_shader);

            _spotLight = new SpotLight()
            {
                linear = 0.09f,
                quadratic = 0.032f,
                cutOff = 0.996f,
                outerCutOff = 0.965f,
                lightProperties = new LightProperties()
                {
                    ambient = new Vector3(0.0f, 0.0f, 0.0f),
                    diffuse = new Vector3(1.0f, 1.0f, 1.0f),
                    specular = new Vector3(1.0f, 1.0f, 1.0f)
                }
            };

            _inputManager.AddKeyAction(Keys.W, _camera.MoveForward);
            _inputManager.AddKeyAction(Keys.A, _camera.MoveLeft);
            _inputManager.AddKeyAction(Keys.S, _camera.MoveBackward);
            _inputManager.AddKeyAction(Keys.D, _camera.MoveRight);

            //_inputManager.AddMouseAction(MouseAxis.MouseX, _camera.MouseX);
            //_inputManager.AddMouseAction(MouseAxis.MouseY, _camera.MouseY);

            _inputManager.AddKeyAction(Keys.Escape, (_) =>
            {
                if (CursorState == CursorState.Grabbed)
                    CursorState = CursorState.Normal;
                else
                    CursorState = CursorState.Grabbed;
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

            _spotLight.position = _camera.Position;
            _spotLight.direction = _camera.Forward;
            _spotLight.Upload(_shader);

            if (_camera != null && _shader != null)
            {
                _shader.SetMatrix4f(6, ref _camera.View);
                _shader.SetMatrix4f(7, ref _camera.Perspective);
                _shader.SetVec3(8, ref _camera.Position);

                _gridShader.SetMatrix4f(6, ref _camera.View);
                _gridShader.SetMatrix4f(7, ref _camera.Perspective);

                _skyBoxShader.SetMatrix4f(6, ref _camera.ViewWithoutTranslation);
                _skyBoxShader.SetMatrix4f(7, ref _camera.Perspective);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _cube2.PrepareForDraw();
            _cube2.Draw();

            _cube3.PrepareForDraw();
            _cube3.Draw();

            GL.DepthFunc(DepthFunction.Lequal);
            GL.FrontFace(FrontFaceDirection.Ccw);
            _skyBoxShader.Use();
            _cubeMap.Bind();
            _skyBoxCube.Draw();
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.DepthFunc(DepthFunction.Less);

            //Write first draw to stencil buffer
            GL.StencilOp(StencilOp.Keep, StencilOp.Replace, StencilOp.Replace);
            //write 1's in the stencil buffer for the fragments where cube1 is drawn
            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilMask(0xFF);
            _shader.Use();
            _cube1.Scale(1.0f);
            _cube1.PrepareForDraw();
            _cube1.Draw();

            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
            GL.StencilMask(0x00);
            GL.Disable(EnableCap.DepthTest);

            _cube1.Scale(1.06f);
            _stencilShader.SetMatrix4f(5, ref _cube1.Transform.Matrix);
            _stencilShader.SetMatrix4f(6, ref _camera.View);
            _stencilShader.SetMatrix4f(7, ref _camera.Perspective);
            _stencilShader.Use();
            _cube1.PrepareForDraw();
            _cube1.DrawWithoutMaterial();

            //POST
            GL.StencilMask(0xFF);
            GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
            GL.Enable(EnableCap.DepthTest);

            _gridThing.PrepareForDraw();
            _gridShader.Use();
            _gridThing.Draw();

            /*_controller.Update(this, (float)args.Time);
            ImGui.ShowDemoWindow();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");*/

            _controller.Update(this, (float)args.Time);
            _imguiEditor.Show();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
        }
    }
}
