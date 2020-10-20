namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Creates a subspace projection matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix CreateProjectionMatrix()
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.CreateProjectionMatrix(InnerMatrix));
        }

        /// <summary>
        ///     Projects specified vector onto the current matrix subspace.
        /// </summary>
        public Vector.Vector Project(double[] vector)
        {
            return Project(new Vector.Vector(vector));
        }

        /// <summary>
        ///     Projects specified vector onto the current matrix subspace.
        /// </summary>
        public Vector.Vector Project(Vector.Vector vector)
        {
            Matrix projectionMatrix = CreateProjectionMatrix();
            Matrix vectorMatrix = vector.AsMatrix();

            return projectionMatrix.Multiply(vectorMatrix).GetColumnVectors()[0];
        }
    }
}