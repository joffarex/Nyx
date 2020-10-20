namespace Nyx.Core.Math
{
    public static class Converters
    {
        public static double DegreesToRadians(double degree)
        {
            return (degree * System.Math.PI) / 180;
        }

        public static double RadiansToDegrees(double radians)
        {
            return (radians / System.Math.PI) * 180;
        }
    }
}