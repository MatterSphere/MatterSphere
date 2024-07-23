using System;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Xml;

namespace MsAdUsersSyncService.MSADSync
{
    class MatterSphereDB
    {
        private readonly SqlConnectionStringBuilder _msadString;
        private const int ExceededTopUsersReturnCount = 10;

        public MatterSphereDB()
        {
            _msadString = new SqlConnectionStringBuilder
            {
                DataSource = Config.GetConfigurationItem("MatterSphereServer"),
                InitialCatalog = Config.GetConfigurationItem("MatterSphereDatabase")
            };

            IsAAD = Config.GetConfigurationItem("MatterSphereLoginType") == "AAD";


            if (IsAAD)
                _msadString.Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
            else
                _msadString.IntegratedSecurity = true;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_msadString.ToString());
        }

        internal DataTable GetGroupsToSync()
        {
            DataTable emailList = new DataTable();
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Item.[Group] WHERE ISNULL(ADDistinguishedName, '') <> ''";
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    adp.Fill(emailList);
                }
            }
            return emailList;
        }

        public bool IsAAD { get; private set; }

        public string NtDomainName
        {
            get
            {
                using (DirectoryEntry root = new DirectoryEntry())
                {
                    var dcProps = root.Properties["dc"];
                    return dcProps[0].ToString() + "\\";
                }
            }
        }

        public string ControllerDomainName
        {
            get
            {
                string ldap;
                using (DirectoryEntry root = new DirectoryEntry())
                {
                    ldap = root.Path;
                    if (String.IsNullOrEmpty(ldap))
                        ldap = root.Parent.Path;
                }
                if (!String.IsNullOrEmpty(ldap))
                {
                    var domainLdap = ldap.Substring(ldap.IndexOf("DC="));
                    var domainName = domainLdap.Replace("DC=", "");
                    domainName = domainName.Replace(",", ".");
                    return "@" + domainName;
                }
                throw new NotSupportedException($"The LDAP path is invalid: {ldap}");
            }
        }

        public string MakeUpnUserName(ResultPropertyCollection props, string suffix)
        {
            if (suffix.StartsWith("@"))
            {
                return (string)props["userPrincipalName"][0];
            }
            throw new ArgumentException($"The parameter has invalid value = {suffix}", nameof(suffix));
        }

        public string MakeNtUserName(ResultPropertyCollection props, string prefix)
        {
            if (prefix.EndsWith("\\"))
            {
                return prefix + props["sAMAccountName"][0];
            }
            throw new ArgumentException($"The parameter has invalid value = {prefix}", nameof(prefix));
        }

        public void SyncUsers(Func<ResultPropertyCollection, string, string> makeNameFunc, string userDomainName)
        {
            string xml;

            GlobalCatalog gc = Forest.GetCurrentForest().FindGlobalCatalog();
            DirectorySearcher search = gc.GetDirectorySearcher();

            search.Filter = $"(&(objectCategory=Person)(objectClass=User){(IsAAD ? "(userPrincipalName=*)" : string.Empty)})";
            search.PageSize = 500;
            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("userAccountControl");
            search.PropertiesToLoad.Add("userPrincipalName");
            SearchResultCollection col = search.FindAll();

            if (col.Count == 0)
            {
                col.Dispose();
                search.Dispose();
                return;
            }

            using (DataTable dt = new DataTable("users") { MinimumCapacity = col.Count })
            {
                dt.Columns.Add(new DataColumn("userNTName", typeof(System.String)));
                dt.Columns.Add(new DataColumn("userName", typeof(System.String)));
                dt.Columns.Add(new DataColumn("active", typeof(System.Int32)));

                foreach (SearchResult srt in col)
                {
                    dt.Rows.Add(makeNameFunc(srt.Properties, userDomainName), srt.Properties["sAMAccountName"][0], srt.Properties["userAccountControl"][0]);
                }

                dt.AcceptChanges();
                col.Dispose();
                search.Dispose();

                using (System.IO.StringWriter stgW = new System.IO.StringWriter())
                {
                    XmlTextWriter xmlTW = new XmlTextWriter(stgW);
                    dt.WriteXml(xmlTW, XmlWriteMode.IgnoreSchema, true);
                    xmlTW.Flush();
                    xml = stgW.ToString();
                }
            }

            // Now pass the user collection to the stored procedure in the database

            using (SqlConnection conn = CreateConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.ADSyncUsers";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("userXML", xml);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        internal void CreateUsers(string xmlUsers, Guid defaultPolicyID, Guid ID)
        {
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.ADSyncGroupUsers";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@defaultPolicyID", defaultPolicyID.ToString());
                cmd.Parameters.AddWithValue("@groupID", ID.ToString());
                cmd.Parameters.AddWithValue("@userXML", xmlUsers);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        internal void OverrideDomainName(ref string strRoot, DirectoryEntry root)
        {
            using (DataTable dts = new DataTable())
            {
                using (SqlConnection conn = CreateConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT SD.spData FROM dbo.dbRegInfo RI JOIN dbo.dbSpecificData SD ON SD.brID = RI.brID WHERE spLookup = 'ADSECDOMAIN'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        da.Fill(dts);
                    }
                }

                if (dts.Rows.Count == 1)
                {
                    strRoot = dts.Rows[0][0].ToString() + "\\";
                }
                else
                {
                    strRoot = root.Properties["dc"][0].ToString() + "\\";
                }
            }
        }

        internal Guid CreateGroup(string distName, Guid defaultPolicyID, string strRoot, string cnName)
        {
            // Check to see if that group exists in OMS and if it does not create it
            Guid ID = Guid.NewGuid();

            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT ID FROM [item].[Group] WHERE ADDistinguishedName = '" + distName + "'";
                using (SqlDataAdapter adt = new SqlDataAdapter(cmd.CommandText, conn))
                {
                    using (DataTable dat = new DataTable())
                    {
                        conn.Open();
                        adt.Fill(dat);
                        if (dat.Rows.Count == 0)
                        {
                            // The group does not exists so create it
                            Random random = new Random();
                            string code = random.Next().ToString();

                            cmd.CommandText = "INSERT [item].[Group] (ID , Name , [Description] , Active , PolicyID , ADDistinguishedName)" +
                                $"VALUES ('{ID}','{code}', '{strRoot}{cnName}', 1, '{defaultPolicyID}' ,'{distName}')";
                            cmd.ExecuteNonQuery();

                            // Now create the codeLookup for the group
                            cmd.CommandText = "INSERT [dbo].[dbCodeLookup] (cdType , cdCode , cdDesc)" +
                                $"VALUES ('SECGROUPS', '{code}', '{cnName}')";
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            DataRow row = dat.Rows[0];
                            ID = (Guid)row["ID"];
                        }
                    }
                    return ID;
                }
            }
        }

        internal void CreateUsers(string xmlUsers, Guid defaultPolicyID, Guid ID, bool debug)
        {
            if (!debug) return;

            // Now pass the user collection to the stored procedure in the database
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "dbo.ADSyncGroupUsers";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@defaultPolicyID", defaultPolicyID.ToString());
                cmd.Parameters.AddWithValue("@groupID", ID.ToString());
                cmd.Parameters.AddWithValue("@userXML", xmlUsers);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public string UpdateUserNames()
        {
            int result = 0;
            string netbiosSourceUserName = Config.GetConfigurationItem("NetbiosSourceUserName");
            string netbiosTargetUserName = Config.GetConfigurationItem("NetbiosTargetUserName");

            if (netbiosTargetUserName.Equals(netbiosSourceUserName, StringComparison.InvariantCultureIgnoreCase))
            {
                return $"Target and source Netbios names for users are equal: '{netbiosSourceUserName}'. See configuration settings for details";
            }

            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "dbo.ADUpdateUsersADID";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@sourceUserName", SqlDbType.NVarChar, 48);
                cmd.Parameters.Add("@targetUserName", SqlDbType.NVarChar, 48);
                cmd.Parameters.Add("@exceededTopUsersReturnCount", SqlDbType.Int);
                cmd.Parameters.Add("@rowsAffected", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@isExceededUserMaxLength", SqlDbType.Bit).Direction = ParameterDirection.Output;

                cmd.Parameters["@sourceUserName"].Value = netbiosSourceUserName;
                cmd.Parameters["@targetUserName"].Value = netbiosTargetUserName;
                cmd.Parameters["@exceededTopUsersReturnCount"].Value = ExceededTopUsersReturnCount;

                var exceededTopUsers = new StringBuilder(ExceededTopUsersReturnCount);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exceededTopUsers.Append($"{reader["userADID"]}<br>");
                    }
                }

                bool isExceededUserMaxLength = Convert.ToBoolean(cmd.Parameters["@isExceededUserMaxLength"].Value);
                int rowsAffected = Convert.ToInt32(cmd.Parameters["@rowsAffected"].Value);

                if (isExceededUserMaxLength)
                {
                    throw new ArgumentException($"Update failed as the combined domain name and username exceeds the maximum length of 50 characters. Please use a shorter name." +
                        $"<br>NetbiosTargetUserName: '{netbiosTargetUserName}'" +
                        $"<br>Below is the list of affected user accounts:" +
                        $"<br>{exceededTopUsers}");
                }

                result = rowsAffected;
            }

            string[] multiStrings = new string[2]
            {
                result == 1 ? "" : "s",
                result == 1 ? "as" : "ere"
            };

            return result == 0
                ? $"No user record was updated with '{netbiosTargetUserName}' netbios user name from '{netbiosSourceUserName}' netbios user name."
                : $"{result} user{multiStrings[0]} w{multiStrings[1]} updated with '{netbiosTargetUserName}' netbios user from '{netbiosSourceUserName}' netbios user name.";
        }

        public string ConvertUsersToUPN()
        {
            int result = 0;
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT usrID, usrADID FROM dbUser WHERE AccessType = 'INTERNAL' AND usrADID LIKE '%\%'";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.UpdateCommand = new SqlCommand("UPDATE dbUser SET usrADID = @usrADID WHERE usrID = @usrID", conn);
                    dataAdapter.UpdateCommand.Parameters.Add("@usrADID", SqlDbType.NVarChar, 50, "usrADID");
                    dataAdapter.UpdateCommand.Parameters.Add("@usrID", SqlDbType.Int, 0, "usrID").SourceVersion = DataRowVersion.Original;

                    using (DataTable usersTable = new DataTable())
                    {
                        conn.Open();
                        dataAdapter.Fill(usersTable);

                        foreach (DataRow row in usersTable.Rows)
                        {
                            string userPrincipalName = GetUserPrincipalName((string)row["usrADID"]);
                            if (userPrincipalName != null)
                                row["usrADID"] = userPrincipalName;
                        }

                        result = dataAdapter.Update(usersTable);
                    }
                }
            }

            string[] multiStrings = new string[2]
            {
                result == 1 ? "" : "s",
                result == 1 ? "as" : "ere"
            };

            return result == 0
                ? $"No user record was updated."
                : $"{result} user logon name{multiStrings[0]} w{multiStrings[1]} converted to UPN.";
        }

        private string GetUserPrincipalName(string userDomainName)
        {
            string userPrincipalName = null;
            string[] userNameParts = userDomainName.Split('\\');
            if (userNameParts.Length == 2)
            {
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, userNameParts[0]);
                Domain domain = Domain.GetDomain(context);
                using (DirectorySearcher searcher = new DirectorySearcher(domain.GetDirectoryEntry()))
                {
                    searcher.Filter = $"(&(objectClass=user)(objectCategory=Person)(sAMAccountName={userNameParts[1]}))";
                    searcher.PropertiesToLoad.Add("userPrincipalName");
                    SearchResult result = searcher.FindOne();
                    if (result != null && result.Properties.Contains("userPrincipalName"))
                        userPrincipalName = (string)result.Properties["userPrincipalName"][0];
                }
            }
            return userPrincipalName;
        }
    }
}
