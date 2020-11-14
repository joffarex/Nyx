using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;

namespace Nyx.Core.Utils
{
    public class Fps
    {
        private static readonly ILogger<Fps> Logger = SerilogLogger.Factory.CreateLogger<Fps>();

        public static float Get(float deltaTime)
        {
            return 1.0f / deltaTime;
        }

        public static string Get(double deltaTime)
        {
            return Math.Round(1.0 / deltaTime, 2).ToString(CultureInfo.InvariantCulture);
        }

        public static void Print(double deltaTime)
        {
            Logger.LogInformation(Get(deltaTime));
        }

        public static void Print(float deltaTime)
        {
            Console.WriteLine($"{Get(deltaTime)}");
        }
    }
}