using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        #region Fields

        private double[] _innerArray;

        #endregion


        public Vector(int dimension)
        {
            InnerArray = new double[dimension];
        }

        public Vector(double[] vector)
        {
            InnerArray = vector;
        }

        #region Indexer

        public double this[int pos]
        {
            get => InnerArray[pos];
            set => InnerArray[pos] = value;
        }

        #endregion

        #region Helpers

        public override string ToString()
        {
            return VectorFunctions.VectorFunctions.ToString(InnerArray);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Returns vector dimension / length.
        /// </summary>
        public int Dimension { get; protected set; }

        /// <summary>
        ///     Returns inner array representation of vector.
        /// </summary>
        public double[] InnerArray
        {
            get => _innerArray;
            protected set
            {
                _innerArray = value;
                Dimension = value.Length;
            }
        }

        #endregion
    }
}