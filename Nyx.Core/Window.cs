using System.Globalization;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using Nyx.Core.Settings;
using Nyx.Core.Utils;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nyx.Core
{
    public class Window : GameWindow
    {
        private readonly ILogger<Window> _logger = SerilogLogger.Factory.CreateLogger<Window>();
        
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

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Fps.Print(e.Time);
            
            base.OnRenderFrame(e);
        }
    }
}