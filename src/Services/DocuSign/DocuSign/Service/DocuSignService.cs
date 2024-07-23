using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using FWBS.OMS.DocuSignWeb;

namespace FWBS.OMS.DocuSign
{
    public class DocuSignService : IDocuSignService
    {
        private readonly string _apiUrl;
        private readonly string _accountId;
        private readonly string _integrationKey;
        private readonly string _serviceAccountLogin;
        private readonly string _serviceAccountPassword;

        private string _userEmail;

        private const string ApiVersion = "/api/3.0/api.asmx";
        private const string DocIdFieldName = "DocID";

        static DocuSignService()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// DocuSignService constructor.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="accountId"></param>
        /// <param name="integrationKey"></param>
        /// <param name="serviceAccountLogin"></param>
        /// <param name="serviceAccountPassword"></param>
        public DocuSignService(string baseUrl, string accountId, string integrationKey, string serviceAccountLogin, string serviceAccountPassword)
        {
            _apiUrl = baseUrl.TrimEnd('/') + ApiVersion;
            _accountId = accountId;
            _integrationKey = integrationKey;
            _serviceAccountLogin = serviceAccountLogin;
            _serviceAccountPassword = serviceAccountPassword;
        }

        /// <summary>
        /// Set user email address for making SOBO requests.
        /// </summary>
        /// <param name="userEmail"></param>
        public void Impersonate(string userEmail)
        {
            _userEmail = userEmail;
        }

        /// <summary>
        /// The method enables API users make a simple call to the API service to determine its active state.
        /// </summary>
        /// <returns>True if the Ping request succeeded.</returns>
        public bool TestConnection()
        {
            using (var client = GetClient())
            {
                return client.Ping();
            }
        }

        /// <summary>
        /// Used to create and send envelopes, specify recipients, documents and actions on those documents within that envelope.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="emailBlurb"></param>
        /// <param name="documents"></param>
        /// <param name="recipients"></param>
        /// <param name="reminder"></param>
        /// <param name="expiration"></param>
        /// <returns>Envelope status.</returns>
        public Status CreateAndSendEnvelope(string subject, string emailBlurb, DocumentInfo[] documents,
            RecipientInfo[] recipients, Reminder reminder = null, Expiration expiration = null)
        {
            var envelope = BuildEnvelope(subject, emailBlurb, documents, recipients, reminder, expiration);

            using (var client = GetClient())
            {
                var status = client.CreateAndSendEnvelope(envelope);
                return status.FromDocuSignWeb();
            }
        }

        /// <summary>
        /// Used to create envelopes, specify recipients, documents and actions on those documents within that envelope. 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="emailBlurb"></param>
        /// <param name="documents"></param>
        /// <param name="recipients"></param>
        /// <param name="reminder"></param>
        /// <param name="expiration"></param>
        /// <returns>Envelope status.</returns>
        public Status CreateEnvelope(string subject, string emailBlurb, DocumentInfo[] documents,
            RecipientInfo[] recipients, Reminder reminder = null, Expiration expiration = null)
        {
            var envelope = BuildEnvelope(subject, emailBlurb, documents, recipients, reminder, expiration);

            using (var client = GetClient())
            {
                var status = client.CreateEnvelope(envelope);
                return status.FromDocuSignWeb();
            }
        }

        /// <summary>
        /// This method is used to get a onetime use login token that allows the user to be placed into the DocuSign sending wizard.
        /// Upon sending completion the user is returned to the return URL provided by the API application.
        /// </summary>
        /// <param name="envelopeId">Optional envelope ID. If provided the user will be placed into the envelope in process.
        /// If left blank the user will be placed into a new envelope for sending.</param>
        /// <param name="returnUrl">URL to send the user to upon completion of the sending process.
        /// The URL will have an event passed to it as a query parameter. The parameter will be named event.
        /// The DocuSign Envelope Id will also be returned in the envelopeId parameter.
        /// Important: You must include HTTPS:// in the URL or the redirect might be blocked by some browsers.</param>
        /// <returns>Target URL.</returns>
        public string GetEditEnvelopeUrl(Guid envelopeId, string returnUrl)
        {
            using (var client = GetClient())
            {
                var editEnvelopUrl = client.RequestSenderToken(envelopeId.ToString(), _accountId, returnUrl);
                return editEnvelopUrl;
            }
        }

        /// <summary>
        /// Used to query the status of existing envelope.
        /// </summary>
        /// <param name="envelopeId">The Envelope ID for which to retrieve status.</param>
        /// <returns>Envelope status.</returns>
        public Status GetEnvelopeStatus(Guid envelopeId)
        {
            using (var client = GetClient())
            {
                var status = client.RequestStatus(envelopeId.ToString());
                return status.FromDocuSignWeb();
            }
        }

        /// <summary>
        /// Used to query the statuses of existing envelopes.
        /// </summary>
        /// <param name="envelopeIds">The array of Envelope IDs for which to retrieve statuses.</param>
        /// <returns>Envelope statuses.</returns>
        public Status[] GetEnvelopeStatuses(Guid[] envelopeIds)
        {
            const int batchSize = 200;
            List<Status> statuses = new List<Status>(envelopeIds.Length);
            EnvelopeStatusFilter filter = new EnvelopeStatusFilter() { AccountId = _accountId };

            using (var client = GetClient())
            {
                for (int offset = 0; offset < envelopeIds.Length; offset += batchSize)
                {
                    int count = Math.Min(batchSize, envelopeIds.Length - offset);
                    string[] ids = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        ids[i] = envelopeIds[offset + i].ToString();
                    }

                    filter.EnvelopeIds = ids;
                    var envStatuses = client.RequestStatuses(filter).EnvelopeStatuses;
                    foreach (EnvelopeStatus envStatus in envStatuses)
                    {
                        statuses.Add(envStatus.FromDocuSignWeb());
                    }
                }
            }
            return statuses.ToArray();
        }

        /// <summary>
        /// Returns an API Envelope object containing all the data of an envelope. The envelope must be owned by the API user.
        /// </summary>
        /// <param name="envelopeId">The envelope ID of the user’s envelope</param>
        /// <returns>Envelope info.</returns>
        public EnvelopeInfo GetEnvelope(Guid envelopeId)
        {
            using (var client = GetClient())
            {
                var envelope = client.RequestEnvelopeWithDocumentFields(envelopeId.ToString(), false);
                if (envelope == null) return null;

                var result = new EnvelopeInfo { Subject = envelope.Subject, EmailBlurb = envelope.EmailBlurb };

                if (envelope.Documents != null && envelope.Documents.Length > 0)
                {
                    var documents = new List<DocumentInfo>();

                    foreach (var document in envelope.Documents)
                    {
                        uint order = 0; long docId = 0;
                        if (document.DocumentFields != null && document.DocumentFields.Length > 0)
                        {
                            string documentFieldValue = document.DocumentFields.FirstOrDefault(p => p.Name.Equals(DocIdFieldName))?.Value;
                            long.TryParse(documentFieldValue, out docId);
                        }

                        var doc = new DocumentInfo
                        {
                            Order = uint.TryParse(document.ID, out order) ? order : 0,
                            Id = docId,
                            Description = document.Name,
                            FileExtension = document.FileExtension
                        };

                        documents.Add(doc);
                    }

                    result.Documents = documents.ToArray();
                }

                if (envelope.Recipients != null && envelope.Recipients.Length > 0)
                {
                    result.Recipients = envelope.Recipients.Select(recipient => 
                        new RecipientInfo
                        {
                            Id = Convert.ToUInt32(recipient.ID),
                            Email = recipient.Email,
                            UserName = recipient.UserName,
                            Type = recipient.Type.FromDocuSignWeb(),
                            Tag = recipient.CustomFields?.FirstOrDefault()
                        }).ToArray();
                }

                if (envelope.Notification != null)
                {
                    var reminders = envelope.Notification.Reminders;
                    if (reminders != null && reminders.ReminderEnabled)
                    {
                        result.Reminder = new Reminder(Convert.ToInt32(reminders.ReminderDelay), Convert.ToInt32(reminders.ReminderFrequency));
                    }

                    var expirations = envelope.Notification.Expirations;
                    if (expirations != null && expirations.ExpireEnabled)
                    {
                        result.Expiration = new Expiration(Convert.ToInt32(expirations.ExpireAfter), Convert.ToInt32(expirations.ExpireWarn));
                    }
                }

                var envelopeStatus = client.RequestStatus(envelopeId.ToString());
                result.StatusCode = envelopeStatus.Status.FromDocuSignWeb();

                if (result.Recipients != null && envelopeStatus.RecipientStatuses != null)
                {
                    foreach (var rcpt in envelopeStatus.RecipientStatuses)
                    {
                        RecipientInfo recipient = result.Recipients.FirstOrDefault(r => r.Email == rcpt.Email && r.Type == rcpt.Type.FromDocuSignWeb());
                        if (recipient != null && recipient.Tag == null)
                        {
                            recipient.Tag = rcpt.CustomFields?.FirstOrDefault();
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Method is used to void an envelope. Only incomplete envelopes can be voided.
        /// EnvelopeID specified in the VoidEnvelope request should exist else an exception is thrown with error message “Envelope_Does_Not_Exist” .
        /// Only envelopes in the 'Sent' or 'Delivered' states may be voided. Else “Envelope_Cannot_Void_Invalid_State” exception will be thrown.
        /// Only the sender of envelope can void the envelope. If a user other than sender tries to void an envelope then exception “User_Not_Envelope_Sender” will be thrown.
        /// </summary>
        /// <param name="envelopeId">This is the envelope ID of the envelope which needs to be voided.
        /// The length of any of the EnvelopeID must not exceed 100 characters. Else, the XML validation fails and the processor throws a Validation error.</param>
        /// <param name="reason">The reason for voiding the envelope. Envelope recipients, when notified of the void, are shown this reason.
        /// The length of reason should not exceed 200 characters and should have a minimum of 1 character.</param>
        /// <returns>Set to true if the void operation succeeded.</returns>
        public bool VoidEnvelope(Guid envelopeId, string reason)
        {
            using (var client = GetClient())
            {
                var voidEnvelopeStatus = client.VoidEnvelope(envelopeId.ToString(), reason);
                return voidEnvelopeStatus.VoidSuccess;
            }
        }

        /// <summary>
        /// This method returns all of the documents combined into a single, contiguous PDF. Additionally it returns the Signing Certificate PDF document,
        /// which details the specific attributes of the participants and landmark events of the signing transaction. It also returns the Electronic Record
        /// and Signature Disclosure, per account settings, associated with the envelope.
        /// </summary>
        /// <param name="envelopeId">This is the envelope ID of the envelope, as returned by the CreateEnvelope/CreateAndSendEnvelope.</param>
        /// <param name="includeCert"></param>
        /// <param name="addWaterMark">If your account has the watermark feature enabled and when true, the watermark for the account is shown on
        /// documents for envelopes that are not completed.</param>
        /// <returns></returns>
        public SignedDocument GetSignedDocumentsInOneFile(Guid envelopeId, bool includeCert, bool addWaterMark = false)
        {
            using (var client = GetClient())
            {
                var status = client.RequestStatus(envelopeId.ToString());
                var doc = new SignedDocument { Description = status.Subject };
                if (status.Status == EnvelopeStatusCode.Completed)
                {
                    var pdf = client.RequestPDFWithOptions(envelopeId.ToString(), new PDFOptions { IncludeCert = includeCert, AddWaterMark = addWaterMark });
                    doc.Data = pdf.PDFBytes;
                    if (!includeCert)
                        doc.IsCertificate = false;
                }
                return doc;
            }
        }

        /// <summary>
        /// Creates the PDF File that contains the entire contents of the envelope, 
        /// so if you sent in an envelope with five different documents in it, the resulting PDF will contain all five documents in order,
        /// plus the Envelope Certificate
        /// </summary>
        /// <param name="envelopeId">This is the envelope ID of the envelope, as returned by the CreateEnvelope/CreateAndSendEnvelope.</param>
        /// <param name="includeCert"></param>
        /// <returns></returns>
        public SignedDocument[] GetSignedDocuments(Guid envelopeId, bool includeCert)
        {
            using (var client = GetClient())
            {
                var documentPdFs = client.RequestDocumentPDFsEx(envelopeId.ToString());
                var status = client.RequestStatusWithDocumentFields(envelopeId.ToString());

                var result = BuildSignedDocuments(documentPdFs.DocumentPDF, status);
                if (!includeCert)
                {
                    result = result.Where(d => d.IsCertificate == false).ToArray();
                }
                return result;
            }
        }

        /// <summary>
        /// GetCertificate returns the signing certificate, which details the specific attributes of the participants and landmark events of the signing transaction, for an envelope.
        /// </summary>
        /// <param name="envelopeId">This is the envelope ID of the envelope, as returned by the CreateEnvelope/CreateAndSendEnvelope.</param>
        /// <returns></returns>
        public SignedDocument GetSigningCertificate(Guid envelopeId)
        {
            using (var client = GetClient())
            {
                var certificate = client.RequestCertificate(envelopeId.ToString());
                var result = BuildSignedDocuments(certificate.DocumentPDF);
                return result.FirstOrDefault(d => d.IsCertificate == true);
            }
        }

        #region Private methods

        private APIServiceSoapClient GetClient()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential)
            {
                MaxReceivedMessageSize = 34 * 1024 * 1024,
                SendTimeout = TimeSpan.FromMinutes(10),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };

            var apiClient = new APIServiceSoapClient(binding, new EndpointAddress(_apiUrl));
            var credentials = apiClient.ClientCredentials;
            if (credentials != null)
            {
                credentials.UserName.UserName = GetUserLogin();
                credentials.UserName.Password = _serviceAccountPassword;
            }

            return apiClient;
        }

        private string GetUserLogin()
        {
            return string.IsNullOrEmpty(_userEmail)
                ? $"[{_integrationKey}]{_serviceAccountLogin}"
                : $"[{_integrationKey}][{_userEmail}]{_serviceAccountLogin}";
        }

        private Envelope BuildEnvelope(string subject, string emailBlurb, DocumentInfo[] documents,
            RecipientInfo[] recipients, Reminder reminder = null, Expiration expiration = null)
        {
            var envelope = new Envelope
            {
                Subject = subject,
                EmailBlurb = emailBlurb,
                AccountId = _accountId,
            };

            if (recipients != null && recipients.Length > 0)
            {
                var recipientList = new List<Recipient>(recipients.Length);
                foreach (var recipient in recipients)
                {
                    var rec = new Recipient
                    {
                        ID = recipient.Id.ToString(),
                        UserName = recipient.UserName,
                        Email = recipient.Email,
                        Type = recipient.Type.ToDocuSignWeb(),
                        CustomFields = recipient.Tag != null ? new string[] { recipient.Tag } : null
                    };

                    recipientList.Add(rec);
                }

                envelope.Recipients = recipientList.ToArray();
            }

            if (documents != null && documents.Length > 0)
            {
                var documentList = new List<Document>(documents.Length);
                foreach (var document in documents)
                {
                    var doc = new Document
                    {
                        ID = document.Order.ToString(),
                        Name = document.Description.Length <= 100 ? document.Description : document.Description.Remove(100),
                        PDFBytes = document.Data,
                        FileExtension = document.FileExtension,
                        DocumentFields = new DocumentField[] { new DocumentField { Name = DocIdFieldName, Value = document.Id.ToString() } }
                    };
                    documentList.Add(doc);
                }

                envelope.Documents = documentList.ToArray();
            }

            if (reminder != null)
            {
                envelope.Notification = new Notification
                {
                    Reminders = new Reminders
                    {
                        ReminderEnabled = true,
                        ReminderDelay = reminder.Delay.ToString(),
                        ReminderFrequency = reminder.Frequency.ToString()
                    }
                };
            }

            if (expiration != null)
            {
                if (envelope.Notification == null)
                {
                    envelope.Notification = new Notification();
                }

                envelope.Notification.Expirations = new Expirations
                {
                    ExpireEnabled = true,
                    ExpireAfter = expiration.ExpireAfter.ToString(),
                    ExpireWarn = expiration.ExpireWarn.ToString()
                };
            }

            return envelope;
        }

        private SignedDocument[] BuildSignedDocuments(IEnumerable<DocumentPDF> documents, EnvelopeStatus envelopeStatus = null)
        {
            var signedDocuments = new List<SignedDocument>();

            foreach (var document in documents)
            {
                uint order = 0; long docId = 0;
                if (envelopeStatus != null)
                {
                    var documentStatus = envelopeStatus.DocumentStatuses?.FirstOrDefault(p => p.ID.Equals(document.DocumentID));
                    if (documentStatus != null && documentStatus.DocumentFields != null && documentStatus.DocumentFields.Length > 0)
                    {
                        string documentFieldValue = documentStatus.DocumentFields.FirstOrDefault(p => p.Name.Equals(DocIdFieldName))?.Value;
                        long.TryParse(documentFieldValue, out docId);
                    }
                }

                var doc = new SignedDocument
                {
                    Order = uint.TryParse(document.DocumentID, out order) ? order : 0,
                    Id = docId,
                    Description = document.Name,
                    Data = document.PDFBytes,
                    IsCertificate = document.DocumentType == DocumentType.SUMMARY
                };

                signedDocuments.Add(doc);
            }

            return signedDocuments.ToArray();
        }

        #endregion
    }
}
