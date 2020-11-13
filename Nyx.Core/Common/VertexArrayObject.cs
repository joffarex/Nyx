using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Common
{
    public class VertexArrayObject<TVertexType> : IDisposable
        where TVertexType : unmanaged
    {
        public bool IsDisposed { get; private set; }


        public int Handle { get; protected set; }

        protected readonly List<uint> Locations = new();

        public VertexArrayObject(BufferObject<TVertexType> vertexBufferObject)
        {
            Handle = GL.GenVertexArray();
            Bind();
            vertexBufferObject.Bind();
        }

        ~VertexArrayObject()
        {
            Dispose(false);
            Console.WriteLine($"Memory leak detected on object: {this}");
        }

        public unsafe void VertexAttributePointer(uint location, int size, VertexAttribPointerType type,
            uint vertexSize,
            int offset)
        {
            GL.VertexAttribPointer(location, size, type, false,
                (int) (vertexSize * sizeof(TVertexType)),
                (IntPtr) (offset * sizeof(TVertexType)));
            GL.EnableVertexAttribArray(location);
            Locations.Add(location);
        }

        public void EnableVertexAttribPointers()
        {
            foreach (uint location in Locations)
            {
                GL.EnableVertexAttribArray(location);
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public void Detach()
        {
            GL.BindVertexArray(0);
        }

        public void DisableVertexAttribPointers()
        {
            foreach (uint location in Locations)
            {
                GL.DisableVertexAttribArray(location);
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            IsDisposed = true;
            Dispose(true);
            // prevent the destructor from being called
            GC.SuppressFinalize(this);
            // make sure the garbage collector does not eat our object before it is properly disposed
            GC.KeepAlive(this);
        }

        public void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteVertexArray(Handle);
        }
    }


    public class VertexArrayObject<TVertexType, TIndexType> : VertexArrayObject<TVertexType>
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        public VertexArrayObject(BufferObject<TVertexType> vertexBufferObject,
            BufferObject<TIndexType> elementBufferObject) : base(vertexBufferObject)
        {
            elementBufferObject.Bind();
        }
    }
}