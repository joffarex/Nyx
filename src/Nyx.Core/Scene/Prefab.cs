using System.Numerics;
using Nyx.Core.Components;
using Nyx.Ecs;

namespace Nyx.Core.Scene
{
    public class Prefab
    {
        public static GameObject GenerateSpriteObject(Sprite sprite, float sizeX, float sizeY)
        {
            var block = new GameObject("Sprite_Object_Gen",
                new Transform(new Vector2(), new Vector2(sizeX, sizeY)), 0);

            var spriteRenderer = new SpriteRenderer(sprite);
            block.AddComponent(spriteRenderer);

            return block;
        }
    }
}