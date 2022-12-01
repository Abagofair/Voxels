using Game.Editor.Windows;
using ImGuiNET;

namespace Game.Editor
{
    internal class ImGuiMainWindow
    {
        private EditorState _editorState;

        public ImGuiMainWindow(
            Game game,
            Time time)
        {
            _editorState = new EditorState(game, time);
        }

        public void Show()
        {
            DrawMainMenuBar();
            _editorState.DrawWindows();
        }

        public void DrawMainMenuBar()
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
                            _editorState.Game.Close();
                        }

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Windows"))
                    {
                        if (ImGui.MenuItem(nameof(SceneTreeWindow)))
                        {
                            _editorState.OpenWindow(_editorState.Windows.sceneTreeWindow);
                        }

                        if (ImGui.MenuItem(nameof(GameWindow)))
                        {
                            _editorState.OpenWindow(_editorState.Windows.gameWindow);
                        }

                        if (ImGui.MenuItem(nameof(LogWindow)))
                        {
                            _editorState.OpenWindow(_editorState.Windows.logWindow);
                        }

                        if (ImGui.MenuItem(nameof(DebugInfoWindow)))
                        {
                            _editorState.OpenWindow(_editorState.Windows.debugInfoWindow);
                        }

                        if (ImGui.MenuItem(nameof(EditGameEntityWindow)))
                        {
                            _editorState.OpenWindow(_editorState.Windows.editGameEntityWindow);
                        }

                        ImGui.EndMenu();
                    }

                    ImGui.EndMainMenuBar();
                }

                ImGui.End();
            }
        }

        /*

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

        public void EditGameEntityMenu(GameEntity gameEntity)
        {
            if (EditorState.EditEntityWindow &&
                ImGui.Begin(nameof(EditorState.EditEntityWindow), ref EditorState.EditEntityWindow))
            {
                var position = new System.Numerics.Vector3(
                    gameEntity.Transform.Position.X,
                    gameEntity.Transform.Position.Y,
                    gameEntity.Transform.Position.Z);

                ImGui.InputFloat3("Position", ref position);

                gameEntity.Transform.Position = new Vector3(position.X, position.Y, position.Z);

                ImGui.End();
            }
        }*/
    }
}
