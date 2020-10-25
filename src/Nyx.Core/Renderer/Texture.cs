using System;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nyx.Core.Renderer
{
    public class Texture : IDisposable
    {
        private uint _handle;

        public unsafe Texture(TextureType type, string path)
        {
            var img = (Image<Rgba32>) Image.Load(path);
            img.Mutate(x => x.Flip(FlipMode.Vertical));

            fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0)))
            {
                Load(type, data, (uint) img.Width, (uint) img.Height);
            }

            img.Dispose();
        }

        public unsafe Texture(TextureType type, Span<byte> data, uint width, uint height)
        {
            fixed (void* d = &data[0])
            {
                Load(type, d, width, height);
            }
        }

        public void Dispose()
        {
            GraphicsContext.Gl.DeleteTexture(_handle);
        }

        private unsafe void Load(TextureType type, void* data, uint width, uint height)
        {
            _handle = GraphicsContext.Gl.GenTexture();
            Bind();

            GraphicsContext.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba, width, height, 0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte, data);

            SetupTextureParameters(type);

            GraphicsContext.Gl.GenerateMipmap(TextureTarget.Texture2D);
        }


        public void Bind()
        {
            GraphicsContext.Gl.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Activate(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            GraphicsContext.Gl.ActiveTexture(textureSlot);
        }

        public void Detach()
        {
            GraphicsContext.Gl.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void SetupTextureParameters(TextureType type)
        {
            switch (type)
            {
                case TextureType.NormalSprite:
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                        (int) GLEnum.ClampToEdge);
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                        (int) GLEnum.ClampToEdge);
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        (int) GLEnum.Linear);
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        (int) GLEnum.Linear);
                    break;
                case TextureType.PixelSprite:
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                        (int) GLEnum.Repeat);
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                        (int) GLEnum.Repeat);
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        (int) GLEnum.Nearest);
                    GraphicsContext.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        (int) GLEnum.Nearest);
                    break;
                default:
                    throw new Exception($"{type} is unknown");
            }

            GraphicsContext.Gl.GenerateMipmap(TextureTarget.Texture2D);
        }
    }
}