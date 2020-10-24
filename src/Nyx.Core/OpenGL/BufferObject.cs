using System;
using Silk.NET.OpenGL;

namespace Nyx.Core.OpenGL
{
    public class BufferObject<TDataType> : IDisposable
        where TDataType : unmanaged
    {
        private readonly BufferTargetARB _bufferType;
        private readonly GL _gl;
        private readonly uint _handle;

        public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType, BufferUsageARB bufferUsage)
        {
            _gl = gl;
            _bufferType = bufferType;

            _handle = _gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                _gl.BufferData(bufferType, (UIntPtr) (data.Length * sizeof(TDataType)), d, bufferUsage);
            }
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_handle);
        }

        public void Bind()
        {
            _gl.BindBuffer(_bufferType, _handle);
        }
    }
}