using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using Document = FWBS.OMS.OMSDocument;

    [System.Runtime.InteropServices.Guid("60DC9259-83A7-4DF8-95F1-89357219E581")] 
	public class OyezOMS : OMSApp
	{
		#region Field Variables
		/// <summary>
		/// Oyez application variable
		/// </summary>
		private OyezFrms.IOyezAutomation _app = null;
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
		/// Used to overlay save and close functionality
		/// </summary>
		frmOyezDialog _frmButtons;
		/// <summary>
		/// Search form used to setup alias fields when in edit mode
		/// </summary>
		frmSearch _frmSrch;
		/// <summary>
		/// Transparent form used as overlay when field pickers are being displayed
		/// </summary>
		frmSpoof _frmspoof;
		/// <summary>
		/// used for blanking out buttons
		/// </summary>
		frmOyezOverlay _frmoverlay;
		/// <summary>
		/// Indicates whether we need to temporary switch DPI context for Save wizard creation
		/// in order to avoid issues on Windows 10 (1703+).
		/// </summary>
		private readonly DPIAwareness.DPI_AWARENESS _dpiAwareness = DPIAwareness.DPI_AWARENESS.UNAWARE;
		
		#endregion

		#region Constructors

		public OyezOMS() : base()
		{
			if (AppDomain.CurrentDomain.GetData("DPI_AWARENESS") != null)
			{
				_dpiAwareness = (DPIAwareness.DPI_AWARENESS)AppDomain.CurrentDomain.GetData("DPI_AWARENESS");
			}
		}
		
		/// <summary>
		/// Destructor of the enquiry object that MAY get called by the garbage collector.
		/// </summary>
		~OyezOMS()
		{
            Dispose(true);
		}

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}
		
		private void Dispose(bool disposing) 
		{
		}




		#endregion
		
				
		#region Save Methods


        protected override void InternalDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version)
        {
            if (Session.CurrentSession.IsLicensedFor("DM"))
            {

                CheckObjectIsDoc(ref obj);

                string newkey;
                string key = _currentKVitem.Key;
                MaximizeWindow(key);

                //reset error if it is not already
                ActiveForm.Error = "";

                IStorageItem item = version;
                if (item == null)
                    item = doc;

                StorageProvider provider = doc.GetStorageProvider();
                System.IO.FileInfo file = provider.GetLocalFile(item);
                ActiveForm.store(file.FullName);

                provider.Store(item, file, obj, true);
                newkey = GetWindowCaption();

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

                string newkey;
                string key = _currentKVitem.Key;
                MaximizeWindow(key);

                //reset error if it is not already
                ActiveForm.Error = "";

                IStorageItem item = version;
                if (item == null)
                    item = prec;

                StorageProvider provider = prec.GetStorageProvider();
                System.IO.FileInfo file = provider.GetLocalFile(item);
                ActiveForm.store(file.FullName);
                provider.Store(item, file, obj, true);
                newkey = GetWindowCaption();

                //oupdate the key values to the new values in the collections
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

		public override void InternalPrint(object objs,int copies)
		{
			for (int ctr = 1; ctr <= copies; ctr++)
				//Pages property returns 1 on flexi forms so no good this works
				ActiveForm.print(1,9999);
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
			return _fields.Tables[ActiveForm.Form].DefaultView.Count;
		}

		
		public override string GetFieldName(object obj, int index)
		{
			CheckObjectIsDoc(ref obj);
			string name = Convert.ToString(_fields.Tables[ActiveForm.Form].DefaultView[index]["fldAlias"]);
			if (name == "") 
				name = "#CONST#" + Convert.ToString(_fields.Tables[ActiveForm.Form].DefaultView[index]["fldConstant"]);  
			return name;
		}

		public override object GetFieldValue(object obj, int index)
		{
			CheckObjectIsDoc(ref obj);
			return GetDocVariable(obj, GetFieldName(obj, index));
		}

		public override void CheckFields(object obj)
		{}

		public override void SetFieldValue(object obj, int index, object val)
		{
			CheckObjectIsDoc(ref obj);
			string name = Convert.ToString(_fields.Tables[ActiveForm.Form].DefaultView[index]["fldName"]);
			int page = FWBS.Common.ConvertDef.ToInt32(_fields.Tables[ActiveForm.Form].DefaultView[index]["fldPage"],1);

			//go to page field relates to
			ActiveForm.selectpage(page);
				
			int id = -1;
			try
			{
				id = int.Parse(name);
			}
			catch{}

			if (id == -1)
				ActiveForm.fill(name, Convert.ToString(val));
			else
				ActiveForm.fillfield(id, Convert.ToString(val));

			SetDocVariable(obj, GetFieldName(obj, index), val);
		
		}

		public override object GetFieldValue(object obj, string name)
		{
			return GetDocVariable(obj, name);
		}

		public override void SetFieldValue(object obj, string name, object val)
		{
			
			int page;
			string fieldName;
			string fieldValue = "";
		
			name = name.Replace("#CONST#","").Replace("'","''");

			//need to loop around all the fields with the same name and check the page number
			//setting all the fileds with the same name based on the page
			DataView vw = _fields.Tables[ActiveForm.Form].DefaultView;

			vw.RowFilter = "fldAlias='" + name + "' or fldConstant='" + name + "'";
			
			//iterate all the rows with the same field name
			//OYEZ can have the same field name on multiple pages
			for(int i = 0;i < vw.Count;i++)
			{
				page = FWBS.Common.ConvertDef.ToInt32(vw[i]["fldPage"],1);
				fieldName = Convert.ToString(vw[i]["fldName"]);
				
                if(Convert.ToString(val).Contains("#CONST#"))
                    fieldValue = Convert.ToString(vw[i]["fldConstant"]);
                else
                    fieldValue = Convert.ToString(val);

				//go to page field relates to
				ActiveForm.selectpage(page);
				
				//if field name is a numeric then fill by ID of field
				//else fill by name
				int id = -1;
				try
				{
					id = int.Parse(fieldName);
				}
				catch{}

                if (id == -1)
                {
                    ActiveForm.fill(fieldName, fieldValue);
                }
                else
                {
                    ActiveForm.fillfield(id, fieldValue);
                }
                ActiveForm.finishfilling(0);
                System.Windows.Forms.Application.DoEvents();
			}
			
			SetDocVariable(obj, name, val);

            //try setting the views filter back to blank
            vw.RowFilter = "";

		}
		
		private void PrintFieldCodes()
		{
			// 
			for(int iPages = 0; iPages < ActiveForm.Pages; iPages++)
			{
				ActiveForm.selectpage( iPages + 1);						  
				
				string s = ActiveForm.FieldList;
				string delim = ",";
			
				string[] sArr = s.Split(delim.ToCharArray());
			
				for(int i = 0; i < sArr.Length; i++)
				{
					int id = -1;
					id = int.Parse(sArr[i]);
					//if id is -1 then field is a string value
					if(id == -1)
						ActiveForm.fill(sArr[i],sArr[i]);	
					else
						ActiveForm.fillfield(Convert.ToInt32(sArr[i]),sArr[i]);
					
				}
				//results are not guaranteed to be displayed unless this is called
				ActiveForm.finishfilling(0);
			}

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
			DateTime dtstart = (DateTime)GetDocVariable(_app, "DOCSTARTEDIT");
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
				OyezOMSForm form = (OyezOMSForm)_currentKVitem.Value;
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
				OyezOMSForm form = (OyezOMSForm)_currentKVitem.Value;
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
				//Added 2 Oct 2008 Don't remove base PrecID as causes it to look for default which doesn't exist
                if (varName.ToUpper() == "BASEPRECID")
                    return;

				OyezOMSForm form = (OyezOMSForm)_currentKVitem.Value;
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
				OyezOMSForm form = (OyezOMSForm)_currentKVitem.Value;
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

		
		public override void Close(object obj)
		{
			CloseOyez();
		}
		
		
		protected override void ScreenRefresh()
		{
			try
			{
				_frmspoof.Close();
				_frmspoof.Dispose();
				FWBS.Common.Functions.MaximizeWindow(ActiveWindow.Handle);
			}
			catch{}
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
			return "olf";
		}

		
		public override string ModuleName
		{
			get
			{
				return "Oyez Legal Forms Integration";
			}
		}

		
		public override IWin32Window ActiveWindow
		{
			get
			{
				return new Window(_app);
			}
		}


        protected override object InternalDocumentOpen(Document document, FetchResults fetchData, OpenSettings settings)
        {
            ActivateApplication();
            
            bool res = _app.load(fetchData.LocalFile.FullName);

            if (CheckOyezError(_app, fetchData.LocalFile.Name))
            {
                if (_opendocs.Count == 0)
                    CloseOyez();

                return null;
            }

            switch (settings.Mode)
            {
                case DocOpenMode.Edit:

                    //need to maximise this window as I rely on the top caption
                    string windowcaption = GetWindowCaption();
                    MaximizeWindow(windowcaption);

                    if (Session.CurrentSession.IsLicensedFor("DM"))
                    {
                        IWin32Window activeWindow = ActiveWindow;
                        //Need to temporary switch DPI awareness context to match Oyez Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
                        bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
                        using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
                        {
                            //Create a form object with caption as key and set current item
                            AddDocumentVariables(windowcaption);

                            //remove Oyez menu Items
                            RemoveMenuItems();
                            //display a save dialog
                            ShowSaveDialog(activeWindow);
                            //return ActiveForm;
                        }
                        break;
                    }
                    else
                        return new object();

                case DocOpenMode.Print:

                    object[] docs = new object[1];
                    docs[0] = ActiveForm;
                    BeginPrint(docs, settings.Printing);
                    return ActiveForm;

                case DocOpenMode.View:

                    if (Session.CurrentSession.IsLicensedFor("DM"))
                    {
                        IWin32Window activeWindow = ActiveWindow;
                        //Need to temporary switch DPI awareness context to match Oyez Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
                        bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
                        using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
                        {
                            //remove standard menus
                            RemoveMenuItems();
                            //display a save dialog
                            ShowSaveDialog(activeWindow);
                        }
                        break;
                    }
                    else
                        return new object();
            }

            if (ActiveForm != null)
            {                               
                //Added to support Save As
                if (Session.OMS.IsLicensedFor("DP"))
                    GetDocumentFields(ActiveForm.Form);
            }

            return ActiveForm;

        }

		
		
		protected override object InternalPrecedentOpen(Precedent precedent, FetchResults fetchData, OpenSettings settings)
		{
			try
			{
                if (Session.CurrentSession.IsLicensedFor("DP"))
                {
                    ActivateApplication();
                }
                else
                    return null;				
                
                
                string title = Convert.ToString(precedent.GetExtraInfo("precpath"));
                bool res = _app.NewForm(title);

                if (CheckOyezError(_app, title))
                {
                    if (_opendocs.Count == 0)
                        CloseOyez();

                    return null;
                }

				switch (settings.Mode)
				{
					case DocOpenMode.Edit:
						//TODO: Perhaps open up a form to add the field parser field to the equvalent mapped oyes form field id's.
                        IWin32Window activeWindow = ActiveWindow;
                        //Need to temporary switch DPI awareness context to match Oyez Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
                        bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
                        using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
                        {
                            _frmSrch = new frmSearch(title);
                            _frmSrch.ShowInTaskbar = false;

                            _frmSrch.ActiveDoc = ActiveForm;

                            //set parent windows and loose unwanted controls
                            FWBS.Common.Functions.SetParentWindow(activeWindow, _frmSrch);

                            //this is to force OYEZ as the parent as the Parent property is always null
                            _frmSrch.ParentWindow = activeWindow;
                            _frmSrch.Show();
                        }
						return ActiveForm;
						//return null;
										
					case DocOpenMode.Print:

						var activeForm = ActiveForm;
						object[] docs = new object[1] { activeForm };
						
						//minimise so the dialog is visible
						_app.minimize();

						//check if they want to print the field codes
						DialogResult result = MessageBox.ShowYesNoQuestion("OYEZFLDCODES","Do you want to print the field codes.",false);				
						//populate doc with the field codes
					
						if(result == DialogResult.Yes)
						{
							PrintFieldCodes();						
						}

						BeginPrint(docs, settings.Printing);
						CheckOyezError(activeForm);
						return activeForm;

					case DocOpenMode.View:

                        if (Session.CurrentSession.IsLicensedFor("DM"))
                        {
                            //prevent from opening past docs outside of Oyez
                            RemovePreviousDocList();
                            return ActiveForm;
                        }
                        else
                            return new object();
				}
			}
			catch (Exception ex)            
			{
				MessageBox.Show(ex);
			}
			return null;
		}
		
		
		protected override DocSaveStatus BeginSave(object obj, SaveSettings settings)
		{
			using (var contextBlock = _dpiAwareness > 0 ? new DPIContextBlock(_dpiAwareness) : null)
			{
				//itentify the document and load its key value pairing
				LoadDocumentVariables();

				return base.BeginSave(obj, settings);
			}
		}



		protected override object TemplateStart(object obj, PrecedentLink preclink)
		{

            //if you are not licesed for Doc production don't do anything
            if (!Session.CurrentSession.IsLicensedFor("DP"))
                return new object();
            
			_app = null;
			
			//Activate or create and activate the Oyez application
			ActivateApplication();
            
			string title = Convert.ToString(preclink.Precedent.GetExtraInfo("precpath")).TrimEnd();
			
			bool res = _app.NewForm(title);
			
			//check if there was an error loading form
			if (CheckOyezError(_app, title))
			{
				if (_opendocs.Count == 0)
					CloseOyez();

				return null;
			}
			
			//need to maximise this window as I rely on the top caption
			string windowcaption = "[" + title + "]";
			MaximizeWindow(windowcaption);
            IWin32Window activeWindow = ActiveWindow;
            //Need to temporary switch DPI awareness context to match Oyez Forms in order to avoid forced reset of caller's Office process-wide DPI awareness.
            bool switchContext = activeWindow.Handle != IntPtr.Zero && DPIAwareness.IsSupported;
            using (var contextBlock = switchContext ? new DPIContextBlock(activeWindow) : null)
            {
                if (Session.CurrentSession.IsLicensedFor("DP"))
                {
                    //Create a form object with caption as key and set current item
                    AddDocumentVariables(windowcaption);

                    //lock down the toolbars
                    RemoveMenuItems();

                    //Added to support save as so move this code into seperate function
                    GetDocumentFields(title);

                    // sort out the constants
                    SetupConstants(title, preclink);

                    //display a save dialog
                    ShowSaveDialog(activeWindow);

                    //overlay transparent form to prevent them from closing before merge has finished
                    _frmspoof = new frmSpoof();
                    FWBS.Common.Functions.SetParentWindow(activeWindow, _frmspoof);
                    _frmspoof.Show();
                }
            }

            return _app;
		}
		
		
		public override void ActivateApplication()
		{
            if (Session.CurrentSession.IsLicensedFor("DM") || Session.CurrentSession.IsLicensedFor("DP"))
            {

                try
                {
                    _app = System.Runtime.InteropServices.Marshal.GetActiveObject("OyezLegalForms.Application") as OyezFrms.IOyezAutomation;
                }
                catch
                {
                    Type t = Type.GetTypeFromProgID("OyezLegalForms.Application");

                    if (t == null)
                    {
                        throw new OMSException2("NOOYEZ", "Oyez Forms is not Installed!");
                    }
                    try
                    {
                        _app = Activator.CreateInstance(t) as OyezFrms.IOyezAutomation;
                        System.Windows.Forms.Application.DoEvents();

                    }
                    catch
                    {
                        throw new OMSException2("OYEZERROR", "Error opening Oyez application, please verify that it is installed correctly!");
                    }
                }

                try
                {
                    if (_app != null)
                        _app.maximize();

                }
                catch { }
                finally
                {
                    InvalidateOpenDocs();
                }
            }
		}

		
		protected override void CheckObjectIsDoc(ref object obj)
		{
			if (obj == this)
				obj = ActiveForm;
			else if (obj is OyezFrms.IOyezAutomation)
				ActiveForm = (OyezFrms.IOyezAutomation)obj;
			else
				throw new OMSException2("OYEZACCEPTNO","The passed parameter is not an acceptable oyez legal form object.");
			obj = ActiveForm;
		}

		
		protected override void InsertText(object obj, PrecedentLink precLink)
		{
		}


		protected override void SetWindowCaption(object obj, string caption)
		{
			CheckObjectIsDoc(ref obj);
		}

		
		/// <summary>
		/// when job finishes processing ad the key value collection to the documents keyvalue collection
		/// </summary>
		protected override void OnJobProcessed()
		{
			//this is normally called by the merge  but doesn't appear to be called when no fields are merged
			ScreenRefresh();

			base.OnJobProcessed();
		}


		#endregion

		#region Methods

        /// <summary>
        /// Manipulates the field constants
        /// </summary>
        private void SetupConstants(string title,PrecedentLink preclink)
        {
            DataTable dt = _fields.Tables[title];
            if (dt == null)
                return;

            System.Data.DataView constants = null;
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

            //clean up and rename
            SearchEngine.SearchList sch = new SearchEngine.SearchList(FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.OyezAliases), null, pars);
            System.Data.DataTable dt = sch.Run() as System.Data.DataTable;
            if (dt != null)
            {
                dt.TableName = title; // preclink.Precedent.Title;
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
			OyezOMSForm form = new OyezOMSForm();
			
			form.OyezForm = _app;
			form.DocumentVariables = new FWBS.Common.KeyValueCollection();

			_opendocs.Add(caption,form);
			
			//set the current item to this newly created one
			_currentKVitem = _opendocs[_opendocs.Count - 1];
			
			SetDocVariable(_app, "DOCSTARTEDIT", dt);
		}


		/// <summary>
		/// Loads the correct key value collection depending on form being shown
		/// </summary>
		private void LoadDocumentVariables()
		{
			//cannot use window caption as it is incorrect when a dialog pops up from OYEZ
			string caption = GetWindowCaption();
			
			//Unfortunately this method is not 100% reliable as a document name might be short enough
			//to be contained within the name of another
			//The Form property is the only one available to me via  
			for(int i = 0;i < _opendocs.Count; i++)
			{
				//capture the document name
				string doc = _opendocs[i].Key;
				
				int iLen = caption.IndexOf(doc);
				if(iLen > - 1)
				{
                    //set key value item to local variable so we can query out the key when saving
					_currentKVitem = _opendocs[i];
					
					//get values from key value collection of documents
					OyezOMSForm form = (OyezOMSForm)_currentKVitem.Value;
					_app = form.OyezForm;
					return;
				}
			}

		}


		/// <summary>
		/// displays and creates the save dialog
		/// </summary>
		private void ShowSaveDialog(IWin32Window activeWindow)
		{
			//check if it has ever been created
			if(_frmButtons == null)
				CreateSaveDialog1();
			
			if(_frmoverlay == null)
				CreateSaveDialog2();
						
			try
			{
				_frmoverlay.Show();
			}
			catch
			{
				CreateSaveDialog2();
				_frmoverlay.Show();
			}
            FWBS.Common.Functions.SetParentWindow(activeWindow, _frmoverlay);
            _frmoverlay.SetPosition();

            if (ButtonsEnabled())
            {
                //this little bit is necessary as the OYEZ app doesnt close properly and 
                // _frm can be disposed off but not null
                try
                {
                    _frmButtons.Show();

                }
                catch
                {
                    CreateSaveDialog1();
                    _frmButtons.Show();
                }
                FWBS.Common.Functions.SetParentWindow(activeWindow, _frmButtons);
                _frmButtons.SetPosition();

            }

			System.Windows.Forms.Application.DoEvents();
		}

        /// <summary>
        /// Checks registry key to see if the buttons are enabled
        /// Moved to a registry key in case the users want the buttons back
        /// </summary>
        /// <returns></returns>
        private bool ButtonsEnabled()
        {
            FWBS.Common.Reg.ApplicationSetting setting = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Oyez", "EnableButtons");
            return Convert.ToBoolean(setting.GetSetting(false));
        }


		/// <summary>
		/// creates the save dialog form and registers the save event
		/// </summary>
		private void CreateSaveDialog1()
		{
			_frmButtons = new frmOyezDialog();
			_frmButtons.OnSave +=new EventHandler(frm_OnSave);
			_frmButtons.OnClose += new EventHandler(_frm_OnClose);
            _frmButtons.OnClose += new EventHandler(_frmoverlay_OnSaveAs);
			_frmButtons.ShowInTaskbar = false;
		}
		
		
		/// <summary>
		/// creates the save dialog form and registers the save event
		/// </summary>
		private void CreateSaveDialog2()
		{
			_frmoverlay = new frmOyezOverlay();
			_frmoverlay.OnSave += new EventHandler(_frmoverlay_OnSave);
			_frmoverlay.OnClose += new EventHandler(_frmoverlay_OnClose);
            _frmoverlay.OnSaveAs += new EventHandler(_frmoverlay_OnSaveAs);
			_frmoverlay.ShowInTaskbar = false;
		}


				
		
		/// <summary>
		/// Closes Oyez App and releases resources
		/// </summary>
		private void CloseOyez()
		{
			try
			{
				//load document based on caption
				LoadDocumentVariables();
				
				string formname = _currentKVitem.Key;

				//## If multiple documents are loaded into Oyez 
				//## discard() closes first document but will not close any more.
				//## OYEZ object model appears to be flawed and thinks no more are loaded

				//remove form from Oyez
				ActiveForm.discard(); // doesnt prompt
				//ActiveForm.throwaway(); //prompts if form is changed
				
				//remove details from docs collection
				_opendocs.Remove(formname);
				
                //see if we have any more docs loaded ##unfortunately this is unrelaiable and a form may be loaded in 
				//OYEZ but the object model does not recognise this.
				formname = ActiveForm.Form;
				if(formname != "<blank>")
					return;

                //try and prevent create handle errors raised by nw version of OYEZ

                if (_frmoverlay != null)
                    _frmoverlay.Close();

                if (_frmButtons != null)
                    _frmButtons.Close();


				//if we get this far we shouldn't have any docs ##the app probably will have though
				//if multiple documents have been loaded as it doesn't appear to remove them properly
				//attempt to close application
				ActiveForm.exit();
				_opendocs.Clear();
				
				FWBS.Common.COM.DisposeObject(_app);
				_app = null;

			}
			catch{}

			//and because 99% of the time this doesn't actually stop the process
			try
			{
				Process[] procs = Process.GetProcesses();

				foreach(Process proc in procs)
				{
					if(proc.ProcessName.ToUpper() == "OYEZFRMS")
					{
						proc.CloseMainWindow();
						System.Windows.Forms.Application.DoEvents();
						CheckProcessIsKilled("OyezFrms");
						
					}
				}
			}
			catch{}
            //and finally a bit of internal housekeeping
            try
            {
                _frmButtons.Close();
                _frmoverlay.Close();
                
                _frmButtons.Dispose();
                _frmoverlay.Dispose();
                                
                _frmspoof = null;
                _frmButtons = null;
                _frmSrch = null;
                _frmoverlay = null;

            }
            catch { }

		}
		
		
		/// <summary>
		/// when in certain modes Oyez will not kill via .net classes
		/// so try to use the API method
		/// </summary>
		private void CheckProcessIsKilled(string processname)
		{
			Process[] procs = Process.GetProcesses();

			foreach(Process proc in procs)
			{
				if(proc.ProcessName.ToUpper() == processname.ToUpper())
				{
					//Close the process
					IntPtr hWnd = ActiveWindow.Handle;
					FWBS.Common.Functions.CloseProcess(hWnd);

                    //check if has gone
					procs = null;
					procs = Process.GetProcesses();
					foreach(Process proc1 in procs)
					{
						//if still there kill it
						if(proc1.ProcessName.ToUpper() == processname.ToUpper())
						{
							FWBS.Common.Functions.KillProcess(hWnd);
							return;
						}
					}
				}
			}
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
			IntPtr hwnd = Common.Functions.FindChildWindow(ActiveWindow.Handle,windowcaption);
			Common.Functions.MaximizeWindow(hwnd);
		}
        		
		/// <summary>
		/// Removes close and exit menus ffrom Oyez app
		/// </summary>
		private void RemoveMenuItems()
		{
			IntPtr hWnd = ActiveWindow.Handle;
			
			//disable the main windows close options
			FWBS.Common.Functions.DisableCloseMenu(hWnd);
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Save");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Save &As");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","Save In");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","E&xit");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"Window","&Cascade");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"Window","&Tile");
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"Window","Copy Wi&ndow");
			
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","&Close");
			//remove the popup menu item that appears below the icon
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"","Close");
			
			FWBS.Common.Functions.RemoveMenuSpacer(hWnd, "File",1);
			FWBS.Common.Functions.RemoveMenuSpacer(hWnd, "File",3);
			FWBS.Common.Functions.RemoveMenuSpacer(hWnd, "File",3);
			FWBS.Common.Functions.RemoveMenuSpacer(hWnd, "File",3);
			FWBS.Common.Functions.RemoveMenuSpacer(hWnd, "File",3);


			RemovePreviousDocList();
			
		}
		
		
		/// <summary>
		/// Remove any items that appear in previously opened file list
		/// </summary>
		private void RemovePreviousDocList()
		{
			IntPtr hWnd = ActiveWindow.Handle;

			//this is the menu name for recent files
			FWBS.Common.Functions.RemoveMenuItem(hWnd,"File","RECENT FILE");
			
			//this is not required more to act as a catch all will comment out if performace problems occur
			for( int i = 1;i < 10 ; i++)
			{
				FWBS.Common.Functions.RemoveMenuItem(hWnd,"File",i.ToString());
			}
		}
		
		
		/// <summary>
		/// Checks to see if Oyez has an error and displays it if so
		/// </summary>
		/// <param name="oyezObject">OyezFrms.IOyezAutomation object.</param>
		/// <param name="additionalInfo">Additional information to be appended to error message.</param>
		private bool CheckOyezError(OyezFrms.IOyezAutomation oyezObject, string additionalInfo = null)
		{
			//Oyez doesn't raise errors instead you have to query the error property
			string error = oyezObject.Error;
			if (!string.IsNullOrEmpty(error))
			{
				if (!string.IsNullOrWhiteSpace(additionalInfo))
					error += Environment.NewLine + additionalInfo;
				//display OYEZ error NOTE: may need refining in the future if this is insufficient
				MessageBox.Show(ActiveWindow, error);
				return true;
			}
			return false;
		}

        /// <summary>
        /// Invalidates _opendocs collection since application could have been closed by a user.
        /// </summary>
        private void InvalidateOpenDocs()
        {
            string s;
            for (int i = _opendocs.Count - 1; i >= 0; i--)
            {
                OyezOMSForm form = (OyezOMSForm)_opendocs[i].Value;
                try
                {
                    s = form.OyezForm.Form;
                }
                catch (Exception ex)
                {
                    s = ex.Message;
                    _opendocs.RemoveAt(i);
                }
            }
        }
		
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the active file.
		/// </summary>
		public OyezFrms.IOyezAutomation ActiveForm
		{
			get
			{
				//get the right application variable from key value collection
				string caption = GetWindowCaption();
			
				for(int i = 0;i < _opendocs.Count; i++)
				{
					//capture the document name
					string doc = _opendocs[i].Key;
				
					int iLen = caption.IndexOf(doc);
					if(iLen > - 1)
					{
						OyezOMSForm form = (OyezOMSForm)_opendocs[i].Value;
						_currentKVitem = _opendocs[i];
						_app = form.OyezForm;
						break;
					}
				}
				return _app;
				
			}
			set
			{
				_app = value;
			}
		}

		#endregion

		#region Event Handlers

        		
		private void frm_OnSave(object sender, EventArgs e)
		{
			//save the OMS document
			this.Save(ActiveForm);
		}

		private void _frm_OnClose(object sender, EventArgs e)
		{
			CloseOyez();
		}

		
		private void _frmoverlay_OnSave(object sender, EventArgs e)
		{
			//save the OMS document
			this.Save(ActiveForm);
		}


		private void _frmoverlay_OnClose(object sender, EventArgs e)
		{
			CloseOyez();
		}


        void _frmoverlay_OnSaveAs(object sender, EventArgs e)
        {
            this.SaveAs(ActiveForm, false);            
        }

		#endregion

		
	}
	
	//class used to hold variables for each OYEZ form loaded
	internal class OyezOMSForm
	{
		public OyezFrms.IOyezAutomation OyezForm;
        public Common.KeyValueCollection DocumentVariables;
		
	}
			


	internal class Window : IWin32Window
	{
		private IntPtr _handle;

		public Window(object obj)
		{
			string caption = "Oyez Legal Forms";
			try
			{					
				_handle = Common.Functions.FindWindow("", caption,false);
				if(_handle == IntPtr.Zero)
					_handle = Common.Functions.FindWindow("", "Macro1 (macro) - Sax Basic",false);
			}
			catch
			{
			}
		}

		public IntPtr Handle 
		{
			get
			{
				return _handle;
			}
		}
	}
}
