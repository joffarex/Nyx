using System;
using System.Collections.Generic;
using System.Linq;
using Nyx.Core.Components;
using Nyx.Core.Renderer;
using Nyx.Utils;

namespace Nyx.Core
{
    public static class AssetManager
    {
        private static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
        private static readonly Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();
        private static readonly Dictionary<string, SpriteSheet> SpriteSheets = new Dictionary<string, SpriteSheet>();

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

            if (Textures.TryGetValue(filePath, out Texture texture))
            {
                return texture;
            }

            texture = new Texture(TextureType.PixelSprite, filePath);
            Textures.Add(filePath, texture);
            return texture;
        }

        public static void AddSpriteSheet(string resourceName, SpriteSheet spriteSheet)
        {
            string filePath = PathUtils.GetFullPath(resourceName);


            if (!SpriteSheets.ContainsKey(filePath))
            {
                SpriteSheets.Add(filePath, spriteSheet);
            }
        }

        public static SpriteSheet GetSpriteSheet(string resourceName)
        {
            string filePath = PathUtils.GetFullPath(resourceName);

            SpriteSheet spriteSheet = SpriteSheets
                .FirstOrDefault(x => x.Key.Equals(filePath))
                .Value;

#if DEBUG
            if (spriteSheet == null)
            {
                throw new Exception($"SpriteSheet: ${resourceName} has not been added to manager");
            }
#endif
            return spriteSheet;
        }
    }
}