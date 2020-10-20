using System;
using System.Linq;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        ///     Removes specific columns from an input matrix.
        /// </summary>
        public static double[,] RemoveColumns(double[,] input, int[] colsToRemove)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);
            int newColCount = colCount - colsToRemove.Distinct().Count();

            if (newColCount <= 0)
            {
                throw new InvalidOperationException("Too much columns to remove.");
            }

            var output = new double[rowCount, newColCount];

            for (var row = 0; row < rowCount; row++)
            {
                var outputCol = 0;
                for (var col = 0; col < colCount; col++)
                {
                    if (colsToRemove.Contains(col))
                    {
                        continue;
                    }

                    output[row, outputCol] = input[row, col];
                    outputCol++;
                }
            }

            return output;
        }

        /// <summary>
        ///     Removes specific rows from an input matrix.
        /// </summary>
        public static double[,] RemoveRows(double[,] input, params int[] rowsToRemove)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);
            int newRowCount = rowCount - rowsToRemove.Distinct().Count();

            if (newRowCount <= 0)
            {
                throw new InvalidOperationException("Too much rows to remove.");
            }

            var output = new double[newRowCount, colCount];

            for (var col = 0; col < colCount; col++)
            {
                var outputRow = 0;
                for (var row = 0; row < rowCount; row++)
                {
                    if (rowsToRemove.Contains(row))
                    {
                        continue;
                    }

                    output[outputRow, col] = input[row, col];
                    outputRow++;
                }
            }

            return output;
        }

        /// <summary>
        ///     Concats two matrices horizontally.
        /// </summary>
        public static double[,] ConcatHorizontally(double[,] matrix1, double[,] matrix2)
        {
            int rowCount = matrix1.GetLength(0);

            if (rowCount != matrix2.GetLength(0))
            {
                throw new InvalidOperationException("Row count mismatch.");
            }

            int matrix1Cols = matrix1.GetLength(1);
            int matrix2Cols = matrix2.GetLength(1);

            var output = new double[rowCount, matrix1Cols + matrix2Cols];
            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < (matrix1Cols + matrix2Cols); col++)
                {
                    if (col < matrix1Cols)
                    {
                        output[row, col] = matrix1[row, col];
                    }
                    else
                    {
                        output[row, col] = matrix2[row, col - matrix1Cols];
                    }
                }
            }

            return output;
        }


        /// <summary>
        ///     Concats two matrices vertically.
        /// </summary>
        public static double[,] ConcatVertically(double[,] matrix1, double[,] matrix2)
        {
            int columnCount = matrix1.GetLength(1);

            if (columnCount != matrix2.GetLength(1))
            {
                throw new InvalidOperationException("Column count mismatch.");
            }

            int matrix1Rows = matrix1.GetLength(0);
            int matrix2Rows = matrix2.GetLength(0);

            var output = new double[matrix1Rows + matrix2Rows, columnCount];
            for (var col = 0; col < columnCount; col++)
            {
                for (var row = 0; row < (matrix1Rows + matrix2Rows); row++)
                {
                    if (row < matrix1Rows)
                    {
                        output[row, col] = matrix1[row, col];
                    }
                    else
                    {
                        output[row, col] = matrix2[row - matrix1Rows, col];
                    }
                }
            }

            return output;
        }
    }
}