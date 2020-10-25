using System.Numerics;
using Nyx.Core.Renderer;

namespace Nyx.Core.Components
{
    public class Sprite
    {
        public Sprite(Texture texture)
        {
            Texture = texture;
            TextureCoordinates = new[]
            {
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
            };
        }

        public Sprite(Texture texture, Vector2[] textureCoordinates)
        {
            Texture = texture;
            TextureCoordinates = textureCoordinates;
        }

        public Texture Texture { get; }
        public Vector2[] TextureCoordinates { get; }
    }
}