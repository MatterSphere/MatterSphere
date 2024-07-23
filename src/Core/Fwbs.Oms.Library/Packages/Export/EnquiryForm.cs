using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class EnquiryForm : ExportBase, IDisposable, ILinkedObjects
    {
        #region Fields
        private EnquiryEngine.Enquiry _enquiry;
        private System.Data.DataSet _dataset;
        private string _code = "";
        private string _name = "";
        private string _desc = "";

        #endregion

        #region Constructors
        public EnquiryForm(string EnquiryForm, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _treeview = TreeView;
            _enquiry = EnquiryEngine.Enquiry.GetEnquiryInDesign(EnquiryForm);
            _dataset = _enquiry.Source;
            _code = EnquiryForm;

            _name = Convert.ToString(_enquiry.Source.Tables["ENQUIRY"].Rows[0]["enqdesc"]);
            _desc = "Screen [" + _code + "] " + Environment.NewLine + Environment.NewLine + _name + Environment.NewLine + Environment.NewLine + "Version : " + Convert.ToString(_enquiry.Version);

            linkedobjects = new List<LinkedObject>();
            linkedobjects = FWBS.OMS.Design.Export.LinkedObjectCollector.BuildLinkedObjectList("SELECT * FROM dbOMSObjects WHERE ObjCode = @Code", _code, "OMSOBJECT");
        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            treeviewParentID = TreeView.Add(7, _code, _code, this.Active, treeviewParentID, PackageTypes.EnquiryForms, _desc, this.RootImportable, this.RunOnce);


            DataView findpage = new DataView(_dataset.Tables["PAGES"]);
            DataView find = new DataView(_dataset.Tables["QUESTIONS"]);
            for (int i = _dataset.Tables["CONTROLS"].Rows.Count - 1; i >= 0; i--)
            {
                find.RowFilter = "[quctrlid] = " + Convert.ToString(_dataset.Tables["CONTROLS"].Rows[i]["ctrlid"]);
                if (find.Count == 0) _dataset.Tables["CONTROLS"].Rows.RemoveAt(i);
            }

            for (int i = _dataset.Tables["LISTS"].Rows.Count - 1; i >= 0; i--)
            {
                find.RowFilter = "[quDataList] = '" + Convert.ToString(_dataset.Tables["LISTS"].Rows[i]["enqTable"] + "'");
                if (find.Count == 0) _dataset.Tables["LISTS"].Rows.RemoveAt(i);
            }

            _dataset.Tables.Remove("FORMS");

            for (int i = _dataset.Tables["EXTENDEDDATA"].Rows.Count - 1; i >= 0; i--)
            {
                find.RowFilter = "[quExtendedData] = '" + Convert.ToString(_dataset.Tables["EXTENDEDDATA"].Rows[i]["extcode"] + "'");
                if (find.Count == 0)
                    _dataset.Tables["EXTENDEDDATA"].Rows.RemoveAt(i);
                else
                {
                    ExtendedData _extdata = new ExtendedData(Convert.ToString(_dataset.Tables["EXTENDEDDATA"].Rows[i]["extcode"]), TreeView);
                    _extdata.TreeViewParentID = treeviewParentID;
                    _extdata.RootImportable = false;
                    _extdata.ExportTo(Directory);
                    _extdata.Dispose();
                }
            }

            _dataset.Tables.Remove("DATA");

            for (int i = _dataset.Tables["LOOKUPS"].Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQQUESTION")
                {
                    find.RowFilter = "[quCode] = '" + Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"] + "'");
                    if (find.Count == 0) _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
                else if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQPAGE")
                {
                    findpage.RowFilter = "[pgeCode] = '" + Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"] + "'");
                    if (findpage.Count == 0 &&
                        Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqWelcomeHeaderCode"]) != Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"]))
                        _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
                else if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQHEADER")
                {
                    if (Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqCode"]) != Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"]))
                        _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
                else if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQWELCOME")
                {
                    if (Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqWelcomeTextCode"]) != Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"]))
                        _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
                else if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQDATALIST")
                {
                    _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
                else if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQCOMMAND")
                {
                    find.RowFilter = "[quCommand] = '" + Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"] + "'");
                    if (find.Count == 0) _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
                else if (Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdType"]) == "ENQQUESTCUETXT")
                {
                    find.RowFilter = "[quFilter] like '%CueTextCode=\"" + Convert.ToString(_dataset.Tables["LOOKUPS"].Rows[i]["cdCode"] + "\"%'");
                    if (find.Count == 0) _dataset.Tables["LOOKUPS"].Rows.RemoveAt(i);
                }
            }

            for (int i = _dataset.Tables["COMMANDS"].Rows.Count - 1; i >= 0; i--)
            {
                find.RowFilter = "[quCommand] = '" + Convert.ToString(_dataset.Tables["COMMANDS"].Rows[i]["cmdCode"] + "'");
                if (find.Count == 0) _dataset.Tables["COMMANDS"].Rows.RemoveAt(i);
            }


            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", _dataset.Tables["ENQUIRY"].Rows[0]["enqID"]);
            _dataset.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbEnquiryDataSource WHERE enqID = @Code", "DATASOURCE", false, paramlist));

            _dataset.Tables.Remove("TABLES");


            System.IO.DirectoryInfo _directory = null;
            _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("EnquiryForms");
            _directory = _directory.CreateSubdirectory(_code);

            DataView questview = new DataView(_dataset.Tables["QUESTIONS"]);
            questview.RowFilter = "quDataList not is null";
            foreach (DataRowView drv in questview)
            {
                try
                {
                    DataLists _datals = new DataLists(Convert.ToString(drv["quDataList"]), TreeView);
                    _datals.TreeViewParentID = treeviewParentID;
                    _datals.RootImportable = false;
                    _datals.ExportTo(Directory);
                    _datals.Dispose();
                }
                catch (System.Data.ConstraintException)
                {
                }

            }

            string script = Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqScript"]);
            if (script != "")
            {
                Scripts _script = new Scripts(script, TreeView);
                _script.TreeViewParentID = treeviewParentID;
                _script.RootImportable = false;
                _script.ExportTo(Directory);
                _script.Dispose();
            }

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dataset.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dataset.Dispose();
            _enquiry.Dispose();
        }
        #endregion
    }
}
