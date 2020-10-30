using System;
using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Nyx.Core.Event;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;
using Nyx.Gui;
using Silk.NET.Input;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Common;

namespace Nyx.Core
{
    public class NyxApp
    {
        private static IWindow _window;
        private static IInputContext _inputContext;

        private static NyxApp _instance;

        // ImGui
        private static ImGuiWrapper _imGuiWrapper;

        private NyxApp(int width, int height, string title)
        {
            var options = WindowOptions.Default;
            options.Size = new Size(width, height);
            options.Title = title;
            options.VSync = VSyncMode.On;
            options.ShouldSwapAutomatically = true;
            options.WindowBorder = WindowBorder.Resizable;
            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Resize += OnResize;
            _window.Render += OnRender;
            _window.Update += OnUpdate;
            _window.Closing += OnClose;
        }

        public static float GetWindowWidth()
        {
            return _window.Size.Width;
        }

        public static float GetWindowHeight()
        {
            return _window.Size.Height;
        }


        public static NyxApp Get(int width, int height, string title)
        {
            if (_instance == null)
            {
                _instance = new NyxApp(width, height, title);
            }

            return _instance;
        }

        public void AddScene(int index, Scene.Scene scene)
        {
            SceneContext.AddScene(index, scene);
        }

        public void SetBaseSize(Vector2 baseSize)
        {
            SceneContext.BaseSize = baseSize;
        }

        private void OnLoad()
        {
            _inputContext = _window.CreateInput();

            foreach (IKeyboard k in _inputContext.Keyboards)
            {
                EventContext.KeyEvent = KeyEvent.Get(k);
                k.KeyDown += KeyDown;
                k.KeyUp += KeyUp;
            }

            foreach (IMouse m in _inputContext.Mice)
            {
                EventContext.MouseEvent = MouseEvent.Get(m);
                m.Click += MouseClick;
                m.MouseMove += MouseMove;
                m.Scroll += MouseScroll;
                m.DoubleClick += MouseDoubleClick;
                m.MouseUp += MouseUp;
                m.MouseDown += MouseDown;
            }

            GraphicsContext.CreateOpenGl(_window);
            EnableAlphaBlending();

            GraphicsContext.Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
            GraphicsContext.Gl.ClearColor(1, 1, 1, 1);

            _imGuiWrapper = new ImGuiWrapper(GraphicsContext.Gl, _window, _inputContext);

            SceneContext.ChangeScene(0);
        }

        private void OnResize(Size size)
        {
            GraphicsContext.Gl.Viewport(size);
        }

        private static void EnableAlphaBlending()
        {
            GraphicsContext.Gl.Enable(EnableCap.Blend);
            GraphicsContext.Gl.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
        }

        /// <summary>
        ///     Update geometry and listen to user events
        /// </summary>
        private void OnUpdate(double deltaTime)
        {
            DebugDraw.BeginFrame();
            SceneContext.CurrentScene.Update((float) deltaTime);

            _imGuiWrapper.Update((float) deltaTime);
        }

        /// <summary>
        ///     Draw stuff to the screen
        /// </summary>
        private void OnRender(double deltaTime)
        {
            //Clear the color channel.
            GraphicsContext.Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
            GraphicsContext.Gl.ClearColor(1, 1, 1, 1);

            DebugDraw.Draw();
            SceneContext.CurrentScene.Render();

            SceneContext.CurrentScene.SceneImGui();
            ImGui.ShowDemoWindow();

            _imGuiWrapper.Render();

            // We need to enable blending again because of internal imgui implementation
            EnableAlphaBlending();
        }

        private void OnClose()
        {
            DebugDraw.Dispose();
            SceneContext.CurrentScene.SaveExit();
            SceneContext.CurrentScene.Dispose();
            _imGuiWrapper?.Dispose();
        }

        public void Run()
        {
            Console.WriteLine("Running window");
            _window.Run();
        }

        #region InputEvents

        private void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            if (key == Key.Escape)
            {
                _window.Close();
            }
        }

        private void KeyUp(IKeyboard keyboard, Key key, int keyCode)
        {
        }


        private void MouseMove(IMouse mouse, PointF position)
        {
            SceneContext.CurrentScene.MouseMove(mouse, position);
        }

        private void MouseClick(IMouse mouse, MouseButton mouseButton)
        {
        }

        private void MouseDown(IMouse mouse, MouseButton mouseButton)
        {
        }

        private void MouseUp(IMouse mouse, MouseButton mouseButton)
        {
        }

        private void MouseDoubleClick(IMouse mouse, MouseButton mouseButton)
        {
        }

        private void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
        }

        #endregion
    }
}