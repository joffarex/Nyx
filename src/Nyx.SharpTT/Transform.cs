using System.Numerics;

namespace Nyx.Core.OpenGL
{
    public class Transform
    {
        public Transform()
        {
            Init(Vector2.Zero, Vector2.Zero);
        }

        public Transform(Vector2 position)
        {
            Init(position, Vector2.Zero);
        }

        public Transform(Vector2 position, Vector2 scale)
        {
            Init(position, scale);
        }

        public Vector2 Poistion { get; set; }
        public Vector2 Scale { get; set; }

        public void Init(Vector2 position, Vector2 scale)
        {
            Poistion = position;
            Scale = scale;
        }
    }
}