namespace Game.Editor.Windows
{
    internal class WindowContainer //for lack of a better name
    {
        public readonly DebugInfoWindow debugInfoWindow = new DebugInfoWindow();
        public readonly LogWindow logWindow = new LogWindow();
        public readonly GameWindow gameWindow = new GameWindow();
        public readonly SceneTreeWindow sceneTreeWindow = new SceneTreeWindow();
        public readonly EditGameEntityWindow editGameEntityWindow = new EditGameEntityWindow();
    }
}
