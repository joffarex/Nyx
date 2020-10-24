using System;
using System.Collections.Generic;
using System.Linq;

namespace Nyx.SharpTT
{
    public class GameObject : IDisposable
    {
        public GameObject(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public List<Component> Components { get; } = new List<Component>();

        public void Dispose()
        {
            foreach (Component component in Components)
            {
                component.Dispose();
            }
        }

        public T GetComponent<T>(T component) where T : Component
        {
            return Components.Any(c => component.GetType().IsAssignableFrom(c.GetType()))
                ? component
                : null;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
            component.GameObject = this;
        }

        public void RemoveComponent<T>(T component) where T : Component
        {
            if (Components.Any(c => component.GetType().IsAssignableFrom(c.GetType())))
            {
                Components.Remove(component);
            }
        }

        public void Start()
        {
            foreach (Component component in Components)
            {
                component.Start();
            }
        }

        public void Update(float dt)
        {
            foreach (Component component in Components)
            {
                component.Update(dt);
            }
        }

        public void Render()
        {
            foreach (Component component in Components)
            {
                component.Render();
            }
        }
    }
}