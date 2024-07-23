using System;
using System.Collections.Generic;
using System.Data;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.StatusManagement
{
    public class ClientStatusManager
    {
        #region "Variables"

        private DataTable _clientStatusTable = null;
        private long _clID;

        #endregion
        
        #region "Constructors"

        public ClientStatusManager(long clID)
            : base()
        {
            _clID = clID;
            GetClientStatusData();
        }

        #endregion
        
        #region "Private"

        private void GetClientStatusData()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("CLID", _clID));
            _clientStatusTable = connection.ExecuteProcedure("sprClientStatusRecord", parList);
        }

        #endregion
        
        #region "Public"

        public DataTable ClientStatusRecord()
        {
            return _clientStatusTable;
        }

        public bool TimeEntry
        {
            get
            {
                return (ConvertDef.ToBoolean(_clientStatusTable.Rows[0]["csTimeEntry"], true));
            }
        }

        public bool FileCreation
        {
            get
            {
                return (ConvertDef.ToBoolean(_clientStatusTable.Rows[0]["csFileCreation"], true));
            }
        }

        public string StatusCode
        {
            get
            {
                return Convert.ToString(_clientStatusTable.Rows[0]["clStatus"]);
            }
        }

        public string AccountCode
        {
            get
            {
                return Convert.ToString(_clientStatusTable.Rows[0]["csAccCode"]);
            }
        }

        #endregion

    }
}
