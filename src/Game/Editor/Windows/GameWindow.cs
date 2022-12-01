using ImGuiNET;
using OpenTK.Mathematics;

namespace Game.Editor.Windows
{
    internal class GameWindow : Window
    {
        public override string Name => nameof(GameWindow);

        public override void Draw(EditorState editorState)
        {
            if (IsOpen &&
                ImGui.Begin(Name, ref IsOpen))
            {
                if (ImGui.IsWindowFocused())
                {
                    editorState.ActiveWindow = this;
                }

                ImGui.BeginChild("Render");
                editorState.Game.ActiveScene.Renderer.RenderTarget.ViewportSize
                    = new Vector2i((int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);
                float newAspectRatio 
                    = (float)editorState.Game.ActiveScene.Renderer.RenderTarget.ViewportSize.X / editorState.Game.ActiveScene.Renderer.RenderTarget.ViewportSize.Y;

                editorState.Game.ActiveScene.Camera.AspectRatio = newAspectRatio;

                ImGui.Image(editorState.Game.ActiveScene.Renderer.RenderTarget.ColorTextureId,
                new System.Numerics.Vector2(
                editorState.Game.ActiveScene.Renderer.RenderTarget.ViewportSize.X,
                    editorState.Game.ActiveScene.Renderer.RenderTarget.ViewportSize.Y),
                    new System.Numerics.Vector2(0.0f, 1.0f),
                    new System.Numerics.Vector2(1.0f, 0.0f));

                ImGui.EndChild();
                ImGui.End();
            }
        }
    }
}
