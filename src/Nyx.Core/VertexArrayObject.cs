using System;
using Silk.NET.OpenGL;

namespace Nyx.Core
{
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private readonly GL _gl;
        private readonly uint _handle;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> verteBufferObject,
            BufferObject<TIndexType> elementBufferObject)
        {
            _gl = gl;

            _handle = _gl.GenVertexArray();
            Bind();
            verteBufferObject.Bind();
            elementBufferObject.Bind();
        }

        public void Dispose()
        {
            _gl.DeleteVertexArray(_handle);
        }

        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize,
            int offSet)
        {
            _gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint) sizeof(TVertexType),
                (void*) (offSet * sizeof(TVertexType)));
            _gl.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            _gl.BindVertexArray(_handle);
        }
    }
}