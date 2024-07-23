using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using FWBS.Common;
using FWBS.OMS.FormsPortalService;

namespace FWBS.OMS
{
    /// <summary>
    /// 
    /// </summary>
    public class FormData : FWBS.OMS.CommonObject2
    {
        private string webserviceurl;
        private string webserviceuser;
        private string webservicepass;

        // Note that fields starting with fpe Prefix was instructed by MNW 29th Jan 2008 10:24
        // and was the reason I did it like this.
        [EnquiryEngine.EnquiryUsage(true)]
        public FormData(int formID, string objectType, Int64 linkID)
        {
            if (String.IsNullOrEmpty(webserviceurl))
            {
                webserviceurl = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSpecificData("FPEURL"));
                webserviceuser = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSpecificData("FPEUSER"));
                webservicepass = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSpecificData("FPEPASS"));
            }

            if (string.IsNullOrEmpty(webserviceurl))
                throw new FormDataException("FPEURL entry is missing from dbSpecificData");
            if (string.IsNullOrEmpty(webserviceuser))
                throw new FormDataException("FPEUSER entry is missing from dbSpecificData");
            if (string.IsNullOrEmpty(webservicepass))
                throw new FormDataException("FPEPASS entry is missing from dbSpecificData");

            if (objectType == null)
                throw new ArgumentNullException("objectType");

            ValidateType(objectType);

            FetchData(objectType,linkID, formID);
            HideColumns();
        }

        [EnquiryEngine.EnquiryUsage(true)]
        public FormData(int formID, string objectType, Int64 linkID, string webserviceUrl, string webserviceUser, string webservicePassword)
        {
            if (string.IsNullOrEmpty(webserviceUrl))
                throw new ArgumentNullException("webserviceUrl");
            if (string.IsNullOrEmpty(webserviceUser))
                throw new ArgumentNullException("webserviceUser");
            if (string.IsNullOrEmpty(webservicePassword))
                throw new ArgumentNullException("webservicePassword");

            webserviceurl = webserviceUrl;
            webserviceuser = webserviceUser;
            webservicepass = webservicePassword;

            if (objectType == null)
                throw new ArgumentNullException("objectType");

            ValidateType(objectType);

            FetchData(objectType, linkID, formID);
            HideColumns();
        }

        public override void Refresh(bool applyChanges)
        {
            base.Refresh(applyChanges);
            AddDynamicColumns();
        }

        public override void Refresh()
        {
            base.Refresh();
            AddDynamicColumns();
        }

        public void ClearData()
        {
             if (_data.Columns.Contains("rowguid"))
                _data.Columns.Remove("rowguid");

            SetExtraInfo("fpeRequested", DBNull.Value);
            SetExtraInfo("fpeRefreshed", DBNull.Value);
            SetExtraInfo("fpeOurRef", DBNull.Value);
            SetExtraInfo("fpeYourRef", DBNull.Value);
            SetExtraInfo("fpeEmail", DBNull.Value);
            SetExtraInfo("fpeDescription", DBNull.Value);
            SetExtraInfo("fpeFormGuid", DBNull.Value);
            base.Update();

        }
        
        public override void Update()
        {
            if (_data.Columns.Contains("rowguid"))
                _data.Columns.Remove("rowguid");
            
            if (this.State != ObjectState.Deleted)
                serializeData();
            base.Update();
        }

        #region CommonObject
        protected override string SelectStatement
        {
            get { return "SELECT * FROM dbFPEData"; }
        }

        protected override string PrimaryTableName
        {
            get { return "FPEData"; }
        }

        protected override string DefaultForm
        {
            get { return ""; }
        }

        public override object Parent
        {
            get { return null; }
        }

        protected override string[] FieldPrimaryKeys
        {
            get { return new string[3] { "fpeLinkID","fpeType","fpeFormID" }; }
        }

        protected override string SelectExistsStatement
        {
            get
            {
                return "SELECT fpeLinkID FROM dbFPEData";
            }
        }

        protected override string FieldCreated
        {
            get
            {
                return "fpeCreated";
            }
        }

        protected override string FieldCreatedBy
        {
            get
            {
                return "fpeCreatedBy";
            }
        }

        protected override string FieldUpdated
        {
            get
            {
                return "fpeUpdated";
            }
        }

        protected override string FieldUpdatedBy
        {
            get
            {
                return "fpeUpdatedBy";
            }
        }
        #endregion

        #region Private
        private void HideColumns()
        {
            if (_data.Columns["fpeData"].ExtendedProperties.ContainsKey("InVisible") == false)
                _data.Columns["fpeData"].ExtendedProperties.Add("InVisible", true);
        }

        private void FetchData(string objectType, Int64 objectID, int formID)
        {
            if (Exists(objectID, objectType, formID))
            {
                Fetch(objectID, objectType, formID);
            }
            else
            {
                base.Create();
                FormID = formID;
                ObjectType = objectType;
                ObjectID = objectID;
                AddDynamicColumns();
            }
        }

        private void ValidateType(string objectType)
        {
            switch (objectType.ToUpperInvariant())
            {
                case "FILE":
                case "CLIENT":
                case "DOCUMENT":
                case "ASSOCIATE":
                case "USER":
                case "CONTACT":
                case "FEEEARNER":
                    break;
                default:
                    throw new FormDataUnknownTypeException(this.ObjectType);
            }
        }

        private bool dynamicColumnsAdded;

        private void AddDynamicColumns()
        {
            if (IsData == false)
            {
                if (dynamicColumnsAdded == false)
                {
                    FormDefinition defs = new FormDefinition(this.FormID,webserviceurl,webserviceuser,webservicepass);
                    foreach (Control ctrl in defs.Controls)
                    {
                        try
                        {
                            AddColumn(ctrl);
                        }
                        catch (DuplicateNameException ex)
                        {
                            System.Diagnostics.Trace.WriteLine("FD:ERR:Duplicate Field" + ex.Message);
                        }
                    }
                    dynamicColumnsAdded = true;
                    if (IsData)
                        deserializeData();
                }
            }
            else
            {
                deserializeData();
            }
        }

        public override void SetExtraInfo(string fieldName, object val)
        {
            if (_data.Columns.Contains(fieldName) == true)
            {
                if (_data.Columns[fieldName].DataType == typeof(bool))
                {
                    string value = Convert.ToString(val);
                    if (value.ToLowerInvariant() == "no") val = "false";
                    if (value.ToLowerInvariant() == "yes") val = "True";
                }

            }
            base.SetExtraInfo(fieldName, val);
        }

        private void serializeData()
        {
            DataSet ds = new DataSet();
            DataTable dt = _data.Copy();

            for (int i = dt.Columns.Count-1; i >= 0; i--)
                if (dt.Columns[i].ColumnName.StartsWith("fpe") == true || dt.Columns[i].ColumnName.StartsWith("rowguid") == true)
                {
                    try
                    {
                        dt.Columns.RemoveAt(i);
                    }
                    catch(ArgumentException) { }
                }
                    
            ds.Tables.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                ds.WriteXml(stream,XmlWriteMode.WriteSchema);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                dataXML = enc.GetString(buffer);
            }
        }

        private void deserializeData()
        {
            if (this.IsData)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    byte[] buffer = encoding.GetBytes(this.dataXML);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Position = 0;
                    DataSet ds1 = new DataSet();
                    ds1.ReadXml(stream,XmlReadMode.Auto);
                    DataSet ds2 = new DataSet();
                    ds2.Tables.Add(_data);
                    ds2.Merge(ds1);
                }
            }
        }

        private void AddColumn(Control ctrl)
        {
            if (ctrl == null)
                throw new ArgumentNullException("ctrl");

            if (string.IsNullOrEmpty(ctrl.Name))
                return;

            if (string.IsNullOrEmpty(ctrl.Type))
                return;

            if (ctrl.Type == "Label")
                return;

            Type coltype;
            switch (ctrl.Type)
            {
                case "Date":
                    coltype= typeof(DateTime);
                    break;
                case "Numeric":
                    coltype = typeof(decimal);
                    break;
                case "YesNo":
                    coltype = typeof(bool);
                    break;
                default:
                    coltype = typeof(String);
                    break;
            }

            DataColumn col = new DataColumn(ctrl.Name);
            col.Caption = ctrl.Caption;
            col.DataType = coltype;
            Int32 size = 0;
            try
            {
                if (Int32.TryParse(ctrl.Length, out size))
                    col.MaxLength = size;
            }
            catch (ArgumentException)
            {
            }
            _data.Columns.Add(col);
        }

        protected override void Fetch(params object[] id)
        {
            if (Exists(id))
            {
                base.Fetch(id);
                AddDynamicColumns();
            }
        }
        #endregion

        #region Properties

        public int FormVersion
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("fpeFormVersion"), 1);
            }
            set
            {
                SetExtraInfo("fpeFormVersion", value);
            }
        }

        private bool IsData
        {
            get
            {
                return GetExtraInfo("fpeData") != DBNull.Value;
            }
        }

        private string dataXML
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fpeData"));
            }
            set
            {
                SetExtraInfo("fpeData", value);
            }
        }

        public int CampaignID
        {
            get
            {
                if (_data.Columns.Contains("fpeCampaignID") && GetExtraInfo("fpeCampaignID") != DBNull.Value)
                    return ConvertDef.ToInt32(GetExtraInfo("fpeCampaignID"), 0);
                else
                    return 0;
            }
            set
            {
                if (_data.Columns.Contains("fpeCampaignID"))
                    SetExtraInfo("fpeCampaignID", value);
            }
        }

        public int FormID
        {
            get
            {
                return Convert.ToInt32(GetExtraInfo("fpeFormID"));
            }
            set
            {
                SetExtraInfo("fpeFormID",value);
            }
        }

        public string ObjectType
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fpeType"));
            }
            private set
            {
                SetExtraInfo("fpeType", value);
            }
        }

        public string OurRef
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fpeOurRef"));
            }
        }

        public string YourRef
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fpeYourRef"));
            }
        }


        public string Email
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fpeEmail"));
            }
        }

        public string Description
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fpeDescription"));
            }
        }

        public DateTime Refreshed
        {
            get
            {
                return Convert.ToDateTime(GetExtraInfo("fpeRefreshed"));
            }
            internal set
            {
                SetExtraInfo("fpeRefreshed", value);
            }
        }

        public DateTime? Requested
        {
            get
            {
                if (_data.Columns.Contains("fpeRequested") && GetExtraInfo("fpeRequested") != DBNull.Value)
                    return Convert.ToDateTime(GetExtraInfo("fpeRequested"));
                else
                    return null;
            }
            internal set
            {
                if (_data.Columns.Contains("fpeRequested") && Requested.HasValue == false)
                    SetExtraInfo("fpeRequested", value);
            }
        }

        public DateTime? Completed
        {
            get
            {
                if (_data.Columns.Contains("fpeCompleted") && GetExtraInfo("fpeCompleted") != DBNull.Value)
                    return Convert.ToDateTime(GetExtraInfo("fpeCompleted"));
                else
                    return null;
            }
            internal set
            {
                if (_data.Columns.Contains("fpeCompleted"))
                    SetExtraInfo("fpeCompleted", value);
            }
        }



        public Guid? FormGuid
        {
            get
            {
                return GetExtraInfo("fpeFormGuid") as Guid?;
            }
            internal set
            {
                SetExtraInfo("fpeFormGuid", value);
            }
        }

        public IEnumerable<KeyValuePair<string, object>> Data
        {
            get
            {
                foreach (DataColumn dc in _data.Columns)
                    if (dc.ColumnName.StartsWith("fpe") == false)
                    {
                        yield return new KeyValuePair<string, object>(dc.ColumnName,_data.Rows[0][dc]);
                    }
            }
        }

        internal string WebserviceUrl
        {
            get
            {
                return webserviceurl;
            }
        }

        internal string WebserviceUser
        {
            get
            {
                return webserviceuser;
            }
        }

        internal string WebservicePassword
        {
            get
            {
                return webservicepass;
            }
        }

        [EnquiryEngine.EnquiryUsage(true)]
        public Int64 ObjectID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("fpeLinkID"));
            }
            private set
            {
                SetExtraInfo("fpeLinkID", value);
            }
        }
        #endregion

    }
}
 
