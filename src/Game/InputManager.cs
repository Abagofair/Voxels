using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game
{
    internal class InputManager
    {
        private bool _mouseMovedThisFrame = false;

        private Vector2 _mouseDelta;
        private Vector2 _mousePosition;
        private Vector2 _previousMousePosition;

        private Dictionary<Keys, Action<Time>> _actionsByKey = new();
        private Dictionary<MouseAxis, Action<float, Time>> _actionsByMousAxis = new();
        private Time _time;

        public InputManager(Time time)
        {
            _time = time;
        }

        public ref Vector2 MouseDelta => ref _mouseDelta;
        public ref Vector2 MousePosition => ref _mousePosition;

        public void AddKeyAction(Keys key, Action<Time> actionHandler)
        {
            if (actionHandler == null) throw new ArgumentNullException(nameof(actionHandler));

            if (_actionsByKey.TryAdd(key, actionHandler))
                _actionsByKey[key] = actionHandler;
        }

        public void AddMouseAction(MouseAxis mouseAxis, Action<float, Time> actionHandler)
        {
            if (actionHandler == null) throw new ArgumentNullException(nameof(actionHandler));

            if (_actionsByMousAxis.TryAdd(mouseAxis, actionHandler))
                _actionsByMousAxis[mouseAxis] = actionHandler;
        }

        public void UpdateMouse(Vector2 mousePosition, Vector2 mouseDelta)
        {
            _mousePosition = mousePosition;
            _mouseDelta = mouseDelta;

            if (_previousMousePosition != _mousePosition)
            {
                _mouseMovedThisFrame = true;
                _previousMousePosition = _mousePosition;
            }
            else
            {
                _mouseMovedThisFrame = false;
            }
        }

        public void HandleActions(KeyboardState keyboardState)
        {
            if (keyboardState == null) throw new ArgumentNullException(nameof(keyboardState));

            if (keyboardState.IsAnyKeyDown)
            {
                foreach (KeyValuePair<Keys, Action<Time>> actionKey in _actionsByKey)
                {
                    if (keyboardState.IsKeyDown(actionKey.Key))
                    {
                        actionKey.Value(_time);
                    }
                }
            }

            if (_mouseMovedThisFrame)
            {
                if (_mouseDelta.X != 0 &&
                    _actionsByMousAxis.TryGetValue(MouseAxis.MouseX, out var action))
                {
                    action(_mouseDelta.X, _time);
                }

                if (_mouseDelta.Y != 0 &&
                    _actionsByMousAxis.TryGetValue(MouseAxis.MouseY, out action))
                {
                    action(_mouseDelta.Y, _time);
                }
            }
        }
    }
}
