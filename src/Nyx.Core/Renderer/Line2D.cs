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

        public Vector2 From { get; set; }
        public Vector2 To { get; set; }
        public Vector3 Color { get; set; }
        public int Lifetime { get; set; }

        public int BeginFrame()
        {
            Lifetime--;
            return Lifetime;
        }
    }
}