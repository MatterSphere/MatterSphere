using System.Data;

namespace FWBS.OMS
{
    /// <summary>
    /// An object which manages state to and from database.  Write to and from the database directly.
    /// </summary>
    public class State
	{
		#region Fields

		private string _code = "";
		private Branch _branch = Session.OMS;
	

		#endregion

		#region Constructors

		private State(){}

		public State(string code)
		{
			_code = code;
		}

		#endregion

		#region Write Methods
		
		public void Write(object data)
		{
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("CODE", System.Data.SqlDbType.NVarChar, 100, _code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("DATA", System.Data.SqlDbType.Variant, 0, data);
			if (Session.CurrentSession.Connection.ExecuteSQL("update dbstate set statedata = @DATA where statecode = @CODE and brid is null and usrid is null", paramlist) <= 0)
				Session.CurrentSession.Connection.ExecuteSQL("insert into dbstate (statecode, statedata) values (@CODE, @DATA)", paramlist);
		}

		#endregion

		#region Read Methods
		
		public object Read()
		{
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("CODE", System.Data.SqlDbType.NVarChar, 100, _code);
			return Session.CurrentSession.Connection.ExecuteSQLScalar("select stateData from  dbstate where statecode = @CODE and brid is null and usrid is null", paramlist);
		}

		#endregion
	}


}
