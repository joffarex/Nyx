using System;
using Nyx.Core.Math.LinearAlgebra.MatrixFunctions.Helpers;
using Nyx.Core.Math.LinearAlgebra.MatrixHelpers;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        ///     Returns a value indicates whether the specified matrix is invertible. Internally uses array determinant.
        /// </summary>
        public static bool IsInvertible(double[,] input)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);

            if (rowCount != colCount)
            {
                return false;
            }

            return Determinant(input) != 0;
        }

        /// <summary>
        ///     Calculates the inverse of a matrix. Returns null if non-invertible.
        /// </summary>
        public static double[,] Invert(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);

            if (rowCount != colCount)
            {
                throw new InvalidOperationException("Square matrix required.");
            }

            double[,] newMatrix = ConcatHorizontally(matrix, CreateIdentityMatrix(rowCount));

            MatrixEliminationResult result = Eliminate(newMatrix, MatrixReductionForm.ReducedRowEchelonForm, rowCount);
            if (result.Rank != colCount)
            {
                return null;
            }

            return result.AugmentedColumns;
        }
    }
}