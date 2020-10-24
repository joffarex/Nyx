using System;
using System.Collections.Generic;
using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.Utils;
using Silk.NET.OpenGL;
using static Nyx.Core.NyxEngine;

namespace Nyx.Core.OpenGL
{
    public class RenderBatch : IDisposable
    {
        private const int PosSize = 2;
        private const int ColorSize = 4;
        private const int PosOffset = 0;
        private const int ColorOffset = PosOffset + PosSize;
        private const int VertexSize = PosSize + ColorSize;

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

        private readonly int _maxBatchSize;
        private int _numSprites;
        private readonly Shader _shader;

        private readonly SpriteRenderer[] _sprites;
        private readonly Texture _texture;
        private readonly float[] _vertices;


        public RenderBatch(int maxBatchSize)
        {
            string shaderPath = PathUtils.GetFullPath("assets/shaders/sprite-with-camera.glsl");
            _shader = new Shader(Gl, shaderPath);
            string texturePath = PathUtils.GetFullPath("assets/sprites/mario.png");
            _texture = new Texture(Gl, TextureType.PixelSprite, texturePath);

            _sprites = new SpriteRenderer[maxBatchSize];
            _maxBatchSize = maxBatchSize;

            // 4 vertices per quad
            _vertices = new float[maxBatchSize * 4 * VertexSize];

            _numSprites = 0;
            HasRoom = true;
        }

        public bool HasRoom { get; private set; }

        public void Dispose()
        {
            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();
            _texture.Dispose();
        }

        public void Start()
        {
            _vertexBufferObject =
                new BufferObject<float>(Gl, _vertexArray, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw);
            _elementBufferObject =
                new BufferObject<uint>(Gl, _elementArray, BufferTargetARB.ElementArrayBuffer,
                    BufferUsageARB.StaticDraw);
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
        }

        public void AddSprite(SpriteRenderer sprite)
        {
            int index = _numSprites; // At the end of sprites;
            _sprites[index] = sprite;
            _numSprites++;

            LoadVertexProperties(index);

            if (_numSprites >= _maxBatchSize)
            {
                HasRoom = false;
            }
        }

        private uint[] GenerateIndices()
        {
            // 6 indices per quad (3 per triangle)
            var elements = new uint[6 * _maxBatchSize];
            for (var i = 0; i < _maxBatchSize; i++)
            {
                LoadElementIndices(elements, i);
            }

            return elements;
        }

        private void LoadElementIndices(IList<uint> elements, int index)
        {
            int offsetArrayIndex = 6 * index;
            var offset = (uint) (4 * index);

            // 3, 2, 0, 0, 2, 1    7, 6, 4, 4, 6, 5

            // Triangle 1 (3, 2, 0), offset automatically gets next "Triangle 1" by index, which is (7, 6, 4)
            elements[offsetArrayIndex] = offset + 3;
            elements[offsetArrayIndex + 1] = offset + 2;
            elements[offsetArrayIndex + 2] = offset + 0;

            // Triangle 2 (0, 2, 1)
            elements[offsetArrayIndex + 3] = offset + 0;
            elements[offsetArrayIndex + 4] = offset + 2;
            elements[offsetArrayIndex + 5] = offset + 1;
        }

        public unsafe void Render()
        {
            _shader.Use();
            _shader.SetUniform("TEX_SAMPLER", 0);
            _texture.Activate();
            _texture.Bind();

            _shader.SetUniform("uModel", Matrix4x4.Identity);
            _shader.SetUniform("uProjection", Game.CurrentScene.Camera.GetProjectionMatrix());
            _shader.SetUniform("uView", Game.CurrentScene.Camera.GetViewMatrix());

            _vertexArrayobject.Bind();
            _vertexArrayobject.EnableVertexAttribPointers();

            Gl.DrawElements(PrimitiveType.Triangles, (uint) _elementArray.Length, DrawElementsType.UnsignedInt, null);

            _vertexArrayobject.DisableVertexAttribPointers();

            _shader.Detach();
            _texture.Detach();
            _vertexArrayobject.Detach();
        }

        private void LoadVertexProperties(int index)
        {
            SpriteRenderer sprite = _sprites[index];

            // Find offset within array (4 vertices per sprite)
            int offset = index * 4 * VertexSize;

            // Pos    Color
            // f f    f f f f 

            Vector4 color = sprite.Color;

            // *      *
            // [*]    *
            // Where [*] -> is a starting vertex position, from what point we start drawing

            var xAdd = 1.0f;
            var yAdd = 1.0f;

            for (var i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 1:
                        yAdd = 0.0f;
                        break;
                    case 2:
                        xAdd = 0.0f;
                        break;
                    case 3:
                        yAdd = 1.0f;
                        break;
                }

                // Load position
                _vertices[offset] = sprite.GameObject.Transform.Poistion.X +
                                    (xAdd * sprite.GameObject.Transform.Scale.X); // X
                _vertices[offset + 1] = sprite.GameObject.Transform.Poistion.Y +
                                        (yAdd * sprite.GameObject.Transform.Scale.Y); // Y

                // Load color
                _vertices[offset + 2] = color.X; // R
                _vertices[offset + 3] = color.Y; // G
                _vertices[offset + 4] = color.Z; // B
                _vertices[offset + 5] = color.W; // A

                offset += VertexSize;
            }
        }
    }
}