using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MCEPGlobalClasses
{
    public class MCEPAdminRoutines
    {
        private MCEPGlobalClasses.MCEPDatabase mcepDB;

        private void GenerateClassReferences()
        {
            if (mcepDB == null) mcepDB = new MCEPDatabase();
        }
        private void DisposeOfClassReferences()
        {
            if (mcepDB != null) mcepDB = null;
        }

        public void ImportMissingUsers(string DefaultRootFolderName)
        {
            GenerateClassReferences();
            DataTable userList = GetUserListFromMatterSphere();
            foreach (DataRow userRow in userList.Rows)
            {
                mcepDB.AddUserRow(userRow, DefaultRootFolderName);
            }
            DisposeOfClassReferences();
        }

        private DataTable GetUserListFromMatterSphere()
        {
            DataTable userList = new DataTable();
            SqlConnectionStringBuilder MatterSphereDatabaseString = new SqlConnectionStringBuilder();
            MatterSphereDatabaseString.DataSource = MCEPConfiguration.GetConfigurationItem("MatterSphereServer");
            MatterSphereDatabaseString.InitialCatalog = MCEPConfiguration.GetConfigurationItem("MatterSphereDatabase");
            if (MCEPConfiguration.GetConfigurationItem("MatterSphereLoginType") == "AAD")
                MatterSphereDatabaseString.Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
            else
                MatterSphereDatabaseString.IntegratedSecurity = true;
            using (SqlConnection matterSphereConnection = new SqlConnection(MatterSphereDatabaseString.ToString()))
            {

                using (SqlCommand userCommand = new SqlCommand())
                {
                    userCommand.Connection = matterSphereConnection;
                    userCommand.CommandType = CommandType.Text;
                    userCommand.CommandText = "Select UsrID, usrEmail from DBUser where usrEMail like '%@%' and usrActive = 1";
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(userCommand))
                    {
                        sqlDataAdapter.Fill(userList);
                    }
                }
            }
            return userList;
        }

        public List<MCEPUser> GetUsers()
        {
            GenerateClassReferences();
            try
            {
                DataTable userTable = mcepDB.UsersFromDatabaseADMIN();
                if (userTable == null)
                {
                    return null;
                }
                List<MCEPUser> users = new List<MCEPUser>();
                foreach (DataRow row in userTable.Rows)
                {
                    users.Add(new MCEPUser(row));
                }
                userTable = null;
                return users;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                DisposeOfClassReferences();
            }
        }

        public void UpdateUserRecord(MCEPUser user)
        {
            GenerateClassReferences();
            mcepDB.UpdateUserRow(user);
            DisposeOfClassReferences();
        }
    }
}
