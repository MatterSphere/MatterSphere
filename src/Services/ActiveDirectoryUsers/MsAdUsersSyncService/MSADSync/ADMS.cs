using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Xml;

namespace MsAdUsersSyncService.MSADSync
{
    public class ADMS
    {
        private MatterSphereDB _msDb;
        private readonly Logging _log;

        public ADMS()
        {
            _log = new Logging("MatterSphere AD Sync");
        }

        public void RunProcess()
        {
            _log.CreateLogEntry("Start of AD Sync Process");
            _log.CreateLogEntry("Connect to MatterSphere Database");
            if (_msDb == null)
                _msDb = new MatterSphereDB();

            // Get List of Groups
            DataTable groupList = _msDb.GetGroupsToSync();
            _log.CreateLogEntry("Number of Groups Returned : " + groupList.Rows.Count.ToString());
            foreach (DataRow r in groupList.Rows)
            {
                try
                {
                    string distinguishedName = r["ADDistinguishedName"].ToString();
                    _log.CreateLogEntry("Processing - Distingusihed Name : " + distinguishedName);
                    string groupName = distinguishedName.Substring(3, distinguishedName.IndexOf(",") - 3);
                    string groupId = r["ID"].ToString();
                    string policyId = r["PolicyID"].ToString();
                    string domainLdap = distinguishedName.Substring(distinguishedName.IndexOf("DC="));
                    _log.CreateLogEntry("Domain LDAP Name : " + domainLdap);
                    string domainName = domainLdap.Replace("DC=", "");
                    domainName = domainName.Replace(",", ".");
                    _log.CreateLogEntry("Group Domain Name : " + domainName);
                    _log.CreateLogEntry("Get Users From AD for Group.");
                    string userXml = GetUsers(domainName, domainLdap, distinguishedName, groupName);
                    if (!string.IsNullOrEmpty(userXml))
                    {
                        _log.CreateLogEntry("Pass Users to MatterSphere Stored Procedure");
                        _msDb.CreateUsers(userXml, new Guid(policyId), new Guid(groupId));
                    }
                    _log.CreateLogEntry("Group Processing Complete");
                }
                catch (Exception ex3)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Group Processing Failure.");
                    sb.AppendLine(ex3.Message);
                    if (ex3.InnerException != null)
                        sb.AppendLine("Inner Exception").AppendLine(ex3.InnerException.Message);
                    _log.CreateErrorEntry(sb.ToString());
                }
            }
            _log.CreateLogEntry("All Groups Processed");

            _msDb = null;
        }

        internal string GetUsers(string domainName, string domainLdap, string groupLdap, string groupName)
        {
            string xml = null;
            PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain, domainName, domainLdap);
            GroupPrincipal insGroupPrincipal = new GroupPrincipal(insPrincipalContext) { Name = groupName };
            PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher { QueryFilter = insGroupPrincipal };
            _log.CreateLogEntry("Before Attempt to Find Group");
            GroupPrincipal gp = (GroupPrincipal)insPrincipalSearcher.FindOne();
            _log.CreateLogEntry("After Group has been found");
            if (gp.DistinguishedName != groupLdap)
            {
                return xml;
            }

            DataTable dt = new DataTable("users");
            dt.Columns.Add(new DataColumn("userNTName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("userName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("active", typeof(System.Int32)));
            string[] listProperties = new string[]
            {
                "sAMAccountName", "userAccountControl", "userPrincipalName"
            };
            _log.CreateLogEntry("Before Get Group Members.");
            var be = gp.Members.GetEnumerator();
            _log.CreateLogEntry("After Members Enumerated");
            bool valid = false;
            valid = RunMoveNext(be);
            while (valid == true)
            {
                try
                {
                    if (be.Current.StructuralObjectClass.Equals("User", StringComparison.OrdinalIgnoreCase))
                    {
                        _log.CreateLogEntry("User Found");
                        AddUserToDataTable(dt, listProperties, be.Current);
                    }
                    if (be.Current.StructuralObjectClass.Equals("Group", StringComparison.OrdinalIgnoreCase))
                    {
                        _log.CreateLogEntry("Group Found");
                        GetGroupUsers(dt, listProperties, be.Current);
                    }

                }
                catch (InvalidOperationException)
                {
                    break;
                }
                catch (Exception)
                {
                    _log.CreateLogEntry("Object does not have a StructuralObjectClass. Name : " + be.Current.Name + " - SID :" + be.Current.Sid.ToString());
                }
                finally
                {
                    valid = RunMoveNext(be);
                }
            }
            dt.AcceptChanges();
            _log.CreateLogEntry("After Get Group Members.");

            using (System.IO.StringWriter stgW = new System.IO.StringWriter())
            {
                XmlTextWriter xmlTW = new XmlTextWriter(stgW);
                dt.WriteXml(xmlTW, XmlWriteMode.IgnoreSchema, true);
                xmlTW.Flush();
                xml = stgW.ToString();
            }
            return xml;

        }

        private bool RunMoveNext(IEnumerator<Principal> be)
        {
            try
            {
                be.MoveNext();
                string strName = be.Current.DisplayName;
                System.Diagnostics.Debug.WriteLine(strName);
                return true;
            }
            catch (InvalidOperationException)
            {
                // Usually Caused by End of List being reached.
                return false;
            }
            catch (System.Security.Authentication.AuthenticationException)
            {
                // Unable to read Properties of User in Group. Usually due to Trust Issues.
                return RunMoveNext(be);

            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GetGroupUsers(DataTable dt, string[] listProperties, Principal principal)
        {
            GroupPrincipal gp = (GroupPrincipal)principal;
            var be = gp.Members.GetEnumerator();
            _log.CreateLogEntry("Group Found in Group : " + gp.DistinguishedName);
            bool valid = true;
            RunMoveNext(be);
            while (valid == true)
            {
                try
                {
                    if (be.Current.StructuralObjectClass.Equals("User", StringComparison.OrdinalIgnoreCase))
                    {
                        AddUserToDataTable(dt, listProperties, be.Current);
                    }
                    if (be.Current.StructuralObjectClass.Equals("Group", StringComparison.OrdinalIgnoreCase))
                    {
                        GetGroupUsers(dt, listProperties, be.Current);
                    }
                    valid = RunMoveNext(be);
                }
                catch (InvalidOperationException)
                {
                    //End of List
                    break;
                }
                catch (System.NullReferenceException)
                {
                    // Invalid User in List
                    valid = RunMoveNext(be);
                }
            }
        }

        private void AddUserToDataTable(DataTable dt, string[] listProperties, Principal p)
        {
            DirectoryEntry e = new DirectoryEntry("LDAP://" + p.DistinguishedName);
            e.RefreshCache(listProperties);

            string ntUserName = _msDb.IsAAD && e.Properties.Contains("userPrincipalName")
                ? (string)e.Properties["userPrincipalName"][0]
                : GetNetBIOSName(p.Context.ConnectedServer) + @"\" + e.Properties["sAMAccountName"][0];

            string ntSearchFiled = $"userNTName ='{ntUserName}'";
            var foundUsers = dt.Select(ntSearchFiled);
            if (foundUsers.Length == 0)
            {
                dt.Rows.Add(ntUserName, e.Properties["sAMAccountName"][0], e.Properties["userAccountControl"][0]);
            }
            _log.CreateLogEntry("User Found in Group : " + p.DistinguishedName);
            e.Dispose();
        }

        private string GetNetBIOSName(string dcFQDN)
        {
            string netBIOSName = null;
            string deString = "LDAP://" + dcFQDN + "/rootDSE";
            DirectoryEntry rootDSE = new DirectoryEntry(deString);
            string domain = (string)rootDSE.Properties["defaultNamingContext"][0];
            rootDSE.Dispose();

            if (!string.IsNullOrEmpty(domain))
            {
                DirectoryEntry parts = new DirectoryEntry("LDAP://" + dcFQDN + "/CN=Partitions,CN=Configuration," + domain);

                foreach (DirectoryEntry part in parts.Children)
                {
                    if ((string)part.Properties["nCName"][0] == domain)
                    {
                        netBIOSName = (string)part.Properties["NetBIOSName"][0];
                        break;
                    }
                }

                parts.Dispose();
            }
            return netBIOSName;
        }

        public string GetDomainName(string defaultRoot)
        {
            if (String.IsNullOrEmpty(defaultRoot))
            {
                using (DirectoryEntry entry = new DirectoryEntry())
                    defaultRoot = entry.Properties["dc"][0].ToString() + "\\";
            }

            return defaultRoot;
        }

        public string GetActiveDirectoryGroupName(string distName)
        {
            string groupName = string.Empty;
            DirectoryEntry entry = new DirectoryEntry();
            DirectorySearcher search = new DirectorySearcher(entry);

            // Check for a valid AD Group 
            search.Filter = $"(&(objectCategory=Group)(distinguishedName={distName}))";
            search.PropertiesToLoad.Add("sAMAccountName");
            SearchResult rslt = search.FindOne();
            if (rslt != null)
                groupName = (string)rslt.Properties["sAMAccountName"][0];

            search.Dispose();
            entry.Dispose();
            return groupName;
        }

        private SearchResultCollection GetSearchResultCollection(string distName, DirectorySearcher search, bool isAAD)
        {
            // Now get a list of users for the group
            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("memberof");
            search.PropertiesToLoad.Add("userAccountControl");
            search.PropertiesToLoad.Add("userPrincipalName");
            search.Filter = $"(&(objectCategory=person)(objectClass=user)(memberOf={distName}){(isAAD ? "(userPrincipalName=*)" : string.Empty)})";
            search.PageSize = 500;
            SearchResultCollection result = search.FindAll();

            return result;
        }

        public string GetGroupUsersXml(string distName, string root, bool isAAD)
        {
            DirectoryEntry entry = new DirectoryEntry();
            DirectorySearcher search = new DirectorySearcher(entry);
            SearchResultCollection result = GetSearchResultCollection(distName, search, isAAD);

            string xml = "";

            if (result.Count != 0)
            {
                DataTable dt = new DataTable("users") { MinimumCapacity = result.Count };
                dt.Columns.Add(new DataColumn("userNTName", typeof(System.String)));
                dt.Columns.Add(new DataColumn("userName", typeof(System.String)));
                dt.Columns.Add(new DataColumn("active", typeof(System.Int32)));

                foreach (SearchResult srt in result)
                {
                    dt.Rows.Add(isAAD ? srt.Properties["userPrincipalName"][0] : root + srt.Properties["sAMAccountName"][0], srt.Properties["sAMAccountName"][0], srt.Properties["userAccountControl"][0]);
                }

                dt.AcceptChanges();

                using (System.IO.StringWriter stgW = new System.IO.StringWriter())
                {
                    XmlTextWriter xmlTW = new XmlTextWriter(stgW);
                    dt.WriteXml(xmlTW, XmlWriteMode.IgnoreSchema, true);
                    xmlTW.Flush();
                    xml = stgW.ToString();
                }
            }

            result.Dispose();
            search.Dispose();
            entry.Dispose();
            return xml;
        }


    }
}