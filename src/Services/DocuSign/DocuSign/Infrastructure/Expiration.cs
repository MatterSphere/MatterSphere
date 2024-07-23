using System;

namespace FWBS.OMS.DocuSign
{
    public class Expiration
    {
        public Expiration(DateTime expirationDate, int expireWarn)
        {
            ExpireAfter = Math.Max((expirationDate - DateTime.Today).Days, 1);
            ExpireWarn = Math.Max(expireWarn, 0);
        }

        internal Expiration(int expireAfter, int expireWarn)
        {
            ExpireAfter = expireAfter;
            ExpireWarn = expireWarn;
        }

        public int ExpireAfter { get; internal set; }
        public int ExpireWarn { get; internal set; }
    }
}