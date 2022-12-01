using ImGuiNET;

namespace Game.Editor.Windows
{
    internal class DebugInfoWindow : Window
    {
        public override string Name => nameof(DebugInfoWindow);

        public override void Draw(EditorState editorState)
        {
            var samples = editorState.Time.DeltaTimeSamples.ToArray();

            if (IsOpen &&
                ImGui.Begin(Name, ref IsOpen, ImGuiWindowFlags.AlwaysAutoResize))
            {
                if (ImGui.IsWindowFocused())
                {
                    editorState.ActiveWindow = this;
                }

                ImGui.LabelText("Runtime", $"{editorState.Time.TotalTime} seconds");
                ImGui.LabelText("FrameCount", editorState.Time.CurrentFrame.ToString());
                ImGui.LabelText("FPS", editorState.Time.FramesPerSecond.ToString());
                ImGui.LabelText("Avg. frameTime", $"{editorState.Time.AverageDeltaTimeF} ms");
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
    }
}
