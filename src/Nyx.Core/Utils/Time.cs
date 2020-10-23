using Silk.NET.GLFW;

namespace Nyx.Core.Utils
{
    public class Time
    {
        public static float GetTimeFromAppicationStart()
        {
            return (float) Glfw.GetApi().GetTime();
        }
    }
}