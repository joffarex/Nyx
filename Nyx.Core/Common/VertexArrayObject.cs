using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Common
{
    public class VertexArrayObject<TVertexType> : IDisposable, IEquatable<VertexArrayObject<TVertexType>>
        where TVertexType : unmanaged
    {
        private static readonly ILogger<VertexArrayObject<TVertexType>> Logger =
            SerilogLogger.Factory.CreateLogger<VertexArrayObject<TVertexType>>();

        public readonly List<int> Locations = new();

        public VertexArrayObject(BufferObject<TVertexType> vertexBufferObject)
        {
            Handle = GL.GenVertexArray();
            Bind();
            AssertActive();
            vertexBufferObject.Bind();
        }

        public bool IsDisposed { get; private set; }


        public int Handle { get; protected set; }

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

        public bool Equals(VertexArrayObject<TVertexType> other)
        {
            return (other != null) && Handle.Equals(other.Handle);
        }

        ~VertexArrayObject()
        {
            Dispose(false);
            Logger.LogError($"Memory leak detected on object: {this}");
        }

        public unsafe void VertexAttributePointer(int location, int size, VertexAttribPointerType type,
            int vertexSize,
            int offset)
        {
            Bind();
            GL.VertexAttribPointer(location, size, type, false,
                vertexSize * sizeof(TVertexType),
                (IntPtr) (offset * sizeof(TVertexType)));
            EnableVertexAttribPointer(location);
            Locations.Add(location);
        }

        public void EnableVertexAttribPointer(int location)
        {
            GL.EnableVertexAttribArray(location);
        }

        public void EnableVertexAttribPointers()
        {
            foreach (int location in Locations)
            {
                GL.EnableVertexAttribArray(location);
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void DisableVertexAttribPointers()
        {
            foreach (int location in Locations)
            {
                GL.DisableVertexAttribArray(location);
            }
        }

        public void DisableVertexAttribPointer(int location)
        {
            GL.DisableVertexAttribArray(location);
        }

        protected void AssertActive()
        {
#if DEBUG
            GL.GetInteger(GetPName.VertexArrayBinding, out int activeHandle);
            if (activeHandle != Handle)
            {
                throw new Exception("Vertex array object is not bound.");
            }
#endif
        }

        public void Dispose(bool manual)
        {
            if (!manual)
            {
                return;
            }

            GL.DeleteVertexArray(Handle);
        }

        public override bool Equals(object obj)
        {
            return obj is VertexArrayObject<TVertexType> vertexArrayObject && Equals(vertexArrayObject);
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


    public class VertexArrayObject<TVertexType, TIndexType> : VertexArrayObject<TVertexType>
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        public VertexArrayObject(BufferObject<TVertexType> vertexBufferObject,
            BufferObject<TIndexType> elementBufferObject) : base(vertexBufferObject)
        {
            AssertActive();
            elementBufferObject.Bind();
        }
    }
}