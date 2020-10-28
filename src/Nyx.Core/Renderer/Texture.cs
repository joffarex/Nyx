using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nyx.Core.Renderer
{
    public class Texture : IDisposable
    {
        [JsonRequired] private uint _handle;

        public Texture()
        {
        }

        public Texture(TextureType type, string path)
        {
            InitWithPath(type, path);
        }

        public Texture(TextureType type, Span<byte> data, uint width, uint height)
        {
            InitWithData(type, data, width, height);
        }

        public string FilePath { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public void Dispose()
        {
            GraphicsContext.Gl.DeleteTexture(_handle);
        }

        public unsafe void InitWithData(TextureType type, Span<byte> data, uint width, uint height)
        {
            fixed (void* d = &data[0])
            {
                Load(type, d, width, height);
            }
        }

        public unsafe void InitWithPath(TextureType type, string path)
        {
            FilePath = path;
            var img = (Image<Rgba32>) Image.Load(FilePath);
            img.Mutate(x => x.Flip(FlipMode.Vertical));
            Width = img.Width;
            Height = img.Height;

            fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0)))
            {
                Load(type, data, (uint) img.Width, (uint) img.Height);
            }

            img.Dispose();
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