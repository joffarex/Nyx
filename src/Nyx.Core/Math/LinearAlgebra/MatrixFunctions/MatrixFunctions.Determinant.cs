using System;
using System.Linq;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        ///     Calculates determinant. Internally uses Laplace Expansion method.
        /// </summary>
        /// <remarks>
        ///     Returns 1 for an empty matrix. See
        ///     https://math.stackexchange.com/questions/1762537/what-is-the-determinant-of/1762542
        /// </remarks>
        public static double Determinant(double[,] input)
        {
            double[] results = CrossProduct(input);

            return results.Sum();
        }

        public static double[] CrossProduct(double[,] input)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);

            if (rowCount != colCount)
            {
                throw new InvalidOperationException("Square matrix required.");
            }

            if (rowCount == 0)
            {
                return new double[] {1};
            }

            if (rowCount == 1)
            {
                return new[] {input[0, 0]};
            }

            if (rowCount == 2)
            {
                return new[] {(input[0, 0] * input[1, 1]) - (input[0, 1] * input[1, 0])};
            }

            var results = new double[colCount];

            for (var col = 0; col < colCount; col++)
            {
                // checkerboard pattern, even col  = 1, odd col = -1
                int checkerboardFactor = (col % 2.0) == 0 ? 1 : -1;
                double coeffecient = input[0, col];

                double[,] crossMatrix = GetCrossMatrix(input, 0, col);
                results[col] = checkerboardFactor * (coeffecient * Determinant(crossMatrix));
            }

            return results;
        }

        /// <summary>
        ///     Retrieves all matrix entries except the specified row and col. Used in cross product and determinant functions.
        /// </summary>
        public static double[,] GetCrossMatrix(double[,] input, int skipRow, int skipCol)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);

            var output = new double[rowCount - 1, colCount - 1];
            var outputRow = 0;

            for (var row = 0; row < rowCount; row++)
            {
                if (row == skipRow)
                {
                    continue;
                }

                var outputCol = 0;

                for (var col = 0; col < colCount; col++)
                {
                    if (col == skipCol)
                    {
                        continue;
                    }

                    output[outputRow, outputCol] = input[row, col];

                    outputCol++;
                }

                outputRow++;
            }

            return output;
        }
    }
}