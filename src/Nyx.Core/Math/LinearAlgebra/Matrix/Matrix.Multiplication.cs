namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Multiplies/scales matrix by a scalar input.
        /// </summary>
        public virtual Matrix Scale(double scalar)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Multiply(scalar, InnerMatrix));
        }


        /// <summary>
        ///     Multiplies matrix by another.
        /// </summary>
        public virtual Matrix Multiply(Matrix matrix)
        {
            return Multiply(matrix.InnerMatrix);
        }


        /// <summary>
        ///     Multiplies matrix by another.
        /// </summary>
        public virtual Matrix Multiply(double[,] matrix)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Multiply(InnerMatrix, matrix));
        }

        /// <summary>
        ///     Raises matrix to the spcified power.
        /// </summary>
        public virtual Matrix Power(int power)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Power(InnerMatrix, power));
        }
    }
}