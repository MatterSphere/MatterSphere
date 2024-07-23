using System;

namespace FWBS.OMS.DocuSign
{
    public enum StatusCode
    {
        Any,
        Voided,
        Created,
        Deleted,
        Sent,
        Delivered,
        Signed,
        Completed,
        Declined,
        TimedOut,
        Template,
        Processing,
        Downloaded = -1 // for MatterSphere internal use only
    }

    static class StatusCodeExtensions
    {
        internal static DocuSignWeb.EnvelopeStatusCode ToDocuSignWeb(this StatusCode value)
        {
            if (value != StatusCode.Downloaded)
                return (DocuSignWeb.EnvelopeStatusCode)value;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        internal static StatusCode FromDocuSignWeb(this DocuSignWeb.EnvelopeStatusCode value)
        {
            return (StatusCode)value;
        }
    }
}