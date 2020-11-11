using Nyx.Core.Settings;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nyx.Core
{
    public class Window : GameWindow
    {
        
        
        public Window(WindowSettings windowSettings) : base(GameWindowSettings.Default, windowSettings.MapToNativeWindowSettings())
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            base.OnUpdateFrame(e);
        }
    }
}