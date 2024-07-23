
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using Document = OMSDocument;

    [System.Runtime.InteropServices.Guid("C764CEDF-2042-4C1E-9400-F6CE8D6A4B7B")] 
	public class LaserOMS : OMSApp
	{
		#region Field Variables

		private const int WM_SYSCOMMAND = 0x112;
		
		/// <summary>
		/// Laserform Application
		/// </summary>
		private LFMApplication _lfmApp = null;

		/// <summary>
		/// Laserform Document 
		/// </summary>
		private LFMDocument _lfmDoc = null;

		private Common.KeyValueCollection _vals = new Common.KeyValueCollection();
		/// <summary>
		/// Used to store a collection of OyezOMSForms to cope with multiple documents
		/// </summary>
		private Common.KeyValueCollection _opendocs = new Common.KeyValueCollection();
		/// <summary>
		/// tracks the currently opened document and key pair
		/// </summary>
		private Common.KeyValueItem _currentKVitem;
		/// <summary>
		/// Dataset of field aliases
		/// </summary>
		private System.Data.DataSet _fields = new System.Data.DataSet();
		/// <summary>
		/// Search form used to setup alias fields when in edit mode
		/// </summary>
		frmSearch _frmSrch;
		/// <summary>
		/// Transparent form used as overlay when field pickers are being displayed
		/// </summary>
		frmSpoof _frmspoof;
		/// <summary>
		/// Indicates whether we need to temporary switch DPI context for Save wizard creation
		/// in order to avoid issues on Windows 10 (1703+).
		/// </summary>
		private readonly DPIAwareness.DPI_AWARENESS _dpiAwareness = DPIAwareness.DPI_AWARENESS.UNAWARE;

		private string _currentCaption;

		#endregion

        #region Private properties to try and wrapper COM errors

        private LFMApplication LFMApp
        {
            get 
            {
                Application.DoEvents();
                return _lfmApp;
            }
            set { _lfmApp = value; 
            }
        }


        #endregion

        #region Constructors

        public LaserOMS() : base()
		{
			if (AppDomain.CurrentDomain.GetData("DPI_AWARENESS") != null)
			{
				_dpiAwareness = (DPIAwareness.DPI_AWARENESS)AppDomain.CurrentDomain.GetData("DPI_AWARENESS");
			}
		}
		
		~LaserOMS()
		{
		}
		
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion
	

		#region Save Methods


        protected override void InternalDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version)
        {
            if (Session.CurrentSession.IsLicensedFor("DM"))
            {

                CheckObjectIsDoc(ref obj);

                //Capture the current key as it will change after a save

                string newkey;
                string key = _currentKVitem.Key;

                MaximizeWindow(key);

                IStorageItem item = version;
                if (item == null)
                    item = doc;

                StorageProvider provider = doc.GetStorageProvider();

                System.IO.FileInfo file = FWBS.OMS.Global.GetTempFile(GetDocExtension(obj));

                string newFileName = ActiveForm.Save(file.FullName);

                System.IO.FileInfo newfile = new System.IO.FileInfo(newFileName);

                //grab caption now that is is saved
                newkey = GetWindowCaption();

                //OMS save to BLOB
                provider.Store(item, newfile, obj, true);

                if (doc.ContinueAfterSave)
                    return;

                //update the key values to the new values in the collections
                for (int i = 0; i < _opendocs.Count; i++)
                {
                    if (_opendocs[i].Key == key)
                    {
                        if (_currentKVitem.Key != newkey)
                        {
                            _currentKVitem.Key = newkey;
                            _opendocs.Remove(key);
                            _opendocs.Add(_currentKVitem); //add a return to this
                            return;
                        }
                    }
                }
            }

        }
        

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec)
        {
            InternalPrecedentSave(obj, saveMode, printMode, prec, null);
        }
        
        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec, PrecedentVersion version)
        {
            if (Session.CurrentSession.IsLicensedFor("DP"))
            {

                CheckObjectIsDoc(ref obj);

                //Capture the current key as it will change after a save

                string newkey;
                string key = _currentKVitem.Key;

                MaximizeWindow(key);

                IStorageItem item = version;
                if (item == null)
                    item = prec;
                                
                StorageProvider provider = prec.GetStorageProvider();

                //if it's a blob storage providor saves it to database but needs a temp file first to save it from.

                System.IO.FileInfo file = FWBS.OMS.Global.GetTempFile("lpd");

                //Save Laserform
                ActiveForm.Save(file.FullName);

                //grab caption now that is is saved
                newkey = GetWindowCaption();

                //OMS save to BLOB
                provider.Store(item, file, obj, true);


                //update the key values to the new values in the collections
                for (int i = 0; i < _opendocs.Count; i++)
                {
                    if (_opendocs[i].Key == key)
                    {
                        if (_currentKVitem.Key != newkey)
                        {
                            _currentKVitem.Key = newkey;
                            _opendocs.Remove(key);
                            _opendocs.Add(_currentKVitem); //add a return to this
                            return;
                        }
                    }
                }
            }
        }
	
		#endregion

		#region Print Methods

		public override void InternalPrint(object obj, int copies)
		{
            LFMDocument lfmdoc = (LFMDocument)obj;

			for (int ctr = 1; ctr <= copies; ctr++)
			{
                lfmdoc.Print();
            }
		}

		
		#endregion

		#region Field Routines

		public override void AddField(object obj, string name)
		{
			return;
		}
		
		
		public override int GetFieldCount(object obj)
		{
			CheckObjectIsDoc(ref obj);
			string formName = GetActiveFormName();
			return _fields.Tables[formName].DefaultView.Count;
		}
		
		
		public override string GetFieldName(object obj, int index)
		{
			//to handle constants
			CheckObjectIsDoc(ref obj);
			string name = Convert.ToString(_fields.Tables[GetActiveFormName()].DefaultView[index]["fldAlias"]);
			
			if (name == "") 
				name = "#CONST#" + Convert.ToString(_fields.Tables[GetActiveFormName()].DefaultView[index]["fldConstant"]); 
				
			return name;
		}

		public override object GetFieldValue(object obj, int index)
		{
			CheckObjectIsDoc(ref obj);
			return GetDocVariable(obj, GetFieldName(obj, index));
		}

		public override void CheckFields(object obj)
		{
			
		}

		public override void SetFieldValue(object obj, int index, object val)
		{
			CheckObjectIsDoc(ref obj);
			string name = Convert.ToString(_fields.Tables[GetActiveFormName()].DefaultView[index]["fldName"]);

            ActiveForm.SetField(name, val);
			SetDocVariable(obj, GetFieldName(obj, index), val);
        }

		public override object GetFieldValue(object obj, string name)
		{
			return GetDocVariable(obj, name);
		}

		//to handle constants and changes to OMSapp that now calls this method passing name not index
		public override void SetFieldValue(object obj, string name, object val)
		{
            string fieldName;
            string fieldValue = "";

            name = name.Replace("#CONST#", "").Replace("'", "''");

            //need to loop around all the fields with the same name and check the page number
            //setting all the fileds with the same name based on the page
            DataView vw = _fields.Tables[GetActiveFormName()].DefaultView;

            vw.RowFilter = "fldAlias='" + name + "' or fldConstant='" + name + "'";

            //Now process all the fields
            for (int i = 0; i < vw.Count; i++)
            {
                fieldName = Convert.ToString(vw[i]["fldName"]);
                
                //needed to modify this now to look for constants
                if (Convert.ToString(val).Contains("#CONST#"))
                    fieldValue = Convert.ToString(vw[i]["fldConstant"]);
                else
                    fieldValue = Convert.ToString(val);
                
                ActiveForm.SetField(fieldName, fieldValue);
            }

			SetDocVariable(obj, name, val);

            //try setting the views filter back to blank
            vw.RowFilter = "";

		}
					


		#endregion

		#region Document Variable Routines
		

		/// <summary>
		/// Gets the editing time that has been spent on a document.
		/// </summary>
		/// <param name="obj">The current document to query.</param>
		/// <returns>The time in minutes.</returns>
		public override int GetDocEditingTime(object obj)
		{
			//return 0;
			DateTime dtstart = (DateTime)GetDocVariable(ActiveForm, "DOCSTARTEDIT");
			DateTime dtend = DateTime.Now;
			TimeSpan ts = dtend.Subtract(dtstart);
			int retval = Convert.ToInt32(FWBS.Common.Math.RoundUp(Convert.ToDecimal(ts.TotalMinutes)));
			return retval;
		}
		
		
		
		
		protected override object GetDocVariable(object obj, string varName)
		{
			CheckObjectIsDoc(ref obj);
			if (HasDocVariable(obj, varName))
			{
				LaserOMSForm form = (LaserOMSForm)_currentKVitem.Value;
				FWBS.Common.KeyValueCollection vals = form.DocumentVariables;
				return vals[varName].Value;
			}
			return null;
		}

		
		public override bool HasDocVariable(object obj, string varName)
		{
			try
			{
				CheckObjectIsDoc(ref obj);
				LaserOMSForm form = (LaserOMSForm)_currentKVitem.Value;
				FWBS.Common.KeyValueCollection vals = form.DocumentVariables;
				return vals.Contains(varName) ;
			}
			catch
			{
				return false;
			}
		}

		
		public override void RemoveDocVariable(object obj, string varName)
		{
			try
			{
                //Don't remove base PrecID as causes it to look for default which doesn't exist
                if (varName.ToUpper() == "BASEPRECID")
                    return;
                
				LaserOMSForm form = (LaserOMSForm)_currentKVitem.Value;
				FWBS.Common.KeyValueCollection vals = form.DocumentVariables;
				vals.Remove(varName);
			}
			catch
			{
			}
			
		}

		
		protected override bool SetDocVariable(object obj, string varName, object val)
		{
			try
			{
				CheckObjectIsDoc(ref obj);
				LaserOMSForm form = (LaserOMSForm)_currentKVitem.Value;
				FWBS.Common.KeyValueCollection vals = form.DocumentVariables;
				vals.Add(varName,val);
				return true;
			}
			catch
			{
				return false;
			}
		}


		#endregion

		#region IOMSApp Implementation

        protected override FetchResults InternalFetchItem(IStorageItem item, OpenSettings settings, FWBS.Common.TriState force)
        {
            if (item is Precedent)
            {
                StorageProvider provider = item.GetStorageProvider();
                StorageSettingsCollection storesettings = provider.GetDefaultSettings(item, SettingsType.Fetch);
                FetchResults res = provider.Fetch(item, false, storesettings, force);

                //system now expects a fetch results and cannot be null
                if (res == null)
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(item.Name);
                    res = new FetchResults(item, file);
                }
                return res;

            }
            else
                return base.InternalFetchItem(item, settings, force);
        }

		public override string ExtractPreview(object obj)
		{
			return "";
		}

		
		/// <summary>
		/// closes laserforms
		/// </summary>
		/// <param name="obj"></param>
		public override void Close(object obj)
		{
			CloseLaser();
		}
		
		

  		protected override void ScreenRefresh()
		{
			try
			{
				_frmspoof.Close();
				_frmspoof.Dispose();
			}
			catch{}
			//## try a min/max to display filled in fields 
			FWBS.Common.Functions.MinimizeWindow(ActiveWindow.Handle);
			FWBS.Common.Functions.MaximizeWindow(ActiveWindow.Handle);

		}

		
		public override string DefaultDocType
		{
			get
			{
				return "FORM";
			}
		}

		
		public override string GetDocExtension(object obj)
		{
			if(LFMApp.VersionAsInt < 881408)
				return "lpd"; //old sytle fixed forms
			else
				return "xfd"; //new forms displayed in activex
		}

		
		public override string ModuleName
		{
			get
			{
				return "Laserforms Integration";
			}
		}

		
		public override IWin32Window ActiveWindow
		{
			get
			{
				return new Window(LFMApp);
			}
		}

        protected override object InternalDocumentOpen(Document document, FetchResults fetchData, OpenSettings settings)
        {
            string windowcaption = "";
            ActivateApplication();
            
            System.IO.FileInfo file = fetchData.LocalFile;

            //close document if it exists
            if (LFMApp.HasActiveDocument())
            {
                LFMDocument doc = (LFMDocument)LFMApp.ActiveDocument;
                doc.Close();
            }
            LFMDocuments docs = LFMApp.Documents;

            switch (settings.Mode)
            {
                case DocOpenMode.Edit:
                    ActiveForm = docs.OpenDocument(file.FullName);

                    if (CheckLaserError())
                    {
                        CloseLaser();
                        return null;
                    }

                    //need to maximise this window as I rely on the top caption
                    windowcaption = GetWindowCaption();
                    MaximizeWindow(windowcaption);

                    if(Session.CurrentSession.IsLicensedFor("DM"))
                    {
                        IWin32Window activeWindow = ActiveWindow;
                        //Need to temporary switch DPI awareness context to match Laser Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
                        bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
                        using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
                        {
                            //Create a form object with caption as key and set current item
                            AddDocumentVariables(windowcaption);

                            RemoveMenuItems();
                            ShowSaveDialog(activeWindow, windowcaption);
                        }
                        break;
                    }
                    else
                        return new object();

                case DocOpenMode.Print:
                    ActiveForm = docs.OpenDocument(file.FullName);

                    if (CheckLaserError())
                    {
                        CloseLaser();
                        return null;
                    }

                    object[] objDocs = new object[1];
                    objDocs[0] = ActiveForm;
                    BeginPrint(objDocs, settings.Printing);
                    break;

                case DocOpenMode.View:
                    ActiveForm = docs.OpenDocument(file.FullName);

                    if (CheckLaserError())
                    {
                        CloseLaser();
                        return null;
                    }
                    windowcaption = GetWindowCaption();
                    MaximizeWindow(windowcaption);

                    if (Session.CurrentSession.IsLicensedFor("DM"))
                    {
                        IWin32Window activeWindow = ActiveWindow;
                        //Need to temporary switch DPI awareness context to match Laser Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
                        bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
                        using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
                        {
                            RemoveMenuItems();
                            ShowSaveDialog(activeWindow, windowcaption);
                        }
                        break;
                    }
                    else
                        return new object();
            }
            
            //Added to support save as
            if (ActiveForm != null)
            {
                ActiveForm.PrecName = document.BasePrecedent.Title;

                if(Session.OMS.IsLicensedFor("DP"))
                    GetDocumentFields(GetActiveFormName());
                
            }


            return ActiveForm;

        }


        protected override object InternalPrecedentOpen(Precedent precedent, FetchResults fetchData, OpenSettings settings)
        {

            if (Session.CurrentSession.IsLicensedFor("DP"))
            {
                ActivateApplication();
            }
            else
                return null;

            string title = Convert.ToString(precedent.GetExtraInfo("precpath"));
            string windowcaption;

            bool hasDoc = LFMApp.HasActiveDocument();

            if (hasDoc)
            {
               LFMApp.ActiveDocument.Close();
            }


            switch (settings.Mode)
            {
                case DocOpenMode.Edit:
                    

                        // Open up a form to add the field parser field to the equvalent mapped oyes form field id's.
                        ActiveForm = LFMApp.Documents.Add(title);

                        if (CheckLaserError())
                        {
                            CloseLaser();
                            return null;
                        }

                        IWin32Window activeWindow = ActiveWindow;
                        //Need to temporary switch DPI awareness context to match Laser Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
                        bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
                        using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
                        {
                            _frmSrch = new frmSearch(title);

                            FWBS.Common.Functions.SetParentWindow(activeWindow, _frmSrch);

                            RemoveMenuItems();

                            //need to maximise this window as I rely on the top caption
                            windowcaption = GetWindowCaption();
                            MaximizeWindow(windowcaption);

                            _frmSrch.FormCaption = windowcaption;

                            AddDocumentVariables(windowcaption);

                            _frmSrch.Show();

                            _frmSrch.ActiveApplication = LFMApp;
                            _frmSrch.ActiveDoc = ActiveForm;

                            _frmSrch.OnClose += new EventHandler(_frmSrch_OnClose);
                        }

                    break;

                case DocOpenMode.Print:
                        ActiveForm = LFMApp.Documents.Add(title);

                        if (CheckLaserError())
                        {
                            CloseLaser();
                            return null;
                        }

                        object[] docs = new object[1];
                        docs[0] = ActiveForm;

                        LFMApp.WindowState = 1;

                        //check if they want to print the field codes
                        DialogResult result = MessageBox.ShowYesNoQuestion("OYEZFLDCODES", "Do you want to print the field codes.", false);
                        //populate doc with the field codes

                        if (result == DialogResult.Yes)
                        {
                            ActiveForm.ShowFieldsCodes();
                        }

                        BeginPrint(docs, settings.Printing);

                    break;

                case DocOpenMode.View:
                    ActiveForm = LFMApp.Documents.Add(title);

                    if (CheckLaserError())
                    {
                        CloseLaser();
                        return null;
                    }

                    if (Session.CurrentSession.IsLicensedFor("DM"))
                    {
                        //remove previous list so they cannopen old forms via menu
                        RemovePreviousDocList();
                    }

                    ActiveForm.Dispose();
                    LFMApp.Dispose();
                    
                    //try return an object to keep OMS App happy but also allow me to get rid off reference and 
                    return new object();
                    
            }

            if (ActiveForm != null)
            {
                ActiveForm.PrecName = title;
                return ActiveForm;
            }
            else
                return null;
        }


        protected override DocSaveStatus BeginSave(object obj, SaveSettings settings)
        {
            using (var contextBlock = _dpiAwareness > 0 ? new DPIContextBlock(_dpiAwareness) : null)
            {
                //load docs key value pairing
                LoadDocumentVariables(_currentCaption);

                return base.BeginSave(obj, settings);
            }
		}

        protected override object TemplateStart(object obj, PrecedentLink preclink)
        {
            //if you are not licesed for Doc production don't do anything
            if (!Session.CurrentSession.IsLicensedFor("DP"))
                return new object();


            System.IO.FileInfo tmppath = null;

            //Get the Precedent File to Load...
            try
            {
                tmppath = preclink.Merge().LocalFile;
            }
            catch { }

            string title = Convert.ToString(preclink.Precedent.GetExtraInfo("precpath"));
            ActiveForm = LFMApp.Documents.Add(title);
            ActiveForm.PrecName = title;

            //need to maximise this window as I rely on the top caption
            string windowcaption = "[" + title;
            MaximizeWindow(windowcaption);
            IWin32Window activeWindow = ActiveWindow;
            //Need to temporary switch DPI awareness context to match Laser Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
            bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
            using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
            {
                if (Session.CurrentSession.IsLicensedFor("DP"))
                {
                    //Create a form object with caption as key and set current item
                    AddDocumentVariables(windowcaption);

                    //Check if laser forms has blown up
                    if (CheckLaserError())
                    {
                        CloseLaser();
                        return null;
                    }

                    //Added to support save as so move this code into seperate function
                    GetDocumentFields(GetActiveFormName());

                    // Sort out the constants
                    SetupConstants(GetActiveFormName(), preclink);
                }
                if (Session.OMS.IsLicensedFor("DM"))
                {
                    //lock down the toolbars
                    RemoveMenuItems();

                    //display a save dialog
                    ShowSaveDialog(activeWindow, windowcaption);

                    //overlay transparent form to prevent them from closing before merge has finished
                    _frmspoof = new frmSpoof();
                    FWBS.Common.Functions.SetParentWindow(activeWindow, _frmspoof);
                    _frmspoof.Show();
                }
            }
            return ActiveForm;
        }



        private void CreateApplication()
        {
           
        }

		public override void ActivateApplication()
		{
            if (Session.CurrentSession.IsLicensedFor("DM") || Session.CurrentSession.IsLicensedFor("DP"))
            {
                System.Windows.Forms.Application.DoEvents();

                if (LFMApp == null)
                    LFMApp = new LFMApplication();
                else
                    LFMApp.Activate();
            }
        }
		
		protected override void CheckObjectIsDoc(ref object obj)
		{
			if (obj == this)
				obj = ActiveForm;
			else if(obj is LFMDocument)
				ActiveForm = (LFMDocument)obj;
			else
				throw new OMSException2("LASERACCEPTNO","The passed parameter is not an acceptable laser form object.");
			obj = ActiveForm;
		}

		protected override void InsertText(object obj, PrecedentLink precLink)
		{
		}


		protected override void SetWindowCaption(object obj, string caption)
		{
            return;
		}


		/// <summary>
		/// when job finishes processing 
		/// </summary>
		protected override void OnJobProcessed()
		{
            try
            {
                _frmspoof.Close();
                _frmspoof.Dispose();
            }
            catch { }

            if (!Session.CurrentSession.IsLicensedFor("DM"))
            {
                LFMApp.Dispose();
                LFMApp = null;
            }

			base.OnJobProcessed();
		}

		
		#endregion

		#region Methods

        /// <summary>
        /// Sets up the constants as we need some different processing
        /// </summary>
        /// <param name="title"></param>
        /// <param name="preclink"></param>
        private void SetupConstants(string title, PrecedentLink preclink)
        {
            DataTable dt = _fields.Tables[title];
            if (dt == null)
                return;

            System.Data.DataView constants = null; //Modified to handle constants
            constants = new DataView(dt);
            constants.RowFilter = "len(fldconstant) > 0 and len(fldalias) = 0";
            
            Common.KeyValueCollection pars2 = new FWBS.Common.KeyValueCollection();
            foreach (DataRowView r in constants)
            {
                //append the worconst in fromnt of the field value to prevent clashes with OMS constants
                string fieldName = "#CONST#" + Convert.ToString(r["fldConstant"]);
                if (pars2.Contains(fieldName) == false)
                    pars2.Add(fieldName, r["fldConstant"]);
            }

            //add my constants first so parser looks at these first before evaluating OMS standard fields
            preclink.Parser.ChangeParameters(pars2);

        }


        /// <summary>
        /// Populates the document fields
        /// </summary>
        /// <param name="title"></param>
        private void GetDocumentFields(string title)
        {
            if (_fields.Tables.Contains(title) == true)
                _fields.Tables.Remove(title);


            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            pars.Add("FORM_NAME", title);


            SearchEngine.SearchList sch = new SearchEngine.SearchList(FWBS.OMS.Session.OMS.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.LaserAliases), null, pars);
            System.Data.DataTable dt = sch.Run() as System.Data.DataTable;
            
            if (dt != null)
            {
                dt.TableName = title;
                _fields.Tables.Add(dt);
                dt.DefaultView.RowFilter = "len(fldAlias) > 0 or len(fldConstant) > 0";
                
            }
        }
        

		/// <summary>
		/// Adds the variables from this documents to the collection of documents varables
		/// </summary>
		private void AddDocumentVariables(string caption)
		{
			DateTime dt = DateTime.Now; 

			//create an instance of OYEZ form object
			LaserOMSForm form = new LaserOMSForm();
			
			form.LaserApplication = LFMApp;
			form.LaserForm = ActiveForm;
			form.DocumentVariables = new FWBS.Common.KeyValueCollection();
			
			_opendocs.Add(caption,form);
			
			//set the current item to this newly created one
			_currentKVitem = _opendocs[_opendocs.Count - 1];
			
			SetDocVariable(ActiveForm, "DOCSTARTEDIT", dt);
		}

		
		/// <summary>
		/// Loads the correct key value collection depending on form being shown
		/// </summary>
		private void LoadDocumentVariables(string caption)
		{
            LaserOMSForm clsfrm = null;

            if (_opendocs.Contains(caption))
                clsfrm = (LaserOMSForm)_opendocs[caption].Value;
            else
            {
                foreach (string item in _opendocs)
                {
                    if (item.Contains(caption))
                    {
                        caption = item;
                        clsfrm = (LaserOMSForm)_opendocs[item].Value;
                        break;
                    }
                }
            }

            if (clsfrm == null)
                throw new Exception("Form Not Found");
			
			_currentKVitem = _opendocs[caption];
            LFMApp = clsfrm.LaserApplication;
			ActiveForm = clsfrm.LaserForm;
			_currentCaption = caption;
			return;
		}


		/// <summary>
		/// displays and creates the save dialog for the last created 
		/// </summary>
		private void ShowSaveDialog(IWin32Window activeWindow, string caption)
		{
			//get class of object variables	based on last one loaded		
			LaserOMSForm frm = (LaserOMSForm)_opendocs[caption].Value;
			//create a new instance of the save dialog witin the class
			frm.SaveDialogForm = CreateSaveDialog();
			//set the inedex number of the form
			frm.SaveDialogForm.FormCaption = caption;
			//show from class
			frm.SaveDialogForm.Show();
			//set parent to current active window Namely last form loaded
			FWBS.Common.Functions.SetParentWindow(activeWindow, frm.SaveDialogForm);
			//form has its own method to position iteslf
			frm.SaveDialogForm.SetPosition(LFMApp.Version);
			System.Windows.Forms.Application.DoEvents();
		}
		

		/// <summary>
		/// creates the save dialog form and registers the save event
		/// </summary>
		private frmLaserDialog CreateSaveDialog()
		{
			frmLaserDialog frm = new frmLaserDialog();
			frm.OnSave +=new EventHandler(_frm_OnSave);
			frm.OnClose +=new EventHandler(_frm_OnClose);
            frm.OnSaveAs += new EventHandler(frm_OnSaveAs);
			frm.ShowInTaskbar = false;
			return frm;
		}


		
		
		/// <summary>
		/// Try and close Laserforms as cleanly as possible
		/// </summary>
		private void CloseLaser()
		{
            bool err = false;
            try
			{
				//get the name of the current item
				string formname = _currentKVitem.Key;

                //remove details from docs collection
                _opendocs.Remove(formname);

                ActiveForm.DoSave(false);

                ActiveForm.Close();
				
                LFMApp.Quit();

                ActiveForm.Dispose();
                LFMApp.Dispose();
				
				ActiveForm = null;
				LFMApp = null;
			}
			catch(Exception)
			{
                err = true;
            }
			//if no more open documents accourding to our tracking kill processes if they exist
			if(_opendocs.Count < 1 || err)
			{
				try
				{
                    CloseProcesses();
				}
				catch{}
			}
			
		}

        private static void CloseProcesses()
        {
            Process[] procs = Process.GetProcessesByName("lfm32");
                    
            foreach (Process proc in procs)
            {
                if (proc.ProcessName == "Lfm32")
                {
                    proc.CloseMainWindow();
                    proc.Close();
                }
            }
        }


        private bool ProcessExists(string procName)
        {
            Process[] procs = Process.GetProcessesByName("lfm32");

            if (procs.Length > 0)
                return true;
            else
                return false;

        }

		

		/// <summary>
		/// Gets the caption of the OYEZ application
		/// the only way to see what form is loaded
		/// </summary>
		/// <returns>string caption of the Oyez application</returns>
		private string GetWindowCaption()
		{
			return FWBS.Common.Functions.GetWindowCaption(ActiveWindow.Handle);
		}

		/// <summary>
		/// Maximises a window with passed caption
		/// </summary>
		/// <param name="windowcaption">caption or part of window caption</param>
		private void MaximizeWindow(string windowcaption)
		{
			IntPtr hwnd = Common.Functions.FindWindow("",windowcaption,true);
			Common.Functions.MaximizeWindow(hwnd);
        }
		
		
		/// <summary>
		/// Removes unwanted save and close options
		/// </summary>
		private void RemoveMenuItems()
		{
			IntPtr hWnd = ActiveWindow.Handle;
			
			FWBS.Common.Functions.DisableCloseMenu(hWnd);
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","E&xit");
			RemovePreviousDocList();
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Close");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","New");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Open");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Save File &As");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Save");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Sen&d To");
			//called twice because there are 2 items left after all the stripping
			FWBS.Common.Functions.RemoveLastMenuItem(hWnd,"File");
			FWBS.Common.Functions.RemoveLastMenuItem(hWnd,"File");
			
			//remove toolbars option so buttons do not look out of place
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"View","Toolbars");
		}
		
		
		/// <summary>
		/// Remove any items that appear in previously opened file list
		/// </summary>
		private void RemovePreviousDocList()
		{
			IntPtr hWnd = ActiveWindow.Handle;
			
			for( int i = 1;i < 10 ; i++)
			{
				FWBS.Common.Functions.RemoveMenuItem(hWnd,"File",i.ToString());
			}
		}

		
		/// <summary>
		/// Checks to see if Oyez has an error and displays it if so
		/// </summary>
		private bool CheckLaserError()
		{
			//Error don't appear to be raised so check if the document is null
			if(ActiveForm == null)
			{
				//display error notification 
				string message = FWBS.OMS.Session.OMS.Resources.GetResource("LASERNOTLOAD","Unable to load selected Laserform","").Text;
				MessageBox.Show(ActiveWindow,message);
				return true;
			}
			return false;
		}



		
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the active file.
		/// </summary>
		internal LFMDocument ActiveForm
		{
            get
            {
                Application.DoEvents();
                return _lfmDoc;
            }
            set { _lfmDoc = value; }
		}
		
		/// <summary>
		/// Gets the name of the currently active document
		/// </summary>
		/// <returns>name of form</returns>
		private string GetActiveFormName()
		{
            return ActiveForm.PrecName;
		}
		
		#endregion
		
		#region Event Handlers

		private void _doc_SaveCalled()
		{
			this.Save(ActiveForm);
		}
		
		private bool CheckSave()
		{
			_frmSrch = null;
			IWin32Window appwindow = new Window(LFMApp);

			DialogResult result = MessageBox.ShowYesNoCancel("LASERSAVEOMS","Do you wish to save document to OMS");

			if(result == DialogResult.Yes)
			{
				//if so save to OMS
				this.Save(ActiveForm);
			}
			else if(result == DialogResult.Cancel)
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Event raised from the field edit form
		/// </summary>
		private void _frmSrch_OnClose(object sender, EventArgs e)
		{
			_frmSrch.Close();
			CloseLaser();
		}

        void frm_OnSaveAs(object sender, EventArgs e)
        {
            //grab the sending form
            frmLaserDialog frm = (frmLaserDialog)sender;
            LoadDocumentVariables(frm.FormCaption);

            //call the save method
            this.SaveAs(ActiveForm,false);
        }

		private void _frm_OnSave(object sender, EventArgs e)
		{
			//grab the sending form
			frmLaserDialog frm = (frmLaserDialog)sender;
			LoadDocumentVariables(frm.FormCaption);
			
			//call the save method
			this.Save(ActiveForm);
		}

		private void _frm_OnClose(object sender, EventArgs e)
		{
			//grab the sending form
			frmLaserDialog frm = (frmLaserDialog)sender;
			//load document variabes pertaining to this form
			LoadDocumentVariables(frm.FormCaption);

			//check if they want to save first returns false if cancel is selected  
			if(CheckSave())
			{
				frm.Close();
				CloseLaser();
			}
		}
		#endregion
	}
	

	//class used to hold variables for each Laser form loaded
	internal class LaserOMSForm
	{
		public LFMApplication LaserApplication;
		public LFMDocument LaserForm;
		public Common.KeyValueCollection DocumentVariables;
		public frmLaserDialog SaveDialogForm;
	}

	
	internal class Window : IWin32Window
	{
		private IntPtr _handle;
		
		// still pass obj although it is not used.  
		// May find a better way to find the window other than by name
		public Window(object obj)
		{
			string caption = "Laserform";
			string className  = "";
			
			try
			{
				_handle = Common.Functions.FindWindow(className, caption,true);
				
			}
			catch{}
		}

		public IntPtr Handle 
		{
			get
			{
				return _handle;
			}
		}
    }


    #region Laserforms Abstraction

    internal sealed class LFMDocuments : LFMBase
    {
        internal LFMDocuments(object comobj, int appVerNumber)
            : base(comobj, appVerNumber)
        { }
    

        protected override string ProgId
        {
            get { return "LFM32.Documents"; }
        }


        public LFMDocument Add(string title)
        {
            object doc = Invoke("Add", BindingFlags.InvokeMethod,title);

            if (doc == null)
                
                throw new InvalidOperationException(String.Format("Unable to load Laserform '{0}'.", title));

            return new LFMDocument(doc, appVersion);
        }

        public LFMDocument OpenDocument(string path)
        {
            return new LFMDocument(Invoke("OpenDocument", BindingFlags.InvokeMethod, path),base.appVersion);
        }
    }

    internal sealed class LFMDocument : LFMBase
    {
        private Type tpage;
        private Type tInfill;
        private string precName;

        internal LFMDocument(object comobj, int appVerNumber)
            : base(comobj, appVerNumber)
        {
            
            this.tpage = Type.GetTypeFromProgID("LFM32.Page");
            this.tInfill = Type.GetTypeFromProgID("LFM32.Infill");
        }

        protected override string ProgId
        {
            get { return "LFM32.Document"; }
        }

        /// <summary>
        /// Used to track the precedent name or this instance
        /// </summary>
        public string PrecName
        {
            get { return precName; }
            set { precName = value; }
        }
                
        public string Name
        {
            get
            {
                return (string)Invoke("FormName", BindingFlags.GetProperty);
            }
        }

        public void Print()
        {

            //well looks like something has changed again 
            //Of course this may be form type specific in which case only the later forms can
            //be used with version 8.8.1408 onwards
            bool duplex = true;
            object dupObj = (object)duplex;

            if (base.appVersion < 881408)
                Invoke("PrintForm2", BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn, null, null, true, null);
            else
                Invoke("PrintForm3", BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn, null, null, true, null);
        }

        public void Close()
        {
            Invoke("CloseDocument", BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn);
        }

        /// <summary>
        /// This must return the saved filename as a file extension may or may not get appended
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string Save(string filename)
        {
            //need the returned file name
            object[] args = new object[1];
            args[0] = filename;

            //parameter modifier array for by ref parameters
            ParameterModifier parMod = new ParameterModifier(1);
            parMod[0] = true;
            ParameterModifier[] parMods = new ParameterModifier[1] { parMod };
                        
            comtype.InvokeMember("Save", BindingFlags.InvokeMethod, null, comobj, args,parMods,null,null);

            filename = Convert.ToString(args[0]);
            return filename;
        }

        public IEnumerator Pages
        {
            get
            {
                return ((IEnumerable)Invoke("Pages", BindingFlags.GetProperty)).GetEnumerator();
            }
        }
        
        public void ShowFieldsCodes()
        {
            IEnumerator pages = Pages;
            while (pages.MoveNext())
            {
                object page = pages.Current;

                object objInFills = tpage.InvokeMember("Infills", BindingFlags.GetProperty, null, page, null);
                
                IEnumerator enumInfills = ((IEnumerable)objInFills).GetEnumerator();
                while (enumInfills.MoveNext())
                {
                    object infill = enumInfills.Current;
                    string infillName = Convert.ToString(tInfill.InvokeMember("InfillName", BindingFlags.GetProperty, null, infill, null));
                    SetInfill(infill, infillName);
                }
            }
        }
        

        /// <summary>
        /// Sets the field (InFill) value
        /// </summary>
        /// <param name="infill"></param>
        /// <param name="data"></param>
        private void SetInfill(object infill, string data)
        {
            tInfill.InvokeMember("Data", BindingFlags.SetProperty, null, infill, new object[] { data });
        }


        /// <summary>
        /// Opens the specified Documemnt
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LFMDocument OpenDocument(string path)
        {
            return new LFMDocument(Invoke("OpenDocument", BindingFlags.InvokeMethod, path), base.appVersion);
        }

        /// <summary>
        /// Returns an Array of all the fild names in the Form
        /// </summary>
        public string[] Fields
        {
            get
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                IEnumerator pages = Pages;
                while (pages.MoveNext())
                {
                    object page = pages.Current;

                    object objInFills = tpage.InvokeMember("Infills", BindingFlags.GetProperty, null, page, null);
                    IEnumerator enumInfills = ((IEnumerable)objInFills).GetEnumerator();
                    while (enumInfills.MoveNext())
                    {
                        object infill = enumInfills.Current;
                        list.Add(Convert.ToString(tInfill.InvokeMember("InfillName", BindingFlags.GetProperty, null, infill, null)));
                    }
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// Sets a field value
        /// </summary>
        /// <param name="name">Name of the field</param>
        /// <param name="val">Value to set</param>
        public void SetField(string name, object val)
        {
            object firstPage = null;
            IEnumerator pages = Pages;
            while (pages.MoveNext())
            {
                if (firstPage == null)
                    firstPage = pages.Current;

                object page = pages.Current;

                object objInFills = tpage.InvokeMember("Infills", BindingFlags.GetProperty, null, page, null);
                IEnumerator enumInfills = ((IEnumerable)objInFills).GetEnumerator();
                while (enumInfills.MoveNext())
                {
                    object infill = enumInfills.Current;
                    string infillName = Convert.ToString(tInfill.InvokeMember("InfillName", BindingFlags.GetProperty, null, infill, null));

                    if (infillName == name)
                        SetInfill(infill, Convert.ToString(val));
                }
            }
			
            if (firstPage != null)
                Invoke("ActivePage", BindingFlags.SetProperty, firstPage);
            
        }

        /// <summary>
        /// Property used to inform the docuemtn to save or not used when closing the app
        /// </summary>
        /// <param name="val"></param>
        public void DoSave(bool val)
        {
            Invoke("DoSave", BindingFlags.SetProperty, val);
        }
    }


    internal sealed class LFMApplication : LFMBase
    {
        /// <summary>
        /// standard contrutor
        /// </summary>
        public LFMApplication()
        {
            Initialise();
        }

        /// <summary>
        /// Called on each activate application to check if instance is still in memory
        /// </summary>
        internal void Activate()
        {
            Initialise();
        }

        /// <summary>
        /// Standard method called from constructor and activate method
        /// </summary>
        private void Initialise()
        {
            if (comobj == null)
            {
                comobj = Activator.CreateInstance(comtype);
            }
            

            //now perform a simple operation to make sur the instance is valid 
            try
            {
                //now query the visible property which may trigger a COM exception if there is a problem
                if (!Visible)
                {
                    Visible = true;
                    WindowState = 2;
                    CloseOnExit = true;

                }
            }
            catch (COMException)
            {
                //If we get an error construct again
                Dispose();
                comobj = Activator.CreateInstance(comtype);
                Visible = true;
                WindowState = 2;
                CloseOnExit = true;
            }
            
            Debug.WriteLine(String.Format("Created Instance"));

        }
        
        /// <summary>
        /// Prog ID of the laserforms Application
        /// </summary>
        protected override string ProgId
        {
            get { return "LFM32.Application"; }
        }

        public string Version
        {
            get
            {
                return (string)Invoke("Version", BindingFlags.GetProperty);
            }
        }

        /// <summary>
        /// Used to check for version
        /// </summary>
        public int VersionAsInt
        {
            get
            {
                if (base.appVersion == 0)
                {
                    string ver = Version;

                    string[] numbers = Convert.ToString(ver).Split(new char[1] { '.' });

                    //newer versions of LFM have 4 point numbers but we won't be interested in the revision for now
                    if (numbers.Length >= 3)
                    {
                        int major = Convert.ToInt32(numbers[0]) * 100000;
                        int minor = Convert.ToInt32(numbers[1]) * 10000;
                        int revision = Convert.ToInt32(numbers[2]);
                        appVersion = major + minor + revision;
                    }
                    else
                        appVersion = 1000000; //if not 3 elements then for now return a million lets not hope they change their versioning 
                }
                return appVersion;
            }
        }

        public int WindowState
        {
            get
            {
                return (int)Invoke("WindowState", BindingFlags.GetProperty);
            }
            set
            {
                Invoke("WindowState", BindingFlags.SetProperty, value);
            }
        }

        public bool Visible
        {
            get
            {
                return (bool)Invoke("Visible", BindingFlags.GetProperty);
            }
            set
            {
                Invoke("Visible", BindingFlags.SetProperty, value);
            }
        }

        public bool CloseOnExit
        {
            set
            {
                //property does not appear to exist in earlier versions, crash seen in 9.21
                int version = VersionAsInt;
                
                if (version > 940000)
                    Invoke("CloseOnExit", BindingFlags.SetProperty, value);
            }
        }

        public bool CloseQuietly
        {
            set
            {
                Invoke("CloseQuietly", BindingFlags.SetProperty, value);
            }

        }


        public bool HasActiveDocument()
        {
            return (bool)Invoke("HasActiveDocument", BindingFlags.InvokeMethod);
        }

        public void Quit()
        {
            Invoke("Quit", BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn);
        }

        public LFMDocument ActiveDocument
        {
            get
            {
                return new LFMDocument(Invoke("ActiveDocument", BindingFlags.GetProperty),VersionAsInt);
            }
        }
        

        public LFMDocuments Documents
        {
            get
            {
                return new LFMDocuments(Invoke("Documents", BindingFlags.GetProperty),VersionAsInt);
            }
        }
    }


    /// <summary>
    /// Base Class for all Laserforms Abstraction Classes
    /// </summary>
    internal abstract class LFMBase : IDisposable
    {
        internal object comobj;
        internal Type comtype;
        internal int appVersion = 0; //unspecified yet when 0

        protected LFMBase()
        {
            comtype = Type.GetTypeFromProgID(ProgId);

            if (comtype == null)
                throw new OMSException2("NOLASER", "Laserform is not Installed!");
        }

        /// <summary>
        /// Alternate Constructor
        /// </summary>
        /// <param name="comobj"></param>
        protected LFMBase(object comobj,int appVerNumber)
            : this()
        {
            if (comobj == null)
                throw new ArgumentNullException("comobj");

            this.comobj = comobj;
            this.appVersion = appVerNumber;
        }

        /// <summary>
        /// Used to store the COM Prog ID
        /// </summary>
        protected abstract string ProgId { get;}
        
        /// <summary>
        /// A standard place for the COM Calls
        /// </summary>
        /// <param name="name">Method name or property</param>
        /// <param name="flags">Binding Flags for Operation</param>
        /// <param name="args">object Array of arguments</param>
        /// <returns>The return Value if there is one</returns>
        protected object Invoke(string name, BindingFlags flags, params object[] args)
        {
            Debug.WriteLine(String.Format("Invoke - {0} {1}", GetType().Name, name));

            return comtype.InvokeMember(name, flags, null, comobj, args);
        }


        #region IDisposable Members

        public virtual void Dispose()
        {
            if (comobj != null)
            {
                FWBS.Common.COM.DisposeObject(comobj);
                comobj = null;

                Debug.WriteLine(String.Format("Destroyed Instance"));
            }
        }

        #endregion
    }

    #endregion

}

