using System;

namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        public static double[] GenerateRandomVector(int dimension, double maxValue = 1)
        {
            var rand = new Random();
            var output = new double[dimension];

            for (var i = 0; i < dimension; i++)
            {
                output[i] = rand.NextDouble() * maxValue;
            }

            return output;
        }

        public static string ToString(double[] input)
        {
            var str = string.Empty;

            for (var i = 0; i < input.GetLength(0); i++)
            {
                str += input[i] + ", ";
            }

            str = str.TrimEnd(',', ' ');

            return str;
        }
    }
}