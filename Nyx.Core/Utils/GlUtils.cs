using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Utils
{
    public class GlUtils
    {
        private static readonly ILogger<GlUtils> Logger = SerilogLogger.Factory.CreateLogger<GlUtils>();

        public static void CheckError(string title)
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Logger.LogDebug($"{title}: {error}");
            }
        }
    }
}