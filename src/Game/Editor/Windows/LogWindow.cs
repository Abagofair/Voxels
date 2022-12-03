using ImGuiNET;

namespace Game.Editor.Windows
{
    internal class LogWindow : Window
    {
        public override string Name => nameof(LogWindow);

        public override void Resize(EditorState editorState)
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(
                editorState.ImGuiWindowWidth, 
                editorState.Game.ClientSize.Y - 200.0f));

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(
                editorState.Game.ClientSize.X - (editorState.ImGuiWindowWidth * 2), 
                editorState.Game.ClientSize.Y - (editorState.Game.ClientSize.Y - 200.0f)));
        }

        public override void Draw(EditorState editorState)
        {
            if (IsOpen &&
                ImGui.Begin(Name, ref IsOpen))
            {
                if (ImGui.IsWindowFocused())
                {
                    editorState.ActiveWindow = this;
                }

                foreach (var logMessage in Logger.GetLogMessages())
                {
                    ImGui.Text(logMessage.ToString());
                }

                ImGui.End();
            }
        }
    }
}
