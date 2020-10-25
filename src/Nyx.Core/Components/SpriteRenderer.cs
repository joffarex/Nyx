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
            Texture = null;
        }

        public SpriteRenderer(Texture texture)
        {
            Texture = texture;
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public Vector4 Color { get; }

        public Vector2[] TextureCoordinates
        {
            get
            {
                return new[]
                {
                    new Vector2(1.0f, 1.0f),
                    new Vector2(1.0f, 0.0f),
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 1.0f),
                };
            }
            private set => TextureCoordinates = value;
        }

        public Texture Texture { get; }

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