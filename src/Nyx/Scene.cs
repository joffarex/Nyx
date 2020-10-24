using System;
using System.Collections.Generic;
using System.Drawing;
using Nyx.SharpTT;
using Silk.NET.Input.Common;
using Silk.NET.OpenGL;
using static Nyx.NyxEngine;

namespace Nyx
{
    public abstract class Scene : IDisposable
    {
        protected static PointF LastMousePosition;
        protected readonly List<GameObject> GameObjects = new List<GameObject>();
        private bool _isRunning;
        public Camera Camera { get; protected set; }

        public virtual void Dispose()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Dispose();
            }
        }

        public virtual void Init()
        {
        }

        public void Start()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Start();
            }

            _isRunning = true;
        }

        public void AddGameObjectToScene(GameObject gameObject)
        {
            if (!_isRunning)
            {
                GameObjects.Add(gameObject);
            }
            else
            {
                GameObjects.Add(gameObject);
                gameObject.Start();
            }
        }

        public virtual void Update(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(deltaTime);
            }
        }

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

        public virtual void Render()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Render();
            }
        }
    }
}