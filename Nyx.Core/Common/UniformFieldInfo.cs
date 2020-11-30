using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Common
{
    public struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }
}