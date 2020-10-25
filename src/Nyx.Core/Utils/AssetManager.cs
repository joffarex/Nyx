using System;
using System.Collections.Generic;
using Nyx.Core.Renderer;

namespace Nyx.Core.Utils
{
    public class AssetManager
    {
        private static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
        private static readonly Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

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

        public static Texture GetTexture(string resourceName)
        {
            string filePath = PathUtils.GetFullPath(resourceName);
            if (Textures.ContainsKey(filePath))
            {
                if (Textures.TryGetValue(filePath, out Texture texture))
                {
                    return texture;
                }

                throw new Exception($"FilePath: ${filePath} not found");
            }
            else
            {
                var texture = new Texture(TextureType.PixelSprite, filePath);
                Textures.Add(filePath, texture);
                return texture;
            }
        }
    }
}