using System;
using System.Globalization;
using ImGuiNET;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;

namespace Nyx.Core.Utils
{
    public class Fps
    {
        private static readonly ILogger<Fps> Logger = SerilogLogger.Factory.CreateLogger<Fps>();

        private static readonly double _fpsDisplayDelay = 1;
        private static double _timeBetweenFpsDisplay;

        private static string _currentFps;

        public static void ImGuiWindow(ref double deltaTime)
        {
            _timeBetweenFpsDisplay -= deltaTime;

            if (_timeBetweenFpsDisplay <= 0)
            {
                _timeBetweenFpsDisplay = _fpsDisplayDelay;
                _currentFps = Get(ref deltaTime);
            }

            ImGui.Begin("Benchmarks");
            ImGui.SetWindowFontScale(1.2f);
            ImGui.Text($"FPS: {_currentFps}");
            ImGui.End();
        }

        public static float Get(float deltaTime)
        {
            return 1.0f / deltaTime;
        }

        public static string Get(ref double deltaTime)
        {
            return Math.Round(1.0 / deltaTime, 2).ToString(CultureInfo.InvariantCulture);
        }

        public static void Print(ref double deltaTime)
        {
            Logger.LogInformation(Get(ref deltaTime));
        }

        public static void Print(float deltaTime)
        {
            Console.WriteLine($"{Get(deltaTime)}");
        }
    }
}