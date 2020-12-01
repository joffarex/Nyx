using System;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL;

namespace Nyx.Core.Renderer
{
    public class Framebuffer : IDisposable, IEquatable<Framebuffer>
    {
        private static readonly ILogger<Framebuffer> Logger = SerilogLogger.Factory.CreateLogger<Framebuffer>();

        public Framebuffer(int width, int height)
        {
            Width = width;
            Height = height;

            Init();
        }

        public bool IsDisposed { get; private set; }

        public int Handle { get; private set; }
        public int Height { get; }
        public int Width { get; }

        public Texture2D Texture { get; private set; }

        public Renderbuffer RenderbufferObject { get; private set; }

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

        public bool Equals(Framebuffer other)
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

        public void Init()
        {
            Handle = GL.GenFramebuffer();
            Bind();

            // Create the texture to render the data to, and attach it to framebuffer
            Texture = new Texture2D(Width, Height);
            SetFramebufferTexture(FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D);

            // Create render buffer, which stores depth info
            RenderbufferObject = new Renderbuffer(Width, Height);
            SetRenderbufferToFramebuffer();

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Framebuffer is not complete");
            }

            Detach();
        }

        private void SetFramebufferTexture(FramebufferAttachment attachment, TextureTarget target)
        {
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, target, Texture.Handle, 0);
        }

        private void SetRenderbufferToFramebuffer()
        {
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
                RenderbufferTarget.Renderbuffer, RenderbufferObject.Handle);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
        }

        public void Detach()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        ~Framebuffer()
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

            Texture.Dispose();
            RenderbufferObject.Dispose();
            GL.DeleteTexture(Handle);
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

            return (obj.GetType() == GetType()) && Equals((Framebuffer) obj);
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