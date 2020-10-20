using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Dot product of current vector and another vector.
        /// </summary>
        public virtual double DotProduct(Vector other)
        {
            return DotProduct(other.InnerArray);
        }

        /// <summary>
        ///     Dot product of current vector and another vector.
        /// </summary>
        public virtual double DotProduct(double[] other)
        {
            return VectorFunctions.VectorFunctions.DotProduct(InnerArray, other);
        }

        /// <summary>
        ///     Multiplies/scales vector by a scalar input.
        /// </summary>
        public virtual Vector Scale(double scalar)
        {
            return new Vector(VectorFunctions.VectorFunctions.Scale(scalar, InnerArray));
        }

        /// <summary>
        ///     Cross product of current vector with another vector.
        /// </summary>
        public virtual Vector CrossProduct(Vector other)
        {
            return CrossProduct(other.InnerArray);
        }


        /// <summary>
        ///     Cross product of current vector with another vector.
        /// </summary>
        public virtual Vector CrossProduct(double[] other)
        {
            return new Vector(VectorFunctions.VectorFunctions.CrossProduct(InnerArray, other));
        }

        /// <summary>
        ///     Returns True if two vectors are perpendicular.
        /// </summary>
        public virtual bool IsPerpendicular(Vector other)
        {
            return IsPerpendicular(other.InnerArray);
        }

        /// <summary>
        ///     Returns True if two vectors are perpendicular.
        /// </summary>
        public virtual bool IsPerpendicular(double[] other)
        {
            return DotProduct(other) == 0;
        }
    }
}