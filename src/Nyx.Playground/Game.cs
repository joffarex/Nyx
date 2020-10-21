using System;
using System.Drawing;
using Nyx.Core.OpenGL;
using Nyx.Engine;
using Silk.NET.GLFW;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using MouseButton = Silk.NET.Input.Common.MouseButton;

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

        private static Scene _currentScene;
        private static Game _instance;
        public float b = 1;
        public float g = 1;
        public float r = 1;

        private Game()
        {
        }

        public static Game Get()
        {
            if (_instance == null)
            {
                _instance = new Game();
            }

            return _instance;
        }


        public static void ChangeScene(int newSceneIndex)
        {
            _currentScene = newSceneIndex switch
            {
                0 => new LevelEditorScene(),
                1 => new LevelScene(),
                _ => throw new Exception($"Scene idx: {newSceneIndex} does not exist"),
            };
        }

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
            ChangeScene(0);
        }

        protected override unsafe void OnRender(double obj)
        {
            base.OnRender(obj);

            //Bind the geometry and shader.
            _vertexArrayobject.Bind();
            _shader.Use();

            //Draw the geometry.
            Gl.DrawElements(PrimitiveType.Triangles, (uint) Indices.Length, DrawElementsType.UnsignedInt, null);

            Gl.ClearColor(r, g, b, 1);

            // Draw all game objects here
        }


        protected override void OnUpdate(double obj)
        {
            base.OnUpdate(obj);

            Console.WriteLine($"FPS: {GetFps(DeltaTime)}");

            if (DeltaTime >= 0)
            {
                _currentScene.Update(DeltaTime);
            }

            EndTime = (float) Glfw.GetApi().GetTime();
            DeltaTime = EndTime - BeginTime;
            BeginTime = EndTime;
        }

        public static float GetFps(float deltaTime)
        {
            return 1.0f / deltaTime;
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

            // Call key down events from game objects here in order to subscribe
            // TODO: Implement functionality to automatically add game object events here
            //
            // There must be centralized object with abstract event methods, which will get called here with looping over them
        }

        protected override void KeyUp(IKeyboard arg1, Key arg2, int arg3)
        {
            base.KeyUp(arg1, arg2, arg3);
        }

        protected override void MouseClick(IMouse arg1, MouseButton arg2)
        {
            base.MouseClick(arg1, arg2);

            // Call mouse click events from game objects here 
        }

        protected override void MouseMove(IMouse arg1, PointF arg2)
        {
            base.MouseMove(arg1, arg2);
            //Console.WriteLine($"X:{arg2.X} Y:{arg2.Y}");

            // Call mouse move events from game objects here 
        }

        protected override void MouseDown(IMouse arg1, MouseButton arg2)
        {
            base.MouseDown(arg1, arg2);
        }

        protected override void MouseUp(IMouse arg1, MouseButton arg2)
        {
            base.MouseUp(arg1, arg2);
        }

        protected override void MouseDoubleClick(IMouse arg1, MouseButton arg2)
        {
            base.MouseDoubleClick(arg1, arg2);
        }

        protected override void MouseScroll(IMouse arg1, ScrollWheel arg2)
        {
            base.MouseScroll(arg1, arg2);
        }
    }
}