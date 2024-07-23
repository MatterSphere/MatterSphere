using System;
using System.Collections.Generic;
using System.Data;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.StatusManagement
{
    public class FileStatusManager
    {
        #region "Variables"

        private DataTable _fileStatusTable = null;
        private long _fileID;
        

        #endregion
        
        #region "Constructors"

        public FileStatusManager(long fileID)
            : base()
        {
            _fileID = fileID;
            GetFileStatusData();
            TimeOut = DateTime.Now.AddMinutes(1);
        }

        #endregion
        
        #region "Private"

        private void GetFileStatusData()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("FILEID", _fileID));
            _fileStatusTable = connection.ExecuteProcedure("sprFileStatusRecord", parList);
        }


        internal DateTime TimeOut { get; set; }
        #endregion

        #region "Public"

        public long FileID
        {
            get { return _fileID; }
        }

        public DataTable FileStatusRecord()
        {
            return _fileStatusTable;
        }
                
        public string StatusCode
        {
            get
            {
                return Convert.ToString(_fileStatusTable.Rows[0]["fileStatus"]);
            }
        }

        public string AccountCode
        {
            get
            {
                return Convert.ToString(_fileStatusTable.Rows[0]["fsAccCode"]);
            }
        }
        
        public Alert.AlertStatus AlertLevel
        {
            get
            {
                int val = Convert.ToInt16(_fileStatusTable.Rows[0]["fsAlertLevel"]);
                return (Alert.AlertStatus)val;
            }
        }
                
        public bool AppointmentCreation
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusTable.Rows[0]["fsAppCreation"], true));
            }
        }
        
        public bool AssociateCreation
        {
            get
            {
                if (_fileID == 0)
                    return true;
                else
                    return (ConvertDef.ToBoolean(_fileStatusTable.Rows[0]["fsAssocCreation"], true));
            }
        }
        
        public bool TaskflowProcessing
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusTable.Rows[0]["fsTaskflowEdit"], true));
            }
        }
        
        public bool TimeEntry
        {
            get
            {
                ClientStatusManager csManager = new ClientStatusManager(FWBS.OMS.OMSFile.GetFile(_fileID).ClientID);

                if (csManager.TimeEntry == false)
                    return false;
                else
                    return (ConvertDef.ToBoolean(_fileStatusTable.Rows[0]["fsTimeEntry"], true));
            }
        }


        public bool DocumentModification
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusTable.Rows[0]["fsDocModification"], true));
            }
        }


        #endregion

    }
}
