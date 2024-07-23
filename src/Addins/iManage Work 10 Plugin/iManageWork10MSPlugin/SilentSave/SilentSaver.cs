using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using iManage.IWInterfaces;
using iManage.Work.Tools;
using iManageWork10.Shell.Exceptions;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;

namespace MatterSphereIntercept.SilentSave
{
    public class SilentSaver
    {

        private const int TimerDelayInSeconds = 2;

        private readonly RestApiClient _restApiClient;

        private readonly IPlugInHostLog _log;

        private readonly Dictionary<string, Tuple<IWDocumentProfile, SilentSaveThreadStateInfo>> _openedDocuments = new Dictionary<string, Tuple<IWDocumentProfile, SilentSaveThreadStateInfo>>();

        public SilentSaver(RestApiClient restApiClient, IPlugInHostLog plugInHostLog)
        {
            _restApiClient = restApiClient;
            _log = plugInHostLog;
        }

        public int TimerTickInSeconds { get; set; } = 10;

        public void StartWorkItem(object document, IWDocumentProfile documentProfile)
        {
            SilentSaveThreadStateInfo silentSaveThreadStateInfo = new SilentSaveThreadStateInfo(document, documentProfile);

            lock (_openedDocuments)
            {
                var documentId = documentProfile.DocumentID;
                var containsKey = _openedDocuments.ContainsKey(documentId);

                if (containsKey)
                {
                    Tuple<IWDocumentProfile, SilentSaveThreadStateInfo> metadata;
                    if (_openedDocuments.TryGetValue(documentId, out metadata))
                    {
                        metadata.Item2.AutoEvent.Set(_log, documentId, "StartWorkItem::OnDocumentOpen");
                    }

                    _openedDocuments.Remove(documentId);
                    metadata.Item2.Dispose();
                }

                var docMetadata = new Tuple<IWDocumentProfile, SilentSaveThreadStateInfo>(documentProfile, silentSaveThreadStateInfo);
                _openedDocuments.Add(documentId, docMetadata);
            }
            ThreadPool.QueueUserWorkItem(ThreadProc, silentSaveThreadStateInfo);
        }

        public void CompleteWorkItem(IWDocumentProfile documentProfile)
        {
            if (documentProfile == null)
            {
                return;
            }
            CompleteSilentSaveItem(documentProfile.DocumentID);
        }

        public void Shutdown()
        {
            string[] docIds;
            lock (_openedDocuments)
            {
                docIds = new string[_openedDocuments.Keys.Count];
                _openedDocuments.Keys.ToList().CopyTo(docIds);
            }
            docIds
                .ToList()
                .ForEach(CompleteSilentSaveItem);
        }

        private void CompleteSilentSaveItem(string docId)
        {
            lock (_openedDocuments)
            {
                Tuple<IWDocumentProfile, SilentSaveThreadStateInfo> metadata;
                if (_openedDocuments.TryGetValue(docId, out metadata))
                {
                    metadata.Item2.AutoEvent.Set(_log, docId, "CompleteSilentSaveItem::Success");
                    try
                    {
                        SaveDocumentSilently(metadata.Item2);
                    }
                    catch (COMException ex)
                    {
                        _log.Error($"Final save error={ex.HResult}. {ex.Message}");
                    }

                    _openedDocuments.Remove(docId);
                    metadata.Item2.Dispose();
                }
            }
        }

        private void ThreadProc(object stateInfo)
        {
            var stateInfoObject = stateInfo as SilentSaveThreadStateInfo;
            var autoEvent = stateInfoObject?.AutoEvent;
            using (var timer = new Timer(TimerCallback, stateInfo, TimerDelayInSeconds * 1000, TimerTickInSeconds * 1000))
            {
                autoEvent?.WaitOne();
            }
            _log.Info($"Silent save thread is completing for document {stateInfoObject?.DocumentProfile.DocumentID} ...");
        }

        private void TimerCallback(object state)
        {
            var stateInfo = state as SilentSaveThreadStateInfo;
            if (stateInfo != null)
            {
                SaveDocumentSilently(stateInfo);
            }
        }

        private void SaveDocumentSilently(SilentSaveThreadStateInfo stateInfo)
        {
            var profile = stateInfo.DocumentProfile;
            var autoEvent = stateInfo.AutoEvent;

            if (profile == null)
            {
                autoEvent?.Set();
                return;
            }

            lock (profile)
            {
                string fullName;
                try
                {
                    fullName = stateInfo.Document.FullName;
                }
                catch (COMException ex)
                {
                    LogDebugMessage(ex.Message, ex);
                    return;
                }
                catch (InvalidComObjectException ex)
                {
                    LogDebugMessage(ex.Message, ex);
                    return;
                }
                if (fullName == null)
                {
                    return;
                }

                var documentId = profile.DocumentID;
                var accessRights = profile.EffectiveRights;

                if (accessRights == WSAccessRights.none || accessRights == WSAccessRights.read)
                {
                    _log.Warn($"Could not process silent save for DocId={documentId}. Not enough access rights ({accessRights}) to execute operation.");
                    autoEvent?.Set(_log, documentId, "SilentSaveTimer::FileAccessRestriction.");
                    return;
                }
                try
                {
                    if (!stateInfo.Document.Saved)
                    {
                        RetrieveDocumentCheckedOutData(stateInfo);
                        if (stateInfo.DocumentCheckOut != null)
                        {
                            if (!fullName.Equals(stateInfo.DocumentCheckOut.Path, StringComparison.OrdinalIgnoreCase))
                            {
                                LogDebugMessage($"Skip silent save for {stateInfo.DocumentProfile.DocumentID} document. Name is : {stateInfo.Document.Name}.");
                                return;
                            }
                            try
                            {
                                DeleteDocumentLock(stateInfo);
                            }
                            catch (Exception e)
                            {
                                _log.Warn(e.Message);
                                return;
                            }
                        }

                        _log.Info($"Document {documentId} silent save begins. ");
                        dynamic doc = stateInfo.Document;
                        doc.Save();

                        if (UpdateFile(profile, stateInfo))
                        {
                            _log.Info($"Document {documentId} silent save completed. ");
                        }

                        if (stateInfo.DocumentCheckOut != null)
                        {
                            CheckOutDocument(stateInfo);
                        }
                    }
                }
                catch (COMException ex)
                {
                    if (ex.HResult != -2147417846)
                    {
                        _log.Error($"COM Exception: {ex.HResult}. {ex.Message}");
                        autoEvent?.Set(_log, documentId, "SilentSaveTimer::COM exception on file save.");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
            }
        }

        private void RetrieveDocumentCheckedOutData(SilentSaveThreadStateInfo stateInfo)
        {
            DocumentsManagement documentsManagement = new DocumentsManagement(_restApiClient);
            try
            {
                stateInfo.DocumentCheckOut = documentsManagement.GetDocumentCheckOut(stateInfo.DocumentProfile.DocumentID, stateInfo.DocumentProfile.DocumentLibrary);
            }
            catch (HttpException exception)
            {
                if (exception.GetHttpCode() != 404)
                {
                    _log.Warn($"Error({exception.ErrorCode}). Can't get checkout data for DocId={stateInfo.DocumentProfile.DocumentID}.");
                    stateInfo.AutoEvent?.Set(_log, stateInfo.DocumentProfile.DocumentID, "SilentSaveTimer::CheckOutDocumentError.");
                    throw;
                }

                stateInfo.DocumentCheckOut = null;
            }
        }

        private void DeleteDocumentLock(SilentSaveThreadStateInfo stateInfo)
        {
            if (stateInfo.DocumentCheckOut.InUseBy != null)
            {
                DocumentsManagement documentsManagement = new DocumentsManagement(_restApiClient);
                UserManagement userManagement = new UserManagement(_restApiClient);
                var currentUserId = userManagement.GetCurrentUserProfile().Id;
                if (stateInfo.DocumentCheckOut.InUseBy.Equals(currentUserId))
                {
                    documentsManagement.DeleteDocumentLock(stateInfo.DocumentProfile.DocumentID, stateInfo.DocumentProfile.DocumentLibrary);
                    _log.Info($"Document checkout data deleted for {stateInfo.DocumentProfile.DocumentID}. ");
                }
                else
                {
                    stateInfo.AutoEvent?.Set(_log, stateInfo.DocumentProfile.DocumentID, "SilentSaveTimer::DocumentCheckedOutByOtherUser.");
                    throw new CheckoutDocumentException($"The file '{stateInfo.DocumentProfile.DocumentID}' is used by {stateInfo.DocumentCheckOut.InUseBy}. Logged on user is {currentUserId}. Stopping silent save...");
                }
            }
        }
        private void CheckOutDocument(SilentSaveThreadStateInfo stateInfo)
        {
            DocumentsManagement documentsManagement = new DocumentsManagement(_restApiClient);
            documentsManagement.PostDocumentLock(stateInfo.DocumentProfile.DocumentID, new PostDocumentLockProperties
            {
                Comments = stateInfo.DocumentCheckOut.Comments,
                DueDate = stateInfo.DocumentCheckOut.DueDate,
                Location = stateInfo.DocumentCheckOut.Location,
                Path = stateInfo.DocumentCheckOut.Path
            }, stateInfo.DocumentProfile.DocumentLibrary);
            _log.Info($"Document {stateInfo.DocumentProfile.DocumentID} checked out.");
        }

        private bool UpdateFile(IWDocumentProfile profile, SilentSaveThreadStateInfo stateInfo)
        {
            var documentId = profile.DocumentID;
            _log.Info($"Start updating document. Id={documentId}");

            DocumentsManagement documentsManagement = new DocumentsManagement(_restApiClient);

            var path = stateInfo.DocumentCheckOut.Path;
            _log.Info($"Document path is '{path}'.");

            var tmp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                File.Copy(path, tmp);

                var size = new FileInfo(tmp).Length;
                _log.Info($"Document size is {size} bytes.");

                documentsManagement.ReplaceDocumentContent(documentId, tmp, profile.DocumentLibrary);
            }
            catch (System.IO.IOException ex)
            {
                _log.Error(ex.Message);
                return false;
            }
            finally
            {
                DeleteFile(tmp);
            }

            return true;
        }

        public void SaveNewDocument(DocumentProfile documentProfile, string folderId, dynamic document, string database)
        {
            var savedFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                document.SaveAs(savedFilePath);
                var tmpCopyToUpload = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                try
                {
                    File.Copy(savedFilePath, tmpCopyToUpload);
                    FileInfo fileInfo = new FileInfo(tmpCopyToUpload);
                    documentProfile.Size = (int)fileInfo.Length;

                    FoldersManagement foldersManagement = new FoldersManagement(_restApiClient);
                    var result = foldersManagement.PostFolderDocument(folderId, documentProfile, tmpCopyToUpload, database);
                    _log.Info($"New file saved. {result}");
                }
                finally
                {
                    DeleteFile(tmpCopyToUpload);
                }
            }
            finally
            {
                DeleteFile(savedFilePath);
            }
        }

        private void DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _log.Error($"Temporary file '{filePath}' is not deleted. Error={ex.Message}.");
            }
            finally
            {
                if (File.Exists(filePath) == false)
                {
                    _log.Info($"Temporary file '{filePath}' is deleted.");
                }
            }
        }
        
        [Conditional("DEBUG")]
        private void LogDebugMessage(string message, Exception ex = null)
        {
            _log.Debug(message, ex);
        }

    }
}
