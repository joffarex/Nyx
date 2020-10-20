using System;
using System.Drawing;
using System.IO;
using Nyx.Core;
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

        private static uint _vertexBufferObject;
        private static uint _elementBufferObject;
        private static uint _vertexArrayobject;
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

            // Creating a vertex array
            _vertexArrayobject = _gl.GenVertexArray();
            _gl.BindVertexArray(_vertexArrayobject);

            // Initialize a vertex buffer that holds the vertex data
            _vertexBufferObject = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);

            fixed (void* v = &Vertices[0])
            {
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint) (Vertices.Length * sizeof(uint)), v,
                    BufferUsageARB.StaticDraw);
            }

            // Initializing a element buffer that holds the index data.
            _elementBufferObject = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);

            fixed (void* i = &Indices[0])
            {
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (uint) (Indices.Length * sizeof(uint)), i,
                    BufferUsageARB.StaticDraw);
            }

            // Create shader
            string baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent!.Parent!.FullName;
            string vertexShaderPath = Path.Combine(baseDir, "Shaders/shader.vert");
            string fragmentShaderPath = Path.Combine(baseDir, "Shaders/shader.frag");
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
            _gl.BindVertexArray(_vertexArrayobject);
            _gl.UseProgram(_vertexArrayobject);
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
            _gl.DeleteBuffer(_vertexBufferObject);
            _gl.DeleteBuffer(_elementBufferObject);
            _gl.DeleteVertexArray(_vertexArrayobject);
            _shader.Dispose();
        }

        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                _window.Close();
            }
        }
    }
}