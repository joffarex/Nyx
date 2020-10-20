namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        ///     Creates projection matrix for the specified subspace.
        /// </summary>
        public static double[,] CreateProjectionMatrix(double[,] subspace)
        {
            double[,] subspaceTranspose = Transpose(subspace);

            double[,] value = Multiply(subspaceTranspose, subspace);

            value = Invert(value);

            value = Multiply(value, subspaceTranspose);

            value = Multiply(subspace, value);

            return value;
        }
    }
}