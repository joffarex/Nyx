using System.Numerics;

namespace Nyx.Core
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

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public void Init(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }

        public Transform Copy()
        {
            return new Transform(new Vector2(Position.X, Position.Y), new Vector2(Scale.X, Scale.Y));
        }

        public void Copy(Transform to)
        {
            to.Position = Position;
            to.Scale = Scale;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Transform))
            {
                return false;
            }

            var t = (Transform) obj;
            return (t.Position == Position) && (t.Scale == Scale);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                hash = (hash * 23) + Position.GetHashCode();
                hash = (hash * 23) + Scale.GetHashCode();
                return hash;
            }
        }
    }
}