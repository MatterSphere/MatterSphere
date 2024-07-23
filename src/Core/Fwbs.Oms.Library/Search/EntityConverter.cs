using System;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Search
{
    public class EntityConverter
    {
        public ResponseItem Convert(CommonRow row)
        {
            switch (row.EntityType)
            {
                case EntityTypeEnum.Appointment:
                    return ConvertAppointment(row);
                case EntityTypeEnum.Associate:
                    return ConvertAssociate(row);
                case EntityTypeEnum.Client:
                    return ConvertClient(row);
                case EntityTypeEnum.Contact:
                    return ConvertContact(row);
                case EntityTypeEnum.Document:
                    return ConvertDocument(row);
                case EntityTypeEnum.File:
                    return ConvertFile(row);
                case EntityTypeEnum.Precedent:
                    return ConvertPrecedent(row);
                case EntityTypeEnum.Task:
                    return ConvertTask(row);
            }

            throw new ArgumentException($"The entity type {row.EntityType} is not expected.");
        }

        private ResponseItem ConvertAppointment(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.AppointmentInfo.Description,
                Summary = $"{row.ClientInfo.Number}:{row.ClientInfo.Name}/{row.FileDescription}: {row.AppointmentInfo.Description} ({row.AppointmentInfo.Type}). {row.AppointmentInfo.Location}",
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = null,
                ClientId = row.ClientId,
                FileId = row.FileId,
                DocumentId = null,
                ObjectType = "Appointment"
            };
        }

        private ResponseItem ConvertAssociate(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.AssociateInfo.Salutation,
                Summary = $"{row.AssociateInfo.Salutation} ({row.AssociateInfo.Type})",
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = row.ContactId,
                ClientId = row.ClientId,
                FileId = row.FileId,
                DocumentId = null,
                ObjectType = "associate",
                Extension = null,
                AssociateId = row.AssociateId
            };
        }

        private ResponseItem ConvertClient(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = $"{row.ClientInfo.Number}: {row.ClientInfo.Name}",
                Summary = row.ClientInfo.Type,
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = null,
                ClientId = row.ClientId,
                FileId = null,
                DocumentId = null,
                ObjectType = "client"
            };
        }

        private ResponseItem ConvertContact(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.ContactName,
                Summary = row.Address,
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = row.ContactId,
                ClientId = null,
                FileId = null,
                DocumentId = null,
                ObjectType = "contact"
            };
        }

        private ResponseItem ConvertDocument(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.DocumentInfo.Description,
                Summary = $"{row.ClientInfo.Number}:{row.ClientInfo.Name} - {row.FileDescription}. {row.DocumentInfo.Description}.{row.DocumentInfo.Extension}",
                Author = row.DocumentInfo.Author,
                ModifiedDate = row.ModifiedDate,
                ContactId = null,
                ClientId = row.ClientId,
                FileId = row.FileId,
                DocumentId = row.DocumentId,
                ObjectType = "document",
                Extension = row.DocumentInfo.Extension
            };
        }

        private ResponseItem ConvertFile(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.FileDescription,
                Summary = $"{row.ClientInfo.Number}:{row.ClientInfo.Name} - {row.FileDescription}",
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = null,
                ClientId = row.ClientId,
                FileId = row.FileId,
                DocumentId = null,
                ObjectType = "file"
            };
        }

        private ResponseItem ConvertPrecedent(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.PrecedentInfo.Title,
                Summary = string.IsNullOrWhiteSpace(row.PrecedentInfo.Category)
                    ? row.PrecedentInfo.Description
                    : string.IsNullOrWhiteSpace(row.PrecedentInfo.Subcategory)
                        ? $"{row.PrecedentInfo.Category}: {row.PrecedentInfo.Description}"
                        : $"{row.PrecedentInfo.Category} ({row.PrecedentInfo.Subcategory}): {row.PrecedentInfo.Description}",
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = null,
                ClientId = null,
                FileId = null,
                DocumentId = null,
                ObjectType = "precedent",
                Extension = row.PrecedentInfo.Extension
            };
        }

        private ResponseItem ConvertTask(CommonRow row)
        {
            return new ResponseItem
            {
                MatterSphereId = row.Key,
                Title = row.TaskInfo.Description,
                Summary = $"{row.FileDescription}: {row.TaskInfo.Description} ({row.TaskInfo.Type})",
                Author = null,
                ModifiedDate = row.ModifiedDate,
                ContactId = null,
                ClientId = row.ClientId,
                FileId = row.FileId,
                DocumentId = null,
                ObjectType = "task"
            };
        }
    }
}
