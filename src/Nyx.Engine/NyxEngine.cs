using System;
using System.Drawing;
using System.IO;
using Nyx.Core.Input;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using Silk.NET.Windowing.Common;
using MouseButton = Silk.NET.Input.Common.MouseButton;

namespace Nyx.Engine
{
    public abstract class NyxEngine
    {
        protected static IWindow Window;
        public static GL Gl;
        public static KeyListener KeyListener;
        public static MouseListener MouseListener;

        protected float BeginTime = (float) Glfw.GetApi().GetTime();
        protected float DeltaTime = -1.0f;
        protected float EndTime;


        protected NyxEngine()
        {
            var options = WindowOptions.Default;
            options.Size = new Size(800, 600);
            options.Title = "Nyx Playground";
            options.VSync = VSyncMode.On;
            options.ShouldSwapAutomatically = true;
            options.WindowBorder = WindowBorder.Fixed;
            Window = Silk.NET.Windowing.Window.Create(options);

            Window.Load += OnLoad;
            Window.Render += OnRender;
            Window.Update += OnUpdate;
            Window.Closing += OnClose;
        }

        protected virtual void OnLoad()
        {
            IInputContext input = Window.CreateInput();

            foreach (IKeyboard k in input.Keyboards)
            {
                KeyListener = KeyListener.Get(k);
                k.KeyDown += KeyDown;
                k.KeyUp += KeyUp;
            }

            foreach (IMouse m in input.Mice)
            {
                MouseListener = MouseListener.Get(m);
                m.Click += MouseClick;
                m.MouseMove += MouseMove;
                m.Scroll += MouseScroll;
                m.DoubleClick += MouseDoubleClick;
                m.MouseUp += MouseUp;
                m.MouseDown += MouseDown;
            }

            // Getting the opengl api for drawing to the screen.
            Gl = GL.GetApi(Window);
        }

        /// <summary>
        ///     Update geometry and listen to user events
        /// </summary>
        protected virtual void OnUpdate(double obj)
        {
        }

        /// <summary>
        ///     Draw stuff to the screen
        /// </summary>
        protected virtual void OnRender(double obj)
        {
            //Clear the color channel.
            Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        }

        protected virtual void OnClose()
        {
            //Remember to delete the buffers.
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
            Window.Run();
        }

        #region InputEvents

        protected virtual void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                Window.Close();
            }
        }

        protected virtual void KeyUp(IKeyboard arg1, Key arg2, int arg3)
        {
        }


        protected virtual void MouseMove(IMouse arg1, PointF arg2)
        {
        }

        protected virtual void MouseClick(IMouse arg1, MouseButton arg2)
        {
        }

        protected virtual void MouseDown(IMouse arg1, MouseButton arg2)
        {
        }

        protected virtual void MouseUp(IMouse arg1, MouseButton arg2)
        {
        }

        protected virtual void MouseDoubleClick(IMouse arg1, MouseButton arg2)
        {
        }

        protected virtual void MouseScroll(IMouse arg1, ScrollWheel arg2)
        {
        }

        #endregion
    }
}