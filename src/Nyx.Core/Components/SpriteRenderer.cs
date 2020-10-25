using System.Numerics;
using Nyx.Core.Renderer;
using Nyx.SharpTT;

namespace Nyx.Core.Components
{
    public class SpriteRenderer : Component
    {
        public SpriteRenderer(Vector4 color)
        {
            Color = color;
            Sprite = new Sprite(null);
        }

        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public Vector4 Color { get; }

        public Vector2[] TextureCoordinates => Sprite.TextureCoordinates;

        public Sprite Sprite { get; set; }

        public Texture Texture => Sprite.Texture;

        public override void Start()
        {
        }


        public override void Update(float deltaTime)
        {
        }

        public override void Render()
        {
        }
    }
}