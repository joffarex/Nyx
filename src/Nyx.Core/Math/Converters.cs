namespace Nyx.Core.Math
{
    public static class Converters
    {
        public static float DegreesToRadians(float degree)
        {
            return (float) (degree * System.Math.PI) / 180;
        }

        public static float RadiansToDegrees(float radians)
        {
            return (float) (radians / System.Math.PI) * 180;
        }
    }
}