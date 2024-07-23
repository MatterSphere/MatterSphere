using System;

namespace FWBS.OMS.DocuSign
{
    public enum RecipientType
    {
        Signer,
        Agent,
        Editor,
        Intermediary,
        CarbonCopy,
        CertifyDelivery,
        InPersonSigner,
        SigningHost,
    }

    static class RecipientTypeExtensions
    {
        internal static DocuSignWeb.RecipientTypeCode ToDocuSignWeb(this RecipientType value)
        {
            return (DocuSignWeb.RecipientTypeCode)value;
        }

        internal static RecipientType FromDocuSignWeb(this DocuSignWeb.RecipientTypeCode value)
        {
            return (RecipientType)value;
        }
    }
}