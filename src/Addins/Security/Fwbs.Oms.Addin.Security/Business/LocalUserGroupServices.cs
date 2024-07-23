using System;
using System.Collections.Generic;
using System.Data;
using FWBS.OMS.Data;


namespace FWBS.OMS.Addin.Security
{
    public class LocalUserGroupServices
    {
        /// <summary>
        /// Creates or Updates a local group
        /// </summary>
        /// <param name="UserIDs">Semi-colon separated list of User IDs</param>
        /// <param name="GroupID"></param>
        public static void AddUsersToGroup(string UserIDs, string GroupID)
        {
            if (String.IsNullOrWhiteSpace(UserIDs))
                throw new Exception ("User IDs not supplied");
            else if (String.IsNullOrWhiteSpace(GroupID))
                throw new Exception("Group ID not supplied");
            
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("userIDs", UserIDs));
            parList.Add(connection.CreateParameter("groupID", GroupID));
            parList.Add(connection.CreateParameter("adminUserID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID));
            connection.ExecuteProcedure("config.AddUsersToGroup", parList);          
        }


        /// <summary>
        /// Retrieves a list of UserIDs for a local group
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static string GetUserIDs(string GroupID)
        {
            if (String.IsNullOrWhiteSpace(GroupID))
                throw new Exception("Group ID not supplied");

            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("groupID", GroupID));
            parList.Add(connection.CreateParameter("adminUserID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID));
            
            DataTable dt = connection.ExecuteProcedure("config.GetUserIDsForGroup", parList);
            
            if (dt == null)
                return "";
            else
                return dt.Rows[0]["UserIDs"].ToString();
        }
        
    }

}
