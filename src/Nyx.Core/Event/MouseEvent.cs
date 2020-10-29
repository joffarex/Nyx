using System.Numerics;
using Nyx.Core.Math;
using Nyx.Core.Scene;
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

        public float GetOrthoX()
        {
            // We need to normalize current values with * 2 - 1;
            // this way we get coordinates between -1; 1, which we need to openGL;
            float currentX = _mouse.Position.X;
            currentX = ((currentX / NyxApp.GetWindowWidth()) * 2) - 1;

            // Convert to world coordinates
            var tmp = new Vector4(currentX, 0, 0, 1);

            currentX = tmp.Multiply(SceneContext.CurrentScene.Camera2D.InverseProjection)
                .Multiply(SceneContext.CurrentScene.Camera2D.InverseView).X;

            return currentX;
        }

        public float GetOrthoY()
        {
            // We need to normalize current values with * 2 - 1;
            // this way we get coordinates between -1; 1, which we need to openGL;
            float currentY = _mouse.Position.Y;
            currentY = ((currentY / NyxApp.GetWindowHeight()) * 2) - 1;

            // Convert to world coordinates
            var tmp = new Vector4(0, currentY, 0, 1);

            currentY = tmp.Multiply(SceneContext.CurrentScene.Camera2D.InverseProjection)
                .Multiply(SceneContext.CurrentScene.Camera2D.InverseView).Y;

            return currentY;
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