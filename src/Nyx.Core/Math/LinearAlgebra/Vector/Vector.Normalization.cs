using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Calculates magnitude/length of vector.
        /// </summary>
        public virtual double GetMagnitude()
        {
            return VectorFunctions.VectorFunctions.GetMagnitude(InnerArray);
        }

        /// <summary>
        ///     Normalizes vector.
        /// </summary>
        public virtual Vector ToUnitVector()
        {
            return new Vector(VectorFunctions.VectorFunctions.ToUnitVector(InnerArray));
        }
    }
}