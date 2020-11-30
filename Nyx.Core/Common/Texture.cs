using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nyx.Core.Common
{
    public abstract class Texture : IDisposable, IEquatable<Texture>
    {
        private static readonly ILogger<Texture> Logger = SerilogLogger.Factory.CreateLogger<Texture>();

        protected Texture(int width, int height)
        {
            Width = width;
            Height = height;
            FilePath = "Generated";

            SetTextureImage2D(IntPtr.Zero, false);
        }

        protected Texture(int width, int height, IntPtr data)
        {
            Width = width;
            Height = height;
            FilePath = "Generated";

            Load(data);
        }

        protected unsafe Texture(string path)
        {
            FilePath = path;
            var img = (Image<Rgba32>) Image.Load(FilePath);
            img.Mutate(x => x.Flip(FlipMode.Vertical));
            Width = img.Width;
            Height = img.Height;

            fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0)))
            {
                Load((IntPtr) data);
            }

            img.Dispose();
        }

        public bool IsDisposed { get; private set; }

        public int Handle { get; private set; }

        public string FilePath { get; }

        public int Height { get; }
        public int Width { get; }

        // Only used for 3D textures
        public int Depth { get; } = 0;

        public abstract TextureTarget TextureTarget { get; }
        public virtual bool SupportsMipmaps => true;

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            Dispose(true);
            // prevent the destructor from being called
            GC.SuppressFinalize(this);
            // make sure the garbage collector does not eat our object before it is properly disposed
            GC.KeepAlive(this);
        }

        public bool Equals(Texture other)
        {
            return (other != null) && Handle.Equals(other.Handle);
        }

        private void Load(IntPtr data, bool withAlpha = true)
        {
            Handle = GL.GenTexture();
            Use();

            SetupTextureParameters();

            switch (TextureTarget)
            {
                case TextureTarget.Texture2D:
                    SetTextureImage2D(data, withAlpha);
                    break;
                case TextureTarget.Texture3D:
                    SetTextureImage3D(data);
                    break;
                default:
                    throw new InvalidOperationException("Invalid dimension");
            }

            GenerateMipMaps();
        }

        ~Texture()
        {
            Dispose(false);
            Logger.LogError($"Memory leak detected on object: {this}");
        }


        private void SetTextureImage2D(IntPtr data, bool withAlpha = true)
        {
            PixelInternalFormat internalFormat = withAlpha ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb;
            PixelFormat format = withAlpha ? PixelFormat.Rgba : PixelFormat.Rgb;

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, Height, Width, 0, format,
                PixelType.UnsignedByte, data);
        }

        private void SetTextureImage3D(IntPtr data)
        {
            GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba, Height, Width, Depth, 0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte, data);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            Bind();
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget, Handle);
        }

        public void Detach()
        {
            GL.BindTexture(TextureTarget, 0);
        }

        public void SetParameter(TextureParameterName parameterName, int value)
        {
            GL.TexParameter(TextureTarget, parameterName, value);
        }

        public void SetWrapMode(TextureWrapMode wrapMode)
        {
            var mode = (int) wrapMode;
            SetParameter(TextureParameterName.TextureWrapR, mode);
            SetParameter(TextureParameterName.TextureWrapS, mode);
            SetParameter(TextureParameterName.TextureWrapT, mode);
        }

        public void SetFilter(TextureMinFilter minFilter, TextureMagFilter magFilter)
        {
            SetParameter(TextureParameterName.TextureMinFilter, (int) minFilter);
            SetParameter(TextureParameterName.TextureMagFilter, (int) magFilter);
        }

        protected abstract void SetupTextureParameters();

        public void GenerateMipMaps()
        {
            if (!SupportsMipmaps)
            {
                throw new InvalidOperationException("Texture does not support mipmaps.");
            }

            Bind();
            GL.GenerateMipmap((GenerateMipmapTarget) TextureTarget);
        }

        public void Dispose(bool manual)
        {
            if (!manual)
            {
                return;
            }

            GL.DeleteTexture(Handle);
        }

        public override bool Equals(object obj)
        {
            return obj is Texture texture && Equals(texture);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"[{GetType().Name}]: {Handle}");
        }
    }
}