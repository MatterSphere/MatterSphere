using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using FWBS.Common;

namespace FWBS.OMS.DTS
{

    public class Configuration
	{
		#region Enums

		public enum DownloadType
		{
			Other,
			LFS,
			FTP,
			Directory
		}

		#endregion

		#region Fields

		private System.Data.DataSet _configuration = null;
		private System.Diagnostics.TraceSwitch _trace = new System.Diagnostics.TraceSwitch("COREIMPORT", "OMS Core Import");
		private string _ref = "";
		private System.IO.FileInfo _file = null;
		private string [] _tables = new string[16]{"CONTTYPE", "CLTYPE", "FILETYPE", "ASSOCTYPE", "INFOTYPE", "SEARCH", "CONTACT", "CLIENT", "FILE", "FILE_ALLOC", "LOOKUP", "EXTENDEDDATA", "MILESTONE", "FEEEARNER", "OPTION", "SOURCE"};
		private XmlElement _errors = null;

		#endregion

		#region Events

		public event FWBS.OMS.MessageEventHandler Error = null;

		private void OnError(FWBS.OMS.MessageEventArgs e)
		{
			if (Error != null)
			{
				Error(this, e);
			}
		}

		private void OnError(Exception ex)
		{
			FWBS.OMS.MessageEventArgs e = new FWBS.OMS.MessageEventArgs(ex);
			OnError(e);
		}

		#endregion

		#region Constants

		private const string CAT_IMPORT = "Importing";
		private const string CAT_CLIENT = "Client Creation";
		private const string CAT_FILE = "File Creation";
		private const string CAT_CONT = "Contact Creation";
		private const string CAT_ASSOC = "Associate Creation";
		private const string CAT_ADDRESS = "Address Creation";
		private const string CAT_PROPSET = "Property Set";
		private const string CAT_FILEALLOC = "File Allocation";

		#endregion

		#region Contructors

		private Configuration(){}
		
		public Configuration(string source)
		{
			Session.CurrentSession.CheckLoggedIn();
			System.Data.IDataParameter [] pars = new System.Data.IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.AddParameter("SOURCE", source);
			_configuration = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprCoreImportConfiguration", _tables, pars);
			_trace.Level = TraceLevel.Error | TraceLevel.Info | TraceLevel.Warning;
		}

		#endregion

		#region Import Methods

		private void Import(XmlDocument doc, string reference)
		{
			try
			{
				_ref = reference;
				XmlNode root = doc.SelectSingleNode("/OMS");
				_errors = doc.SelectSingleNode("/OMS/Errors") as XmlElement;

				if (root != null)
				{
					if (_errors == null)
					{
						_errors = doc.CreateElement("Errors");
						root.AppendChild(_errors);
					}

					foreach(XmlNode nd in root.ChildNodes)
					{
						if (nd != _errors)
						{
							switch (nd.Name.ToUpper())
							{
								case "CLIENT":
									FWBS.OMS.Client cl = CreateClient(nd as XmlElement);
									break;
								default:
									WriteError(String.Format("Root element not supported for importing. - Name: {0}", nd.Name), CAT_IMPORT, nd.InnerXml); 
									break;
							}
						}
					}
					foreach (PrecedentJob job in Session.CurrentSession.CurrentPrecedentJobList)
					{
						job.JobRow["usrid"] = SystemUsers.JobProcessor;
					}
					Session.CurrentSession.CurrentPrecedentJobList.Save();
				}
				else
                    throw new Exception(String.Format(Session.CurrentSession.Resources.GetMessage("XMLFLNTVLD", "Xml file not a valid OMS Core Import File.", "").Text));

			}
			catch(Exception ex)
			{
				WriteError(ex, CAT_IMPORT, "");
			}
			finally
			{
			}
		}

		public void Import(System.IO.FileInfo file)
		{
			XmlDocument doc = null;
			_file = file;
			try
			{
				doc = new XmlDocument();
				_ref = file.Name;
				doc.Load (file.FullName);
				Import(doc, file.Name);
				doc.Save(file.FullName);
			}
			catch(Exception ex)
			{
				WriteError(ex, CAT_IMPORT, "");
			}
			finally
			{
			}
		}


		#endregion

		#region Static Methods

		public static System.Data.DataTable GetSourceTypes()
		{
			Session.CurrentSession.CheckLoggedIn();
			System.Data.IDataParameter [] pars = new System.Data.IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.AddParameter("TYPE", "SOURCE");
			return Session.CurrentSession.Connection.ExecuteSQLTable("select cidata as [source] from dbcoreimportconfig where citype = @Type", "TYPES", pars);
		}

		#endregion

		#region Trace Methods

		private void WriteError(Exception ex, string category, string detail)
		{
			OnError(ex);
			SendMailFailure(ex.Message, ex.ToString() + Environment.NewLine + Environment.NewLine + detail);
			Trace.WriteIf(_trace.TraceError, ex, category);

            WriteXmlInfo(ex.Message, category, detail);
		}

		private void WriteError(string text, string category, string detail)
		{
			OnError(new FWBS.OMS.MessageEventArgs(text));
			SendMailFailure(text, detail);
			Trace.WriteIf(_trace.TraceError, text, category);

            WriteXmlInfo(text, category, detail);
		}

		private void WriteInfo(string text, string category, string detail)
		{
			Trace.WriteLineIf(_trace.TraceInfo, text, category);

            WriteXmlInfo(text, category, detail);

		}

        private void WriteXmlInfo(string text, string category, string detail)
        {
            if (_errors != null)
            {
                XmlNode err = _errors.OwnerDocument.CreateElement("Error");
                XmlAttribute atttime = _errors.OwnerDocument.CreateAttribute("time");
                XmlAttribute atttimezone = _errors.OwnerDocument.CreateAttribute("timeZone");
                XmlAttribute attmsg = _errors.OwnerDocument.CreateAttribute("message");
                XmlAttribute attcat = _errors.OwnerDocument.CreateAttribute("category");
                //UTCFIX: DM - 30/11/06 - Convert to utc time.
                err.Attributes.Append(atttime).Value = System.DateTime.UtcNow.ToString();
                err.Attributes.Append(atttimezone).Value = String.Format("UTC{0:zzz}", System.DateTime.Now);
                err.Attributes.Append(attmsg).Value = text;
                err.Attributes.Append(attcat).Value = category;
                _errors.AppendChild(err);
            }
        }

		#endregion

		#region Properties

		public string AccountSource
		{
			get
			{
				return Convert.ToString(_configuration.Tables["SOURCE"].Rows[0]["cidata"]);
			}
		}

		public Configuration.DownloadType AccountDownloadType
		{
			get
			{
				switch(Convert.ToString(_configuration.Tables["SOURCE"].Rows[0]["cisource"]))
				{
					case "LFS":
						return Configuration.DownloadType.LFS;
					case "WEB":
						return Configuration.DownloadType.LFS;
					case "DIR":
						return Configuration.DownloadType.Directory;
					case "FTP":
						return Configuration.DownloadType.FTP;
					default:
						return Configuration.DownloadType.Other;
				}
			}
		}


		public string AccountSite
		{
			get
			{
				return Convert.ToString(_configuration.Tables["SOURCE"].Rows[0]["cifilter1"]);
			}
			set
			{
				_configuration.Tables["SOURCE"].Rows[0]["cifilter1"] = value;
			}
		}

		public string AccountName
		{
			get
			{
				return Convert.ToString(_configuration.Tables["SOURCE"].Rows[0]["cifilter2"]);
			}
			set
			{
				_configuration.Tables["SOURCE"].Rows[0]["cifilter2"] = value;
			}
		}

		public string AccountPassword
		{
			get
			{
				return Convert.ToString(_configuration.Tables["SOURCE"].Rows[0]["cifilter3"]);
			}
			set
			{
				_configuration.Tables["SOURCE"].Rows[0]["cifilter3"] = value;
			}
		}

		public int AccountMaxDownload
		{
			get
			{
				try
				{
					return Convert.ToInt32(GetOption("MAXDOWNLOAD", "0"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetOption("MAXDOWNLOAD", value.ToString());
			}
		}

		public string LoggingEmail
		{
			get
			{
				return GetOption("LOGGINGEMAIL", "");
			}
			set
			{
				SetOption("LOGGINGEMAIL", value);
			}
		}


		public bool AccountDeleteFiles
		{
			get
			{
				return (GetOption("DELETEFILES", "FALSE") == "TRUE");
			}
			set
			{
				if (value)
					SetOption("DELETEFILES", "TRUE");
				else
					SetOption("DELETEFILES", "FALSE");
			}
		}

		public bool AccountTransformNeeded
		{
			get
			{
				return (GetOption("TRANSFORM", "TRUE") == "TRUE");
			}
			set
			{
				if (value)
					SetOption("TRANSFORM", "TRUE");
				else
					SetOption("TRANSFORM", "FALSE");
			}
		}

		public int FirmContact
		{
			get
			{
				try
				{
					return Convert.ToInt32(GetOption("FIRMCONTACT", "-1"));
				}
				catch
				{
					return -1;
				}
			}
			set
			{
				SetOption("FIRMCONTACT", value.ToString());
			}
		}


		public bool VerboseLogging
		{
			get
			{
				return (GetOption("LOGGING", "VERBOSE") == "VERBOSE");
			}
			set
			{
				if (value)
					SetOption("LOGGING", "VERBOSE");
				else
					SetOption("LOGGING", "ERRORSONLY");
			}
		}
		#endregion

		#region Methods

		public void Update()
		{
			string [] sql_s = new string[_tables.Length];
			for(int ctr = 0 ; ctr < sql_s.Length; ctr++)
				sql_s[ctr] = "select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata from dbcoreimportconfig";
			Session.CurrentSession.Connection.Update(_configuration, _tables, sql_s);
		}

		public System.Data.DataTable GetConfigTable(string name)
		{
			System.Data.DataTable dt = _configuration.Tables[name];
			dt.Columns["citype"].DefaultValue = name;
			dt.Columns["cisource"].DefaultValue = AccountSource;
			return dt;
		}

		#endregion

		private FWBS.OMS.Address CreateAddress(XmlElement el_add)
		{
			string info = "";

			try
			{
				FWBS.OMS.Address add = null;

				if (el_add.Name.ToUpper() == "ADDRESS")
				{
					XmlAttribute attr_id = el_add.Attributes["ident"];
					XmlAttribute attr_type = el_add.Attributes["type"];
					
					//Find an exisitn gidentifier or type.
					long id = -1;
					string type = "HOME";
					
					if (attr_id != null)
						id = ConvertDef.ToInt64(attr_id.Value, -1);
					else
						attr_id = el_add.Attributes.Append(el_add.OwnerDocument.CreateAttribute("ident"));

					if (attr_type != null)
						type = Convert.ToString(attr_type.Value);
					else
						attr_type = el_add.Attributes.Append(el_add.OwnerDocument.CreateAttribute("type"));

				
					if (type == String.Empty)
						type = "HOME";

					type = GetInfoType(type);
					
					if (type == String.Empty) type = "HOME";

					//Create or fetch the object.
					if (id == -1)
					{
						add = new FWBS.OMS.Address();
					}
					else
					{
						add = FWBS.OMS.Address.GetAddress(id);
					}

					info = "Type : " + type;
					add.AddType = type;

					//Loop through each property element.
					foreach (XmlNode nd in el_add.ChildNodes)
					{
						XmlElement el = (XmlElement)nd;
						switch (el.Name.ToUpper())
						{
							case "LINE1":
								add.Line1 = el.InnerText.Trim();
								break;
							case "LINE2":
								add.Line2 = el.InnerText.Trim();
								break;
							case "LINE3":
								add.Line3 = el.InnerText.Trim();
								break;
							case "LINE4":
								add.Line4 = el.InnerText.Trim();
								break;
							case "LINE5":
								add.Line5 = el.InnerText.Trim();
								break;
							case "POSTCODE":
								add.Postcode = el.InnerText.Trim();
								break;
							case "COUNTRY":
								add.Country = el.InnerText.Trim();
								break;
							case "COUNTRYID":
								add.CountryID = FWBS.Common.ConvertDef.ToInt32(el.InnerText.Trim(), -1);
								break;
						}

						info += Environment.NewLine;
						info = info + el.Name + ": ";
						info = info + el.InnerText;
					}

					bool isnew = add.IsNew;

					add.Update();

					if (isnew)
					{
						WriteInfo(String.Format("Address Created - ID: {0}", add.ID), CAT_ADDRESS, info);
					}
					else
					{
						WriteInfo(String.Format("Address Updated. - ID: {0}", add.ID), CAT_ADDRESS, info);
					}
					attr_id.Value = add.ID.ToString();

				}
				else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("ADDRXMLNTXP", "Address XML Node Expected.", "").Text);

				return add;
			}
			catch (Exception ex)
			{
				WriteError(ex, CAT_ADDRESS, el_add.InnerXml);
				return null;
			}
		}

		private FWBS.OMS.Contact CreateContact(XmlElement el_cont)
		{
			string info = "";

			try
			{
				FWBS.OMS.Contact cont = null;
				FWBS.OMS.ContactType ct = null;
				FWBS.OMS.Address defadd = null;

				if (el_cont.Name.ToUpper() == "CONTACT")
				{
					XmlAttribute attr_id = el_cont.Attributes["ident"];
					XmlAttribute attr_type = el_cont.Attributes["type"];
					XmlAttribute attr_code = el_cont.Attributes["code"];
					XmlAttribute attr_assoc = el_cont.Attributes["associate"];
					XmlAttribute attr_theirref = el_cont.Attributes["theirRef"];

					//Find an exisitn gidentifier or type.
					long id = -1;
					string type = "INDIVIDUAL";
					string code = "";
					string assoc = "";
					string theirref = "";

					if (attr_id != null)
						id = ConvertDef.ToInt64(attr_id.Value, -1);
					else
						attr_id = el_cont.Attributes.Append(el_cont.OwnerDocument.CreateAttribute("ident"));
					
					if (attr_type != null)
						type = Convert.ToString(attr_type.Value);
					else
						attr_type = el_cont.Attributes.Append(el_cont.OwnerDocument.CreateAttribute("type"));

					if (attr_code != null)
						code = Convert.ToString(attr_code.Value);
					else
						attr_code = el_cont.Attributes.Append(el_cont.OwnerDocument.CreateAttribute("code"));

					if (attr_assoc != null)
						assoc = Convert.ToString(attr_assoc.Value);
					else
						attr_assoc = el_cont.Attributes.Append(el_cont.OwnerDocument.CreateAttribute("assoc"));

					if (attr_theirref != null)
						theirref = Convert.ToString(attr_theirref.Value);
					else
						attr_theirref = el_cont.Attributes.Append(el_cont.OwnerDocument.CreateAttribute("theirRef"));

					if (type == String.Empty)
						type = "INDIVIDUAL";

					if (assoc != String.Empty)
						assoc = GetAssociateType(assoc);



					//Get the contact
					if (code != String.Empty)
					{
						cont = GetContact(code);
						if (cont == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("PRDFCTDNTMTCH", "Pre-Defined Contact does not match. - Code: %1%", "", code).Text);
					}



					//If contact still not found use the identifier.
					if (cont == null)
					{
						//Create or fetch the object.
						if (id == -1)
						{	
							info = "Type : " + type;

							//Fetch the object type.
							ct = GetContactType(type);	

							if (ct == null)
                                throw new Exception(Session.CurrentSession.Resources.GetMessage("CFGCTDNTMTCH", "Configured Contact Type not valid. - Type: %1%", "", type).Text);

							XmlNode el_defadd = el_cont.SelectSingleNode("DefaultAddress/Address");
							if (el_defadd == null)
							{
								XmlNode nd_path = el_cont.SelectSingleNode("DefaultAddress/@xpath");
								if (nd_path != null)  						
								{
									if (nd_path.Value == "") nd_path.Value = "../../Addresses/Address[1]";
									el_defadd = nd_path.SelectSingleNode(nd_path.Value);
								}

								if (el_defadd == null)
									el_defadd = el_cont.SelectSingleNode("Addresses/Address[1]");

								if (el_defadd == null)
								{
									if (nd_path != null)
									{
										if (nd_path.Value.Trim() == String.Empty) nd_path.Value = "../../../Contact[1]/Addresses/Address[1]"; 
										el_defadd = nd_path.SelectSingleNode(nd_path.Value);
									}

									if (el_defadd == null)
										el_defadd = el_cont.SelectSingleNode("../Contact[1]/Addresses/Address[1]");
								}

								if (el_defadd == null)
                                    throw new Exception(Session.CurrentSession.Resources.GetMessage("CTDFADDREQ", "A contacts Default Address is required.", "").Text);
							}

							//Check to see if required information exists.
							XmlNode el_contname = el_cont.SelectSingleNode("Name");
							if (el_contname == null || el_contname.InnerText.Trim() == String.Empty)
                                throw new Exception(Session.CurrentSession.Resources.GetMessage("CNTNMREQ", "Contact Name is required.", "").Text);


							defadd = CreateAddress(el_defadd as XmlElement);
						
							if (defadd == null)
                                throw new Exception(Session.CurrentSession.Resources.GetMessage("CTDFADDREQ", "A contacts Default Address is required.", "").Text);

                            info += Environment.NewLine;
							info = info + "Address :" + defadd.ToString();

							cont = new FWBS.OMS.Contact(ct);

							cont.DefaultAddress = defadd;

						}
						else
						{
							cont = FWBS.OMS.Contact.GetContact(id);
						}
					}

					defadd = cont.DefaultAddress;
					ct = cont.CurrentContactType;

					//Loop through each property element.
					foreach (XmlNode nd in el_cont.ChildNodes)
					{
						XmlElement el = (XmlElement)nd;
						switch (el.Name.ToUpper())
						{
							case "NAME":
								cont.Name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(el.InnerText.Trim().ToLower());
								break;
							case "INDIVIDALTITLE":
								SetObjectProperty(cont, el, true);
								break;
							case "INDIVIDUALCHRISTIANNAMES":
								goto case "INDIVIDALTITLE";
							case "INDIVIDUALSURNAME":
								goto case "INDIVIDALTITLE";

							case "DEFAULTADDRESS": //Already implemented above.
								break;
							case "ADDRESSES":
								foreach(XmlNode nd_othadds in el.ChildNodes)
								{
									XmlElement el_othadds = (XmlElement)nd_othadds;
									FWBS.OMS.Address add = CreateAddress(el_othadds);
									if (add != null)
										cont.AddAddress(add);
								}
								break;
							default:
								SetObjectProperty(cont, el, false);
								break;
						}

						info += Environment.NewLine;
						info = info + el.Name + ": ";
						info = info + el.InnerText;

					}

					bool isnew = cont.IsNew;

					//Associate specific memory values.
					cont.AssociateAs = assoc;
					cont.AssociateTheirRef = theirref;

					cont.Update();
					if (isnew)
					{
						WriteInfo(String.Format("Contact Created - ID: {0}", cont.ID), CAT_CONT, info);
					}
					else
						WriteInfo(String.Format("Contact Updated. - ID: {0} Name: {1}", cont.ID, cont.Name), CAT_CONT, info);
					
					attr_id.Value = cont.ID.ToString();

				}
				else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("CNTXMLNDEXP", "Contact XML Node Expected.", "").Text);

				return cont;
			}
			catch (Exception ex)
			{
				WriteError(ex, CAT_CONT, el_cont.InnerXml);
				return null;
			}
		}

		private FWBS.OMS.Client CreateClient(XmlElement el_cl)
		{
			string info = "";

			try
			{
				FWBS.OMS.Client cl = null;
				FWBS.OMS.ClientType clt = null;
				FWBS.OMS.Contact defcont = null;

				if (el_cl.Name.ToUpper() == "CLIENT")
				{
					XmlAttribute attr_id = el_cl.Attributes["ident"];
					XmlAttribute attr_type = el_cl.Attributes["type"];
					XmlAttribute attr_number = el_cl.Attributes["number"];
					XmlAttribute attr_code = el_cl.Attributes["code"];

					//Additional Search
					XmlAttribute attr_schfld = el_cl.Attributes["schFld"];
					XmlAttribute attr_schfldval = el_cl.Attributes["schFldVal"];
	
					//Find an exisitng identifier or type.
					long id = -1;
					string type = "INDIVIDUAL";
					string number = "";
					string code = "";
					string schfld = "";
					string schfldval = "";


					if (attr_id != null)
						id = ConvertDef.ToInt64(attr_id.Value, -1);
					else
						attr_id = el_cl.Attributes.Append(el_cl.OwnerDocument.CreateAttribute("ident"));

					if (attr_type != null)
						type = Convert.ToString(attr_type.Value);
					else
						attr_type = el_cl.Attributes.Append(el_cl.OwnerDocument.CreateAttribute("type"));

					if (attr_number != null)
						number = Convert.ToString(attr_number.Value);
					else
						attr_number = el_cl.Attributes.Append(el_cl.OwnerDocument.CreateAttribute("number"));

					if (attr_code != null)
						code = Convert.ToString(attr_code.Value);
					else
						attr_code = el_cl.Attributes.Append(el_cl.OwnerDocument.CreateAttribute("code"));

					//Additional filter.
					if (attr_schfld != null)
						schfld = Convert.ToString(attr_schfld.Value);
					else
						attr_schfld = el_cl.Attributes.Append(el_cl.OwnerDocument.CreateAttribute("schFld"));

					if (attr_schfldval != null)
						schfldval = Convert.ToString(attr_schfldval.Value);
					else
						attr_schfldval = el_cl.Attributes.Append(el_cl.OwnerDocument.CreateAttribute("schFldVal"));




					if (type == String.Empty)
						type = "INDIVIDUAL";

					//Check to see if an external code is used for the client first.
					if (schfld != String.Empty && schfldval != String.Empty)
					{
						try
						{
							cl = Client.GetClient(schfldval, schfld);
						}
						catch{}
					}

					//Check to see if any of the files exist under an existing client before creating a client.
					//Loop through all of the files to add.
					XmlNode el_files = el_cl.SelectSingleNode("Files");
					if (el_files != null)
					{
						foreach (XmlNode nd_file in el_files.ChildNodes)
						{
							XmlAttribute attr_file_id = nd_file.Attributes["ident"];
							XmlAttribute attr_file_schfld = nd_file.Attributes["schFld"];
							XmlAttribute attr_file_schval = nd_file.Attributes["schFldVal"];
							
							FWBS.OMS.OMSFile file = null;

							if (attr_file_schfld != null && attr_file_schval != null)
							{
								if (attr_file_schfld.Value.Trim() != String.Empty && attr_file_schval.Value.Trim() != String.Empty)
								{
									try
									{
										file = OMSFile.GetFile(attr_file_schval.Value, attr_file_schfld.Value);
									}
									catch{}
									
								}
							}

							if (file == null && attr_file_id != null)
							{
								if (attr_file_id.Value.Trim() != String.Empty)
								{
									try
									{
										file = OMSFile.GetFile(Convert.ToInt64(attr_file_id.Value));
									}
									catch{}
								}
							}

							if (file != null)
							{
								cl = file.Client;
								break;
							}
						}
					}
						
					//Get the client
					if (code != String.Empty && cl == null)
					{
						cl = GetClient(code);
						if (cl == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("PRDFCLDNTMTCHCD", "Pre Defined Client Code does not match. - Code: %1%", "", code).Text);
					}

					XmlNode el_defcont = el_cl.SelectSingleNode("DefaultContact/Contact");
					if (el_defcont == null)
					{
						XmlNode nd_path = el_cl.SelectSingleNode("DefaultContact/@xpath");

						if (nd_path != null)  						
						{
							if (nd_path.Value == "") nd_path.Value = "../../Contacts/Contact[1]";
							el_defcont = nd_path.SelectSingleNode(nd_path.Value);
						}

						if (el_defcont == null)
							el_defcont = el_cl.SelectSingleNode("Contacts/Contact[1]");

						if (el_defcont == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("DFTCNTREQ", "A Default Contact is required.", "").Text);
							
					}

					//Check to see if required information exists.
					XmlNode el_clname = el_cl.SelectSingleNode("Name");
					if (el_clname == null || el_clname.InnerText.Trim() == String.Empty)
                        throw new Exception(Session.CurrentSession.Resources.GetMessage("CLNAMREQ", "Client Name is required.", "").Text);

					defcont = CreateContact(el_defcont as XmlElement);

					if (defcont == null)
                        throw new Exception(Session.CurrentSession.Resources.GetMessage("DFTCNTREQ", "A Default Contact is required.", "").Text);


                    //If client still not found use the identifier.
                    if (cl == null)
					{
						//Create or fetch the object.
						if (id == -1)
						{
							
							//Fetch the object type.
							clt = GetClientType(type);	

							info = "Type : " + type;

							if (clt == null)
                                throw new Exception(Session.CurrentSession.Resources.GetMessage("CFGCLTPNTVLD", "Configured Client Type not valid. - Type: %1%", "", type).Text);

							cl = new FWBS.OMS.Client(clt, defcont, false);
							cl.SetExtraInfo("feeusrid", FirmContact);

						}
						else
						{
							cl = FWBS.OMS.Client.GetClient(id);
						}
					}

					clt = cl.CurrentClientType;

					cl.SetExtraInfo("clautocreated", System.DateTime.Now);
					cl.SetExtraInfo("clautotype", "XML");
					cl.SetExtraInfo("clautosource", "COREIMPORT");

					info += Environment.NewLine;
					info += "Default Contact : " ;
					info += defcont.Name;

					//Loop through each property element.
					foreach (XmlNode nd in el_cl.ChildNodes)
					{
						XmlElement el = (XmlElement)nd;
						switch (el.Name.ToUpper())
						{
							case "NAME":
								cl.ClientName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(el.InnerText.Trim().ToLower());
								cl.GenerateSearchKeywords(true);
								break;
							case "DEFAULTCONTACT": //Already implemented above.
								break;
							case "CONTACTS":
								foreach(XmlNode nd_othconts in el.ChildNodes)
								{
									XmlElement el_othconts = (XmlElement)nd_othconts;
									FWBS.OMS.Contact cont = CreateContact(el_othconts);
									if (cont != null)
									{
										FWBS.OMS.ClientContactLink link = new FWBS.OMS.ClientContactLink(cl, cont);
										cl.AddContact(link);
									}
																					
								}
								break;
							case "FILES":	//Implemented after the update, just incase the client has not been created yet.
								break;
							case "SOURCE":
								if (el.ChildNodes.Count > 0)
								{
									Contact src = CreateContact(el.ChildNodes[0] as XmlElement);
									if (src == null)
									{
										cl.SourceOfBusiness = GetLookup("SOURCE", el.InnerText);
										info += Environment.NewLine;
										info = info + el.Name + ": ";
										info = info + el.InnerText;
									}
									else
									{
										cl.SourceIsContact = src;
										info += Environment.NewLine;
										info = info + el.Name + ": ";
										info = info + src.Name;
									}

								}
								continue;
							default:
								SetObjectProperty(cl, el, false);
								break;
						}

						info += Environment.NewLine;
						info = info + el.Name + ": ";
						info = info + el.InnerText;
					}
					
					bool isnew = cl.IsNew;

					cl.Update();

					if (isnew)
					{
						SendMailSuccess(String.Format("Client Created - ID: {0} - No: {1}", cl.ClientID, cl.ClientNo), info);
						WriteInfo(String.Format("Client Created - ID: {0} - No: {1}", cl.ClientID, cl.ClientNo), CAT_CLIENT, info);
					}
					else
						WriteInfo(String.Format("Client Updated. - ID: {0} - No: {1} - Name: {2}", cl.ClientID, cl.ClientNo, cl.ClientName), CAT_CLIENT, info);
					
					attr_id.Value = cl.ClientID.ToString();
					attr_number.Value = cl.ClientNo;

					//Loop through all of the files to add.
					XmlNode el_files_col = el_cl.SelectSingleNode("Files");
					if (el_files_col != null)
					{
						foreach (XmlNode nd_file in el_files_col.ChildNodes)
						{
							FWBS.OMS.OMSFile file = CreateFile(cl, nd_file as XmlElement);
						}
					}

				}
				else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("CLTXMLNDEXP", "Client XML Node Expected.", "").Text);

				return cl;
			}
			catch (Exception ex)
			{
				WriteError(ex, CAT_CLIENT, el_cl.InnerXml);
				return null;
			}
		}

		

		

		private FWBS.OMS.OMSFile CreateFile(FWBS.OMS.Client cl, XmlElement el_file)
		{
			string info = "";

			try
			{
				FWBS.OMS.OMSFile file = null;
				FWBS.OMS.FileType ft = null;

				if (el_file.Name.ToUpper() == "FILE")
				{
					XmlAttribute attr_id = el_file.Attributes["ident"];
					XmlAttribute attr_type = el_file.Attributes["type"];
					XmlAttribute attr_number = el_file.Attributes["number"];
					XmlAttribute attr_code = el_file.Attributes["code"];
					XmlAttribute attr_ref = el_file.Attributes["theirRef"];


					//Additional Search
					XmlAttribute attr_schfld = el_file.Attributes["schFld"];
					XmlAttribute attr_schfldval = el_file.Attributes["schFldVal"];

					//Find an exisitn gidentifier or type.
					long id = -1;
					string type = "TEMPLATE";
					string number = "";
					string code = "";
					string schfld = "";
					string schfldval = "";
					string theirref = "";

					if (attr_id != null)
						id = ConvertDef.ToInt64(attr_id.Value, -1);
					else
						attr_id = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("ident"));

					if (attr_type != null)
						type = Convert.ToString(attr_type.Value);
					else
						attr_type = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("type"));

					if (attr_number != null)
						number = Convert.ToString(attr_number.Value);
					else
						attr_number = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("number"));

					if (attr_code != null)
						code = Convert.ToString(attr_code.Value);
					else
						attr_code = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("code"));


					//Additional filter.
					if (attr_schfld != null)
						schfld = Convert.ToString(attr_schfld.Value);
					else
						attr_schfld = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("schFld"));

					if (attr_schfldval != null)
						schfldval = Convert.ToString(attr_schfldval.Value);
					else
						attr_schfldval = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("schFldVal"));

					if (attr_ref != null)
						theirref = Convert.ToString(attr_ref.Value);
					else
						attr_ref = el_file.Attributes.Append(el_file.OwnerDocument.CreateAttribute("theirRef"));


					if (type == String.Empty)
						type = "TEMPLATE";


					//Check to see if an external code is used for the file first.
					if (schfld != String.Empty && schfldval != String.Empty)
					{
						try
						{
							file = OMSFile.GetFile(schfldval, schfld);
						}
						catch{}
					}

					
					//Get the file
					if (code != String.Empty && file == null)
					{
						file = GetFile(code);
						if (file == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("PRDFFLCDNTMTCH", "Pre-defined File Code does not match. - Code: %1%", "", code).Text);
					}

					//Check to see if required information exists.
					XmlNode el_filedesc = el_file.SelectSingleNode("Description");
					if (el_filedesc == null || el_filedesc.InnerText.Trim() == String.Empty)
                        throw new Exception(Session.CurrentSession.Resources.GetMessage("FLDSCRREQ", "File Description is required.", "").Text);

					//If file still not found use the identifier.
					if (file == null)
					{
						//Fetch the object type.
						ft = GetFileType(type);	

						info = "Type : " + type;
					
						if (ft == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("CFGFLTPNTVLD", "Configured File Type not valid. - Type: %1%", "", type).Text);


						//Create or fetch the object.
						if (id == -1)
						{
							FeeEarner fee = GetNextFeeEarner(ft.Code, ft.DefaultDepartment, "");
							Session.CurrentSession.CurrentFeeEarner = fee;
							file = new FWBS.OMS.OMSFile(ft, cl);	
							file.Status = "LIVEAWAITACK";

							info += Environment.NewLine;
							info = info + "Fee Earner: ";
							info = info + fee.FullName;

							file.AddEvent("AUTOCREATE", Session.CurrentSession.Resources.GetMessage("AUTOSOURCE", "Auto Created By %1%", "", cl.AutoCreatedSource).Text, "");
						}
						else
						{
							file = FWBS.OMS.OMSFile.GetFile(id);
						}
						
					}

					ft = file.CurrentFileType;

					//Loop through each property element.
					foreach (XmlNode nd in el_file.ChildNodes)
					{
						XmlElement el = (XmlElement)nd;
						switch (el.Name.ToUpper())
						{
							case "DESCRIPTION":
								file.FileDescription = el.InnerText.Trim();
								break;
							case "ASSOCIATES":		//Do Later after the file is created.
								break;
							case "SOURCE":
								if (el.ChildNodes.Count > 0)
								{
									Contact src = CreateContact(el.ChildNodes[0] as XmlElement);
									if (src == null)
									{
										file.SourceOfBusiness = GetLookup("SOURCE", el.InnerText);
										info += Environment.NewLine;
										info = info + el.Name + ": ";
										info = info + el.InnerText;
									}
									else
									{
										file.SourceIsContact = src;
										info += Environment.NewLine;
										info = info + el.Name + ": ";
										info = info + src.Name;
									}
								}
								continue;
							default:
								SetObjectProperty(file, el, false);
								break;
						}

						info += Environment.NewLine;
						info = info + el.Name + ": ";
						info = info + el.InnerText;

					}

					bool isnew = file.IsNew;

					//If their is a reference then set all associate their references.
					if (theirref != "")
					{
						for(int assocctr = 0; assocctr < file.Associates.Count; assocctr++)
						{
							Associate assoc = file.Associates[assocctr];
							if (assoc.IsClient) assoc.TheirRef = theirref;
						}
					}

					file.Update();
		
					if (isnew)
					{
						SendMailSuccess(String.Format("File Created - ID: {0} - No: {1}", file.ID, file.FileNo), info);
						WriteInfo(String.Format("File Created - ID: {0} - No: {1}", file.ID, file.FileNo), CAT_FILE, info);
					}
					else
						WriteInfo(String.Format("File Updated. - ID: {0} - No: {1} - Desc: {2}", file.ID, file.FileNo, file.FileDescription), CAT_FILE, info);
					
					attr_id.Value = file.ID.ToString();
					attr_number.Value = file.FileNo;

					XmlElement el_assocs = el_file.SelectSingleNode("Associates") as XmlElement;
					if (el_assocs != null)
					{
						foreach (XmlNode nd_assoc in el_assocs.ChildNodes)
						{
							XmlElement el_assoc = (XmlElement)nd_assoc;
							FWBS.OMS.Associate assoc = CreateAssociate(file, el_assoc);
							if (assoc != null)
								file.Associates.Add(assoc);
						}
					}


				}
				else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("FLXMLNDEXP", "File XML Node Expected.", "").Text);

				return file;
			}
			catch (Exception ex)
			{
				WriteError(ex, CAT_FILE, el_file.InnerXml);
				return null;
			}
		}


		

		private FWBS.OMS.Associate CreateAssociate(OMSFile file, XmlElement el_assoc)
		{
			string info = "";

			try
			{
				FWBS.OMS.Associate assoc = null;
				FWBS.OMS.Contact cont = null;

				if (el_assoc.Name.ToUpper() == "ASSOCIATE")
				{
					XmlAttribute attr_id = el_assoc.Attributes["ident"];
					XmlAttribute attr_type = el_assoc.Attributes["type"];
					XmlAttribute attr_code = el_assoc.Attributes["code"];

					//Find an exisitn gidentifier or type.
					long id = -1;
					string type = "CLIENT";
					string code = "";

					if (attr_id != null)
						id = ConvertDef.ToInt64(attr_id.Value, -1);
					else
						attr_id = el_assoc.Attributes.Append(el_assoc.OwnerDocument.CreateAttribute("ident"));

					if (attr_type != null)
						type = Convert.ToString(attr_type.Value);
					else
						attr_type = el_assoc.Attributes.Append(el_assoc.OwnerDocument.CreateAttribute("type"));

					if (attr_code != null)
						code = Convert.ToString(attr_code.Value);
					else
						attr_code = el_assoc.Attributes.Append(el_assoc.OwnerDocument.CreateAttribute("code"));
				
					type = GetAssociateType(type);

					if (type == String.Empty)
						type = "CLIENT";
					
					//Get the contact
					if (code != String.Empty)
					{
						cont = GetContact(code);
						if (cont == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("PRDFCNTDNTMTCH", "Pre-Defined Contact Code does not match. - Code: %1%", "", code).Text);
					}



					//If cotnact still not found use the identifier.
					if (cont == null)
					{
						XmlElement el_cont = el_assoc.SelectSingleNode("Contact") as XmlElement;
						if (el_cont != null)
							cont = CreateContact(el_cont);
						if (cont == null)
                            throw new Exception(Session.CurrentSession.Resources.GetMessage("CNTREQASSOC", "Contact required for associate.", "").Text);

						info += Environment.NewLine;
						info = info + "Contact: ";
						info = info + cont.Name;
					}

					//Create or fetch the object.
					if (id == -1)
					{
						type = GetAssociateType(type);
					
						if (type == String.Empty)
							type = "CLIENT";

						info = "Type : " + type;

						assoc = new FWBS.OMS.Associate(cont, file, type);
					}
					else
					{
						assoc = FWBS.OMS.Associate.GetAssociate(id);
					}

					type = assoc.AssocType;

					//Loop through each property element.
					foreach (XmlNode nd in el_assoc.ChildNodes)
					{
						XmlElement el = (XmlElement)nd;
						switch (el.Name.ToUpper())
						{
							case "CONTACT":		//Performed above.
								break;
							default:
								SetObjectProperty(assoc, el, false);
								break;
						}

						info += Environment.NewLine;
						info = info + el.Name + ": ";
						info = info + el.InnerText;


					}

					//Dont update the associate if the file is new.
					if (file.IsNew == false)
					{
						bool isnew = assoc.IsNew;

						assoc.Update();
						
						if (isnew)
							WriteInfo(String.Format("Associate Created - ID: {0}", assoc.ID), CAT_FILE, info);
						else
							WriteInfo(String.Format("Associate Updated. - ID: {0} - Type: {1} - Name: {2}", assoc.ID, assoc.AssocType, assoc.Contact.Name), CAT_ASSOC, info);
					}
					attr_id.Value = assoc.ID.ToString();

				}
				else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("ASSXMLNDEXP", "Associate XML Node Expected.", "").Text);

				return assoc;
			}
			catch (Exception ex)
			{
				WriteError(ex, CAT_ASSOC, el_assoc.InnerXml);
				return null;
			}
		}

		private void SetObjectProperty(object obj, XmlElement el_prop, bool properCase)
		{
			try
			{
				if (obj == null || el_prop == null)
					return;

				string prop = el_prop.Name;
				string val = el_prop.InnerText;
				string ext = el_prop.GetAttribute("extended");
				string lookup = el_prop.GetAttribute("lookup");
				bool parse = Common.ConvDef.ToBoolean(el_prop.GetAttribute("parse"), false);

				if (el_prop.HasAttribute("properCase"))
					properCase = Common.ConvDef.ToBoolean(el_prop.GetAttribute("properCase"), false);
				
				Type t = obj.GetType();
				Type proptype = typeof(string);

				object data = null;

				if (parse)
				{
					FieldParser parser = new FieldParser(obj);
					val = Convert.ToString(parser.Parse(Convert.ToString(val)));
				}
					
				if (properCase)
					val = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(val.Trim().ToLower());

				if (ext != String.Empty)
				{
					ext = GetExtendedData(ext);
					if (obj is FWBS.OMS.Interfaces.IExtendedDataCompatible)
					{
						try
						{
							FWBS.OMS.Interfaces.IExtendedDataCompatible extinfo = (FWBS.OMS.Interfaces.IExtendedDataCompatible)obj;
							proptype = extinfo.ExtendedData[ext].GetExtendedDataType(prop);
							if (lookup != String.Empty)
								val = GetLookup(lookup, val);
							data = Convert.ChangeType(val, proptype);
							extinfo.ExtendedData[ext].SetExtendedData(prop, data);
							return;
						}
						catch(Exception ex)
						{
							Trace.WriteLineIf(_trace.TraceError, ex, CAT_PROPSET);
						}
					}
					else
						Trace.WriteLineIf(_trace.TraceWarning, String.Format("Extended item not used because '{0}' is not IExtendedDataCompatible.", t.Name), CAT_PROPSET);
				}
				
				PropertyInfo p = t.GetProperty(prop);
				if (p != null)
				{
					proptype = p.PropertyType;
					if (lookup != String.Empty)
						val = GetLookup(lookup, val);
					data = Convert.ChangeType(val, proptype);
					p.SetValue(obj, data, null); 
					return;
				}
				else
				{
					if (obj is FWBS.OMS.Interfaces.IExtraInfo)
					{
						FWBS.OMS.Interfaces.IExtraInfo extinfo = (FWBS.OMS.Interfaces.IExtraInfo)obj;
						proptype = extinfo.GetExtraInfoType(prop);
						if (lookup != String.Empty)
							val = GetLookup(lookup, val);
						data = Convert.ChangeType(val, proptype);
						extinfo.SetExtraInfo(prop, data);
						return;
					}
					Trace.WriteLineIf(_trace.TraceWarning, String.Format("Property cannot be found. - Property: {0}", prop), CAT_PROPSET);
				}
			}
			catch(Exception ex)
			{
				WriteError(ex, CAT_PROPSET, "");
			}

		}

		public FeeEarner GetNextFeeEarner(string filetype, string source1, string source2)
		{
			//Fetch the last fee earner used and store the next one to use.
			System.Data.DataTable dt = _configuration.Tables["FILE_ALLOC"];
			string filter = "(cifilter1 is null and cifilter2 is null and cifilter3 is null)";
			string statecode = "FILE_ALLOC";

			filter = "(cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(filetype) + "')";
	
			dt.DefaultView.RowFilter = filter;
			dt.DefaultView.Sort = "cidata";
	
			if (dt.DefaultView.Count == 0)
			{
				filter = "(cifilter1 is null and cifilter2 is null and cifilter3 is null)";;
				dt.DefaultView.RowFilter = filter;
				dt.DefaultView.Sort = "cidata";
			}
			else
				statecode = "FILE_ALLOC" + "_" + filetype;
						
			State state = new State(statecode);
			
			int feeid = Common.ConvertDef.ToInt32(state.Read(), Session.CurrentSession.CurrentFeeEarner.ID);

	
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = filter + " and cidata < " + feeid.ToString();
			vw.Sort = "cidata";
			int idx = vw.Count + 1;

			if (idx >= dt.DefaultView.Count)
				idx = 0;

			if (dt.DefaultView.Count > 0)
				feeid = Common.ConvertDef.ToInt32(dt.DefaultView[idx]["cidata"], Session.CurrentSession.CurrentFeeEarner.ID);
			state.Write(feeid);

			FeeEarner fee = null;
			try
			{
				fee = FeeEarner.GetFeeEarner(feeid);
			}
			catch(Exception ex)
			{
				fee = Session.CurrentSession.CurrentFeeEarner;
				Trace.WriteLineIf(_trace.TraceError, ex, CAT_FILEALLOC);
			}
			return fee;
		}

		public ContactType GetContactType(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["CONTTYPE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count > 0)
				type = Convert.ToString(vw[0]["cidata"]);
			
			return ContactType.GetContactType(type);
		}

		public ClientType GetClientType(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["CLTYPE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count > 0)
				type = Convert.ToString(vw[0]["cidata"]);
			
			return ClientType.GetClientType(type);
		}

		public FileType GetFileType(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["FILETYPE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count > 0)
				type = Convert.ToString(vw[0]["cidata"]);
			
			return FileType.GetFileType(type);

		}

		public string GetAssociateType(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["ASSOCTYPE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count <= 0)
				return type;
			else
				return Convert.ToString(vw[0]["cidata"]);

		}

		public string GetMilestonePlan(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["MILESTONE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count > 0)
				type = Convert.ToString(vw[0]["cidata"]);
			
			return type;

		}

		public string GetFeeEarner(string inits)
		{
			System.Data.DataTable dt = _configuration.Tables["FEEEARNER"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(inits) + "'";
			if (vw.Count > 0)
				inits = Convert.ToString(vw[0]["cidata"]);
			
			return inits;
		}

		public string GetInfoType(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["INFOTYPE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count <= 0)
				return type;
			else
				return Convert.ToString(vw[0]["cidata"]);
		}

		public string GetLookup(string type, string code)
		{
			System.Data.DataTable dt = _configuration.Tables["LOOKUP"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "' and cifilter2 = '" + Common.SQLRoutines.RemoveRubbish(code) + "'";
			if (vw.Count <= 0)
				return code;
			else
				return Convert.ToString(vw[0]["cidata"]);
		}

		public string GetExtendedData(string code)
		{
			System.Data.DataTable dt = _configuration.Tables["EXTENDEDDATA"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(code) + "'";
			if (vw.Count <= 0)
				return code;
			else
				return Convert.ToString(vw[0]["cidata"]);
		}

		public Contact GetContact(string code)
		{
			long id = -1;
			System.Data.DataTable dt = _configuration.Tables["CONTACT"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(code) + "'";
			if (vw.Count <= 0)
				id = -1;
			else
				id = Common.ConvertDef.ToInt64(vw[0]["cidata"], -1);

			if (id == -1)
				return null;
			else
				return Contact.GetContact(id);
		}

		public Client GetClient(string code)
		{
			long id = -1;
			System.Data.DataTable dt = _configuration.Tables["CLIENT"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(code) + "'";
			if (vw.Count <= 0)
				id = -1;
			else
				id = Common.ConvertDef.ToInt64(vw[0]["cidata"], -1);

			if (id == -1)
				return null;
			else
				return Client.GetClient(id);
		}

		public OMSFile GetFile(string code)
		{
			long id = -1;
			System.Data.DataTable dt = _configuration.Tables["FILE"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(code) + "'";
			if (vw.Count <= 0)
				id = -1;
			else
				id = Common.ConvertDef.ToInt64(vw[0]["cidata"], -1);

			if (id == -1)
				return null;
			else
				return OMSFile.GetFile(id);
		}

		public SearchEngine.SearchList GetSearch(string type)
		{
			System.Data.DataTable dt = _configuration.Tables["SEARCH"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count <= 0)
				return null;
			else
				return new SearchEngine.SearchList(Convert.ToString(vw[0]["cidata"]), null, null);

		}

		public string GetOption(string type, string def)
		{
			System.Data.DataTable dt = _configuration.Tables["OPTION"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			if (vw.Count <= 0)
				return def;
			else
			{
				if (Convert.ToString(vw[0]["cidata"]) == "")
					return def;
				else
					return Convert.ToString(vw[0]["cidata"]);
			}

		}

		public void SetOption(string type, string val)
		{
			System.Data.DataTable dt = _configuration.Tables["OPTION"];
			System.Data.DataView vw = new System.Data.DataView(dt);
			vw.RowFilter = "cifilter1 = '" + Common.SQLRoutines.RemoveRubbish(type) + "'";
			System.Data.DataRow r = null; 
			if (vw.Count <= 0)
			{
				r = dt.NewRow();
				r["citype"] = "OPTION";
				r["cisource"] = AccountSource;
				r["cifilter1"] = type;
				dt.Rows.Add(r);
			}
			else r = vw[0].Row;
			
			r["cidata"] = val;

		}

		public void SendMailFailure(string error, string detailedInfo)
		{
			try
			{
				string subject = "OMS Auto Creation - (Failure) - " + _ref + " - " + error;
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(Session.CurrentSession.CurrentUser.Email, LoggingEmail);
				m.Subject = subject;
				m.Body = detailedInfo;
				if (_file != null)
					m.Attachments.Add(new System.Net.Mail.Attachment(_file.FullName));
				Session.CurrentSession.SendMail(m);
			}
			catch(Exception ex)
			{
				OnError(ex);
			}
		}

		public void SendMailSuccess(string briefInfo, string detailedInfo)
		{
			try
			{
				string subject = "OMS Auto Creation - (Success) - " + _ref + " - " + briefInfo;
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(Session.CurrentSession.CurrentUser.Email, LoggingEmail);
	            m.Subject = subject;
				m.Body = detailedInfo;
				Session.CurrentSession.SendMail(m);

			}
			catch(Exception ex)
			{
				OnError(ex);
			}
			
		}
	}
}
