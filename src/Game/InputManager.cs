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
        private Dictionary<MouseAxis, Action<float, Time>> _actionsByMouseAxis = new();
        private Dictionary<MouseButton, Action<MouseState, Time>> _actionByMouseButton = new();
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

        public void AddMouseAxisAction(MouseAxis mouseAxis, Action<float, Time> actionHandler)
        {
            if (actionHandler == null) throw new ArgumentNullException(nameof(actionHandler));

            if (_actionsByMouseAxis.TryAdd(mouseAxis, actionHandler))
                _actionsByMouseAxis[mouseAxis] = actionHandler;
        }

        public void AddMouseButtonAction(MouseButton mouseButton, Action<MouseState, Time> actionHandler)
        {
            if (actionHandler == null) throw new ArgumentNullException(nameof(actionHandler));

            if (_actionByMouseButton.TryAdd(mouseButton, actionHandler))
                _actionByMouseButton[mouseButton] = actionHandler;
        }

        public void HandleMouseActions(MouseState mouseState)
        {
            _mousePosition = mouseState.Position;
            _mouseDelta = mouseState.Delta;

            if (mouseState.IsAnyButtonDown)
            {
                foreach (KeyValuePair<MouseButton, Action<MouseState, Time>> actionMouseButton in _actionByMouseButton)
                {
                    if (mouseState.IsButtonDown(actionMouseButton.Key))
                    {
                        actionMouseButton.Value(mouseState, _time);
                    }
                }
            }

            if (_previousMousePosition != _mousePosition)
            {
                _previousMousePosition = _mousePosition;

                if (_mouseDelta.X != 0 &&
                    _actionsByMouseAxis.TryGetValue(MouseAxis.MouseX, out var action))
                {
                    action(_mouseDelta.X, _time);
                }

                if (_mouseDelta.Y != 0 &&
                    _actionsByMouseAxis.TryGetValue(MouseAxis.MouseY, out action))
                {
                    action(_mouseDelta.Y, _time);
                }
            }

            if (mouseState.ScrollDelta.LengthFast > 0)
            {
                if (mouseState.ScrollDelta.Y != 0 &&
                    _actionsByMouseAxis.TryGetValue(MouseAxis.ScrollY, out var action))
                {
                    action(mouseState.ScrollDelta.Y, _time);
                }
            }
        }

        public void HandleKeyboardActions(KeyboardState keyboardState)
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
        }
    }
}
