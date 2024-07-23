using System;
using System.Data;
using System.Diagnostics;

namespace FWBS.OMS
{
    /// <summary>
    /// 13000 Implments a collection of associates.
    /// </summary>
    public class TimeCollection : System.Collections.IEnumerator, System.Collections.IEnumerable
	{
		#region Fields
		/// <summary>
		/// Contains a reference to the _currentdocument
		/// </summary>
		private OMSDocument _currentdocument = null;
		/// <summary>
		/// Contains a reference to the currentfile;
		/// </summary>
		private OMSFile _currentfile = null;

		/// <summary>
		/// An internal data table that holds a collection of time records.
		/// </summary>
		private DataTable _timerecords = null;

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "TIMERECORDS";

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbTimeLedger";

		/// <summary>
		/// Skip Saving Time
		/// </summary>
		private bool _skiptime = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor not used.
		/// </summary>
		internal TimeCollection(OMSDocument Document) : this(Document,null) {}
		public TimeCollection(OMSFile File) : this(null,File) {}
		private TimeCollection(OMSDocument Document, OMSFile File)
		{
			if (Document != null)
			{
				_currentdocument = Document;
				_currentfile = Document.OMSFile;
			}
			else if (File != null)
			{
				_currentdocument = null;
				_currentfile = File;
			}
			_timerecords = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
            _timerecords.Columns.Add("timeActivityCodeDesc");
            //Add a new record.
			//Set the created by and created date of the item.
			_timerecords.Columns["ID"].AutoIncrement=true;		
			foreach (DataColumn col in _timerecords.Columns)
				if (!col.AllowDBNull) col.AllowDBNull = true;
		}


		/// <summary>
		/// Constructor to allow an already formed data table to be passed to it
		/// and become the data source.
		/// </summary>
		/// <param name="dt">A data table containing the data source of Time Records.</param>
        /// <param name="Document"></param>
		internal TimeCollection(ref DataTable dt, OMSDocument Document) : this(Document,null) {}
		internal TimeCollection(ref DataTable dt, OMSFile File) : this(null,File) {}
		internal TimeCollection(ref DataTable dt) : this(null,null) {}
		private TimeCollection(ref DataTable dt, OMSDocument Document, OMSFile File) 
		{
			if (Document != null)
			{
				_currentdocument = Document;
				_currentfile = Document.OMSFile;
			}
			else if (File != null)
			{
				_currentdocument = null;
				_currentfile = File;
			}
			_timerecords = dt;
			_timerecords.Columns["ID"].AutoIncrement=true;
			_timerecords.TableName = Table;
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Gets an associate using are prefered order ordinal value.
		/// </summary>
		public TimeRecord this[int order]
		{
			get
			{
				if (Count < (order + 1))
				{
					order = Count - 1;
				}

				if (order < 0)
				{
					order = 0;
				}
				
				//Get an associate object based on the row number.
				DataRow current = _timerecords.DefaultView[order].Row;
				TimeRecord _cp = new TimeRecord(this,order);
				_cp.Import(current);
				if (_currentfile != null) _cp.OMSFile = _currentfile;
				return _cp;
			}
			set
			{
				if (Count < (order + 1))
				{
					order = Count - 1;
				}

				if (order < 0)
				{
					order = 0;
				}

				DataRow current = value.GetDataRow();
				foreach (DataColumn col in _timerecords.Columns)
				{
					if (col.AutoIncrement == false)
						_timerecords.DefaultView[order].Row[col.ColumnName] = current[col.ColumnName];
				}
			}
		}
        #endregion


        public void SetBusinessObject(object obj)
        {
            _currentfile = obj as FWBS.OMS.OMSFile;
            if(obj is OMSDocument)
            {
                OMSDocument doc = (OMSDocument)obj;
                _currentfile = doc.OMSFile;
                _currentdocument = doc;
            }
        }


        #region Methods,
        /// <summary>
        /// Clears the Time Collection Array
        /// </summary>
        public void Clear()
		{
			_timerecords.Clear();
			_timerecords.Rows.Clear();
		}

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public void Update()
		{
			if (!_skiptime)
			{
				foreach(TimeRecord tm in this)
				{
					tm.Update();
				}
				//Check if there are any changes made before setting the updated
				//and updated by properties then update.
				if (_timerecords.GetChanges()!= null)
				{
					FWBS.Common.OMSDebug.DebugDataTableList(_timerecords);
					Session.CurrentSession.Connection.Update(_timerecords,Sql);
				}
			}
		}

		/// <summary>
		/// Removes A row at Index
		/// </summary>
		/// <param name="index">The Index of the Row</param>
		public void RemoveAt(int index)
		{
			if (index < 0)
				throw new OMSException2("13001","Error cannot delete a Time Record Index below 0 (Zero)");
			if (index > _timerecords.Rows.Count)
				throw new OMSException2("13002","Error cannot delete a Time Record Index of %1% because it's greater than the total rows of %2%",new Exception(),true,index.ToString(),_timerecords.Rows.Count.ToString());
			_timerecords.Rows[index].Delete();
		}

		/// <summary>
		/// Adds a Time Recording entry to the end of the collection.
		/// </summary>
		/// <param name="timerec">Time Record to add.</param>
		/// <returns>True, if the item was successfully added.</returns>
		public bool Add(TimeRecord timerec)
		{
			try
			{
				if (timerec.GetDataRow().Table.TableName == "TIMERECORD")
				{
					// Add Row to Addresses Table
					DataRow dr = _timerecords.NewRow();
					timerec.Clone(ref dr);
					_timerecords.Rows.Add(dr);
					timerec.SetIndex(_timerecords.Rows.Count-1);
					timerec.SetParent(this);
				}
			}
			catch(Exception ex)
			{
				Debug.Write(ex.Message + Environment.NewLine,"ADDTIME");
				throw ex;
			}
			return true;
		}

		/// <summary>
		/// Gets all the Time Records within the collection.
		/// </summary>
		/// <returns>A data table of Time Records.</returns>
		public DataTable GetTimeRecords()
		{
			return _timerecords;
		}

		#endregion

		#region Properties

		private int position = -1;

		/// <summary>
		/// Gets the number of Time Records currently in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _timerecords.DefaultView.Count;
			}
		}

		public OMSDocument Document
		{
			get
			{
				return _currentdocument;
			}
		}

		public OMSFile File
		{
			get
			{
				return _currentfile;
			}
		}

		public bool SkipTime
		{
			get
			{
				return _skiptime;
			}
			set
			{
				_skiptime = value;
			}
		}
		#endregion

		#region IEnumerator Implementation

		public object Current
		{
			get
			{
				return this[position];
			}
		}
		
		public void Reset()
		{
			position = -1;
		}

		public bool MoveNext()
		{
			position++;
			if (position < Count) 
				return true; 
			else 
				return false; 
		}

		#endregion

		#region IEnumerable Implementation

		public System.Collections.IEnumerator GetEnumerator() 
		{ 
			return (System.Collections.IEnumerator)this; 
		} 

		#endregion
	}


}

