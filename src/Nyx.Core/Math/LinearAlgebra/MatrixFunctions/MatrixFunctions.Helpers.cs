using System;
using System.Linq;

namespace Nyx.Core.Math.LinearAlgebra.MatrixFunctions
{
    internal static partial class MatrixFunctions
    {
        internal static double[,] FromString(string str)
        {
            string[] lines = str.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            var output = new double[lines.Length, lines[0].Split(',').Count()];

            for (var r = 0; r < lines.Length; r++)
            {
                string[] split = lines[r].Split(',');

                for (var c = 0; c < split.Length; c++)
                {
                    string val = split[c].Trim();

                    output[r, c] = double.Parse(val);
                }
            }

            return output;
        }


        internal static string ToString(double[,] matrix, int augmentedCols = 0)
        {
            var str = string.Empty;
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);
            int augmentedColsStartIndex = matrix.GetLength(1) - augmentedCols;

            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    str += matrix[row, col].ToString();

                    if (col == (augmentedColsStartIndex - 1))
                    {
                        str += " | ";
                    }
                    else
                    {
                        str += ", ";
                    }
                }

                str = str.TrimEnd(',', ' ', '|');

                if ((row + 1) != rowCount)
                {
                    str += Environment.NewLine;
                }
            }

            return str;
        }
    }
}