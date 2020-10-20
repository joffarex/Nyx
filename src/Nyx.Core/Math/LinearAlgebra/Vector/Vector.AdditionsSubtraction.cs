using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Adds another vector (right).
        /// </summary>
        public virtual Vector Add(double[] vector)
        {
            return new Vector(VectorFunctions.VectorFunctions.Add(InnerArray, vector));
        }

        /// <summary>
        ///     Adds another vector (right).
        /// </summary>
        public virtual Vector Add(Vector vector)
        {
            return Add(vector.InnerArray);
        }

        /// <summary>
        ///     Subtracts another vector (right).
        /// </summary>
        public virtual Vector Subtract(double[] vector)
        {
            return new Vector(VectorFunctions.VectorFunctions.Subtract(InnerArray, vector));
        }

        /// <summary>
        ///     Subtracts another vector (right).
        /// </summary>
        public virtual Vector Subtract(Vector vector)
        {
            return Subtract(vector.InnerArray);
        }
    }
}