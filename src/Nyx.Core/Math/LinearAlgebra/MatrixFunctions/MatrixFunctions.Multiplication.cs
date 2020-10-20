using System;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        ///     Multiplies/Scales a matrix by a scalar input.
        /// </summary>
        public static double[,] Multiply(double scalar, double[,] input)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);

            // creating the final product matrix
            var output = new double[rowCount, colCount];

            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    output[row, col] = scalar * input[row, col];
                }
            }

            return output;
        }

        /// <summary>
        ///     Multiplies two matrices together.
        /// </summary>
        public static double[,] Multiply(double[,] matrix1, double[,] matrix2)
        {
            // cahing matrix lengths for better performance
            int matrix1Rows = matrix1.GetLength(0);
            int matrix1Cols = matrix1.GetLength(1);
            int matrix2Rows = matrix2.GetLength(0);
            int matrix2Cols = matrix2.GetLength(1);

            // checking if product is defined
            if (matrix1Cols != matrix2Rows)
            {
                throw new InvalidOperationException(
                    "Undefined product. n columns of first matrix must equal to n rows of second matrix.");
            }

            // creating the final product matrix
            var output = new double[matrix1Rows, matrix2Cols];

            // looping through matrix 1 rows
            for (var matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
            {
                // for each matrix 1 row, loop through matrix 2 columns
                for (var matrix2_col = 0; matrix2_col < matrix2Cols; matrix2_col++)
                {
                    // loop through matrix 1 columns to calculate the dot product
                    for (var matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                    {
                        output[matrix1_row, matrix2_col] +=
                            matrix1[matrix1_row, matrix1_col] * matrix2[matrix1_col, matrix2_col];
                    }
                }
            }

            return output;
        }

        /// <summary>
        ///     Multiplies two matrices together. Uses unsafe code. Better with extremely large matrices.
        /// </summary>
        public static double[,] MultiplyUnsafe(double[,] matrix1, double[,] matrix2)
        {
            // cahing matrix lengths for better performance
            int matrix1Rows = matrix1.GetLength(0);
            int matrix1Cols = matrix1.GetLength(1);
            int matrix2Rows = matrix2.GetLength(0);
            int matrix2Cols = matrix2.GetLength(1);

            // checking if product is defined
            if (matrix1Cols != matrix2Rows)
            {
                throw new InvalidOperationException(
                    "Undefined product. n columns of first matrix must equal to n rows of second matrix.");
            }

            // creating the final product matrix
            var output = new double[matrix1Rows, matrix2Cols];

            unsafe
            {
                // fixing pointers to matrices
                fixed (
                    double* pProduct = output,
                    pMatrix1 = matrix1,
                    pMatrix2 = matrix2)
                {
                    var i = 0;
                    // looping through matrix 1 rows
                    for (var matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
                    {
                        // for each matrix 1 row, loop through matrix 2 columns
                        for (var matrix2_col = 0; matrix2_col < matrix2Cols; matrix2_col++)
                        {
                            // loop through matrix 1 columns to calculate the dot product
                            for (var matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                            {
                                double val1 = *(pMatrix1 + (matrix1Rows * matrix1_row) + matrix1_col);
                                double val2 = *(pMatrix2 + (matrix2Cols * matrix1_col) + matrix2_col);

                                *(pProduct + i) += val1 * val2;
                            }

                            i++;
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        ///     Raises an input matrix to the nth power.
        /// </summary>
        public static double[,] Power(double[,] input, int power)
        {
            if (input.GetLength(0) != input.GetLength(1))
            {
                throw new InvalidOperationException("Square matrix required.");
            }

            if (power < 0)
            {
                throw new ArgumentException(nameof(power));
            }

            if (power == 0)
            {
                return CreateIdentityMatrix(input.GetLength(0));
            }

            double[,] output = CreateCopy(input);
            for (var i = 0; i < power; i++)
            {
                output = Multiply(output, input);
            }

            return output;
        }
    }
}