using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class Precedent : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dsprec = new DataSet("PRECEDENTS");
        System.IO.FileInfo _filename;
        System.IO.DirectoryInfo _root;
        #endregion

        #region Constructors
        public Precedent(string FileName)
        {
            _dsprec.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _root = _filename.Directory.Parent.Parent;
            _source = _dsprec;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            return Execute(false);
        }

        public override bool Execute(bool Copy)
        {
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbPrecedents ", "PRECEDENTS", true, new IDataParameter[0]);
            string title = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecTitle"]);
            string type = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecType"]);
            string library = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecLibrary"]);
            string category = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecCategory"]);
            string subcategory = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecSubCategory"]);
            string minorcategory = string.Empty;
            if (_dsprec.Tables["PRECEDENT"].Columns.Contains("PrecMinorCategory"))
            {
                minorcategory = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecMinorCategory"]);
            }

            string language = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecLanguage"]);

            if (this.Fieldreplacer.Goto("PRECEDENT", "PrecTitle"))
                if (this.Fieldreplacer.ChangeType == 2)
                    title = Convert.ToString(this.Fieldreplacer.ChangeValue);

            _dsprec.Tables["PRECEDENT"].Columns.Remove("PrecID");

            OnProgress("Importing Precedent : " + title);
            //Make sure that the parameters list is cleared after use.	
            IDataParameter[] paramlist = new IDataParameter[8];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("TITLE", System.Data.SqlDbType.NVarChar, 50, title);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("TYPE", System.Data.SqlDbType.NVarChar, 15, type);
            paramlist[2] = Session.CurrentSession.Connection.AddParameter("LIBRARY", System.Data.SqlDbType.NVarChar, 15, library);
            paramlist[3] = Session.CurrentSession.Connection.AddParameter("CATEGORY", System.Data.SqlDbType.NVarChar, 15, category);
            paramlist[4] = Session.CurrentSession.Connection.AddParameter("SUBCATEGORY", System.Data.SqlDbType.NVarChar, 15, subcategory);
            paramlist[5] = Session.CurrentSession.Connection.AddParameter("MINORCATEGORY", System.Data.SqlDbType.NVarChar, 15, minorcategory);
            paramlist[6] = Session.CurrentSession.Connection.AddParameter("LANGUAGE", System.Data.SqlDbType.NVarChar, 10, language);
            paramlist[7] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            DataSet _precedent = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprPrecedentMAtch", new string[1] { "MATCH" }, paramlist);

            ImportTable _import = new ImportTable(this.Fieldreplacer);

            if (_precedent != null && _precedent.Tables["MATCH"].Rows.Count == 0) // IF NO MATCH THEN ADD IT
            {
                _import.OverrideColumnValue += new SetValueOverideHandler(_import_OverrideColumnValue);
                _import.Import(_dsprec.Tables["PRECEDENT"], _table);
                Session.CurrentSession.Connection.Update(_table.Rows[0], "dbPrecedents",true,GetColumns(_table,false));
                if (Copy)
                {
                    this.CurrentRow["Code"] = _table.Rows[0]["precID"];
                }
                _import.OverrideColumnValue -= new SetValueOverideHandler(_import_OverrideColumnValue);

                // Internal Storage in the Database
                if (_dsprec.Tables["PRECEDENTSTORAGE"].Rows.Count > 0)
                {
                    DataTable _tablestg = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbPrecedentStorage", "PRECEDENTSTORAGE", true, new IDataParameter[0]);
                    _import.Import(_dsprec.Tables["PRECEDENTSTORAGE"], _tablestg);
                    Session.CurrentSession.Connection.Update(_tablestg, "SELECT * FROM dbPrecedentStorage");
                }

                var precedent = FWBS.OMS.Precedent.GetPrecedent(title, type, library, category, subcategory, language, minorcategory);
                HandleMultiPrecedents(precedent, false);
            }
            else
            {
                var precedent = FWBS.OMS.Precedent.GetPrecedent(title, type, library, category, subcategory, language, minorcategory);
                HandleMultiPrecedents(precedent, true);
                
                FWBS.OMS.Session.CurrentSession.OnWarning(this, new FWBS.OMS.MessageEventArgs(Session.CurrentSession.Resources.GetMessage("PRCLRDEXT", "Precedent : ''%1%, %2%, %3%'' already exists skipping", "", title, category, type).Text));
                return true;
            }

            string script = Convert.ToString(_dsprec.Tables["PRECEDENT"].Rows[0]["PrecScript"]);
            if (script != "")
            {
                Scripts _script = new Scripts(_root.FullName + @"\Scripts\" + script + @"\manifest.xml");
                _script.Fieldreplacer = this.Fieldreplacer;
                _script.Execute();
                _script.Dispose();
            }


            FWBS.OMS.Precedent _prec = FWBS.OMS.Precedent.GetPrecedent(title, type, library, category, subcategory, language, minorcategory);

            System.IO.FileInfo _source = new System.IO.FileInfo(_filename.DirectoryName + @"\" + Convert.ToString(_table.Rows[0]["precPath"]));
            if (_source.Exists)
            {
                try
                {
                    if (_dsprec.Tables["PRECEDENTSTORAGE"].Rows.Count == 0)
                    {
                        System.IO.FileInfo destinationFile = new System.IO.FileInfo(Session.CurrentSession.GetDirectory(_prec.DirectoryID) + "\\" + _prec.PrecedentPath);
                        if (destinationFile.Exists)
                        { 
                            //Message indicating that the file already exists
                            string precFileExistsMessage = string.Format("The file '{0}' already exists. A new file will be created for this precedent", destinationFile.FullName);
                            System.Diagnostics.Debug.WriteLine(precFileExistsMessage, "Import Precedent");

                            //Precedent goes into the standard Company folder ('Company' folder hardcoded in FileSystemStorageProvider, line 116 - GenerateVirtualDocumentPath())
                            string precFolderPath = @"Company\{0}{1}"; 
                            
                            //Update precedent details
                            _prec.PrecedentPath = string.Format(precFolderPath, _prec.ID, destinationFile.Extension);
                            _prec.Update();

                            //Create new file based on precID
                            destinationFile = new System.IO.FileInfo(Session.CurrentSession.GetDirectory(_prec.DirectoryID) + "\\" + _prec.PrecedentPath);
                            destinationFile.Directory.Create();
                            _source.CopyTo(Session.CurrentSession.GetDirectory(_prec.DirectoryID) + "\\" + _prec.PrecedentPath, true);
                        }
                        else
                        {
                            //standard behaviour, create the file as normal
                            destinationFile.Directory.Create();
                            _source.CopyTo(Session.CurrentSession.GetDirectory(_prec.DirectoryID) + "\\" + _prec.PrecedentPath, true);
                        }

                    }
                }
                catch 
                {
                    FWBS.OMS.Session.CurrentSession.OnWarning(this, new FWBS.OMS.MessageEventArgs(Session.CurrentSession.Resources.GetMessage("FLDTOCP", "Failed to Copy : ''%1%'' to ''%2%\\%3%''", "", _source.FullName, Session.CurrentSession.GetDirectory(_prec.DirectoryID).ToString(), _prec.PrecedentPath).Text));
                }
            }
            else
                FWBS.OMS.Session.CurrentSession.OnWarning(this, new FWBS.OMS.MessageEventArgs(Session.CurrentSession.Resources.GetMessage("PRCISMSGFRMPKG", "Precedent : ''%1%'' is missing from the package", "", _source.FullName).Text));
            return true;
        }


        private void HandleMultiPrecedents(OMS.Precedent precedent, bool precedentAlreadyExists)
        {
            var multiPrecedentImporter = new MultiPrecedentImporter(precedent, _dsprec, precedentAlreadyExists);
            multiPrecedentImporter.Process();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dsprec.Dispose();
        }

        #endregion

        #region Private
        private void _import_OverrideColumnValue(string name, object fromvalue, out object value)
        {
            if (name == "PrecLocation")
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(fromvalue));
                DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbStorageProvider WHERE spID = @Code", "DOCTYPE", false, paramlist);
                if (_table.Rows.Count == 0)
                {
                    DataView drv = new DataView(_dsprec.Tables["STORAGEPROVIDER"]);
                    drv.RowFilter = "spID = " + Convert.ToString(fromvalue);
                    ImportTable tb = new ImportTable(this.Fieldreplacer);
                    tb.ImportView(drv, _table);
                    Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbStorageProvider");
                }
                value = null;
            }
            else if (name == "PrecType")
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(fromvalue));
                DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbDocumentType WHERE typeCode = @Code", "DOCTYPE", false, paramlist);
                if (_table.Rows.Count == 0)
                {
                    DataView drv = new DataView(_dsprec.Tables["DOCTYPE"]);
                    drv.RowFilter = "typeCode = '" + Convert.ToString(fromvalue) + "'";
                    ImportTable tb = new ImportTable(this.Fieldreplacer);
                    tb.ImportView(drv, _table);
                    Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbDocumentType");
                }
                value = null;
            }
            else if (name == "PrecTimeRecCode" || name == "precTimeRecInCode")
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(fromvalue));
                DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbActivities WHERE actCode = @Code", "ACTIVITIES", false, paramlist);
                if (_table.Rows.Count == 0)
                {
                    DataView drv = new DataView(_dsprec.Tables["ACTIVITIES"]);
                    drv.RowFilter = "actCode = '" + Convert.ToString(fromvalue) + "'";
                    ImportTable tb = new ImportTable(this.Fieldreplacer);
                    tb.ImportView(drv, _table);
                    Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbActivities");
                }
                value = null;
            }
            else if (name == "PrecDirID")
            {
                short dir;
                Session.CurrentSession.GetDirectory("OMPRECEDENTS", out dir);
                value = dir;
            }
            else
                value = null;
        }
        #endregion
    }

    internal interface IPrecedentImporter
    {

    }

    internal static class MultiPrecedentImporterExtension
    {
        internal static string GetMultiPrecMinorCategory(this IPrecedentImporter importer, DataRow row)
        {
            string multiPrecMinorCategory = String.Empty;
            if (row.Table.Columns.Contains("multiPrecMinorCategory"))
            {
                multiPrecMinorCategory = row["multiPrecMinorCategory"].ToString();
            }

            return multiPrecMinorCategory;
        }
    }

    internal class MultiPrecedentImporter : IPrecedentImporter
    {
        #region Members

        private DataSet precedentDataSet;
        private FWBS.OMS.Precedent precedent;
        private bool precedentAlreadyExists;
        const string MULTI_PRECEDENT_SQL = "select * from dbPrecedentMulti ";
        const string MULTI_PRECEDENT_TABLE_NAME = "PRECEDENTMULTI";
        
        #endregion Members

        #region Constructors

        internal MultiPrecedentImporter(FWBS.OMS.Precedent precedent, DataSet precedentDataSet, bool precedentAlreadyExists = false)
        {
            this.precedent = precedent;
            this.precedentDataSet = precedentDataSet;
            this.precedentAlreadyExists = precedentAlreadyExists;
        }

        #endregion Constructors

        #region Methods

        internal void Process()
        {
            var multiPrecedentChildImporter = new MultiPrecedentChildImporter(precedent, precedentDataSet);

            if (PrecedentIsMultiPrecedentParent())
            {
                var multiPrecedentParentImporter = new MultiPrecedentParentImporter(precedent, precedentDataSet, precedentAlreadyExists);
                multiPrecedentParentImporter.Import();

                var childPrecedentsInDB = GetPrecedentsFromImportingDataSet();
                if (childPrecedentsInDB.Count > 0)
                {
                    multiPrecedentChildImporter.ProcessExisting(childPrecedentsInDB);
                }
            }

            multiPrecedentChildImporter.Process();
        }


        private bool PrecedentIsMultiPrecedentParent()
        {
            if (precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME] == null)
            {
                return false;
            }
            else
            {
                // Multi-precedent data of child precedents is carried within a parent manifest.
                // If there is any data in the PRECEDENTMULTI table it will indicate the precedent
                // is being used in a parent capacity.
                return precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME].Rows.Count > 0;
            }
        }

        private List<FWBS.OMS.Precedent> GetPrecedentsFromImportingDataSet()
        {
            var childPrecedentsInDB = new List<FWBS.OMS.Precedent>();

            foreach (DataRow multiPrecedentDataRow in precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME].Rows)
            {
                FWBS.OMS.Precedent childPrecedent = null;

                try
                {
                    childPrecedent = FWBS.OMS.Precedent.GetPrecedent(
                            multiPrecedentDataRow["multiPrecTitle"].ToString(),
                            multiPrecedentDataRow["multiPrecType"].ToString(),
                            multiPrecedentDataRow["multiPrecLibrary"].ToString(),
                            multiPrecedentDataRow["multiPrecCategory"].ToString(),
                            multiPrecedentDataRow["multiPrecSubCategory"].ToString(),
                            this.GetMultiPrecMinorCategory(multiPrecedentDataRow));
                }
                catch { }

                if (childPrecedent != null)
                {
                    childPrecedentsInDB.Add(childPrecedent);
                }
            }

            return childPrecedentsInDB;
        }

        #endregion Methods
    }


    internal class MultiPrecedentParentImporter
    {
        #region Members

        private DataSet precedentDataSet;
        private FWBS.OMS.Precedent precedent;
        private bool precedentAlreadyExists;
        const string MULTI_PRECEDENT_SQL = "select * from dbPrecedentMulti ";
        const string MULTI_PRECEDENT_TABLE_NAME = "PRECEDENTMULTI";

        #endregion Members

        #region Constructors

        internal MultiPrecedentParentImporter(FWBS.OMS.Precedent precedent, DataSet precedentDataSet, bool precedentAlreadyExists = false)
        {
            this.precedent = precedent;
            this.precedentDataSet = precedentDataSet;
            this.precedentAlreadyExists = precedentAlreadyExists;
        }

        #endregion Constructors

        #region Methods

        public void Import()
        {
            ProcessMultiPrecedentDataImportFromParent();
        }


        private void ProcessMultiPrecedentDataImportFromParent()
        {
            if (precedentAlreadyExists)
            {
                ProcessMultiPrecedentDataImportForExistingPrecedent();
                return;
            }

            ProcessMultiPrecedentDataImportForNewPrecedent();
        }


        private void ProcessMultiPrecedentDataImportForExistingPrecedent()
        {
            var multiPrecedentData = GetMultiPrecedentDataFromDB();
            var multiPrecedentDataForImport = GetMultiPrecedentSchemaFromDB();

            if (multiPrecedentData.Rows.Count > 0)
            {
                foreach (DataRow importingRow in precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME].Rows)
                {
                    bool ignore = false;

                    if (MultiIdExists(multiPrecedentData.Rows, importingRow["multiID"].ToString()))
                    {
                        ignore = true;
                    }

                    if (!ignore)
                    {
                        importingRow["multiMasterID"] = precedent.ID;
                        importingRow["multiChildID"] = precedent.ID;
                        multiPrecedentDataForImport.Rows.Add(importingRow.ItemArray);
                    }
                }
            }

            if (multiPrecedentDataForImport.Rows.Count > 0)
            {
                Session.CurrentSession.Connection.Update(multiPrecedentDataForImport, MULTI_PRECEDENT_SQL);
            }
        }


        private DataTable GetMultiPrecedentDataFromDB()
        {
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.AddParameter("prec", precedent.ID);
            DataTable multiPrecedentData = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbprecedentmulti where multimasterid = @PREC", "MULTIPREC", pars);
            return multiPrecedentData;
        }


        private static DataTable GetMultiPrecedentSchemaFromDB()
        {
            DataTable multiPrecedentDataForImport = Session.CurrentSession.Connection.ExecuteSQLTable(MULTI_PRECEDENT_SQL, MULTI_PRECEDENT_TABLE_NAME, true, new IDataParameter[0]);
            return multiPrecedentDataForImport;
        }


        private bool MultiIdExists(DataRowCollection multiPrecedents, string importingMultiId)
        {
            bool multiPrecedentExists = false;

            foreach (DataRow multiPrecedent in multiPrecedents)
            {
                if (multiPrecedent["multiID"].ToString() == importingMultiId)
                {
                    multiPrecedentExists = true;
                }
            }

            return multiPrecedentExists;
        }


        private void ProcessMultiPrecedentDataImportForNewPrecedent()
        {
            for (int row = 0; row <= precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME].Rows.Count - 1; row++)
            {
                precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME].Rows[row]["multiMasterID"] = precedent.ID;
                precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME].Rows[row]["multiChildID"] = precedent.ID;
            }

            var multiPrecedentCollection = new FWBS.OMS.Precedent.MultiPrecedentCollection(precedent, precedentDataSet.Tables[MULTI_PRECEDENT_TABLE_NAME]);
            if (multiPrecedentCollection != null)
                multiPrecedentCollection.Update();
        }

        #endregion Methods
    }


    internal class MultiPrecedentChildImporter : IPrecedentImporter
    {
        #region Members

        private DataSet precedentDataSet;
        private FWBS.OMS.Precedent precedent;
        const string MULTI_PRECEDENT_SQL = "select * from dbPrecedentMulti ";
        const string MULTI_PRECEDENT_TABLE_NAME = "PRECEDENTMULTI";

        #endregion Members

        #region Constructors

        internal MultiPrecedentChildImporter(FWBS.OMS.Precedent precedent, DataSet precedentDataSet)
        {
            this.precedent = precedent;
            this.precedentDataSet = precedentDataSet;
        }

        #endregion Constructors

        #region Methods

        internal void ProcessExisting(List<FWBS.OMS.Precedent> childPrecedentsInDB)
        {
            var allMultiPrecedentDataFromDB = GetMultiPrecedentDataFromDB();

            foreach (var childPrecedent in childPrecedentsInDB)
            {
                foreach (DataRow multiPrecedentRow in allMultiPrecedentDataFromDB.Rows)
                {
                    if (PrecedentIsMultiPrecedentChild(childPrecedent, multiPrecedentRow))
                    {
                        UpdateMultiPrecedentChildData(allMultiPrecedentDataFromDB, multiPrecedentRow, childPrecedent.ID);
                    }
                }
            }
        }


        internal void Process()
        {
            DataTable allMultiPrecedentDataInDB = Session.CurrentSession.Connection.ExecuteSQLTable(MULTI_PRECEDENT_SQL, MULTI_PRECEDENT_TABLE_NAME, false, new IDataParameter[0]);

            if (allMultiPrecedentDataInDB.Rows.Count > 0)
            {
                foreach (DataRow row in allMultiPrecedentDataInDB.Rows)
                {
                    if (PrecedentIsMultiPrecedentChild(precedent, row))
                    {
                        UpdateMultiPrecedentChildData(allMultiPrecedentDataInDB, row, precedent.ID);
                    }
                }
            }
        }


        private DataTable GetMultiPrecedentDataFromDB()
        {
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.AddParameter("prec", precedent.ID);
            DataTable multiPrecedentData = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbprecedentmulti where multimasterid = @PREC", "MULTIPREC", pars);
            return multiPrecedentData;
        }


        private bool PrecedentIsMultiPrecedentChild(FWBS.OMS.Precedent precedent, DataRow multiPrecedentDataRow)
        {
            bool isSameType = precedent.PrecedentType == multiPrecedentDataRow["multiPrecType"].ToString();
            bool isSameTitle = precedent.Title == multiPrecedentDataRow["multiPrecTitle"].ToString();
            bool isSameLibrary = precedent.Library == multiPrecedentDataRow["multiPrecLibrary"].ToString();
            bool isSameCategory = precedent.Category == multiPrecedentDataRow["multiPrecCategory"].ToString();
            bool isSameSubCategory = precedent.SubCategory == multiPrecedentDataRow["multiPrecSubCategory"].ToString();
            bool isSameMinorCategory = precedent.MinorCategory == this.GetMultiPrecMinorCategory(multiPrecedentDataRow);
            // bool isSameLanguage = No language field present in the multi-precedents table to validate against

            if (isSameType
                && isSameTitle
                && isSameLibrary
                && isSameCategory
                && isSameSubCategory
                && isSameMinorCategory)
            {
                return true;
            }

            return false;
        }


        private void UpdateMultiPrecedentChildData(DataTable multiPrecedentData, DataRow row, long multiPrecedentID)
        {
            row["multiChildID"] = multiPrecedentID;

            if (multiPrecedentData.GetChanges() != null)
            {
                Session.CurrentSession.Connection.Update(multiPrecedentData, MULTI_PRECEDENT_SQL + " where multiID = " + row["multiID"].ToString());
            }
        }

        #endregion Methods
    }
}
