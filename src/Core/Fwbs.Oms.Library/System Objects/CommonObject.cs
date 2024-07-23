using System;
using System.ComponentModel;
using System.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{

    /// <summary>
    /// 10000 The Base class of new Business Objects
    /// </summary>
    public abstract class CommonObject : LookupTypeDescriptor, ICommonObject
	{
		#region Fields

		/// <summary>
		/// Underlying data table that holds the objects internal data.
		/// </summary>
		protected internal DataTable _data = null;
		/// <summary>
		/// Force is Dirty 
		/// </summary>
		private bool _isdirty = false;


        private bool partial = false;


		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor that is used to create a new instance of the object.
		/// </summary>
		public CommonObject()
		{
			Create();
		}

		/// <summary>
		/// Fetches an existing object from a unique identifier.
		/// </summary>
		/// <param name="id">The unqiue identifier of the object being fetched.</param>
		internal protected CommonObject(object id)
		{
			Fetch(id);
		}


        protected CommonObject(System.IO.FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            try
            {
                using (DataSet ds = new DataSet())
                {
                    ds.ReadXml(file.FullName);
                    _data = ds.Tables[0].Copy();
                }
                _data.AcceptChanges();
            }
            catch (System.Xml.XmlException ex)
            {
                throw new CommonObjectException("ERRINVALIDCACHE", "Invalid cache file format", ex);
            }

            _data.DefaultView.RowStateFilter = DataViewRowState.OriginalRows;
            _data.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
        }

		#endregion

		#region Properties

        protected virtual string PartialSelectStatement
        {
            get
            {
                return SelectStatement;
            }
        }

		/// <summary>
		/// This is the select statement that is used to retrieve the underlying data.
		/// </summary>
		protected abstract string SelectStatement{get;}
	
		/// <summary>
		/// This is the Update select statement that is used to Update the underlying data.
		/// </summary>
    	protected virtual string UpdateSelectStatement
		{
			get
			{
				return SelectStatement;
			}
		}
		
        /// <summary>
		/// The table name of the main underlying data object.
		/// </summary>
		protected abstract string PrimaryTableName{get;}

        protected virtual bool RefreshOnUpdate
        {
            get
            {
                return true;
            }
        }

        protected virtual string DatabaseTableName 
        {
            get
            {
                string[] split = SelectStatement.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return split[split.GetUpperBound(0)];
            }
        }

		/// <summary>
		/// The default enquiry form that will be used for the object.
		/// </summary>
		protected virtual string DefaultForm
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// If the Object is only partially loaded
        /// </summary>
        [Browsable(false)]
        public bool IsPartial
        {
            get
            {
                return partial;
            }
        }

		/// <summary>
		/// The created by field name.
		/// </summary>
		protected virtual string FieldCreatedBy
		{
			get
			{	
				return "CreatedBy";
			}
		}
			
		/// <summary>
		/// The created time field name.
		/// </summary>
		protected virtual string FieldCreated
		{
			get
			{
				return "Created";
			}
		}

		/// <summary>
		/// The updated by field name.
		/// </summary>
		protected virtual string FieldUpdatedBy 
		{
			get
			{
				return "UpdatedBy";
			}
		}
				
		/// <summary>
		/// The updated time field name.
		/// </summary>
		protected virtual string FieldUpdated
		{
			get
			{
				return "Updated";
			}
		}

		/// <summary>
		/// The active field name, used for temporary deletions.
		/// </summary>
		protected virtual string FieldActive 
		{
			get
			{
				return "Active";
			}
		}

        /// <summary>
        /// The Scalar Select Statement to check the Existance
        /// </summary>
        protected virtual string SelectExistsStatement
        {
            get
            {
                return String.Empty;
            }
        }
		#endregion
		
		#region Abstract Methods


		/// <summary>
		/// Gets an exception to throw if the item in the database does not exist.
		/// </summary>
		/// <param name="id">The unique id of the object given.</param>
		/// <returns>An exception instance to throw.</returns>
		protected virtual OMSException2 GetMissingException(object id)
		{
			return new MissingCommonObjectException(id);
		}

        protected PartialStateCommonObjectException GetPartialStateException()
        {
            return new PartialStateCommonObjectException();
        }

		#endregion

		#region IExtraInfo Implementation
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public virtual void SetExtraInfo (string fieldName, object val)
		{
            if (IsPartial)
            {
                if (!_data.Columns.Contains(fieldName))
                    throw GetPartialStateException();
            }

            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime )
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			object original = GetExtraInfo(fieldName);
			string prop = GetPropertyName(fieldName);

			ValueChangingEventArgs changing = new ValueChangingEventArgs(prop, original, val);

			if(this.OnExtValueChanging(true, changing))
				return;

			OnValueChanging(changing);
			if (changing.Cancel)
				return;

			_data.Rows[0][fieldName] = val;
			OnDataChanged();
            HasUniqueIDChanged(fieldName);

			ValueChangedEventArgs changed = new ValueChangedEventArgs(prop, original, val);
            this.OnExtValueChanged(changed);
			OnValueChanged(changed);
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public virtual object GetExtraInfo(string fieldName)
		{
            if (IsPartial)
            {
                if (!_data.Columns.Contains(fieldName))
                    throw GetPartialStateException();
            }

			object val = _data.Rows[0][fieldName];
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
        public virtual Type GetExtraInfoType(string fieldName)
        {
            if (IsPartial)
            {
                if (!_data.Columns.Contains(fieldName))
                    throw GetPartialStateException();
            }
            return _data.Columns[fieldName].DataType;
        }

		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public virtual DataSet GetDataset()
		{
            if (IsPartial)
                throw GetPartialStateException();

			DataSet ds = new DataSet();
			ds.Tables.Add (GetDataTable());
			return ds;
		}

		
		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public virtual DataTable GetDataTable()
		{
            if (IsPartial)
                throw GetPartialStateException();

			return _data.Copy();
		}

				
		/// <summary>
		/// Returns the data table
		/// </summary>
		/// <returns>DataTable object.</returns>
		public virtual DataTable GetDataLiveTable()
		{
            if (IsPartial)
                throw GetPartialStateException();

			return _data;
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
		protected void OnPropertyChanged (EnquiryEngine.PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

		/// <summary>
		/// Edits the current object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public virtual Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(DefaultForm, param);
		}

		/// <summary>
		/// Edits the current object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public virtual Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry (customForm, Parent, this, param);
		}


		#endregion
		
		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		[Browsable(false)]
		public virtual object Parent
        {
            get
            {
                return null;
            }
        }

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public virtual void Update()
		{
            if (IsPartial)
                throw GetPartialStateException();

			if (_data.Rows.Count > 0)
			{

				//New addin object event arguments
				ObjectState state = State;
                if (this.OnExtCreatingUpdatingOrDeleting(state))
                    return;

				CancelEventArgs cancel = new CancelEventArgs();		
				OnUpdating(cancel);
				if (cancel.Cancel)
					return;


				DataRow row = _data.Rows[0];

				//Check if there are any changes made before setting the updated
				//and updated by properties then update.
			
				if (IsDirty)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					_data.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
					if (_data.DefaultView.Count >0 )
					{
                        SetPrimaryKeys();

					
						if (_data.Columns.Contains(FieldUpdatedBy))
							SetExtraInfo(FieldUpdatedBy, Session.CurrentSession.CurrentUser.ID);
					
						if (_data.Columns.Contains(FieldUpdated))
							SetExtraInfo(FieldUpdated, DateTime.Now);
					}


                    if (_data.Rows.Count == 1 && _data.DefaultView.Count > 0)
                    {
                        Session.CurrentSession.Connection.Update(row, DatabaseTableName, RefreshOnUpdate);
                    }
                    else
                    {
                        //For backward compatibility only.
                        if (!UpdateSelectStatement.Equals(SelectStatement))
                            Session.CurrentSession.Connection.Update(_data, UpdateSelectStatement);
                        else
                        {
                            //Should be using this statement.
                            Session.CurrentSession.Connection.Update(_data, DatabaseTableName, RefreshOnUpdate);
                        }
                    }
				}

                this.OnExtCreatedUpdatedDeleted(state);

				OnUpdated();
			}

		}

        /// <summary>
        /// Sets the Primary Keys
        /// </summary>
        protected virtual void SetPrimaryKeys()
        {
            if (_data != null)
                if (_data.PrimaryKey == null || _data.PrimaryKey.Length == 0)
                    _data.PrimaryKey = new DataColumn[1] { _data.Columns[FieldPrimaryKey] };
        }

        /// <summary>
        /// Has the Uniquie Key Changed
        /// </summary>
        /// <param name="fieldName">Field Name to Check</param>
        protected virtual void HasUniqueIDChanged(string fieldName)
        {
            if (fieldName == FieldPrimaryKey)
                OnUniqueIDChanged();
        }



		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public virtual void Refresh()
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

            if (this.OnExtRefreshing())
				return;

            DataTable changes = _data.GetChanges();


            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.UniqueID, changes.Rows[0]);
            else
                Fetch(this.UniqueID, null);

            this.OnExtRefreshed();
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public virtual void Cancel()
		{
			_data.RejectChanges();
		}

		#endregion

		#region IDisposable Implementation


		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public virtual void Dispose()
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
				if (_data != null)
				{
					_data.Dispose();
					_data = null;
				}
			}
			
			//Dispose unmanaged objects.
		}


		#endregion

		#region ICommonObject Implementation

		/// <summary>
		/// An event that gets raised when the unique id value changes.
		/// </summary>
		public event EventHandler UniqueIDChanged = null;

		/// <summary>
		/// The event that fires when any other data within the object changes.
		/// </summary>
		public event EventHandler DataChanged = null;

		/// <summary>
		/// An event that gets raised before an update occurs.
		/// </summary>
		public event System.ComponentModel.CancelEventHandler Updating;

		/// <summary>
		/// An event that get fired after an update has occurred.
		/// </summary>
		public event EventHandler Updated;

		/// <summary>
		/// Gets a value indicating whether the  object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		[Browsable(false)]
		public bool IsNew
		{
			get
			{
				try
				{
					return (_data.Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

        [Browsable(false)]
		public virtual bool IsDeleted
		{
			get
			{
                if (_data.Columns.Contains(FieldActive))
                    return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo(FieldActive), false);
                else
                    return State == ObjectState.Deleted || State == ObjectState.Unitialised;
			}
		}

		/// <summary>
		/// Gets the unique id value of the object.
		/// </summary>
		[Browsable(false)]
		public virtual object UniqueID
		{
			get
			{
				return GetExtraInfo(FieldPrimaryKey);
			}
			set
			{
				SetExtraInfo(FieldPrimaryKey,value);
			}
		}

		/// <summary>
		/// Gets a flag indicting whether there are any changes made to the current object.
		/// </summary>
		[Browsable(false)]
		public virtual bool IsDirty
		{
			get
			{
				try
				{
					return (_isdirty || _data.GetChanges() != null);
				}
				catch
				{
					return _isdirty;
				}
			}
			set
			{
				_isdirty = value;
			}
		}


		/// <summary>
		/// Gets the primary key field name.
		/// </summary>
		[Browsable(false)]
		public abstract string FieldPrimaryKey{get;}


        protected virtual DataTable FetchSchema()
        {
            return Session.CurrentSession.Connection.ExecuteSQLTable(SelectStatement, PrimaryTableName, true, new IDataParameter[0]);
        }

		/// <summary>
		/// Creates a new instance / blank record to work with.
		/// </summary>
		public virtual void Create()
		{
            partial = false;

            if (_data != null)
            {
                _data.Rows.Clear();
                _data.AcceptChanges();
            }
            else
            {

                _data = FetchSchema();
            }

            SetPrimaryKeys();


			//Add a new record.
			Global.CreateBlankRecord(ref _data, true);

			//Set the created by and created date of the item.
			if(_data.Columns.Contains(FieldCreatedBy))
				this.SetExtraInfo(FieldCreatedBy, Session.CurrentSession.CurrentUser.ID);

			if(_data.Columns.Contains(FieldCreated))
				this.SetExtraInfo(FieldCreated, DateTime.Now);
		}

		/// <summary>
		/// Fetches an instance of the object.
		/// </summary>
		/// <param name="id">The unique identifier value of the object to be fetched.</param>
		protected virtual void Fetch(object id)
		{
            Fetch(id, null);
		}

        protected virtual void Fetch(object id, DataRow merge)
        {
            partial = false;

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID", id);
            DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(String.Format("{0} where {1} = @ID", SelectStatement, FieldPrimaryKey), PrimaryTableName, paramlist);

            data.DefaultView.RowStateFilter = DataViewRowState.OriginalRows;
            data.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
            if ((data == null) || (data.Rows.Count == 0))
            {
                throw GetMissingException(id);
            }

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _data = data;
            SetPrimaryKeys();

            this.OnExtLoaded();
        }

        protected virtual void PartialFetch(object id)
        {
            PartialFetch(id, null);
        }

        protected virtual void PartialFetch(object id, DataRow merge)
        {
            partial = true;

            if (String.IsNullOrEmpty(PartialSelectStatement) || PartialSelectStatement.ToUpperInvariant() == SelectStatement.ToUpperInvariant())
                Fetch(id, merge);
            else
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID", id);
                DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(String.Format("{0} where {1} = @ID", PartialSelectStatement, FieldPrimaryKey), PrimaryTableName, paramlist);

                

                if ((data == null) || (data.Rows.Count == 0))
                {
                    throw GetMissingException(id);
                }

                if (merge != null)
                    Global.Merge(data.Rows[0], merge);

                _data = data;
                SetPrimaryKeys();
            }
        }

	
		/// <summary>
		/// Gets a list of all active items of the same type.
		/// </summary>
		/// <returns>A data table of items.</returns>
		public DataTable GetList()
		{
			return GetList(true);
		}

		/// <summary>
		/// Gets a list of all items of the same type based on an active state.
		/// </summary>
		/// <param name="active">True to list all active items.</param>
		/// <returns>A data table of items.</returns>
		public DataTable GetList(bool active)
		{
			if (_data.Columns.Contains(FieldActive))
			{
				IDataParameter[] p = new IDataParameter[1];
				p[0] = Session.CurrentSession.Connection.AddParameter("ACTIVE", SqlDbType.Bit, 0, active);
				return Session.CurrentSession.Connection.ExecuteSQLTable(SelectStatement + " where " + FieldActive + " = @ACTIVE", PrimaryTableName, p);
			}
			else
				return Session.CurrentSession.Connection.ExecuteSQLTable(SelectStatement, PrimaryTableName, new IDataParameter[0]);
		}

        /// <summary>
        /// Checks to see if an item of the specified id exists within the database.
        /// </summary>
        /// <param name="id">The unique id to search for.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        public virtual bool Exists(object id)
        {
            try
            {
                if (String.IsNullOrEmpty(this.SelectExistsStatement))
                {
                    Fetch(id);
                    return true;
                }
                else
                {
                    Session.CurrentSession.CheckLoggedIn();
                    IDataParameter[] paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID", id);
                    object dt = Session.CurrentSession.Connection.ExecuteSQLScalar(String.Format("{0} where {1} = @ID", this.SelectExistsStatement, this.FieldPrimaryKey), paramlist);
                    if (dt != null)
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

		/// <summary>
		/// Deletes the current instance of the item.
		/// Makes the item inactive if an active flag field exists .
		/// </summary>
		public virtual void Delete()
		{
            if (IsPartial)
                throw GetPartialStateException();

			if (_data.Columns.Contains(FieldActive))
			{
				SetExtraInfo(FieldActive, false);
			}
			else
			{
				_data.Rows[0].Delete();
			}
			Update();
		}

		/// <summary>
		/// Restores an existing inactive item.
		/// </summary>
		public void Restore()
		{
            if (IsPartial)
                throw GetPartialStateException();

			if (!IsNew)
			{
				if (_data.Columns.Contains(FieldActive))
				{
					SetExtraInfo(FieldActive, true);
				}
				Update();
			}
		}

		/// <summary>
		/// Raises the UniqueIDChanged event.
		/// </summary>
		protected void OnUniqueIDChanged()
		{
			if (UniqueIDChanged != null)
				UniqueIDChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the DataChanged event.
		/// </summary>
		protected void OnDataChanged()
		{
			if (DataChanged != null)
				DataChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the Updating event.
		/// </summary>
		protected void OnUpdating(CancelEventArgs e)
		{
			if (Updating != null)
				Updating(this, e);
		}


		/// <summary>
		/// Raises the Updating event.
		/// </summary>
		protected void OnUpdated()
		{
			if (Updated != null)
				Updated(this, EventArgs.Empty);
		}

		#endregion

		#region CommonObjectEx for Extensibility

		public event ValueChangedEventHandler ValueChanged = null;
		public event ValueChangingEventHandler ValueChanging = null;
		
		protected virtual void OnValueChanged(ValueChangedEventArgs e)
		{
			ValueChangedEventHandler ev = ValueChanged;
			if (ev != null)
				ev(this, e);
		}

		protected virtual void OnValueChanging(ValueChangingEventArgs e)
		{
            //UTCFIX: DM - 01/12/06 - Make sure date values are compared with same kind.
            object originalvalue = e.OriginalValue;
            object proposedvalue = e.ProposedValue;

            if (originalvalue is DateTime)
                originalvalue = ((DateTime)originalvalue).ToLocalTime();
            if (proposedvalue is DateTime)
                proposedvalue = ((DateTime)proposedvalue).ToLocalTime();

			if (originalvalue == e.ProposedValue)
			{
				e.Cancel = true;
				return;
			}

            if (originalvalue == e.ProposedValue)
            {
                e.Cancel = true;
                return;
            }

            if (originalvalue != null)
            {
                if (originalvalue.Equals(proposedvalue))
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (proposedvalue != null)
            {
                if (proposedvalue.Equals(originalvalue))
                {
                    e.Cancel = true;
                    return;
                }
            }

			ValueChangingEventHandler ev = ValueChanging;
			if (ev != null)
				ev(this, e);
		}

		protected string GetPropertyName(string field)
		{
			Type t = this.GetType();
			System.Reflection.MemberInfo [] props = t.FindMembers(System.Reflection.MemberTypes.Property, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic, new System.Reflection.MemberFilter(this.FieldPropertyFilter), field.ToUpper());
			if (props.Length > 0)
				return props[0].Name;
			else
				return field;
		}

		private bool FieldPropertyFilter(System.Reflection.MemberInfo mi, object data)
		{
			object[] attrs = mi.GetCustomAttributes(typeof(FieldPropertyMapperAttribute), true);
			foreach (FieldPropertyMapperAttribute attr in attrs)
			{
				if (attr.FieldName.ToUpper() == Convert.ToString(data))
					return true;
			}
			return false;
		}

		[Browsable(false)]
		public ObjectState State
		{
			get
			{
				try
				{
					switch (_data.Rows[0].RowState)
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

		#endregion

        #region Clone

        public virtual CommonObject Clone()
        {
            if (IsPartial)
                throw GetPartialStateException();

            CommonObject clone = (CommonObject)Session.CurrentSession.TypeManager.Create(this.GetType());

            foreach (DataColumn col in this._data.Columns)
            {
                if (clone._data.Columns.Contains(col.ColumnName) 
                    && clone._data.Columns[col.ColumnName].ReadOnly == false)
                    clone._data.Rows[0][col.ColumnName] = this._data.Rows[0][col];
            }

            
            return clone;
        }

        #endregion

        #region Serialisation & Persistence

        public void Save(System.IO.FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException();

            if (IsPartial)
                throw GetPartialStateException();

            file.Refresh();
            if (file.Exists)
                file.Delete();

            if (!file.Directory.Exists)
                file.Directory.Create();

            _data.WriteXml(file.FullName, XmlWriteMode.WriteSchema, false);
        }

      
        #endregion

    }

	public enum ObjectState
	{
		Added,
		Deleted,
        Unchanged,
		Unitialised,
		Modified
	}


	public delegate void ValueChangingEventHandler (object sender, ValueChangingEventArgs e);
	public delegate void ValueChangedEventHandler (object sender, ValueChangedEventArgs e);

	public sealed class ValueChangedEventArgs : EventArgs
	{
		private string _name = String.Empty;
		private object _proposed = null;
		private object _original = null;

		private ValueChangedEventArgs(){}

		public ValueChangedEventArgs (string name, object originalValue, object proposedValue)
		{
			_name = name;
			_proposed = proposedValue;
			_original = originalValue;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public object ProposedValue
		{
			get
			{
				return _proposed;
			}
		}

		public object OriginalValue
		{
			get
			{
				return _original;
			}
		}

	}

	public sealed class ValueChangingEventArgs : CancelEventArgs
	{
		private string _name = String.Empty;
		private object _proposed = null;
		private object _original = null;

		private ValueChangingEventArgs(){}

		public ValueChangingEventArgs (string name, object originalValue, object proposedValue)
		{
			_name = name;
			_proposed = proposedValue;
			_original = originalValue;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public object ProposedValue
		{
			get
			{
				return _proposed;
			}
		}

		public object OriginalValue
		{
			get
			{
				return _original;
			}
		}

	}

	[AttributeUsage(AttributeTargets.Property, Inherited=true, AllowMultiple=false )]
	public class FieldPropertyMapperAttribute : Attribute
	{
		private string _field = String.Empty;

		public FieldPropertyMapperAttribute(string field)
		{
			_field = field;
		}

		public string FieldName
		{
			get
			{
				return _field;
			}
		}
	}
}
