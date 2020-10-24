using System.Numerics;
using Nyx.SharpTT;

namespace Nyx.Core.Components
{
    public class SpriteRenderer : Component
    {
        public SpriteRenderer(Vector4 color)
        {
            Color = color;
        }

        public Vector4 Color { get; }

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