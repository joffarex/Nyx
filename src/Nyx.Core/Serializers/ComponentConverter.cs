#nullable enable
using System;
using Newtonsoft.Json.Linq;
using Nyx.Core.Components;

namespace Nyx.Core.Serializers
{
    public class ComponentConverter : JsonCreationConverter<Component>
    {
        protected override Component Create(Type objectType, JObject jObject)
        {
            const string fieldName = "Type";

            if (FieldExists(fieldName, jObject) && (jObject[fieldName]!.ToString() == typeof(SpriteRenderer).FullName))
            {
                return new SpriteRenderer();

                // if (properties!["Sprite"]!["Texture"]!.ToObject<Texture>() == null)
                // {
                // return new SpriteRenderer(properties["Color"]!.ToObject<Vector4>());
                // }

                // return new SpriteRenderer(properties["Sprite"]!.ToObject<Sprite>());
            }

            if (FieldExists(fieldName, jObject) && (jObject[fieldName]!.ToString() == typeof(RigidBody).FullName))
            {
                return new RigidBody();
            }

            throw new Exception("Can not create instance of a component");
        }

        private static bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}