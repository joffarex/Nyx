using System;
using System.Drawing;
using Silk.NET.Input.Common;

namespace Nyx.Engine
{
    public abstract class Scene : IDisposable
    {
        protected static PointF LastMousePosition;
        public Camera Camera { get; protected set; }

        public virtual void Dispose()
        {
        }

        public virtual void Init()
        {
        }

        public abstract void Update(float deltaTime);

        public virtual void MouseMove(IMouse mouse, PointF position)
        {
        }

        public virtual void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
        }

        public abstract void Render();
    }
}