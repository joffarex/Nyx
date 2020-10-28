using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nyx.Core.Serializers
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite => true;
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public static JsonReader CopyReaderForObject(JsonReader reader, JToken jToken)
        {
            JsonReader jTokenReader = jToken.CreateReader();
            jTokenReader.Culture = reader.Culture;
            jTokenReader.DateFormatString = reader.DateFormatString;
            jTokenReader.DateParseHandling = reader.DateParseHandling;
            jTokenReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jTokenReader.FloatParseHandling = reader.FloatParseHandling;
            jTokenReader.MaxDepth = reader.MaxDepth;
            jTokenReader.SupportMultipleContent = reader.SupportMultipleContent;
            return jTokenReader;
        }

        public override void WriteJson(JsonWriter writer, object component, JsonSerializer serializer)
        {
            JObject serializedComponent = JObject.FromObject(new
            {
                Type = component.GetType().FullName,
                Properties = component,
            });
            writer.WriteRawValue(serializedComponent.ToString());
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jObject = JObject.Load(reader);

            T target = Create(objectType, jObject);

            using JsonReader jObjectReader = CopyReaderForObject(reader, jObject["Properties"]);
            serializer.Populate(jObjectReader, target);

            return target;
        }
    }
}