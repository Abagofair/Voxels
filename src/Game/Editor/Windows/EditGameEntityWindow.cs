using ImGuiNET;

namespace Game.Editor.Windows
{
    internal class EditGameEntityWindow : Window
    {
        private System.Numerics.Vector3 _position;

        public override string Name => nameof(EditGameEntityWindow);

        public unsafe override void Draw(EditorState editorState)
        {
            ImGui.ShowDemoWindow();
            if (IsOpen &&
                ImGui.Begin(Name, ref IsOpen))
            {
                if (ImGui.IsWindowFocused())
                {
                    editorState.ActiveWindow = this;
                }

                var selectedNodeEntity = editorState.SelectedNode?.GameEntity;

                if (selectedNodeEntity == null)
                {
                    ImGui.Text("Select a node first");
                    ImGui.Separator();
                    ImGui.End();
                    return;
                }
                else
                {
                    ImGui.Text($"You are editing {selectedNodeEntity.Name}");
                    ImGui.Separator();
                }
                
                fixed (float* value = &selectedNodeEntity.Transform.Position.X)
                {
                    ImGui.InputScalarN("Position", ImGuiDataType.Float, (IntPtr)value, 3);
                }

                ImGui.End();
            }
        }
    }
}
