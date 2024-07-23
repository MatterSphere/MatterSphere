using System;
using System.Collections.Generic;
using System.Linq;
using FWBS.OMS.Commands;
using FWBS.OMS.HighQ;

namespace FWBS.OMS.DocumentManagement.Commands
{
    public class HighQDocumentUploadCommand : FWBS.OMS.Commands.Command
    {
        private readonly IHighQ _highQ;

        public HighQDocumentUploadCommand(IHighQ highQ)
        {
            _highQ = highQ;
        }

        public List<OMSDocument> Documents { get; set; }
        public int TargetFolderId { get; set; }

        public override ExecuteResult Execute()
        {
            var executeResult = new FWBS.OMS.Commands.ExecuteResult()
            {
                Status = FWBS.OMS.Commands.CommandStatus.Success
            };

            try
            {
                if (_highQ == null)
                {
                    return SetFailedStatus(executeResult, "DOCUPLOADED", "HighQ Addin Not Found.");
                }

                if (Documents == null)
                {
                    return SetFailedStatus(executeResult, "DOCNOTSELECTED", "There is no any documents were selected");
                }

                if (Documents.Count > 0)
                {
                    var results = _highQ.UploadDocuments(Documents.Select(doc => doc.ID).ToArray(), TargetFolderId);
                    foreach (var result in results)
                    {
                        if (result.Value == null)
                        {
                            var document = Documents.First(doc => doc.ID == result.Key);
                            document.AddActivity("HIGHQ", "UPLOADED");
                        }
                        else
                        {
                            executeResult.Status = FWBS.OMS.Commands.CommandStatus.Failed;
                            executeResult.Errors.Add(result.Value);
                        }
                    }
                }
            }
            catch (AggregateException ex)
            {
                executeResult.Status = FWBS.OMS.Commands.CommandStatus.Failed;
                executeResult.Errors.AddRange(ex.InnerExceptions);
            }
            catch (Exception ex)
            {
                executeResult.Status = FWBS.OMS.Commands.CommandStatus.Failed;
                executeResult.Errors.Add(ex);
            }

            return executeResult;
        }

        private ExecuteResult SetFailedStatus(ExecuteResult result, string code, string defaultText)
        {
            result.Status = FWBS.OMS.Commands.CommandStatus.Failed;
            var errorMessage = FWBS.OMS.Session.CurrentSession.Resources
                .GetMessage(code, defaultText, "").Text;
            result.Errors.Add(new Exception(errorMessage));

            return result;
        }
    }
}
