using System;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Common
{
    public class BufferObject<TDataType> : IDisposable where TDataType : unmanaged
    {
        public bool IsDisposed { get; private set; }

        public BufferTarget BufferType { get; private set; }
        public int Handle { get; private set; }

        public unsafe BufferObject(TDataType[] data, BufferTarget bufferType, BufferUsageHint bufferUsage)
        {
            BufferType = bufferType;

            Handle = GL.GenBuffer();
            Bind();
            GL.BufferData(bufferType, (IntPtr) (data.Length * sizeof(TDataType)), data, bufferUsage);
        }
        
        ~BufferObject()
        {
            Dispose(false);
            Console.WriteLine($"Memory leak detected on object: {this}");
        }

        public unsafe void ReBufferData(TDataType[] data)
        {
            GL.BufferSubData(BufferType, (IntPtr) 0, (IntPtr) (data.Length * sizeof(TDataType)), data);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferType, Handle);
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
            GL.DeleteBuffer(Handle);
        }
    }
}