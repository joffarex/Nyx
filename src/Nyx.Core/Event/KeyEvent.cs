using Silk.NET.Input.Common;

namespace Nyx.Core.Event
{
    public class KeyEvent
    {
        private static KeyEvent _instance;
        private static readonly object _mutex = new object();
        private readonly IKeyboard _keyboard;

        private KeyEvent(IKeyboard keyboard)
        {
            _keyboard = keyboard;
        }

        public static KeyEvent Get(IKeyboard keyboard)
        {
            if (_instance is null)
            {
                lock (_mutex)
                {
                    if (_instance is null)
                    {
                        _instance = new(keyboard);
                    }
                }
            }

            return _instance;
        }

        public bool IsKeyPressed(Key key)
        {
            return _keyboard.IsKeyPressed(key);
        }
    }
}