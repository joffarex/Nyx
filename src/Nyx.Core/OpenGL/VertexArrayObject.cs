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

        private readonly List<uint> _locations = new List<uint>();

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

        public unsafe void VertexAttributePointer(uint location, int size, VertexAttribPointerType type,
            uint vertexSizeBytes,
            int offset)
        {
            _gl.VertexAttribPointer(location, size, type, false, vertexSizeBytes * (uint) sizeof(TVertexType),
                (void*) (offset * sizeof(TVertexType)));
            _gl.EnableVertexAttribArray(location);
            _locations.Add(location);
        }

        public void EnableVertexAttribPointers()
        {
            foreach (uint location in _locations)
            {
                _gl.EnableVertexAttribArray(location);
            }
        }

        public void Bind()
        {
            _gl.BindVertexArray(_handle);
        }

        public void Detach()
        {
            _gl.BindVertexArray(0);
        }

        public void DisableVertexAttribPointers()
        {
            foreach (uint location in _locations)
            {
                _gl.DisableVertexAttribArray(location);
            }
        }
    }
}