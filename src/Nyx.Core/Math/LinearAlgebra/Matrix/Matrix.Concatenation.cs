using System;
using System.Linq;
using Nyx.Core.Math.LinearAlgebra.MatrixHelpers;

namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        #region Concatenation

        /// <summary>
        ///     Concats another matrix horizontally / vertically.
        /// </summary>
        public virtual Matrix Concat(Matrix matrix, MatrixDirection direction)
        {
            return Concat(matrix.InnerMatrix, direction);
        }

        /// <summary>
        ///     Concats another matrix horizontally / vertically.
        /// </summary>
        public virtual Matrix Concat(double[,] matrix, MatrixDirection direction)
        {
            if (direction == MatrixDirection.Horizontal)
            {
                return new Matrix(MatrixFunctions.MatrixFunctions.ConcatHorizontally(InnerMatrix, matrix));
            }

            return new Matrix(MatrixFunctions.MatrixFunctions.ConcatVertically(InnerMatrix, matrix));
        }

        #endregion

        #region Expanding

        /// <summary>
        ///     Expands matrix by the specified number of columns (at the start / at the end.)
        /// </summary>
        public virtual Matrix ExpandColumns(int cols, MatrixPosition pos)
        {
            if (cols < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cols));
            }

            var newMatrix = new double[RowCount, cols];

            if (pos == MatrixPosition.Start)
            {
                return new Matrix(newMatrix).Concat(this, MatrixDirection.Horizontal);
            }

            return Concat(newMatrix, MatrixDirection.Horizontal);
        }

        /// <summary>
        ///     Expands matrix by the specified number of rows (at the start / at the end.)
        /// </summary>
        public virtual Matrix ExpandRows(int rows, MatrixPosition pos)
        {
            if (rows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows));
            }

            var newMatrix = new double[rows, ColumnCount];
            if (pos == MatrixPosition.Start)
            {
                return new Matrix(newMatrix).Concat(this, MatrixDirection.Vertical);
            }

            return Concat(newMatrix, MatrixDirection.Vertical);
        }

        #endregion

        #region Shrinking

        /// <summary>
        ///     Shrinks matrix by number of columns (from the start / from the end.)
        /// </summary>
        public virtual Matrix ShrinkColumns(int colsToShrink, MatrixPosition pos)
        {
            int[] cols = null;
            if (pos == MatrixPosition.Start)
            {
                cols = Enumerable.Range(0, colsToShrink).ToArray();
            }
            else
            {
                cols = Enumerable.Range(ColumnCount - colsToShrink, colsToShrink).ToArray();
            }

            return new Matrix(MatrixFunctions.MatrixFunctions.RemoveColumns(InnerMatrix, cols));
        }

        /// <summary>
        ///     Shrinks matrix by number of rows (from the start / from the end.)
        /// </summary>
        public virtual Matrix ShrinkRows(int rowsToShrink, MatrixPosition pos)
        {
            int[] rows = null;
            if (pos == MatrixPosition.Start)
            {
                rows = Enumerable.Range(0, rowsToShrink).ToArray();
            }
            else
            {
                rows = Enumerable.Range(RowCount - rowsToShrink, rowsToShrink).ToArray();
            }

            return new Matrix(MatrixFunctions.MatrixFunctions.RemoveRows(InnerMatrix, rows));
        }

        #endregion

        #region Removing

        /// <summary>
        ///     Removes specific columns.
        /// </summary>
        public virtual Matrix RemoveColumns(int[] cols)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.RemoveColumns(InnerMatrix, cols));
        }

        /// <summary>
        ///     Removes specific rows.
        /// </summary>
        public virtual Matrix RemoveRows(int[] rows)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.RemoveRows(InnerMatrix, rows));
        }

        #endregion
    }
}