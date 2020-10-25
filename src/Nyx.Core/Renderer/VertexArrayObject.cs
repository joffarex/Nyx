using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace Nyx.Core.Renderer
{
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private readonly uint _handle;

        private readonly List<uint> _locations = new List<uint>();

        public VertexArrayObject(BufferObject<TVertexType> vertexBufferObject,
            BufferObject<TIndexType> elementBufferObject)
        {
            _handle = GraphicsContext.Gl.GenVertexArray();
            Bind();
            vertexBufferObject.Bind();
            elementBufferObject.Bind();
        }

        public void Dispose()
        {
            GraphicsContext.Gl.DeleteVertexArray(_handle);
        }

        public unsafe void VertexAttributePointer(uint location, int size, VertexAttribPointerType type,
            uint vertexSize,
            int offset)
        {
            GraphicsContext.Gl.VertexAttribPointer(location, size, type, false,
                (uint) (vertexSize * sizeof(TVertexType)),
                (void*) (offset * sizeof(TVertexType)));
            GraphicsContext.Gl.EnableVertexAttribArray(location);
            _locations.Add(location);
        }

        public void EnableVertexAttribPointers()
        {
            foreach (uint location in _locations)
            {
                GraphicsContext.Gl.EnableVertexAttribArray(location);
            }
        }

        public void Bind()
        {
            GraphicsContext.Gl.BindVertexArray(_handle);
        }

        public void Detach()
        {
            GraphicsContext.Gl.BindVertexArray(0);
        }

        public void DisableVertexAttribPointers()
        {
            foreach (uint location in _locations)
            {
                GraphicsContext.Gl.DisableVertexAttribArray(location);
            }
        }
    }
}