using System;

namespace FWBS.OMS
{
	/// <summary>
	/// Summary description for InteractiveFile.
	/// </summary>
	public class InteractiveFile : CommonObject
	{
		#region Fields

		/// <summary>
		/// A file variable used if the file has not yet been saved to the databse.
		/// </summary>
		private OMSFile _file = null;

		#endregion

		#region Constructors

		public InteractiveFile() : base()
		{
		}

		#endregion

		#region CommonObject Implementation

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
				return "ID";
			}
		}

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "INTERACTIVEFILE";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbInteractiveFileProfile";
			}
		}

		public override void Update()
		{
			if (_file == null)
				base.Update();
			else
			{
				if (_file.IsNew == false)
				{
					if (FileID == 0)FileID = _file.ID;
					base.Update ();
				}
			}
		}


		public void Create (OMSFile file, Contact contact)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			if (contact == null)
				throw new ArgumentNullException("contact");

			_file = file;
			if (_file.IsNew == false)
			{
				System.Data.IDataParameter[] pars = new System.Data.IDataParameter[2];
				pars[0] = Session.CurrentSession.Connection.CreateParameter("fileid", _file.ID);
				pars[1] = Session.CurrentSession.Connection.CreateParameter("contid", contact.ID);
				_data = Session.CurrentSession.Connection.ExecuteSQLTable(SelectStatement + " where fileid=@fileid and contid = @contid", PrimaryTableName, false, pars);

			}
			else
			{
				_file.AddInteractiveProfile(this);
			}

			if (_data.Rows.Count == 0)
			{
				_data = null;
				Create();
			}

			_data.PrimaryKey = new System.Data.DataColumn [] {_data.Columns[this.FieldPrimaryKey]};

			ContID = contact.ID;
			FileID = _file.ID;
			ClientID = _file.Client.ClientID;

		}

		#endregion


		#region Properties
	
		public Int64 ID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("ID"));
			}
		}

		public Int64 ContID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("ContID"));
			}
			set
			{
				SetExtraInfo("ContID",value);
			}
		}

		public Int64 ClientID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("clid"));
			}
			set
			{
				SetExtraInfo("clid",value);
			}
		}

		public Int64 FileID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("fileid"));
			}
			set
			{
				SetExtraInfo("fileid",value);
			}
		}

		public bool ContactTab
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("proContact"));
			}
			set
			{
				SetExtraInfo("proContact",value);
			}
		}

		public bool MilestonesTab
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("proMilestones"));
			}
			set
			{
				SetExtraInfo("proMilestones",value);
			}
		}

		public bool NotesTab
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("proNotes"));
			}
			set
			{
				SetExtraInfo("proNotes",value);
			}
		}

		public bool EnquiryTab
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("proActionable"));
			}
			set
			{
				SetExtraInfo("proActionable",value);
			}
		}

		public bool DocumentsTab
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("proDocs"));
			}
			set
			{
				SetExtraInfo("proDocs",value);
			}
		}

		public bool SMS
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("proSMS"));
			}
			set
			{
				SetExtraInfo("proSMS",value);
			}
		}

		public Int64 Security
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("proSecValue"));
			}
			set
			{
				SetExtraInfo("proSecValue",value);
			}
		}
		#endregion

		#region Static

		public static string GeneratePassword(int Size)
		{
			int r;
			string ret = "";

            //UTCFIX: DM - 30/11/06 - Just incase use UTC date.
			Random rnd = new Random(System.DateTime.UtcNow.Millisecond);
			for (int i = 1; i < Size + 1; i++)
			{
				r = ((int)((52D-1D) * rnd.NextDouble() + 1D)) + 65;
				ret += (char)r;
			}
			return ret;
		}
		#endregion
	}
}
