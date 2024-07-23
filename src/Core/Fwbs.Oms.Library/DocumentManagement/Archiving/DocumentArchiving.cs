using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FWBS.OMS.Data;

namespace FWBS.OMS.DocumentManagement
{
    public class DocumentArchiving
    {
        public class ExclusionsManager
        {
            #region "Variables"
            private static List<long> _listOfDocIDs = new List<long>();
            public enum ExclusionMode
            { 
                Add,
                Remove
            }
            private ExclusionMode _mode = ExclusionMode.Add;
            private static StringBuilder _stringBuilderDocList = null;
            private static string _docIDString;
            private static ArchivingManager.ArchivingMode _archiveAction;
            #endregion

            #region "Constructors"
            public ExclusionsManager(ExclusionMode mode)
            {
                Clear();
                Mode = mode;    
            }

            public ExclusionsManager() : base()
            {
                Clear();
            }
            #endregion

            #region "Public"
            public ExclusionMode Mode
            {
                get
                {
                    return _mode;
                }
                set
                {
                    _mode = value;
                }
            }

            public ArchivingManager.ArchivingMode ArchivingMode
            {
                get
                {
                    return _archiveAction;
                }
                set
                {
                    _archiveAction = value;
                }
            }

            public void Add(long DocID)
            {
                if (_listOfDocIDs == null || !_listOfDocIDs.Contains(DocID))
                    _listOfDocIDs.Add(DocID);
            }

            public void Clear()
            {
                if (_listOfDocIDs != null)
                    _listOfDocIDs.Clear();
            }

            public void Execute()
            {
                Execute(false);
            }

            public void Execute(bool reset)
            {                
                if (_listOfDocIDs == null || _listOfDocIDs.Count < 1)
                    return;

                if (_mode == ExclusionMode.Add)
                    AddDocumentToArchiveExclusionsTable(FormattedDocumentList);
                else if (_mode == ExclusionMode.Remove)
                    RemoveDocumentFromArchiveExclusionsTable(FormattedDocumentList);
                else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("MSGEXCLMDNTIM", "Exclusion Mode:''%1%'' not yet implemented", "", _mode.ToString()).Text);

                if (reset)
                    Clear();
            }
            #endregion

            #region "Private"

            private static string FormattedDocumentList
            {
                get
                {
                    _stringBuilderDocList = new StringBuilder();

                    foreach (long docID in _listOfDocIDs)
                    {
                        _stringBuilderDocList.Append(docID);
                        _stringBuilderDocList.Append(",");
                    }

                    _docIDString = _stringBuilderDocList.ToString();
                    _stringBuilderDocList = null;
                    return _docIDString;
                }
            }

            private static void AddDocumentToArchiveExclusionsTable(string documentList)
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("Docidlist", documentList));
                parList.Add(connection.CreateParameter("CreatedBy", FWBS.OMS.Session.CurrentSession.CurrentUser.ID));
                parList.Add(connection.CreateParameter("Created", DateTime.UtcNow));
                parList.Add(connection.CreateParameter("ArchiveAction", ArchivingManager.ArchivingModeValue(_archiveAction)));
                connection.ExecuteProcedure("sprAddDocumentToArchiveExclusions", parList);
            }

            private static void RemoveDocumentFromArchiveExclusionsTable(string documentList)
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("Docidlist", documentList));
                parList.Add(connection.CreateParameter("ArchiveAction", ArchivingManager.ArchivingModeValue(_archiveAction)));
                connection.ExecuteProcedure("sprRemoveDocumentfromArchiveExclusions", parList);
            }

            #endregion

        }
    
        public class ArchivingManager
        {
            #region "Variables"

            private static short _archiveDirID = -1;
        
            public enum ArchivingMode
            {
                Archive,
                Delete
            }
            private static ArchivingMode _mode = ArchivingMode.Archive;
            #endregion

            #region "Constructors"
            public ArchivingManager(ArchivingMode mode)
            {
                _mode = mode;
            }

            public ArchivingManager() : base()
            {
                
            }
            #endregion
            
            #region "Public"

            public short ArchiveDirectoryID
            {
                get
                {
                    return _archiveDirID;
                }
                set 
                {
                    _archiveDirID = value;
                }
            }

            public ArchivingMode Mode
            {
                get
                {
                    return _mode;
                }
                set 
                {
                    _mode = value;
                }
            }

            public void Execute()
            {
                if (_mode == ArchivingMode.Archive)
                {
                    if (_archiveDirID < 1)
                        throw new Exception(Session.CurrentSession.Resources.GetMessage("MSGARCHNTVLD", "The Archiving Directory ID (%1%) is not valid", "", _archiveDirID.ToString()).Text);
                }
                
                AddDocumentsToArchiveInfoTable();
            }

            #endregion 
                        
            #region "Private"

            private static void AddDocumentsToArchiveInfoTable()
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();

                parList.Add(connection.CreateParameter("Action", ArchivingModeValue(_mode)));
                parList.Add(connection.CreateParameter("ArchiveDirID", _archiveDirID));
                parList.Add(connection.CreateParameter("RequestedBy", FWBS.OMS.Session.CurrentSession.CurrentUser.ID));
                parList.Add(connection.CreateParameter("Created", DateTime.UtcNow));

                connection.ExecuteProcedure("sprAddDocumentToArchiveInfo", parList);
            }

            #endregion


            #region "Internal"
            
            internal static string ArchivingModeValue(ArchivingMode mode)
            {
                return mode == ArchivingMode.Archive ? "A" : "D";
            }

            #endregion


        }

        public class DeletionPeriodsManager
        {

            /// <summary>
            /// Removes all Deletion Period Entries
            /// </summary>
            /// <returns></returns>
            public bool RemoveAll()
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                connection.ExecuteProcedure("sprRemoveDocumentArchiveDeletionPeriods", null);
                return true;
            }


            /// <summary>
            /// Edits the Deletion Period for the given list of entries
            /// </summary>
            /// <param name="list"></param>
            /// <param name="period"></param>
            /// <returns></returns>
            public bool Edit(string list, short? period)
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("listOfIDs", list));
                parList.Add(connection.CreateParameter("UpdatedBy", Session.CurrentSession.CurrentUser.ID));
                parList.Add(connection.CreateParameter("Updated", DateTime.UtcNow));
                parList.Add(connection.CreateParameter("deletionPeriod", period));
                connection.ExecuteProcedure("sprEditDocumentArchiveDeletionPeriods", parList);
                return true;
            }


            /// <summary>
            /// Generates new Deletion Period entries for File Types &amp; Branches that do not exist
            /// </summary>
            /// <param name="period"></param>
            /// <returns></returns>
            public bool Generate(short? period)
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("deletionPeriod", period));
                parList.Add(connection.CreateParameter("CreatedBy", Session.CurrentSession.CurrentUser.ID));
                parList.Add(connection.CreateParameter("Created", DateTime.UtcNow));
                connection.ExecuteProcedure("sprAddDocumentArchiveDeletionPeriods", parList);
                return true;
            }
        }
    }    
}
