using System.Collections.Generic;
using System.Numerics;
using Nyx.Core.Renderer;

namespace Nyx.Core.Graphics
{
    public class SpriteSheet
    {
        public SpriteSheet()
        {
        }

        public SpriteSheet(Texture texture, Vector2 spriteSize, int numberOfSprites, int padding)
        {
            Init(texture, spriteSize, numberOfSprites, padding);
        }

        public Vector2 SpriteSize { get; set; }
        public Texture Texture { get; set; }
        public List<Sprite> Sprites { get; set; }
        public int SpritesCount => Sprites.Count;

        public void Init(Texture texture, Vector2 spriteSize, int numberOfSprites, int padding)
        {
            Sprites = new List<Sprite>();

            Texture = texture;
            SpriteSize = spriteSize;

            // Bottom left cornet of top left sprite in sheet
            float currentX = 0;
            float currentY = texture.Height - SpriteSize.Y;

            for (var i = 0; i < numberOfSprites; i++)
            {
                // Normalize coordinates
                float topY = (currentY + SpriteSize.Y) / texture.Height;
                float rightX = (currentX + SpriteSize.X) / texture.Width;
                float leftX = currentX / texture.Width;
                float bottomY = currentY / texture.Height;

                Vector2[] textureCoordinates =
                {
                    new(rightX, topY),
                    new(rightX, bottomY),
                    new(leftX, bottomY),
                    new(leftX, topY),
                };

                var sprite = new Sprite(texture, textureCoordinates, (int) SpriteSize.X, (int) SpriteSize.Y);
                Sprites.Add(sprite);

                currentX += SpriteSize.X + padding;
                if (currentX >= texture.Width)
                {
                    currentX = 0;
                    currentY -= SpriteSize.Y + padding;
                }
            }
        }
    }
}