using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms.Design;
using System.Xml;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.SourceEngine;


namespace FWBS.OMS.Design.Package
{
    /// <summary>
    /// Summary description for Packages.
    /// </summary>
    public class Packages : FWBS.OMS.CommonObject
	{
		#region Events
		public event FWBS.OMS.Design.Import.ProgressEventHandler Progress;
		#endregion

		#region Fields
		private bool _cancel = false;
		private int _packagecount = 0;
		private bool _pathchanged = false;
		private System.IO.DirectoryInfo _expdir;
		private FWBS.OMS.Design.Export.TreeView _treeview = new FWBS.OMS.Design.Export.TreeView();
		private string _desc = "";
		private bool _descdirty = false;
		private string _help = "";
        private bool _versionoverride;
        private string[] _conditional;
        private string _afterinstall;
        private bool _readonly;

		#endregion
		
		#region Constructors
		public Packages(bool Readonly) : base()
		{
			_expdir = new System.IO.DirectoryInfo(FWBS.Common.SpecialFolders.GetFolderPath(Environment.SpecialFolder.Personal));
			_expdir = _expdir.CreateSubdirectory("OMS Exports");
			_treeview.Add(0,24,"ROOT","Root",true,-1,FWBS.OMS.Design.Export.PackageTypes.None,"Root",false,false);
            _readonly = Readonly;
            Session.CurrentSession.Resources.GetResource("AfterInstall", "After Install", "");
            Session.CurrentSession.Resources.GetResource("External", "External", "");
        }

		/// <summary>
		/// Clones a new Package from an old one. Derr!
		/// </summary>
		/// <param name="clone">The Package to Clone From</param>
        /// <param name="NewCode"></param>
		internal Packages(Packages clone, string NewCode) : this(false)
		{
			foreach (DataColumn cm in _data.Columns)
			{
                if (cm.ColumnName != this.FieldPrimaryKey)
                    _data.Rows[0][cm.ColumnName] = clone.GetDataTable().Rows[0][cm.ColumnName];
                else
                {
                    if (NewCode.Length > 15)
                        _data.Rows[0][cm.ColumnName] = NewCode.Substring(0, 15);
                    else
                        _data.Rows[0][cm.ColumnName] = NewCode;
                }
			}
		}
		#endregion

		#region Static
        public static Packages GetPackage(string Code)
        {
            return GetPackage(Code, false);
        }

		public static Packages GetPackage(string Code, bool ReadOnly)
		{
			Packages _package = new Packages(ReadOnly);
			_package.Fetch(Code);
			_package.Initilise(false,false);
			return _package;
		}

		public static bool PackageExists(string Code)
		{
			Packages _package = new Packages(true);
			return _package.Exists(Code);
		}

        /// <summary>
        /// Clones a New Package from a old One
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="NewCode"></param>
        /// <returns>Printer Object</returns>
        public static Packages Clone(string Code, string NewCode)
		{
			Packages _clone = new Packages(false);
			_clone.Fetch(Code);
			Packages _new = new Packages(_clone, NewCode);
			_new.Initilise();
			return _new;
		}

		public static DataTable GetPackageList()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT *, dbo.GetCodeLookupDesc('PACKAGE', pkgCode, @UI) as pkgdesc FROM dbPackages", "PACKAGES", paramlist);
			return dt;
		}

		public static DataTable GetImportedPackageList()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT *, dbo.GetCodeLookupDesc('PACKAGE', pkgCode, @UI) as pkgdesc FROM dbPackages WHERE pkgExternal = 1", "INSTALLED", paramlist);
			return dt;
		}

		
		public static void DeletePackage(string Code)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, Code);
			Session.CurrentSession.Connection.ExecuteSQL("DELETE FROM dbPackages WHERE pkgCode = @Code", paramlist);
		}
		#endregion

		#region Overrides
		public override object Parent
		{
			get
			{
				return null;
			}
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
				return "pkgCode";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "PACKAGE";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbPackages";
			}
		}

		protected override void Fetch(object id)
		{
			base.Fetch(id);
            try
			{
				_expdir = new System.IO.DirectoryInfo(Convert.ToString(GetExtraInfo("pkgExportLoc")));

				//TODO: Add Code to Load the Dependents from the XML
			}
			catch
			{
			}
		}

		#endregion

		#region Properties
		[LocCategory("(DETAILS)")]
		[RefreshProperties(RefreshProperties.All)]
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("pkgCode"));
			}
			set
			{	
				if (value != this.Code)
				{
					_desc = "";
					SetExtraInfo("pkgCode",value);
				}
			}
		}


		[LocCategory("(DETAILS)")]
		public string Description
		{
			get
			{
				return _desc;
			}
			set
			{
				_desc = value;
				_descdirty = true;
				OnDataChanged();
				if (this.TreeView.GotoRoot())
					this.TreeView.Name = value;
			}
		}

        [LocCategory("DATA")]
        [TextOnlyAttribute(true)]
        [Editor("FWBS.OMS.UI.Windows.Design.EnquiryFormEditor,omsAdmin", typeof(System.Drawing.Design.UITypeEditor))]
        public string AfterInstall
        {
            get
            {
                return _afterinstall;
            }
            set
            {
                _afterinstall = value;
            }
        }
		
        [LocCategory("(DETAILS)")]
		[TextOnlyAttribute(true)]
		[Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
		public string Help
		{
			get
			{
				return _help;
			}
			set
			{
				_help = value;
				_descdirty = true;
				if (this.TreeView.GotoRoot())
					this.TreeView.Description = value;
			}
		}

		[LocCategory("EXPORT")]
		[Editor(typeof(PackageDirectoryBrowser), typeof(System.Drawing.Design.UITypeEditor))]
		public string ExportPath
		{
			get
			{
				return _expdir.FullName;
			}
			set
			{
				if (this.ExportPath != value)
				{
					_expdir = new System.IO.DirectoryInfo(value);
					if (_expdir.Exists == false) _expdir.Create();
					SetExtraInfo("pkgExportLoc",value);
					_pathchanged = true;
				}
			}
		}

		[LocCategory("(DETAILS)")]
		public int Version
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("pkgVersion"),0);
			}
            set
            {
                SetExtraInfo("pkgVersion", value);
                _versionoverride = true;
            }
		}

        [LocCategory("(DETAILS)")]
        public string[] Conditional
        {
            get
            {
                return _conditional;
            }
            set
            {
                _conditional = value;
            }
        }

		[System.ComponentModel.Editor(typeof(DependentEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[LocCategory("DATA")]
		[Lookup("Dependents")]
		public DependentCollection DependentPackages
		{
			get
			{
				return this.TreeView.DependentPackages;
			}
		}

		[Browsable(false)]
		public FWBS.OMS.Design.Export.TreeView TreeView
		{
			get
			{
				return _treeview;
			}
			set
			{
				_treeview = value;
			}
		}

        [LocCategory("DATA")]
        public bool External
        {
            get
            {
                if (GetExtraInfo("pkgExternal") == DBNull.Value)
                    return false;
                else
                    return Convert.ToBoolean(GetExtraInfo("pkgExternal"));
            }
            set
            {
                if (value == false) _readonly = false;
                SetExtraInfo("pkgExternal", value);
            }
        }

		#endregion

		#region Methods
		public override void Update()
		{
			if (_descdirty)
				FWBS.OMS.CodeLookup.Create("PACKAGE",this.Code,this.Description,this.Help,CodeLookup.DefaultCulture,true,true,true);

			if (Convert.ToString(GetExtraInfo("pkgCode")) == "")
                throw new Exception(Session.CurrentSession.Resources.GetResource("PKGNOCODE", "A code is required for this Package.", "").Text);
			
			if (string.IsNullOrWhiteSpace(_desc))
                throw new Exception(Session.CurrentSession.Resources.GetResource("PKGNODESC", "A description is required for this Package.", "").Text);
				
			
            if (_versionoverride == false)
			    SetExtraInfo("pkgVersion",FWBS.Common.ConvertDef.ToInt64(GetExtraInfo("pkgVersion"),0)+1);
            _versionoverride = false;

			CreateTreeViewXml(this.Description,this.Help);

			try
			{
				base.Update();
			}
			catch (ConnectionException cex)
			{
				SqlException sqlex = cex.InnerException as SqlException;
				if (sqlex != null && sqlex.Message.StartsWith("Violation of PRIMARY KEY constraint 'PK_dbPackages'."))
					throw new OMSException2("ERRDUPPKG", "Package Name '%1%' already exists please choose another.",null, true, Code);

				throw;				
			}

			if (_pathchanged)
			{
				this.RefreshPackage();
				_pathchanged = false;
			}
		}

		public void RefreshPackage()
		{
			Initilise(true,true);
		}

		internal void Initilise()
		{
			Initilise(false,true);
		}
			
		internal void Initilise(bool Quitable, bool Update)
		{
            List<string> errors = new List<string>();
            bool _damaged = false;
            bool update = false;

			_help = FWBS.OMS.CodeLookup.GetLookupHelp("PACKAGE",this.Code);
			_desc = FWBS.OMS.CodeLookup.GetLookup("PACKAGE",this.Code);
			update = Update;

			System.IO.DirectoryInfo lastexpdir = new System.IO.DirectoryInfo(_expdir.FullName);
			if (lastexpdir.Exists)
				lastexpdir = lastexpdir.CreateSubdirectory(this.Code);
			else
				update = false;

			FWBS.Common.ConfigSetting _save = new FWBS.Common.ConfigSetting(base.GetDataLiveTable().Rows[0],"pkgXML");

            if (!ClearFolderBeforeUpdatingPackage(lastexpdir, update))
                return;

            ClearTreeViews();
            AddRootToTreeView(update, _save);
            ProcessTreeViewItems(_save, update, _damaged, errors, lastexpdir);

			this.DependentPackages.Cleared -=new Crownwood.Magic.Collections.CollectionClear(DependentPackages_Cleared);
			this.DependentPackages.Cleared +=new Crownwood.Magic.Collections.CollectionClear(DependentPackages_Cleared);

            ReportErrorsToUser(errors, _damaged);

            RebuildPackageTreeView(update, _damaged, _save);

			OnProgress(true);

			DataView dvc = new DataView(_treeview.Source);
			dvc.RowFilter = "RootImportable = true";
			_packagecount = dvc.Count;
            CreateTreeViewXml(this.Description, this.Help);
		}


        private void RebuildPackageTreeView(bool update, bool _damaged, FWBS.Common.ConfigSetting _save)
        {
            if (update && _damaged && _save.CurrentChildItems.Length > 0)
            {
                this.TreeView.Clear();
                foreach (FWBS.Common.ConfigSettingItem item in _save.CurrentChildItems)
                {
                    FWBS.OMS.Design.Export.PackageTypes _type = (FWBS.OMS.Design.Export.PackageTypes)FWBS.Common.ConvertDef.ToEnum(item.GetString("Type", ""), FWBS.OMS.Design.Export.PackageTypes.None);
                    if (_type != FWBS.OMS.Design.Export.PackageTypes.None || item.GetString("ParentID", "") == "-1")
                        this.TreeView.Add(Convert.ToInt32(item.GetString("ID", "")), Convert.ToInt32(item.GetString("ImageIndex", "")), item.GetString("Code", ""), item.GetString("Name", ""), Convert.ToBoolean(item.GetString("Active", "")), Convert.ToInt32(item.GetString("ParentID", "0")), _type, item.GetString("Description", ""), Convert.ToBoolean(item.GetString("RootImportable", "false")), Convert.ToBoolean(item.GetString("InstallOnce", "false")));
                }
            }
        }


        private void ProcessTreeViewItems(FWBS.Common.ConfigSetting _save, bool update, bool _damaged, List<string> errors, System.IO.DirectoryInfo lastexpdir)
        {
            foreach (FWBS.Common.ConfigSettingItem item in _save.CurrentChildItems)
            {
                if (item.Element.Name == "Items")
                {
                    FWBS.OMS.Design.Export.PackageTypes _type = (FWBS.OMS.Design.Export.PackageTypes)FWBS.Common.ConvertDef.ToEnum(item.GetString("Type", ""), FWBS.OMS.Design.Export.PackageTypes.None);
                    bool IsRoot = (Convert.ToBoolean(item.GetString("RootImportable", "true")));
                    if (!update)
                        this.TreeView.Add(Convert.ToInt32(item.GetString("ID", "")), Convert.ToInt32(item.GetString("ImageIndex", "")), item.GetString("Code", ""), item.GetString("Name", ""), Convert.ToBoolean(item.GetString("Active", "")), Convert.ToInt32(item.GetString("ParentID", "0")), _type, item.GetString("Description", ""), Convert.ToBoolean(item.GetString("RootImportable", "false")), Convert.ToBoolean(item.GetString("InstallOnce", "false")));
                    else
                    {
                        try
                        {
                            string active = item.GetString("Active", "true");
                            string runonce = item.GetString("InstallOnce", "false");
                            string itemcode = item.GetString("Code", "");
                            string itemimageindex = item.GetString("ImageIndex", "");
                            if (_cancel) break;
                            UpdateFileSystem(itemcode, itemimageindex, IsRoot, _damaged, _type, runonce, active, lastexpdir, _save);
                        }
                        catch (System.Data.ConstraintException ex)
                        {
                            Trace.TraceError(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(ex.Message);
                            errors.Add(item.GetString("Name", "").ToUpper() + " - " + item.GetString("Type", "") + " - ERROR: " + ex.Message);
                        }
                    }
                }
                else if (item.Element.Name == "Dependents")
                {
                    this.DependentPackages.Add(new Dependent(item.GetString("Code", ""), Convert.ToInt32(item.GetString("Version", "")), Convert.ToBoolean(item.GetString("VersionError", "False")), Convert.ToBoolean(item.GetString("VersionWarning", "False"))));
                }
            }
        }


        private void UpdateFileSystem(string itemcode, string itemimageindex, bool IsRoot, bool _damaged, FWBS.OMS.Design.Export.PackageTypes _type, string runonce, string active, System.IO.DirectoryInfo lastexpdir, FWBS.Common.ConfigSetting _save)
        {
            if (Convert.ToInt32(itemimageindex) == 25) _damaged = true;
            FWBS.OMS.Design.Export.ExportBase eb = GetExportBaseObject(_type, IsRoot, itemcode);
            if (eb != null)
                CompleteTreeViewItem(eb, _save, lastexpdir, active, runonce);
        }


        private Export.ExportBase GetExportBaseObject(Export.PackageTypes packageType, bool IsRoot, string itemcode)
        {
            FWBS.OMS.Design.Export.ExportBase eb = null;

            if (packageType == Export.PackageTypes.Milestones && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.Milestones(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.FileManagement && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.FileManagement(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.DataLists && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.DataLists(itemcode, this.TreeView);
            }
            else if ((packageType == Export.PackageTypes.Screens || packageType == Export.PackageTypes.EnquiryForms) && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.EnquiryForm(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.SearchLists && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.SearchList(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.Reports && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.Report(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.CodeLookups && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.CodeLookupType(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.Precedents && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.Precedent(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.Scripts && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.Scripts(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.SQLScripts && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.SQLScripts(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.ExtendedData && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.ExtendedData(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.OMSObjects && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.OMSObjects(itemcode, this.TreeView);
            }
            else if (packageType == Export.PackageTypes.DataPackages && IsRoot)
            {
                eb = new FWBS.OMS.Design.Export.DataPackage(itemcode, this.TreeView);
            }

            return eb;
        }


        private void CompleteTreeViewItem(FWBS.OMS.Design.Export.ExportBase eb, FWBS.Common.ConfigSetting _save, System.IO.DirectoryInfo lastexpdir, string active = "true", string runonce = "false")
        {
            OnProgress("Updating Package ...");
            eb.Active = Convert.ToBoolean(active);
            eb.RunOnce = Convert.ToBoolean(runonce);
            eb.TreeViewParentID = 0;
            eb.ExportTo(lastexpdir.FullName);
            ProcessLinkedObjects(eb, _save, lastexpdir);
        }


        private void ProcessLinkedObjects(FWBS.OMS.Design.Export.ExportBase eb, FWBS.Common.ConfigSetting _save, System.IO.DirectoryInfo lastexpdir)
        {
            if (eb.LinkedObjects != null && eb.LinkedObjects.Count > 0)
            {
                foreach (FWBS.OMS.Design.Export.LinkedObject lo in eb.LinkedObjects)
                {
                    if (!LinkedObjectExists(lo, _save))
                    {
                        Export.ExportBase eb2 = GetExportBaseObject(lo.packageType, true, lo.ID);
                        CompleteTreeViewItem(eb2, _save, lastexpdir);
                    }
                }
            }
        }


        private bool LinkedObjectExists(FWBS.OMS.Design.Export.LinkedObject lo, FWBS.Common.ConfigSetting _save)
        {
            foreach (FWBS.Common.ConfigSettingItem item in _save.CurrentChildItems)
            {
                if (lo.ID == item.GetString("Code", "") && lo.packageType.ToString() == item.GetString("Type", ""))
                    return true;
            }
            return false;
        }


        private void AddRootToTreeView(bool update, FWBS.Common.ConfigSetting _save)
        {
            if (update)
                this.TreeView.Add(0, 24, "ROOT", "Root", true, -1, FWBS.OMS.Design.Export.PackageTypes.None, "Root", false, false);
            Conditional = _save.GetString("Conditional", "").Split("|".ToCharArray());
            this.AfterInstall = _save.GetString("AfterInstall", "");
        }


        private void ClearTreeViews()
        {
            this.TreeView.Clear();
            this.TreeView.DependentPackages.Clear();
        }


        private bool ClearFolderBeforeUpdatingPackage(System.IO.DirectoryInfo lastexpdir, bool update)
        {
        	if (lastexpdir.Exists && update)
			{
				AskEventArgs askargs = new AskEventArgs("DELPKGFLD","Do you wish to Clear the Folder '%1%' before Updating?","",FWBS.OMS.AskResult.Yes,lastexpdir.FullName);
                Session.CurrentSession.OnAsk(this, askargs);
				if (askargs.Result == FWBS.OMS.AskResult.Yes) 
				{
					try
					{
                        foreach (System.IO.DirectoryInfo d in lastexpdir.GetDirectories())
                        {
                            d.Delete(true);
                        }
					}
					catch
					{
						askargs = new AskEventArgs("FAILEDTODEL","Failed to Delete. The Folder '%1%' is in use. Continue the Update ?","",FWBS.OMS.AskResult.Yes,lastexpdir.FullName);
						Session.CurrentSession.OnAsk(this,askargs);
						if (askargs.Result == FWBS.OMS.AskResult.No) 
							return false;
					}
				}
			}
            return true;
        }


        private void ReportErrorsToUser(List<string> errors, bool _damaged)
        {
            if (errors.Count > 0)
            {
                string msg = Session.CurrentSession.Resources.GetResource("ERRORPKGLST", "Error List\n\n%1%", "", String.Join(Environment.NewLine + Environment.NewLine, errors.ToArray())).Text;
                MessageEventArgs warn = new MessageEventArgs(msg);
                Session.CurrentSession.OnWarning(this, warn);
                _damaged = true;
            }
        }


        public void CreateTreeViewXml(string Name, string Help)
		{
			CreateTreeViewXml(false, Name, Help);
		}
		
		public void CreateTreeViewXml(bool Backup, string Name, string Help)
		{
            if (_readonly) return;

            this.SetExtraInfo("pkgXML","");

			FWBS.Common.ConfigSetting _save = new FWBS.Common.ConfigSetting(base.GetDataLiveTable().Rows[0],"pkgXML");
			
			FWBS.Common.ConfigSettingItem _item = null;
			DataView nodes = new DataView(this.TreeView.Source);
			nodes.RowStateFilter = DataViewRowState.OriginalRows;
			nodes.RowStateFilter = DataViewRowState.CurrentRows;
			nodes.Sort = "ID";

			foreach (System.Data.DataRowView dr in nodes)
			{
				this.TreeView.Goto(dr);
				_item = _save.AddChildItem("Items");
				_item.SetString("ID",this.TreeView.ID.ToString());
				_item.SetString("ImageIndex",this.TreeView.ImageIndex.ToString());
                _item.SetString("Code", this.TreeView.Code);
                if (this.TreeView.Code == "ROOT")
                {
                    _item.SetString("Name", Name);
                    _item.SetString("Description", Help);
                }
                else
                {
                    _item.SetString("Name", this.TreeView.Name);
                    _item.SetString("Description", this.TreeView.Description.ToString());
                }
				_item.SetString("Active",this.TreeView.Active.ToString());
				_item.SetString("ParentID",this.TreeView.ParentID.ToString());
				_item.SetString("Type",this.TreeView.Type.ToString());
				_item.SetString("RootImportable",this.TreeView.RootImportable.ToString());
                _item.SetString("InstallOnce", this.TreeView.InstallOnce.ToString());
            }
			
			foreach(Dependent dpt in this.DependentPackages)
			{
				if (dpt.Package.Code != "")
				{
					_item = _save.AddChildItem("Dependents");
					_item.SetString("Code",dpt.Package.Code);
					_item.SetString("Version",dpt.Version.ToString());
					_item.SetString("VersionWarning",dpt.VersionWarning.ToString());
					_item.SetString("VersionError",dpt.VersionError.ToString());
				}
			}

            _save.SetString("AfterInstall", this.AfterInstall);
			_save.SetString("Code",this.Code);
			_save.SetString("Backup",Backup.ToString());
			_save.SetString("Version",Convert.ToString(GetExtraInfo("pkgVersion")));
            if (this.Conditional != null)
                _save.SetString("Conditional", String.Join("|",this.Conditional));
            _save.Synchronise();

			System.IO.DirectoryInfo lastexpdir = new System.IO.DirectoryInfo(_expdir.FullName);
			if (lastexpdir.Exists)
			{
				lastexpdir = lastexpdir.CreateSubdirectory(this.Code);
				if (TreeView.FieldReplacer.Count > 0)
				{
					DataSet fieldreplace = new DataSet("FIELDREPLACER");
					fieldreplace.Tables.Add(TreeView.FieldReplacer.Copy);
					fieldreplace.WriteXml(lastexpdir.FullName + @"\" + FWBS.Common.FilePath.ExtractInvalidChars(this.Code) + ".Replacement",XmlWriteMode.WriteSchema);
				}
				_save.DocObject.Save(lastexpdir.FullName + @"\" + FWBS.Common.FilePath.ExtractInvalidChars(this.Code) + ".Manifest.xml");
			}
		}


		#endregion

		#region Progress
		[Browsable(false)]
		public int PackageCount
		{
			get
			{
				return _packagecount;
			}
		}

		protected bool OnProgress(bool close)
		{
			if (Progress != null)
			{
				FWBS.OMS.Design.Import.ProgressEventArgs e = new FWBS.OMS.Design.Import.ProgressEventArgs(close);
				Progress(this,e);
				return e.Cancel;
			}
			else
				return false;			
		}
		
		protected bool OnProgress(string label)
		{
			if (Progress != null)
			{
				FWBS.OMS.Design.Import.ProgressEventArgs e = new FWBS.OMS.Design.Import.ProgressEventArgs(label);
				Progress(this,e);
				return e.Cancel;
			}
			else
				return false;			
		}

		private void OnProgress(object sender, FWBS.OMS.Design.Import.ProgressEventArgs e)
		{
			_cancel = OnProgress(e.Label);
		}
		#endregion

		private void DependentPackages_Cleared()
		{

		}
	}

	public class Dependent
	{
		private CodeLookupDisplayReadOnly _package;
		private int _version = 0;
		private bool _versionwarning = false;
		private bool _versionerror = false;

		public Dependent(string Package)
		{
			_package = new CodeLookupDisplayReadOnly("PACKAGE");
			_package.Code = Package;
		}

		public Dependent(string Package, int Version, bool VersionError, bool VersionWarning)
		{
			_package = new CodeLookupDisplayReadOnly("PACKAGE");
			_package.Code = Package;
			_version = Version;
			_versionerror = VersionError;
			_versionwarning = VersionWarning;
		}

		[LocCategory("(Details)")]
		[CodeLookupSelectorTitle("PACKAGES","Packages")]
		[Parameter(CodeLookupDisplaySettings.Dependents)]
		public CodeLookupDisplayReadOnly Package
		{
			get
			{
				return _package;
			}
			set
			{
				_package = value;
				Packages _pkg = Packages.GetPackage(_package.Code);
				_version = Convert.ToInt32(_pkg.Version);
			}
		}

		[LocCategory("Data")]
		public int Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		[LocCategory("Data")]
		[RefreshProperties(RefreshProperties.All)]
		public bool VersionWarning
		{
			get
			{
				return _versionwarning;
			}
			set
			{
				_versionwarning = value;
				_versionerror = !value;
			}
		}

		[LocCategory("Data")]
		[RefreshProperties(RefreshProperties.All)]
		public bool VersionError
		{
			get
			{
				return _versionerror;
			}
			set
			{
				_versionerror = value;
				_versionwarning = !value;
			}
		}

		public override string ToString()
		{
			return _package.Description;
		}

	}

	public class DependentCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		public Dependent Add(Dependent value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);
			return value;
		}

		public void AddRange(Dependent[] values)
		{
			// Use existing method to add each array entry
			foreach(Dependent page in values)
				Add(page);
		}

		public void Remove(Dependent value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, Dependent value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(Dependent value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public Dependent this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as Dependent); }
		}

		public int IndexOf(Dependent value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}

	public class DependentEditor : System.ComponentModel.Design.CollectionEditor
	{
		public DependentEditor() : base (typeof(System.Collections.ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Dependent);
		}

		protected override object CreateInstance(System.Type t)
		{
			return new Dependent("");
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Dependent)};
		}
    }


	internal class PackageDirectoryBrowser : FolderNameEditor
	{

		private FolderBrowser folderBrowser = new FolderBrowser();
		private string description = "Please select a Location for Package";

		/// <summary>
		/// Gets a value that represents the folder the user chose.
		/// </summary>
		public string DirectoryPath
		{
			get
			{
				return folderBrowser.DirectoryPath;
			}
		}

		/// <summary>
		/// Initializes the dialog.
		/// </summary>
		/// <param name="folderBrowser">A FolderBrowser object.</param>
		protected override void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
			folderBrowser.Description = description;
			folderBrowser.StartLocation = FolderBrowserFolder.Desktop;
		}

		/// <summary>
		/// Displays the dialog.
		/// </summary>
		/// <param name="description">The description displayed with the dialog.</param>
		/// <returns>A DialogResult value representing whether the user chose OK or Cancel.</returns>
 
		public System.Windows.Forms.DialogResult ShowDialog(string description)
		{
			this.description = description;
			InitializeDialog(folderBrowser);
			return folderBrowser.ShowDialog();
		}
	}

	public enum ImportMethod {NewOnly,Overwrite};

	/// <summary>
	/// A PackageData class that holds the configuration data.
	/// </summary>
	public class PackageData : SourceEngine.Source, IDisposable
	{
		#region Fields
		protected DataTable _searchlisttb = null;
		protected DataTable _data = null;
		protected internal static string TableEdit = "dbPackageData";
		protected static string sql = "select *, dbo.GetCodeLookupDesc('PACKAGEDATA', pkdCode, @UI) as pkdDesc from " + TableEdit;
		protected static string updateablesql = "select * from " + TableEdit;
		/// <summary>
		/// The Data Row that contains the Settings
		/// </summary>
		protected DataRow _dr;
		/// <summary>
		/// Data List Description
		/// </summary>
		protected string _description;
		/// <summary>
		/// The Main Document XML
		/// </summary>
		protected XmlDocument _xmlDParams;
		/// <summary>
		/// The Parameter XMLs
		/// </summary>
		protected XmlNode _xmlParameter;
		/// <summary>
		/// The Original Code to compare against for Renaming
		/// </summary>
		protected string _orgcode = "";

		#endregion

		#region Constructors
	
		/// <summary>
		/// Disable the use of the default constructor.  
		/// This object does not need to be created in this way.
		/// </summary>
		public PackageData() : this("")
		{}

		internal PackageData(DataTable FromDataPackage) : this("",true)
		{
			_searchlisttb.Rows.Add(FromDataPackage.Rows[0].ItemArray);
			DataListEditorInt();
		}

		public PackageData(string code) : this (code,false)
		{
			DataListEditorInt();
		}
		
		public PackageData(string code, bool Clone)
		{
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Code", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name);
			_searchlisttb = Session.CurrentSession.Connection.ExecuteSQLTable(sql + " where pkdCode = @Code" , TableEdit,  paramlist);
			if (_searchlisttb.Rows.Count==0 && Clone == false)
			{
				_searchlisttb.Rows.Add(_searchlisttb.NewRow());
				_searchlisttb.Rows[0]["pkdParameters"] = "<params></params>";
				_searchlisttb.Rows[0]["pkdSourceType"] = "OMS";
			}
		}


		#endregion
	
		#region Methods
		public DataTable GetTable()
		{
			//A data object that could hold any list compatible data.
			object data = null;
			data = base.Run();
			if (data is DataTable)
			{
				DataTable dt = (DataTable)data;
				dt.TableName = "SOURCE";
				_data = (DataTable)data;
				return _data;
			}
			else
			{
				throw new FWBS.OMS.SearchEngine.SearchException(HelpIndexes.SearchNotCompatibleResultset);
			}
		}

		public void LoadTable(DataTable From, FWBS.OMS.Design.Export.FieldReplacer fieldreplacer)
		{
			object data = null;
			data = base.Run();
			if (data is DataTable)
			{
				_data = data as DataTable;
				Import.ImportTable _import = new FWBS.OMS.Design.Import.ImportTable(fieldreplacer);
				if (this.ImportMethod == ImportMethod.NewOnly)
					_import.NewOnlyImport(From,_data,this.PrimaryKey,this.PrimaryKeyRequiresQuotes);
				else if (this.ImportMethod == ImportMethod.Overwrite)
					_import.ImportOver(From,_data,this.PrimaryKey,this.PrimaryKeyRequiresQuotes);

			}
			else
			{
				throw new FWBS.OMS.SearchEngine.SearchException(HelpIndexes.SearchNotCompatibleResultset);
			}
		}

		public virtual void Update()
		{
			if (Convert.ToString(_searchlisttb.Rows[0]["pkdCode"]) == "")
				throw new EnquiryException(HelpIndexes.DataListNoCodeSet);

			_searchlisttb.Rows[0]["pkdParameters"] = _xmlDParams.InnerXml;
			
			Session.CurrentSession.Connection.Update(_searchlisttb, updateablesql);
		}

		public virtual void UpdateData()
		{
			if (_data != null && this.UpdateSelect != "")
			{
				Session.CurrentSession.Connection.Update(_data,this.UpdateSelectCall);
			}
			else if (_data != null && Call.ToUpper().StartsWith("SELECT"))
			{
				Session.CurrentSession.Connection.Update(_data, Call);
			}

		}

		public virtual void UpdateData(string Select)
		{
			if (_data != null && Select.StartsWith("SELECT"))
			{
				Session.CurrentSession.Connection.Update(_data, Select);
			}
		}
		#endregion

		#region IDisposable Implementation

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
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
				//Destroy the underlying data source.
				if (_searchlisttb != null)
				{
					_searchlisttb.Dispose();
					_searchlisttb = null;
				}
			}
		}


		#endregion

		#region Static Methods

		/// <summary>
		/// Delete a Search List
		/// </summary>
		/// <param name="Code">Search List Code</param>
		/// <returns>True if Succesful</returns>
		public static bool Delete(string Code)
		{
			Session.CurrentSession.Connection.ExecuteSQL("delete from DBCodeLookup where cdcode = @Code and cdtype = 'PACKAGEDATA';delete from dbPackageData where pkdCode = @Code", new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
			return true;
		}

		/// <summary>
		/// Does the Code Exist
		/// </summary>
		/// <returns>Boolean.</returns>
		public static bool Exists(string Code)
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbPackageData where pkdCode = @Code", "EXISTS",  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
			if (dt.Rows.Count > 0)
				return true;
			else
				return false;
		}


		public static DataTable GetDataLists()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "PACKAGEDATA", paramlist);
			//Terminology parse each of the items.
			foreach (DataRow row in dt.Rows)
			{
				row["pkdDesc"] = Session.CurrentSession.Terminology.Parse(row["pkdDesc"].ToString(), true);
			}
			dt.AcceptChanges();
			
			return dt;
		}

		#endregion

		#region Protected Methods
		/// <summary>
		/// Retreives the Attribure
		/// </summary>
		/// <param name="Node">The Node to Work On</param>
		/// <param name="Name">The Name of The Attribute</param>
		/// <param name="Value">The Value</param>
		protected void WriteAttribute(XmlNode Node, string Name, string Value)
		{
			try{Node.SelectSingleNode("@" + Name).Value = Value;}
			catch{Node.Attributes.Append(CreateAttribute(Node, Name,Value));}
		}

		/// <summary>
		/// Create the Attribute
		/// </summary>
		/// <param name="Node">The Node to Add the Attribute to</param>
		/// <param name="Name">The Name of the Attribute</param>
		/// <param name="Value">The Value </param>
		/// <returns>The Attribute Just Created</returns>
		protected System.Xml.XmlAttribute CreateAttribute(XmlNode Node, string Name, string Value)
		{
			System.Xml.XmlAttribute n = Node.OwnerDocument.CreateAttribute(Name);
			n.Value = Value;
			return n;
		}

		/// <summary>
		/// Returns the Attribute
		/// </summary>
		/// <param name="Node">The Node to Work On</param>
		/// <param name="Name">The Name of the Attribute</param>
		/// <param name="Value">The Value if it should Return a Error</param>
		/// <returns>The Contents of the Attribute</returns>
		protected string GetAttribute(XmlNode Node, string Name, string Value)
		{
			try
			{
				return Node.SelectSingleNode("@" + Name).Value;
			}
			catch
			{
				WriteAttribute(Node,Name,Value);
				return Value;
			}

		}

		/// <summary>
		/// A Protected Method to allow reuse of multi contructors
		/// </summary>
		protected internal virtual void DataListEditorInt()
		{
			_dr = _searchlisttb.Rows[0];
			_description = Convert.ToString(_dr["pkdDesc"]);
			_orgcode = Code;

			//
			// XML Parameters
			//
			_xmlDParams = new XmlDocument();
			_xmlDParams.PreserveWhitespace = false;
			_xmlDParams.LoadXml(Convert.ToString(_dr["pkdParameters"]));
			_xmlParameter = _xmlDParams.DocumentElement.SelectSingleNode("/params");

			base.Parameters = _xmlDParams.InnerXml;
			base.Call =  Convert.ToString(_dr["pkdCall"]);
			base.Src = Convert.ToString(_dr["pkdSource"]);
			base.SourceType = (SourceType)Enum.Parse(typeof(SourceType),Convert.ToString(_dr["pkdSourceType"]),true);
			base.ReBind();
		}

		#endregion

		#region Properties
		/// <summary>
		/// The Short name of the Data List
		/// </summary>
		[LocCategory("Data")]
		[Description("The Code that defines the individual Data List")]
		public string Code
		{
			get
			{
				return Convert.ToString(_dr["pkdCode"]);
			}
			set
			{
				if (DataLists.Exists(value))
				{
					if (value != _orgcode)
						throw new ExtendedDataException(HelpIndexes.ExtendedDataCodeAlreadyExists,value);
				}
				else
				{
					_dr["pkdCode"] = value;
					Description = _description;
				}
			}
		}

		/// <summary>
		/// The Localized Description of the Data List
		/// </summary>
		[LocCategory("Data")]
		[Description("The Description of the Data List")]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
				if (Code != "")
					FWBS.OMS.CodeLookup.Create("PACKAGEDATA",Code,value,"",CodeLookup.DefaultCulture,true,true,true);
			}
		}

		[LocCategory("PrimaryKey")]
		public string PrimaryKey
		{
			get
			{
				return GetAttribute(_xmlParameter,"PrimaryKey","");
			}
			set
			{
				WriteAttribute(_xmlParameter,"PrimaryKey",value);
			}
		}

		[LocCategory("PrimaryKey")]
		[Lookup("PRIMARYKEYQTE")]
		public bool PrimaryKeyRequiresQuotes
		{
			get
			{
				return FWBS.Common.ConvertDef.ToBoolean(GetAttribute(_xmlParameter,"PrimaryKeyRequiresQuotes","false"),false);
			}
			set
			{
				WriteAttribute(_xmlParameter,"PrimaryKeyRequiresQuotes",value.ToString());
			}
		}

		[LocCategory("Method")]
		public ImportMethod ImportMethod
		{
			get
			{
				return (ImportMethod)FWBS.Common.ConvertDef.ToEnum(GetAttribute(_xmlParameter,"ImportMethod", "NewOnly"),ImportMethod.NewOnly);
			}
			set
			{
				WriteAttribute(_xmlParameter,"ImportMethod",value.ToString());
			}
		}

		public string UpdateSelect
		{
			get
			{
				return Convert.ToString(_dr["pkdUpdateSelect"]);
			}
		}

		[Browsable(false)]
		public string UpdateSelectCall
		{
			get
			{
				FWBS.Common.ConfigSetting _data = new FWBS.Common.ConfigSetting(_dr,"pkdUpdateSelect");
				return _data.GetString("Call",base.Call);
			}
		}
		
		#endregion

	}

}
