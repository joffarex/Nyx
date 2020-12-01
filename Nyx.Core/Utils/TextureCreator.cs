using System;
using Nyx.Core.Renderer;

namespace Nyx.Core.Utils
{
    public static class TextureCreator
    {
        public static T Validate<T>(TextureType textureType, string path, Func<TextureType, string, T> factory)
            where T : Texture
        {
            return factory(textureType, path);
        }
    }
}