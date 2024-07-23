using System.Collections.Generic;
using System.Data.SqlClient;

namespace FWBS.OMS.OMSEXPORT.ResetFlagCommands
{
    /// <summary>
    /// Represents a command for reset needExportFlag for contact, <see cref="IResetNeedExportFlagCommand"/> .
    /// </summary>
    internal sealed class ResetNeedExportFlagForContactCommand : IResetNeedExportFlagCommand
    {

        private IDatabaseProvider database;

        /// <summary>
        /// Gets contact id.
        /// </summary>
        public long Id
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetNeedExportFlagForContactCommand"/> 
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <param name="dbProvider">The IDatabaseProvider</param>
        public ResetNeedExportFlagForContactCommand(long contactId, IDatabaseProvider dbProvider)
        {
            Id = contactId;
            database = dbProvider;
        }

        /// <summary>
        /// Resets the  need export flag for Contact with id equals to the contactId
        /// </summary>
        public void Execute()
        {
            List<SqlParameter> parList = new List<SqlParameter>();
            parList.Add(new SqlParameter("@CONTID", Id));
            database.ExecuteSQL("UPDATE DBCONTACT SET CONTNEEDEXPORT = 0 WHERE CONTID = @CONTID", parList);
        }
    }
}
