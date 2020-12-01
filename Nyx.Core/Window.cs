using Microsoft.Extensions.Logging;
using Nyx.Core.Gui;
using Nyx.Core.Logger;
using Nyx.Core.Renderer;
using Nyx.Core.Scenes;
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

        private Vector4 _color = new(1, 1, 1, 1);

        private ImGuiController _controller;

        public Window(WindowSettings windowSettings) : base(GameWindowSettings.Default,
            windowSettings.MapToNativeWindowSettings())
        {
            WindowSettings = windowSettings;
        }

        public static WindowSettings WindowSettings { get; private set; }
        public static Framebuffer Framebuffer { get; private set; }

        private static void EnableAlphaBlending()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
        }

        public static void AddScene(string name, Scene scene)
        {
            SceneManager.AddScene(name, scene);
        }

        protected override void OnLoad()
        {
            EnableAlphaBlending();

            _controller = new ImGuiController(ClientSize);
            Framebuffer = new Framebuffer(1920, 1080);
            GL.Viewport(0, 0, 1920, 1080);

            SceneManager.ChangeScene("test");

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

            double deltaTime = e.Time;
            SceneManager.CurrentScene.Update(ref deltaTime);

            _controller.Update(this, ref deltaTime);

            base.OnUpdateFrame(e);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            double deltaTime = e.Time;

            Framebuffer.Bind();
            GL.ClearColor(new Color4(0, 32, 48, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);

            // Current scene renders
            SceneManager.CurrentScene.Render(ref deltaTime);

            Framebuffer.Detach();

            _controller.Render(ref deltaTime);

            GlUtils.CheckError("End of frame");

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
            SceneManager.CurrentScene.Dispose();
            _controller.Dispose();
            Framebuffer.Dispose();

            base.Close();
        }
    }
}