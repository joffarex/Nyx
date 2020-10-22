using System;

namespace Nyx.Engine
{
    public abstract class Scene : IDisposable
    {
        public virtual void Dispose()
        {
        }

        public virtual void Init()
        {
        }

        public abstract void Update(float deltaTime);
        public abstract void Render();
    }
}