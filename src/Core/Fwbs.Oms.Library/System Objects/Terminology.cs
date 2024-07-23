using System;
using System.Data;
using System.Threading;

namespace FWBS.OMS
{
	/// <summary>
	/// Terminology Parser struct.
	/// </summary>
	public class Terminology
	{
		public readonly string Client;
		public readonly string File;
		public readonly string Associate;
        public readonly string Contact;
        public readonly string Precedent;
		public readonly string FeeEarner;
		public readonly string Responsible;
		public readonly string Clients;
		public readonly string Files;
		public readonly string Associates;
        public readonly string Contacts;
        public readonly string Precedents;
		public readonly string FeeEarners;
		public readonly string Responsibles;
		public readonly string AppName;
        public readonly string Department;
        public readonly string Departments;
		public readonly System.Collections.Specialized.NameValueCollection Custom = null;
		
		/// <summary>
		/// Constructor that sets the terminology data.
		/// </summary>
		internal Terminology()
		{
			Client = "Client";
			File = "File";
			Associate = "Associate";
            Contact = "Contact";
            Precedent = "Precedent";
			FeeEarner = "Fee Earner";
			Responsible = "Responsible";
            Department = "Department";
			Clients = "Clients";
			Files = "Files";
			Associates = "Associates";
            Contacts = "Contacts";
            Precedents = "Precedents";
			FeeEarners = "Fee Earners";
			Responsibles = "Responsibles";
            Departments = "Departments";
			AppName = Global.ApplicationName;
			
			Custom = new System.Collections.Specialized.NameValueCollection();
	
			string edition = Session.CurrentSession.Edition;
			if (edition=="") edition = "EP";


			if (Session.CurrentSession.IsLoggedIn)
			{
				IDataParameter[] paramlist = new IDataParameter[2];
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Type", System.Data.SqlDbType.NVarChar, 15, edition);
				paramlist[1] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
				DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCodeLookupList", "TERMINOLOGY", paramlist);
	
				foreach (DataRow row in dt.Rows)
				{
					if (row["cddesc"].ToString().Length > 0)
					{
						string desc = row["cddesc"].ToString();
						switch(row["cdcode"].ToString())
						{
							case "%CLIENT%"		:	Client = desc; break;
							case "%CLIENTS%"	: 	Clients = desc;	break;
							case "%FILE%"		:	File = desc; break;
							case "%FILES%"		:	Files = desc; break;
							case "%ASSOCIATE%"	:	Associate = desc; break;
							case "%ASSOCIATES%"	:	Associates = desc; break;
                            case "%CONTACT%"    :   Contact = desc; break;
                            case "%CONTACTS%"   :   Contacts = desc; break;
                            case "%PRECEDENT%"	:	Precedent = desc; break;
							case "%PRECEDENTS%"	:	Precedents = desc; break;
							case "%FEEEARNER%"	:	FeeEarner = desc; break;
							case "%FEEEARNERS%"	:	FeeEarners = desc; break;
							case "%RESPONSIBLE%":	Responsible = desc; break;
							case "%RESPONSIBLES%":	Responsibles = desc; break;
                            case "%DEPT%"       : Department = desc; break;
                            case "%DEPTS%"      : Departments = desc; break;
							default:
								Custom.Add (row["cdcode"].ToString(), desc);
								break;
						}	

					}
				}
				dt.Dispose();
				dt = null;
			
			}


		}

		
		/// <summary>
		/// Parses the passed string with the relevant terminology changes.
		/// </summary>
		/// <param name="text">Text to be parsed.</param>
		/// <param name="plurals">True to check plurals.</param>
		/// <returns>Correctly parsed return value.</returns>
		public string Parse(string text, bool plurals)
		{
			text = text.Replace("#13#10", Environment.NewLine);

			if (text.IndexOf("%") == -1)
				return text;

			text = text.Replace("%CLIENT%", Client);
			text = text.Replace("%FILE%", File);
			text = text.Replace("%ASSOCIATE%", Associate);
            text = text.Replace("%CONTACT%", Contact);
            text = text.Replace("%PRECEDENT%", Precedent);
			text = text.Replace("%FEEEARNER%", FeeEarner);
			text = text.Replace("%RESPONSIBLE%", Responsible );
			text = text.Replace("%APPNAME%", AppName);
            text = text.Replace("%DEPT%", Department);

			if (plurals)
			{
				text = text.Replace("%CLIENTS%", Clients);
				text = text.Replace("%FILES%", Files);
				text = text.Replace("%ASSOCIATES%", Associates);
                text = text.Replace("%CONTACTS%", Contacts);
                text = text.Replace("%PRECEDENTS%", Precedents);
				text = text.Replace("%FEEEARNERS%", FeeEarners);
				text = text.Replace("%RESPONSIBLES%", Responsibles);
                text = text.Replace("%DEPTS%", Departments);
			}

			foreach (string itm in Custom.Keys)
			{
				text = text.Replace(itm, Custom[itm]);
			}

			return text;
		}


		/// <summary>
		/// String represetation of Terminology Object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string text = Session.CurrentSession.Edition;
			if (text=="") text = "EP";
			return text + " (" + Thread.CurrentThread.CurrentCulture.Name + ")";
		}


	}
}
