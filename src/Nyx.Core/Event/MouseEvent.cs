using Silk.NET.Input.Common;

namespace Nyx.Core.Event
{
    public class MouseEvent
    {
        private static MouseEvent _instance;
        private static readonly object _mutex = new object();
        private readonly IMouse _mouse;

        private MouseEvent(IMouse mouse)
        {
            _mouse = mouse;
        }

        public static MouseEvent Get(IMouse mouse)
        {
            if (_instance == null)
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new MouseEvent(mouse);
                    }
                }
            }

            return _instance;
        }

        public bool IsButtonPressed(MouseButton mouseButton)
        {
            return _mouse.IsButtonPressed(mouseButton);
        }
    }
}