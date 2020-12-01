using System;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Renderer
{
    public class Texture2D : Texture
    {
        public Texture2D(TextureType textureType, string path) : base(path)
        {
            TextureType = textureType;
        }

        public Texture2D(int width, int height) : base(width, height)
        {
            TextureType = TextureType.PixelSprite;
        }

        public TextureType TextureType { get; }

        public override TextureTarget TextureTarget => TextureTarget.Texture2D;

        protected override void SetupTextureParameters()
        {
            switch (TextureType)
            {
                case TextureType.NormalSprite:
                    SetWrapMode(TextureWrapMode.ClampToEdge);
                    SetFilter(TextureMinFilter.Linear, TextureMagFilter.Linear);
                    break;
                case TextureType.PixelSprite:
                    SetWrapMode(TextureWrapMode.Repeat);
                    SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
                    break;
                default:
                    throw new InvalidOperationException($"{TextureType} is unknown");
            }
        }
    }
}