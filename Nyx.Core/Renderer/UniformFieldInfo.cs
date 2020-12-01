using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Renderer
{
    public struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }
}