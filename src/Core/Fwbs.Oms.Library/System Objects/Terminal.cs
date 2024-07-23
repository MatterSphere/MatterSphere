using System;
using System.Data;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{

    /// <summary>
    /// 3000 Registered Terminal object.  This Terminal object can be used with the enquiry engine.
    /// </summary>
    public sealed class Terminal : LookupTypeDescriptor, IEnquiryCompatible, IDisposable
	{

		#region Fields

		/// <summary>
		/// Internal data store.
		/// </summary>
		private DataTable _terminal = null;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql =  "select * from dbTerminal";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "TERMINAL";

		/// <summary>
		/// The terminal id will never change as it is the unique identifeir of the row.  Any change in this will result in
		/// an entirely different.
		/// </summary>
		private string _id = "";

		
		/// <summary>
		/// Override Table Is Dirty
		/// </summary>
		private bool _isdirty = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new terminal object.  This routine is used by the enquiry engine
		/// to create new terminal object.
		/// </summary>
		public Terminal()
		{
			_terminal = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
			
			//Set the default values.
			_terminal.Columns["termloggedin"].DefaultValue = false;
			if (_terminal.Columns.Contains("brid"))
                _terminal.Columns["brid"].DefaultValue = Session.CurrentSession.ID;


			//Set up a new empty record for the enquiry engine to manipulate.
			foreach (DataColumn col in _terminal.Columns)
				if (!col.AllowDBNull) col.AllowDBNull = true;

			_terminal.Rows.Add(_terminal.NewRow());

			BuildXML();

		}


        /// <summary>
        /// Initialised an existing terminal object with the specified terminla name.
        /// </summary>
        /// <param name="terminalName">Terminal Name.</param>
        internal Terminal (string terminalName)
		{
			Fetch(terminalName, null);

			BuildXML();
		}

        internal Terminal(DataTable data)
        {
            //Only used for loggin in.
            if (data == null)
                throw new ArgumentNullException("data");

            this._terminal = data.Copy();

            SetDefaults(_terminal, Common.Functions.GetMachineName());

            BuildXML();
              
        }

		/// <summary>
		/// Fetches the terminal without constructing a new object.
		/// </summary>
		/// <param name="terminalName"></param>
        /// <param name="merge"></param>
		private void Fetch(string terminalName, DataRow merge)
		{
			DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where termName = '" + SQLRoutines.RemoveRubbish(terminalName) + "'", Table, new IDataParameter[0]);
            SetDefaults(data, terminalName);

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _terminal = data;
		}

        private static void SetDefaults(DataTable data, string terminalName)
        {
            if (data.Rows.Count == 0)
            {
                DataRow row = data.NewRow();
                row["termName"] = terminalName;
                row["brid"] = Session.CurrentSession.ID;
                row["termloggedin"] = false;
                data.Rows.Add(row);
            }

            if (data.PrimaryKey == null || data.PrimaryKey.Length == 0)
                data.PrimaryKey = new DataColumn[1] { data.Columns["termname"] };

        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Net Bios Terminal name.
		/// </summary>
		public string TerminalName
		{
			get
			{
				if (_id == String.Empty || _id == null)
					_id = Convert.ToString(_terminal.Rows[0]["termname"]);

				return _id;
			}
		}


		/// <summary>
		/// Gets a flag specifying whether the terminal is currently logged into the system.
		/// </summary>
		public bool IsLoggedIn
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("termLoggedIn");
				}
				catch
				{
					return false;
				}
			}
		}


		/// <summary>
		/// Gets a boolean value specifying whether this ternminal is registered / licensed to use the system.
		/// </summary>
		public bool IsRegistered
		{
			get
			{
				return true;
			}
		}


		/// <summary>
		/// Gets the last user object that was logged into the system through this specific terminal.
		/// </summary>
		[EnquiryUsage(true)]
		public string LastUser
		{
			get
			{
				try
				{
					return Convert.ToString(GetExtraInfo("termLastUserInits"));
				}
				catch
				{
					return Convert.ToString(GetExtraInfo("termLastUser"));
				}
			}
		}

		/// <summary>
		/// Gets the date and time that this terminal was last logged into the system.
		/// </summary>
		[EnquiryUsage(true)]
		public DateTimeNULL LastLoggedIn
		{
			get
			{
				return ConvertDef.ToDateTimeNULL(GetExtraInfo("termLastLogin"), DBNull.Value);
			}
		}



		/// <summary>
		/// Gets the last OMS engine version used on the terminal.
		/// </summary>
		public Version EngineVersion
		{
			get
			{
				Version ver = null;
				try
				{
					string val = Convert.ToString(GetExtraInfo("termVersion"));
					ver = new Version(val);
				}
				catch
				{
					ver = new Version(0, 0, 0, 0);
				}

				return ver;
			}

		}

		/// <summary>
		/// Gets or Sets the terminal license key to log into PostCode Anywhere.
		/// </summary>
		public string PCALicenseKey
		{
			get
			{
				return Convert.ToString(GetXmlProperty("pcaLicenseKey", ""));
			}
			set
			{
				SetXmlProperty("pcaLicenseKey", value);
			}
		}
		
		#endregion

		#region Methods

		internal void SetVersionInfo(Version version)
		{
			try
			{
				SetExtraInfo("termVersion", version);
			}
			catch{}	
		}

		public void Delete()
		{
			_terminal.Rows[0].Delete();
		}



		/// <summary>
		/// Returns the text representation of the terminal object.  In this case it returns the terminals name.
		/// </summary>
		/// <returns>The terminal name.</returns>
		public override string ToString()
		{
			return this.TerminalName;
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

			_terminal.Rows[0][fieldName] = val;
		}


		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public object GetExtraInfo(string fieldName)
		{
			object val = _terminal.Rows[0][fieldName];
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
				return _terminal.Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("3001","Error Getting Extra Info Field " + fieldName + " Probably Not Initialized");
			}
		}
		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public DataSet GetDataset()
		{
			DataSet ds = new DataSet();
			ds.Tables.Add (GetDataTable());
			return ds;
		}

		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public DataTable GetDataTable()
		{
			return _terminal.Copy();
		}

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Gets a value indicating whether the object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		public bool IsNew
		{
			get
			{
				try
				{
					return (_terminal.Rows[0].RowState == DataRowState.Added);
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
			DataRow row = _terminal.Rows[0];

			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (IsDirty)
			{
				if (row.RowState != DataRowState.Deleted)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					if (_terminal.PrimaryKey == null || _terminal.PrimaryKey.Length == 0)
						_terminal.PrimaryKey = new DataColumn[1]{_terminal.Columns["termname"]};

                    if (xmlprops != null)xmlprops.Update();
				}


				Session.CurrentSession.Connection.Update(row, "dbterminal");
				_isdirty = false;
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

			DataTable changes = _terminal.GetChanges();

			xmlprops = null;

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.TerminalName, changes.Rows[0]);
            else
                Fetch(this.TerminalName, null);
		}
	
		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
			xmlprops = null;
			_terminal.RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return (_isdirty || _terminal.GetChanges() != null);
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
		/// Edits the current terminal object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
		}

		/// <summary>
		/// Edits the current terminal object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
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
		public object Parent
		{
			get
			{
				return null;
			}
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
                if (_terminal != null)
                {
                    _terminal.Dispose();
                    _terminal = null;
                }
			}
		}


		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a registered terminal based on ther terminalname given.
		/// </summary>
		/// <param name="terminalName">Terminal name.</param>
		/// <returns>A terminal object.</returns>
		public static Terminal GetTerminal (string terminalName)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Terminal(terminalName);
		}

		public static DataTable GetTerminals()
		{
			Session.CurrentSession.CheckLoggedIn();
			return Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where brid = " + Convert.ToString(Session.CurrentSession.ID),"TERMINALS",false,new IDataParameter[0]);
		}


		#endregion

		#region XML Settings Methods

		private XmlProperties xmlprops = null;

		
		private void BuildXML()
		{
            if (xmlprops == null)
			xmlprops = new XmlProperties(this, "termXML");
		}

		public object GetXmlProperty(string name, object defaultValue)
		{
            BuildXML();
            return xmlprops.GetProperty(name, defaultValue);
		}

		public void SetXmlProperty(string name, object val)
		{
			BuildXML();
            if (xmlprops.SetProperty(name, val))
                _isdirty = true;
		}

	

		#endregion
	}
}
