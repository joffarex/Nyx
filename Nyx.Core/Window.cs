using System.Drawing;
using System.Globalization;
using ImGuiNET;
using Microsoft.Extensions.Logging;
using Nyx.Core.Gui;
using Nyx.Core.Logger;
using Nyx.Core.Settings;
using Nyx.Core.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nyx.Core
{
    public class Window : GameWindow
    {
        private readonly ILogger<Window> _logger = SerilogLogger.Factory.CreateLogger<Window>();

        private ImGuiController _controller;
        private readonly WindowSettings _windowSettings;

        public Window(WindowSettings windowSettings) : base(GameWindowSettings.Default,
            windowSettings.MapToNativeWindowSettings())
        {
            _windowSettings = windowSettings;
        }

        protected override void OnLoad()
        {
            _controller = new ImGuiController(ClientSize);

            base.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

            _controller.WindowResized(ClientSize);

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            _controller.Update(this, (float) e.Time);

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Fps.Print(e.Time);

            GL.ClearColor(new Color4(0, 32, 48, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);

            ImGui.ShowDemoWindow();

            _controller.Render();

            Gui.Utils.CheckGlError("End of frame");

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            _controller.PressChar((char) e.Key);

            base.OnKeyDown(e);
        }

        public override void Close()
        {
            _controller.Dispose();

            base.Close();
        }
    }
}