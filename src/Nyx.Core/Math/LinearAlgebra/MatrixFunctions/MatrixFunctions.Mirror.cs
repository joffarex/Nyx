namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        public static double[,] MirrorVertically(double[,] input)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);


            var output = new double[rowCount, colCount];

            for (var row = 0; row < rowCount; row++)
            {
                for (int col = colCount - 1, outCol = 0; col >= 0; col--, outCol++)
                {
                    output[row, outCol] = input[row, col];
                }
            }

            return output;
        }

        public static double[,] MirrorHorizontally(double[,] input)
        {
            int rowCount = input.GetLength(0);
            int colCount = input.GetLength(1);


            var output = new double[rowCount, colCount];

            for (var col = 0; col < colCount; col++)
            {
                for (int row = rowCount - 1, outRow = 0; row >= 0; row--, outRow++)
                {
                    output[outRow, col] = input[row, col];
                }
            }

            return output;
        }
    }
}