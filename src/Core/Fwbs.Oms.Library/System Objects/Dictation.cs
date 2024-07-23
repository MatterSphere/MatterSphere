using System;
using System.Data;

namespace FWBS.OMS
{
    /// <summary>
    /// Summary description for Dictation.
    /// </summary>
    public class Dictation : CommonObject
	{
		public enum DictationStatus {New,InProgress,Completed,All}

		#region ICommonObject
		public static Dictation GetDictation(string id)
		{
			Dictation _dication = new Dictation();
			_dication.Fetch(id);
			return _dication;
		}


		public static string GetPoolNameFromID(int PoolID, string defaultValue)
		{
			Session.CurrentSession.CheckLoggedIn();
			object name;
			string sql = "select dicPoolName from dbDictationsPools where dicPoolID = @PoolID";
			name = Session.CurrentSession.Connection.ExecuteSQLScalar(sql, new IDataParameter[1]{Session.CurrentSession.Connection.AddParameter("PoolID", SqlDbType.Int, 0, PoolID)});
			if (name is DBNull || name == null)
				return defaultValue;
			else
				return (string)name;
		}

		public static void UpdateMembersofPool(int PoolID,DataTable dt, string Type)
		{
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID",PoolID);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("dicPoolLinkType",Type);
			Session.CurrentSession.Connection.ExecuteSQLTable("DELETE FROM dbDictationsPoolMembers WHERE dicPoolID = @ID and dicPoolLinkType = @dicPoolLinkType","ACTIVE",false,paramlist);
			foreach (DataRow dr in dt.Rows)
			{
				User u = new User(Convert.ToInt32(dr["UsrID"]));
				if (Type == "T" && u.IsInRoles("DICTRANSCRIBER") == false)
				{
					u.Roles = u.Roles + ",DICTRANSCRIBER";
					u.Roles = u.Roles.Replace(",,",",");
					u.Update();
				}
				else if (Type == "A" && u.IsInRoles("DICAUTHOR") == false)
				{
					u.Roles = u.Roles + ",DICAUTHOR";
					u.Roles = u.Roles.Replace(",,",",");
					u.Update();
				}
				IDataParameter[] paramlist2 = new IDataParameter[3];
				paramlist2[0] = Session.CurrentSession.Connection.AddParameter("ID",PoolID);
				paramlist2[1] = Session.CurrentSession.Connection.AddParameter("UserID",dr["UsrID"]);
				paramlist2[2] = Session.CurrentSession.Connection.AddParameter("dicPoolLinkType",Type);
				Session.CurrentSession.Connection.ExecuteSQLTable("INSERT INTO dbDictationsPoolMembers (dicPoolID,UserID,dicPoolLinkType) VALUES (@ID,@UserID,@dicPoolLinkType)","ACTIVE",false,paramlist2);
			}
		}

		public static DataSet GetOfflineDataSet()
		{
			IDataParameter[] param = new IDataParameter[1];
			param[0] = Session.CurrentSession.Connection.AddParameter("UserID",Session.CurrentSession.CurrentUser.ID);
			DataSet offline = Session.CurrentSession.Connection.ExecuteProcedureDataSet("schDictationOfflineCache",false,new string[2]{"PRECEDENTS","NEWPOOLUSERS"},param);
			offline.Tables["NEWPOOLUSERS"].PrimaryKey = new DataColumn[2]{offline.Tables["NEWPOOLUSERS"].Columns["dicPoolID"],offline.Tables["NEWPOOLUSERS"].Columns["Type"]};

			Dictation dic = new Dictation();
			dic.Create();
			dic.Cancel();
			offline.Tables.Add(dic.GetDataTable());
			return offline;
		}

		public void GetDictationByDocID(long DocID)
		{
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID", System.Data.SqlDbType.BigInt, 10, DocID);
			DataTable doctable = OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT dicID from dbDictations WHERE docID = @ID","DICTATIONS",false,paramlist);
			Fetch(Convert.ToString(doctable.Rows[0]["dicID"]));
		}

		public bool AddDictation(DataRow dictation)
		{
			if (dictation["docID"] != DBNull.Value)
			{
				Dictation old = new Dictation();
				old.Fetch(dictation["docID"]);
				foreach (DataColumn c in dictation.Table.Columns)
				{
					try
					{
						if (c.ColumnName.ToLower() != "dicid")
                            old.SetExtraInfo(c.ColumnName,dictation[c]);
					}
					catch
					{}
				}
				old.SetExtraInfo("docid",DBNull.Value);
				old.Update();
				return false;
			}
			else
			{
				_data.Rows.Add(dictation.ItemArray);
				return true;
			}
		}

		public void Fetch(string id)
		{
			base.Fetch (id);
		}


		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}


		public override string FieldPrimaryKey
		{
			get
			{
				return "dicID";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "DICTATIONS";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbDictations";
			}
		}

		protected override string FieldActive
		{
			get
			{
				return "dicActive";
			}
		}
		#endregion

		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		public override object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion	
	}
}
