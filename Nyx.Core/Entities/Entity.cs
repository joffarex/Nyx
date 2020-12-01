using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nyx.Core.Components;
using Nyx.Core.Logger;
using Nyx.Core.Shared;

namespace Nyx.Core.Entities
{
    public class Entity : IDisposable, IEquatable<Entity>
    {
        protected readonly ILogger<Entity> Logger = SerilogLogger.Factory.CreateLogger<Entity>();

        public Entity()
        {
        }

        public Entity(string name, Transform transform, int zIndex)
        {
            Init(name, transform, zIndex);
        }

        public Guid Uid { get; protected set; }

        public Transform Transform { get; set; }
        public string Name { get; set; }
        public int ZIndex { get; set; }

        public HashSet<Component> Components { get; set; }

        public virtual void Dispose()
        {
            foreach (Component component in Components)
            {
                component.Dispose();
            }
        }

        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Uid.Equals(other.Uid) && Equals(Transform, other.Transform) && (Name == other.Name) &&
                   Equals(Components, other.Components);
        }

        public void Init(string name, Transform transform, int zIndex)
        {
            Uid = new Guid();
            Name = name;
            Transform = transform;
            ZIndex = zIndex;
            Components = new HashSet<Component>();
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
            component.Entity = this;
        }

        public T GetComponent<T>() where T : Component
        {
            return Components.Where(component => typeof(T) == component.GetType()).Cast<T>().FirstOrDefault();
        }


        public void RemoveComponent<T>() where T : Component
        {
            Components.RemoveWhere(component => component.GetType() == typeof(T));
        }

        public virtual void Start()
        {
            foreach (Component component in Components)
            {
                component.Start();
            }
        }

        public virtual void Update(ref double deltaTime)
        {
            foreach (Component component in Components)
            {
                component.Update(ref deltaTime);
            }
        }

        public virtual void Render(ref double deltaTime)
        {
            foreach (Component component in Components)
            {
                component.Render(ref deltaTime);
            }
        }

        public virtual void ImGui()
        {
            foreach (Component component in Components)
            {
                component.ImGui();
            }
        }

        public override string ToString()
        {
            return string.Format(
                $"[{GetType().Name}]: {Uid} - Name: {Name} - Transform: {{{Transform}}} - ZIndex: {ZIndex}");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid, Transform, Name, Components);
        }
    }
}