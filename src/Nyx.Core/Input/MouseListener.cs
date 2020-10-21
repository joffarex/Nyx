using Silk.NET.Input.Common;

namespace Nyx.Core.Input
{
    public class MouseListener
    {
        private static MouseListener _instance;
        private static readonly object _mutex = new object();
        private readonly IMouse _mouse;

        private MouseListener(IMouse mouse)
        {
            _mouse = mouse;
        }

        public static MouseListener Get(IMouse mouse)
        {
            if (_instance == null)
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new MouseListener(mouse);
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