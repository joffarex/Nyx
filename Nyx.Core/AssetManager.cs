using System;
using System.Collections.Generic;
using Nyx.Core.Common;
using Nyx.Core.Utils;

namespace Nyx.Core
{
    public static class AssetManager
    {
        private static readonly Dictionary<string, Shader> Shaders = new();
        private static readonly Dictionary<string, Texture> Textures = new();

        public static Shader GetShader(string resourceName)
        {
            string filePath = PathUtils.GetFullPath(resourceName);

            if (Shaders.TryGetValue(filePath, out Shader shader))
            {
                return shader;
            }

            shader = new Shader(filePath);
            Shaders.Add(filePath, shader);
            return shader;
        }

        public static T GetTexture<T>(string resourceName, Func<TextureType, string, T> factory) where T : Texture
        {
            string filePath = PathUtils.GetFullPath(resourceName);

            if (Textures.TryGetValue(filePath, out Texture texture))
            {
                return (T) texture;
            }

            texture = TextureCreator.Validate(TextureType.PixelSprite, filePath, factory);
            Textures.Add(filePath, texture);
            return (T) texture;
        }
    }
}