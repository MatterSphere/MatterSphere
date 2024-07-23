using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using FWBS.OMS.Data;
using FWBS.OMS.Data.Exceptions;

namespace FWBS.OMS
{
    /// <summary>
    /// Favourites class is a class of objects used in areas were user favourites maybe persisted to disk
    /// quick client list and other features like quick precedents can be used to persist the type
    /// of object of the business layer.
    /// </summary>
    public class Favourites  : IDisposable
	{
		#region Fields

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _favourites = null;
		
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		private static string Sql = "select * from dbuserFavourites";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		private const string Table = "FAVOURITES";

		/// <summary>
		/// Current favourites type to being used.
		/// </summary>
		private string _currentfavtype;

        /// <summary>
        /// Current favourites filter to being used.
        /// </summary>
        private string _currentfavfilter;

        /// <summary>
        /// Current user id used.
        /// </summary>
        private int _userId = -1;

		/// <summary>
		/// The current filter of the favourites object. 
		/// </summary>
		private DataView _vw = null;

		/// <summary>
		/// Original filter string to rollback to if an addtional filter is added.
		/// </summary>
		private string _orignalFilter = "";


		#endregion

		#region Constructors
		
		private Favourites(){}


        internal Favourites(DataTable data, int userid)
        {
            //Only used for loggin in.
            if (data == null)
                throw new ArgumentNullException("data");

            this._favourites = data.Copy();
            _userId = userid;
            SetDefaults();
        }


		internal Favourites(int userId)
		{
			Session.CurrentSession.CheckLoggedIn();
            _favourites = GetFavouritesData(userId);
            SetDefaults();
		}


		public Favourites(string favouriteType) : this(favouriteType,"")
		{
		}


		public Favourites(string favouriteType,string favouriteFilter)
		{
            _currentfavtype  = favouriteType;
            _currentfavfilter = favouriteFilter;

            InitializeData();
		}


        private DataTable GetFavouritesFromTheDB()
        {
            List<IDataParameter> parList = new List<IDataParameter>();
            FWBS.OMS.Data.IConnection cnn = Session.CurrentSession.Connection;
            parList.Add(cnn.CreateParameter("@USRID", Session.CurrentSession.CurrentUser.ID));
            string sql = "select * from dbUserFavourites where usrID = @USRID order by FavID asc";
            return cnn.ExecuteSQL(sql, parList);
        }

        public void UpdateFavouriteType(int index, long favid)
        {
            List<IDataParameter> parList = new List<IDataParameter>();
            FWBS.OMS.Data.IConnection cnn = Session.CurrentSession.Connection;
            parList.Add(cnn.CreateParameter("@INDEX", index));
            parList.Add(cnn.CreateParameter("@FAVID", favid));
            string sql = "update dbUserFavourites set usrfavobjparam4 = @INDEX where FavID = @FAVID";
            cnn.ExecuteSQL(sql, parList);
        }

		#endregion Constructors

		#region Methods

        private DataTable GetFavouritesData(int userID)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("USRID", System.Data.SqlDbType.BigInt, 0, userID);
            try
            {
                return Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where usrID = @USRID or not usrfavdept is null order by usrFavType asc, usrFavDesc asc, len(usrFavObjParam3) asc, usrFavObjParam1 asc", Table, paramlist);
            }
            catch
            {
                return Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where usrid = @USRID order by favid asc", Table, paramlist);
            }
        }


        private void SetDefaults()
        {
            if (_favourites.PrimaryKey == null || _favourites.PrimaryKey.Length == 0)
            {
                _favourites.PrimaryKey = new DataColumn[] { _favourites.Columns["favid"] };
                long start = 1;
                if (_favourites.Rows.Count > 0)
                    start = Convert.ToInt64(_favourites.Rows[_favourites.Rows.Count - 1]["favid"]) + 1;
                _favourites.Columns["favid"].AutoIncrement = true;
                _favourites.Columns["favid"].AutoIncrementSeed = start;
            }
            _vw = _favourites.DefaultView;
        }


        private static bool FavouriteTypeIsAdminRelated(string favouriteType)
        {
            return favouriteType == "ADMINFAV"
                   || favouriteType == "ADMINLAST10";
        }

        /// <summary>
        /// Gets the ID of a favourites item.
        /// </summary>
        /// <param name="row">Row identifier.</param>
        /// <returns>An int value.</returns>
        public int FavoriteId(int row)
        {
            return Convert.ToInt32(GetExtraInfo("favid", row));
        }

        /// <summary>
        /// Gets the description of a favourites item..
        /// </summary>
        /// <param name="row">Row identifier.</param>
        /// <returns>A string value.</returns>
        public string Description(int row)
		{
			return Convert.ToString(GetExtraInfo("usrfavdesc", row));
		}

		/// <summary>
		/// Gets the glyph code of a favourites item..
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <returns>A string value.</returns>
		public string Glyph(int row)
		{
			return Convert.ToString(GetExtraInfo("usrFavGlyph", row));
		}

		/// <summary>
		/// Gets the first optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <returns>A string value.</returns>
		public string Param1(int row)
		{
			return Convert.ToString(this.GetExtraInfo("usrFavObjParam1", row));
		}

		/// <summary>
		/// Gets the second optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <returns>A string value.</returns>
		public string Param2(int row)
		{
			return Convert.ToString(this.GetExtraInfo("usrFavObjParam2", row));
		}

		/// <summary>
		/// Gets the third optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <returns>A string value.</returns>
		public string Param3(int row)
		{
			return Convert.ToString(this.GetExtraInfo("usrFavObjParam3", row));
		}

		/// <summary>
		/// Gets the fourth optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <returns>A string value.</returns>
		public Int64 Param4(int row)
		{
			return Convert.ToInt64("0" + Convert.ToString(this.GetExtraInfo("usrFavObjParam4", row)));
		}


		/// <summary>
		/// Sets the description of a favourites item.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <param name="val">The value to set.</param>
		public void Description(int row, string val)
		{
			SetExtraInfo("usrFavDesc", row, val);
		}

		/// <summary>
		/// Sets the description of a favourites item.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <param name="val">The value to set.</param>
		/// <returns>A string value.</returns>
		public void Glyph(int row, string val)
		{
			SetExtraInfo("usrFavGlyph", row, val);
		}

		/// <summary>
		/// Sets the first optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <param name="val">The value to set.</param>
		public void Param1(int row, string val)
		{
			SetExtraInfo("usrFavObjParam1", row, val);
		}

		/// <summary>
		/// Sets the second optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <param name="val">The value to set.</param>
		public void Param2(int row, string val)
		{
			SetExtraInfo("usrFavObjParam2", row, val);
		}

		/// <summary>
		/// Sets the third optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <param name="val">The value to set.</param>
		public void Param3(int row, string val)
		{
			SetExtraInfo("usrFavObjParam3", row, val);
		}

		/// <summary>
		/// Sets the fourth optional parameter.
		/// </summary>
		/// <param name="row">Row identifier.</param>
		/// <param name="val">The value to set.</param>
		public void Param4(int row, Int64 val)
		{
			SetExtraInfo("usrFavObjParam4", row, val);
		}

        /// <summary>
        /// Removes a favourtie item based on identifier.
        /// </summary>
        /// <param name="id">Favorite identifier to remove.</param>
        /// <returns>A boolean value on removal success.</returns>
        public bool RemoveFavourite(string id)
        {
            InitializeData();

            try
            {
                int row = -1;
                for (int i = 0; i < _vw.Table.Rows.Count; i++)
                {
                    if (_vw.Table.Rows[i]["FavID"].ToString() == id)
                    {
                        row = i;
                        break;
                    }
                }

                if (row == -1)
                {
                    return false;
                }
               
                var result = RemoveFavourite(_vw.Table.Rows[row]);
                this.Update();
                return result;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a favourtie item based on a row identifier.
        /// </summary>
        /// <param name="row">Row identifier to remove.</param>
        /// <returns>A boolean value on removal success.</returns>
        public bool RemoveFavourite(int row)
		{
			try
			{
				var result = RemoveFavourite(_vw[row].Row);
                //place check for registry key being turned on here to bypass the favourite update if performance when not using caching is an issue
                //this needs to be doen wherever an update is done in this class and once within the session class - RN 02/12/16
                this.Update();
                return result;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Removes a favourtie item based on a row identifier.
		/// </summary>
		/// <param name="row">DataRow to Delete</param>
		/// <returns>A boolean value on removal success.</returns>
		public bool RemoveFavourite(DataRow row)
		{
			try
			{
				row.Delete();
                this.Update();
                return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Adds a favourtie item to the underlying data sourceready for updating to the database..
		/// </summary>
		/// <param name="description">Description of the favourite item.</param>
		/// <param name="glyph">A glyph code to store against the item.</param>
		/// <param name="param">A parameter array list of information to store against the item.</param>
		/// <returns>A boolean value on whether the item has successfully been added.</returns>
		public bool AddFavourite(string description, string glyph, params string [] param)
		{
			var result =  AddGlobalFavourite(description, glyph, null, param);
            this.Update();
            InitializeData();

            return result;
		}

		public bool AddGlobalFavourite(string description, string glyph, string dept, params string [] param)
		{
			try
			{
                _favourites = GetFavouritesFromTheDB();
                SetDefaults();

				DataRow drAddFav = _favourites.NewRow();

				drAddFav["usrFavType"] = CurrentFavouriteType;
				drAddFav["usrFavDesc"] = description;
				drAddFav["usrFavGlyph"] = glyph;
				drAddFav["usrID"] = _userId;

				if (_favourites.Columns.Contains("usrfavdept"))
				{
					if (dept == null)
						drAddFav["usrfavdept"] = DBNull.Value;
					else
						drAddFav["usrfavdept"] = dept;
				}

				for (int ctr = param.GetLowerBound(0); (ctr < param.Length) && ctr < 4; ctr++)
					drAddFav["usrFavObjParam" + (ctr+1).ToString()] = Convert.ToString(param[ctr]);
			
				_favourites.Rows.Add(drAddFav);
                this.Update();
                return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				return false;
			}

		}
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="row">The row number to set.</param>
		/// <param name="val">Value to use.</param>
		private void SetExtraInfo (string fieldName, int row, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			if (Count == 0)
			{
				DataRow drAddFav = _favourites.NewRow();
				drAddFav["usrFavType"] = CurrentFavouriteType;
				drAddFav["usrID"] = _userId;			
				_favourites.Rows.Add(drAddFav);
			}
			_vw[row][fieldName] = val;
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="row">The row number to set.</param>
		/// <returns>The current data value.</returns>
		private object GetExtraInfo(string fieldName, int row)
		{
            if (_vw[row][fieldName] is System.DBNull)
                return "";
            else
            {
                object val = _vw[row][fieldName];
                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
            }
		}

		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public DataView GetDataView()
		{
			return _vw;
		}

		/// <summary>
		/// Updates the underlying data source and persists it to the database.
		/// </summary>
		public void Update()
		{
            if (_favourites.GetChanges() != null)
            {
                if (_favourites.PrimaryKey == null || _favourites.PrimaryKey.Length == 0)
				{
					_favourites.PrimaryKey = new DataColumn[]{_favourites.Columns["favid"]};
					_favourites.Columns["favid"].AutoIncrement = true;
				}
				try
				{
					Session.CurrentSession.Connection.Update(_favourites, "dbUserFavourites", true, null);
				}
                catch (ConnectionException conex)
				{
                    DBConcurrencyException ex = conex.InnerException as DBConcurrencyException;

                    if (ex == null)
                        throw;

					_favourites.AcceptChanges();
					try
					{
						Session.CurrentSession.Connection.Update(_favourites, "dbUserFavourites", true, null);
					}
					catch
					{}
				}
			}
		}

        private void InitializeData()
        {
            _userId = Session.CurrentSession.CurrentUser.ID;
            _favourites = (FavouriteTypeIsAdminRelated(_currentfavtype))
                                                ? GetFavouritesData(Session.CurrentSession.CurrentUser.ID)
                                                : Session.CurrentSession.CurrentFavourites._favourites;
            SetDefaults();
            
	        _vw = new DataView(_favourites);
			
			if (_currentfavtype == null || _currentfavtype == String.Empty)
				_orignalFilter = "";
			else
			{
				if (_currentfavfilter == null || _currentfavfilter == String.Empty)
					_orignalFilter = "usrFavType = '" + FWBS.Common.SQLRoutines.RemoveRubbish(_currentfavtype) + "'";
				else
					_orignalFilter = "usrFavType = '" + FWBS.Common.SQLRoutines.RemoveRubbish(_currentfavtype) + "' and usrFavDesc = '" + FWBS.Common.SQLRoutines.RemoveRubbish(_currentfavfilter) + "'";
            }

            _vw.RowFilter = _orignalFilter;
        }

		public void ApplyFilter(string dept, string addionalFilter)
		{
			try
			{
				string filter = "(" + _orignalFilter + ") and ([usrFavDept] = '" + Common.SQLRoutines.RemoveRubbish(dept) + "')";
				if (addionalFilter != String.Empty)
					filter += " and " + addionalFilter;
				_vw.RowFilter = filter;
			}
			catch{}
		}

		public void ApplyFilter(string addionalFilter)
		{
			try
			{
				string filter = "(" + _orignalFilter + ")";
				if (addionalFilter != String.Empty)
					filter += " and " + addionalFilter;
				_vw.RowFilter = filter;
			}
			catch{}
		}

		/// <summary>
		/// Resets to the original filter string.
		/// </summary>
		public void ResetFilter()
		{
			_vw.RowFilter = _orignalFilter;
		}


        public static void UpdateUserFavourites(FWBS.Common.KeyValueCollection updateKVC)
        {
            List<IDataParameter> parList = new List<IDataParameter>();
            FWBS.OMS.Data.IConnection cnn = Session.CurrentSession.Connection;
            parList.Add(cnn.CreateParameter("@ID", Convert.ToString(updateKVC["ID"].Value)));
            parList.Add(cnn.CreateParameter("@objectCode", Convert.ToString(updateKVC["objectCode"].Value)));
            parList.Add(cnn.CreateParameter("@description", Convert.ToString(updateKVC["description"].Value)));
            if (!string.IsNullOrWhiteSpace(Convert.ToString(updateKVC["roles"].Value)))
                parList.Add(cnn.CreateParameter("@roles", Convert.ToString(updateKVC["roles"].Value)));
            else
                parList.Add(cnn.CreateParameter("@roles", DBNull.Value));
            cnn.ExecuteProcedure("sprUpdateAdminFavourites", parList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number favourite items under the favourit type.
        /// </summary>
        public int Count
		{
			get
			{
				try
				{
					return _vw.Count;
				}
				catch
				{
					return 0;
				}
			}
		}


		/// <summary>
		/// Gets the current favourite type filter.
		/// </summary>
		public string CurrentFavouriteType
		{
			get
			{
				return _currentfavtype;
			}
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
				_favourites = null;
				_vw = null;
			}
			
			//Dispose unmanaged objects.
		}


		#endregion


	}
}
