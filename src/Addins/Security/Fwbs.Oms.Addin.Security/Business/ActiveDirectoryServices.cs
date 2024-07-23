using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.DirectoryServices;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace FWBS.OMS.Addin.Security
{
    public class ActiveDirectoryServices
    {
        public static DataTable ListActiveGroups()
        {
            FWBS.OMS.ReportingServer OMS = new ReportingServer("FWBS Limited 2005");
            DataTable already = OMS.Connection.ExecuteSQLTable("SELECT ADGUID FROM item.[Group]", "DATA", new IDataParameter[0]);

            DataTable data = new DataTable("Groups");
            data.Columns.Add("name");
            data.Columns.Add("id");
            data.Columns.Add("objectguid");
            using (DirectorySearcher srch = new DirectorySearcher())
            {
                const string query = "(&(objectCategory=group))";
                srch.Filter = query;
                /*
                DMB: By default only a max of 1000 items are returned from a search 
                 * Investigations revealed that setting the Size Limit had no effect at SMK site
                 * setting Page size did have an effect and allow all items to be returned.
                 * If Page Size is left to the default 0 then the 1000 restriction kicks in.   
                 */
                srch.PageSize = 500;

                using (SearchResultCollection sResult = srch.FindAll())
                {
                    if (null != sResult)
                        for (int i = 0; i < sResult.Count; i++)
                            if (null != sResult[i].Properties["name"] && sResult[i].Properties["name"].Count == 1)
                            {
                                Guid guid = new Guid(sResult[i].Properties["objectguid"][0] as byte[]);
                                already.DefaultView.RowFilter = "ADGUID = '" + guid.ToString() + "'";
                                if (already.DefaultView.Count == 0)
                                {
                                    DataRow row = data.NewRow();
                                    row["objectguid"] = guid;
                                    row["id"] = sResult[i].Properties["distinguishedname"][0];
                                    row["name"] = sResult[i].Properties["name"][0];
                                    data.Rows.Add(row);
                                }
                            }
                }
            }
            return data;
        }

        public static void AddGroup(string distinguishedName, string policy)
        {
            string endpoint = "";

            try
            {
                endpoint = (string)Session.CurrentSession.GetSpecificData("ADGROUPSYNC");

                string defaultRoot = GetDefaultRoot();

                string result;

                using (var webClient = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["distinguishedName"] = distinguishedName;
                    values["defaultRoot"] = defaultRoot;
                    var response = webClient.UploadValues(endpoint, values);
                    result = webClient.Encoding.GetString(response);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var array = serializer.Deserialize<object[]>(string.Concat("[", result ,"]"));
                var dict = array[0] as Dictionary<string, object>;
                
                var root = dict["root"].ToString();
                var groupName = dict["groupName"].ToString(); 
                var xmlUsers = dict["groupUsersXml"].ToString();

                Guid defaultPolicyId = Guid.Parse(policy);
                Guid ID = CreateGroup(distinguishedName, defaultPolicyId, root, groupName);

                if (!string.IsNullOrEmpty(xmlUsers))
                {
                    CreateUsers(xmlUsers, defaultPolicyId, ID);
                }
            }
            catch (Exception e)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendLine("FWBS.OMS.Addin.Security.ActiveDirectoryServices.AddGroup method error.");
                strBuilder.AppendLine("The 'ADGROUPSYNC' specific data is '" + endpoint + "'.");
                strBuilder.AppendLine("Details: " + e.Message);

                throw new Exception(strBuilder.ToString(), e);
            }

        }

        public static string GetDefaultRoot()
        {
            FWBS.OMS.ReportingServer OMS = new ReportingServer("FWBS Limited 2005");
            DataTable dts = new DataTable();

            dts = OMS.Connection.ExecuteSQLTable("SELECT SD.spData FROM dbo.dbRegInfo RI JOIN dbo.dbSpecificData SD ON SD.brID = RI.brID WHERE spLookup = 'ADSECDOMAIN'", "SpecificData", null);

            if (dts.Rows.Count == 1)
            {
                return dts.Rows[0][0].ToString() + "\\";
            }

            return "";
        }

        private static Guid CreateGroup(string distName, Guid defaultPolicyID, string strRoot, string cnName)
        {
            FWBS.OMS.ReportingServer OMS = new ReportingServer("FWBS Limited 2005");

            // Check to see if that group exists in OMS and if it does not create it
            Guid ID = System.Guid.NewGuid();
            DataTable dat = new DataTable();

            dat = OMS.Connection.ExecuteSQLTable("SELECT ID FROM [item].[Group] WHERE ADDistinguishedName = '" + distName + "'", "Group", null);

            if (dat.Rows.Count == 0)
            {
                // The group does not exists so create it
                Random random = new Random();
                string code = random.Next().ToString();

                OMS.Connection.ExecuteSQL("INSERT [item].[Group] ( ID , Name , [Description] , Active , PolicyID , ADDistinguishedName ) VALUES ( '" + ID.ToString() + "','" + code + "', '" + strRoot + cnName + "' , 1 , '" + defaultPolicyID.ToString() + "' ,'" + distName + "' )");
                
                // Now create the codeLookup for the group
                OMS.Connection.ExecuteSQL("INSERT [dbo].[dbCodeLookup] ( cdType , cdCode , cdDesc ) VALUES ( 'SECGROUPS' , '" + code + "' , '" + cnName + "' ) ");
            }
            else
            {
                DataRow row = dat.Rows[0];
                ID = (Guid)row["ID"];
            }

            return ID;
        }

        private static void CreateUsers(string xmlUsers, Guid defaultPolicyID, Guid ID)
        {
            // Now pass the user collection to the stored procedure in the database
            FWBS.OMS.ReportingServer OMS = new ReportingServer("FWBS Limited 2005");

            IDataParameter[] parameters = new IDataParameter[3];
            parameters[0] = OMS.Connection.AddParameter("defaultPolicyID", defaultPolicyID.ToString());
            parameters[1] = OMS.Connection.AddParameter("groupID", ID.ToString());
            parameters[2] = OMS.Connection.AddParameter("userXML", xmlUsers);
            OMS.Connection.ExecuteProcedure("dbo.ADSyncGroupUsers", parameters);
        }
    }
}
