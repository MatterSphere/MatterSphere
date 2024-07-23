using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;


namespace FWBS.OMS.UI.Windows
{
    public class LockState
    {

        #region Fields

        private string lockedby;
        private int lockedbyid;
        private DateTime lockeddate;

        public string LockedBy
        {
            get { return lockedby; }
        }

        private readonly IConnection connection;

        private readonly ObjectLinkRetriever retriever;

        public LockState()
        {
            retriever = new ObjectLinkRetriever();
            connection = Session.CurrentSession.CurrentConnection;
        }


        #endregion 

        #region Constants

        const string locktablename = "dbObjectLocking";
        const string lockaudittablename = "dbObjectLockingAudit";

        #endregion

        public void LockSearchListObject(string code)
        {
            DataTable dt = retriever.RetrieveObjectLinks(new SearchListComparer(code, 0, 0), false);
            DealWithObjects(dt, LockObject);
        }

        public void UnlockSearchListObject(string code)
        {
            UnlockCurrentObject(code, LockableObjects.SearchList);
        }


        public void LockEnquiryFormObject(string code)
        {
            DataTable dt = retriever.RetrieveObjectLinks(new EnquiryFormComparer(code, 0, 0), false);
            DealWithObjects(dt, LockObject);
        }

        public void UnlockEnquiryFormObject(string code)
        {
            UnlockCurrentObject(code, LockableObjects.EnquiryForm);
        }


        public void LockScriptObject(string code)
        {
            DataTable dt = retriever.RetrieveObjectLinks(new ScriptComparer(code), false);
            DealWithObjects(dt, LockObject);
        }

        public void UnlockScriptObject(string code)
        {
            UnlockCurrentObject(code, LockableObjects.Script);
        }


        public void LockPrecedentObject(string code)
        {
            DataTable dt = retriever.RetrieveObjectLinks(new PrecedentComparer(code), false);
            DealWithObjects(dt, LockObject);
        }

        public void UnlockPrecedentObject(string code)
        {
            UnlockCurrentObject(code, LockableObjects.Precedent);
        }


        public void LockDataListObject(string code)
        {
            DataTable dt = retriever.RetrieveObjectLinks(new DataListComparer(code));
            DealWithObjects(dt, LockObject);
        }

        public void UnlockDataListObject(string code)
        {
            UnlockCurrentObject(code, LockableObjects.DataList);
        }


        public void LockFileManagementObject(string code)
        {
            DataTable dt = retriever.RetrieveObjectLinks(new FileManagementComparer(code), false);
            DealWithObjects(dt, LockObject);
        }

        public void UnlockFileManagementObject(string code)
        {
            UnlockCurrentObject(code, LockableObjects.FileManagement);
        }


        private void UnlockCurrentObject(string code, LockableObjects type)
        {
            if (Session.CurrentSession.IsConnected && !string.IsNullOrWhiteSpace(code))
            {
                MarkObjectAsClosed(code, type);
                IObjectComparison obj = ObjectComparisonFactory.CreateIObjectComparison(code, type.ToString());
                DataTable dt = retriever.RetrieveObjectLinks(obj);
                DealWithObjects(dt, UnlockObject);
            }
        }


        private void DealWithObjects(DataTable linkedObjects, Action<string, LockableObjects> lockUnlock)
        {
            if (linkedObjects != null && linkedObjects.Rows.Count > 0 && lockUnlock != null)
            {
                foreach (DataRow lo in linkedObjects.Rows)
                {
                    LockableObjects type;
                    if (Enum.TryParse(Convert.ToString(lo["ObjectType"]), out type))
                    {
                        lockUnlock(Convert.ToString(lo["ObjectCode"]), type);
                    }
                }
            }
        }


        public bool ObjectLockStateCheck(string Code, string ObjectType)
        {
            bool result = true;
            LockableObjects lockableobject = (LockableObjects)Enum.Parse(typeof(LockableObjects), ObjectType);
            result = CheckObjectLockStateForImport(Code, lockableobject);
            return result;
        }


        private bool CheckObjectLockStateForImport(string code, LockableObjects obj)
        {
            bool result = false;
            try
            {
                DataTable dtCheck = GetLockData(code, obj);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    lockeddate = Convert.ToDateTime(dtCheck.Rows[0]["Locked"]);
                    lockedbyid = ConvertDef.ToInt32(dtCheck.Rows[0]["LockedBy"], 0);
                    if (lockedbyid != 0)
                    {
                        result = true;
                        ObjectLockedMessage(obj, code, lockeddate);
                    }
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
            return result;
        }


        public bool CheckObjectLockState(string code, LockableObjects obj)
        {
            bool result = false;
            try
            {
                DataTable dtCheck = GetLockData(code, obj);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    lockedbyid = ConvertDef.ToInt32(dtCheck.Rows[0]["LockedBy"], 0);
                    result = true;
                    if (lockedbyid == Session.CurrentSession.CurrentUser.ID)
                    {
                        ObjectAlreadyOpenMessage(obj);
                    }
                    else
                    {
                        SetupObjectLockedFeedback(dtCheck, code, obj);
                    }
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
            return result;
        }


        public bool CheckObjectLockStateForPackaging(string code, LockableObjects obj)
        {
            bool result = false;
            try
            {
                DataTable dtCheck = GetLockData(code, obj);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    result = true;
                    SetupObjectLockedFeedback(dtCheck, code, obj);
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
            return result;
        }


        private void SetupObjectLockedFeedback(DataTable dtCheck, string code, LockableObjects obj)
        {
            lockeddate = Convert.ToDateTime(dtCheck.Rows[0]["Locked"]);
            lockedbyid = ConvertDef.ToInt32(dtCheck.Rows[0]["LockedBy"], 0);
            ObjectLockedMessage(obj, code, lockeddate);
        }


 
        public bool CheckObjectIsLockedByCurrentUser(string code, LockableObjects obj)
        {
            DataTable dtCheck = GetLockData(code, obj);
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                return (Convert.ToInt32(dtCheck.Rows[0]["LockedBy"]) == Session.CurrentSession.CurrentUser.ID);
            }
            return false;
        }


        private DataTable GetLockData(string code, LockableObjects obj)
        {
            string sql = $"select ObjectCode, ObjectType, Locked, LockedBy from {locktablename} where ObjectCode = '{code}' and ObjectType = '{obj}'";
            DataTable dtCheck = ExecuteSQL(sql);
            return dtCheck;
        }


        private void LockObject(string code, LockableObjects type)
        {
            string sql = string.Format(@"if not exists (select * from {0} where ObjectCode = @code and ObjectType = @type)
                                        insert into {0} (ObjectCode, ObjectType, Locked, LockedBy) values (@code , @type, getdate(), {1})",
                                        locktablename, Session.CurrentSession.CurrentUser.ID);
            IDataParameter[] parList = new IDataParameter[2];
            parList[0] = connection.CreateParameter("code", code);
            parList[1] = connection.CreateParameter("type", type.ToString());
            DataTable dt = connection.ExecuteSQL(sql, parList);

            InsertLockingAuditRecord(code, type);
        }


        private void UnlockObject(string code, LockableObjects type)
        {
            string sql = $"delete from {locktablename} where ObjectCode = '{code}' and ObjectType = '{type}'";
            ExecuteSQL(sql);
        }


        private void InsertLockingAuditRecord(string code, LockableObjects type)
        {
            string sql = $"insert into {lockaudittablename} (ObjectCode, ObjectType, Locked, LockedBy) values ('{code}', '{type}', getdate(), {Session.CurrentSession.CurrentUser.ID})";
            ExecuteSQL(sql);
        }


        private DataTable ExecuteSQL(string sql)
        {
            DataTable dt = connection.ExecuteSQL(sql, null);
            connection.Disconnect();
            return dt;
        }

        private void ObjectLockedMessage(LockableObjects obj, string objectcode, DateTime lockeddate)
        {
            lockedby = User.GetUser(lockedbyid).FullName;
            MessageBox.ShowInformation((IWin32Window)null, "OBJLOCKEDBYUSER", "This %1% '%2%' has been locked by %3% since %4% at %5% and cannot be accessed.",
                GetLockableObjectDescription(obj), objectcode, lockedby, lockeddate.ToLongDateString(), lockeddate.ToShortTimeString());
        }

        private void ObjectAlreadyOpenMessage(LockableObjects obj)
        {
            lockedby = User.GetUser(lockedbyid).FullName;
            MessageBox.ShowInformation((IWin32Window)null, "OBJLOCKEDBYYOU", "You already have this %1% open therefore you cannot open it again.",
                GetLockableObjectDescription(obj));
        }

        private static string GetLockableObjectDescription(LockableObjects obj)
        {
            string value = obj.ToString();
            var memberInfo = typeof(LockableObjects).GetMember(value);
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    value = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return value;
        }

        #region Object Opening Methods

        public void MarkObjectAsOpen(string code, LockableObjects type)
        {
            ChangeObjectOpenState(code, type, 1);
        }


        public void MarkObjectAsClosed(string code,  LockableObjects type)
        {
            ChangeObjectOpenState(code, type, 0);
        }


        private void ChangeObjectOpenState(string code, LockableObjects type, int value)
        {
            IDataParameter[] parList = new IDataParameter[2];
            parList[0] = connection.CreateParameter("code", code);
            parList[1] = connection.CreateParameter("type", type.ToString());
            DataTable dt = connection.ExecuteSQL($"update {locktablename} set ObjectOpen = {value} where  ObjectCode = @code and ObjectType = @type", parList);
            connection.Disconnect();
        }


        public bool CheckForOpenObjects(string code, LockableObjects type)
        {
            IObjectComparison obj = ObjectComparisonFactory.CreateIObjectComparison(code, type.ToString(), 0, 0);
            DataTable dt = retriever.RetrieveObjectLinks(obj);
            return GatherObjects(dt, code);
        }

        public bool CheckIfObjectIsAlreadyOpen(string code, LockableObjects type)
        {
            bool result = false;
            try
            {
                IDataParameter[] parList = new IDataParameter[2];
                parList[0] = connection.CreateParameter("code", code);
                parList[1] = connection.CreateParameter("type", type.ToString());
                DataTable dt = connection.ExecuteSQL($"select * from {locktablename} where ObjectCode = @code and ObjectType = @type and ObjectOpen = 1", parList);
                connection.Disconnect();
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = true;
                    ObjectAlreadyOpenMessage(type);
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
            return result;
        }

        private bool GatherObjects(DataTable linkedobjects, string code = "")
        {
            if (linkedobjects != null && linkedobjects.Rows.Count > 0)
            {
                foreach (DataRow r in linkedobjects.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        if (Convert.ToString(r["objectcode"]) != code)
                        {
                            if(Convert.ToString(r["objecttype"]) != "Precedent")
                                if (ObjectIsOpen(Convert.ToString(r["objectcode"]), Convert.ToString(r["objecttype"])))
                                    return true;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(r["objecttype"]) != "Precedent")
                            if (ObjectIsOpen(Convert.ToString(r["objectcode"]), Convert.ToString(r["objecttype"])))
                                return true;
                    }
                }
            }
            return false;
        }

        private bool ObjectIsOpen(string code, string type)
        {
            string sql = $"select ObjectOpen from {locktablename} where ObjectCode = @code and ObjectType = @type";
            IDataParameter[] parList = new IDataParameter[2];
            parList[0] = connection.CreateParameter("code", code);
            parList[1] = connection.CreateParameter("type", type.ToString());
            DataTable dt = connection.ExecuteSQL(sql, parList);
            if (dt != null && dt.Rows.Count > 0)
                return ConvertDef.ToBoolean(dt.Rows[0]["ObjectOpen"], false);
            else
                return false;
        }

        #endregion

        public static void UnlockObjectsByUser(int userID)
        {
            IConnection connection = Session.CurrentSession.CurrentConnection;
            string sql = $"delete from {locktablename} where LockedBy = @userID";
            IDataParameter[] parList = new IDataParameter[1];
            parList[0] = connection.CreateParameter("userID", userID);
            DataTable dt = connection.ExecuteSQL(sql, parList);
        }

    }


    public enum LockableObjects
    {
        [Description("")]
        None,
        [Description("Enquiry Form")]
        EnquiryForm,
        [Description("Search List")]
        SearchList,
        [Description("Script")]
        Script,
        [Description("Precedent")]
        Precedent,
        [Description("Data List")]
        DataList,
        [Description("File Management")]
        FileManagement
    }


}
