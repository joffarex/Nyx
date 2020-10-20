namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public class Vector3 : Vector
    {
        public Vector3() : base(3)
        {
        }

        public Vector3(double x, double y, double z) : base(new[] {x, y, z})
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

        public double Z
        {
            get => this[2];
            set => this[2] = value;
        }
    }
}