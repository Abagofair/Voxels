namespace Game.Editor.Windows
{
    internal abstract class Window
    {
        public abstract string Name { get; }

        private bool _isOpen = false;
        public ref bool IsOpen => ref _isOpen;

        public abstract void Draw(EditorState editorState);

        public void OpenWindow()
        {
            _isOpen = true;
            Logger.Debug($"Opened window {Name}");
        }
    }
}
