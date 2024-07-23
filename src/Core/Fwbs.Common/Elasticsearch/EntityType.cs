namespace FWBS.Common.Elasticsearch
{
    public enum EntityTypeEnum
    {
        Unknown = 0,
        Contact = 1,
        Client = 2,
        File = 3,
        Document = 4,
        Email = 5,
        Precedent = 6,
        Appointment = 7,
        Note = 8,
        Task = 9,
        Associate = 11
    }

    public class EntityType
    {
        public EntityType(EntityTypeEnum type)
        {
            Key = type;
            Label = Convert(type);
        }

        public EntityTypeEnum Key { get; set; }
        public string Label { get; set; }

        public static string Convert(EntityTypeEnum key)
        {
            return key.ToString("F").ToLower();
        }

        public static EntityTypeEnum Convert(string type)
        {
            switch (type.ToLower())
            {
                case "contact":
                    return EntityTypeEnum.Contact;
                case "client":
                    return EntityTypeEnum.Client;
                case "file":
                    return EntityTypeEnum.File;
                case "document":
                    return EntityTypeEnum.Document;
                case "email":
                    return EntityTypeEnum.Email;
                case "precedent":
                    return EntityTypeEnum.Precedent;
                case "appointment":
                    return EntityTypeEnum.Appointment;
                case "note":
                    return EntityTypeEnum.Note;
                case "task":
                    return EntityTypeEnum.Task;
                case "associate":
                    return EntityTypeEnum.Associate;
                default:
                    return EntityTypeEnum.Unknown;
            }
        }
    }
}
