using Game.Editor.Windows;
using ImGuiNET;

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

        public int ImGuiMainMenubarHeight => 17;
        public int ImGuiWindowWidth => 350;
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

        public void Initialize()
        {
            var sceneTree = new SceneTreeWindow();
            sceneTree.OpenWindow();
            OpenWindows.Add(sceneTree);

            var gameWindow = new GameWindow();
            gameWindow.OpenWindow();
            OpenWindows.Add(gameWindow);

            var editGame = new EditGameEntityWindow();
            editGame.OpenWindow();
            OpenWindows.Add(editGame);

            var logWindow = new LogWindow();
            logWindow.OpenWindow();
            OpenWindows.Add(logWindow);
        }

        public void DrawWindows()
        {
            foreach (var window in OpenWindows)
            {
                if (window.IsOpen)
                {
                    window.Resize(this);
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
