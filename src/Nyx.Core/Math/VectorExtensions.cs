using System.Numerics;

namespace Nyx.Core.Math
{
    public static class VectorExtensions
    {
        public static Vector4 Multiply(this Vector4 vector, Matrix4x4 matrix)
        {
            float x = vector.X;
            float y = vector.Y;
            float z = vector.Z;
            float w = vector.W;

            vector.X = NyxMath.Fma(matrix.M11, x,
                NyxMath.Fma(matrix.M21, y, NyxMath.Fma(matrix.M31, z, matrix.M41 * w)));
            vector.Y = NyxMath.Fma(matrix.M12, x,
                NyxMath.Fma(matrix.M22, y, NyxMath.Fma(matrix.M32, z, matrix.M42 * w)));
            vector.Z = NyxMath.Fma(matrix.M13, x,
                NyxMath.Fma(matrix.M23, y, NyxMath.Fma(matrix.M33, z, matrix.M43 * w)));
            vector.W = NyxMath.Fma(matrix.M14, x,
                NyxMath.Fma(matrix.M24, y, NyxMath.Fma(matrix.M34, z, matrix.M44 * w)));

            return vector;
        }
    }
}