using System;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Common
{
    public class BufferObject<TDataType> : IDisposable, IEquatable<BufferObject<TDataType>> where TDataType : unmanaged
    {
        private static readonly ILogger<BufferObject<TDataType>> Logger =
            SerilogLogger.Factory.CreateLogger<BufferObject<TDataType>>();
        
        public bool IsDisposed { get; private set; }

        public BufferTarget BufferType { get; private set; }
        public int Handle { get; private set; }

        public int ElementCount { get; private set; }

        /// <summary>
        /// Element index which will be used to start subbuffering data
        /// </summary>
        public int CurrentElementIndex { get; private set; }

        public int ActiveElementCount { get; private set; }

        public BufferObject(TDataType[] data, BufferTarget bufferType, BufferUsageHint bufferUsage)
        {
            Init(data, data.Length, bufferType, bufferUsage);

            ActiveElementCount = ElementCount;
            CurrentElementIndex = 0;
        }

        public BufferObject(int elementCount, BufferTarget bufferType, BufferUsageHint bufferUsage)
        {
            Init(null, elementCount, bufferType, bufferUsage);

            ActiveElementCount = 0;
            CurrentElementIndex = 0;
        }

        public unsafe void Init(TDataType[] data, int elementCount, BufferTarget bufferType,
            BufferUsageHint bufferUsage)
        {
            BufferType = bufferType;
            Handle = GL.GenBuffer();
            ElementCount = elementCount;
            int fullSize = (ElementCount * sizeof(TDataType));

            Bind();
            GL.BufferData(bufferType, (IntPtr) fullSize, data, bufferUsage);
            CheckBufferSize(bufferType, fullSize);
        }

        ~BufferObject()
        {
            Dispose(false);
            Logger.LogError($"Memory leak detected on object: {this}");
        }

        public void ReBufferData(TDataType[] data)
        {
            if (data.Length > ElementCount)
                throw new ArgumentException(
                    $"Buffer not large enough to hold data. Buffer size: {ElementCount}. Elements to write: {data.Length}.");

            // Check if data does not fit at the end of the buffer
            int rest = ElementCount - CurrentElementIndex;
            if (rest >= data.Length)
            {
                BufferSubData(CurrentElementIndex, data, data.Length);
                // Make sure that we dont rewrite already buffered data on next rebuffer 
                CurrentElementIndex += data.Length;
                if (ActiveElementCount < CurrentElementIndex)
                {
                    ActiveElementCount = CurrentElementIndex;
                }

                // Make sure to reset offset as data exactly fits in buffer and reached the end of memory allocated to it
                if (CurrentElementIndex >= ElementCount) CurrentElementIndex = 0;
            }
            else
            {
                // If this case is reached, that means there is more data than it can be written in allocated memory
                // so we need to rewrite leftovers on top of the beginning of the buffer (or we could throw in the future)
                BufferSubData(CurrentElementIndex, data, rest);
                // Make sure that we add ONLY remaining elements of what could not be added at the end of a buffer in
                // the beginning of the buffer
                rest = data.Length - rest;
                BufferSubData(0, data, rest);
                CurrentElementIndex = rest;

                // Buffer is full
                ActiveElementCount = ElementCount;
            }
        }

        public unsafe void BufferSubData(int offset, TDataType[] data, int count)
        {
            if (count > ElementCount - offset)
                throw new ArgumentException(
                    $"Buffer not large enough to hold data. Buffer size: {ElementCount}. Offset: {offset}. Elements to write: {count}.");
            if (count > data.Length)
                throw new ArgumentException(
                    $"Not enough data to write to buffer. Data length: {data.Length}. Elements to write: {count}.");
            Bind();

            GL.BufferSubData(BufferType, (IntPtr) offset, (IntPtr) (count * sizeof(TDataType)), data);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferType, Handle);
        }

        public void CheckBufferSize(BufferTarget bufferTarget, int size)
        {
            int uploadedSize;
            GL.GetBufferParameter(bufferTarget, BufferParameterName.BufferSize, out uploadedSize);
            if (uploadedSize != size)
                throw new ApplicationException(
                    string.Format(
                        "Problem uploading data to buffer object. Tried to upload {0} bytes, but uploaded {1}.", size,
                        uploadedSize));
        }

        public void Clear()
        {
            BufferSubData(0, new TDataType[ElementCount], ElementCount);
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

        public bool Equals(BufferObject<TDataType> other)
        {
            return other != null && Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            return obj is BufferObject<TDataType> bufferObject && Equals(bufferObject);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"[{GetType().Name}]: {Handle}");
        }
    }
}