namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public class Vector2 : Vector
    {
        public Vector2() : base(2)
        {
        }

        public Vector2(double x, double y) : base(new[] {x, y})
        {
        }

        public double X
        {
            get => this[0];
            set => this[0] = value;
        }

        public double Y
        {
            get => this[1];
            set => this[1] = value;
        }
    }
}