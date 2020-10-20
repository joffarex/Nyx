using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        public static Vector operator +(Vector m)
        {
            return m;
        }

        public static Vector operator -(Vector m)
        {
            return m.Scale(-1);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return a.Add(b);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return a.Subtract(b);
        }

        public static Vector operator +(Vector a, double[] b)
        {
            return a.Add(b);
        }

        public static Vector operator -(Vector a, double[] b)
        {
            return a.Subtract(b);
        }

        public static Vector operator *(double a, Vector m)
        {
            return m.Scale(a);
        }

        public static Vector operator *(Vector a, double b)
        {
            return a.Scale(b);
        }

        public static double operator *(Vector a, Vector b)
        {
            return a.DotProduct(b);
        }

        public static double operator *(Vector a, double[] b)
        {
            return a.DotProduct(b);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            if ((a as object == null) || (b as object == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Vector a, Vector b)
        {
            if ((a as object == null) || (b as object == null))
            {
                return false;
            }

            return a.Equals(b) == false;
        }

        public static implicit operator Matrix.Matrix(Vector m)
        {
            return m.AsMatrix();
        }

        public static implicit operator Vector(Matrix.Matrix m)
        {
            return FromMatrix(m);
        }

        public static implicit operator double[](Vector m)
        {
            return m.InnerArray;
        }

        public static explicit operator Vector(double[] m)
        {
            return new Vector(m);
        }
    }
}