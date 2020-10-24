using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace Nyx.Core.OpenGL
{
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private readonly GL _gl;
        private readonly uint _handle;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vertexBufferObject,
            BufferObject<TIndexType> elementBufferObject)
        {
            _gl = gl;

            _handle = _gl.GenVertexArray();
            Bind();
            vertexBufferObject.Bind();
            elementBufferObject.Bind();
        }

        public void Dispose()
        {
            _gl.DeleteVertexArray(_handle);
        }

        public unsafe void VertexAttributePointer(uint index, int size, VertexAttribPointerType type,
            uint vertexSizeBytes,
            int offset)
        {
            _gl.VertexAttribPointer(index, size, type, false, vertexSizeBytes * (uint) sizeof(TVertexType),
                (void*) (offset * sizeof(TVertexType)));
            _gl.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            _gl.BindVertexArray(_handle);
        }

        public void Detach()
        {
            _gl.BindVertexArray(0);
        }

        public void EnableVertexAttribPointers(IEnumerable<uint> locations)
        {
            foreach (uint l in locations)
            {
                _gl.EnableVertexAttribArray(l);
            }
        }

        public void DisableVertexAttribPointers(IEnumerable<uint> locations)
        {
            foreach (uint l in locations)
            {
                _gl.DisableVertexAttribArray(l);
            }
        }
    }
}