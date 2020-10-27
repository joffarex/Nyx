﻿using System;
using System.Drawing;
using ImGuiNET;
using Nyx.Core.Event;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;
using Silk.NET.Input;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Common;
using Ultz.SilkExtensions.ImGui;

namespace Nyx.Core
{
    public class NyxApp
    {
        private static IWindow _window;
        private static IInputContext _inputContext;

        // public static Scene.Scene CurrentScene;
        private static NyxApp _instance;

        // ImGui
        private static ImGuiController _imGuiController;

        private NyxApp(int width, int height, string title)
        {
            var options = WindowOptions.Default;
            options.Size = new Size(width, height);
            options.Title = title;
            options.VSync = VSyncMode.On;
            options.ShouldSwapAutomatically = true;
            options.WindowBorder = WindowBorder.Fixed;
            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;
            _window.Update += OnUpdate;
            _window.Closing += OnClose;
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

            _imGuiController = new ImGuiController(GraphicsContext.Gl, _window, _inputContext);

            SceneContext.ChangeScene(0);
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
            SceneContext.CurrentScene.Update((float) deltaTime);

            _imGuiController.Update((float) deltaTime);
        }

        /// <summary>
        ///     Draw stuff to the screen
        /// </summary>
        private void OnRender(double deltaTime)
        {
            //Clear the color channel.
            GraphicsContext.Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
            GraphicsContext.Gl.ClearColor(1, 1, 1, 1);

            SceneContext.CurrentScene.Render();

            ImGui.ShowDemoWindow();

            _imGuiController.Render();

            // We need to enable blending again because of internal imgui implementation
            EnableAlphaBlending();
        }

        private void OnClose()
        {
            SceneContext.CurrentScene.Dispose();
            _imGuiController?.Dispose();
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