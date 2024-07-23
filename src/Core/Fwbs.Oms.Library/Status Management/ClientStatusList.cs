using System;
using System.Collections.Generic;
using System.Data;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.StatusManagement
{
    internal class ClientStatusList
    {
        #region "Variables"
        
        private string _status;
        private DataTable _clientStatusList;

        #endregion
        

        #region "Internal"
        
        internal ClientStatusList(string status) 
            : base()
        {
            _status = status;
            GetClientStatusList();
        }
        
        internal DataTable ClientStatusRecord()
        {
            return _clientStatusList;
        }

        internal bool TimeEntry
        {
            get
            {
                return (ConvertDef.ToBoolean(_clientStatusList.Rows[0]["csTimeEntry"], true));
            }
        }

        internal bool FileCreation
        {
            get
            {
                return (ConvertDef.ToBoolean(_clientStatusList.Rows[0]["csFileCreation"], true));
            }
        }

        internal string StatusCode
        {
            get
            {
                return Convert.ToString(_clientStatusList.Rows[0]["csCode"]);
            }
        }

        internal string AccountCode
        {
            get
            {
                return Convert.ToString(_clientStatusList.Rows[0]["csAccCode"]);
            }
        }

        
        #endregion

        internal bool ClientStatusListIsNullOrEmpty()
        {
            return (_clientStatusList == null || _clientStatusList.Rows.Count == 0);
        }

        #region "Private"


        private void GetClientStatusList()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("STATUS", _status));
            _clientStatusList = connection.ExecuteProcedure("sprClientStatusList", parList);
        }
        

        #endregion

    }

}
