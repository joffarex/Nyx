namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Adds another matrix (right).
        /// </summary>
        public virtual Matrix Add(double[,] matrix)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Add(InnerMatrix, matrix));
        }

        /// <summary>
        ///     Adds another matrix (right).
        /// </summary>
        public virtual Matrix Add(Matrix matrix)
        {
            return Add(matrix.InnerMatrix);
        }

        /// <summary>
        ///     Subtracts another matrix (right).
        /// </summary>
        public virtual Matrix Subtract(double[,] matrix)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Subtract(InnerMatrix, matrix));
        }

        /// <summary>
        ///     Subtracts another matrix (right).
        /// </summary>
        public virtual Matrix Subtract(Matrix matrix)
        {
            return Subtract(matrix.InnerMatrix);
        }
    }
}