using System;
using System.Collections.Generic;
using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.Scene;
using Nyx.Core.Utils;
using Silk.NET.OpenGL;

namespace Nyx.Core.Renderer
{
    public class Batch : IDisposable
    {
        private const int PosSize = 2;
        private const int ColorSize = 4;
        private const int PosOffset = 0;
        private const int ColorOffset = PosOffset + PosSize;
        private const int VertexSize = PosSize + ColorSize;

        private static BufferObject<float> _vertexBufferObject;
        private static BufferObject<uint> _elementBufferObject;
        private static VertexArrayObject<float, uint> _vertexArrayobject;

        private readonly int _maxBatchSize;
        private readonly Shader _shader;

        private readonly SpriteRenderer[] _sprites;
        private readonly float[] _vertices;
        private int _numSprites;

        public Batch(int maxBatchSize)
        {
            _shader = AssetManager.GetShader("assets/shaders/default.glsl");

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
        }

        public void Start()
        {
            _vertexBufferObject =
                new BufferObject<float>(_vertices, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw);
            _elementBufferObject =
                new BufferObject<uint>(GenerateIndices(), BufferTargetARB.ElementArrayBuffer,
                    BufferUsageARB.StaticDraw);
            _vertexArrayobject =
                new VertexArrayObject<float, uint>(_vertexBufferObject, _elementBufferObject);

            _vertexArrayobject.VertexAttributePointer(0, PosSize, VertexAttribPointerType.Float, VertexSize,
                PosOffset);
            _vertexArrayobject.VertexAttributePointer(1, ColorSize, VertexAttribPointerType.Float, VertexSize,
                ColorOffset);
        }

        public void Render()
        {
            _vertexBufferObject.Bind();
            _vertexBufferObject.ReBufferData(_vertices);

            _shader.Use();

            _shader.SetUniform("uProjection", SceneContext.CurrentScene.Camera2D.ProjectionMatrix);
            _shader.SetUniform("uView", SceneContext.CurrentScene.Camera2D.GetViewMatrix());

            _vertexArrayobject.Bind();
            _vertexArrayobject.EnableVertexAttribPointers();

            GraphicsContext.DrawElements(PrimitiveType.Triangles, (uint) _numSprites * VertexSize);

            _vertexArrayobject.DisableVertexAttribPointers();
            _vertexArrayobject.Detach();

            _shader.Detach();
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

        private static void LoadElementIndices(IList<uint> elements, int index)
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
                _vertices[offset] = sprite.GameObject.Transform.Position.X +
                                    (xAdd * sprite.GameObject.Transform.Scale.X); // X
                _vertices[offset + 1] = sprite.GameObject.Transform.Position.Y +
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