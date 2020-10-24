using System.Drawing;
using System.Numerics;
using Nyx.Core.OpenGL;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using static Nyx.Playground.Game;
using static Nyx.NyxEngine;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        private static BufferObject<float> _vertexBufferObject;
        private static BufferObject<uint> _elementBufferObject;
        private static VertexArrayObject<float, uint> _vertexArrayobject;

        private readonly uint[] _elementArray =
        {
            2, 1, 0, // Top right triangle,
            0, 1, 3, // Bottom left triangle
        };

        private readonly float[] _vertexArray =
        {
            // position        // color
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, // Bottom right 0
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, // Top left 1
            0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, // Top right 2
            -0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, // Bottom left 3
        };

        private Shader _shader;

        public override void Init()
        {
            Camera = new Camera(Vector3.UnitZ * 6, Vector3.UnitZ * -1, Vector3.UnitY, (float) Width / Height);

            string shaderPath = GetFullPath("Shaders/square.glsl");
            _shader = new Shader(Gl, shaderPath);

            _vertexBufferObject = new BufferObject<float>(Gl, _vertexArray, BufferTargetARB.ArrayBuffer);
            _elementBufferObject =
                new BufferObject<uint>(Gl, _elementArray, BufferTargetARB.ElementArrayBuffer);
            _vertexArrayobject =
                new VertexArrayObject<float, uint>(Gl, _vertexBufferObject, _elementBufferObject);

            _vertexArrayobject.SetVertexAttribPointers3Pos4Col();
            _vertexArrayobject.EnableVertexAttribPointers(new uint[] {0, 1});
        }

        public override void Update(float deltaTime)
        {
            float moveSpeed = 2.5f * deltaTime;

            if (Input.IsKeyPressed(Key.W))
            {
                Camera.Position += moveSpeed * Camera.Front;
            }

            if (Input.IsKeyPressed(Key.S))
            {
                Camera.Position -= moveSpeed * Camera.Front;
            }

            if (Input.IsKeyPressed(Key.A))
            {
                Camera.Position -= Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * moveSpeed;
            }

            if (Input.IsKeyPressed(Key.D))
            {
                Camera.Position += Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * moveSpeed;
            }
        }

        public override void MouseMove(IMouse mouse, PointF position)
        {
            const float lookSensitivity = 0.1f;
            if (LastMousePosition == default)
            {
                LastMousePosition = position;
            }
            else
            {
                float xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                float yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;

                Camera.ModifyDirection(xOffset, yOffset);
            }
        }

        public override void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
            Camera.ModifyZoom(scrollWheel.Y);
        }

        public override void Render()
        {
            _shader.Use();
            _shader.SetUniform("uModel", Matrix4x4.Identity);
            _shader.SetUniform("uProjection", Camera.GetProjectionMatrix());
            _shader.SetUniform("uView", Camera.GetViewMatrix());
            _vertexArrayobject.Bind();

            _vertexArrayobject.EnableVertexAttribPointers(new uint[] {0, 1});

            DrawElements(_elementArray);

            _vertexArrayobject.DisableVertexAttribPointers(new uint[] {0, 1});

            _shader.Detach();
            _vertexArrayobject.Detach();
        }

        public override void Dispose()
        {
            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();
        }
    }
}