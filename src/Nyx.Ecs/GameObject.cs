using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nyx.Ecs
{
    public class GameObject : IDisposable
    {
        public GameObject()
        {
        }

        public GameObject(string name)
        {
            Init(name, new Transform(), 0);
        }

        public GameObject(string name, Transform transform, int zIndex)
        {
            Init(name, transform, zIndex);
        }


        public Transform Transform { get; set; }
        public int ZIndex { get; set; }
        public string Name { get; set; }

        [JsonRequired] private List<Component> Components { get; set; }

        public void Dispose()
        {
            foreach (Component component in Components)
            {
                component.Dispose();
            }
        }

        public void Init(string name, Transform transform, int zIndex)
        {
            Name = name;
            Components = new List<Component>();
            Transform = transform;
            ZIndex = zIndex;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in Components)
            {
                if (typeof(T) == component.GetType())
                {
                    return (T) component;
                }
            }

            return null;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
            component.GameObject = this;
        }

        public void RemoveComponent<T>() where T : Component
        {
            foreach (Component component in Components)
            {
                if (typeof(T) == component.GetType())
                {
                    Components.Remove(component);
                }
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

        public void ImGui()
        {
            foreach (Component component in Components)
            {
                component.ImGui();
            }
        }
    }
}