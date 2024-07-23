using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using FWBS.OMS.Extensibility;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Providers;

namespace FWBS.OMS.HighQ
{
    public class HighQ : OMSExtensibility, IHighQ
    {
        private const string CLIENT_ID = "HQ_CLNTID";
        private const string CLIENT_SECRET = "HQ_SCRT";
        private const string SITE_NAME = "HQ_SITE";
        private const string REDIRECT_URI = "HQ_RDRURI";
        private const string OMS_FILE_COLUMN = "HQ_FILECLM";
        private const string CLIENT_COLUMN = "HQ_CLNCLM";
        private const string FOLDER_COLUMN = "HQ_FLDRCLM";

        private static readonly object locker = new object();

        private readonly int _msUserId;
        private readonly int _apiUserId;
        private readonly ITargetFolderPicker _targetFolderPicker;
        private readonly IHighQProvider _hqProvider;
        private readonly IDbProvider _dbProvider;
        private readonly ISessionProvider _sessionProvider;
        private bool _hasTokens;

        public HighQ() : this(new HighQProvider(), new DbProvider(), new SessionProvider(), new TargetFolderPicker())
        {
        }

        internal HighQ(IHighQProvider hqProvider, IDbProvider dbProvider, ISessionProvider sessionProvider, ITargetFolderPicker targetFolderPicker)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            _sessionProvider = sessionProvider;
            _msUserId = _sessionProvider.GetUserId();

            _targetFolderPicker = targetFolderPicker;
            _dbProvider = dbProvider;
            _hqProvider = hqProvider;
            _hqProvider.TokensUpdated += OnTokensUpdated;

            Int32.TryParse(_sessionProvider.GetSpecificData(CLIENT_ID), out _apiUserId);
        }

        private void OnTokensUpdated(object sender, TokensEventArgs e)
        {
            _dbProvider.SetUserTokens(
                _msUserId,
                e.AccessToken,
                e.RefreshToken,
                e.AccessTokenExpiresAt,
                e.RefreshTokenExpiresAt);
        }

        #region Private methods

        private DataTable GetISheetsCollection(int iSheetId)
        {
            Validate();

            return _hqProvider.GetISheetsDataTable(iSheetId);
        }
        
        private void Validate()
        {
            lock (locker)
            {
                if (_hasTokens)
                {
                    return;
                }

                var userTokens = _dbProvider.GetUserTokens(_msUserId);
                _hqProvider.Build(
                    clientId: _apiUserId,
                    secret: _sessionProvider.GetSpecificData(CLIENT_SECRET),
                    site: _sessionProvider.GetSpecificData(SITE_NAME).TrimEnd('/'),
                    redirectUri: _sessionProvider.GetSpecificData(REDIRECT_URI),
                    tokenDetails: userTokens);
                _hasTokens = true;
            }
        }

        private bool Upload(long docId, int? targetFolderId = null)
        {
            var documentInfo = _dbProvider.GetDocumentInfo(docId);
            var folderId = targetFolderId ??
                           GetMatterFolderId(documentInfo.ClientId, documentInfo.ClientNo, documentInfo.OmsFileNo);
            var documentDetails = new DocumentDetails(docId, documentInfo.FullPath, documentInfo.Description, folderId);
            var hqFileId = _hqProvider.UploadDocument(documentDetails);
            if (hqFileId > 0)
            {
                _dbProvider.LogDocumentUpload(docId, _msUserId, hqFileId);
                return true;
            }

            return false;
        }

        private int GetDocumentFolderId(long docId)
        {
            var documentInfo = _dbProvider.GetDocumentInfo(docId);

            return GetMatterFolderId(documentInfo.ClientId, documentInfo.ClientNo, documentInfo.OmsFileNo);
        }

        private int GetMatterFolderId(long clientId, string clientNo, string omsFileNo)
        {
            var iSheetId = _dbProvider.GetOMSFileEntityId(clientId);
            var folderDetails = new FolderDetails(iSheetId, clientNo, omsFileNo,
                _sessionProvider.GetSpecificData(CLIENT_COLUMN), _sessionProvider.GetSpecificData(OMS_FILE_COLUMN),
                _sessionProvider.GetSpecificData(FOLDER_COLUMN));

            return _hqProvider.GetFolderId(folderDetails);
        }
        
        #endregion

        #region IHighQ

        public DataTable GetMatters(long clientId)
        {
            var iSheetId = _dbProvider.GetOMSFileEntityId(clientId);

            return GetISheetsCollection(iSheetId);
        }

        public IDictionary<long, Exception> UploadDocuments(long[] docIds, int? targetFolderId = null)
        {
            Validate();

            var results = new Dictionary<long, Exception>();
            foreach (var docId in docIds)
            {
                results.Add(docId, null);
            }

            var tasksInfo = new Dictionary<int, long>();
            System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[docIds.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                int index = i;
                tasks[index] = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    Upload(docIds[index], targetFolderId);
                });

                tasksInfo.Add(tasks[index].Id, docIds[index]);
            }

            try
            {
                System.Threading.Tasks.Task.WaitAll(tasks);
            }
            catch (AggregateException)
            {
                foreach (var task in tasks)
                {
                    var docId = tasksInfo[task.Id];
                    if (task.Status == TaskStatus.Faulted)
                    {
                        results[docId] = task.Exception?.InnerException;
                    }
                }
            }

            return results;
        }

        public int GetRootFolderId(long docId)
        {
            Validate();

            return GetDocumentFolderId(docId);
        }

        public int GetTargetFolderId(int rootFolderId, IWin32Window owner = null)
        {
            Validate();
            
            var rootFolder = _hqProvider.GetFolderInfo(rootFolderId);
            var folders = _hqProvider.GetFolders(rootFolderId);

            return _targetFolderPicker.GetTargetFolderId(rootFolder, folders, _hqProvider, owner);
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_hqProvider != null)
                {
                    _hqProvider.TokensUpdated -= OnTokensUpdated;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
