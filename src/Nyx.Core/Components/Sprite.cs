using System;
using System.Numerics;
using Nyx.Core.Renderer;

namespace Nyx.Core.Components
{
    public class Sprite
    {
        public Sprite()
        {
        }

        public Sprite(Texture texture)
        {
            if (texture == null)
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
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Init(texture, textureCoordinates, texture.Width, texture.Height);
        }

        public Sprite(Texture texture, Vector2[] textureCoordinates, int width, int height)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Init(texture, textureCoordinates, width, height);
        }

        public Texture Texture { get; set; }

        public Vector2[] TextureCoordinates { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        private void Init(Texture texture, Vector2[] textureCoordinates, int width, int height)
        {
            Texture = texture;
            TextureCoordinates = textureCoordinates;
            Width = width;
            Height = height;
        }

        public uint GetTextureHandle()
        {
            return Texture == null ? 0 : Texture.Handle;
        }
    }
}