using Silk.NET.OpenGL;
using Silk.NET.Windowing.Common;

namespace Nyx.Core.Renderer
{
    public class GraphicsContext
    {
        public static GL Gl;

        public static void Create(IWindow window)
        {
            Gl = GL.GetApi(window);
        }

        public static unsafe void DrawElements(PrimitiveType type, uint count)
        {
            Gl.DrawElements(type, count, DrawElementsType.UnsignedInt,
                null);
        }
    }
}