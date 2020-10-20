using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Creates a deep copy of the vector.
        /// </summary>
        public object Clone()
        {
            return CreateCopy();
        }

        /// <summary>
        ///     Creates a deep copy of the vector.
        /// </summary>
        public virtual Vector CreateCopy()
        {
            return new Vector(VectorFunctions.VectorFunctions.Clone(InnerArray));
        }
    }
}