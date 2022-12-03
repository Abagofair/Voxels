using ImGuiNET;

namespace Game.Editor.Windows
{
    internal class EditGameEntityWindow : Window
    {
        private System.Numerics.Vector3 _rotation;

        public override string Name => nameof(EditGameEntityWindow);

        public override void Resize(EditorState editorState)
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(
                editorState.Game.ClientSize.X - editorState.ImGuiWindowWidth, 
                editorState.ImGuiMainMenubarHeight));

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(
                editorState.ImGuiWindowWidth,
                editorState.Game.ClientSize.Y - editorState.ImGuiMainMenubarHeight));
        }

        public unsafe override void Draw(EditorState editorState)
        {
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

                ImGui.InputText("Name", ref selectedNodeEntity.Name, 125, ImGuiInputTextFlags.EnterReturnsTrue);
                ImGui.Separator();

                fixed (float* value = &selectedNodeEntity.Transform.Position.X)
                {
                    ImGui.DragScalarN("Position", ImGuiDataType.Float, (IntPtr)value, 3);
                }

                fixed (float* value = &selectedNodeEntity.Transform.Scale.X)
                {
                    ImGui.DragScalarN("Scale", ImGuiDataType.Float, (IntPtr)value, 3);
                }

                fixed (float* value = &selectedNodeEntity.Transform.Scale.X)
                {
                    ImGui.DragFloat3("Rotation in degrees", ref _rotation);
                    selectedNodeEntity.Transform.Pitch = _rotation.X * (MathF.PI / 180.0f);
                    selectedNodeEntity.Transform.Yaw = _rotation.Y * (MathF.PI / 180.0f);
                    selectedNodeEntity.Transform.Roll = _rotation.Z * (MathF.PI / 180.0f);
                }
                ImGui.Separator();

                ImGui.End();
            }
        }
    }
}
