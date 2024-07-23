using Models.Common;
using XmlConverter.Models;

namespace IndexingController
{
    public class MessageTypeValidator
    {
        public MessageTypeEnum Validate(BaseMessage message)
        {
            if (!string.IsNullOrWhiteSpace(message.Target))
            {
                return MessageTypeEnum.Relation;
            }

            var entity = EntityType.GetEnum(message.Entity);
            if (entity == EntityEnum.User)
            {
                return MessageTypeEnum.Users;
            }

            return entity == EntityEnum.Document || entity == EntityEnum.Email || entity == EntityEnum.Precedent
                ? MessageTypeEnum.Contentable
                : MessageTypeEnum.Uncontentable;
        }
    }
}
