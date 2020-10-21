using System;
using System.Drawing;
using System.IO;
using Nyx.Core.OpenGL;
using Silk.NET.Input;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Common;

namespace Nyx.Playground
{
    public class Program
    {
        private static IWindow _window;
        private static GL _gl;

        private static BufferObject<float> _vertexBufferObject;
        private static BufferObject<uint> _elementBufferObject;
        private static VertexArrayObject<float, uint> _vertexArrayobject;
        private static Shader _shader;

        //Vertex data, uploaded to the VBO.
        private static readonly float[] Vertices =
        {
            //X    Y      Z
            0.5f, 0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            -0.5f, 0.5f, 0.5f,
        };

        //Index data, uploaded to the EBO.
        private static readonly uint[] Indices =
        {
            0, 1, 3,
            1, 2, 3,
        };

        public static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Size(800, 600);
            options.Title = "Nyx Playground";
            options.VSync = VSyncMode.On;
            options.ShouldSwapAutomatically = true;
            options.WindowBorder = WindowBorder.Fixed;
            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;
            _window.Update += OnUpdate;
            _window.Closing += OnClose;

            _window.Run();
        }

        private static unsafe void OnLoad()
        {
            IInputContext input = _window.CreateInput();
            foreach (IKeyboard t in input.Keyboards)
            {
                t.KeyDown += KeyDown;
            }

            // Getting the opengl api for drawing to the screen.
            _gl = GL.GetApi(_window);

            // Initialize a vertex buffer that holds the vertex data
            _vertexBufferObject = new BufferObject<float>(_gl, Vertices, BufferTargetARB.ArrayBuffer);

            // Initializing a element buffer that holds the index data.
            _elementBufferObject = new BufferObject<uint>(_gl, Indices, BufferTargetARB.ElementArrayBuffer);

            // Creating a vertex array
            _vertexArrayobject = new VertexArrayObject<float, uint>(_gl, _vertexBufferObject, _elementBufferObject);

            // Create shader
            (string vertexShaderPath, string fragmentShaderPath) =
                GetShaderFullPaths("Shaders/shader.vert", "Shaders/shader.frag");
            _shader = new Shader(_gl, vertexShaderPath, fragmentShaderPath);

            // Tell opengl how to give the data to the shaders.
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
            _gl.EnableVertexAttribArray(0);
        }

        private static unsafe void OnRender(double obj)
        {
            //Clear the color channel.
            _gl.Clear((uint) ClearBufferMask.ColorBufferBit);

            //Bind the geometry and shader.
            _vertexArrayobject.Bind();
            _shader.Use();

            //Draw the geometry.
            _gl.DrawElements(PrimitiveType.Triangles, (uint) Indices.Length, DrawElementsType.UnsignedInt, null);
        }


        private static void OnUpdate(double obj)
        {
        }

        private static void OnClose()
        {
            //Remember to delete the buffers.
            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();
        }

        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                _window.Close();
            }
        }

        private static (string, string) GetShaderFullPaths(string vertexPath, string fragmentPath)
        {
            string baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent!.Parent!.FullName;
            string vertexShaderPath = Path.Combine(baseDir, vertexPath);
            string fragmentShaderPath = Path.Combine(baseDir, fragmentPath);

            return (vertexShaderPath, fragmentShaderPath);
        }
    }
}