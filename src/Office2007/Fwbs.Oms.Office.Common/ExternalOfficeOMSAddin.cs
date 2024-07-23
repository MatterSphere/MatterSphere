using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fwbs.Oms.Office.Common
{
    using FWBS.OMS;

    [Guid("42CD0541-7641-4bc8-90B5-D6912571D8A4")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ExternalOfficeOMSAddin : StandardOleMarshalObject, Fwbs.Oms.Office.Common.IExternalOfficeOMSAddin
    {
        private OfficeOMSAddin addin;

        public OfficeOMSAddin Addin
        {
            get { return addin; }
            set { addin = value; }
        }

        public ExternalOfficeOMSAddin(OfficeOMSAddin addin)
        {
            if (addin == null)
                throw new ArgumentNullException("addin");
            this.addin = addin;
        }

        public ExternalOfficeOMSAddin()
        {
            // Blank Constructor override.
        }

        public object Parse(string field, object defVal)
        {
            FieldParser parser = new FieldParser();
            Associate assoc = addin.OMSApplication.GetCurrentAssociate(addin.OMSApplication);
            parser.ChangeObject(assoc);
            return parser.Parse(field, defVal);
        }

        
        public string RunScript(string commandName, string[] fieldNames, object[] values)
        {

            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            int ctr = 0;

            if (fieldNames != null)
            {
                foreach (string fld in fieldNames)
                {
                    if (String.IsNullOrEmpty(fld) == false)
                    {
                        if (values == null || values.Length <= ctr)
                            pars.Add(fld, null);
                        else
                        {
                            pars.Add(fld, values[ctr]);
                            ctr++;
                        }
                    }
                }
            }



            return addin.RunScriptCommand(commandName, pars);

        }

        public object RunCommand(string command, object application)
        {

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                return addin.RunCommand(String.Empty, command, application);

            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                return true;
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }


        public object GetDocVariable(string name)
        {
            return addin.OMSApplication.GetDocVariable(addin.OMSApplication, name, String.Empty);
        }
        public void SetDocVariableS(object obj, string varName, string val)
        {
            addin.OMSApplication.SetDocVariable(obj, varName, val);
        }
        public void SetDocVariableL(object obj, string varName, long val)
        {
            addin.OMSApplication.SetDocVariable(obj, varName, val);
        }
        public void SetDocVariableB(object obj, string varName, bool val)
        {
            addin.OMSApplication.SetDocVariable(obj, varName, val);
        }

        public string GetDocVariableS(string name, string def)
        {
            return addin.OMSApplication.GetDocVariable(addin.OMSApplication, name, def);
        }

        public long GetDocVariableN(string name, long def)
        {
            return addin.OMSApplication.GetDocVariable(addin.OMSApplication, name, def);
        }

        public bool GetDocVariableB(string name, bool def)
        {
            return addin.OMSApplication.GetDocVariable(addin.OMSApplication, name, def);
        }

        public string GetDocumentVariableS(object obj, string name, string def)
        {
            return addin.OMSApplication.GetDocVariable(obj, name, def);
        }

        public long GetDocumentVariableN(object obj, string name, long def)
        {
            return addin.OMSApplication.GetDocVariable(obj, name, def);
        }

        public bool GetDocumentVariableB(object obj, string name, bool def)
        {
            return addin.OMSApplication.GetDocVariable(obj, name, def);
        }


        public void RemoveDocVariable(object obj, string name)
        {
            addin.OMSApplication.RemoveDocVariable(obj, name);
        }

        public BillInfo CreateBill(string category)
        {
            BillInfo b = new BillInfo(addin.OMSApplication.GetDocVariable(addin.OMSApplication, FWBS.OMS.Interfaces.IOMSApp.ASSOCIATE, -1));
            b.Category = category;
            return b;
        }

        public void RefreshUI(object obj)
        {
            addin.RefreshUI(obj);
        }

        public bool Online
        {
            get
            {
                return Session.CurrentSession.IsLoggedIn;
            }
        }
                        
        public string GetSpecificData(string code)
        {
            return Session.CurrentSession.GetSpecificData(code).ToString();
        }

        
        public string GetCodeLookup(string type, string code)
        {
            return FWBS.OMS.CodeLookup.GetLookup(type, code);
        }
        
        public string GetResourceText(string code, string description, string help)
        {
            return Session.CurrentSession.Resources.GetResource(code, description, help).Text;
        }

        #region Recordset Methods

        public ADODB.Recordset FetchDataList(string code, string[] fieldNames, object[] values)
        {
            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            int ctr = 0;

            if (fieldNames != null)
            {
                foreach (string fld in fieldNames)
                {
                    if (String.IsNullOrEmpty(fld) == false)
                    {
                        if (values == null || values.Length <= ctr)
                            pars.Add(fld, null);
                        else
                        {
                            pars.Add(fld, values[ctr]);
                            ctr++;
                        }
                    }
                }
            }

            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(code);
            dl.ChangeParameters(pars);
            System.Data.DataTable dt = dl.Run() as System.Data.DataTable;
            return CreateRecordset(dt);
        }

        public ADODB.Recordset FetchSearchList(string code, string[] fieldNames, object[] values)
        {
            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            int ctr = 0;

            if (fieldNames != null)
            {
                foreach (string fld in fieldNames)
                {
                    if (String.IsNullOrEmpty(fld) == false)
                    {
                        if (values == null || values.Length <= ctr)
                            pars.Add(fld, null);
                        else
                        {
                            pars.Add(fld, values[ctr]);
                            ctr++;
                        }
                    }
                }
            }
            FWBS.OMS.SearchEngine.SearchList sl = new FWBS.OMS.SearchEngine.SearchList(code, null, pars);
            System.Data.DataTable dt = sl.Run() as System.Data.DataTable;
            return CreateRecordset(dt);
        }

        private ADODB.Recordset CreateRecordset(System.Data.DataTable dt)
        {
            ADODB.Recordset rst = new ADODB.RecordsetClass();

            rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            if (dt != null)
            {
                List<string> cols = new List<string>();

                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    rst.Fields.Append(col.ColumnName, TypeToADOType(col.DataType), 0, ADODB.FieldAttributeEnum.adFldUpdatable | ADODB.FieldAttributeEnum.adFldMayBeNull, null);

                    cols.Add(col.ColumnName);
                }

                rst.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockBatchOptimistic, 0);

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    rst.AddNew(System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                    foreach (string col in cols)
                    {
                        if (rst.Fields[col].Type == ADODB.DataTypeEnum.adGUID)
                        { }
                        else
                        {
                            try
                            {

                                //UTCFIX: MNW - 30/08/07 - Display local time.
                                if (row[col] is DateTime) 
                                {
                                    rst.Fields[col].Value = ((DateTime)row[col]).ToLocalTime();
                                }
                                else
                                {
                                    rst.Fields[col].Value = row[col];
                                }

                            }
                            catch { }

                        }
                    }
                }

                dt.Clear();
                dt.Dispose();

                try
                {
                    rst.MoveFirst();
                }
                catch { }

            }


            return rst;
        }


        private ADODB.DataTypeEnum TypeToADOType(object type)
        {
            if (type is UInt64)
                return ADODB.DataTypeEnum.adUnsignedBigInt;
            else if (type is UInt32)
                return ADODB.DataTypeEnum.adUnsignedInt;
            else if (type is UInt16)
                return ADODB.DataTypeEnum.adUnsignedSmallInt;
            else if (type is long)
                return ADODB.DataTypeEnum.adBigInt;
            else if (type is int)
                return ADODB.DataTypeEnum.adInteger;
            else if (type is short)
                return ADODB.DataTypeEnum.adSmallInt;
            else if (type is Byte)
                return ADODB.DataTypeEnum.adTinyInt;
            else if (type is Decimal)
                return ADODB.DataTypeEnum.adDecimal;
            else if (type is Double)
                return ADODB.DataTypeEnum.adDouble;
            else if (type is Single)
                return ADODB.DataTypeEnum.adSingle;
            else if (type is Boolean)
                return ADODB.DataTypeEnum.adBoolean;
            else if (type is Guid)
                return ADODB.DataTypeEnum.adGUID;
            else if (type is String)
                return ADODB.DataTypeEnum.adBSTR;
            else if (type is DateTime)
                return ADODB.DataTypeEnum.adDate;
            else if (type is Byte[])
                return ADODB.DataTypeEnum.adVarBinary;
            else
                return ADODB.DataTypeEnum.adVariant;
        }

        #endregion

        #region Table Routines

        public void BuildDataListTable(object obj, string code, bool includeHeader, string[] fieldNames, object[] values)
        {

            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            int ctr = 0;

            if (fieldNames != null)
            {
                foreach (string fld in fieldNames)
                {
                    if (String.IsNullOrEmpty(fld) == false)
                    {
                        if (values == null || values.Length <= ctr)
                            pars.Add(fld, null);
                        else
                        {
                            pars.Add(fld, values[ctr]);
                            ctr++;
                        }
                    }
                }
            }

            FieldParser parser = new FieldParser(addin.OMSApplication.GetCurrentAssociate(obj));
            parser.ChangeParameters(pars);
            addin.OMSApplication.BuildTable(obj, code, (System.Data.DataView)parser.Parse(FieldParser.FieldPrefixDataList + code), includeHeader);
        }
        public void BuildSearchListTable(object obj, string code, bool includeHeader, string[] fieldNames, object[] values)
        {
            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            int ctr = 0;

            if (fieldNames != null)
            {
                foreach (string fld in fieldNames)
                {
                    if (String.IsNullOrEmpty(fld) == false)
                    {
                        if (values == null || values.Length <= ctr)
                            pars.Add(fld, null);
                        else
                        {
                            pars.Add(fld, values[ctr]);
                            ctr++;
                        }
                    }
                }
            }
            FieldParser parser = new FieldParser(addin.OMSApplication.GetCurrentAssociate(obj));
            parser.ChangeParameters(pars);
            addin.OMSApplication.BuildTable(obj, code, (System.Data.DataView)parser.Parse(FieldParser.FieldPrefixSearchList + code), includeHeader);

        }

        #endregion

      

#region IManageMethods (Please Rename)

    public bool IsCompanyDocument(object doc)
    {
        return addin.OMSApplication.IsCompanyDocument(doc);
    }   

    public void AttachDocumentVars(object doc, long assocId)
    {
        Associate assoc = Associate.GetAssociate(assocId);
        addin.OMSApplication.AttachDocumentVars(doc, true, assoc);
    }

    public void AttachDocumentVariables(object doc, string clientNo, string fileNo)
    {
        if (string.IsNullOrEmpty(clientNo) || string.IsNullOrEmpty(fileNo))
            return;

        var cli = Client.GetClient(clientNo);

        System.Data.DataView files = cli.GetFiles();
        files.RowFilter = string.Format("FILENO = '{0}'", fileNo);


        if (files.Count > 0)
        {
            OMSFile f = OMSFile.GetFile((long)files[0]["FILEID"]);

            addin.OMSApplication.AttachDocumentVars(doc, true, f.DefaultAssociate);
        }
    }

    public IAssociate SelectAssociate()
    {
         Associate assoc;
        if ((Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.Null && !Session.CurrentSession.UseDefaultAssociate) || Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.False )
            assoc = FWBS.OMS.UI.Windows.Services.SelectAssociate();
        else
            assoc = FWBS.OMS.UI.Windows.Services.SelectDefaultAssociate();
        
        return assoc;
    }

    public IFile GetFile(long fileId)
    {
        return OMSFile.GetFile(fileId);
    }
    public IClient GetClient(long clientId)
    {
        return Client.GetClient(clientId);
    }


    public FWBS.OMS.Mappers.IMappingManager GetMappingManager()
    {
        return FWBS.OMS.Mappers.MappingManager.GetMappingManager;
    }

    #endregion
    }
}
