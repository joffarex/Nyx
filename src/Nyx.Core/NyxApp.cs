using System;
using System.Drawing;
using System.IO;
using Nyx.Core.Event;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;
using Nyx.Core.Utils;
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

        // public static Scene.Scene CurrentScene;
        private static NyxApp _instance;

        // private readonly Dictionary<int, Scene.Scene> _scenes = new Dictionary<int, Scene.Scene>();

        protected float BeginTime = Time.GetTimeFromAppicationStart();
        protected float DeltaTime = -1.0f;
        protected float EndTime;

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

        protected virtual void OnLoad()
        {
            IInputContext input = _window.CreateInput();

            foreach (IKeyboard k in input.Keyboards)
            {
                EventContext.KeyEvent = KeyEvent.Get(k);
                k.KeyDown += KeyDown;
                k.KeyUp += KeyUp;
            }

            foreach (IMouse m in input.Mice)
            {
                EventContext.MouseEvent = MouseEvent.Get(m);
                m.Click += MouseClick;
                m.MouseMove += MouseMove;
                m.Scroll += MouseScroll;
                m.DoubleClick += MouseDoubleClick;
                m.MouseUp += MouseUp;
                m.MouseDown += MouseDown;
            }

            GraphicsContext.Create(_window);

            SceneContext.ChangeScene(0);
        }

        /// <summary>
        ///     Update geometry and listen to user events
        /// </summary>
        protected virtual void OnUpdate(double obj)
        {
            if (DeltaTime >= 0)
            {
                SceneContext.CurrentScene.Update(DeltaTime);
            }

            EndTime = Time.GetTimeFromAppicationStart();
            DeltaTime = EndTime - BeginTime;
            BeginTime = EndTime;
        }

        /// <summary>
        ///     Draw stuff to the screen
        /// </summary>
        protected virtual void OnRender(double obj)
        {
            //Clear the color channel.
            GraphicsContext.Gl.Clear((uint) ClearBufferMask.ColorBufferBit);

            SceneContext.CurrentScene.Render();
        }

        protected virtual void OnClose()
        {
            SceneContext.CurrentScene.Dispose();
        }

        protected static (string, string) GetShaderFullPaths(string vertexPath, string fragmentPath)
        {
            string vertexShaderPath = GetFullPath(vertexPath);
            string fragmentShaderPath = GetFullPath(fragmentPath);

            return (vertexShaderPath, fragmentShaderPath);
        }

        public static string GetFullPath(string path)
        {
            string baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent!.Parent!.FullName;
            return Path.Combine(baseDir, path);
        }

        public void Run()
        {
            Console.WriteLine("Running window");
            _window.Run();
        }

        #region InputEvents

        protected virtual void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            if (key == Key.Escape)
            {
                _window.Close();
            }
        }

        protected virtual void KeyUp(IKeyboard keyboard, Key key, int keyCode)
        {
        }


        protected virtual void MouseMove(IMouse mouse, PointF position)
        {
            SceneContext.CurrentScene.MouseMove(mouse, position);
        }

        protected virtual void MouseClick(IMouse mouse, MouseButton mouseButton)
        {
        }

        protected virtual void MouseDown(IMouse mouse, MouseButton mouseButton)
        {
        }

        protected virtual void MouseUp(IMouse mouse, MouseButton mouseButton)
        {
        }

        protected virtual void MouseDoubleClick(IMouse mouse, MouseButton mouseButton)
        {
        }

        protected virtual void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
        }

        #endregion
    }
}