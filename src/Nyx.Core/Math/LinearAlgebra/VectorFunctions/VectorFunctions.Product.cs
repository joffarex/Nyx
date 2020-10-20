using System;

namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        public static double DotProduct(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new InvalidOperationException("Dimensions mismatch.");
            }

            double product = 0;
            for (var i = 0; i < vector1.Length; i++)
            {
                product += vector1[i] * vector2[i];
            }

            return product;
        }

        public static double[] Scale(double scalar, double[] vector)
        {
            var product = new double[vector.Length];

            for (var i = 0; i < vector.Length; i++)
            {
                product[i] = scalar * vector[i];
            }

            return product;
        }


        public static double[] CrossProduct(double[] vector1, double[] vector2)
        {
            var length = 3;

            if ((length != vector1.Length) || (length != vector2.Length))
            {
                throw new InvalidOperationException("3-dimensional required.");
            }

            var matrix = new double[length, length];
            for (var row = 0; row < length; row++)
            {
                for (var col = 0; col < length; col++)
                {
                    if (row == 0)
                    {
                        matrix[row, col] = 1;
                    }
                    else if (row == 1)
                    {
                        matrix[row, col] = vector1[col];
                    }
                    else if (row == 2)
                    {
                        matrix[row, col] = vector2[col];
                    }
                }
            }


            return MatrixFunctions.MatrixFunctions.CrossProduct(matrix);
        }
    }
}