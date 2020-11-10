using System.Numerics;

namespace Nyx.Core.Renderer
{
    public class Line2D
    {
        private uint _handle;

        public Line2D(Vector2 from, Vector2 to, Vector3 color, int lifetime)
        {
            From = from;
            To = to;
            Color = color;
            Lifetime = lifetime;
        }

        public Vector2 From { get; init; }
        public Vector2 To { get; init; }
        public Vector3 Color { get; init; }
        public int Lifetime { get; private set; }

        public int BeginFrame()
        {
            Lifetime--;
            return Lifetime;
        }
    }
}