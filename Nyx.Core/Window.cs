using ImGuiNET;
using Microsoft.Extensions.Logging;
using Nyx.Core.Common;
using Nyx.Core.Gui;
using Nyx.Core.Logger;
using Nyx.Core.Settings;
using Nyx.Core.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector4 = System.Numerics.Vector4;

namespace Nyx.Core
{
    public class Window : GameWindow
    {
        private readonly ILogger<Window> _logger = SerilogLogger.Factory.CreateLogger<Window>();
        private readonly WindowSettings _windowSettings;

        private Vector4 _color = new(1, 1, 1, 1);

        private ImGuiController _controller;
        private Framebuffer _framebuffer;

        public Window(WindowSettings windowSettings) : base(GameWindowSettings.Default,
            windowSettings.MapToNativeWindowSettings())
        {
            _windowSettings = windowSettings;
        }

        private static void EnableAlphaBlending()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
        }

        protected override void OnLoad()
        {
            EnableAlphaBlending();

            _controller = new ImGuiController(ClientSize);
            _framebuffer = new Framebuffer(1920, 1080);

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
            GL.ClearColor(new Color4(0, 32, 48, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);

            _framebuffer.Bind();
            // Current scene renders
            ImGui.Begin("Picker");
            if (ImGui.ColorPicker4("Color Picker: ", ref _color))
            {
            }

            ImGui.End();

            ImGui.ShowDemoWindow();

            Fps.ImGuiWindow(e.Time);
            _framebuffer.Detach();

            _controller.Render();

            Gui.Utils.CheckGlError("End of frame");

            // We need to enable blending again because of internal imgui implementation
            EnableAlphaBlending();

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
            _framebuffer.Dispose();

            base.Close();
        }
    }
}