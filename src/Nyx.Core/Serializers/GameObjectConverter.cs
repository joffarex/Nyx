#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nyx.Core.Components;

namespace Nyx.Core.Serializers
{
    public class GameObjectConverter : JsonConverter<GameObject>
    {
        public override void WriteJson(JsonWriter writer, GameObject? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            writer.WriteValue(value);
        }

        public override GameObject ReadJson(JsonReader reader, Type objectType, GameObject? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            JToken? nameJToken = jObject.SelectToken("Name");
            JToken? transformJToken = jObject.SelectToken("Transform");
            JToken? zIndexJToken = jObject.SelectToken("ZIndex");
            JToken? componentsJToken = jObject.SelectToken("Components");

            if ((nameJToken == null) || (transformJToken == null) || (zIndexJToken == null) ||
                (componentsJToken == null))
            {
                throw new JsonSerializationException("Components can not be null");
            }

            var gameObject = new GameObject(nameJToken.ToString(), transformJToken.ToObject<Transform>(),
                zIndexJToken.ToObject<int>());

            var components = (JArray) componentsJToken;
            foreach (JToken component in components)
            {
                var c = JsonConvert.DeserializeObject<Component>(component.ToString(), new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Include,
                    NullValueHandling = NullValueHandling.Include,

                    Converters = new List<JsonConverter> {new ComponentConverter()},
                });
                gameObject.AddComponent(c);
            }

            return gameObject;
        }
    }
}