using System;
using System.Numerics;

namespace Nyx.Core.Shared
{
    public class Transform : IEquatable<Transform>
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

        public bool Equals(Transform other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Position.Equals(other.Position) && Scale.Equals(other.Scale);
        }

        public void Init(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }

        public Transform Copy()
        {
            return new(new Vector2(Position.X, Position.Y), new Vector2(Scale.X, Scale.Y));
        }

        public void Copy(Transform to)
        {
            to.Position = Position;
            to.Scale = Scale;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
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
            return HashCode.Combine(Position, Scale);
        }

        public override string ToString()
        {
            return string.Format($"Position: {Position} - Scale: {Scale}");
        }
    }
}