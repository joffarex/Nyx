using OpenTK.Mathematics;

namespace Nyx.Core.Graphics
{
    public class Line2D
    {
        public Line2D(Vector2 from, Vector2 to, Vector3 color, int lifetime)
        {
            From = from;
            To = to;
            Color = color;
            Lifetime = lifetime;
        }

        public Vector2 From { get; }
        public Vector2 To { get; }
        public Vector3 Color { get; }
        public int Lifetime { get; private set; }

        public int BeginFrame()
        {
            Lifetime--;
            return Lifetime;
        }
    }
}