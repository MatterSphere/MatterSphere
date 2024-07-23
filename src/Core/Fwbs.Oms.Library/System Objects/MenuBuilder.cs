using System;
using System.Data;

namespace FWBS.OMS
{
	/// <summary>
	/// Summary description for CommandBarBuilder.
	/// </summary>
	public class CommandBarBuilder : CommonObject
	{
		public DataTable _commandBarControls = null;
		
		public static CommandBarBuilder GetCommandBarBuilder(string cbCode)
		{
			CommandBarBuilder _CommandBarBuilder = new CommandBarBuilder();
			_CommandBarBuilder.Fetch(cbCode);
			return _CommandBarBuilder;
		}

		public static DataTable GetCommandBars()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			string lstsql = "SELECT *, dbo.GetCodeLookupDesc('COMMANDBAR', cbCode, @UI) as [cbDesc] FROM dbo.dbCommandBar";
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(lstsql, "COMMANDBARS", paramlist);
			return dt;
		}
		
		public CommandBarBuilder()
		{

		}

		protected override void Fetch(object id)
		{
			base.Fetch (id);
			
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("Code", id);
			pars[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			_commandBarControls = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT *, dbo.GetCodeLookupDesc('CBCCAPTIONS', ctrlCode, @UI) as ctrlDesc FROM dbCommandBarControl WHERE ctrlCommandBar = @Code","COMMANDBARCONTROL",false,pars);
		}

		public DataTable CommandBarControls
		{
			get
			{
				return _commandBarControls;
			}
		}

		public override void Update()
		{
			base.Update ();
			Session.CurrentSession.Connection.Update(_commandBarControls,"SELECT * FROM dbCommandBarControl");
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
				return "cbCode";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "COMMANDBAR";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbo.dbCommandBar";
			}
		}

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

		#region Properties
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo(FieldPrimaryKey));
			}
			set
			{
				SetExtraInfo(FieldPrimaryKey,value);
			}
		}
		#endregion

	}
}
