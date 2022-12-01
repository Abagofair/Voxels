using ImGuiNET;

namespace Game.Editor.Windows
{
    internal class LogWindow : Window
    {
        public override string Name => nameof(LogWindow);

        public override void Draw(EditorState editorState)
        {
            if (IsOpen &&
                ImGui.Begin(Name, ref IsOpen))
            {
                if (ImGui.IsWindowFocused())
                {
                    editorState.ActiveWindow = this;
                }

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
    }
}
