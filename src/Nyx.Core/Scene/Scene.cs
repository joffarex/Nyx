using System;
using System.Collections.Generic;
using System.Drawing;
using Nyx.Core.Renderer;
using Nyx.SharpTT;
using Silk.NET.Input.Common;

namespace Nyx.Core.Scene
{
    public abstract class Scene : IDisposable
    {
        protected readonly BatchRenderer BatchRenderer = new BatchRenderer();

        // protected static PointF LastMousePosition;
        protected readonly List<GameObject> GameObjects = new List<GameObject>();
        private bool _isRunning;

        // public Camera3D Camera3D { get; protected set; }
        public Camera2D Camera2D { get; protected set; }

        public virtual void Dispose()
        {
            BatchRenderer.Dispose();

            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Dispose();
            }
        }

        public abstract void Init();

        public void Start()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Start();
                BatchRenderer.Add(gameObject);
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
                BatchRenderer.Add(gameObject);
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

        public virtual void Render()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Render();
            }
        }
    }
}