using System;

namespace NyxEngine.Utils
{
    public static class Logger
    {
        public static void Log(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[MSG]: {msg}");
            NormalizeMessage();
        }
        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[INFO]: {msg}");
            NormalizeMessage();
        }
        public static void Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[MSG]: {msg}");
            NormalizeMessage();
        }
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[MSG]: {msg}");
            NormalizeMessage();
        }

        private static void NormalizeMessage()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}