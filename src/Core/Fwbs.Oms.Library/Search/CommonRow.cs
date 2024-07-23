using System;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Search
{
    public class CommonRow
    {
        public CommonRow(string entityType, string key)
        {
            EntityType = GetEntityType(entityType);
            Key = key;
            SetId(key, EntityType);
        }
        
        public EntityTypeEnum EntityType { get; set; }
        public string Key { get; set; }

        public Associate AssociateInfo { get; set; }
        public Appointment AppointmentInfo { get; set; }
        public Client ClientInfo { get; set; }
        public Document DocumentInfo { get; set; }
        public Precedent PrecedentInfo { get; set; }
        public Task TaskInfo { get; set; }
        
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string FileDescription { get; set; }

        public DateTime ModifiedDate { get; set; }

        public long? AssociateId { get; set; }
        public long? ClientId { get; set; }
        public long? FileId { get; set; }
        public long? ContactId { get; set; }
        public long? DocumentId { get; set; }

        private EntityTypeEnum GetEntityType(string value)
        {
            switch (value.ToLower())
            {
                case "contact":
                    return EntityTypeEnum.Contact;
                case "client":
                    return EntityTypeEnum.Client;
                case "file":
                    return EntityTypeEnum.File;
                case "document":
                    return EntityTypeEnum.Document;
                case "precedent":
                    return EntityTypeEnum.Precedent;
                case "appointment":
                    return EntityTypeEnum.Appointment;
                case "task":
                    return EntityTypeEnum.Task;
                case "associate":
                    return EntityTypeEnum.Associate;
                default:
                    throw new ArgumentException($"The {value} is not expected entity type");
            }
        }

        private void SetId(string value, EntityTypeEnum entityType)
        {
            long id;
            Int64.TryParse(value, out id);
            switch (entityType)
            {
                case EntityTypeEnum.Associate:
                    AssociateId = id;
                    break;
                case EntityTypeEnum.Client:
                    ClientId = id;
                    break;
                case EntityTypeEnum.File:
                    FileId = id;
                    break;
                case EntityTypeEnum.Contact:
                    ContactId = id;
                    break;
                case EntityTypeEnum.Document:
                    DocumentId = id;
                    break;
            }
        }

        public class Client
        {
            public Client(string number, string name, string type)
            {
                Number = number;
                Name = name;
                Type = type;
            }

            public string Number { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }

        public class Document
        {
            public Document(string author, string description, string extension)
            {
                Author = author;
                Description = description;
                Extension = extension;
            }

            public string Author { get; set; }
            public string Description { get; set; }
            public string Extension { get; set; }
        }

        public class Precedent
        {
            public Precedent(string title, string category, string description, string extension)
            {
                Title = title;
                Category = category;
                Description = description;
                Extension = extension;
            }

            public string Title { get; set; }
            public string Category { get; set; }
            public string Subcategory { get; set; }
            public string Description { get; set; }
            public string Extension { get; set; }
        }

        public class Appointment
        {
            public Appointment(string description, string type, string location)
            {
                Description = description;
                Type = type;
                Location = location;
            }

            public string Description { get; set; }
            public string Type { get; set; }
            public string Location { get; set; }
        }

        public class Associate
        {
            public Associate(string salutation, string type)
            {
                Salutation = salutation;
                Type = type;
            }

            public string Salutation { get; set; }
            public string Type { get; set; }
        }

        public class Task
        {
            public Task(string description, string type)
            {
                Description = description;
                Type = type;
            }

            public string Description { get; set; }
            public string Type { get; set; }
        }
    }
}
