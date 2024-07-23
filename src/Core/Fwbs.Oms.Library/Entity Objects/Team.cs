using System;
using System.Data;

namespace FWBS.OMS.FileManagement
{
    using EnquiryEngine;

    public class Team :  CommonObject
    {
        #region Fields

        private static DataTable teams;
        private DataTable members;
        private DataTable users;
        private string name;

        #endregion

        #region Constructors & Destructors

        internal Team () : base()
		{
		}

		[EnquiryUsage(true)]
		internal Team (int tmid) : base (tmid)
		{
		}

		#endregion
	
		#region CommonObject Implementation


		protected override string DefaultForm
		{
			get
			{
				return "SCRFILTEAMEDIT";
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return "tmid";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "TEAM";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbteam";
			}
        }

        public override object Parent
        {
            get { return null; }
        }

        #endregion

        #region Properties


        public int ID
		{
			get
			{
				return Convert.ToInt32(UniqueID);
			}
		}

        [EnquiryUsage(true)]
        public string Code
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tmcode"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("tmcode", DBNull.Value);
                else
                    SetExtraInfo("tmcode", value);
            }
        }

        [EnquiryUsage(true)]
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(name))
                    name = CodeLookup.GetLookup("TEAM", Code);

                return name;
            }
            set
            {
                if (value == null) value = String.Empty;
                if (name != value)
                {
                    CodeLookup.Create("TEAM", Code, value, String.Empty, CodeLookup.DefaultCulture, true, true, true);
                    name = value;
                }
            }
        }

		#endregion

        #region Methods


        public DataView GetMembers()
        {
            if (members == null)
                members = GetTeamMembers(ID);

            DataView vw = new DataView(members);
            vw.RowStateFilter = DataViewRowState.CurrentRows;
            return vw;
        }

        public DataView GetNonMembers()
        {
            if (users == null)
            {
                users = Session.CurrentSession.Connection.ExecuteSQLTable("select usrid, usrfullname from dbuser WHERE NOT usrID Between -101 AND -1", "USERS", false, new IDataParameter[0]);
            }

            DataTable nonmembers = users.Copy();
            for (int ctr = nonmembers.Rows.Count - 1; ctr >= 0; ctr--)
            {
                if (ContainsMember(Convert.ToInt32(nonmembers.Rows[ctr]["usrid"])))
                    nonmembers.Rows[ctr].Delete();
            }

            nonmembers.AcceptChanges();

            DataView vw = new DataView(nonmembers);
            vw.RowStateFilter = DataViewRowState.CurrentRows;
            return vw;
        }

        public bool ContainsMember(int userId)
        {
            try
            {
                DataView vw = GetMembers();
                vw.RowFilter = String.Format("usrid = {0}", userId);
                return (vw.Count > 0);
            }
            catch
            {
                return false;
            }
        }

        [EnquiryUsage(true)]
        public void AddMember(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
           
            DataView vw = GetMembers();
            vw.RowFilter = String.Format("usrid = {0}", user.ID);
            if (vw.Count == 0)
            {
                DataRow r = members.NewRow();
                r["tmid"] = ID;
                r["usrid"] = user.ID;
                r["usrfullname"] = user.FullName;
                members.Rows.Add(r);

                IsDirty = true;
            }
        }

        [EnquiryUsage(true)]
        public void AddMember(int userId)
        {
            AddMember(User.GetUser(userId));
        }

        [EnquiryUsage(true)]
        public void RemoveMember(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            RemoveMember(user.ID);
        }

        [EnquiryUsage(true)]
        public void RemoveMember(int userId)
        {
            DataView vw = GetMembers();
            vw.RowFilter = String.Format("usrid = {0}", userId);
            if (vw.Count > 0)
            {
                for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
                {
                    vw.Delete(ctr);
                }
                IsDirty = true;
            }
        }

        public override void Update()
        {
            base.Update();

            if (members != null)
            {
                if (members.GetChanges() != null)
                {
                    foreach (DataRow r in members.Rows)
                    {
                        if (r.RowState != DataRowState.Deleted)
                        {
                            r["tmid"] = ID;
                        }
                    }
                    Session.CurrentSession.Connection.Update(members, "select tmid, usrid from dbteammembership");
                    members.AcceptChanges();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (members != null)
                    {
                        members.Dispose();
                        members = null;
                    }

                    if (users != null)
                    {
                        users.Dispose();
                        users = null;
                    }

                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region Static Methods


        public static DataTable GetTeams()
		{
            Session.CurrentSession.CheckLoggedIn();

            string sql = "select tmid, tmcode, dbo.GetCodeLookupDesc('TEAM', tmcode, @UI) as [tmname] from dbteam";
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.CreateParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "TEAM", pars);
            return dt;
		}


        public static DataTable GetTeamMembers(int teamId)
        {
            Session.CurrentSession.CheckLoggedIn();

            string sql = "select TM.tmid, U.usrid, U.usrfullname from dbteammembership TM inner join dbuser U on U.usrid = TM.usrid where tmid = @tmid";
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.CreateParameter("tmid", teamId);
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "MEMBERS", pars);
            dt.PrimaryKey = new DataColumn[2] { dt.Columns["tmid"], dt.Columns["usrid"] };
            return dt;
        }

        public static DataTable GetTeamMembers(string code)
        {
            Session.CurrentSession.CheckLoggedIn();

            string sql = "select TM.tmid, U.usrid, U.usrfullname from dbteammembership TM inner join dbuser U on U.usrid = TM.usrid where tmcode = @tmcode";
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.CreateParameter("tmcode", code);
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "MEMBERS", pars);
            dt.PrimaryKey = new DataColumn[2] { dt.Columns["tmid"], dt.Columns["usrid"] };
            return dt;
        }

		public static Team GetTeam(int id)
		{
            Session.CurrentSession.CheckLoggedIn();

			return new Team(id);
        }

        public static Team GetTeam(string code)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");

            if (teams == null)
                teams = GetTeams();

            DataView vw = new DataView(teams);
            vw.RowFilter = String.Format("tmcode = '{0}'", code);
            if (vw.Count > 0)
                return GetTeam(Convert.ToInt32(vw[0]["tmid"]));
            else
            {
                teams.Dispose();
                teams = null;
                return GetTeam(-1);
            }

        }

        #endregion
    }
}
