namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Creates a deep copy of the matrix.
        /// </summary>
        public object Clone()
        {
            return CreateCopy();
        }

        /// <summary>
        ///     Creates a deep copy of the matrix.
        /// </summary>
        public virtual Matrix CreateCopy()
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.CreateCopy(InnerMatrix));
        }

        /// <summary>
        ///     Creates a deep copy of the matrix using unsafe code. Might provide better performance with large matrices.
        /// </summary>
        public virtual Matrix CreateCopyUnsafe()
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.CreateCopyUnsafe(InnerMatrix));
        }
    }
}