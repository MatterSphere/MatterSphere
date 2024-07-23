using System;

namespace FWBS.OMS.DocuSign
{
    public interface IDocuSignService
    {
        void Impersonate(string userEmail);
        bool TestConnection();

        Status CreateAndSendEnvelope(string subject, string emailBlurb, DocumentInfo[] documents,
            RecipientInfo[] recipients, Reminder reminder = null, Expiration expiration = null);

        Status CreateEnvelope(string subject, string emailBlurb, DocumentInfo[] documents,
            RecipientInfo[] recipients, Reminder reminder = null, Expiration expiration = null);

        string GetEditEnvelopeUrl(Guid envelopeId, string returnUrl);

        Status GetEnvelopeStatus(Guid envelopeId);

        Status[] GetEnvelopeStatuses(Guid[] envelopeIds);

        EnvelopeInfo GetEnvelope(Guid envelopeId);

        bool VoidEnvelope(Guid envelopeId, string reason);

        SignedDocument GetSignedDocumentsInOneFile(Guid envelopeId, bool includeCert, bool addWaterMark = false);

        SignedDocument[] GetSignedDocuments(Guid envelopeId, bool includeCert);

        SignedDocument GetSigningCertificate(Guid envelopeId);
    }
}
