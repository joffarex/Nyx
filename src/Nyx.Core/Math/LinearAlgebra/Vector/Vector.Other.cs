using System;

namespace Nyx.Core.Math.LinearAlgebra.Vector
{
    public partial class Vector : ICloneable, IEquatable<Vector>
    {
        /// <summary>
        ///     Rounds vector entries to the nearest integeral value.
        /// </summary>
        public virtual Vector Round(int decimals)
        {
            return new Vector(VectorFunctions.VectorFunctions.Round(InnerArray, decimals));
        }
    }
}