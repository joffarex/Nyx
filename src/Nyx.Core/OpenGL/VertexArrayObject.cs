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

        /// <summary>
        ///     Make sure that vertex array is formatted in this way: 3 floats for position (x, y, z); 4 floats for color (r, g, b,
        ///     a)
        /// </summary>
        public unsafe void SetVertexAttribPointers3Pos4Col()
        {
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), null);
            _gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float),
                (void*) (3 * sizeof(float)));
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