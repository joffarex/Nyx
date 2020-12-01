using System;
using System.Numerics;
using Nyx.Core.Renderer;

namespace Nyx.Core.Graphics
{
    public class Sprite
    {
        public Sprite()
        {
        }

        public Sprite(Texture texture)
        {
            if (texture is null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Init(texture, new[]
            {
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
            }, texture.Width, texture.Height);
        }

        public Sprite(int width, int height)
        {
            Init(null, new[]
            {
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
            }, width, height);
        }

        public Sprite(Texture texture, Vector2[] textureCoordinates)
        {
            if (texture is null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Init(texture, textureCoordinates, texture.Width, texture.Height);
        }

        public Sprite(Texture texture, Vector2[] textureCoordinates, int width, int height)
        {
            if (texture is null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Init(texture, textureCoordinates, width, height);
        }

        public bool IsFlipped { get; private set; }

        public Texture Texture { get; set; }

        public Vector2[] TextureCoordinates { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public void FlipY(bool backToNormal = false)
        {
            if (!IsFlipped)
            {
                Array.Reverse(TextureCoordinates);
                IsFlipped = true;
            }

            if (backToNormal && IsFlipped)
            {
                Array.Reverse(TextureCoordinates);
                IsFlipped = false;
            }
        }

        private void Init(Texture texture, Vector2[] textureCoordinates, int width, int height)
        {
            Texture = texture;
            TextureCoordinates = textureCoordinates;
            Width = width;
            Height = height;
        }

        public int GetTextureHandle()
        {
            return Texture?.Handle ?? 0;
        }
    }
}