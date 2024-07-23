using System;
using System.Data;
using System.Xml;

namespace FWBS.OMS.Design.Import
{
    internal class SearchList : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dssearch = new DataSet("SEARCH");
        System.IO.FileInfo _filename;
        System.IO.DirectoryInfo _root;
        private XmlDocument _xmlDListViews;
        private XmlNode _xmlButton;
        private XmlNode _xmlColumn;
        private XmlNode _xmlsearchList;
        #endregion

        #region Constructors
        public SearchList(string FileName)
        {
            _dssearch.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _root = _filename.Directory.Parent.Parent;
            _source = _dssearch;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schCode"]));
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbSearchListConfig where schCode = @Code", "SEARCH", false, paramlist);

            bool create = true;
            long sourceversion = 0;
            long destinversion = Convert.ToInt64(_dssearch.Tables["SEARCH"].Rows[0]["schVersion"]);
            if (_table.Rows.Count > 0) sourceversion = Convert.ToInt64(_table.Rows[0]["schVersion"]);

            long destinuser = (long)FWBS.OMS.SystemUsers.Admin;
            long sourceuser = destinuser;
            if (_table.Rows.Count > 0) sourceuser = FWBS.Common.ConvertDef.ToInt64(_table.Rows[0]["UpdatedBy"], Session.CurrentSession.Administrator.ID);
            string code = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schCode"]);

            if (sourceuser != destinuser || (sourceuser == destinuser && destinversion < sourceversion))
            {
                string sourceusername = "{Unknown}";
                try { sourceusername = new User(Convert.ToInt32(sourceuser)).FullName; }
                catch { }
                AskEventArgs askargs = null;
                if (sourceuser != destinuser)
                    askargs = new AskEventArgs("ALLRDYEXISTV2", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?" + Environment.NewLine + Environment.NewLine + "Last Modified By : %5%", "", FWBS.OMS.AskResult.No, "Search List", code, sourceversion.ToString(), destinversion.ToString(), sourceusername);
                else
                    askargs = new AskEventArgs("ALLRDYEXISTV1", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?", "", FWBS.OMS.AskResult.No, "Search List", code, sourceversion.ToString(), destinversion.ToString());
                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }

            if (create)
            {

                OnProgress("Importing Search List : " + Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schCode"]));

                ImportTable _import = new ImportTable(this.Fieldreplacer);

                string enqform = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schEnquiry"]);
                if (enqform != "")
                {
                    using (EnquiryForm _enquiry = new EnquiryForm(_root.FullName + @"\EnquiryForms\" + enqform + @"\manifest.xml"))
                    {
                        _enquiry.Fieldreplacer = this.Fieldreplacer;
                        if (!_enquiry.Execute())
                            return false;
                    }
                }

                string script = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schScript"]);
                if (script != "")
                {
                    using (Scripts _script = new Scripts(_root.FullName + @"\Scripts\" + script + @"\manifest.xml"))
                    {
                        _script.Fieldreplacer = this.Fieldreplacer;
                        _script.Execute();
                    }
                }

                _import.OverrideColumnValue += new SetValueOverideHandler(_import_OverrideColumnValue);
                if (_table.Rows.Count > 0)
                    _import.ImportRowOver(_dssearch.Tables["SEARCH"], _table, 0);
                else
                    _import.Import(_dssearch.Tables["SEARCH"], _table);

                CodeLookupType _lookup = new CodeLookupType(_dssearch.Tables["CODELOOKUPS"].Copy());
                _lookup.Execute();
                _lookup.Dispose();

                //
                // XML Search List Main Heading
                //
                _xmlDListViews = new XmlDocument();
                _xmlDListViews.PreserveWhitespace = false;
                _xmlDListViews.LoadXml(Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schListView"]));

                //
                // XML If the Base Element searchList does not exist create it.
                //
                _xmlsearchList = _xmlDListViews.DocumentElement.SelectSingleNode("/searchList");
                if (_xmlsearchList == null)
                {
                    _xmlsearchList = _xmlDListViews.CreateElement("", "searchList", "");
                    _xmlDListViews.AppendChild(_xmlsearchList);
                }

                //
                // XML Columns
                //
                _xmlColumn = _xmlDListViews.DocumentElement.SelectSingleNode("/searchList/listView");
                if (_xmlColumn == null)
                {
                    _xmlColumn = _xmlDListViews.CreateElement("", "listView", "");
                    _xmlsearchList.AppendChild(_xmlColumn);
                }

                //
                // XML Buttons
                //
                _xmlButton = _xmlDListViews.DocumentElement.SelectSingleNode("/searchList/buttons");
                if (_xmlButton == null)
                {
                    _xmlButton = _xmlDListViews.CreateElement("", "buttons", "");
                    _xmlsearchList.AppendChild(_xmlButton);
                }

                Session.CurrentSession.Connection.Update(_table, "select * from dbSearchListConfig");

                return true;
            }
            return false;

        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dssearch.Dispose();
        }

        #endregion

        private void _import_OverrideColumnValue(string name, object fromvalue, out object value)
        {
            if (name == "schActive")
                value = fromvalue;
            else
                value = null;
        }
    }
}
