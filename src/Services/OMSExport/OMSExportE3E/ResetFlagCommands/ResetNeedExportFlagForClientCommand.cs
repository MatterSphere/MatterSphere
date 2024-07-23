using System.Collections.Generic;
using System.Data.SqlClient;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("OMSExportE3E.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001001d6d320def528c823076862f317f5d2e40d70f71b8e4b0faf486c7108afc219227157bedcee9cf9d215be30245546c6c380f7472e729ede9cc36cbef9c5e9c04456749acc68a5242a05c0f54cdbb5e8bd20f50e2faf48b185afbd67c120daef17b22e6c91a86d37e18e2cd2e70c9dd648ee4fbcc4b48644663746ef80849e5b1")]
namespace FWBS.OMS.OMSEXPORT.ResetFlagCommands
{
    /// <summary>
    /// Represents a command for reset needExportFlag for client, <see cref="IResetNeedExportFlagCommand"/> 
    /// </summary>
    internal class ResetNeedExportFlagForClientCommand : IResetNeedExportFlagCommand
    {

        private IDatabaseProvider database;

        /// <summary>
        /// Gets client id.
        /// </summary>
        public long Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetNeedExportFlagForClientCommand"/> 
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="dbProvider">The IDatabaseProvider.</param>
        public ResetNeedExportFlagForClientCommand(long clientId, IDatabaseProvider dbProvider)
        {
            Id = clientId;
            database = dbProvider;
        }

        /// <summary>
        /// Resets the need export flag for Client with id equals to the clientId.
        /// </summary>
        public void Execute()
        {
            var parList = new List<SqlParameter>();
            SqlParameter parCLID = new SqlParameter("@CLID", Id);
            parList.Add(parCLID);
            database.ExecuteSQL("UPDATE DBCLIENT SET CLNEEDEXPORT = 0 WHERE CLID = @CLID", parList);
        }
    }
}
