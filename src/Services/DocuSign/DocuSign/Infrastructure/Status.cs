using System;

namespace FWBS.OMS.DocuSign
{
    public class Status
    {
        public Guid EnvelopeId { get; internal set; }
        public StatusCode Code { get; internal set; }
        public string Subject { get; internal set; }
    }

    static class StatusExtensions
    {
        internal static Status FromDocuSignWeb(this DocuSignWeb.EnvelopeStatus value)
        {
            return new Status
            {
                EnvelopeId = Guid.Parse(value.EnvelopeID),
                Code = value.Status.FromDocuSignWeb(),
                Subject = value.Subject
            };
        }
    }
}
