using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Nyx.Core.Entities;
using Nyx.Core.Logger;

namespace Nyx.Core.Components
{
    public abstract class Component : IDisposable, IEquatable<Component>
    {
        protected readonly ILogger<Component> Logger = SerilogLogger.Factory.CreateLogger<Component>();

        public Guid Uid { get; protected set; }

        public Entity Entity { get; set; }

        public abstract void Dispose();

        public bool Equals(Component other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Uid.Equals(other.Uid);
        }

        public void Init()
        {
            Uid = new Guid();
        }

        public abstract void Start();

        public abstract void Update(ref double deltaTime);
        public abstract void Render(ref double deltaTime);

        public virtual void ImGui()
        {
            try
            {
                Type type = GetType();
                IEnumerable<FieldInfo> fields = type.GetRuntimeFields();

                foreach (FieldInfo field in fields)
                {
                    Type fieldType = field.FieldType;
                    object fieldValue = field.GetValue(this);
                    string name = field.Name;

                    if (field.GetCustomAttributes(false).OfType<JsonIgnoreAttribute>().Any() || (fieldValue == null))
                    {
                        continue;
                    }

                    if (fieldType == typeof(int))
                    {
                        var castedValue = (int) fieldValue;
                        if (ImGuiNET.ImGui.DragInt($"{name}: ", ref castedValue, 1))
                        {
                            field.SetValue(this, castedValue);
                        }
                    }
                    else if (fieldType == typeof(float))
                    {
                        var castedValue = (float) fieldValue;
                        if (ImGuiNET.ImGui.DragFloat($"{name}: ", ref castedValue, 1))
                        {
                            field.SetValue(this, castedValue);
                        }
                    }
                    else if (fieldType == typeof(Vector3))
                    {
                        var castedValue = (Vector3) fieldValue;
                        if (ImGuiNET.ImGui.DragFloat3($"{name}: ", ref castedValue, 1))
                        {
                            field.SetValue(this, castedValue);
                        }
                    }
                    else if (fieldType == typeof(Vector4))
                    {
                        var castedValue = (Vector4) fieldValue;
                        if (ImGuiNET.ImGui.DragFloat4($"{name}: ", ref castedValue, 1))
                        {
                            field.SetValue(this, castedValue);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override string ToString()
        {
            return string.Format($"[{GetType().Name}]: {Uid}");
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

            return Equals((Component) obj);
        }

        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }
    }
}