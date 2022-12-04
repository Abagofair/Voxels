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

        public void Initialize()
        {
            _editorState.Initialize();
            _editorState.OnWindowChanged += _editorState_OnWindowChanged;
        }

        private void _editorState_OnWindowChanged(Window? obj)
        {
            if (obj is GameWindow)
                _editorState.Game.ActiveScene.IgnoreInput = false;
            else
                _editorState.Game.ActiveScene.IgnoreInput = true;
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
    }
}
