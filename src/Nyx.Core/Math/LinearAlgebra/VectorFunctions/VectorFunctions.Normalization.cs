namespace Nyx.Core.Math.LinearAlgebra.VectorFunctions
{
    internal static partial class VectorFunctions
    {
        /// <summary>
        ///     Normalizes a vector.
        /// </summary>
        public static double[] ToUnitVector(double[] input)
        {
            int length = input.Length;

            var output = new double[length];
            double coeffecient = 1.0 / GetMagnitude(input);

            for (var i = 0; i < length; i++)
            {
                output[i] = coeffecient * input[i];
            }

            return output;
        }


        public static double GetMagnitude(double[] input)
        {
            double val = 0;

            for (var i = 0; i < input.Length; i++)
            {
                val += input[i] * input[i];
            }

            val = System.Math.Sqrt(val);

            return val;
        }
    }
}