using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace XmlConverter.Models
{
    public class MultiEntities
    {
        public MultiEntities() { }

        public MultiEntities(SingleEntity entity)
        {
            Bucket = new Message
            {
                Entity = entity.Bucket?.Entity,
                Target = entity.Bucket?.Target,
                Elements = entity.Bucket == null
                    ? null
                    : new List<ExpandoObject> { entity.Bucket.Element }
            };
        }

        [JsonProperty("root")]
        public Message Bucket { get; set; }

        public bool IsEmptyBucket()
        {
            return Bucket?.Elements == null || Bucket?.Elements.Count == 0 || Bucket?.Elements[0] == null;
        }

        public class Message : BaseMessage
        {
            [JsonProperty("row")]
            public List<ExpandoObject> Elements { get; set; }
        }
    }
}
