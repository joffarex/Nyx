using System;
using Nyx.Core.Math.LinearAlgebra.MatrixHelpers;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        public static double[,] Create2DReflectionMatrix(MatrixAxis axis)
        {
            if (axis == MatrixAxis.Z)
            {
                throw new InvalidOperationException("Invalid axis.");
            }

            double[,] output = CreateIdentityMatrix(2);

            if (axis == MatrixAxis.X)
            {
                output[1, 1] *= -1;
            }
            else if (axis == MatrixAxis.Y)
            {
                output[0, 0] *= -1;
            }

            return output;
        }

        public static double[,] Create3DReflectionMatrix(Matrix3DReflectionPlane axis)
        {
            double[,] output = CreateIdentityMatrix(4);

            if (axis == Matrix3DReflectionPlane.XY)
            {
                output[2, 2] *= -1;
            }

            else if (axis == Matrix3DReflectionPlane.YZ)
            {
                output[0, 0] *= -1;
            }

            else if (axis == Matrix3DReflectionPlane.ZX)
            {
                output[1, 1] *= -1;
            }

            return output;
        }
    }
}