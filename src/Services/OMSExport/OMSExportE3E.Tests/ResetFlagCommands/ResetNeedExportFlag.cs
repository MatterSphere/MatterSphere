using System.Collections.Generic;
using System.Data.SqlClient;
using FWBS.OMS.OMSEXPORT;
using FWBS.OMS.OMSEXPORT.ResetFlagCommands;

namespace OMSExportE3E.Tests.ResetFlagCommands
{
    public class ResetNeedExportFlag
    {      
        protected IResetNeedExportFlagCommand Command
        {
            get;
            set;
        }

        protected IDatabaseProvider Provider
        {
            get;
            set;
        }
    }
}