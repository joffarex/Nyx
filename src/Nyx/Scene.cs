using System;
using System.Drawing;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using static Nyx.NyxEngine;

namespace Nyx
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

        protected static unsafe void DrawElements(uint[] shapes)
        {
            Gl.DrawElements(PrimitiveType.Triangles, (uint) shapes.Length,
                DrawElementsType.UnsignedInt, null);
        }

        protected static void CleanupScreen()
        {
            Gl.BindVertexArray(0);
            Gl.UseProgram(0);
        }

        public abstract void Render();
    }
}