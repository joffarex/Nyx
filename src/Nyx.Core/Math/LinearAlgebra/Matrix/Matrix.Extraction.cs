using System.Linq;

namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        #region Extraction

        /// <summary>
        ///     Returns a new matrix consists of the specified column range.
        /// </summary>
        public virtual Matrix ExtractColumns(int startCol, int endCol)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.ExtractColumns(InnerMatrix, startCol, endCol));
        }

        /// <summary>
        ///     Returns a new matrix consists of the specified row range.
        /// </summary>
        public virtual Matrix ExtractRows(int startRow, int endRow)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.ExtractRows(InnerMatrix, startRow, endRow));
        }

        /// <summary>
        ///     Returns a new matrix consists of the specified column indexes.
        /// </summary>
        public virtual Matrix ExtractColumns(int[] cols)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.ExtractColumns(InnerMatrix, cols));
        }

        /// <summary>
        ///     Returns a new matrix consists of the specified row indexes.
        /// </summary>
        public virtual Matrix ExtractRows(int[] rows)
        {
            return new Matrix(MatrixFunctions.MatrixFunctions.ExtractRows(InnerMatrix, rows));
        }

        #endregion


        #region Column and Row Vectors

        /// <summary>
        ///     Returns an array of column vectors of current matrix.
        /// </summary>
        public virtual Vector.Vector[] GetColumnVectors()
        {
            return MatrixFunctions.MatrixFunctions.GetColumnVectors(InnerMatrix).Select(s => new Vector.Vector(s))
                .ToArray();
        }

        /// <summary>
        ///     Returns an array of row vectors of current matrix.
        /// </summary>
        public virtual Vector.Vector[] GetRowVectors()
        {
            return MatrixFunctions.MatrixFunctions.GetRowVectors(InnerMatrix).Select(s => new Vector.Vector(s))
                .ToArray();
        }

        #endregion
    }
}