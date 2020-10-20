using System;

namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        public static double AngleBetween(double[] vector1, double[] vector2, AngleUnit unit)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new InvalidOperationException("Dimensions mismatch.");
            }

            double dotProduct = DotProduct(vector1, vector2);
            double lengthProduct = GetMagnitude(vector1) * GetMagnitude(vector2);

            double result = System.Math.Acos(dotProduct / lengthProduct);
            if (unit == AngleUnit.Degrees)
            {
                result = Converters.RadiansToDegrees(result);
            }

            return result;
        }
    }
}