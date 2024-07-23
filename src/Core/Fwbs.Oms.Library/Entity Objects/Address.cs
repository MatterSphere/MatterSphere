using System;
using System.Data;
using System.Net;
using System.Threading;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;


namespace FWBS.OMS
{

    /// <summary>
    /// Defines an address object. This address object can be used with the enquiry engine
    /// but would be quite fixed in its structure.  Use the eAddress enquiry control to extend
    /// the addresses visual format.
    /// </summary>
    /// <remarks></remarks>
	public sealed class Address : IEnquiryCompatible, IDisposable, IConvertible
	{

		#region Fields

        /// <summary>
        /// A null address object.
        /// </summary>
		public static Address Null = new Address();

        /// <summary>
        /// Internal data source.
        /// </summary>
		private DataTable _address = null;

        /// <summary>
        /// 
        /// </summary>
		private string _addressformat = null;

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string Sql = "select * from dbaddress";

        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
		public const string Table = "ADDRESS";

        /// <summary>
        /// Holds a temprorary variable that describes the type of address.
        /// This is used in contact addresses.
        /// </summary>
		private string _addtype = "";

		#endregion

		#region Constructors

        /// <summary>
        /// Creates a new address object.  This routine is used by the enquiry engine
        /// to create new address object.
        /// If type initialiser throws an exception its because the oms user does not have a default branch.
        /// </summary>
        /// <remarks></remarks>
		public Address()
		{
            bool check = Session.CurrentSession._skiplogincheck;

            try
            {
                Session.CurrentSession._skiplogincheck = true;

                _address = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);

               
                _address.Columns["addCountry"].DefaultValue = GetDefaultCountryId();
                //Set up a new empty record for the enquiry engine to manipulate.
                foreach (DataColumn col in _address.Columns)
                    if (!col.AllowDBNull) col.AllowDBNull = true;

                _address.Rows.Add(_address.NewRow());

                //Set the created by and created date of the item.
                SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
                SetExtraInfo("Created", DateTime.Now);

                this.OnExtLoaded();
            }
            finally
            {
                Session.CurrentSession._skiplogincheck = check;
            }
		}

        /// <summary>
        /// Gets the default country id.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int GetDefaultCountryId()
        {
            int countryid = 223;

            if (Session.CurrentSession.CurrentFeeEarner != null && Session.CurrentSession.CurrentFeeEarner.Branch != null)
            {
                Address branchAddress = Session.CurrentSession.CurrentFeeEarner.Branch.Address;
                if (branchAddress != null)
                    countryid = branchAddress.CountryID;
            }

             return countryid;

        }

        /// <summary>
        /// Initialised an existing address object with the specified identifier.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Address (long id)
		{
            Fetch(id, null);
		}


        /// <summary>
        /// Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="merge">The merge.</param>
        /// <remarks></remarks>
        private void Fetch(long id, DataRow merge)
        {
            DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where addID = " + id.ToString(), Table, new IDataParameter[0]);
            if ((data == null) || (data.Rows.Count == 0))
                throw new OMSException(HelpIndexes.AddressNotAssigned);

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _address = data;

            this.OnExtLoaded();
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
                    switch (_address.Rows[0].RowState)
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
        /// Gets a value indicating whether the contact object is new and needs to be
        /// updated to exist in the database.
        /// </summary>
        /// <remarks></remarks>
		public bool IsNew
		{
			get
			{
				try
				{
					return (_address.Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}


        /// <summary>
        /// Gets the one line.
        /// </summary>
        /// <remarks></remarks>
        public string OneLine
        {
            get
            {
                return this.GetAddressString(", ");
            }
        }

        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <remarks></remarks>
        public string Lines
        {
            get
            {
                return this.GetAddressString(Environment.NewLine);
            }
        }


        /// <summary>
        /// Gets the unqiue addres pointer identifier.
        /// </summary>
        /// <remarks></remarks>
		public long ID
		{
			get
			{
				return (long)GetExtraInfo("addid");
			}
		}
        /// <summary>
        /// Gets the first line of the address.
        /// </summary>
        /// <value>The line1.</value>
        /// <remarks></remarks>
		public string Line1
		{
			get
			{
				return Convert.ToString(GetExtraInfo("addLine1"));
			}
			set
			{
				SetExtraInfo("addLine1",value);
			}

		}

        /// <summary>
        /// Gets the second line of the address.
        /// </summary>
        /// <value>The line2.</value>
        /// <remarks></remarks>
		public string Line2
		{
			get
			{
				return Convert.ToString(GetExtraInfo("addLine2"));
			}
			set
			{
				SetExtraInfo("addLine2",value);
			}

		}

        /// <summary>
        /// Gets the third line of the address, this could be a Town name.
        /// </summary>
        /// <value>The line3.</value>
        /// <remarks></remarks>
		public string Line3
		{
			get
			{
				return Convert.ToString(GetExtraInfo("addLine3"));
			}
			set
			{
				SetExtraInfo("addLine3",value);
			}

		}

        /// <summary>
        /// Gets the fourth line of the address, this could be a City name.
        /// </summary>
        /// <value>The line4.</value>
        /// <remarks></remarks>
		public string Line4
		{
			get
			{
				return Convert.ToString(GetExtraInfo("addLine4"));
			}
			set
			{
				SetExtraInfo("addLine4",value);
			}
		}

        /// <summary>
        /// Gets the fifth line of the address, this could be a county name.
        /// </summary>
        /// <value>The line5.</value>
        /// <remarks></remarks>
		public string Line5
		{
			get
			{
				return Convert.ToString(GetExtraInfo("addLine5"));
			}
			set
			{
				SetExtraInfo("addLine5",value);
			}

		}

        /// <summary>
        /// Gets the post code / zip code / mailing code of the address.
        /// This will tend to be a searchable field.
        /// </summary>
        /// <value>The postcode.</value>
        /// <remarks></remarks>
		public string Postcode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("addPostcode"));
			}
			set
			{
				SetExtraInfo("addPostcode",value);
			}

		}


        /// <summary>
        /// Gets/Sets the AddressFormat for Leters
        /// </summary>
        /// <remarks></remarks>
		public string AddressFormat
		{
			get
			{
				if (_addressformat != null)
					return _addressformat;
				else
				{
					this.GetDisplayFormat();
					return _addressformat;
				}
			}
			// Set code needs to write back to the Countries table bit dodgy.
		}

        /// <summary>
        /// Gets the country in it's identifier form.
        /// </summary>
        /// <value>The country ID.</value>
        /// <remarks></remarks>
		public int CountryID
		{
			get
			{
				return Common.ConvertDef.ToInt32(GetExtraInfo("addCountry"), -1);
			}
			set
			{
				SetExtraInfo("addCountry",value);
			}
		}

        /// <summary>
        /// Gets or Sets the country name that the address is using.
        /// </summary>
        /// <value>The country.</value>
        /// <remarks></remarks>
		public string Country
		{
			get
			{
				if (CountryID == -1)
					return Convert.ToString(GetExtraInfo("addCountryOld"));
				else
					return GetCountryName(CountryID);
			}
			set
			{
				CountryID = Session.CurrentSession.GetCountryIDByName(value);
			}
		}

        /// <summary>
        /// Gets or Sets the country Code that the address is using.
        /// </summary>
        /// <value>The country code.</value>
        /// <remarks></remarks>
        public string CountryCode
        {
            get
            {
                if (CountryID == -1)
                    return Convert.ToString(GetExtraInfo("addCountryOld"));
                else
                    return GetCountryCode(CountryID);
            }
            set
            {
                CountryID = Session.CurrentSession.GetCountryIDByCode(value);
            }
        }

        /// <summary>
        /// Set the Add Type Code, not persisted in the Address Table but available
        /// for storage in the link table
        /// </summary>
        /// <value>The type of the add.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string AddType
		{
			get
			{
				return _addtype;
			}
			set
			{
				_addtype = value;
			}
		}


        /// <summary>
        /// Gets or Sets the current address object to the one specified.
        /// </summary>
        /// <value>The address as format.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Address AddressAsFormat
		{
			get
			{
				return this;
			}
			set
			{
				DataTable add = value.GetDataTable();
				if (add == _address || add == null)
					return;
				else
				{
					foreach (DataColumn col in add.Columns)
					{
						
						if (_address.Columns[col.ColumnName].ReadOnly == false)
							_address.Rows[0][col.ColumnName] = add.Rows[0][col];
					}
				}
			}
		}

        /// <summary>
        /// Gets or Sets the current address object to the one specified copying ID also.
        /// </summary>
        /// <value>The address as clone.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Address AddressAsClone
		{
			get
			{
				return this;
			}
			set
			{
				DataTable add = value.GetDataTable();
				if (add == _address || add == null)
					return;
				else
				{
					_address = add;
				}
			}
		}


		#endregion

		#region Methods

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
		public Address Clone()
		{
			Address a = new Address();
			a.AddType = this.AddType;
			a.CountryID = this.CountryID;
			a.Line1 = this.Line1;
			a.Line2 = this.Line2;
			a.Line3 = this.Line3;
			a.Line4 = this.Line4;
			a.Line5 = this.Line5;
			a.Postcode = this.Postcode;
			return a;
		}


        /// <summary>
        /// Overrides the object equals method to compare to address values.
        /// </summary>
        /// <param name="obj">Object being compared.</param>
        /// <returns>True if the objects are the same.</returns>
        /// <remarks></remarks>
		public override bool Equals (object obj)
		{
			//Modified to check every field in the case of an edit
			if (obj is Address)
			{
				Address a = (Address)obj;
				if(a.AddType != this.AddType || a.CountryID != this.CountryID || a.Line1 != this.Line1 || a.Line2 != this.Line2 || a.Line3 != this.Line3 
					|| a.Line4 != this.Line4 || a.Line5 != this.Line5 || a.Postcode != this.Postcode || a.ID != this.ID)
					return false;
				else
					return true;
			}	
				
			else
                return false;
		}

        /// <summary>
        /// Override the the GetHashCode
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks></remarks>
		public override int GetHashCode()
		{
			return (int)this.ID;
		}


        /// <summary>
        /// Returns the whole address as a string.
        /// </summary>
        /// <returns>String representation of the address.</returns>
        /// <remarks></remarks>
		public override string ToString()
		{
			string ret = "";
			if (this.Line1 != "")
				ret += this.Line1;
			if (this.Line2 != "")
				ret += Environment.NewLine + this.Line2;
			if (this.Line3 != "")
				ret += Environment.NewLine + this.Line3;
			if (this.Line4 != "")
				ret += Environment.NewLine + this.Line4;
			if (this.Line5 != "")
				ret += Environment.NewLine + this.Line5;
			if (this.Postcode != "")
				ret += Environment.NewLine + this.Postcode;
			return ret;
		}

        /// <summary>
        /// Returns the address as a multi line string.  Newlines are removed if blank.
        /// </summary>
        /// <returns>A multi line address.</returns>
        /// <remarks></remarks>
		public string GetAddressString()
		{
			return this.GetAddressString(Environment.NewLine);
		}

        /// <summary>
        /// Returns a one line address, separated with a comma, , use GetAddressString(delimiter) if you wish to change the delimiter.
        /// </summary>
        /// <returns>One Line address using , as a delimiter.</returns>
        /// <remarks></remarks>
		public string GetOneLineAddress()
		{
			return this.GetAddressString(", ");
		}



        /// <summary>
        /// Returns the address as a multiline string using a specified delimiter.
        /// </summary>
        /// <param name="delimiter">Example: ', ' will work like 1 High Street, Stamford</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public string GetAddressString(string delimiter)
		{
			
			if (this.AddressFormat == null || this.AddressFormat.Trim() == "")
			{
				// Old way of formatting the string
				System.Text.StringBuilder ret = new System.Text.StringBuilder();

				ret.Append(this.Line1.Trim());
				ret.Append(delimiter);
				ret.Append(this.Line2.Trim());
				ret.Append(delimiter);
				ret.Replace(delimiter + delimiter,delimiter);
				ret.Append(this.Line3.Trim());
				ret.Append(delimiter);
				ret.Replace(delimiter + delimiter,delimiter);
				ret.Append(this.Line4.Trim());
				ret.Append(delimiter);
				ret.Replace(delimiter + delimiter,delimiter);
				ret.Append(this.Line5.Trim());
				ret.Append(delimiter);
				ret.Replace(delimiter + delimiter,delimiter);
				ret.Append(this.Postcode.Trim());

				int ctryid = this.CountryID;
				int brctryid = Session.CurrentSession.CurrentBranch.Address.CountryID;
				if (ctryid != -1 && ctryid != brctryid)
				{
					string ctry = this.Country.Trim();
					if (ctry != "")
					{
						ret.Append(delimiter);
						ret.Replace(delimiter + delimiter,delimiter);
						ret.Append(ctry);
					}
				}

				return ret.ToString();
			}
			else
			{
				// Text Layout system
				string _addressstring;
				_addressstring = this.AddressFormat.Trim();
				_addressstring = _addressstring.Replace("DELIMITER", delimiter);
				_addressstring = _addressstring.Replace("LINE1", this.Line1.Trim());
				_addressstring = _addressstring.Replace("LINE2", this.Line2.Trim());
				_addressstring = _addressstring.Replace("LINE3", this.Line3.Trim());
				_addressstring = _addressstring.Replace("LINE4", this.Line4.Trim());
				_addressstring = _addressstring.Replace("LINE5", this.Line5.Trim());
				_addressstring = _addressstring.Replace("ZIPCODE", this.Postcode.Trim());
				_addressstring = _addressstring.Replace("POSTCODE", this.Postcode.Trim());
				

				int ctryid = this.CountryID;
				int brctryid = Session.CurrentSession.CurrentBranch.Address.CountryID;
                if (ctryid != -1 && ctryid != brctryid)
                {
                    string ctry = this.Country.Trim();
                    if (ctry != "")
                    {
                        // Country Logic if in the Same Country then dont write on the address format but make blank
                        _addressstring = _addressstring.Replace("COUNTRY", ctry).Trim();
                    }
                    else
                    {
                        // Country Logic if in the Same Country then dont write on the address format but make blank
                        _addressstring = _addressstring.Replace("COUNTRY", "").Trim();
                    }
                }
                else
                {
                    _addressstring = _addressstring.Replace("COUNTRY", "").Trim();
                }

				
				_addressstring = _addressstring.Replace(delimiter + " ", delimiter);
				_addressstring = _addressstring.Replace("   ", " ");
				_addressstring = _addressstring.Replace("  ", " ");

				_addressstring = _addressstring.Replace(delimiter + delimiter, delimiter);
				_addressstring = _addressstring.Replace(" " + delimiter + delimiter, delimiter);
				_addressstring = _addressstring.Replace(" " + delimiter + " " + delimiter, delimiter);
				_addressstring = _addressstring.Replace(delimiter + " " + delimiter, delimiter);

				_addressstring = _addressstring.Replace(", , , ,", ", ");
				_addressstring = _addressstring.Replace(", , ,", ", ");
				_addressstring = _addressstring.Replace(", , ", ", ");
				_addressstring = _addressstring.Replace(delimiter + ", ", delimiter);
				_addressstring = _addressstring.Replace(delimiter + " , ", delimiter);
				_addressstring = _addressstring.Replace(", " + delimiter, delimiter);
				_addressstring = _addressstring.Replace(" , " + delimiter, delimiter);

                _addressstring = _addressstring.Replace(" ,", ",");
                
				return _addressstring;
			}

	}




        /// <summary>
        /// Gets a data table with the correct labels etc.. for displaying an international address.
        /// The format of this data table is compatible to run through a Form Renderer.
        /// </summary>
        /// <returns>A data table object.</returns>
        /// <remarks></remarks>
		public DataTable GetDisplayFormat()
		{
			DataTable dt;
			dt = Address.GetAddressFormat(this.CountryID);
			if (dt.Columns.Contains("AddressFormat") && dt.Rows.Count > 0 )
			{
				_addressformat = Convert.ToString(dt.Rows[0]["AddressFormat"]);
			}
			return Address.GetAddressFormat(this.CountryID);
		}

        /// <summary>
        /// Gets a valid list of countries in the current localized language.
        /// </summary>
        /// <returns>A data table.</returns>
        /// <remarks></remarks>
		public DataTable GetCountries()
		{
			return Session.CurrentSession.GetCountries();
		}

	

		#endregion

		#region IExtraInfo Implementaion

        /// <summary>
        /// Sets the raw internal data object with the specified value under the specified field name.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="val">Value to use.</param>
        /// <remarks></remarks>
		public void SetExtraInfo (string fieldName, object val)
		{
            this.SetExtraInfo(_address.Rows[0], fieldName, val);
		}

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName)
		{
            if (_address.Rows[0][fieldName] is System.DBNull)
                return "";
            else
            {
                object val =_address.Rows[0][fieldName];
                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
            }
		}

        /// <summary>
        /// Returns the specified fields type.
        /// </summary>
        /// <param name="fieldName">Field Name.</param>
        /// <returns>Type of Field.</returns>
        /// <remarks></remarks>
		public Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _address.Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("23001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        /// <remarks></remarks>
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
        /// <remarks></remarks>
		public DataTable GetDataTable()
		{
			return _address;
		}

		#endregion

		#region IUpdateable Implementaion

        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>
        /// <remarks></remarks>
		public void Update()
		{
			if (this != Null)
			{
                ObjectState state = State;
                if (this.OnExtCreatingUpdatingOrDeleting(state))
                    return;
				
				DataRow row = _address.Rows[0];

				//Check if there are any changes made before setting the updated
				//and updated by properties then update.
				if (_address.GetChanges() != null)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					if (_address.PrimaryKey == null || _address.PrimaryKey.Length == 0)
						_address.PrimaryKey = new DataColumn[1]{_address.Columns["addid"]};

					SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
					SetExtraInfo("Updated", DateTime.Now);
						
					Session.CurrentSession.Connection.Update(row, "dbAddress");

                    this.OnExtCreatedUpdatedDeleted(state);
				}
			}
		}


        /// <summary>
        /// Validates the address so that it tries to find an existing address with the same information.
        /// </summary>
        /// <remarks></remarks>
		public void Match()
		{
			if (this != Null)
			{
	
				//Added DMB 6/2/2004 Need to check an address with the same characteristics does not already exist
				// as existing code will produce duplicate records particularly when editing
				IDataParameter [] pars = new IDataParameter[7];
				pars[0] = Session.CurrentSession.Connection.AddParameter("@A1",System.Data.SqlDbType.NVarChar,50, Line1);
				pars[1] = Session.CurrentSession.Connection.AddParameter("@A2",System.Data.SqlDbType.NVarChar,50, Line2);
				pars[2] = Session.CurrentSession.Connection.AddParameter("@A3",System.Data.SqlDbType.NVarChar,50, Line3);
				pars[3] = Session.CurrentSession.Connection.AddParameter("@A4",System.Data.SqlDbType.NVarChar,50, Line4);
				pars[4] = Session.CurrentSession.Connection.AddParameter("@A5",System.Data.SqlDbType.NVarChar,50, Line5);
				pars[5] = Session.CurrentSession.Connection.AddParameter("@A6",System.Data.SqlDbType.NVarChar,15, Postcode);
				pars[6] = Session.CurrentSession.Connection.AddParameter("@A7",System.Data.SqlDbType.Int,4,CountryID);

				string sqlselect = Sql + " where coalesce(addline1, '') =@A1 and coalesce(addline2, '') =@A2 and coalesce(addline3, '') =@A3 and coalesce(addline4, '') =@A4 and coalesce(addline5, '') =@A5 and coalesce(addpostcode, '') =@A6 and coalesce(addcountry, -1) =@A7";
					
				DataTable newTable = Session.CurrentSession.Connection.ExecuteSQLTable(sqlselect,Table,pars);
				if (newTable.Rows.Count > 0)
				{
					_address.Columns["addid"].ReadOnly = false;
					_address.Rows[0].ItemArray = newTable.Rows[0].ItemArray;
					_address.Columns["addid"].ReadOnly = true;
					_address.AcceptChanges();
				}
			}
		}

        /// <summary>
        /// Refreshes the current object with the one from the database to prevent
        /// any potential concurrency issues.
        /// </summary>
        /// <remarks></remarks>
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
        /// <remarks></remarks>
		public void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

            this.OnExtRefreshing();

            DataTable changes = _address.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID, changes.Rows[0]);
            else
                Fetch(this.ID, null);

            this.OnExtRefreshed();
		}

        /// <summary>
        /// Cancels any changes made to the object.
        /// </summary>
        /// <remarks></remarks>
		public void Cancel()
		{
			_address.RejectChanges();
		}

        /// <summary>
        /// Gets a boolean flag indicating whether any changes have been made to the object.
        /// </summary>
        /// <remarks></remarks>
		public bool IsDirty
		{
			get
			{
				return (_address.GetChanges() != null);
			}
		}

		#endregion

		#region IEnquiryCompatible Implementaion

        /// <summary>
        /// An event that gets raised when a property changes within the object.
        /// </summary>
        /// <remarks></remarks>
		public event EnquiryEngine.PropertyChangedEventHandler PropertyChanged = null;

        /// <summary>
        /// Raises the property changed event with the specified event arguments.
        /// </summary>
        /// <param name="e">Property Changed Event Arguments.</param>
        /// <remarks></remarks>
		private void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

        /// <summary>
        /// Edits the current address object in the form of an enquiry (if the database states that is edit compatible).
        /// </summary>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
		}

        /// <summary>
        /// Edits the current address object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
        /// </summary>
        /// <param name="customForm">Enquiry form code.</param>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry(customForm, Parent, this, param);
		}


		#endregion

		#region IParent Implementation

        /// <summary>
        /// Gets the parent related object.
        /// </summary>
        /// <remarks></remarks>
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
        /// <remarks></remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        /// <remarks></remarks>
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
                if (_address != null)
                {
                    _address.Dispose();
                    _address = null;
                }
			}
		
		}

		
		#endregion

		#region Static Methods
        /// <summary>
        /// Gets the localized country name from an id value.
        /// </summary>
        /// <param name="countryID">The country ID.</param>
        /// <returns>The country name.</returns>
        /// <remarks></remarks>
		static public string GetCountryName(int countryID)
		{
			Session.CurrentSession.CheckLoggedIn();
			string sql = "select dbo.GetCodeLookupDesc('COUNTRIES', ctrycode, @UI) as [name] from dbcountry where ctryid = @CTRYID";
			IDataParameter [] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			pars[1] = Session.CurrentSession.Connection.AddParameter("CTRYID", countryID);
			return Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar(sql, pars));

		}

        /// <summary>
        /// Gets the country code.
        /// </summary>
        /// <param name="countryID">The country ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static public string GetCountryCode(int countryID)
        {
            Session.CurrentSession.CheckLoggedIn();
            string sql = "select ctryCode from dbCountry where ctryid = @CTRYID";
            IDataParameter[] sqlparams = new IDataParameter[1];
            sqlparams[0] = Session.CurrentSession.Connection.AddParameter("CTRYID", countryID);
            return Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar(sql, sqlparams));
        }

        /// <summary>
        /// Returns an address object based on the identifier passed.
        /// </summary>
        /// <param name="addID">Address Identifier.</param>
        /// <returns>Structured Address Object.</returns>
        /// <remarks></remarks>
		static public Address GetAddress(long addID)
		{
			Session.CurrentSession.CheckLoggedIn();
			if (addID == 0)
				return Null;
			else
				return new Address(addID);
		}

        /// <summary>
        /// Returns an address format for a specified country.
        /// </summary>
        /// <param name="countryID">Country Identifier.</param>
        /// <returns>DataTable with the data in.</returns>
        /// <remarks></remarks>
		static public DataTable GetAddressFormat(int countryID)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Country", System.Data.SqlDbType.Int, 15, countryID);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprAddressFormat", "ADDRESSFORMAT", paramlist);
			return dt;
		}

        /// <summary>
        /// Gets the table schema of the address object.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
		static public DataTable GetAddressTableSchema()
		{
			Session.CurrentSession.CheckLoggedIn();
			return Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
		}



        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static Address GetAddress(string id)
		{
			Session.CurrentSession.CheckLoggedIn();
			Session.CurrentSession.ValidateLicensedFor("POSTCODE");
			if (id == String.Empty) return null;

			DataSet ds = null;
			if (Session.CurrentSession.PCAWebServiceURL != "DEMO")
			{
				PostcodeAnywhere.LookupUK pca = new PostcodeAnywhere.LookupUK();

				pca.Url = Session.CurrentSession.PCAWebServiceURL;
                pca.Proxy = GetProxy(pca.Url);
				string PCAAccountCode = Session.CurrentSession.PCAAccountCode;
				string PCALicenseKey = Session.CurrentSession.PCALicenseKey;
				string PCAMachineId = Session.CurrentSession.PCAMachineId;

				if (Session.CurrentSession.PCATerminalLicense)
				{
					PCALicenseKey = Session.CurrentSession.CurrentTerminal.PCALicenseKey;
					PCAMachineId = Session.CurrentSession.CurrentTerminal.TerminalName;
				}

				ds = pca.FetchAddress_DataSet(id.ToString(),PostcodeAnywhere.enLanguage.enLanguageEnglish, PostcodeAnywhere.enContentType.enContentStandardAddress, PCAAccountCode, PCALicenseKey, PCAMachineId);
				CheckPCAError(ds);
			}
			else
			{
				ds = CreateDemoAddress();
				ds.Tables["DATA"].DefaultView.RowFilter = "id <> " + id.Replace("'", "''");
				for (int ctr = ds.Tables["DATA"].DefaultView.Count - 1; ctr > -1; ctr--)
				{
					DataRowView r = ds.Tables["DATA"].DefaultView[ctr];
					r.Delete();
				}
				ds.AcceptChanges();
				ds.Tables["DATA"].DefaultView.RowFilter = "";
			}
			
			return CreatePCAAddress(ds.Tables["DATA"]);
		}

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <param name="building">The building.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static Address GetAddress(string postcode, string building)
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = FetchByBuildingAndPostCode(postcode, building);
			return CreatePCAAddress(dt);
		}

        /// <summary>
        /// Fetches the by building and post code.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <param name="building">The building.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static System.Data.DataTable FetchByBuildingAndPostCode(string postcode, string building)
		{
			Session.CurrentSession.CheckLoggedIn();
			Session.CurrentSession.ValidateLicensedFor("POSTCODE");

			DataSet ds = null;
			if (Session.CurrentSession.PCAWebServiceURL != "DEMO")
			{
				PostcodeAnywhere.LookupUK pca = new PostcodeAnywhere.LookupUK();

				pca.Url = Session.CurrentSession.PCAWebServiceURL;
                pca.Proxy = GetProxy(pca.Url);
				string PCAAccountCode = Session.CurrentSession.PCAAccountCode;
				string PCALicenseKey = Session.CurrentSession.PCALicenseKey;
				string PCAMachineId = Session.CurrentSession.PCAMachineId;

				if (Session.CurrentSession.PCATerminalLicense)
				{
					PCALicenseKey = Session.CurrentSession.CurrentTerminal.PCALicenseKey;
					PCAMachineId = Session.CurrentSession.CurrentTerminal.TerminalName;
				}


				ds = pca.FastAddress_DataSet(postcode, building, PostcodeAnywhere.enLanguage.enLanguageEnglish, PostcodeAnywhere.enContentType.enContentStandardAddress, PCAAccountCode, PCALicenseKey, PCAMachineId);
				CheckPCAError(ds);
			}
			else
			{
				ds = CreateDemoAddress();
				ds.Tables["DATA"].DefaultView.RowFilter = "building <> '" + building.Replace("'", "''") + "' or postcode <> '" + postcode.Trim(' ').Replace("'", "''") + "'";
				for (int ctr = ds.Tables["DATA"].DefaultView.Count - 1; ctr > -1; ctr--)
				{
					DataRowView r = ds.Tables["DATA"].DefaultView[ctr];
					r.Delete();
				}
				ds.AcceptChanges();
				ds.Tables["DATA"].DefaultView.RowFilter = "";
			}
			
			return ds.Tables["DATA"];
		}

        /// <summary>
        /// Fetches the by building.
        /// </summary>
        /// <param name="building">The building.</param>
        /// <param name="town">The town.</param>
        /// <param name="justBuilding">if set to <c>true</c> [just building].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static System.Data.DataTable FetchByBuilding(string building, string town, bool justBuilding)
		{
			Session.CurrentSession.CheckLoggedIn();
			Session.CurrentSession.ValidateLicensedFor("POSTCODE");
			
			DataSet ds = null;
			if (Session.CurrentSession.PCAWebServiceURL != "DEMO")
			{
				PostcodeAnywhere.LookupUK pca = new PostcodeAnywhere.LookupUK();

				pca.Url = Session.CurrentSession.PCAWebServiceURL;
                pca.Proxy = GetProxy(pca.Url);
				string PCAAccountCode = Session.CurrentSession.PCAAccountCode;
				string PCALicenseKey = Session.CurrentSession.PCALicenseKey;
				string PCAMachineId = Session.CurrentSession.PCAMachineId;

				if (Session.CurrentSession.PCATerminalLicense)
				{
					PCALicenseKey = Session.CurrentSession.CurrentTerminal.PCALicenseKey;
					PCAMachineId = Session.CurrentSession.CurrentTerminal.TerminalName;
				}

				ds = pca.ByBuilding_Dataset(building, justBuilding, town, PCAAccountCode, PCALicenseKey, PCAMachineId);
				CheckPCAError(ds);
			}
			else
				throw new OMSException2("ERNOADDSCHINDEM", "Search not avaialable in demo.");
			
			return ds.Tables["DATA"];
		}

        /// <summary>
        /// Fetches the by postcode.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static System.Data.DataTable FetchByPostcode(string postcode)
		{
			Session.CurrentSession.CheckLoggedIn();
			Session.CurrentSession.ValidateLicensedFor("POSTCODE");

			DataSet ds = null;
			if (Session.CurrentSession.PCAWebServiceURL != "DEMO")
			{
				PostcodeAnywhere.LookupUK pca = new PostcodeAnywhere.LookupUK();

				pca.Url = Session.CurrentSession.PCAWebServiceURL;
                pca.Proxy = GetProxy(pca.Url);
				string PCAAccountCode = Session.CurrentSession.PCAAccountCode;
				string PCALicenseKey = Session.CurrentSession.PCALicenseKey;
				string PCAMachineId = Session.CurrentSession.PCAMachineId;

				if (Session.CurrentSession.PCATerminalLicense)
				{
					PCALicenseKey = Session.CurrentSession.CurrentTerminal.PCALicenseKey;
					PCAMachineId = Session.CurrentSession.CurrentTerminal.TerminalName;
				}

				ds = pca.ByPostcode_DataSet(postcode, PCAAccountCode, PCALicenseKey, PCAMachineId);
				CheckPCAError(ds);
			}
			else
			{
				ds = CreateDemoAddress();
				ds.Tables["DATA"].DefaultView.RowFilter = "postcode <> '" + postcode.Trim(' ').Replace("'", "''") + "'";
				for (int ctr = ds.Tables["DATA"].DefaultView.Count - 1; ctr > -1; ctr--)
				{
					DataRowView r = ds.Tables["DATA"].DefaultView[ctr];
					r.Delete();
				}
				ds.AcceptChanges();
				ds.Tables["DATA"].DefaultView.RowFilter = "";
			}
			
			return ds.Tables["DATA"];
		}

        /// <summary>
        /// Fetches the by organisation.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        /// <param name="town">The town.</param>
        /// <param name="fuzzy">if set to <c>true</c> [fuzzy].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static System.Data.DataTable FetchByOrganisation(string organisation, string town, bool fuzzy)
		{
			Session.CurrentSession.CheckLoggedIn();
			Session.CurrentSession.ValidateLicensedFor("POSTCODE");
			
			DataSet ds = null;
			if (Session.CurrentSession.PCAWebServiceURL != "DEMO")
			{
				PostcodeAnywhere.LookupUK pca = new PostcodeAnywhere.LookupUK();

				pca.Url = Session.CurrentSession.PCAWebServiceURL;
                pca.Proxy = GetProxy(pca.Url);
				string PCAAccountCode = Session.CurrentSession.PCAAccountCode;
				string PCALicenseKey = Session.CurrentSession.PCALicenseKey;
				string PCAMachineId = Session.CurrentSession.PCAMachineId;

				if (Session.CurrentSession.PCATerminalLicense)
				{
					PCALicenseKey = Session.CurrentSession.CurrentTerminal.PCALicenseKey;
					PCAMachineId = Session.CurrentSession.CurrentTerminal.TerminalName;
				}

				ds = pca.ByOrganisation_DataSet(organisation, town, fuzzy, PCAAccountCode, PCALicenseKey, PCAMachineId);
				CheckPCAError(ds);
			}
			else
                throw new OMSException2("ERNOADDSCHINDEM", "Search not avaialable in demo.");

			return ds.Tables["DATA"];
		}

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static IWebProxy GetProxy(string url)
        {
            var proxy = WebRequest.DefaultWebProxy;

            Uri resource = new Uri(url);

            // See what proxy is used for resource.
            Uri resourceProxy = proxy.GetProxy(resource);

            // Test to see whether a proxy was selected.
            if (resourceProxy == resource)
            {
                return null;
            }
            else
            {
                //set proxy to use the users default network credentials
                proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                return proxy;
            }
        }

        /// <summary>
        /// Fetches the by street.
        /// </summary>
        /// <param name="street">The street.</param>
        /// <param name="town">The town.</param>
        /// <param name="fuzzy">if set to <c>true</c> [fuzzy].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		[Obsolete("This method has been deprecated in V10.1")]
		public static System.Data.DataTable FetchByStreet(string street, string town, bool fuzzy)
		{
			Session.CurrentSession.CheckLoggedIn();
			Session.CurrentSession.ValidateLicensedFor("POSTCODE");

			DataSet ds = null;
			if (Session.CurrentSession.PCAWebServiceURL != "DEMO")
			{
				PostcodeAnywhere.LookupUK pca = new PostcodeAnywhere.LookupUK();

				pca.Url = Session.CurrentSession.PCAWebServiceURL;
                pca.Proxy = GetProxy(pca.Url);
				string PCAAccountCode = Session.CurrentSession.PCAAccountCode;
				string PCALicenseKey = Session.CurrentSession.PCALicenseKey;
				string PCAMachineId = Session.CurrentSession.PCAMachineId;

				if (Session.CurrentSession.PCATerminalLicense)
				{
					PCALicenseKey = Session.CurrentSession.CurrentTerminal.PCALicenseKey;
					PCAMachineId = Session.CurrentSession.CurrentTerminal.TerminalName;
				}

				DataSet ds_orig = pca.ByStreet_Dataset(street, town, fuzzy, PCAAccountCode, PCALicenseKey, PCAMachineId);
				CheckPCAError(ds_orig);

				foreach (System.Data.DataRow r in ds_orig.Tables["DATA"].Rows)
				{
					DataSet ds_temp = pca.ByStreetKey_DataSet(Convert.ToString(r["id"]), PCAAccountCode, PCALicenseKey, PCAMachineId);
					if (ds == null)
						ds = ds_temp;
					else
						ds.Merge(ds_temp);
				}
                if (ds != null)
                    CheckPCAError(ds);
                else
                    return new DataTable();
			}
			else
                throw new OMSException2("ERNOADDSCHINDEM", "Search not avaialable in demo.");

    
			return ds.Tables["DATA"];
		}

		//INFO: Temporary demo data.
        /// <summary>
        /// Creates the demo address.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
		private static DataSet CreateDemoAddress()
		{
			DataSet ds = new DataSet();
			DataTable data = new DataTable("DATA");
			data.Columns.Add("id", typeof(long));
			data.Columns.Add("building", typeof(string));
			data.Columns.Add("description", typeof(string));		
			data.Columns.Add("Line1", typeof(string));
			data.Columns.Add("Line2", typeof(string));
			data.Columns.Add("Line3", typeof(string));
			data.Columns.Add("Line4", typeof(string));
			data.Columns.Add("Line5", typeof(string));
			data.Columns.Add("PostTown", typeof(string));
			data.Columns.Add("County", typeof(string));
			data.Columns.Add("Postcode", typeof(string));
			ds.Tables.Add(data);

			data.Rows.Add(new object[11]{1, "6-8", "Gemini Press 6-8 Whittons Lane Towcester NN12 6YZ", "6-8 Whittons Lane", "", "", "", "", "Towcester", "Northamptonshire", "NN126YZ"});
			data.Rows.Add(new object[11]{2, "4", "4 Whittons Lane Towcester NN12 6YZ", "4 Whittons Lane", "", "", "", "", "Towcester", "Northamptonshire", "NN126YZ"});
			data.Rows.Add(new object[11]{3, "2", "2 Whittons Lane Towcester NN12 6YZ", "2 Whittons Lane", "", "", "", "", "Towcester", "Northamptonshire", "NN126YZ"});
			
			data.Rows.Add(new object[11]{4, "The Priory", "Pericom Technology Plc The Priory Cosgrove Milton Keynes MK19 7JJ", "The Priory", "Cosgrove", "", "", "", "Milton Keynes", "Northamptonshire", "MK197JJ"});
			data.Rows.Add(new object[11]{5, "Ivy Cottage", "Ivy Cottage Cosgrove Cosgrove Milton Keynes MK19 7JJ", "Ivy Cottage", "Cosgrove", "", "", "", "Milton Keynes", "Northamptonshire", "MK197JJ"});
			data.Rows.Add(new object[11]{6, "Stable Cottage", "Stable Cottage Cosgrove Milton Keynes MK19 7JJ", "Stable Cottage", "Cosgrove", "", "", "", "Milton Keynes", "Northamptonshire", "MK197JJ"});
			
			data.Rows.Add(new object[11]{7, "10", "Dental Surgery 10 Castillian Street Northampton NN1 1JX", "10 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{8, "12-14", "Franklins 12-14 Castillian Street Northampton NN1 1JX", "12-14 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{9, "6-8", "Franklins Solicitors 6-8 Castillian Street Northampton NN1 1JX", "6-8 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{10, "16", "16 Castillian Street Northampton NN1 1JX", "16 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{11, "18", "18 Castillian Street Northampton NN1 1JX", "18 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{12, "20", "20 Castillian Street Northampton NN1 1JX", "20 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{13, "26", "26 Castillian Street Northampton NN1 1JX", "26 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});
			data.Rows.Add(new object[11]{14, "30-32", "30-32 Castillian Street Northampton NN1 1JX", "30-32 Castillian Street", "", "", "", "", "Northampton", "Northamptonshire", "NN11JX"});

			return ds;
		}


        /// <summary>
        /// Creates the PCA address.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		private static Address CreatePCAAddress(DataTable dt)
		{
			Address add = null;
			if (dt.Rows.Count == 1)
			{
				DataRow r = dt.Rows[0];
				add = new Address();
				add.Line1 = Convert.ToString(r["Line1"]);
				add.Line2 = Convert.ToString(r["Line2"]);
				add.Line3 = Convert.ToString(r["Line3"]);
				add.Line4 = Convert.ToString(r["PostTown"]);
				add.Line5 = Convert.ToString(r["County"]);
				add.Postcode = Convert.ToString(r["Postcode"]);
                add.CountryID = GetDefaultCountryId();
			
				add.Match();
			}
			return add;
		}

        /// <summary>
        /// Checks the PCA error.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <remarks></remarks>
		private static void CheckPCAError(DataSet ds)
		{
			if (ds != null && ds.Tables.Contains("ERRORS"))
			{
				if (ds.Tables["ERRORS"].Rows.Count > 0)
				{
					string message = "";

					foreach (DataRow r in ds.Tables["ERRORS"].Rows)
					{
						message = message + Convert.ToString(r["description"]) + Environment.NewLine;
					}

					if (message != String.Empty)
						throw new Exception(message);

				}
			}
		}

		#endregion

		#region IConvertible Members

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public ulong ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this.ID);
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public sbyte ToSByte(IFormatProvider provider)
		{
			return 0;
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A double-precision floating-point number equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public double ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this.ID);
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public DateTime ToDateTime(IFormatProvider provider)
		{
			return new DateTime().ToLocalTime();
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A single-precision floating-point number equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public float ToSingle(IFormatProvider provider)
		{
			return 0;
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A Boolean value equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public bool ToBoolean(IFormatProvider provider)
		{
			return false;
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public int ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this.ID);
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public ushort ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this.ID);
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public short ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this.ID);
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.String"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="T:System.String"/> instance equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		string System.IConvertible.ToString(IFormatProvider provider)
		{
			return this.ToString();
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public byte ToByte(IFormatProvider provider)
		{
			return 0;
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A Unicode character equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public char ToChar(IFormatProvider provider)
		{
			return '\0';
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public long ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this.ID);
		}

        /// <summary>
        /// Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.</returns>
        /// <remarks></remarks>
		public System.TypeCode GetTypeCode()
		{
			return System.TypeCode.Object;
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public decimal ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this.ID);
		}

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public object ToType(Type conversionType, IFormatProvider provider)
		{
			// TODO:  Add Address.ToType implementation
			return null;
		}

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <remarks></remarks>
		public uint ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this.ID);
		}
		#endregion
	}

	
}
