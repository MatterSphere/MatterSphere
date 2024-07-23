using System;
using System.Data;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{

    public class ObjectLinkRetriever
    {
        private DataTable dtLinks;
        private IConnection connection;
        private int parent = 0;
        private int objnum = 1;
        private bool includeDataLists;


        public ObjectLinkRetriever()
        {
            ConstructDataTable();
        }


        private void ConstructDataTable()
        {
            dtLinks = new DataTable();
            dtLinks.Columns.Add("objectcode", typeof(string));
            dtLinks.Columns.Add("objecttype", typeof(string));
            dtLinks.Columns.Add("objectversion", typeof(int));
            dtLinks.Columns.Add("objectnum", typeof(int)); 
            dtLinks.Columns.Add("parentobject", typeof(int));
        }


        public DataTable RetrieveObjectLinks(IObjectComparison obj, bool includeDataLists = true)
        {
            this.includeDataLists = includeDataLists;
            dtLinks.Clear();
            AddLink(obj.code, obj.type);
            parent = objnum;
            DetermineLinkRetrievalRoute(obj.code, obj.type);
            return dtLinks;
        }


        private void GetObjectLinks(string code,string type)
        {
            DetermineLinkRetrievalRoute(code, type);
        }


        private void DetermineLinkRetrievalRoute(string code, string type)
        {
            switch (type)
            {
                case "EnquiryForm":
                    CollectEnquiryFormLinks(code);
                    break;

                case "Script":
                    CollectScriptLinks(code);
                    break;

                case "SearchList":
                    CollectSearchListLinks(code);
                    break;

                case "Precedent":
                    CollectPrecedentLinks(code);
                    break;

                case "FileManagement":
                    CollectFileManagementLinks(code);
                    break;
            }
        }


        private void CollectEnquiryFormLinks(string code)
        {
            parent = objnum;
            CheckForScript(code, @"select enqScript as objectcode, scrVersion as objectversion from dbEnquiry e inner join dbScript scr on e.enqScript = scr.scrCode where enqCode = @code");

            if (includeDataLists)
            {
                CheckForDataList(code, @"select distinct eq.quDataList as objectcode, edl.enqDLVersion as objectversion
                                        from dbenquiryquestion eq
                                        inner join dbEnquiry e on e.enqid = eq.enqID
                                        inner join dbEnquiryDataList edl on edl.enqTable = eq.quDataList
                                        where e.enqCode = @code
                                        and eq.quDataList is not null");
            }
            
            CheckForSearchList(code, @"select schCode as objectcode, schVersion as objectversion from dbSearchListConfig slc inner join dbEnquiry e on e.enqCode = slc.schEnquiry where schEnquiry = @code");
        }


        private void CollectScriptLinks(string code)
        {
            parent = objnum;
            CheckForEnquiryForm(code, @"select enqCode as objectcode, enqVersion as objectversion from dbEnquiry where enqScript = @code");
            CheckForSearchList(code, @"select schCode as objectcode, schVersion as objectversion from dbSearchListConfig where schScript = @code");
            CheckForPrecedent(code, @"select PrecID as objectcode, 1 as objectversion from dbPrecedents where precScript = @code");
            CheckForFileManagement(code, @"select appCode as objectcode, appVer as objectversion from dbFileManagementApplication where appScript = @code");
        }


        private void CollectSearchListLinks(string code)
        {
            parent = objnum;
            CheckForEnquiryForm(code, @"select schEnquiry as objectcode, enqVersion as objectversion from dbSearchListConfig slc inner join dbEnquiry e on slc.schEnquiry = e.enqCode where schCode = @code");
            CheckForScript(code, @"select schScript as objectcode, scrVersion as objectversion from dbSearchListConfig slc inner join dbScript s on slc.schScript = s.scrCode where schCode = @code");
        }


        private void CollectPrecedentLinks(string code)
        {
            parent = objnum;
            CheckForScript(code, @"select precScript as objectcode, scrVersion as objectversion from dbPrecedents p inner join dbScript s on p.PrecScript = s.scrCode where PrecID = @code");
        }


        private void CollectFileManagementLinks(string code)
        {
            parent = objnum;
            CheckForScript(code, @"select appScript as objectcode, scrVersion as objectversion from dbFileManagementApplication fm inner join dbScript s on fm.appScript = s.scrCode where appCode = @code");
        }


        private void CollectDataListLinks(string code)
        {
            parent = objnum;
            CheckForEnquiryForm(code, @"select distinct e.enqCode as objectcode, e.enqVersion as objectversion
                                        from dbenquiryquestion eq
                                        inner join dbEnquiry e on e.enqid = eq.enqID
                                        where eq.quDataList = @code");
        }


        private void CheckForEnquiryForm(string code, string sql)
        {
            ProcessObjectLinks(code, sql, LockableObjects.EnquiryForm);
        }


        private void CheckForScript(string code, string sql)
        {
            ProcessObjectLinks(code, sql, LockableObjects.Script);
        }


        private void CheckForDataList(string code, string sql)
        {
            ProcessObjectLinks(code, sql, LockableObjects.DataList);
        }


        private void CheckForSearchList(string code, string sql)
        {
            ProcessObjectLinks(code, sql, LockableObjects.SearchList);
        }


        private void CheckForPrecedent(string code, string sql)
        {
            ProcessObjectLinks(code, sql, LockableObjects.Precedent);
        }


        private void CheckForFileManagement(string code, string sql)
        {
            ProcessObjectLinks(code, sql, LockableObjects.FileManagement);
        }


        private void ProcessObjectLinks(string code, string sql, LockableObjects type)
        {
            DataTable dt = GetLinkData(code, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                AddLinks(dt, type.ToString());
            }
        }


        private DataTable GetLinkData(string code, string sql)
        {
            connection = Session.CurrentSession.CurrentConnection;
            IDataParameter[] parList = new IDataParameter[1];
            parList[0] = connection.CreateParameter("code", code);
            DataTable dt = connection.ExecuteSQL(sql, parList);
            return dt;
        }
        

        private void AddLinks(DataTable dt, string type)
        {
            foreach (DataRow r in dt.Rows)
            {
                string objectcode = Convert.ToString(r["objectcode"]);
                if (string.IsNullOrWhiteSpace(objectcode))
                {
                    continue;
                }

                if (!IsObjectInTheTable(objectcode, type))
                {
                    objnum++;
                    dtLinks.Rows.Add(objectcode, type, Convert.ToInt32(r["objectversion"]), objnum, parent);
                    GetObjectLinks(objectcode, type);
                }
            }
        }


        private void AddLink(string code, string type)
        {
            dtLinks.Rows.Add(code, type, objnum, parent);
        }


        private bool IsObjectInTheTable(string code, string type)
        {
            DataRow [] result = dtLinks.Select($"objectcode = '{code}' and objecttype = '{type}'");
            return result.Length > 0;
        }

    }
}
