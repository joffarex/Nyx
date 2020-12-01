using System;
using System.Collections.Generic;
using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.Scenes;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Renderer
{
    public class Batch : IComparable<Batch>, IDisposable
    {
        private const int PositionSize = 2;
        private const int ColorSize = 4;
        private const int TextureCoordinatesSize = 2;
        private const int TextureIdSize = 1;
        private const int PositionOffset = 0;
        private const int ColorOffset = PositionOffset + PositionSize;
        private const int TextureCoordinatesOffset = ColorOffset + ColorSize;
        private const int TextureIdOffset = TextureCoordinatesOffset + TextureCoordinatesSize;
        private const int VertexSize = PositionSize + ColorSize + TextureCoordinatesSize + TextureIdSize;

        private static BufferObject<float> _vertexBufferObject;
        private static BufferObject<uint> _elementBufferObject;
        private static VertexArrayObject<float, uint> _vertexArrayobject;

        private readonly int _maxBatchSize;
        private readonly Shader _shader;

        private readonly SpriteRenderer[] _sprites;
        private readonly List<Texture> _textures;
        private readonly int[] _textureSlots = {0, 1, 2, 3, 4, 5, 6, 7};
        private readonly float[] _vertices;
        private int _numberOfSprites;

        private bool _rebufferData;

        public Batch(int maxBatchSize, int zIndex)
        {
            _shader = AssetManager.GetShader("assets/shaders/default.glsl");

            _sprites = new SpriteRenderer[maxBatchSize];
            _maxBatchSize = maxBatchSize;

            // 4 vertices per quad
            _vertices = new float[maxBatchSize * 4 * VertexSize];

            _numberOfSprites = 0;
            HasRoom = true;
            _textures = new List<Texture>();
            ZIndex = zIndex;
        }

        public bool HasRoom { get; private set; }
        public bool HasTextureRoom => _textures.Count < 8;
        public int ZIndex { get; }

        // We need this method in order to sort batches in renderer
        public int CompareTo(Batch other)
        {
            return ZIndex.CompareTo(other.ZIndex);
        }


        public void Dispose()
        {
            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();

            foreach (Texture texture in _textures)
            {
                texture.Dispose();
            }
        }

        public bool HasTexture(Texture texture)
        {
            return _textures.Contains(texture);
        }

        public void Start()
        {
            _vertexBufferObject =
                new BufferObject<float>(_vertices, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);
            _elementBufferObject =
                new BufferObject<uint>(GenerateIndices(), BufferTarget.ElementArrayBuffer,
                    BufferUsageHint.StaticDraw);
            _vertexArrayobject =
                new VertexArrayObject<float, uint>(_vertexBufferObject, _elementBufferObject);

            _vertexArrayobject.VertexAttributePointer(0, PositionSize, VertexAttribPointerType.Float, VertexSize,
                PositionOffset);
            _vertexArrayobject.VertexAttributePointer(1, ColorSize, VertexAttribPointerType.Float, VertexSize,
                ColorOffset);
            _vertexArrayobject.VertexAttributePointer(2, TextureCoordinatesSize, VertexAttribPointerType.Float,
                VertexSize,
                TextureCoordinatesOffset);
            _vertexArrayobject.VertexAttributePointer(3, TextureIdSize, VertexAttribPointerType.Float, VertexSize,
                TextureIdOffset);
        }

        public void Update(ref double deltaTime)
        {
            for (var i = 0; i < _numberOfSprites; i++)
            {
                SpriteRenderer sprite = _sprites[i];

                if (sprite.IsDirty)
                {
                    LoadVertexProperties(i);
                    sprite.IsDirty = false;
                    _rebufferData = true;
                }
            }
        }

        public void Render(ref double deltaTime)
        {
            if (_rebufferData)
            {
                _vertexBufferObject.Bind();
                _vertexBufferObject.ReBufferData(_vertices);
            }

            _shader.Use();

            _shader.SetMatrix4("uProjection", SceneManager.CurrentScene.Camera2D.ProjectionMatrix);
            _shader.SetMatrix4("uView", SceneManager.CurrentScene.Camera2D.GetViewMatrix());

            for (var i = 0; i < _textures.Count; i++)
            {
                Texture texture = _textures[i];
                texture.Use(TextureUnit.Texture0 + i + 1);
            }

            _shader.SetIntArray("uTextures", _textureSlots);

            _vertexArrayobject.Bind();
            _vertexArrayobject.EnableVertexAttribPointers();

            GL.DrawElements(PrimitiveType.Triangles, _numberOfSprites * VertexSize, DrawElementsType.UnsignedInt,
                IntPtr.Zero);

            _vertexArrayobject.DisableVertexAttribPointers();
            _vertexArrayobject.Unbind();

            foreach (Texture texture in _textures)
            {
                texture.Detach();
            }

            _shader.Detach();
        }

        public void AddSprite(SpriteRenderer sprite)
        {
            int index = _numberOfSprites; // At the end of sprites;
            _sprites[index] = sprite;
            _numberOfSprites++;

            if (sprite.Texture != null)
            {
                if (!_textures.Contains(sprite.Texture))
                {
                    _textures.Add(sprite.Texture);
                }
            }

            LoadVertexProperties(index);

            if (_numberOfSprites >= _maxBatchSize)
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

            Vector4 color = sprite.Color;
            Vector2[] textureCoordinates = sprite.TextureCoordinates;

            var textureId = 0;

            if (sprite.Texture != null)
            {
                for (var i = 0; i < _textures.Count; i++)
                {
                    Texture texture = _textures[i];
                    if (ReferenceEquals(texture, sprite.Texture))
                    {
                        textureId = i + 1;
                        break;
                    }
                }
            }

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
                _vertices[offset] = sprite.Entity.Transform.Position.X +
                                    (xAdd * sprite.Entity.Transform.Scale.X); // X
                _vertices[offset + 1] = sprite.Entity.Transform.Position.Y +
                                        (yAdd * sprite.Entity.Transform.Scale.Y); // Y

                // Load color
                _vertices[offset + 2] = color.X; // R
                _vertices[offset + 3] = color.Y; // G
                _vertices[offset + 4] = color.Z; // B
                _vertices[offset + 5] = color.W; // A

                // Load texture coordinates
                _vertices[offset + 6] = textureCoordinates[i].X;
                _vertices[offset + 7] = textureCoordinates[i].Y;

                // Load texture id
                _vertices[offset + 8] = textureId;

                offset += VertexSize;
            }
        }
    }
}