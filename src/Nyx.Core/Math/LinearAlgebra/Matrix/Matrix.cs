using System;

namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix : ICloneable, IEquatable<Matrix>
    {
        #region Fields

        private double[,] _innerMatrix;

        #endregion

        /// <summary>
        ///     Creates a square n-dimensional zero matrix.
        /// </summary>
        public Matrix(int length) : this(length, length)
        {
        }

        /// <summary>
        ///     Creates a n by m zero matrix.
        /// </summary>
        public Matrix(int rows, int cols)
        {
            InnerMatrix = new double[rows, cols];
        }

        /// <summary>
        ///     Creates a matrix from the specified raw array. Sets augmented columns (if any.)
        /// </summary>
        public Matrix(double[,] matrix, int augmentedCols = 0)
        {
            InnerMatrix = matrix;
            SetAugmentedColumnCount(augmentedCols);
        }

        #region Indexers

        /// <summary>
        ///     Sets/returns the required entry.
        /// </summary>
        public double this[int r, int c]
        {
            get => InnerMatrix[r, c];
            set => InnerMatrix[r, c] = value;
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Outputs matrix in a string format.
        /// </summary>
        public override string ToString()
        {
            return MatrixFunctions.MatrixFunctions.ToString(InnerMatrix, AugmentedColumnCount);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Total row count.
        /// </summary>
        public int RowCount { get; protected set; }

        /// <summary>
        ///     Total column count including augmented columns (if any.)
        /// </summary>
        public int ColumnCount { get; protected set; }

        /// <summary>
        ///     Returns True if matrix is a square matrix.
        /// </summary>
        public bool IsSquare => RowCount == ColumnCount;

        /// <summary>
        ///     Returns True if matrix is a 2-dimensional matrix.
        /// </summary>
        public bool Is2DMatrix => RowCount == 2;

        /// <summary>
        ///     Returns True if matrix is a 2-dimensional matrix.
        /// </summary>
        public bool Is3DMatrix => RowCount == 3;

        /// <summary>
        ///     Default number of augmented columns (if any.) Used only in matrix reduction and elimination functions.
        /// </summary>
        public int AugmentedColumnCount { get; set; }

        /// <summary>
        ///     Inner raw array.
        /// </summary>
        public double[,] InnerMatrix
        {
            get => _innerMatrix;
            protected set
            {
                _innerMatrix = value;
                RefreshLengths();
            }
        }

        #endregion

        #region Other

        /// <summary>
        ///     Sets augmented column count. Augmented columns value is used only in matrix reduction/elimination functions.
        /// </summary>
        public virtual void SetAugmentedColumnCount(int cols)
        {
            if (cols >= ColumnCount)
            {
                throw new ArgumentException(nameof(cols), "Augmented columns must be less than total columns.");
            }

            AugmentedColumnCount = cols;
        }

        /// <summary>
        ///     Refreshes <seealso cref="Matrix.RowCount" /> and <seealso cref="Matrix.ColumnCount" />.
        /// </summary>
        protected void RefreshLengths()
        {
            RowCount = InnerMatrix.GetLength(0);
            ColumnCount = InnerMatrix.GetLength(1);
        }

        #endregion
    }
}