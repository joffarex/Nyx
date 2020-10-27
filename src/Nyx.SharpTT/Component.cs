using System;

namespace Nyx.SharpTT
{
    public abstract class Component : IDisposable
    {
        public GameObject GameObject { get; set; }

        public virtual void Dispose()
        {
        }

        public virtual void Start()
        {
        }

        public abstract void Update(float deltaTime);
        public abstract void Render();

        public virtual void ImGui()
        {
        }
    }
}