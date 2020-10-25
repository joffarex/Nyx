using System;
using Silk.NET.OpenGL;

namespace Nyx.Core.Renderer
{
    public class BufferObject<TDataType> : IDisposable
        where TDataType : unmanaged
    {
        private readonly BufferTargetARB _bufferType;
        private readonly uint _handle;

        public unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType, BufferUsageARB bufferUsage)
        {
            _bufferType = bufferType;

            _handle = GraphicsContext.Gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                GraphicsContext.Gl.BufferData(bufferType, (UIntPtr) (data.Length * sizeof(TDataType)), d, bufferUsage);
            }
        }

        public void Dispose()
        {
            GraphicsContext.Gl.DeleteBuffer(_handle);
        }

        public unsafe void ReBufferData(Span<TDataType> data)
        {
            fixed (void* d = data)
            {
                GraphicsContext.Gl.BufferSubData(_bufferType, 0, (uint) (data.Length * sizeof(TDataType)), d);
            }
        }

        public void Bind()
        {
            GraphicsContext.Gl.BindBuffer(_bufferType, _handle);
        }
    }
}