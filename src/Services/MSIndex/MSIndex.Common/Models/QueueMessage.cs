using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace MSIndex.Common.Models
{
    public class QueueMessage
    {
        [JsonProperty("root")]
        public Message Data { get; set; }

        public class Message
        {
            [JsonProperty("entityname")]
            public string Entity { get; set; }

            [JsonProperty("row")]
            [JsonConverter(typeof(SingleOrArrayConverter<ExpandoObject>))]
            public List<ExpandoObject> Items { get; set; }
        }

        public EntityType GetEntityType()
        {
            switch (Data.Entity.ToUpper())
            {
                case "ADDRESS":
                    return EntityType.Address;
                case "APPOINTMENT":
                    return EntityType.Appointment;
                case "ASSOCIATE":
                    return EntityType.Associate;
                case "CLIENT":
                    return EntityType.Client;
                case "CONTACT":
                    return EntityType.Contact;
                case "DOCUMENT":
                    return EntityType.Document;
                case "FILE":
                    return EntityType.File;
                case "PRECEDENT":
                    return EntityType.Precedent;
                case "TASK":
                    return EntityType.Task;
                case "USERS":
                    return EntityType.User;
                default:
                    throw new ArgumentException($"{Data.Entity} is not an expected type");
            }
        }
    }
}
