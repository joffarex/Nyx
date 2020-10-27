using System;

namespace Nyx.Utils
{
    public static class Fps
    {
        public static float Get(float deltaTime)
        {
            return 1.0f / deltaTime;
        }

        public static void Print(float deltaTime)
        {
            Console.WriteLine($"{Get(deltaTime)}");
        }
    }
}