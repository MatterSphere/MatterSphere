using System.Collections.Generic;
using System.Data.SqlClient;

namespace FWBS.OMS.OMSEXPORT.ResetFlagCommands
{
    /// <summary>
    /// Represents a command for reset needExportFlag for matter, <see cref="IResetNeedExportFlagCommand"/> 
    /// </summary>
    internal sealed class ResetNeedExportFlagForMatterCommand : IResetNeedExportFlagCommand
    {
        private IDatabaseProvider database;

        /// <summary>
        /// Gets matter id.
        /// </summary>
        public long Id
        {
            get;
            private set;           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetNeedExportFlagForMatterCommand"/> 
        /// </summary>
        /// <param name="matterId">The matter id.</param>
        /// <param name="dbProvider">The IDatabaseProvider</param>
        public ResetNeedExportFlagForMatterCommand(long matterId, IDatabaseProvider dbProvider)
        {
            database = dbProvider;
            Id = matterId;
        }

        /// <summary>
        /// Resets the need export flag for Matter with id equals to the matterId.
        /// </summary>
        public void Execute()
        {
            List<SqlParameter> parList = new List<SqlParameter>();
            SqlParameter parFILEID = new SqlParameter("@FILEID", Id);
            parList.Add(parFILEID);
            database.ExecuteSQL("UPDATE DBFILE SET FILENEEDEXPORT = 0 WHERE FILEID = @FILEID", parList);                    
        }
    }
}
