using ImGuiNET;

namespace Game.Editor
{
    internal class ImGuiMainWindow
    {
        private readonly Game _game;
        private readonly Time _time;

        private bool _debugInfoWindow = false;
        private bool _rightDockWindow = true;

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
            DrawRightDock();
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
                        if (ImGui.MenuItem("Debug info"))
                        {
                            _debugInfoWindow = true;
                        }

                        if (ImGui.MenuItem("Right dock"))
                        {
                            _rightDockWindow = true;
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

        public void DrawRightDock()
        {
            if (_rightDockWindow && 
                ImGui.Begin("Right dock", ref _rightDockWindow,
                    ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize))
            {
                ImGui.SetWindowPos(
                    new System.Numerics.Vector2(_game.ClientSize.X - 350.0f, 
                        ImGui.GetItemRectSize().Y));
                ImGui.SetWindowSize(
                    new System.Numerics.Vector2(
                        350.0f, 
                    _game.ClientSize.Y - ImGui.GetItemRectSize().Y));
                ImGui.End();
            }
        }
    }
}
