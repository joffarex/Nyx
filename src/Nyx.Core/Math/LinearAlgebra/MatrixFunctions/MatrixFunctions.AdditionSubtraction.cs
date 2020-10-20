using System;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        public static double[,] Add(double[,] matrix1, double[,] matrix2)
        {
            return InternalAdd(matrix1, matrix2, 1);
        }

        public static double[,] Subtract(double[,] matrix1, double[,] matrix2)
        {
            return InternalAdd(matrix1, matrix2, -1);
        }

        private static double[,] InternalAdd(double[,] matrix1, double[,] matrix2, double coeffecient)
        {
            int rowCount = matrix1.GetLength(0);
            int colCount = matrix1.GetLength(1);

            if ((rowCount != matrix2.GetLength(0)) || (colCount != matrix2.GetLength(1)))
            {
                throw new InvalidOperationException("Matrices dimensions mismatch.");
            }

            var output = new double[matrix1.GetLength(0), matrix2.GetLength(1)];

            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    output[row, col] = matrix1[row, col] + (coeffecient * matrix2[row, col]);
                }
            }

            return output;
        }
    }
}