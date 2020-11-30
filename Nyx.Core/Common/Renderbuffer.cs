using System;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Common
{
    public class Renderbuffer : IDisposable, IEquatable<Renderbuffer>
    {
        private static readonly ILogger<Renderbuffer> Logger = SerilogLogger.Factory.CreateLogger<Renderbuffer>();


        public Renderbuffer(int width, int height)
        {
            Width = width;
            Height = height;

            Init();
        }

        public bool IsDisposed { get; private set; }
        public int Handle { get; private set; }
        public int Height { get; }
        public int Width { get; }

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

        public bool Equals(Renderbuffer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (IsDisposed == other.IsDisposed) && (Handle == other.Handle) && (Height == other.Height) &&
                   (Width == other.Width);
        }

        ~Renderbuffer()
        {
            Dispose(false);
            Logger.LogError($"Memory leak detected on object: {this}");
        }

        public void Dispose(bool manual)
        {
            if (!manual)
            {
                return;
            }

            GL.DeleteTexture(Handle);
        }

        public void Init()
        {
            Handle = GL.GenRenderbuffer();
            Bind();
            SetRenderbufferStorage();
        }

        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
        }

        public void SetRenderbufferStorage()
        {
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, Width,
                Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Renderbuffer) obj);
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