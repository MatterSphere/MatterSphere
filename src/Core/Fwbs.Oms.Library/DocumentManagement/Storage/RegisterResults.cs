using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public sealed class RegisterResult
    {
        private readonly IStorageItem item;
        private readonly string externalId;
        private readonly string token;

        public RegisterResult(IStorageItem item, string externalId, string token)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (externalId == null)
                externalId = String.Empty;

            if (token == null)
                token = String.Empty;

            this.item = item;
            this.externalId = externalId;
            this.item.Token = token;
        }

        public string ExternalId
        {
            get
            {
                return externalId;
            }
        }

        public string Token
        {
            get
            {
                return token;
            }
        }

        public IStorageItem Item
        {
            get
            {
                return item;
            }
        }
    }
}
