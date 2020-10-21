using Silk.NET.Input.Common;

namespace Nyx.Core.Input
{
    public class KeyListener
    {
        private static KeyListener _instance;
        private static readonly object _mutex = new object();
        private readonly IKeyboard _keyboard;

        private KeyListener(IKeyboard keyboard)
        {
            _keyboard = keyboard;
        }

        public static KeyListener Get(IKeyboard keyboard)
        {
            if (_instance == null)
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new KeyListener(keyboard);
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