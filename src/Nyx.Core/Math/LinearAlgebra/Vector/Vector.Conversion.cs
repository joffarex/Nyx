using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Converts vector to matrix.
        /// </summary>
        public virtual Matrix.Matrix AsMatrix()
        {
            return new Matrix.Matrix(VectorFunctions.VectorFunctions.ToMatrix(InnerArray));
        }
    }
}