using Nyx.Core.Math.LinearAlgebra.MatrixHelpers;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        ///     Creates 2-dimensional rotation matrix using the specified angle.
        /// </summary>
        public static double[,] Create2DRotationMatrix(double angle, AngleUnit unit, MatrixRotationDirection direction)
        {
            // sin and cos accept only radians

            double angleRadians = angle;
            if (unit == AngleUnit.Degrees)
            {
                angleRadians = Converters.DegreesToRadians(angleRadians);
            }


            var output = new double[2, 2];

            output[0, 0] = System.Math.Cos(angleRadians);
            output[1, 0] = System.Math.Sin(angleRadians);
            output[0, 1] = System.Math.Sin(angleRadians);
            output[1, 1] = System.Math.Cos(angleRadians);

            if (direction == MatrixRotationDirection.Clockwise)
            {
                output[1, 0] *= -1;
            }
            else
            {
                output[0, 1] *= -1;
            }


            return output;
        }

        /// <summary>
        ///     Creates 2-dimensional rotation matrix using the specified angle and axis.
        /// </summary>
        public static double[,] Create3DRotationMatrix(double angle, AngleUnit unit, MatrixAxis axis)
        {
            // sin and cos accept only radians

            double angleRadians = angle;
            if (unit == AngleUnit.Degrees)
            {
                angleRadians = Converters.DegreesToRadians(angleRadians);
            }


            var output = new double[3, 3];

            if (axis == MatrixAxis.X)
            {
                output[0, 0] = 1;
                output[1, 1] = System.Math.Cos(angleRadians);
                output[2, 1] = System.Math.Sin(angleRadians);
                output[1, 2] = -1 * System.Math.Sin(angleRadians);
                output[2, 2] = System.Math.Cos(angleRadians);
            }
            else if (axis == MatrixAxis.Y)
            {
                output[1, 1] = 1;
                output[0, 0] = System.Math.Cos(angleRadians);
                output[2, 0] = -1 * System.Math.Sin(angleRadians);
                output[0, 2] = System.Math.Sin(angleRadians);
                output[2, 2] = System.Math.Cos(angleRadians);
            }
            else if (axis == MatrixAxis.Z)
            {
                output[2, 2] = 1;
                output[0, 0] = System.Math.Cos(angleRadians);
                output[1, 0] = System.Math.Sin(angleRadians);
                output[0, 1] = -1 * System.Math.Sin(angleRadians);
                output[1, 1] = System.Math.Cos(angleRadians);
            }


            return output;
        }
    }
}