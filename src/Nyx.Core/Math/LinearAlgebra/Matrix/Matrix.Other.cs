namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Calculates determinant. Internally uses Laprace Expansion method.
        /// </summary>
        /// <remarks>
        ///     Returns 1 for an empty matrix. See
        ///     https://math.stackexchange.com/questions/1762537/what-is-the-determinant-of/1762542
        /// </remarks>
        public virtual double GetDeterminant()
        {
            return MatrixFunctions.MatrixFunctions.Determinant(InnerMatrix);
        }

        /// <summary>
        ///     Transposes matrix.
        /// </summary>
        public virtual Matrix Transpose()
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Transpose(InnerMatrix));
        }

        /// <summary>
        ///     Rounds matrix entries to the nearest integeral value.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public virtual Matrix Round(int decimals)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.Round(InnerMatrix, decimals));
        }
    }
}