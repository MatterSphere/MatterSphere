using System;

namespace FWBS.OMS.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed class SecurableTypeAttribute : Attribute
    {
        public SecurableTypeAttribute(string securableTypeCode)
        {
            if (String.IsNullOrEmpty(securableTypeCode))
                throw new ArgumentNullException(securableTypeCode);

            this.code = securableTypeCode;
        }

        private string code;
        public string SecurableTypeCode
        {
            get
            {
                return code;
            }
        }
    }
}
