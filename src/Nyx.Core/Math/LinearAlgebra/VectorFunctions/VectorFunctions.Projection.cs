namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        public static double ProjectionFactor(double[] vector1, double[] vector2)
        {
            return DotProduct(vector1, vector2) / DotProduct(vector2, vector2);
        }

        public static double[] Projection(double[] vector1, double[] vector2)
        {
            double factor = ProjectionFactor(vector1, vector2);
            return Scale(factor, vector2);
        }
    }
}