using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FWBS.OMS.HighQ.Converters
{
    class SingleOrArrayJsonConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<T[]>();
            }

            return new T[] {token.ToObject<T>()};
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T[]);
        }
    }
}
