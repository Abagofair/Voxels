using Game.Editor.Windows;

namespace Game.Editor
{
    internal class EditorState
    {
        public EditorState(
            Game game,
            Time time)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            Time = time ?? throw new ArgumentNullException(nameof(time));
        }

        public event Action<Window?>? OnWindowChanged;
        public event Action<SceneTree.Node?>? OnNodeChanged;

        public WindowContainer Windows { get; } = new WindowContainer();
        public Game Game { get; }
        public Time Time { get; }
        public HashSet<Window> OpenWindows { get; private set; } = new HashSet<Window>();

        private SceneTree.Node? _selectedNode;
        public SceneTree.Node? SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (_selectedNode == value) return;

                _selectedNode = value;
                OnNodeChanged?.Invoke(_selectedNode);
            }
        }

        private Window? _activeWindow;
        public Window? ActiveWindow 
        {
            get => _activeWindow;
            set
            {
                if (_activeWindow == value) return;

                _activeWindow = value;
                OnWindowChanged?.Invoke(_activeWindow);
            }
        }

        public void DrawWindows()
        {
            foreach (var window in OpenWindows)
            {
                if (window.IsOpen)
                {
                    window.Draw(this);
                }
            }
        }

        public void OpenWindow(Window window)
        {
            if (window.IsOpen == true) return;

            OpenWindows.Add(window);
            window.OpenWindow();
        }
    }
}
