using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Gui
{
    internal struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }
}