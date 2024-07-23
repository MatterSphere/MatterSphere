using Elasticsearch.Models.Entities;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;
using Client = Elasticsearch.Models.Entities.Client;
using Contact = Elasticsearch.Models.Entities.Contact;
using Precedent = Elasticsearch.Models.Entities.Precedent;

namespace Elasticsearch.Provider
{
    public class EntityConverter
    {
        private readonly bool _isSummaryFieldEnabled;

        public EntityConverter(bool isSummaryFieldEnabled)
        {
            _isSummaryFieldEnabled = isSummaryFieldEnabled;
        }

        public ResponseItem Convert(dynamic model)
        {
            string entityType = model.Source.objecttype.ToString();
            switch (entityType.ToLower())
            {
                case "contact":
                    Contact contact = Convert<Contact>(model);
                    return contact.GetResponseItem();
                case "client":
                    Client client = Convert<Client>(model);
                    return client.GetResponseItem();
                case "file":
                    File file = Convert<File>(model);
                    return file.GetResponseItem();
                case "document":
                    Document document = Convert<Document>(model);
                    return document.GetResponseItem();
                case "email":
                    Email email = Convert<Email>(model);
                    return email.GetResponseItem();
                case "precedent":
                    Precedent precedent = Convert<Precedent>(model);
                    return precedent.GetResponseItem();
                case "appointment":
                    Appointment appointment = Convert<Appointment>(model);
                    return appointment.GetResponseItem();
                case "task":
                    Task task = Convert<Task>(model);
                    return task.GetResponseItem();
                case "note":
                    Note note = Convert<Note>(model);
                    return note.GetResponseItem();
                case "associate":
                    Associate associate = Convert<Associate>(model);
                    return associate.GetResponseItem();
                default:
                    return null;
            }
        }

        private T Convert<T>(dynamic model) where T : BaseItem
        {
            var json = JsonConvert.SerializeObject(model.Source);
            var document = (T)JsonConvert.DeserializeObject<T>(json);
            document.ElasticsearchId = model.Key;
            document.Highlights = model.Highlight;
            document.IsSummaryFieldEnabled = _isSummaryFieldEnabled;

            return document;
        }
    }
}
