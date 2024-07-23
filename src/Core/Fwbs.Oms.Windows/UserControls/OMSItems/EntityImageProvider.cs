using System.Drawing;
using System.Text.RegularExpressions;

namespace FWBS.OMS.UI.Windows
{
    public enum Entity
    {
        Unknown,
        Appointment,
        Task,
        KeyDate,
        Associate,
        Undertaking,
        Contact
    };

    public class EntityImageProvider
    {
        private string _code;
        private Entity _entity;

        public EntityImageProvider(string enquiryCode)
        {
            _code = TransformEnquiryCodeToBase(enquiryCode);
            _entity = ConvertCodeToEntity();
        }

        public Entity Entity
        {
            get
            {
                return _entity;
            }
        }

        public Image Image
        {
            get
            {
                string imageName = string.Format("new-{0}-success", _entity.ToString().ToLower());
                return Images.GetNewEntityImage(imageName);
            }
        }

        private Entity ConvertCodeToEntity()
        {
            Entity entity = Entity.Unknown;
            switch (_code.ToUpper())
            {
                case "SIPFILAPPT":
                    entity = Entity.Appointment;
                    break;
                case "SIPFILTASK":
                    entity = Entity.Task;
                    break;
                case "SIPDWZNEW":
                    entity = Entity.KeyDate;
                    break;
                case "SIPASSPICKNEW":
                    entity = Entity.Associate;
                    break;
                case "SIPFILUNDERTAKI":
                    entity = Entity.Undertaking;
                    break;
                case "SIPCONPICKNEW":
                    entity = Entity.Contact;
                    break;
                default:
                    break;
            }
            return entity;
        }

        private string TransformEnquiryCodeToBase(string customEnquiryCode)
        {
            return Regex.Replace(customEnquiryCode, @"^(fd|ud|u)?SIP(.+)", "SIP$2", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }
}
