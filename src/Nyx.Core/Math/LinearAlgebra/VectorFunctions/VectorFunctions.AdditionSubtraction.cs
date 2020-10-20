﻿using System;

namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        public static double[] Add(double[] vector1, double[] vector2)
        {
            return InternalAdd(vector1, vector2, 1);
        }

        public static double[] Subtract(double[] vector1, double[] vector2)
        {
            return InternalAdd(vector1, vector2, -1);
        }

        private static double[] InternalAdd(double[] vector1, double[] vector2, double coeffecient)
        {
            int length = vector1.GetLength(0);

            if (length != vector2.Length)
                throw new InvalidOperationException("Dimensions mismatch.");

            var output = new double[length];

            for (var i = 0; i < length; i++)
            {
                output[i] = vector1[i] + (coeffecient * vector2[i]);
            }

            return output;
        }
    }
}