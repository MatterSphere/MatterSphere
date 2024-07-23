using System;
using System.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    //TODO: printOnlyatBranch (Query this property with Mike)
    //TODO: printXml (Query this property with Mike)

    /// <summary>
    /// 5000 Printer defined within the system.  This printer object can be used with the enquiry engine.
    /// </summary>
    public sealed class Printer : LookupTypeDescriptor, IEnquiryCompatible, IDisposable
	{
		#region Events
		public event EventHandler Dirty = null;
		#endregion

		#region Event Methods
		
        public void OnDirty()
		{
			if (Dirty != null)
				Dirty(this,EventArgs.Empty);
		}

		#endregion

		#region Fields

        private DataSet _printer = null;

        private bool isDeleted = false;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
        internal const string Sql = "select * from dbprinter";
	
		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		private const string Table = "PRINTER";


        private const string usersSQL = "select usrID, usrFullName, usrprintID from dbUser";
        private const string usersTable = "USER";

		private System.Drawing.Printing.PrinterSettings _printset = null;

        private DataTable PrinterTable
        {
            get { return _printer.Tables[Table]; }
        }

        private DataTable UsersTable
        {
            get { return _printer.Tables[usersTable]; }
        }


		#endregion

		#region Constructors


		/// <summary>
		/// Creates a new printer object.  This routine is used by the enquiry engine
		/// to create new printer object.
		/// </summary>
		public Printer()
		{
            GetTableSchemas();
            
			_printset = new System.Drawing.Printing.PrinterSettings();

            PrinterTable.Rows.Add(PrinterTable.NewRow());
		}

        private void GetTableSchemas()
        {
            string sql = Sql + " ; " + usersSQL;
            string[] tables = { Table, usersTable };

            _printer = Session.CurrentSession.Connection.ExecuteSQLDataSet(sql, true, tables, new IDataParameter[0]);

            foreach (DataTable dt in _printer.Tables)
            {
                foreach (DataColumn col in dt.Columns)
                    if (!col.AllowDBNull) col.AllowDBNull = true;
            }
        }

        internal Printer(DataTable data)
        {
            //Only used for loggin in.
            if (data == null)
                throw new ArgumentNullException("data");

            GetTableSchemas();

            _printer.Tables.Remove(_printer.Tables[Table]);
            _printer.Tables.Add(data.Copy());
            this._printset = new System.Drawing.Printing.PrinterSettings();

            //Set up a new empty record for the enquiry engine to manipulate.
            foreach (DataColumn col in PrinterTable.Columns)
                if (!col.AllowDBNull) col.AllowDBNull = true;

        }

		/// <summary>
		/// Clones a new Printer from an old one. Derr!
		/// </summary>
		/// <param name="clone">The Printer to Clone From</param>
		internal Printer(Printer clone)
		{

            GetTableSchemas();


			_printset = new System.Drawing.Printing.PrinterSettings();
			//Set up a new empty record for the enquiry engine to manipulate.
			
			PrinterTable.Rows.Add(PrinterTable.NewRow());
			foreach (DataColumn cm in PrinterTable.Columns)
			{
				if (cm.ColumnName != "printID")
					PrinterTable.Rows[0][cm.ColumnName] = clone.GetDataTable().Rows[0][cm.ColumnName];
			}
		}

		/// <summary>
		/// Initialised an existing printer object with the specified identifier.
		/// </summary>
		/// <param name="id">Printer Identifier.</param>
		[EnquiryUsage(true)]
		internal Printer(int id)
		{
            Fetch(id, null);
		}


        private void Fetch(int id, DataRow merge)
        {
            string sql = Sql + " where printID = " + id.ToString() + " ; ";
            sql += usersSQL + " where usrprintID = " + id.ToString();

            string[] tables = { Table, usersTable };

            var data = Session.CurrentSession.Connection.ExecuteSQLDataSet(sql, tables, new IDataParameter[0]);

            if (merge != null)
                Global.Merge(data.Tables[Table].Rows[0], merge);

            _printer = data;
        
            _printset = new System.Drawing.Printing.PrinterSettings();
            _printset.PrinterName = this.PrinterName;
        }

		#endregion

		#region Properties


		/// <summary>
		/// Gets the unique printer identifier.
		/// </summary>
		[LocCategory("(DETAILS)")]
		[Lookup("PRINTID")]
		public int ID
		{
			get
			{
				return (int)GetExtraInfo("printID");
			}
		}

		/// <summary>
		/// Gets or Sets the readable readable printer display name.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		[System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.PrinterLister,omsadmin")]
		public string PrinterName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("printName"));
			}
			set
			{
				SetExtraInfo("printName", value);
				try
				{
					_printset.PrinterName = this.PrinterName;
				}
				catch
				{
				}

			}
		}

		/// <summary>
		/// Gets or Sets the full UNC path name of the printer.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string UNCName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("printUNCName"));
			}
			set
			{
				SetExtraInfo("printUNCName", value);
			}
		}

		/// <summary>
		/// Gets or Sets the location / positioning of the printer within the office.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string Location
		{
			get
			{
				return Convert.ToString(GetExtraInfo("printLocation"));
			}
			set
			{
				SetExtraInfo("printLocation", value);
			}
		}

		/// <summary>
		/// Gets or Sets any extra printer description applied.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("printDescription"));
			}
			set
			{
				SetExtraInfo("printDescription", value);
			}
		}


		/// <summary>
		/// Gets or Sets the number of trays that the printer has.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		public byte Trays
		{
			get
			{
				try
				{
					return (byte)GetExtraInfo("printTrays");
				}
				catch
				{
					return 1;
				}
			}
			set
			{
				if (value <= 0) value = 1;
				SetExtraInfo("printTrays", value);
			}
		}
		
		/// <summary>
		/// Gets or Sets the copy paper tray description / code.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		public string CopyTray

		{
			get
			{
				return Convert.ToString(GetExtraInfo("printCopyTray"));
			}
			set
			{
				SetExtraInfo("printCopyTray", value);

			}
		}

		/// <summary>
		/// Gets or Sets the letterhead paper tray description / code.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		public string LetterheadTray

		{
			get
			{
				return Convert.ToString(GetExtraInfo("printLetterheadTray"));
			}
			set
			{
				SetExtraInfo("printLetterheadTray", value);

			}
		}

		/// <summary>
		/// Gets or Sets the bill paper tray description / code.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		public string BillPaperTray

		{
			get
			{
				return Convert.ToString(GetExtraInfo("printBillPaperTray"));
			}
			set
			{
				SetExtraInfo("printBillPaperTray", value);

			}
		}

		/// <summary>
		/// Gets or Sets the engrossment paper tray description / code.
		/// </summary>
		[LocCategory("TRAYS")]
		[Lookup("ENGROSSTRAY")]
		public string EngrossmentTray


		{
			get
			{
				return Convert.ToString(GetExtraInfo("printEngrossmentTray"));
			}
			set
			{
				SetExtraInfo("printEngrossmentTray", value);

			}
		}

		/// <summary>
		/// Gets or Sets the coloured paper tray description / code.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		public string ColouredTray

		{
			get
			{
				return Convert.ToString(GetExtraInfo("printColouredTray"));
			}
			set
			{
				SetExtraInfo("printColouredTray", value);

			}
			
		}

		/// <summary>
		/// Gets or Sets the default paper tray description / code.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		public string DefaultTray

		{
			get
			{
				return Convert.ToString(GetExtraInfo("printDefaultTray"));
			}
			set
			{
				SetExtraInfo("printDefaultTray", value);

			}
		}

		/// <summary>
		/// Gets or Sets the continuation paper tray description / code.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TRAYS")]
		[Lookup("CONTINUETRAY")]
		public string ContinuationTray


		{
			get
			{
				return Convert.ToString(GetExtraInfo("printContinuationTray"));
			}
			set
			{
				SetExtraInfo("printContinuationTray", value);

			}
		}


		/// <summary>
		/// Gets or Sets the InstallCMD used to switch to remote install of a printer.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("INSTALL")]
		[Lookup("INSTALLCMD")]
		public string InstallCommand
		{
			get
			{
				return Convert.ToString(GetExtraInfo("printInstallcmd"));
			}
			set
			{
				SetExtraInfo("printInstallcmd", value);
			}
		}

		[System.ComponentModel.Browsable(false)]
		public System.Drawing.Printing.PrinterSettings PrinterSettings
		{
			get
			{
				return _printset;
			}
		}

		#endregion

		#region IExtraInfo Implementation
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

            _printer.Tables[Table].Rows[0][fieldName] = val;

            OnDirty();
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public object GetExtraInfo(string fieldName)
		{

            object val = _printer.Tables[Table].Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}


		/// <summary>
		/// Returns the specified fields data.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public Type GetExtraInfoType(string fieldName)
		{
			try
			{
                return _printer.Tables[Table].Columns[fieldName].DataType;
			}
			catch (Exception ex)
			{
				throw new OMSException2("5001","Error Getting Extra Info Field %1% Probably Not Initialized",ex,true,fieldName);
			}
		}


		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public DataSet GetDataset()
		{
            return _printer.Copy() ;
		}
		
		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public DataTable GetDataTable()
		{
			return PrinterTable.Copy();
		}

        public DataTable GetUsersTable()
        {
            return UsersTable.Copy();
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsPrinterAssignedToUsers
        {
            get { return UsersTable.Rows.Count > 0; }
        }

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Gets a value indicating whether the object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsNew
		{
			get
			{
				try
				{
					return (PrinterTable.Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public void Update()
		{
			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (_printer.GetChanges()!= null)
			{

                if (isDeleted && UsersTable.GetChanges() == null)
                    MoveUsersTo(DBNull.Value);

				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
                if (PrinterTable.PrimaryKey == null || PrinterTable.PrimaryKey.Length == 0)
                    PrinterTable.PrimaryKey = new DataColumn[1] { PrinterTable.Columns["printid"] };

                if (UsersTable.PrimaryKey == null || UsersTable.PrimaryKey.Length == 0)
                    UsersTable.PrimaryKey = new DataColumn[1] { UsersTable.Columns["usrID"] };

                Session.CurrentSession.Connection.Update(UsersTable, "dbUser", true);
                Session.CurrentSession.Connection.Update(PrinterTable, "dbPrinter", true);
                isDeleted = false;
			}
		}

		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public void Refresh()
		{
			Refresh(false);
		}

		/// <summary>
		/// Gets the changes of the current object and and refreshes the object
		/// then reapplies the changes to avoid any concurrency issues.  This is in 
		/// theory forcing any changes made to the object.
		/// </summary>
		/// <param name="applyChanges">Applies / merges the current changes to the refreshed data.</param>
		public void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

            DataTable changes = _printer.Tables[Table].GetChanges();

             if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID, changes.Rows[0]);
            else
                Fetch(this.ID, null);


            isDeleted = false;
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
            _printer.RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return (_printer.GetChanges() != null);
			}
		}

		#endregion

		#region IEnquiryCompatible Implementation

		/// <summary>
		/// An event that gets raised when a property changes within the object.
		/// </summary>
		public event EnquiryEngine.PropertyChangedEventHandler PropertyChanged = null;

		/// <summary>
		/// Raises the property changed event with the specified event arguments.
		/// </summary>
		/// <param name="e">Property Changed Event Arguments.</param>
		private void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

		/// <summary>
		/// Edits the current printer object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
		}

		/// <summary>
		/// Edits the current printer object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry(customForm, Parent, this, param);
		}

		#endregion
		
		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns a string representation of the printer object which in this case is the printers friendly description.
		/// </summary>
		/// <returns>The printers full name.</returns>
		public override string ToString()
		{
			return this.PrinterName;
		}

		/// <summary>
		/// Installs the printer remotely.
		/// </summary>
		/// <returns>True if successfully installs.</returns>
		public bool Install()
		{
			if (this.InstallCommand == "") // Check Remote install command is present
				return false;

			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("RUNDLL32.EXE");
			psi.Arguments = "printui.dll,PrintUIEntry " + this.InstallCommand;
			
			System.Diagnostics.Process.Start(psi);
			return true;
		}

        public void DeleteCurrentPrinter()
        {
            PrinterTable.Rows[0].Delete();
            isDeleted = true;
        }

        public void MoveUsersTo(int printerID)
        {
            //check the printer is valid
            Printer newPrinter = new Printer(printerID);
            if (newPrinter.PrinterTable.Rows.Count == 0)
                throw new NotSupportedException("Invalid Printer selected");

            MoveUsersTo((object)printerID);

        }

        private void MoveUsersTo(object printerID)
        {
            foreach (DataRow row in UsersTable.Rows)
                row["usrprintID"] = printerID;
        }


		#endregion

		#region IDisposable Implementation

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
                if (_printer != null)
                {
                    _printer.Dispose();
                    _printer = null;
                }
			}
				

		}



		#endregion

		#region Static Methods
		/// <summary>
		/// Gets a list of valid extended data objects.
		/// </summary>
		/// <returns>A data table of Printers</returns>
		public static DataTable GetAllPrinters()
		{
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * From DBPrinter order by printName" , "PRINTERS",  new IDataParameter[0]);
			return dt;
		}

		/// <summary>
		/// Gets a list of valid extended data objects.
		/// </summary>
		/// <param name="withblank">If True will return additional row as Blank</param>
		/// <returns>A data datble of extended data items.</returns>
		public static DataTable GetPrinterList(bool withblank )
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt ;
			if (withblank)
				dt = Session.CurrentSession.Connection.ExecuteSQLTable("select null as printID, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as printName union SELECT printID,printName From DBPrinter where printOnlyatBranch is null or printOnlyatBranch = @BRANCHID order by printName" , "PRINTERS",  new IDataParameter[2] {Session.CurrentSession.Connection.AddParameter("BRANCHID", SqlDbType.Int, 4, Session.CurrentSession.ID),Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentUICulture.Name)});
			else
				dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT printID,printName From DBPrinter where printOnlyatBranch is null or printOnlyatBranch = @BRANCHID order by printName" , "PRINTERS",  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("BRANCHID", SqlDbType.Int, 4, Session.CurrentSession.ID)});

			return dt;
		}

		/// <summary>
		/// Clones a New Printer from a old One
		/// </summary>
		/// <param name="printId">Printer ID</param>
		/// <returns>Printer Object</returns>
		public static FWBS.OMS.Printer Clone(int printId)
		{
			FWBS.OMS.Printer _clone = GetPrinter(printId);
			return new FWBS.OMS.Printer(_clone);
		}

		/// <summary>
		/// Returns a Printer Object based on the Print ID
		/// </summary>
		/// <param name="printId">Printer ID.</param>
		/// <returns>Printer Object</returns>
		public static FWBS.OMS.Printer GetPrinter(int printId)
		{
			return new FWBS.OMS.Printer(printId);
		}

        public static void DeletePrinter(int printerId)
        {
            using (Printer p = Printer.GetPrinter(printerId))
            {
                p.DeleteCurrentPrinter();
                p.Update();
            }
        }

        public static void MoveUsersToNewPrinter(int fromPrinterID, int toPrinterID)
        {

            using (Printer fromPrinter = new Printer(fromPrinterID))
            {
                fromPrinter.MoveUsersTo(toPrinterID);
            }
        }


		#endregion
	}
}
