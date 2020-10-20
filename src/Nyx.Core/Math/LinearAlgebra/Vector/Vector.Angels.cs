using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Returns angle in radians / degrees between two vectors.
        /// </summary>
        public virtual double AngleBetween(Vector other, AngleUnit unit)
        {
            return AngleBetween(other.InnerArray, unit);
        }

        /// <summary>
        ///     Returns angle in radians / degrees between two vectors.
        /// </summary>
        public virtual double AngleBetween(double[] other, AngleUnit unit)
        {
            return VectorFunctions.VectorFunctions.AngleBetween(InnerArray, other, unit);
        }
    }
}