using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace Nyx.Core.Renderer
{
    public class VertexArrayObject<TVertexType> : IDisposable
        where TVertexType : unmanaged
    {
        protected readonly uint Handle;

        protected readonly List<uint> Locations = new List<uint>();

        public VertexArrayObject(BufferObject<TVertexType> vertexBufferObject)
        {
            Handle = GraphicsContext.Gl.GenVertexArray();
            Bind();
            vertexBufferObject.Bind();
        }

        public void Dispose()
        {
            GraphicsContext.Gl.DeleteVertexArray(Handle);
        }

        public unsafe void VertexAttributePointer(uint location, int size, VertexAttribPointerType type,
            uint vertexSize,
            int offset)
        {
            GraphicsContext.Gl.VertexAttribPointer(location, size, type, false,
                (uint) (vertexSize * sizeof(TVertexType)),
                (void*) (offset * sizeof(TVertexType)));
            GraphicsContext.Gl.EnableVertexAttribArray(location);
            Locations.Add(location);
        }

        public void EnableVertexAttribPointers()
        {
            foreach (uint location in Locations)
            {
                GraphicsContext.Gl.EnableVertexAttribArray(location);
            }
        }

        public void Bind()
        {
            GraphicsContext.Gl.BindVertexArray(Handle);
        }

        public void Detach()
        {
            GraphicsContext.Gl.BindVertexArray(0);
        }

        public void DisableVertexAttribPointers()
        {
            foreach (uint location in Locations)
            {
                GraphicsContext.Gl.DisableVertexAttribArray(location);
            }
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