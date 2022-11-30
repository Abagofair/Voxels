using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game
{
    internal class Scene
    {
        private readonly Game _game;
        private readonly Renderer _renderer;
        private readonly InputManager _inputManager;
        private readonly FreeLookCamera _freeLookCamera;
        private readonly Skybox _skybox;

        public Scene(
            Game game,
            Renderer renderer,
            InputManager inputManager,
            Skybox skybox)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
            _skybox = skybox ?? throw new ArgumentNullException(nameof(skybox));

            _freeLookCamera = new FreeLookCamera(
                MathF.PI / 4.0f, 
                (float)_game.ClientSize.X / _game.ClientSize.Y, 
                0.01f, 
                1000.0f);

            _freeLookCamera.ViewChanged += UpdateCameraView;
            _freeLookCamera.PerspectiveChanged += UpdateCameraPerspective;

            _inputManager.AddKeyAction(Keys.W, _freeLookCamera.MoveForward);
            _inputManager.AddKeyAction(Keys.A, _freeLookCamera.MoveLeft);
            _inputManager.AddKeyAction(Keys.S, _freeLookCamera.MoveBackward);
            _inputManager.AddKeyAction(Keys.D, _freeLookCamera.MoveRight);

            //contains world transform
            var rootGameObject = GameEntityManager.Create<GameEntity>("sceneRoot", new Transform());
            SceneTree = new SceneTree(rootGameObject);
        }

        public FreeLookCamera Camera => _freeLookCamera;
        public Renderer Renderer => _renderer;
        public SceneTree SceneTree { get; }

        private void UpdateCameraView()
        {
            foreach (Shader shader in AssetManager.Shaders.Values.Where(x => x.Type == ShaderType.Skybox))
            {
                shader.SetMatrix4f("View", ref _freeLookCamera.ViewWithoutTranslation);
            }

            foreach (var shader in AssetManager.Shaders.Values.Where(x => x.Type != ShaderType.Skybox))
            {
                shader.SetMatrix4f("View", ref _freeLookCamera.View);
                shader.SetVec3("CameraPosition", ref _freeLookCamera.Position);
            }
        }

        public void UpdateCameraPerspective()
        {
            foreach (Shader shader in AssetManager.Shaders.Values)
            {
                shader.SetMatrix4f("Projection", ref _freeLookCamera.Projection);
            }
        }

        public void Initialize()
        {
            foreach (var item in SceneTree.GetDirectionalLights())
            {
                foreach (var shader in AssetManager.Shaders.Values.Where(x => x.Type == ShaderType.MaterialShader))
                {
                    item.Upload(shader);
                    Logger.Debug($"Uploaded directional light for {item}");
                }
            }

            UpdateCameraView();
            UpdateCameraPerspective();
        }

        public void Update(Time time)
        {
            SceneTree.Update(time);
        }

        public void Render()
        {
            /*
             * To render a scene i have to:
             *  Upload camera view and projection uniforms
             *  Upload light uniforms
             *  foreach renderable
             *      apply transform uniform
             *      apply material
             *      render
             */

            IEnumerable<Renderable> stuffToRender = GameEntityManager.GetRenderables(SceneTree)
                .Append(_skybox);

            _renderer.Render(stuffToRender);
        }
    }
}
