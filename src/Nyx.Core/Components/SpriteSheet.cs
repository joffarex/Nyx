using System.Collections.Generic;
using System.Numerics;
using Nyx.Core.Renderer;

namespace Nyx.Core.Components
{
    public class SpriteSheet
    {
        public SpriteSheet(Texture texture, int spriteWidth, int spriteHeight, int numSprites, int spacing)
        {
            Sprites = new List<Sprite>();

            Texture = texture;

            SpriteSize = new Vector2(spriteWidth, spriteHeight);

            // Bottom left cornet of top left sprite in sheet
            var currentX = 0;
            int currentY = texture.Height - spriteHeight;

            for (var i = 0; i < numSprites; i++)
            {
                // Normalize coordinates
                float topY = (currentY + spriteHeight) / (float) texture.Height;
                float rightX = (currentX + spriteWidth) / (float) texture.Width;
                float leftX = currentX / (float) texture.Width;
                float bottomY = currentY / (float) texture.Height;

                Vector2[] textureCoordinates =
                {
                    new Vector2(rightX, topY),
                    new Vector2(rightX, bottomY),
                    new Vector2(leftX, bottomY),
                    new Vector2(leftX, topY),
                };

                var sprite = new Sprite(texture, textureCoordinates, spriteWidth, spriteHeight);
                Sprites.Add(sprite);

                currentX += spriteWidth + spacing;
                if (currentX >= texture.Width)
                {
                    currentX = 0;
                    currentY -= spriteHeight + spacing;
                }
            }
        }

        public Vector2 SpriteSize { get; set; }

        public Texture Texture { get; set; }
        public List<Sprite> Sprites { get; }

        public int Count()
        {
            return Sprites.Count;
        }
    }
}