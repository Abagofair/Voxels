using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static Game.SceneTree;

namespace Game.Editor
{
    internal class ImGuiMainWindow
    {
        private readonly Game _game;
        private readonly Time _time;

        private bool _debugInfoWindow = false;
        private bool _rightDockWindow = true;
        private bool _leftDockWindow = true;
        private bool _logWindow = true;
        private bool _gameWindow = true;
        private bool _sceneTree = true;
        private bool _createEntity = false;

        public ImGuiMainWindow(
            Game game,
            Time time)
        {
            _game = game;
            _time = time ?? throw new ArgumentNullException(nameof(time));
        }

        public void Show()
        {
            DrawTopMenu();
            DrawDebugWindow();
            DrawLeftDock();
            DrawRightDock();
            DrawLogWindow();
            DrawGameWindow();
            DrawSceneTree();
        }

        public void DrawTopMenu()
        {
            if (ImGui.Begin(
                "Editor",
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoDecoration |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoBackground))
            {
                if (ImGui.BeginMainMenuBar())
                {
                    if (ImGui.BeginMenu("File"))
                    {
                        if (ImGui.MenuItem("Exit"))
                        {
                            _game.Close();
                        }

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Windows"))
                    {
                        if (ImGui.MenuItem("Scene tree"))
                        {
                            _sceneTree = !_sceneTree;
                        }

                        if (ImGui.MenuItem("Game window"))
                        {
                            _gameWindow = !_gameWindow;
                        }

                        if (ImGui.MenuItem("Log"))
                        {
                            _logWindow = !_logWindow;
                        }

                        if (ImGui.MenuItem("Debug info"))
                        {
                            _debugInfoWindow = !_debugInfoWindow;
                        }

                        if (ImGui.MenuItem("Left dock"))
                        {
                            _leftDockWindow = !_leftDockWindow;
                        }

                        if (ImGui.MenuItem("Right dock"))
                        {
                            _rightDockWindow = !_rightDockWindow;
                        }

                        ImGui.EndMenu();
                    }

                    ImGui.EndMainMenuBar();
                }

                ImGui.End();
            }
        }

        public void DrawDebugWindow()
        {
            var samples = _time.DeltaTimeSamples.ToArray();

            if (samples.Length == 0)
                return;

            if (_debugInfoWindow && ImGui.Begin("Debug info", ref _debugInfoWindow, ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.LabelText("Runtime", $"{_time.TotalTime} seconds");
                ImGui.LabelText("FrameCount", _time.CurrentFrame.ToString());
                ImGui.LabelText("FPS", _time.FramesPerSecond.ToString());
                ImGui.LabelText("Avg. frameTime", $"{_time.AverageDeltaTimeF} ms");
                ImGui.PlotLines(
                    string.Empty,
                    ref samples[0],
                    samples.Length,
                    0,
                    "FrameTime",
                    0.0f,
                    0.2f,
                    new System.Numerics.Vector2(300.0f, 75.0f));

                ImGui.End();
            }
        }

        public void DrawLeftDock()
        {
            if (_leftDockWindow &&
                ImGui.Begin("Left dock", ref _leftDockWindow,
                    ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize))
            {
                var viewport = ImGui.GetMainViewport();

                ImGui.SetWindowPos(
                    new System.Numerics.Vector2(
                        0.0f,
                        ImGui.GetItemRectSize().Y));

                ImGui.SetWindowSize(
                    new System.Numerics.Vector2(
                        350.0f,
                        viewport.Size.Y - ImGui.GetItemRectSize().Y));

                ImGui.End();
            }
        }

        public void DrawRightDock()
        {
            if (_rightDockWindow && 
                ImGui.Begin("Right dock", ref _rightDockWindow,
                    ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize))
            {
                var viewportSize = ImGui.GetMainViewport().WorkSize;

                ImGui.SetWindowPos(
                    new System.Numerics.Vector2(
                        viewportSize.X - 350.0f,
                        ImGui.GetItemRectSize().Y));

                ImGui.SetWindowSize(
                    new System.Numerics.Vector2(
                        350.0f,
                        viewportSize.Y));

                //ImGui.Image(_game._renderTargetColorTexture, viewportSize);
                
                ImGui.End();
            }
        }

        public void DrawLogWindow()
        {
            if (_logWindow &&
                ImGui.Begin("Log", ref _rightDockWindow))
            {
                var viewportSize = ImGui.GetMainViewport().WorkSize;

                ImGui.SetWindowSize(
                    new System.Numerics.Vector2(300.0f, 200.0f));

                ImGui.SetWindowPos(
                    new System.Numerics.Vector2(
                        350.0f,
                        viewportSize.Y + ImGui.GetItemRectSize().Y - 200.0f));

                foreach (var logMessage in Logger.GetLogMessages())
                {
                    ImGui.Text(logMessage.ToString());
                }

                ImGui.End();
            }
        }

        public void DrawGameWindow()
        {
           // ImGui.SetNextWindowPos(ImGui.GetWindowPos());
           // ImGui.SetNextWindowSize(new System.Numerics.Vector2(800.0f, 600.0f));
            if (_gameWindow && 
                ImGui.Begin("Game"))
            {
                ImGui.BeginChild("Render");
                _game.ActiveScene.Renderer.RenderTarget.ViewportSize
                    = new Vector2i((int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);

                float newAspectRatio = (float)_game.ActiveScene.Renderer.RenderTarget.ViewportSize.X / _game.ActiveScene.Renderer.RenderTarget.ViewportSize.Y;

                _game.ActiveScene.Camera.AspectRatio = newAspectRatio;

                ImGui.Image(_game.ActiveScene.Renderer.RenderTarget.ColorTextureId, 
                    new System.Numerics.Vector2(
                        _game.ActiveScene.Renderer.RenderTarget.ViewportSize.X,
                    _game.ActiveScene.Renderer.RenderTarget.ViewportSize.Y),
                    new System.Numerics.Vector2(0.0f, 1.0f),
                    new System.Numerics.Vector2(1.0f, 0.0f));

                ImGui.EndChild();
                ImGui.End();
            }
        }

        public void DrawSceneTree()
        {
            ImGui.ShowDemoWindow();
            if (_sceneTree &&
                ImGui.Begin("Scene tree", ref _sceneTree))
            {
                var rootNode = _game.ActiveScene.SceneTree.Root;

                if (ImGui.TreeNodeEx(rootNode.GameEntity.Name,
                            ImGuiTreeNodeFlags.OpenOnDoubleClick |
                            ImGuiTreeNodeFlags.OpenOnArrow |
                            ImGuiTreeNodeFlags.SpanFullWidth))
                {
                    SetupCreateEntityMenu(rootNode);

                    if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                    {
                        ImGui.OpenPopup("CreateEntityMenu");
                    }

                    Iterate(rootNode);
                }

                ImGui.End();
            }

            void Iterate(SceneTree.Node parent)
            { 
                foreach (var node in parent.Children)
                {
                    if (ImGui.TreeNodeEx(node.GameEntity.Name,
                        ImGuiTreeNodeFlags.OpenOnDoubleClick |
                        ImGuiTreeNodeFlags.OpenOnArrow |
                        ImGuiTreeNodeFlags.SpanFullWidth))
                    {
                        SetupCreateEntityMenu(node);

                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            ImGui.OpenPopup("CreateEntityMenu");
                        }

                        Iterate(node);
                        ImGui.TreePop();
                    }
                }
            }
        }

        public void SetupCreateEntityMenu(SceneTree.Node node)
        {
            //$"Add entity to parent {parent.GameEntity.Name}"
            if (ImGui.BeginPopup("CreateEntityMenu"))
            {
                if (ImGui.Selectable("Add block"))
                {
                    var shader = AssetManager.CreateOrGetShader("quad", ShaderType.MaterialShader, false);
                    var dif = new BasicMaterial(shader, new MaterialProperties()
                    {
                        ambient = new Vector3(1.0f, 0.5f, 0.31f),
                        diffuse = AssetManager.CreateTextureFromPng("container_diffuse", PixelInternalFormat.Srgb),
                        specular = AssetManager.CreateTextureFromPng("container_specular", PixelInternalFormat.Rgb),
                        shininess = 32.0f
                    });

                    var block0 = GameEntityManager.Create<GameEntity>($"block_{Guid.NewGuid()}", new Transform());
                    GameEntityManager.AddAsDynamicRenderable(block0, new Renderable(1, dif, Model.CreateUnitCube()));
                    var block0Node = new SceneTree.Node(block0);
                    node.Children.Add(block0Node);
                }

                ImGui.EndPopup();
            }
        }
    }
}
