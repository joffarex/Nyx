using System;
using Nyx.Core.OpenGL;
using Nyx.Engine;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;

namespace Nyx.Playground
{
    public class Game : NyxEngine
    {
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

        protected override unsafe void OnLoad()
        {
            base.OnLoad();

            // Initialize a vertex buffer that holds the vertex data
            _vertexBufferObject = new BufferObject<float>(Gl, Vertices, BufferTargetARB.ArrayBuffer);

            // Initializing a element buffer that holds the index data.
            _elementBufferObject = new BufferObject<uint>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);

            // Creating a vertex array
            _vertexArrayobject = new VertexArrayObject<float, uint>(Gl, _vertexBufferObject, _elementBufferObject);

            // Create shader
            (string vertexShaderPath, string fragmentShaderPath) =
                GetShaderFullPaths("Shaders/shader.vert", "Shaders/shader.frag");
            _shader = new Shader(Gl, vertexShaderPath, fragmentShaderPath);

            // Tell opengl how to give the data to the shaders.
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
            Gl.EnableVertexAttribArray(0);

            // Initial startup drawings
        }

        protected override unsafe void OnRender(double obj)
        {
            base.OnRender(obj);

            //Bind the geometry and shader.
            _vertexArrayobject.Bind();
            _shader.Use();

            //Draw the geometry.
            Gl.DrawElements(PrimitiveType.Triangles, (uint) Indices.Length, DrawElementsType.UnsignedInt, null);

            // Draw all game objects here
        }

        protected override void OnUpdate(double obj)
        {
            base.OnUpdate(obj);

            // Do all the updating stuff
        }

        protected override void OnClose()
        {
            base.OnClose();

            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();

            // Clear out every object here
        }

        protected override void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            base.KeyDown(arg1, arg2, arg3);

            if (arg2 == Key.Space)
            {
                Console.WriteLine("SPAAAAAAAAAAAAAAAAAAAAACCEEEE");
            }

            // Call key down events from game objects here in order to subscribe
        }
    }
}