using System;
using System.Collections.Generic;
using System.Data;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.StatusManagement
{
    internal class FileStatusList
    {
        #region "Variables"

        private string _status;
        private DataTable _fileStatusList;

        #endregion


        #region "Internal"

        internal FileStatusList(string status) 
            : base()
        {
            _status = status;
            GetFileStatusList();
        }


        internal DataTable FileStatusRecord()
        {
            return _fileStatusList;
        }


        internal string StatusCode
        {
            get
            {
                return Convert.ToString(_fileStatusList.Rows[0]["fsCode"]);
            }
        }

        internal string AccountCode
        {
            get
            {
                return Convert.ToString(_fileStatusList.Rows[0]["fsAccCode"]);
            }
        }

        internal Alert.AlertStatus AlertLevel
        {
            get
            {
                int val = ConvertDef.ToInt16(_fileStatusList.Rows[0]["fsAlertLevel"], -1);
                return (Alert.AlertStatus)val;
            }
        }

        internal bool AppointmentCreation
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusList.Rows[0]["fsAppCreation"], true));
            }
        }

        internal bool AssociateCreation
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusList.Rows[0]["fsAssocCreation"], true));
            }
        }

        internal bool TaskflowProcessing
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusList.Rows[0]["fsTaskflowEdit"], true));
            }
        }

        internal bool TimeEntry
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusList.Rows[0]["fsTimeEntry"], true));
            }
        }

        internal bool DocumentModification
        {
            get
            {
                return (ConvertDef.ToBoolean(_fileStatusList.Rows[0]["fsDocModification"], true));
            }
        }

        internal bool FileStatusListIsNullOrEmpty()
        {
            return (_fileStatusList == null || _fileStatusList.Rows.Count == 0);
        }

        #region "Private"

        private void GetFileStatusList()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("STATUS", _status));
            _fileStatusList = connection.ExecuteProcedure("sprFileStatusList", parList);
        }

        #endregion


        #endregion

    }
}
