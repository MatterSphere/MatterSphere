
using System;
using System.ComponentModel;
using System.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{

    //TODO: Not currently using br1LineAdd, this may be found out from the address object itself.
    //TODO: Default country ID, perhaps a country object, this is mainly used for address formats.
    //TODO: brLegalAidRef

    /// <summary>
    /// 20000 Main Branch Object.  This branch object can be used with the enquiry engine.
    /// </summary>
    [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.BranchEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
    public class Branch : LookupTypeDescriptor, IEnquiryCompatible, IDisposable
	{
		#region Fields

		/// <summary>
		/// Internal data object.
		/// </summary>
		private DataTable _branch = null;

		/// <summary>
		/// Address object that holds this branch instance address information.
		/// </summary>
		private Address _address = null;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbbranch";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "BRANCH";

		
		/// <summary>
		/// Override Table Is Dirty
		/// </summary>
		private bool _isdirty = false;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Creates a new branch object.  This routine is used by the enquiry engine
		/// to create new branch.
		/// </summary>
		internal Branch()
		{
			_branch = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
			
			//Set up a new empty record for the enquiry engine to manipulate.
			foreach (DataColumn col in _branch.Columns)
				if (!col.AllowDBNull) col.AllowDBNull = true;
            
			_branch.Rows.Add(_branch.NewRow());

            this.OnExtLoaded();
        }
		

		/// <summary>
		/// Initialised an existing branch object with the specified identifier.
		/// </summary>
		/// <param name="id">Branch Identifier.</param>
		[EnquiryUsage(true)]
		public Branch (int id)
		{
			SetDataSource(id);

            this.OnExtLoaded();
        }

		/// <summary>
		/// All derived classes like Session should call this constructor because of the enquiry
		/// compatibility issues of the enquiry engine creating new objects with its default constructor.
		/// </summary>
		/// <param name="derived">True if a derived class creates this as its base class.</param>
		protected Branch (bool derived)
		{

        }

		#endregion

		#region Destructors

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
		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_branch != null)
				{
					_branch.Dispose();
					_branch = null;
				}

				_address = null;
			}
			
			//Free any unmanaged objects.

		}



        #endregion

        #region Properties

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <remarks></remarks>
        public ObjectState State
        {
            get
            {
                try
                {
                    switch (_branch.Rows[0].RowState)
                    {
                        case DataRowState.Added:
                            return ObjectState.Added;
                        case DataRowState.Modified:
                            return ObjectState.Modified;
                        case DataRowState.Deleted:
                            return ObjectState.Deleted;
                        case DataRowState.Unchanged:
                            return ObjectState.Unchanged;
                        default:
                            return ObjectState.Unitialised;
                    }
                }
                catch
                {
                    return ObjectState.Unitialised;
                }
            }
        }

        /// <summary>
        /// Gets the unqiue branch identifier.
        /// </summary>
        public int ID
		{
			get
			{
				return (int)GetExtraInfo("brid");
			}
		}

		/// <summary>
		/// Gets or Sets the branch name, e.g, Milton Keynes.
		/// </summary>
		[EnquiryUsage(true)]
		public string BranchName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brName"));
			}
			set
			{
				SetExtraInfo("brName", value);
			}
		}

		/// <summary>
		/// Gets or Sets if the Branch has Interactive Law
		/// </summary>
		[EnquiryUsage(true)]
		public bool Interactive
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("brIntActive"));
			}
			set
			{
				SetExtraInfo("brIntActive", value);
			}
		}
		
		/// <summary>
		/// Gets or Sets Default Interactive Security Settings for New Accounts
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public string InteractiveSecurity
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brIntDefaultSecurity"));
			}
			set
			{
				SetExtraInfo("brIntDefaultSecurity", value);
			}
		}
		
		/// <summary>
		/// Gets or Sets a brief branch description.
		/// </summary>
		[EnquiryUsage(true)]
		public string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brDescription"));
			}
			set
			{
				SetExtraInfo("brDescription", value);
			}
		}

		/// <summary>
		/// Gets or Sets the address details of the branch in the form of an address object.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public Address Address
		{
			get
			{
				if (_address == null)
				{
					_address = Address.GetAddress(Common.ConvertDef.ToInt64(GetExtraInfo("braddID"), 0));
				}

				return _address;
			}
			set
			{
				_address = value;
				if (_address != null)
					SetExtraInfo("braddid", _address.ID);
			}
		}


		/// <summary>
		/// Gets or Sets the branch telephone number.
		/// </summary>
		[EnquiryUsage(true)]
		public string Telephone
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brTelephone"));
			}
			set
			{
				SetExtraInfo("brTelephone", value);
			}
		}

		/// <summary>
		/// Gets or Sets the branches Fax Number.
		/// </summary>
		[EnquiryUsage(true)]
		public string Fax
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brFax"));
			}
			set
			{
				SetExtraInfo("brFax", value);
			}
		}


		/// <summary>
		/// Gets or Sets the branches VAT registration number.
		/// </summary>
		[EnquiryUsage(true)]
		public string VATNumber
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brvatno"));
			}
			set
			{
				SetExtraInfo("brvatno", value);
			}
		}


		/// <summary>
		/// Gets or Sets the website address for this occurence of the branch.  All branches may use the same address.
		/// </summary>
		[EnquiryUsage(true)]
		public string Website
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brWebsite"));
			}
			set
			{
				SetExtraInfo("brWebsite", value);
			}
		}


		/// <summary>
		/// Gets or Sets the branches Intranet Address.
		/// </summary>
		[EnquiryUsage(true)]
		public string Intranet
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brIntranet"));
			}
			set
			{
				SetExtraInfo("brIntranet", value);
			}
		}


		/// <summary>
		/// Gets the head office status of this branch, only one branch within the company can be the head office.
		/// </summary>
		[EnquiryUsage(true)]
		public bool IsHeadOffice
		{
			get
			{
				try
				{
					return (bool) GetExtraInfo("brHeadOffice");
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("brHeadOffice", value);
			}
		}

		/// <summary>
		/// Gets the UI culture string ID (en-GB) for the branch.  This will only be accessed by the
		/// Session class and not outside.  This is because the session object will determine
		/// the currently used UICulture setting by checking whether one exists on the
		/// branch, reginfo or user.
		/// </summary>
		[EnquiryUsage(false)]
		[Browsable(false)]
		public string PreferedCulture
		{
			get
			{
				if (GetExtraInfo("brUICultureInfo") is DBNull)
					return "";
				else
				{
					try
					{
						string culture = Convert.ToString(GetExtraInfo("brUICultureInfo"));
						System.Globalization.CultureInfo.CreateSpecificCulture(culture);
						return culture;
					}
					catch
					{
						return "";
					}
				}
			}
		}

		/// <summary>
		/// Gets or Sets the branch specific currency.
		/// </summary>
		[EnquiryUsage(true)]
		internal string Currency
		{
			get
			{
				return Convert.ToString(GetExtraInfo("brcurISOCode"));
			}
			set
			{
				SetExtraInfo("brcurISOCode", value);
			}
		}

		/// <summary>
		/// Gets a user object for the manager of the branch.
		/// </summary>
		[EnquiryUsage(false)]
		[Browsable(false)]
		public User Manager
		{
			get
			{
				if (GetExtraInfo("brManager") is DBNull)
					return null;
				else
					return new User((int)GetExtraInfo("brManager"));
			}
		}

		/// <summary>
		/// Gets or Sets the overriding company signature of the Branch.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public Signature CompanySignature
		{
            get
            {
                if (GetExtraInfo("brSignature") == DBNull.Value)
                    return Signature.Empty;
                else
                    return new Signature((byte[])GetExtraInfo("brSignature"), "company");
            }
            set
            {
                if (value == null || value.Equals(Signature.Empty) || value == Signature.Empty)
                    SetExtraInfo("brSignature", DBNull.Value);
                else
                    SetExtraInfo("brSignature", value.ToByteArray());
            }
		}

		/// <summary>
		/// Gets or Sets the default sharepoint server location.
		/// </summary>
		[EnquiryUsage(true)]
		public string SPSServer
		{
			get
			{
				return Convert.ToString(GetXmlProperty("spsServer", ""));
			}
			set
			{
				SetXmlProperty("spsServer", value);
			}
		}

		/// <summary>
		/// Gets or Sets the default sharepoint user name credential.
		/// </summary>
		[EnquiryUsage(true)]
		public string SPSUserName
		{
			get
			{
				return Convert.ToString(GetXmlProperty("spsUserName", ""));
			}
			set
			{
				SetXmlProperty("spsUserName", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Search Priority for FileAccCode First or not.
		/// </summary>
		[EnquiryUsage(true)]
		public bool SchFileAccFirst
		{
			get
			{
				return Convert.ToBoolean(GetXmlProperty("searchfileacccodefirst", "false"));
			}
			set
			{
				SetXmlProperty("searchfileacccodefirst", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Search Button Visible on the OMSType Window.
		/// </summary>
		[EnquiryUsage(true)]
		public bool DisableSearchButton
		{
			get
			{
				return Convert.ToBoolean(GetXmlProperty("disablesearchbutton", "false"));
			}
			set
			{
				SetXmlProperty("disablesearchbutton", value);
			}
		}

		/// <summary>
		/// Gets or Sets the default sharepoint password credential.
		/// </summary>
		[EnquiryUsage(true)]
		public string SPSPassword
		{
			get
			{
				return Convert.ToString(GetXmlProperty("spsPassword", ""));
			}
			set
			{
				SetXmlProperty("spsPassword", value);
			}
		}

        /// <summary>
        /// Gets or Sets the visibility of button Add New in Matter List Tile in Dashboard.
        /// </summary>
        [EnquiryUsage(true)]
        public bool HideAddNewForMatterListTile
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("hideAddNewForMatterListTile", "false"));
            }
            set
            {
                SetXmlProperty("hideAddNewForMatterListTile", value);
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Sets the datasource of the branch from a derived class, in this case the Session object gets created
        /// when the branch id is not yet known.
        /// </summary>
        /// <param name="id">Branch id</param>
        private void SetDataSource(int id)
		{
            Fetch(id, null);
		}


        private void Fetch(int id, DataRow merge)
        {
            var data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where brid = " + id.ToString(), Table, new IDataParameter[0]);
            xmlprops = null;
            if ((data == null) || (data.Rows.Count == 0))
                throw new OMSException(HelpIndexes.InvalidBranchSet);

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _branch = data;
            timestamp = DateTime.UtcNow;
        }

		/// <summary>
		/// Sets the datasource of the branch from a derived class, in this case the Session object gets created
		/// when the branch id is not yet known.
		/// </summary>
		/// <param name="dt">Already formed data table.</param>
		protected void SetDataSource(DataTable dt)
		{
			if (dt == null)
				throw new OMSException(HelpIndexes.InvalidBranchSet);

			_branch = dt.Copy();
            xmlprops = null;

			if (_branch.Rows.Count == 0) 
				throw new OMSException(HelpIndexes.InvalidBranchSet);

            timestamp = DateTime.UtcNow;

			_branch.TableName = Table;
			_branch.AcceptChanges();
		}

		/// <summary>
		/// Returns the string representation of the branch.  In this case it is the branches name.
		/// </summary>
		/// <returns>The branches name.</returns>
		public override string ToString()
		{
			return this.BranchName;
		}

		#endregion

		#region IExtraInfo Implementation
	
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public void SetExtraInfo(string fieldName, object val)
		{
            this.SetExtraInfo(_branch.Rows[0], fieldName, val);
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public object GetExtraInfo(string fieldName)
		{
			object val = _branch.Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		/// <summary>
		/// Returns the specified fields type.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _branch.Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("20001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}

		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public DataSet GetDataset()
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(GetDataTable());
			return ds;
		}

		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public DataTable GetDataTable()
		{
			return _branch.Copy();
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
					return (_branch.Rows[0].RowState == DataRowState.Added);
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
		public virtual void Update()
		{
			DataRow row = _branch.Rows[0];

			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (IsDirty)
			{
                ObjectState state = State;
                if (this.OnExtCreatingUpdatingOrDeleting(state))
                    return;

				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				if (row.RowState != DataRowState.Deleted)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					if (_branch.PrimaryKey == null || _branch.PrimaryKey.Length == 0)
						_branch.PrimaryKey = new DataColumn[1]{_branch.Columns["brid"]};

					if(xmlprops != null) xmlprops.Update();
				}

				Session.CurrentSession.Connection.Update(row, "dbbranch");

                this.OnExtCreatedUpdatedDeleted(state);
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
		public virtual void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

            this.OnExtRefreshing();

			DataTable changes = _branch.GetChanges();

			xmlprops = null;

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID, changes.Rows[0]);
            else
                Fetch(this.ID, null);

            timestamp = DateTime.UtcNow;

            this.OnExtRefreshed();
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public virtual void Cancel()
		{
			xmlprops = null;
			_branch.RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		[EnquiryUsage(false)]
		[Browsable(false)]
		public virtual bool IsDirty
		{
			get
			{
				return (_isdirty || _branch.GetChanges() != null);
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
		protected void OnPropertyChanged (FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

		/// <summary>
		/// Edits the current branch object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public  Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.BranchEdit), param);
		}

		/// <summary>
		/// Edits the current branch object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
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
		[EnquiryUsage(false)]
		[Browsable(false)]
		public object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Internal Enquiry Exposed properties

		/// <summary>
		/// Gets or sets the default address for the branch.  This could be a long value
		/// indicating a pointer to the address, it could be an address object containing the 
		/// address information to be inserted / updated.  The address can be unassigned
		/// by giving the property a null reference or a DBNull.
		/// </summary>
		[EnquiryUsage(true)]
		internal object DefaultAddress
		{
			get
			{
				return Address;
			}
			set
			{
				if (value == null)
				{
					SetExtraInfo("braddid", DBNull.Value);
				}
				else if (value == Address.Null)
				{
					SetExtraInfo("braddid", DBNull.Value);
				}
				else if (value is long)
				{
					SetExtraInfo("braddid", value);
				}
				else if (value is DBNull)
				{
					SetExtraInfo("braddid", DBNull.Value);
				}
				else if (value is FWBS.OMS.Address)
				{
					_address = (FWBS.OMS.Address)value;
				}
			}
		}

		#endregion

		#region Static
		public static DataTable GetBranches()
		{
			IDataParameter[] pars = new IDataParameter[0];
			return Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM DBBranch", "BRANCHES", pars); 
		}
		#endregion

		#region XML Settings Methods

		private XmlProperties xmlprops;

        protected void BuildXML()
		{
			//Create the document if it does not already exist.
            if (xmlprops == null)
                xmlprops = new XmlProperties(this, "brXml");
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


        private DateTime timestamp;
        public DateTime TimeStamp
        {
            get
            {
                return timestamp;
            }
        }

    }


}
