using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game
{
    internal class Scene
    {
        private readonly Game _game;
        private readonly Renderer _renderer;
        private readonly InputManager _inputManager;
        private readonly EditorCamera _editorCamera;
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

            _editorCamera = new EditorCamera(
                MathF.PI / 4.0f, 
                (float)_game.ClientSize.X / _game.ClientSize.Y, 
                0.01f, 
                1000.0f);

            _editorCamera.ViewChanged += UpdateCameraView;
            _editorCamera.PerspectiveChanged += UpdateCameraPerspective;

            //contains world transform
            var rootGameObject = GameEntityManager.Create<GameEntity>("sceneRoot", new Transform());
            SceneTree = new SceneTree(rootGameObject);
        }

        public EditorCamera Camera => _editorCamera;
        public Renderer Renderer => _renderer;
        public SceneTree SceneTree { get; }
        //TODO: Look into input contexts and changing that instead of just an ignore flag
        public bool IgnoreInput { get; set; }

        public void Initialize()
        {
            _inputManager.AddKeyAction(Keys.W, _editorCamera.MoveForward);
            _inputManager.AddKeyAction(Keys.A, _editorCamera.MoveLeft);
            _inputManager.AddKeyAction(Keys.S, _editorCamera.MoveBackward);
            _inputManager.AddKeyAction(Keys.D, _editorCamera.MoveRight);
            _inputManager.AddMouseButtonAction(MouseButton.Right, _editorCamera.Rotate);
            _inputManager.AddMouseButtonAction(MouseButton.Middle, _editorCamera.VerticalHorizontal);
            _inputManager.AddMouseAxisAction(MouseAxis.ScrollY, _editorCamera.Dolly);

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

        public void UpdateInput(KeyboardState keyboardState, MouseState mouseState)
        {
            if (IgnoreInput) return;

            _inputManager.HandleKeyboardActions(keyboardState);
            _inputManager.HandleMouseActions(mouseState);
        }

        private void UpdateCameraView()
        {
            foreach (Shader shader in AssetManager.Shaders.Values.Where(x => x.Type == ShaderType.Skybox))
            {
                shader.SetMatrix4f("View", ref _editorCamera.ViewWithoutTranslation);
            }

            foreach (var shader in AssetManager.Shaders.Values.Where(x => x.Type != ShaderType.Skybox))
            {
                shader.SetMatrix4f("View", ref _editorCamera.View);
                shader.SetVec3("CameraPosition", ref _editorCamera.Position);
            }
        }

        public void UpdateCameraPerspective()
        {
            foreach (Shader shader in AssetManager.Shaders.Values)
            {
                shader.SetMatrix4f("Projection", ref _editorCamera.Projection);
            }
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
