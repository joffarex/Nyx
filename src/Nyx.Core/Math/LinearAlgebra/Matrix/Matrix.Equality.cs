namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Returns true if both matrices contain the same entry values.
        /// </summary>
        public bool Equals(Matrix other)
        {
            if (other == null)
            {
                return false;
            }

            return MatrixFunctions.MatrixFunctions.Equals(InnerMatrix, other.InnerMatrix);
        }

        /// <summary>
        ///     Returns true if both matrices contain the same entry values.
        /// </summary>
        public bool Equals(double[,] other)
        {
            if (other == null)
            {
                return false;
            }

            return MatrixFunctions.MatrixFunctions.Equals(InnerMatrix, other);
        }

        /// <summary>
        ///     Returns true if both matrices contain the same entry values.
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix);
        }

        public override int GetHashCode()
        {
            return InnerMatrix.GetHashCode();
        }


        /// <summary>
        ///     Returns True if both matrices have the same dimensions.
        /// </summary>
        public bool IsSameDimensions(Matrix other)
        {
            return (RowCount == other.RowCount) && (ColumnCount == other.ColumnCount);
        }

        /// <summary>
        ///     Returns True if both matrices have the same dimensions.
        /// </summary>
        public bool IsSameDimensions(double[,] other)
        {
            return (RowCount == other.GetLength(0)) && (ColumnCount == other.GetLength(1));
        }

        /// <summary>
        ///     Returns True of the transpose of the other matrix has the same dimensions as this matrix.
        /// </summary>
        public bool IsSameTransposeDimensions(Matrix other)
        {
            return (RowCount == other.ColumnCount) && (ColumnCount == other.RowCount);
        }

        /// <summary>
        ///     Returns True of the transpose of the other matrix has the same dimensions as this matrix.
        /// </summary>
        public bool IsSameTransposeDimensions(double[,] other)
        {
            return (RowCount == other.GetLength(1)) && (ColumnCount == other.GetLength(0));
        }
    }
}