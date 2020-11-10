using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Newtonsoft.Json;

namespace Nyx.Core.Components
{
    public abstract class Component : IDisposable
    {
        private static int _idCounter;
        public int Uid { get; set; } = -1;

        [JsonIgnore] public GameObject GameObject { get; set; }

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
            try
            {
                Type type = GetType();
                IEnumerable<FieldInfo> fields = type.GetRuntimeFields();

                foreach (FieldInfo field in fields)
                {
                    Type fieldType = field.FieldType;
                    object value = field.GetValue(this);
                    string name = field.Name;

                    if (field.GetCustomAttributes(false).OfType<JsonIgnoreAttribute>().Any())
                    {
                        continue;
                    }

                    if (fieldType == typeof(int))
                    {
                        var val = (int) value;
                        if (ImGuiNET.ImGui.DragInt($"{name}: ", ref val, 1))
                        {
                            field.SetValue(this, val);
                        }
                    }
                    else if (fieldType == typeof(float))
                    {
                        var val = (float) value;
                        if (ImGuiNET.ImGui.DragFloat($"{name}: ", ref val, 1))
                        {
                            field.SetValue(this, val);
                        }
                    }
                    else if (fieldType == typeof(Vector3))
                    {
                        var val = (Vector3) value;
                        if (ImGuiNET.ImGui.DragFloat3($"{name}: ", ref val, 1))
                        {
                            field.SetValue(this, val);
                        }
                    }
                    else if (fieldType == typeof(Vector4))
                    {
                        var val = (Vector4) value;
                        if (ImGuiNET.ImGui.DragFloat4($"{name}: ", ref val, 1))
                        {
                            field.SetValue(this, val);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void GenerateId()
        {
            if (Uid is -1)
            {
                Uid = _idCounter++;
            }
        }

        public static void Init(int maxId)
        {
            _idCounter = maxId;
        }
    }
}