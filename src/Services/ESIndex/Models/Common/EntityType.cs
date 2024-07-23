namespace Models.Common
{
    public static class EntityType
    {
        public static EntityEnum GetEnum(string value)
        {
            value = value.ToLower().Trim();
            switch (value)
            {
                case "contact":
                    return EntityEnum.Contact;
                case "client":
                    return EntityEnum.Client;
                case "file":
                    return EntityEnum.File;
                case "document":
                    return EntityEnum.Document;
                case "email":
                    return EntityEnum.Email;
                case "precedent":
                    return EntityEnum.Precedent;
                case "note":
                    return EntityEnum.Note;
                case "appointment":
                    return EntityEnum.Appointment;
                case "task":
                    return EntityEnum.Task;
                case "users":
                    return EntityEnum.User;
                default:
                    return EntityEnum.Unknown;
            }
        }
    }
}
