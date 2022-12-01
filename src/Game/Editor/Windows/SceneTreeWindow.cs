using ImGuiNET;

namespace Game.Editor.Windows
{
    internal class SceneTreeWindow : Window
    {
        public override string Name => nameof(SceneTreeWindow);

        public override void Draw(EditorState editorState)
        {
            if (IsOpen &&
                ImGui.Begin(Name, ref IsOpen))
            {
                if (ImGui.IsWindowFocused())
                {
                    editorState.ActiveWindow = this;
                }

                var rootNode = editorState.Game.ActiveScene.SceneTree.Root;

                if (ImGui.TreeNodeEx(rootNode.GameEntity.Name,
                            ImGuiTreeNodeFlags.OpenOnArrow |
                            ImGuiTreeNodeFlags.SpanFullWidth))
                {
                    //SetupCreateEntityMenu(rootNode);

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
                        ImGuiTreeNodeFlags.OpenOnArrow |
                        ImGuiTreeNodeFlags.SpanFullWidth))
                    {
                        //SetupCreateEntityMenu(node);

                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            ImGui.OpenPopup("CreateEntityMenu");
                        }

                        if (ImGui.IsItemActive())
                        {
                            editorState.SelectedNode = node;
                        }

                        Iterate(node);
                        ImGui.TreePop();
                    }
                }
            }
        }
    }
}
