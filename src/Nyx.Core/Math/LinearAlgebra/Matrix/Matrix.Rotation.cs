﻿using System;
using Nyx.Core.Math.LinearAlgebra.MatrixHelpers;

namespace Nyx.Core.Math.LinearAlgebra.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        ///     Rotates a 2D matrix to a specific angle in a spcific direction (clockwise / counter-clockwise.)
        /// </summary>
        public virtual Matrix Rotate(double angle, AngleUnit unit, MatrixRotationDirection direction)
        {
            if (Is2DMatrix == false)
            {
                throw new InvalidOperationException("2-dimensional required.");
            }

            return new Matrix(MatrixFunctions.MatrixFunctions.Create2DRotationMatrix(angle, unit, direction));
        }

        /// <summary>
        ///     Rotates a 3D matrix to a specific angle in a given axis.
        /// </summary>
        public virtual Matrix Rotate(double angle, AngleUnit unit, MatrixAxis axis)
        {
            if (Is3DMatrix == false)
            {
                throw new InvalidOperationException("3-dimensional required.");
            }

            return new Matrix(MatrixFunctions.MatrixFunctions.Create3DRotationMatrix(angle, unit, axis));
        }


        /// <summary>
        ///     Mirrors matrix entries horizontally / vertically.
        /// </summary>
        public virtual Matrix Mirror(MatrixDirection direction)
        {
            if (direction == MatrixDirection.Horizontal)
            {
                return new Matrix(MatrixFunctions.MatrixFunctions.MirrorHorizontally(InnerMatrix));
            }

            return new Matrix(MatrixFunctions.MatrixFunctions.MirrorVertically(InnerMatrix));
        }
    }
}