using System;
using Nyx.Core.Math.LinearAlgebra.MatrixFunctions.Helpers;
using Nyx.Core.Math.LinearAlgebra.MatrixHelpers;

namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Reduces matrix to row-echelon (REF/Gauss) or reduced row-echelon (RREF/Gauss-Jordan) form.
        ///     Accepts the number of augmeted columns. If the number specified is null, the default number specified in the matrix
        ///     is used.
        /// </summary>
        /// <remarks>
        ///     If
        ///     <param name="augmentedColCount">augmentedColCount</param>
        ///     is null, <seealso cref="AugmentedColumnCount" /> is used.
        /// </remarks>
        public virtual Matrix Reduce(MatrixReductionForm form, int? augmentedColCount = null)
        {
            int augmentedCols = augmentedColCount ?? AugmentedColumnCount;

            return new Matrix(MatrixFunctions.MatrixFunctions.Eliminate(InnerMatrix, form, augmentedCols).FullMatrix);
        }


        /// <summary>
        ///     Reduces matrix to reduced row-echelon (RREF/Gauss-Jordan) form and solves for augmented columns.
        ///     Returns the matrix solution and outputs the full matrix (reduced matrix along with the solution.
        ///     Accepts the number of augmeted columns. If the number specified is null, the default number specified in the matrix
        ///     is used.
        /// </summary>
        /// <returns>
        ///     Returns the matrix solution result.
        /// </returns>
        /// <remarks>
        ///     The default value for <seealso cref="AugmentedColumnCount" /> is used.
        /// </remarks>
        public virtual MatrixEliminationResult Solve(int? augmentedColCount)
        {
            int augmentedCols = augmentedColCount ?? AugmentedColumnCount;

            if (augmentedCols <= 0)
            {
                throw new InvalidOperationException("No augmented columns found.");
            }

            return MatrixFunctions.MatrixFunctions.Eliminate(InnerMatrix, MatrixReductionForm.ReducedRowEchelonForm,
                augmentedCols);
        }

        /// <summary>
        ///     Returns matrix rank.
        ///     Accepts the number of augmeted columns. If the number specified is null, the default number specified in the matrix
        ///     is used.
        /// </summary>
        /// <remarks>
        ///     If
        ///     <param name="augmentedColCount">augmentedColCount</param>
        ///     is null, <seealso cref="AugmentedColumnCount" /> is used.
        /// </remarks>
        public virtual int GetRank(int? augmentedColCount = null)
        {
            int augmentedCols = augmentedColCount ?? AugmentedColumnCount;
            return MatrixFunctions.MatrixFunctions.GetRank(InnerMatrix, augmentedCols);
        }
    }
}