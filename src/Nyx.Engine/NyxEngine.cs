using System;
using System.Drawing;
using System.IO;
using Silk.NET.Input;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using Silk.NET.Windowing.Common;

namespace Nyx.Engine
{
    public abstract class NyxEngine
    {
        protected static IWindow Window;
        protected static GL Gl;

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
            foreach (IKeyboard t in input.Keyboards)
            {
                t.KeyDown += KeyDown;
            }

            // Getting the opengl api for drawing to the screen.
            Gl = GL.GetApi(Window);
        }

        protected virtual void OnRender(double obj)
        {
            //Clear the color channel.
            Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        }


        protected virtual void OnUpdate(double obj)
        {
        }

        protected virtual void OnClose()
        {
            //Remember to delete the buffers.
        }

        protected virtual void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                Window.Close();
            }
        }

        protected static (string, string) GetShaderFullPaths(string vertexPath, string fragmentPath)
        {
            string baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent!.Parent!.FullName;
            string vertexShaderPath = Path.Combine(baseDir, vertexPath);
            string fragmentShaderPath = Path.Combine(baseDir, fragmentPath);

            return (vertexShaderPath, fragmentShaderPath);
        }

        public void Run()
        {
            Console.WriteLine("Running window");
            Window.Run();
        }
    }
}