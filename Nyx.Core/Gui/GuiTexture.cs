using System;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace Nyx.Core.Gui
{
    public enum TextureCoordinate
    {
        S = TextureParameterName.TextureWrapS,
        T = TextureParameterName.TextureWrapT,
        R = TextureParameterName.TextureWrapR
    }

    internal class GuiTexture : IDisposable
    {
        private const SizedInternalFormat Srgb8Alpha8 = (SizedInternalFormat)All.Srgb8Alpha8;
        public const SizedInternalFormat Rgb32F = (SizedInternalFormat)All.Rgb32f;

        private const GetPName MaxTextureMaxAnisotropy = (GetPName)0x84FF;

        private static readonly float MaxAniso;

        static GuiTexture()
        {
            MaxAniso = GL.GetFloat(MaxTextureMaxAnisotropy);
        }

        public readonly int GlTexture;

        public GuiTexture(string name, int width, int height, IntPtr data, bool generateMipmaps = false, bool srgb = false)
        {
            int width1 = width;
            int height1 = height;
            SizedInternalFormat internalFormat = srgb ? Srgb8Alpha8 : SizedInternalFormat.Rgba8;
            int mipmapLevels = generateMipmaps == false ? 1 : (int)Math.Floor(Math.Log(Math.Max(width1, height1), 2));

            Utils.CreateTexture(TextureTarget.Texture2D, name, out GlTexture);
            GL.TextureStorage2D(GlTexture, mipmapLevels, internalFormat, width1, height1);

            GL.TextureSubImage2D(GlTexture, 0, 0, 0, width1, height1, PixelFormat.Bgra, PixelType.UnsignedByte, data);

            if (generateMipmaps) GL.GenerateTextureMipmap(GlTexture);

            SetWrap(TextureCoordinate.S, TextureWrapMode.Repeat);
            SetWrap(TextureCoordinate.T, TextureWrapMode.Repeat);

            GL.TextureParameter(GlTexture, TextureParameterName.TextureMaxLevel, mipmapLevels - 1);
        }

        public void SetMinFilter(TextureMinFilter filter)
        {
            GL.TextureParameter(GlTexture, TextureParameterName.TextureMinFilter, (int)filter);
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            GL.TextureParameter(GlTexture, TextureParameterName.TextureMagFilter, (int)filter);
        }

        public void SetAnisotropy(float level)
        {
            const TextureParameterName textureMaxAnisotropy = (TextureParameterName)0x84FE;
            GL.TextureParameter(GlTexture, textureMaxAnisotropy, Utils.Clamp(level, 1, MaxAniso));
        }

        public void SetLod(int @base, int min, int max)
        {
            GL.TextureParameter(GlTexture, TextureParameterName.TextureLodBias, @base);
            GL.TextureParameter(GlTexture, TextureParameterName.TextureMinLod, min);
            GL.TextureParameter(GlTexture, TextureParameterName.TextureMaxLod, max);
        }

        private void SetWrap(TextureCoordinate coord, TextureWrapMode mode)
        {
            GL.TextureParameter(GlTexture, (TextureParameterName)coord, (int)mode);
        }

        public void Dispose()
        {
            GL.DeleteTexture(GlTexture);
        }
    }
}
