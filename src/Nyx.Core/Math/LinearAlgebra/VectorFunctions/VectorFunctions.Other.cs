using System;

namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        public static double[,] ToMatrix(double[] input)
        {
            var output = new double[input.Length, 1];

            for (var i = 0; i < input.Length; i++)
            {
                output[i, 0] = input[i];
            }

            return output;
        }


        internal static double[] Round(double[] input, int decimals)
        {
            int length = input.Length;
            var output = new double[length];

            for (var i = 0; i < length; i++)
            {
                output[i] = System.Math.Round(input[i], decimals);
            }

            return output;
        }

        public static double[] ToVector(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);

            if (false == ((rowCount == 1) || (colCount == 1)))
            {
                throw new InvalidOperationException("Invalid matrix.");
            }

            int length = rowCount == 1 ? colCount : rowCount;
            var output = new double[length];
            for (var i = 0; i < length; i++)
            {
                output[i] = rowCount == 1 ? matrix[0, i] : matrix[i, 0];
            }

            return output;
        }
    }
}