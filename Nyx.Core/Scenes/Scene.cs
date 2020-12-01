using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nyx.Core.Entities;
using Nyx.Core.Logger;

namespace Nyx.Core.Scenes
{
    public abstract class Scene : IDisposable
    {
        protected readonly List<Entity> Entities = new();
        protected readonly ILogger<Scene> Logger = SerilogLogger.Factory.CreateLogger<Scene>();
        public bool IsRunning { get; private set; }
        public bool SceneLoaded { get; protected set; }
        public Entity ActiveEntity { get; protected set; }

        //camera

        public virtual void Dispose()
        {
            foreach (Entity entity in Entities)
            {
                entity.Dispose();
            }
        }

        public abstract void Init();

        public virtual void Start()
        {
            foreach (Entity entity in Entities)
            {
                entity.Start();
            }

            IsRunning = true;
        }

        public void AddEntityToScene(Entity entity)
        {
            if (!IsRunning)
            {
                Entities.Add(entity);
            }
            else
            {
                Entities.Add(entity);
                entity.Start();
            }
        }

        public virtual void Update(ref double deltaTime)
        {
            foreach (Entity entity in Entities)
            {
                entity.Update(ref deltaTime);
            }
        }

        public virtual void Render(ref double deltaTime)
        {
            foreach (Entity entity in Entities)
            {
                entity.Render(ref deltaTime);
            }
        }

        public virtual void ImGui()
        {
            if (ActiveEntity != null)
            {
                ImGuiNET.ImGui.Begin("Inspector");
                ActiveEntity.ImGui();
                ImGuiNET.ImGui.End();
            }
        }

        public abstract void LoadResources();

        public T GetEntityByName<T>(string name) where T : Entity
        {
            return Entities.Where(entity => (typeof(T) == entity.GetType()) && entity.Name.Equals(name)).Cast<T>()
                .FirstOrDefault();
        }

        public T GetEntityByUid<T>(Guid uid) where T : Entity
        {
            return Entities.Where(entity => (typeof(T) == entity.GetType()) && entity.Uid.Equals(uid)).Cast<T>()
                .FirstOrDefault();
        }
    }
}