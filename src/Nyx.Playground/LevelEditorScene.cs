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
            // position        // color                // UV coordinates
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, // Bottom right 0
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, // Top left 1
            0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, // Top right 2
            -0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Bottom left 3
        };

        private readonly uint[] _vertexLocations = {0, 1, 2};

        private Shader _shader;
        private Texture _texture;

        public override void Init()
        {
            Camera = new Camera(Vector3.UnitZ * 6, Vector3.UnitZ * -1, Vector3.UnitY, (float) Width / Height);

            string shaderPath = GetFullPath("assets/shaders/square.glsl");
            _shader = new Shader(Gl, shaderPath);
            string texturePath = GetFullPath("assets/textures/mario.png");
            _texture = new Texture(Gl, TextureType.PixelSprite, texturePath);

            _vertexBufferObject = new BufferObject<float>(Gl, _vertexArray, BufferTargetARB.ArrayBuffer);
            _elementBufferObject =
                new BufferObject<uint>(Gl, _elementArray, BufferTargetARB.ElementArrayBuffer);
            _vertexArrayobject =
                new VertexArrayObject<float, uint>(Gl, _vertexBufferObject, _elementBufferObject);

            const int positionSize = 3;
            const int colorSize = 4;
            const int uVSize = 2;
            const uint vertexSizeBytes = (uint) (positionSize + colorSize + uVSize);

            _vertexArrayobject.VertexAttributePointer(0, positionSize, VertexAttribPointerType.Float, vertexSizeBytes,
                0);
            _vertexArrayobject.VertexAttributePointer(1, colorSize, VertexAttribPointerType.Float, vertexSizeBytes,
                positionSize);
            _vertexArrayobject.VertexAttributePointer(2, uVSize, VertexAttribPointerType.Float, vertexSizeBytes,
                positionSize + colorSize);

            _vertexArrayobject.EnableVertexAttribPointers(_vertexLocations);
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

            _shader.SetUniform("TEX_SAMPLER", 0);
            _texture.Activate();
            _texture.Bind();

            _shader.SetUniform("uModel", Matrix4x4.Identity);
            _shader.SetUniform("uProjection", Camera.GetProjectionMatrix());
            _shader.SetUniform("uView", Camera.GetViewMatrix());
            _vertexArrayobject.Bind();

            _vertexArrayobject.EnableVertexAttribPointers(_vertexLocations);

            DrawElements(_elementArray);

            _vertexArrayobject.DisableVertexAttribPointers(_vertexLocations);

            _shader.Detach();
            _texture.Detach();
            _vertexArrayobject.Detach();
        }

        public override void Dispose()
        {
            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();
            _texture.Dispose();
        }
    }
}