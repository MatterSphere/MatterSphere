using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace FWBS.OMS.Design.Export
{
    public class SearchList : ExportBase, IDisposable, ILinkedObjects
    {
        #region Fields
        private DataSet _dssearch = new DataSet("SEARCH");
        private string _code = "";
        private string _name = "";
        private string _desc = "";

        private XmlDocument _xmlDListViews;
        private XmlNode _xmlButton;
        private XmlNode _xmlColumn;
        private XmlNode _xmlsearchList;

        #endregion

        #region Contructors
        public SearchList(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _treeview = TreeView;
            _code = Code;

            _name = FWBS.OMS.CodeLookup.GetLookup("OMSSEARCH", _code);

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dssearch.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbSearchListConfig WHERE schCode = @Code", "SEARCH", false, paramlist));

            _desc = "Search List [" + _code + "] " + Environment.NewLine + _name + Environment.NewLine + "Version : " + Convert.ToString(_dssearch.Tables[0].Rows[0]["schVersion"]);

            string call = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schSourceCall"]);
            string omstype = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schSourceType"]);

            if (call.ToUpper().StartsWith("SELECT ") == false && omstype != "INSTANCE" && omstype != "DYNAMIC")
            {
                paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("name", call);
                _dssearch.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteProcedureTable("sprExportProcedure", "STOREPROC", false, paramlist));
                _desc += Environment.NewLine + Environment.NewLine + "Stored Procedure : " + call;
                if (_desc.Length >= 490) _desc = _desc.Substring(0, 490) + "...";
            }

            linkedobjects = new List<LinkedObject>();
            linkedobjects = FWBS.OMS.Design.Export.LinkedObjectCollector.BuildLinkedObjectList("SELECT * FROM dbOMSObjects WHERE ObjCode = @Code", _code, "OMSOBJECT");
        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            treeviewParentID = TreeView.Add(5, _code, _code, this.Active, treeviewParentID, PackageTypes.SearchLists, _desc, this.RootImportable, this.RunOnce);
            System.IO.FileInfo _filename = null;
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("SearchList");
            _directory = _directory.CreateSubdirectory(_code);

            _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");

            string enqform = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schEnquiry"]);
            if (enqform != "")
            {
                using (EnquiryForm _enquiry = new EnquiryForm(enqform, TreeView))
                {
                    _enquiry.TreeViewParentID = treeviewParentID;
                    _enquiry.RootImportable = false;
                    _enquiry.ExportTo(Directory);
                }
            }

            string script = Convert.ToString(_dssearch.Tables["SEARCH"].Rows[0]["schScript"]);
            if (script != "")
            {
                using (Scripts _script = new Scripts(script, TreeView))
                {
                    _script.TreeViewParentID = treeviewParentID;
                    _script.RootImportable = false;
                    _script.ExportTo(Directory);
                }
            }

            CodeLookupType _lookups = new CodeLookupType("OMSSEARCH", _code);

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

            string columns = "";
            foreach (XmlNode dr in _xmlColumn.ChildNodes)
            {
                string __lookup = "";
                try { __lookup = dr.SelectSingleNode("@lookup").Value; }
                catch { }
                columns += __lookup + "','";
            }
            _lookups.Add("SLCAPTION", columns.TrimEnd("',".ToCharArray()));

            string buttons = "";
            foreach (XmlNode dr in _xmlButton.ChildNodes)
            {
                string __codelookup = "";
                try { __codelookup = dr.SelectSingleNode("@lookup").Value; }
                catch { }
                buttons += __codelookup + "','";
            }
            _lookups.Add("SLBUTTON", buttons.TrimEnd("',".ToCharArray()));

            string pbuttons = "";
            foreach (XmlNode dr in _xmlButton.ChildNodes)
            {
                string __codelookup = "";
                try { __codelookup = dr.SelectSingleNode("@pnlBtnText").Value; }
                catch { }
                pbuttons += __codelookup + "','";
            }
            _lookups.Add("SLPBUTTON", pbuttons.TrimEnd("',".ToCharArray()));

            _dssearch.Tables.Add(_lookups.DataTable);
            _dssearch.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dssearch.Dispose();
        }

        #endregion
    }
}
