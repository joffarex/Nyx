using System;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nyx.Core.OpenGL
{
    public class Texture : IDisposable
    {
        private GL _gl;
        private uint _handle;

        public unsafe Texture(GL gl, TextureType type, string path)
        {
            var img = (Image<Rgba32>) Image.Load(path);
            img.Mutate(x => x.Flip(FlipMode.Vertical));

            fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0)))
            {
                Load(gl, type, data, (uint) img.Width, (uint) img.Height);
            }

            img.Dispose();
        }

        public unsafe Texture(GL gl, TextureType type, Span<byte> data, uint width, uint height)
        {
            fixed (void* d = &data[0])
            {
                Load(gl, type, d, width, height);
            }
        }

        public void Dispose()
        {
            _gl.DeleteTexture(_handle);
        }

        private unsafe void Load(GL gl, TextureType type, void* data, uint width, uint height)
        {
            _gl = gl;

            _handle = _gl.GenTexture();
            Bind();

            _gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, data);

            SetupTextureParameters(type);

            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }


        public void Bind()
        {
            _gl.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Activate(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            _gl.ActiveTexture(textureSlot);
        }

        public void Detach()
        {
            _gl.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void SetupTextureParameters(TextureType type)
        {
            switch (type)
            {
                case TextureType.NormalSprite:
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                        (int) GLEnum.ClampToEdge);
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                        (int) GLEnum.ClampToEdge);
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        (int) GLEnum.Linear);
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        (int) GLEnum.Linear);
                    break;
                case TextureType.PixelSprite:
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.Repeat);
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.Repeat);
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        (int) GLEnum.Nearest);
                    _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        (int) GLEnum.Nearest);
                    break;
                default:
                    throw new Exception($"{type} is unknown");
            }
        }
    }
}