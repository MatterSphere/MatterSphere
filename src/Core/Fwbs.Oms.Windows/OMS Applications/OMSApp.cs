using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    using System.Collections.Generic;
    using FWBS.Common;
    using FWBS.OMS.Data;
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using FWBS.OMS.Security.Permissions;
    using FWBS.OMS.StatusManagement;
    using FWBS.OMS.StatusManagement.Activities;
    using Document = OMSDocument;

    /// <summary>
    /// An abstract class that will carry out all the common IOMSApp precedent
    /// and field parsing etc...
    /// </summary>
    public abstract class OMSApp : IOMSApp
    {


        #region Fields

        /// <summary>
        /// A reference to the active document.  Used if a document has been opened by a dialog.
        /// </summary>
        protected object _activeDoc = null;

        /// <summary>
        /// A reference to the OMSApp registerd application type.
        /// </summary>
        private Apps.RegisteredApplication _application = null;

        /// <summary>
        /// A flag indicating that there are multiple documents being saved to the system.
        /// </summary>
        private bool _multisave = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default OMSApp constructor.
        /// </summary>
        public OMSApp()
        {

        }


        #endregion

        #region Bookmarks

        /// <summary>
        /// Checks to see if the passed document has a bookmark of the specified name.
        /// </summary>
        /// <param name="obj">The object / document.</param>
        /// <param name="name">Bookmark name.</param>
        /// <returns>True, if the bookmark exists within the document.</returns>
        protected virtual bool HasBookmark(object obj, string name)
        {
            return false;
        }


        /// <summary>
        /// Gets the number of bookmarks within the document.
        /// </summary>
        /// <param name="obj">The object / document.</param>
        /// <returns></returns>
        protected virtual int GetBookmarkCount(object obj)
        {
            return 0;
        }

        /// <summary>
        /// Gets the bookmark name from the specified index.
        /// </summary>
        /// <param name="obj">The object / document.</param>
        /// <param name="index">The bookmark index.</param>
        /// <returns>A bookmark string name.</returns>
        protected virtual string GetBookmark(object obj, int index)
        {
            return "";
        }

        /// <summary>
        /// Replaces a specific bookmark with the specified text.
        /// </summary>
        /// <param name="obj">The object / document.</param>
        /// <param name="name">The name of the bookmark.</param>
        /// <param name="val">The value to replace the bookmark with.</param>
        protected virtual bool SetBookmark(object obj, string name, string val)
        {
            return false;
        }

        #endregion

        #region AdditionalText

        /// <summary>
        /// Activates the additional text form within the save routine.
        /// </summary>
        /// <param name="obj">The object / document.</param>
        private bool AdditionalText(object obj)
        {
            //Check the current document for Bookmarks System Based or 
            //proceeding with ADDitionalTEXT

            string arg1 = "", arg2 = "", arg3 = "";


            if (HasAddionalText(obj, AdditionalTextType.Top))
            {
                arg1 = "ADDTEXTTOP";
            }
            if (HasAddionalText(obj, AdditionalTextType.Middle))
            {
                arg2 = "ADDTEXTMIDDLE";
            }
            if (HasAddionalText(obj, AdditionalTextType.Bottom))
            {
                arg3 = "ADDTEXTFOOT";
            }


            if (arg1 == "" && arg2 == "" && arg3 == "")
            {
                //Arguments not set and no lookups so quit 
                return true;
            }

            using (FWBS.OMS.UI.Windows.frmAdditionalText addSaveFrm = new FWBS.OMS.UI.Windows.frmAdditionalText(this, obj, Environment.NewLine, arg1, arg2, arg3))
            {
                if (addSaveFrm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return false;

                if (addSaveFrm.PanelOneText != "")
                {
                    SetAddionalText(obj, AdditionalTextType.Top, addSaveFrm.PanelOneText);
                }

                if (addSaveFrm.PanelTwoText != "")
                {
                    SetAddionalText(obj, AdditionalTextType.Middle, addSaveFrm.PanelTwoText);
                }

                if (addSaveFrm.PanelThreeText != "")
                {
                    SetAddionalText(obj, AdditionalTextType.Bottom, addSaveFrm.PanelThreeText);
                }

                if (addSaveFrm.Slogan)
                {
                    AddSlogan(obj, FetchSlogan(obj));
                }
            }
            return true;

        }

        public enum AdditionalTextType
        {
            Top,
            Middle,
            Bottom
        }

        protected virtual bool HasAddionalText(object obj, AdditionalTextType type)
        {
            CheckObjectIsDoc(ref obj);


            //Old letterheads in OMS2k used AdditionalText, AdditionalTextTop and FooterText as the system pointers. Use these but translate to the following lookups
            switch (type)
            {
                case AdditionalTextType.Top:
                    return (HasBookmark(obj, "AdditionalTextTop") || HasBookmark(obj, "ADDTEXTTOP"));
                case AdditionalTextType.Middle:
                    return (HasBookmark(obj, "AdditionalText") || HasBookmark(obj, "ADDTEXMIDDLE"));
                case AdditionalTextType.Bottom:
                    return (HasBookmark(obj, "FooterText") || HasBookmark(obj, "ADDTEXTFOOT"));
                default:
                    return false;
            }
        }

        /// <summary>
        /// Sets the additional text.
        /// </summary>
        /// <param name="type">Additional text type.</param>
        /// <param name="val">Value to set.</param>
        /// <returns></returns>
        protected virtual void SetAddionalText(object obj, AdditionalTextType type, string val)
        {
            CheckObjectIsDoc(ref obj);
            switch (type)
            {
                case AdditionalTextType.Top:
                    if (HasBookmark(obj, "AdditionalTextTop"))
                        SetBookmark(obj, "AdditionalTextTop", val);
                    else if (HasBookmark(obj, "ADDTEXTTOP"))
                        SetBookmark(obj, "ADDTEXTTOP", val);
                    break;
                case AdditionalTextType.Middle:
                    if (HasBookmark(obj, "AdditionalText"))
                        SetBookmark(obj, "AdditionalText", val);
                    else if (HasBookmark(obj, "ADDTEXTMIDDLE"))
                        SetBookmark(obj, "ADDTEXTMIDDLE", val);
                    break;
                case AdditionalTextType.Bottom:
                    if (HasBookmark(obj, "FooterText"))
                        SetBookmark(obj, "FooterText", val);
                    else if (HasBookmark(obj, "ADDTEXTFOOT"))
                        SetBookmark(obj, "ADDTEXTFOOT", val);
                    break;
            }
        }

        /// <summary>
        /// Fetches the slogan to put at the bottom of the addional text.
        /// </summary>
        /// <returns></returns>
        protected string FetchSlogan(object obj)
        {
            DataTable slogans = CodeLookup.GetLookups("SLOGAN");
            slogans.DefaultView.RowFilter = "cdcode = 'COMPANY'";
            if (slogans.DefaultView.Count > 0)
                return Convert.ToString(slogans.DefaultView[0]["cddesc"]);

            try
            {
                OMSFile file = OMSFile.GetFile(GetDocVariable(obj, FILE, -1));
                slogans.DefaultView.RowFilter = "cdcode = '" + Common.SQLRoutines.RemoveRubbish(file.Department) + "'";

                if (slogans.DefaultView.Count > 0)
                    return Convert.ToString(slogans.DefaultView[0]["cddesc"]);
            }
            catch { }
            return "";
        }

        public virtual void AddSlogan(object obj, string val)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGSLGNTSUPP", "Slogan feature is not supported.", "").Text);
        }

        #endregion

        #region Document Variable Methods

        /// <summary>
        /// Gets the editing time that has been spent on a document.
        /// </summary>
        /// <param name="obj">The current document to query.</param>
        /// <returns>The time in minutes.</returns>
        public override int GetDocEditingTime(object obj)
        {
            return 0;
        }

        /// <summary>
        /// Displays the document variables.
        /// </summary>
        /// <param name="obj">The document object of which to vie the variables.</param>
        /// <param name="val">The value to print to out.</param>
        protected override void DisplayDocVariables(object obj, string val)
        {
            MessageBox.Show(val);
        }


        #endregion

        #region Field Routines

        protected override void UpdateDocFields(object obj, PrecedentLink preclink)
        {
            DocumentVersion ver = GetCurrentDocumentVersion(obj);

            UpdateDocFields(obj, preclink, ver, null);
        }

        protected override void UpdateDocFields(object obj, PrecedentLink preclink, DocumentVersion ver, string[] fields)
        {
            Associate assocobj = preclink.Associate;
            Precedent prec = preclink.Precedent;

            assocobj.Refresh(false);

            FieldParser parser = preclink.Parser;

            if (ver != null)
                parser.ChangeObject(ver);

            parser.Prompt += new FieldParserPromptHandler(_fieldParser_Prompt);
            parser.ListPrompt += new FieldParserListPromptHandler(_fieldParser_ListPrompt);
            preclink.Associate.MultiEmployersPromptHandler += new EventHandler<KeyValueCollectionEventArgs>(Associate_MultiEmployersPromptHandler);

            if (fields == null)
                fields = GetFieldNames(obj);

            if (GetDocVariable(obj, "DONTUPDATEFORMFIELDS", "NOTCONFIG") == "NOTCONFIG")
            {
                foreach (string fieldName in fields)
                {
                    ///Make sure the field exclusions are not overwritten.
                    if (Array.IndexOf(FieldExclusionList, fieldName.ToUpper()) > -1)
                        continue;

                    ///Make sure that the listed values id values are not overwritten.
                    ///If set to 0 = CANCELLED then set them to -1 = NOTSET.
                    if (fieldName.ToUpper().IndexOf(SEP) > -1)
                    {
                        long id = GetDocVariable(obj, fieldName, -1);
                        if (id == 0)
                            SetDocVariable(obj, fieldName, -1);
                        continue;
                    }

                    object val = parser.Parse(true, fieldName);
                    SetFieldValue(obj, fieldName, val);
                }

            }

            //Only if a null precedent has been passed or the precedent is text only do the second stage
            //merge.
            if (this is FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)
            {
                FWBS.OMS.Interfaces.ISecondStageMergeOMSApp ssm = (FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)this;
                if (prec.TextOnly == true && prec.AllowSecondStageMerge == true)
                {
                    ssm.SecondStageMerge(obj, preclink.SecondStageMerge());
                }
            }

            //Loop round any bookmark and create tables where ever specified.
            int bmcount = GetBookmarkCount(obj);
            if (bmcount > 0)
            {
                for (int ctr = 0; ctr < bmcount; ctr++)
                {
                    string bmname = GetBookmark(obj, ctr);
                    if (bmname.Length > 0)
                    {
                        try
                        {
                            if (bmname.StartsWith(FieldParser.FieldPrefixDataList))
                            {
                                BuildTable(obj, bmname, (DataView)parser.Parse(bmname), false);
                            }
                            else if (bmname.StartsWith(FieldParser.FieldPrefixSearchList))
                            {
                                BuildTable(obj, bmname, (DataView)parser.Parse(bmname), true);
                            }
                        }
                        catch
                        {
                        }
                    }

                }
            }

            CustomUpdateDocFields(obj, preclink);

            ScreenRefresh();

            CheckFields(obj);

            preclink.Associate.MultiEmployersPromptHandler -= new EventHandler<KeyValueCollectionEventArgs>(Associate_MultiEmployersPromptHandler);

        }

        private void Associate_MultiEmployersPromptHandler(object sender, KeyValueCollectionEventArgs e)
        {
            var ass = (Associate)sender;
            KeyValueCollection kvc = new KeyValueCollection() { { "contID", ass.Contact.ID } };
            KeyValueCollection kvcEmployers = (KeyValueCollection)FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, "LCLGETEMPRS", false, new System.Drawing.Size(1000, 1200), null, kvc);
            e.Collection = kvcEmployers;
        }

        protected virtual void CustomUpdateDocFields(object obj, PrecedentLink precLink)
        {
        }

        #endregion

        #region Print Routines

        /// <summary>
        /// Prints the specified passed object.  This should run the most complex
        /// print routine where a print dialog may be shown.
        /// </summary>
        /// <param name="obj">The object to be printed.</param>
        public override void Print(object obj)
        {
            PrintSettings settings = PrintSettings.Default;
            settings.Mode = PrecPrintMode.Dialog;
            BeginPrint(new object[1] { obj }, settings);
        }

        /// <summary>
        /// A quicjk print method which should take all printing defaults
        /// of the pass object.
        /// </summary>
        /// <param name="obj">object to be worked on</param>
        public override void PrintQuick(object obj)
        {
            PrintSettings settings = PrintSettings.Default;
            settings.Mode = PrecPrintMode.Print;
            BeginPrint(new object[1] { obj }, settings);
        }

        /// <summary>
        /// The derived class must implement this print method using the different 
        /// print combinations.
        /// </summary>
        /// <param name="objs">The object to be printed.</param>
        /// <param name="printMode">The print mode to be used.</param>
        public abstract void InternalPrint(object obj, int copies);

        /// <summary>
        /// The begin print method which will ask a generic print question.
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="printMode"></param>
        /// 


        public virtual void BeginPrint(object[] objs, PrintSettings settings)
        {
            if (settings.Mode == PrecPrintMode.None || CanPrint == false)
                return;

            if ((settings.Mode | PrecPrintMode.Print) == settings.Mode)
            {
                int copies = copies = settings.CopiesToPrint;

                if (objs.Length > 0)
                {

                    if (settings.BulkPrintMode == false)
                    {
                        copies = 1;
                        //If the dialog flag is set then ask for the number of copies.
                        if ((settings.Mode | PrecPrintMode.Dialog) == settings.Mode)
                        {
                            copies = Common.ConvertDef.ToInt32(InputBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("PRINTCOPIES", "How many copies would you like to print?", "").Text, "", copies.ToString()), 0);
                            if (copies == 0) return;
                        }
                    }



                    foreach (object obj in objs)
                    {
                        object tmp = obj;
                        CheckObjectIsDoc(ref tmp);
                        InternalPrint(obj, copies);
                    }
                }
            }

            if ((settings.Mode | PrecPrintMode.Email) == settings.Mode)
            {
                foreach (object obj in objs)
                {
                    if (IsPrecedent(obj) == false)
                    {
                        object tmp = obj;
                        CheckObjectIsDoc(ref tmp);
                        //Can only send through email if set the document has been saved.

                        DocumentVersion version = GetCurrentDocumentVersion(tmp);
                        Document doc = GetCurrentDocument(tmp);
                        if (version != null)
                        {
                            SendDocViaEmail(version, null);
                        }
                        else if (doc != null)
                        {
                            SendDocViaEmail(doc, null);
                        }

                    }
                }
            }

            EndPrint();
        }

        /// <summary>
        /// Gets called when the print procedure ends.
        /// </summary>
        public virtual void EndPrint()
        {
        }

        /// <summary>
        /// Gets a boolean value indicating whether the OMS application can print.
        /// </summary>
        public virtual bool CanPrint
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Save Routines

        /// <summary>
        /// Carries out a full save routine displaying a print dialog
        /// </summary>
        /// <param name="obj">Object to be saved.</param>
        public override bool Save(object obj)
        {
            return Save(obj, (Action<SaveSettings>)null);
        }

        public bool Save(object obj, Action<SaveSettings> callback)
        {

            SaveSettings settings = SaveSettings.Default;
            settings.ContinueOnError = false;
            settings.Mode = PrecSaveMode.Save;

            return Save(obj, settings, callback);

        }

        public bool Save(object obj, SaveSettings settings)
        {
            return Save(obj, settings, (Action<SaveSettings>)null);
        }

        public bool Save(object obj, SaveSettings settings, Action<SaveSettings> callback)
        {
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();

            CheckObjectIsDoc(ref obj);

            if (ItemMustBeSavedAsNewDocument(obj, true))
                return SaveAs(obj, false);

            if (settings == null)
                settings = SaveSettings.Default;

            if (callback != null)
                callback(settings);

            return (BeginSave(obj, settings) == DocSaveStatus.Success);

        }


        public override bool SaveAs(object obj, bool asPrecedent)
        {
            return SaveAs(obj, asPrecedent, (Action<SaveSettings>)null);
        }

        public bool SaveAs(object obj, bool asPrecedent, SaveSettings settings, Action<SaveSettings> callback)
        {
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();

            CheckObjectIsDoc(ref obj);

            bool runAsSave = false;

            SaveSettings saveSettings = settings ?? SaveSettings.Default;

            StorageSettingsCollection storageSettings = null;
            IStorageItem item = null;

            if (!asPrecedent && !IsPrecedent(obj) && GetDocVariable(obj, DOCUMENT, 0) != 0)
            {
                item = GetCurrentDocumentVersion(obj);

                if (item == null)
                    item = GetCurrentDocument(obj);

                if (item != null)
                {
                    storageSettings = item.GetSettings();
                    if (storageSettings == null || storageSettings.Count == 0)
                        storageSettings = item.GetStorageProvider().GetDefaultSettings(item, SettingsType.Store);
                }
            }

            if (item != null && !ItemMustBeSavedAsNewDocument(obj, false))
            {
                FWBS.Common.TriState promptState = Session.CurrentSession.CurrentUser.PromptBeforeSaveAsOnResave;
                if (promptState == FWBS.Common.TriState.Null && Session.CurrentSession.PromptBeforeSaveAsOnResave)
                    promptState = FWBS.Common.TriState.True;

                if (promptState == FWBS.Common.TriState.True)
                {
                    FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker vers = new FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker();
                    if (vers.Render(item))
                        vers.ShowDialog(ActiveWindow);

                    if (vers.DialogResult == DialogResult.Cancel)
                    {
                        vers.Dispose();
                        return false;
                    }

                    VersionStoreSettings version = new VersionStoreSettings();

                    switch (vers.PickedVersion)
                    {
                        case FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker.VersionPicked.NewDocument:
                            //run the save as usual
                            break;
                        case FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker.VersionPicked.NewMajorVersion:
                            runAsSave = true;
                            version.SaveItemAs = VersionStoreSettings.StoreAs.NewMajorVersion;
                            break;
                        case FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker.VersionPicked.NewVersion:
                            runAsSave = true;
                            version.SaveItemAs = VersionStoreSettings.StoreAs.NewVersion;
                            break;
                        case FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker.VersionPicked.Overwrite:
                            runAsSave = true;
                            version.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;
                            break;
                        case FWBS.OMS.UI.DocumentManagement.VersionOptionsPicker.VersionPicked.NewSubVersion:
                            runAsSave = true;
                            version.SaveItemAs = VersionStoreSettings.StoreAs.NewSubVersion;
                            break;
                    }

                    saveSettings.StorageSettings.Add(version);
                    vers.Dispose();
                }
            }

            if (runAsSave)
            {
                return Save(obj, saveSettings, callback);
            }
            else
            {
                if (callback != null)
                    callback(saveSettings);

                string docDesc = null;
                bool isCompanyDocument = IsCompanyDocument(obj);
                Common.KeyValueCollection old = CopyVariables(obj);
                IStorageItem originalDocument = GetCurrentDocumentVersion(obj);
                if (originalDocument == null)
                {
                    originalDocument = GetCurrentDocument(obj);
                    if (originalDocument != null)
                        docDesc = ReturnDocumentName(((OMSDocument)originalDocument).Description);
                    else if (IsPrecedent(obj) && old.Contains(PRECEDENT))
                        originalDocument = FWBS.OMS.Precedent.GetPrecedent(Convert.ToInt64(old[PRECEDENT].Value));
                }
                else
                {
                    Common.ApplicationSetting useDocumentNameDuringSaveAs = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "UseDocumentNameDuringSaveAs", "false");
                    if (useDocumentNameDuringSaveAs.ToBoolean())
                    {
                        try
                        {
                            docDesc = ReturnDocumentName(((FWBS.OMS.UI.Windows.ShellFile)obj).Name);
                        }
                        catch
                        {
                            docDesc = ((DocumentVersion)originalDocument).ParentDocument.Description;
                        }
                    }
                    else
                    {
                        docDesc = ((DocumentVersion)originalDocument).ParentDocument.Description;
                    }
                }

                var assoc = GetCurrentAssociate(obj);

                DettachDocumentVars(obj);
                DettachPrecedentVars(obj);

                if (asPrecedent)
                {
                    SetDocVariable(obj, ISPREC, true);
                }
                else if (originalDocument == null && isCompanyDocument && old.Contains(BASEPRECEDENT) && old.Contains(TYPE))
                {
                    SetDocVariable(obj, COMPANY, old[COMPANY].Value);
                    SetDocVariable(obj, BASEPRECEDENT, old[BASEPRECEDENT].Value);
                    SetDocVariable(obj, TYPE, old[TYPE].Value);
                }

                if (assoc != null)
                    SetDocVariable(obj, RELINK, assoc.ID);
                else
                    RemoveDocVariable(obj, RELINK);

                SaveSettings newDocSettings = SaveSettings.Default;
                newDocSettings.ContinueOnError = false;
                newDocSettings.Mode = PrecSaveMode.Save;
                newDocSettings.DocumentDescription = docDesc;

                if (saveSettings.FolderGuid != null)
                {
                    newDocSettings.FolderGuid = saveSettings.FolderGuid;
                }
                if (saveSettings.TargetAssociate != null)
                {
                    newDocSettings.TargetAssociate = saveSettings.TargetAssociate;
                }
                    
                newDocSettings.AllowRelink = saveSettings.AllowRelink;
                
                if (saveSettings.SkipTimeRecords)
                {
                    newDocSettings.SkipTimeRecords = true;
                }

                newDocSettings.StorageSettings.Merge(storageSettings);

                DocSaveStatus ret = BeginSave(obj, newDocSettings);

                if (ret != DocSaveStatus.Success)
                {
                    RemoveDocVariable(obj, RELINK);
                    RemoveDocVariable(obj, ISPREC);

                    ApplyVariables(obj, old);


                }
                else if (Session.CurrentSession.UnlockOriginalOnSaveAs && originalDocument != null)
                {
                    IStorageItemLockable lockable = originalDocument.GetStorageProvider().GetLockableItem(originalDocument);

                    if (lockable != null)
                    {
                        if (lockable.CanUndo)
                            lockable.UndoCheckOut();
                    }

                    if (originalDocument is Precedent)
                    {
                        UnlockPrecedent(((Precedent)originalDocument).ID.ToString());
                    }
                }

                return (ret == DocSaveStatus.Success);
            }
        }



        public bool SaveAs(object obj, bool asPrecedent, Action<SaveSettings> callback)
        {
            return SaveAs(obj, asPrecedent, null, callback);
        }

        // Remove part of name in last parentheses
        // Example "A (B)" => "A"
        private string ReturnDocumentName(string objName)
        {
            if (objName == null)
            {
                return objName;
            }
            var indexOfParenthesis = objName.LastIndexOf('(');
            if (indexOfParenthesis != -1)
            {
                return objName.Substring(0, indexOfParenthesis).TrimEnd();
            }
            else
            {
                return objName;
            }
        }

        /// <summary>
        /// Carries out a quick save routine with an auto print option.
        /// </summary>
        /// <param name="obj">Object to be saved.</param>
        public override bool SaveQuick(object obj)
        {
            return SaveQuick(obj, (Action<SaveSettings>)null);
        }

        /// <summary>
        /// Carries out a quick save routine with an auto print option.
        /// </summary>
        /// <param name="obj">Object to be saved.</param>
        public virtual bool SaveQuick(object obj, Action<SaveSettings> callback)
        {
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();

            CheckObjectIsDoc(ref obj);

            if (ItemMustBeSavedAsNewDocument(obj, true))
                return SaveAs(obj, false);

            SaveSettings settings = SaveSettings.Default;

            settings.ContinueOnError = false;
            settings.Mode = PrecSaveMode.Quick;

            if (callback != null)
                callback(settings);

            return (BeginSave(obj, settings) == DocSaveStatus.Success);
        }



        /// <summary>
        /// Saves multiple of Documents under the same client, file and associate.
        /// </summary>
        /// <param name="objs"></param>
        public void Save(object[] objs)
        {
            if (objs == null)
                throw new ArgumentNullException("objs");

            Save(objs, objs.Length, (Action<SaveSettings>)null);
        }

        public void Save(System.Collections.IEnumerable objs, int count)
        {
            Save(objs, count, (Action<SaveSettings>)null);
        }

        /// <summary>
        /// Saves multiple of Documents under the same client, file and associate.
        /// </summary>
        /// <param name="objs"></param>
        public void Save(object[] objs, Action<SaveSettings> callback)
        {
            if (objs == null)
                throw new ArgumentNullException("objs");

            Save(objs, objs.Length, callback);
        }

        public void Save(System.Collections.IEnumerable objs, int count, Action<SaveSettings> callback)
        {
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();

            SaveSettings settings = SaveSettings.Default;

            settings.Printing.Mode = PrecPrintMode.None;
            settings.Mode = PrecSaveMode.Quick;
            settings.ContinueOnError = false;

            Save(objs, settings, callback);
        }

        public void Save(object[] objs, SaveSettings settings)
        {
            if (objs == null)
                throw new ArgumentNullException("objs");

            Save(objs, objs.Length, settings, (Action<SaveSettings>)null);
        }

        public void Save(System.Collections.IEnumerable objs, int count, SaveSettings settings)
        {
            Save(objs, count, settings, (Action<SaveSettings>)null);
        }

        private void Save(System.Collections.IEnumerable objs, int count, SaveSettings settings, Action<SaveSettings> callback)
        {
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();

            try
            {
                _multisave = true;

                Associate assoc = null;

                if (settings.TargetAssociate != null)
                {
                    assoc = settings.TargetAssociate;
                }
                else if (settings.UseDefaultAssociate)
                {
                    assoc = Services.SelectDefaultAssociate(ActiveWindow);
                }
                else
                {
                    assoc = Services.SelectAssociate(ActiveWindow);
                }

                if (callback != null)
                    callback(settings);

                if (assoc != null)
                {
                    try
                    {
                        string msg = "Multiple Document Save";
                        string saving = "Saving selected documents...";

                        if (objs.Count() == 1)
                        {
                            msg = "Saving Document";
                            saving = "Saving selected document...";
                        }
                        OnProgressStart(msg, saving, count, true);

                        int ctr = 0;
                        foreach (object obj in objs)
                        {
                            ctr++;
                            bool cancel = false;
                            bool unprofiledDocument = true;
                            string text = GenerateDocDesc(obj);
                            DocSaveStatus status = DocSaveStatus.Error;

                            try
                            {
                                Application.DoEvents();
                                OnProgress(String.Format("Saving- {0} ...", text), ctr, out cancel);
                                Application.DoEvents();

                                if (HasDocVariable(obj, "DOCUMENT"))
                                {
                                    unprofiledDocument = false;
                                }
                                AttachDocumentVars(obj, settings.UseDefaultAssociate, assoc);

                                status = BeginSave(obj, settings);
                                Application.DoEvents();
                                if (status == DocSaveStatus.Error)
                                {
                                    DettachDocumentVars(obj);
                                    OnProgress(String.Format("Error - {0} ...", text), ctr, out cancel);
                                }
                                Application.DoEvents();


                                if (status == DocSaveStatus.Success)
                                {
                                    Application.DoEvents();
                                    OnProgress(String.Format("Moving - {0} ...", text), ctr, out cancel);
                                    if (settings.AllowMove && settings.Move)
                                    {
                                        Application.DoEvents();
                                        Move(obj);
                                    }
                                    Application.DoEvents();
                                }

                                if (status == DocSaveStatus.Cancel && unprofiledDocument)
                                {
                                    DettachDocumentVars(obj);
                                }
                            }
                            catch
                            {
                                Application.DoEvents();
                                OnProgress(String.Format("Error - {0} ...", text), ctr);
                                Application.DoEvents();
                            }
                            finally
                            {
                            }

                            if (cancel) return;
                        }

                    }
                    finally
                    {
                        OnProgressFinished();
                    }
                }
            }
            finally
            {
                _multisave = false;
            }
        }

        private bool ItemMustBeSavedAsNewDocument(object obj, bool runningSave)
        {
            if (IsPrecedent(obj))
                return false;

            IStorageItem item = GetCurrentDocumentVersion(obj);
            if (item == null)
                item = GetCurrentDocument(obj);

            if (item == null)
                return false;

            if (item.IsNew)
                return false;

            if (!item.Supports(StorageFeature.Versioning) && !runningSave)
                return true;

            if (!Session.CurrentSession.ForceSaveAsWhenEditedByAnother)
                return false;

            OMSDocument doc = item as OMSDocument;
            DocumentVersion vers = item as DocumentVersion;

            if (vers != null)
                doc = vers.ParentDocument;

            IStorageItemLockable lockable = item.GetStorageProvider().GetLockableItem(item);
            //Document cant be locked, it cant fail
            if (lockable == null)
                return false;
            //current user has complete control over the document
            if (!lockable.CanCheckIn && !lockable.CanCheckOut)
                return true;

            if (vers == null)
                vers = ((IStorageItemVersionable)item).GetLatestVersion() as DocumentVersion;

            DateTime? lastUpdated = vers.LastUpdated;

            //document has never been updated
            if (lastUpdated == null)
                return false;

            DataTable dt = StorageManager.CurrentManager.LocalDocuments.GetLocalDocumentInfo();

            DataView vw = dt.DefaultView;
            vw.RowFilter = string.Format("verid='{0}'", vers.Id);
            if (vw.Count <= 0)
                return false;

            if (vw[0]["FileModified"] == DBNull.Value)
                return false;

            DateTime localcreated = Convert.ToDateTime(vw[0]["CacheDate"]);
            return lastUpdated.Value.ToUniversalTime() >= localcreated.ToUniversalTime();
        }

        protected override bool IsReadOnly(object obj)
        {
            return false;
        }


        /// <summary>
        /// Runs the first part of the save routine.
        /// </summary>
        /// <param name="obj">The object to be saved.</param>
        protected virtual DocSaveStatus BeginSave(object obj, SaveSettings settings)
        {
            if (_application == null)
                _application = Apps.ApplicationManager.CurrentManager.GetRegisteredApplication(this.GetType().GUID);

            Common.KeyValueCollection old = CopyVariables(obj);
            bool internalSave = true;


            if (IsPrecedent(obj) == false)
            {
                #region "Begin Save - Document"

                //Different document interface variables.
                Document docobj = null;
                IStorageItem storeItem = null;
                IStorageItemLockable lockable = null;
                IStorageItemVersionable versionable = null;
                IStorageItemDuplication duplication = null;
                DocumentVersion version = null;

                string docdesc = settings.DocumentDescription;

                if (IsCompanyDocument(obj) == false)
                {
                    DettachDocumentVars(obj);
                }


                if (GetDocVariable(obj, DOCUMENT, 0) == 0)
                {
                    if (settings.Mode != PrecSaveMode.None)
                    {
                        Precedent precobj = null;
                        Precedent baseprecobj = null;

                        Associate assocobj = GetCurrentAssociate(obj);

                        //Get the last and base precedent used on the document.
                        long precid = GetDocVariable(obj, PRECEDENT, 0);
                        long precbaseid = GetDocVariable(obj, BASEPRECEDENT, 0);


                        if (settings.TargetAssociate != null)
                        {
                            assocobj = settings.TargetAssociate;
                        }
                        else
                        {
                            if (settings.UseExistingAssociate)
                            {
                                assocobj = FWBS.OMS.Associate.GetAssociate(GetDocVariable(obj, ASSOCIATE, 0));
                            }
                            else
                            {
                                if (settings.UsePreviousAssoicate && settings.PreviousAssociate != null)
                                    assocobj = settings.PreviousAssociate;
                                else
                                    assocobj = SelectAssociate(obj, settings.UseDefaultAssociate, assocobj);
                            }
                        }


                        if (settings.UsePreviousAssoicate)
                            settings.PreviousAssociate = assocobj;

                        if (assocobj != null)
                        {

                            // Check Security on File to see if a new Document can be added
                            new FilePermission(assocobj.OMSFile, StandardPermissionType.SaveDocument).Check();

                            //Client and Matter Status check
                            new FileActivity(assocobj.OMSFile, FileStatusActivityType.DocumentModification).Check();

                            //Get the last used precedent object.
                            if (precid != 0)
                                precobj = FWBS.OMS.Precedent.GetPrecedent(precid);

                            //Find the base precedent object.  This could be the same as the time recorded prec id.
                            if (precbaseid != 0 && precbaseid != precid)
                                baseprecobj = Precedent.GetPrecedent(precbaseid);
                            else
                                baseprecobj = precobj;

                            //Find the applications base blank precedent to use if a base tyep cannot be found.
                            if (baseprecobj == null)
                            {
                                string doctype = GetActiveDocType(obj);
                                if (doctype != "")
                                { // Construct a Precedent object of the GetActiveType() return if not ""
                                    baseprecobj = FWBS.OMS.Precedent.GetDefaultPrecedent(doctype, assocobj);
                                }
                                else
                                {
                                    //If not active type has been set then find the default precedent andset it.
                                    baseprecobj = Precedent.GetDefaultPrecedent(this);
                                    SetDocVariable(obj, BASEPRECEDENT, baseprecobj.ID);
                                    SetDocVariable(obj, TYPE, baseprecobj.PrecedentType);
                                }
                            }

                            //Relink to document fields if requested to.
                            long oldAssocId = GetDocVariable(obj, RELINK, 0);
                            if (oldAssocId != 0 && settings.AllowRelink)
                            {
                                if (assocobj.ID != oldAssocId)
                                {
                                    Associate oldAssoc = Associate.GetAssociate(oldAssocId);

                                    bool relink = false;

                                    if (oldAssoc != null && assocobj.OMSFileID != oldAssoc.OMSFileID)
                                        relink = MessageBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("ASSOCUPDATE", "You have chosen to save to another %FILE% would you like to Relink and update fields?", ""), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
                                    else
                                        relink = MessageBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("ASSOCUPDATE£", "You have chosen to save to another %ASSOCIATE% would you like to Relink and update fields?", ""), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

                                    if (relink)
                                    {
                                        AttachDocumentVars(obj, settings.UseDefaultAssociate, assocobj);
                                        RelinkFields(obj);
                                    }
                                }
                                RemoveDocVariable(obj, RELINK);
                            }

                            DocumentDirection direction = GetActiveDocDirection(obj, baseprecobj);
                            if (string.IsNullOrEmpty(docdesc))
                                docdesc = GenerateDocDesc(obj);

                            SubDocument[] subdocs = AttachSubDocuments(obj);

                            docobj = new Document(assocobj, docdesc, baseprecobj, precobj, GetDocEditingTime(obj), direction, GetDocExtension(obj), GetDocStorageLocation(obj), subdocs);
                            docobj.ContinueAfterSave = settings.ContinueEditing && settings.AllowContinueEditing;
                            docobj.AllowContinueAfterSave = settings.AllowContinueEditing;
                            storeItem = docobj;
                            versionable = docobj;
                            duplication = docobj;
                            lockable = docobj.GetStorageProvider().GetLockableItem(docobj);
                            version = (DocumentVersion)versionable.GetLatestVersion();
                            versionable.SetWorkingVersion(version);
                            if (settings.FolderGuid != Guid.Empty)
                            {
                                docobj.FolderGUID = settings.FolderGuid;
                            }

                            docobj.DocProgType = GetRegisteredApplication(obj, storeItem.Extension);

                            string preview = ExtractPreview(obj);
                            version.Preview = preview;
                            version.GenerateChecksum(preview);

                            if (settings.SkipTimeRecords)
                            {
                                docobj.TimeRecords.SkipTime = true;
                            }

                            BeforeDocumentSave(obj, docobj, version);

                            //Moved out of the following if statement to ensure that settings are merged on quick save
                            if (settings.StorageSettings.Count > 0)
                            {
                                StorageSettingsCollection coll = docobj.GetSettings();
                                if (coll == null)
                                    coll = docobj.GetStorageProvider().GetDefaultSettings(docobj, SettingsType.Store);

                                coll.Merge(settings.StorageSettings);

                                docobj.ApplySettings(coll);
                            }

                            //Added for shell files saved into MatterCentre for consistency to an email - using CreationTime as requested by ADW for Rouse      
                            if (obj.GetType().UnderlyingSystemType.FullName.ToUpper() == "FWBS.OMS.UI.Windows.ShellFile".ToUpper())
                            {
                                System.IO.FileInfo fi = ((FWBS.OMS.UI.Windows.ShellFile)obj).File;
                                docobj.AuthoredDate = fi.CreationTime;
                            }

                            if ((settings.Mode | PrecSaveMode.Save) == settings.Mode)
                            {
                                //Additional text.
                                if (AdditionalText(obj) == false)
                                {
                                    return DocSaveStatus.Cancel;
                                }

                                //Only allow the the duplication process if the wizard is to be shown.
                                duplication.AllowDuplication = true;

                                if (!FWBS.OMS.UI.Windows.Services.Wizards.SaveDocument(ActiveWindow, ref version))
                                {
                                    return DocSaveStatus.Cancel;
                                }
                            }
                            else
                            {
                                try
                                {
                                    docobj.Update();
                                    docobj.GetStorageProvider().ValidateStoreSettings(docobj);
                                }
                                catch (Exception ex)
                                {
                                    var ap = ex as EnquiryEngine.IAssociatedEnquiryPage;

                                    if (ex is StorageItemDuplicatedException)
                                    {
                                        //If the document is to be attached then add the document properties
                                        //to the document and finish the save without creating a document.
                                        switch (Session.CurrentSession.DuplicateDocumentAction)
                                        {
                                            case "ATTACH":
                                                {
                                                    Document duplicate = duplication.CheckForDuplicate() as Document;
                                                    if (duplicate == null)
                                                        goto case "PROMPT";
                                                    else
                                                    {
                                                        docobj = duplicate;

                                                        docobj.ContinueAfterSave = settings.ContinueEditing && settings.AllowContinueEditing;
                                                        docobj.AllowContinueAfterSave = settings.AllowContinueEditing;
                                                        storeItem = docobj;
                                                        versionable = docobj;
                                                        duplication = docobj;
                                                        lockable = docobj.GetStorageProvider().GetLockableItem(docobj);
                                                        version = (DocumentVersion)versionable.GetLatestVersion();
                                                        versionable.SetWorkingVersion(version);
                                                        internalSave = false;
                                                        goto AfterSave;
                                                    }
                                                }

                                            case "NODUPLICATION":
                                                {
                                                    return DocSaveStatus.Error;
                                                }

                                            case "PROMPT":
                                                {

                                                }
                                                break;
                                            default:
                                                goto case "PROMPT";
                                        }
                                    }

                                    if (settings.ContinueOnError == false)
                                    {
                                        //Additional text.
                                        if (AdditionalText(obj) == false)
                                        {
                                            return DocSaveStatus.Cancel;
                                        }

                                        //Only allow the the duplication process if the wizard is to be shown.
                                        duplication.AllowDuplication = true;
                                        if (!FWBS.OMS.UI.Windows.Services.Wizards.SaveDocument(ActiveWindow, ap == null ? null : ap.PageName, ref docobj))
                                        {
                                            return DocSaveStatus.Cancel;
                                        }
                                    }
                                    else
                                        return DocSaveStatus.Error;
                                }
                            }

                        AfterSave:

                            if (docobj.ID != GetDocVariable(obj, DOCUMENT, -1))
                                SetDocVariable(obj, DOCUMENT, docobj.ID);

                            //Register the document into the storage provider before
                            //committing a full store.  This allows an external document
                            //id to be stored inside the document object.
                            StorageProvider sp = docobj.GetStorageProvider();
                            if (sp.Supports(StorageFeature.Register, docobj))
                            {
                                RegisterResult result = sp.Register(docobj, obj, true);
                                docobj.ExternalId = result.ExternalId;
                            }
                            SetDocVariable(obj, DOCUMENT_EXTERNAL, docobj.ExternalId);


                            //Set the document key.
                            if (Session.CurrentSession.EmailChecksum)
                                SetDocKey(obj, GenerateDocKey(obj));


                            //Assign the retention policy information if any.
                            RetentionPolicy policy = docobj.GetRetentionPolicy();
                            if (policy.IsValid)
                            {
                                SetDocVariable(obj, RETENTION_POLICY, policy.Name);
                                SetDocVariable(obj, RETENTION_PERIOD, policy.Period);
                            }


                            AttachDocumentVars(obj, docobj, version);



                            try
                            {
                                versionable.NewVersion += new EventHandler<NewVersionEventArgs>(OnNewVersion);
                                if (internalSave)
                                {
                                    if (CanRefreshDocumentFields(obj))
                                    {
                                        RefreshDocumentFields(obj, version);
                                    }
                                    InternalDocumentSave(obj, settings.Mode, settings.Printing.Mode, docobj, version);
                                    //Call doc saved

                                    docobj.PhysicalDocumentSaved();

                                }
                            }
                            finally
                            {
                                versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(OnNewVersion);
                            }

                            if (_multisave == false)
                            {
                                if (this._application.AutoPrint == true)
                                    if (settings.Printing.Mode != PrecPrintMode.None)
                                        BeginPrint(new object[1] { obj }, settings.Printing);
                            }

                            EndDocumentSave(obj, settings.Mode, settings.Printing.Mode, docobj, version);

                            FWBS.OMS.Interfaces.OMSAppSavedEventArgs e = new OMSAppSavedEventArgs(docobj, obj);
                            OnSaved(e);

                            try
                            {
                                if (!docobj.ContinueAfterSave || !docobj.AllowContinueAfterSave)
                                    Close(obj);
                                else
                                {
                                    string caption = "";
                                    version = GetCurrentDocumentVersion(obj);
                                    if (version != null)
                                    {
                                        caption = version.DisplayID + " - " + version.ParentDocument.Description;
                                    }
                                    else if (docobj != null)
                                    {
                                        caption = docobj.DisplayID + " - " + docobj.Description;
                                    }

                                    if (!string.IsNullOrEmpty(caption))
                                        SetWindowCaption(obj, caption);
                                }
                            }
                            catch { }

                            if (_multisave == false && settings.AllowMove && settings.Move) Move(obj);
                        }
                        else
                            return DocSaveStatus.Error;
                    }

                }
                else
                {
                    //Make sure the existing documents associate has not been changed.
                    Associate assocobj = GetCurrentAssociate(obj);

                    if (assocobj == null)
                        throw new ArgumentNullException("assocobj", Session.CurrentSession.Resources.GetMessage("MSGASCDNTEXT", "The associate of the current document does not exist.", "").Text);

                    docobj = GetCurrentDocument(obj);
                    if (docobj == null)
                        return DocSaveStatus.Error;

                    // Check Security on File to see if a new Document can be Updated
                    new DocumentPermission(docobj, StandardPermissionType.Update).Check();

                    //Client and Matter Status check
                    new FileActivity(assocobj.OMSFile, FileStatusActivityType.DocumentModification).Check();


                    docobj.ContinueAfterSave = settings.ContinueEditing && settings.AllowContinueEditing;
                    docobj.AllowContinueAfterSave = settings.AllowContinueEditing;
                    storeItem = docobj;
                    versionable = docobj;
                    duplication = docobj;

                    version = GetCurrentDocumentVersion(obj);
                    if (version == null)
                        version = (DocumentVersion)versionable.GetLatestVersion();

                    if (version == null)
                        lockable = docobj.GetStorageProvider().GetLockableItem(docobj);
                    else
                        lockable = docobj.GetStorageProvider().GetLockableItem(version);

                    if (settings.StorageSettings.Count > 0)
                    {
                        IStorageItem storageitem = null;
                        if (version == null)
                            storageitem = docobj;
                        else
                            storageitem = version;

                        StorageSettingsCollection settingsColl = storageitem.GetSettings();

                        if (settingsColl == null)
                            settingsColl = storageitem.GetStorageProvider().GetDefaultSettings(storageitem, SettingsType.Store);

                        settingsColl.Merge(settings.StorageSettings);
                        docobj.ApplySettings(settingsColl);
                    }

                    if (lockable.IsCheckedOutByAnother)
                    {
                        lockable.CheckOut(null);
                        return DocSaveStatus.Error;
                    }


                    //Check for any changes.
                    if (storeItem.IsConflicting)
                    {
                        IStorageItem conflict = storeItem.GetConflict();

                        if (CanCompare)
                        {
                            if (MessageBox.ShowYesNoQuestion("QDOCCONFLICT", "The current document has been changed since you last opened it, would you like to compare?") == DialogResult.Yes)
                            {
                                CompareDocument(obj, storeItem);
                                return DocSaveStatus.Cancel;
                            }

                        }
                        else
                        {
                            if (MessageBox.ShowYesNoQuestion("QDOCCONFLICT2", "The current document has been changed since you last opened it, would you like to view and manually compare?") == DialogResult.Yes)
                            {
                                OpenSettings oset = OpenSettings.Default;
                                oset.Mode = DocOpenMode.View;
                                Open(conflict, oset, Common.TriState.True);
                                return DocSaveStatus.Cancel;
                            }
                        }
                    }

                    versionable.SetWorkingVersion(version);
                    string preview = ExtractPreview(obj);
                    version.Preview = preview;
                    version.GenerateChecksum(preview);

                    if (assocobj.ID != docobj.Associate.ID)
                        docobj.ChangeAssociate(assocobj);

                    BeforeDocumentSave(obj, docobj, version);

                    SetMaxRevisionCountOnDocument(version);

                    if((settings.Mode | PrecSaveMode.Save) == settings.Mode || HasRevisionWarning(docobj))
                    {
                        //Additional text.
                        if (AdditionalText(obj) == false)
                        {
                            return DocSaveStatus.Cancel;
                        }

                        if (!FWBS.OMS.UI.Windows.Services.Wizards.SaveDocument(ActiveWindow, ref version))
                        {
                            return DocSaveStatus.Cancel;
                        }
                    }
                    else
                    {
                        try
                        {
                            docobj.Update();
                        }
                        catch (Exception)
                        {

                            if (settings.ContinueOnError == false)
                            {
                                //Additional text.
                                if (AdditionalText(obj) == false)
                                {
                                    return DocSaveStatus.Cancel;
                                }

                                //Only allow the the duplication process if the wizard is to be shown.
                                duplication.AllowDuplication = true;
                                if (!FWBS.OMS.UI.Windows.Services.Wizards.SaveDocument(ActiveWindow, ref version))
                                {
                                    return DocSaveStatus.Cancel;
                                }
                            }
                            else
                                return DocSaveStatus.Error;
                        }
                    }

                    AttachDocumentVars(obj, docobj, version);

                    try
                    {
                        versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(OnNewVersion);
                        versionable.NewVersion += new EventHandler<NewVersionEventArgs>(OnNewVersion);
                        if (CanRefreshDocumentFields(obj))
                        {
                            RefreshDocumentFields(obj, version);
                        }

                        if (docobj.GetSettings() != null && docobj.GetSettings().Count == 0)
                        {
                            docobj.ClearSettings();
                        }

                        VersionStoreSettings currentdocver = null;

                        if (docobj.GetSettings() != null)
                        {
                            currentdocver = docobj.GetSettings().GetSettings<VersionStoreSettings>();
                        }

                        if (docobj.GetSettings() != null && currentdocver.SaveItemAs == VersionStoreSettings.StoreAs.NewSubVersion && docobj.ReachDocMaxRevisionCount)
                        {
                            Save(obj, SaveSettings.Default);
                        }
                        else if (docobj.GetSettings() != null && currentdocver.SaveItemAs != VersionStoreSettings.StoreAs.NewSubVersion && docobj.ReachDocMaxRevisionCount)
                        {
                            InternalDocumentSave(obj, settings.Mode, settings.Printing.Mode, docobj, version);
                        }
                        else if (docobj.GetSettings() == null && HasRevisionWarning(docobj))
                        {
                            settings.Printing.Mode = PrecPrintMode.None;
                            Save(obj);
                        }
                        else
                        {
                            InternalDocumentSave(obj, settings.Mode, settings.Printing.Mode , docobj, version);
                        }
                    }
                    finally
                    {
                        versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(OnNewVersion);
                    }

                    if (_application.AutoPrint)

                        if (settings.Printing.Mode != PrecPrintMode.None)
                            BeginPrint(new object[1] { obj }, settings.Printing);

                    EndDocumentSave(obj, settings.Mode, settings.Printing.Mode, docobj, version);

                    FWBS.OMS.Interfaces.OMSAppSavedEventArgs e = new OMSAppSavedEventArgs(docobj, obj);
                    OnSaved(e);

                    try
                    {
                        if (!docobj.ContinueAfterSave || !docobj.AllowContinueAfterSave)
                            Close(obj);
                        else
                        {
                            string caption = "";
                            version = GetCurrentDocumentVersion(obj);
                            if (version != null)
                            {
                                caption = version.DisplayID + " - " + version.ParentDocument.Description;
                            }
                            else if (docobj != null)
                            {
                                caption = docobj.DisplayID + " - " + docobj.Description;
                            }

                            if (!string.IsNullOrEmpty(caption))
                                SetWindowCaption(obj, caption);
                        }
                    }
                    catch { }
                }

                #endregion
            }
            else
            {
                #region "Begin Save - Precedent"

                Precedent precobj = null;
                IStorageItem storeItem = null;
                IStorageItemLockable lockable = null;
                IStorageItemVersionable versionable = null;
                IStorageItemDuplication duplication = null;
                PrecedentVersion version = null;

                CheckSavePrecedent(obj, precobj);

                if (IsCompanyDocument(obj) == false || GetDocVariable(obj, PRECEDENT, 0) == 0)
                {
                    if (settings.Mode != PrecSaveMode.None)
                    {
                        //Make sure that the user has permissions to edit a precedent
                        string type = GetActiveDocType(obj);
                        if (type == "") type = DefaultPrecedentType;
                        precobj = new FWBS.OMS.Precedent("", type, "", "", -1);
                        precobj.PrecProgType = GetRegisteredApplication(obj, ((IStorageItem)precobj).Extension);
                        precobj.PrecedentPreview = ExtractPreview(obj);
                        precobj.TextOnly = GetDocVariable(obj, TEXTONLY, false);

                        if (this is FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)
                        {
                            precobj.AllowSecondStageMerge = ((FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)this).IsSecondStageMergeDoc(obj);
                        }

                        if (Session.CurrentSession.EnablePrecedentVersioning)
                        {
                            storeItem = precobj;
                            versionable = precobj;
                            duplication = precobj;
                            lockable = precobj.GetStorageProvider().GetLockableItem(precobj);
                            version = (PrecedentVersion)versionable.GetLatestVersion();
                            versionable.SetWorkingVersion(version);

                            if (version == null)
                                BeforePrecedentSave(obj, precobj);
                            else
                                BeforePrecedentSave(obj, precobj, version);
                        }
                        else
                        {
                            BeforePrecedentSave(obj, precobj);
                        }


                        if (!FWBS.OMS.UI.Windows.Services.Wizards.SavePrecedent(ref precobj, this))
                        {
                            return DocSaveStatus.Cancel;
                        }

                        precobj.Update();
                        AttachPrecedentVars(obj, precobj);

                        if (Session.CurrentSession.EnablePrecedentVersioning)
                        {
                            InternalPrecedentVersionSave(obj, settings, precobj, versionable, version);
                        }
                        else
                        {
                            InternalPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj);
                        }

                        EndPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj);

                        FWBS.OMS.Interfaces.OMSAppSavedEventArgs e = new OMSAppSavedEventArgs(precobj, obj);
                        OnSaved(e);

                        CheckPrecedentLocking(precobj);

                        if (Session.CurrentSession.EnablePrecedentVersioning)
                        {
                            SetPrecedentVersionWindowCaption(obj, precobj, version);
                        }
                    }
                }
                else
                {
                    precobj = FWBS.OMS.Precedent.GetPrecedent(GetDocVariable(obj, PRECEDENT, 0));

                    CheckSavePrecedent(obj, precobj);

                    precobj.PrecedentPreview = ExtractPreview(obj);

                    if (Session.CurrentSession.EnablePrecedentVersioning)
                    {
                        storeItem = precobj;
                        versionable = precobj;
                        duplication = precobj;
                        lockable = precobj.GetStorageProvider().GetLockableItem(precobj);

                        version = GetCurrentPrecedentVersion(obj);
                        if (version == null)
                            version = (PrecedentVersion)versionable.GetLatestVersion();

                        versionable.SetWorkingVersion(version);

                        string preview = ExtractPreview(obj);
                        version.Preview = preview;
                        version.GenerateChecksum(preview);

                        if (version == null)
                            BeforePrecedentSave(obj, precobj);
                        else
                            BeforePrecedentSave(obj, precobj, version);
                    }
                    else
                    {
                        BeforePrecedentSave(obj, precobj);
                    }


                    if ((settings.Mode | PrecSaveMode.Save) == settings.Mode)
                    {
                        if (!FWBS.OMS.UI.Windows.Services.Wizards.SavePrecedent(ref precobj, this))
                            return DocSaveStatus.Cancel;
                    }

                    precobj.Update();
                    AttachPrecedentVars(obj, precobj);

                    if (Session.CurrentSession.EnablePrecedentVersioning)
                    {
                        InternalPrecedentVersionSave(obj, settings, precobj, versionable, version);
                        EndPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj, version);
                    }
                    else
                    {
                        InternalPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj);
                        EndPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj);
                    }


                    FWBS.OMS.Interfaces.OMSAppSavedEventArgs e = new OMSAppSavedEventArgs(precobj, obj);
                    OnSaved(e);


                    if (Session.CurrentSession.EnablePrecedentVersioning)
                    {
                        SetPrecedentVersionWindowCaption(obj, precobj, version);
                    }
                }

                #endregion
            }

            return DocSaveStatus.Success;
        }

        private static bool HasRevisionWarning(OMSDocument docobj)
        {
            return docobj.ReachDocMaxRevisionCount && Session.CurrentSession.DocumentVersioning == "V";
        }

        private static void SetMaxRevisionCountOnDocument(DocumentVersion version)
        {
            if (version.Label.Split('.').Length - 1 >= FWBS.OMS.Session.CurrentSession.DocumentMaximumRevisionCount)
            {
                version.ParentDocument.ReachDocMaxRevisionCount = true;
            }
            else
            {
                version.ParentDocument.ReachDocMaxRevisionCount = false;
            }
        }


        private bool CheckPrecedentLocking(Precedent prec)
        {
            string appCode = GetAppCode(prec);
            string precID = Convert.ToString(prec.ID);
            if (appCode == "OUTLOOK" || appCode == "WORD" || appCode == "EXCEL")
            {
                if (Session.CurrentSession.ObjectLocking)
                {
                    LockState ls = new LockState();
                    if (ls.CheckObjectLockState(precID, LockableObjects.Precedent))
                        return true;
                    else
                    {
                        ls.LockPrecedentObject(precID);
                        ls.MarkObjectAsOpen(precID, LockableObjects.Precedent);
                    }
                }
            }
            return false;
        }

        private string GetAppCode(Precedent prec)
        {
            Apps.RegisteredApplication app = prec.PrecProgType;
            if (app.Code == "SHELL")
            {
                FWBS.OMS.DocumentManagement.Storage.IStorageItem item = prec;
                app = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(item.Extension);
                if (app == null)
                    app = prec.PrecProgType;
                else
                {
                    prec.PrecProgType = app;
                    prec.Update();
                }
            }
            return prec.PrecProgType.Code;
        }

        #region "Precedent version window caption"

        private void SetPrecedentVersionWindowCaption(object obj, Precedent precobj, PrecedentVersion version)
        {
            try
            {
                version = GetCurrentPrecedentVersion(obj);
                SetWindowCaption(obj, precobj, version);
            }
            catch { }
        }

        private void SetWindowCaption(object obj, Precedent precobj, PrecedentVersion version)
        {
            string caption = GetPrecedentVersionCaption(precobj, version);

            if (!string.IsNullOrEmpty(caption))
                SetWindowCaption(obj, caption);
        }

        private static string GetPrecedentVersionCaption(Precedent precobj, PrecedentVersion version)
        {
            string caption = "";
            if (version != null)
            {
                caption = version.DisplayID + " - " + version.ParentDocument.Description;
            }
            else if (precobj != null)
            {
                caption = precobj.DisplayID + " - " + precobj.Description;
            }
            return caption;
        }

        #endregion

        private void InternalPrecedentVersionSave(object obj, SaveSettings settings, Precedent precobj, IStorageItemVersionable versionable, PrecedentVersion version)
        {
            try
            {
                versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(OnNewPrecedentVersion);
                versionable.NewVersion += new EventHandler<NewVersionEventArgs>(OnNewPrecedentVersion);

                if (version == null)
                    InternalPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj);
                else
                {
                    AttachPrecedentVersionVars(obj, precobj, version);
                    InternalPrecedentSave(obj, settings.Mode, settings.Printing.Mode, precobj, version);
                }
            }
            finally
            {
                versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(OnNewPrecedentVersion);
            }
        }


        private void CheckSavePrecedent(object obj, Precedent prec)
        {
            new SystemPermission(StandardPermissionType.UpdatePrecedent).Check();
            if (prec != null)
                new PrecedentPermission(prec, StandardPermissionType.UpdatePrecedent).Check();

            if (IsReadOnly(obj))
                throw new OMSException2("SAVEREADONLYDOC", "Cannot save a read only document.", "");
        }

        protected void OnNewVersion(object sender, NewVersionEventArgs e)
        {
            object current = e.Tag;
            if (current == null)
                current = this;
            DocumentVersion version = (DocumentVersion)e.Version;
            Document document = (Document)e.Version.BaseStorageItem;
            Document currentdoc = GetCurrentDocument(current);
            if (currentdoc.ID == document.ID)
            {
                AttachDocumentVars(current, document, version);
                //In case VERSIONID or VERSIONLABEL is used as a field then relink fields
                RefreshDocumentFields(current, version);
                ScreenRefresh();

                //If the content of the document has changed then re-applythe preview and checksum
                string preview = ExtractPreview(current);
                version.Preview = preview;
                version.GenerateChecksum(preview);

                InternalOnNewVersion(current, e);
            }

        }


        protected void OnNewPrecedentVersion(object sender, NewVersionEventArgs e)
        {
            object current = e.Tag;

            if (current == null)
                current = this;

            PrecedentVersion version = (PrecedentVersion)e.Version;
            Precedent prec = (Precedent)e.Version.BaseStorageItem;
            Precedent currentprec = GetCurrentPrecedent(current);

            if (currentprec.ID == prec.ID)
            {
                AttachPrecedentVars(current, prec);
                AttachPrecedentVersionVars(current, prec, version);
                ScreenRefresh();

                //If the content of the precedent has changed then re-apply the preview and checksum
                string preview = ExtractPreview(current);
                version.Preview = preview;
                version.GenerateChecksum(preview);

                InternalOnNewVersion(current, e);
            }
        }


        protected virtual void InternalOnNewVersion(object current, NewVersionEventArgs args)
        {
            //Resave the document to the 
            InternalSave(current, args.Version);
        }

        /// <summary>
        /// Runs the end part of the save routine.
        /// </summary>
        /// <param name="obj">The object to be saved.</param>
        protected virtual void EndDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version)
        {
        }

        /// <summary>
        /// Runs the end part of the save routine.
        /// </summary>
        /// <param name="obj">The object to be saved.</param>
        protected virtual void EndPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec)
        {
        }

        protected virtual void EndPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec, PrecedentVersion version)
        {
        }


        /// <summary>
        /// Saves a specific object to OMS using specified print and save settings.
        /// </summary>
        protected abstract void InternalDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version);

        protected abstract void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec);

        protected abstract void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec, PrecedentVersion version);


        /// <summary>
        /// Like InternalSave but flags the document as unsaved rather than saving it.
        /// </summary>
        /// <param name="obj"></param>
        protected void InternalSave(object obj, IStorageItem item)
        {
            InternalSave(obj, item, true);
        }

        protected void InternalSave(object obj, IStorageItem item, bool acceptChangesIfNew)
        {
            //DM - 27/03/07 - This code was added bacause the document was being modified
            //without the user realising making the local document think it has been changed.

            if (item == null)
            {
                try
                {
                    item = GetCurrentDocumentVersion(obj);
                    if (item == null)
                        item = GetCurrentDocument(obj);
                    if (item == null)
                        item = GetCurrentPrecedent(obj);
                    if (item == null)
                        item = GetCurrentPrecedentVersion(obj);
                }
                catch { }
            }

            DateTime? date;
            System.IO.FileInfo localfile = null;
            if (item != null)
                localfile = StorageManager.CurrentManager.LocalDocuments.GetLocalFile(item, out date);

            date = null;

            if (localfile != null)
            {
                date = localfile.LastAccessTimeUtc;
            }

            InternalSave(obj, acceptChangesIfNew);

            if (localfile != null)
            {
                localfile.Refresh();
                if (date < localfile.LastAccessTimeUtc)
                    StorageManager.CurrentManager.LocalDocuments.Set(item, localfile, true);
            }

        }

        public sealed override void LocalSave(object obj)
        {
            InternalSave(obj, false);
        }

        /// <summary>
        /// Performs an internal save routine which will save a document quickly without any OMS intervention.
        /// </summary>
        /// <param name="obj">The document object.</param>
        [Obsolete("Please override InternalSave(obj, bool) instead. Thank you")]
        protected virtual void InternalSave(object obj)
        {
        }

        /// <summary>
        /// Performs an internal save routine which will save a document quickly without any OMS intervention.
        /// </summary>
        /// <param name="obj">The document object.</param>
        /// <param name="createFileIfNew">Does not save a new file as a temp but just flags the document as saved.</param>
        protected virtual void InternalSave(object obj, bool createFileIfNew)
        {
            InternalSave(obj);
        }

        /// <summary>
        /// Allows the OMS application to set default values for a storage item.
        /// </summary>
        protected virtual void BeforeDocumentSave(object obj, Document doc, DocumentVersion version)
        {
        }

        protected virtual void BeforePrecedentSave(object obj, Precedent prec)
        {
        }

        protected virtual void BeforePrecedentSave(object obj, Precedent prec, PrecedentVersion version)
        {
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// Moves a document to another location.  Mainly used for Outlook OMAApp.
        /// </summary>
        protected virtual void Move(object obj)
        {
        }

        /// <summary>
        /// Fetches sub documents for the current document.
        /// </summary>
        /// <param name="obj">The document object being described.</param>
        protected virtual SubDocument[] AttachSubDocuments(object obj)
        {
            return null;
        }

        /// <summary>
        /// Generates a default document description.
        /// </summary>
        /// <param name="obj">The document object being described.</param>
        /// <param name="direction">The direction that the document id going, inward, outward.</param>
        /// <param name="info">The client / associate that the document is.</param>
        /// <returns>A default document description.</returns>
        protected virtual string GenerateDocDesc(object obj)
        {
            if (obj == null)
                return "";
            CheckObjectIsDoc(ref obj);
            return "";
        }


        /// <summary>
        /// Gets the default direction of the document.
        /// </summary>
        /// <param name="obj">The document object being queried.</param>
        /// <returns>The direction enumeration member.</returns>
        public FWBS.OMS.DocumentDirection GetDocDirection(object obj, Precedent precedent)
        {
            return GetActiveDocDirection(obj, precedent);
        }

        protected virtual FWBS.OMS.DocumentDirection GetActiveDocDirection(object obj, Precedent precedent)
        {
            if (precedent == null)
                return DocumentDirection.Out;
            else
                return precedent.DefaultDirection;
        }


        /// <summary>
        /// Gets the document extension of the active document.
        /// </summary>
        /// <param name="obj">The active document object.</param>
        /// <returns>The file extension to use or empty string if the default one is to be used.</returns>
        public virtual string GetDocExtension(object obj)
        {
            return "";
        }

        /// <summary>
        /// Gets the document storage location of the active document.
        /// </summary>
        /// <param name="obj">The active document object.</param>
        /// <returns>The storage location id, or -1 if the default one is to be used.</returns>
        public virtual short GetDocStorageLocation(object obj)
        {
            return -1;
        }

        protected virtual Apps.RegisteredApplication GetRegisteredApplication(object obj, string extension)
        {
            return _application;
        }

        /// <summary>
        /// Gets the default precedent type when saving a new precedent.
        /// </summary>
        public override string DefaultPrecedentType
        {
            get
            {
                string type = DefaultDocType;
                if (type == String.Empty)
                    type = GetActiveDocType(this);
                return type;
            }
        }

        public bool ShowFaxOptions(System.Windows.Forms.IWin32Window owner, object obj)
        {
            frmFax frmfaxdlg;
            Associate assoc = GetCurrentAssociate(obj);
            if (assoc != null)
            {
                frmfaxdlg = new frmFax(assoc);

                string name = assoc.Addressee;
                if (String.IsNullOrEmpty(name))
                    name = assoc.Contact.Addressee;
                if (string.IsNullOrEmpty(name))
                    name = assoc.Contact.Name;

                frmfaxdlg.FaxTo = GetDocVariable(obj, "FAXTO", name);
                frmfaxdlg.FaxNumber = GetDocVariable(obj, "FAXNUMBER", assoc.DefaultFaxNumber);
            }
            else
            {
                frmfaxdlg = new frmFax();
                frmfaxdlg.FaxTo = GetDocVariable(obj, "FAXTO", "");
                frmfaxdlg.FaxNumber = GetDocVariable(obj, "FAXNUMBER", "");
            }

            if (Session.CurrentSession.IsLicensedFor("FAX"))
                frmfaxdlg.FaxTime = GetDocVariable(obj, "FAXTIME", "NOW");
            else
                frmfaxdlg.FaxTime = GetDocVariable(obj, "FAXTIME", "DONTFAX");

            frmfaxdlg.FaxCompName = GetDocVariable(obj, "FAXCOMPNAME", "");

            frmfaxdlg.ShowDialog(owner);

            if (frmfaxdlg.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SetDocVariable(obj, "FAXTIME", frmfaxdlg.FaxTime);
                SetDocVariable(obj, "FAXTO", frmfaxdlg.FaxTo);
                SetDocVariable(obj, "FAXCOMPNAME", frmfaxdlg.FaxCompName);
                SetDocVariable(obj, "FAXNUMBER", frmfaxdlg.FaxNumber);
                frmfaxdlg.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sends the document to the associates(s).
        /// </summary>
        public void SendDocViaEmail(object obj, System.IO.FileInfo filePath, bool asPDF = false)
        {

            IStorageItem item = obj as IStorageItem;

            if (item != null)
                SendDocViaEmail(item as IStorageItem, filePath, false, asPDF);
            else
            {

                item = GetCurrentDocumentVersion(obj);
                if (item == null)
                    item = GetCurrentDocument(obj);
                if (item == null)
                    item = GetCurrentPrecedent(obj);

                if (item != null)
                {
                    AttachOutboundDocumentVars(obj, item);
                    InternalSave(obj, null);
                    SendDocViaEmail(item, filePath, false, asPDF);
                }
            }
        }



        public void SendDocViaEmail(IStorageItem item, System.IO.FileInfo filePath, bool attachOutgoingVars, bool asPDF = false)
        {
            DocumentManagement.SendDocumentViaEmailCommand send = new FWBS.OMS.UI.Windows.DocumentManagement.SendDocumentViaEmailCommand();

            Document doc = item as Document;
            DocumentVersion version = item as DocumentVersion;
            if (version != null)
                doc = version.ParentDocument;

            send.EmailTemplate = Precedent.GetPrecedent("DOCSEND", "EMAIL", "", "SYSTEM", "", "");
            send.ToAssociate = doc.Associate;
            send.AdditionalCCs = true;
            send.DocumentsToAttach.Add(item);
            send.AsPDF = asPDF;
            send.Execute();

        }

        /// <summary>
        /// Sends the a file document for authorisation.
        /// </summary>
        public void SendDocForAuthorisation(object obj)
        {

            CheckObjectIsDoc(ref obj);

            IStorageItem item = GetCurrentDocumentVersion(obj);
            if (item == null)
                item = GetCurrentDocument(obj);

            FWBS.OMS.UI.Windows.DocumentManagement.SendToAuthoriseCommand cmd = new FWBS.OMS.UI.Windows.DocumentManagement.SendToAuthoriseCommand();
            cmd.AllowUI = true;
            cmd.ContinueOnError = true;
            cmd.Item = item;

            FWBS.OMS.Commands.ExecuteResult result = cmd.Execute();

            if (result.Status == FWBS.OMS.Commands.CommandStatus.Failed)
            {
                if (result.Errors.Count > 0)
                {
                    //to match original code, dont know what happens with this
                    if (result.Errors[0] is MailDisabledException)
                        throw result.Errors[0];

                    MessageBox.Show(result.Errors[0]);
                }
            }
        }


        /// <summary>
        /// Sets the current documents window caption.
        /// </summary>
        /// <param name="obj">The active document to manipulate.</param>
        /// <param name="caption">Caption string.</param>
        protected abstract void SetWindowCaption(object obj, string caption);

        /// <summary>
        /// Gets the current window caption with the associate / file and client ifnormation.
        /// </summary>
        /// <param name="obj">The active document to manipulate.</param>
        /// <returns>Caption text.</returns>
        public string GetWindowCaption(object obj)
        {
            Associate assoc = GetCurrentAssociate(obj);
            if (assoc != null)
                return Session.CurrentSession.Resources.GetResource("DOCUMENTTO", "Document to", "").Text + " " + assoc.Contact.GetOMSType().Description + " (" + assoc.OMSFile.Client.ClientNo + "/" + assoc.OMSFile.FileNo + ")";
            else
                return Session.CurrentSession.Resources.GetResource("DOCUMENTTO", "Document to", "").Text;
        }

        /// <summary>
        /// Activates the derived application.
        /// </summary>
        public override void ActivateApplication() { }

        /// <summary>
        /// Activates the current document.
        /// </summary>
        public override void ActivateDocument(object obj) { }

        public void ActivateWindow(IWin32Window win)
        {
            if (win != null)
            {
                //Im am adduming the following line is needed because....
                //the MainWindow may have focus due to being the parent of the 
                //previous form / dialog that was opened.
                //The Main window needs to be owned by the active application 
                //window
                Common.Functions.SetParentWindow(win, Services.MainWindow);
                FWBS.Common.Functions.SetForegroundWindow(win.Handle);
            }
            else
                ActivateApplication();
        }


        /// <summary>
        /// Performs a screen refresh.
        /// </summary>
        protected override void ScreenRefresh() { }

        /// <summary>
        /// Positions the cursor on the derived application current document.
        /// </summary>
        protected virtual void PositionCursor() { }





        /// <summary>
        /// Selects an associate or default associate.
        /// </summary>
        protected override Associate SelectAssociate(object obj, bool useDefaultAssociate, Associate assoc)
        {
            if (assoc == null)
            {
                //Check a refering document first.
                Client clienttoalert = null;
                if (obj != null)
                {
                    Document ref_doc = ResolveDocKey(GetDocKey(obj));
                    if (ref_doc != null)
                    {
                        assoc = ref_doc.Associate;
                        clienttoalert = assoc.OMSFile.Client;
                        Session.CurrentSession.CurrentFiles.Add(assoc.OMSFile.ID.ToString(), assoc.OMSFile);
                        Session.CurrentSession.CurrentAssociates.Add(assoc.ID.ToString(), assoc);
                        clienttoalert.AddAlert(new Alert(Session.CurrentSession.Resources.GetMessage("DOCKEYRESOLVED", "NOTE: The %CLIENT%/%FILE% has been gathered by the current document/email", "").Text, Alert.AlertStatus.Green));
                    }
                }

                if (useDefaultAssociate)
                {
                    assoc = Services.SelectDefaultAssociate(ActiveWindow);
                }
                else
                    assoc = Services.SelectAssociate(ActiveWindow);

                if (clienttoalert != null)
                    clienttoalert.ClearAlerts();
            }

            return assoc;
        }



        /// <summary>
        /// Displays the wait cursor.
        /// </summary>
        protected void StartTask()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        }

        /// <summary>
        /// Displays the wait cursor.
        /// </summary>
        protected void StopTask()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }


        #endregion

        #region Progress

        private frmProgress _prog = new frmProgress();


        protected void OnProgressStart(string caption, string label, int max)
        {
            OnProgressStart(caption, label, max, false);
        }

        protected void OnProgressStart(string caption, string label, int max, bool canCancel)
        {
            if (max > 0)
            {
                if (_prog == null) _prog = new frmProgress();
                caption = (caption ?? string.Empty) + " - ({0}%)";
                _prog.Owner = Services.MainWindow as Form;
                _prog.CanCancel = canCancel;
                FWBS.Common.Functions.SetParentWindow(ActiveWindow, Services.MainWindow);
                _prog.ProgressBar.Maximum = max;
                _prog.Text = string.Format(caption, 0);
                _prog.Tag = caption;
                _prog.Show();
            }

        }

        protected void OnProgress(string text, float iteration)
        {
            bool cancel;
            OnProgress(text, iteration, out cancel);
        }

        protected void OnProgress(string text, float iteration, out bool cancelled)
        {
            cancelled = false;
            Application.DoEvents();
            if (_prog != null)
            {
                _prog.Text = String.Format(Convert.ToString(_prog.Tag), Convert.ToInt32(((iteration / (float)_prog.ProgressBar.Maximum) * 100)));
                _prog.Label = text;
                _prog.ProgressBar.Value = Convert.ToInt32(iteration);
                cancelled = _prog.Cancel;
            }
            Application.DoEvents();
        }

        protected void OnProgressFinished()
        {
            if (_prog != null)
            {
                _prog.Close();
                _prog.Dispose();
                _prog = null;
            }
        }

        #endregion

        #region IOMSApp Implementation


        /// <summary>
        /// Goes to the next stop code within the current document.
        /// </summary>
        /// <param name="startAtTop">Starts at the top if specified.</param>
        public virtual void GotoNextStopCode(bool startAtTop)
        { }


        #endregion

        #region Opening Routines

        /// <summary>
        /// Opens a file from a file name.
        /// </summary>
        /// <param name="file"></param>
        public override object Open(System.IO.FileInfo file)
        {
            object ret = OpenFile(file);
            _activeDoc = ret;
            return ret;
        }

        /// <summary>
        /// Opens a document based on the document object passed.
        /// </summary>
        /// <param name="document">The document to be opened.</param>
        /// <param name="mode">The open mode of the document.</param>
        /// <returns>The document object that has been opened.</returns>
        public override object Open(Document document, DocOpenMode mode)
        {
            if (document == null)
                throw new ArgumentNullException("document");


            OpenSettings settings = OpenSettings.Default;
            settings.Mode = mode;
            return Open((IStorageItem)document, settings, Common.TriState.Null);
        }

        public override object Open(Document document, DocOpenMode mode, bool bulkprint, int noofcopies)
        {
            if (document == null)
                throw new ArgumentNullException("document");


            OpenSettings settings = OpenSettings.Default;
            settings.Mode = mode;
            settings.Printing.BulkPrintMode = true;
            settings.Printing.CopiesToPrint = noofcopies;

            return Open((IStorageItem)document, settings, Common.TriState.Null);
        }


        public override object Open(DocumentVersion version, DocOpenMode mode)
        {
            if (version == null)
                throw new ArgumentNullException("version");


            OpenSettings settings = OpenSettings.Default;
            settings.Mode = mode;
            return Open((IStorageItem)version, settings, Common.TriState.Null);
        }

        public object Open(DocumentVersion version, DocOpenMode mode, Common.TriState force)
        {
            if (version == null)
                throw new ArgumentNullException("version");

            OpenSettings settings = OpenSettings.Default;
            settings.Mode = mode;
            return Open((IStorageItem)version, settings, force);

        }

        protected virtual FetchResults InternalFetchItem(IStorageItem item, OpenSettings openSettings, Common.TriState force)
        {
            StorageProvider provider = item.GetStorageProvider();
            StorageSettingsCollection settings = item.GetSettings();
            if (settings == null)
                settings = provider.GetDefaultSettings(item, SettingsType.Fetch);
            LockableFetchSettings locksettings = settings.GetSettings<LockableFetchSettings>();
            VersionFetchSettings versettings = settings.GetSettings<VersionFetchSettings>();

            if (openSettings.Mode == DocOpenMode.Edit)
            {
                if (!locksettings.CheckOut)
                    locksettings.CheckOut = provider.GetCheckoutOption(item);
            }

            locksettings.CheckOut = openSettings.CheckOut ?? locksettings.CheckOut;
            versettings.Version =
                openSettings.LatestVersion == null ?
                versettings.Version :
                (openSettings.LatestVersion.Value ? VersionFetchSettings.FetchAs.Latest : VersionFetchSettings.FetchAs.Current);


            return provider.Fetch(item, true, settings, force);
        }

        private object Open(IStorageItem item, OpenSettings settings, Common.TriState force)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            object ret = null;

            string caption = String.Empty;



            FetchResults results = InternalFetchItem(item, settings, force);

            if (results.HasChanged || results.NewerExists)
            {
                var sb = new System.Text.StringBuilder();

                var resloc = Session.CurrentSession.Resources.GetMessage("MSGDOCISLOCAL", "This is a local copy of the document.", "").Text;

                sb.AppendLine(resloc);

                if (results.HasChanged)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.AppendLine(Session.CurrentSession.Resources.GetMessage("LOCALDOCCHANGES", "It appears to have been modified and may differ to the one stored against the %FILE%.\r\nThe document will need to be resaved before any changes are available to other users.", "").Text);
                }
                if (results.NewerExists)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.AppendLine(Session.CurrentSession.Resources.GetMessage("MSGMODDOCEXISTS", "It also appears to have been modified on the server.", "").Text);
                }

                if (sb.Length > 0)
                {
                    MessageBox.ShowInformation(sb.ToString());
                }
            }


            Precedent precedent = results.Item as Precedent;
            Document document = results.Item as Document;
            DocumentVersion version = results.Item as DocumentVersion;
            PrecedentVersion precVersion = results.Item as PrecedentVersion;

            if (precedent == null && document == null && version == null && precVersion == null)
            {
                ret = Open(results.LocalFile);
            }
            else if (precedent != null)
            {
                ret = InternalPrecedentOpen(precedent, results, settings);
                if (ret != null && settings.Mode == DocOpenMode.Edit)
                {
                    //Make sure that the precedent id and flag is set when opened in edit mode.
                    AttachPrecedentVars(ret, precedent);
                }
                caption = GetPrecedentVersionCaption(precedent, precVersion);
            }
            else if (precVersion != null)
            {
                precedent = (Precedent)precVersion.BaseStorageItem;
                ((IStorageItemVersionable)precedent).SetWorkingVersion(precVersion);

                //Open Precedent
                ret = InternalPrecedentOpen(precedent, results, settings);
                if (ret != null && settings.Mode == DocOpenMode.Edit)
                {
                    if (precedent.ID != GetDocVariable(ret, PRECEDENT, -1))
                        SetDocVariable(ret, PRECEDENT, precedent.ID);

                    Guid verid;
                    try
                    {
                        verid = new Guid(GetDocVariable(ret, PRECEDENT_VERSION, Guid.Empty.ToString()));
                    }
                    catch
                    {
                        verid = Guid.Empty;
                    }
                    if (verid == Guid.Empty)
                    {
                        SetDocVariable(ret, PRECEDENT_VERSION, precVersion.Id.ToString());
                        SetDocVariable(ret, PRECEDENT_LABEL, precVersion.Label);
                    }

                    AttachPrecedentVars(ret, precedent);
                    AttachPrecedentVersionVars(ret, precedent, precVersion);
                    InternalSave(ret, precVersion);
                }
                caption = GetPrecedentVersionCaption(precedent, precVersion);
            }
            else
            {
                if (version != null)
                {
                    document = (Document)version.BaseStorageItem;
                }
                else if (document != null)
                {
                    version = results.Item as DocumentVersion;
                    if (version == null)
                        version = (DocumentVersion)((IStorageItemVersionable)document).GetLatestVersion();
                }

                ((IStorageItemVersionable)document).SetWorkingVersion(version);


                if ((Session.CurrentSession.CurrentUser.PromptDocAlreadyOpen == FWBS.Common.TriState.Null && Session.CurrentSession.PromptDocAlreadyOpen) || (Session.CurrentSession.CurrentUser.PromptDocAlreadyOpen == FWBS.Common.TriState.True))
                {
                    DataTable activities = ((IStorageItem)document).GetActivities();

                    DataView act = activities.DefaultView;

                    int subtractTime = Session.CurrentSession.CurrentUser.PromptDocAlreadyOpenTime;
                    if (subtractTime <= 0)
                        subtractTime = Session.CurrentSession.PromptDocAlreadyOpenTime;

                    if (subtractTime <= 0)
                        subtractTime = 4;

                    DateTime lookfrom = DateTime.Now.Subtract(TimeSpan.FromHours(subtractTime));
                    lookfrom = lookfrom.ToUniversalTime();
                    act.RowFilter = string.Format("logTime > '{0}' and usrID <> {1}", lookfrom, Session.CurrentSession.CurrentUser.ID);
                    act.Sort = "logTime DESC";

                    int rows = act.Count;

                    if (rows > 0)
                    {
                        if (rows > 5)
                            rows = 5;

                        System.Text.StringBuilder recentActivities = new System.Text.StringBuilder();

                        for (int i = 0; i < rows; i++)
                        {
                            recentActivities.AppendLine(BuildLogRow(document, act[i]));
                        }

                        MessageBox.ShowInformation(this.ActiveWindow, "DOCRECACTIV", "This document has had some recent activity:" + Environment.NewLine + "%1%", recentActivities.ToString());

                    }
                }

                ret = InternalDocumentOpen(document, results, settings);
                if (ret != null && settings.Mode == DocOpenMode.Edit)
                {
                    if (document.ID != GetDocVariable(ret, DOCUMENT, -1))
                        SetDocVariable(ret, DOCUMENT, document.ID);

                    //I don't think this following block is needed. I believe it may be done in AttachDocumentVars.
                    //
                    Guid verid;
                    try
                    {
                        verid = new Guid(GetDocVariable(ret, DOCUMENT_VERSION, Guid.Empty.ToString()));
                    }
                    catch
                    {
                        verid = Guid.Empty;
                    }
                    if (verid == Guid.Empty)
                    {
                        SetDocVariable(ret, DOCUMENT_VERSION, version.Id.ToString());
                        SetDocVariable(ret, DOCUMENT_LABEL, version.Label);
                    }
                    //

                    AttachDocumentVars(ret, document, version);

                    InternalSave(ret, version);
                }


                Associate assoc = GetCurrentAssociate(ret);

                if (assoc == null)
                    caption = results.Item.DisplayID;
                else
                {
                    caption = MaxCaptionLength(String.Format("{0}/{1} - {2} - {3}", assoc.OMSFile.Client.ClientNo, assoc.OMSFile.FileNo, results.Item.DisplayID, document.Description), string.Empty);
                }
            }
            _activeDoc = ret;

            //Set local copy caption
            if (results.IsLocalCopy)
            {
                ResourceItem res = Session.CurrentSession.Resources.GetResource("LOCALCOPY", "Local Copy", "");
                string append = String.Format(" - {0} @ '{1}'", res.Text, results.CachedDate.Value.ToLocalTime().ToString());
                caption = MaxCaptionLength(caption, append);
            }


            if (!string.IsNullOrEmpty(results.AdditionalCaptionText))
            {
                caption += " " + results.AdditionalCaptionText;
                caption = MaxCaptionLength(caption, string.Empty);
            }


            SetWindowCaption(ret, caption);

            if (document != null)
            {
                IDataParameter[] pars = new IDataParameter[2];
                pars[0] = Session.CurrentSession.CurrentConnection.CreateParameter("docid", document.ID);
                pars[1] = Session.CurrentSession.CurrentConnection.CreateParameter("Opened", DateTime.UtcNow);
                try
                {
                    Session.CurrentSession.CurrentConnection.ExecuteProcedure("sprUpdateDocumentLastOpened", pars);
                }
                catch (Exception)
                {                 
                }
            }
            return ret;

        }


        private static string MaxCaptionLength(string caption, string append)
        {
            if (!string.IsNullOrEmpty(caption) && string.IsNullOrEmpty(append) && caption.Length > 200)
            {
                caption = caption.Substring(0, 200);
            }

            else
                if ((!string.IsNullOrEmpty(caption) && (!string.IsNullOrEmpty(append)) && (caption.Length + append.Length) > 200))
            {
                caption = caption.Substring(0, 200 - append.Length);
                caption += append;

            }

            return caption;
        }


        private static string BuildLogRow(Document document, DataRowView view)
        {
            System.Text.StringBuilder recentActivity = new System.Text.StringBuilder();
            DateTime logTime = (DateTime)view["LogTime"];


            recentActivity.Append(logTime.ToLocalTime());
            recentActivity.Append("- ");

            if (view["VerID"] != DBNull.Value)
            {

                DocumentVersion vers = ((IStorageItemVersionable)document).GetVersion((Guid)view["VerID"]) as DocumentVersion;
                if (vers != null)
                {
                    recentActivity.Append("- Version ");
                    recentActivity.Append(vers.Version);
                    recentActivity.Append(" ");

                    if (!string.IsNullOrEmpty(vers.Comments))
                    {
                        recentActivity.Append("(");
                        recentActivity.Append(vers.Comments);
                        recentActivity.Append(")");
                    }
                }
            }

            string logType = Convert.ToString(view["logType"]);
            logType.Trim();
            recentActivity.Append(logType.ToLower());

            int userId = Convert.ToInt32(view["usrID"]);


            User loguser = User.GetUser(userId);

            if (loguser != null)
            {
                recentActivity.Append(" by ");
                recentActivity.Append(loguser.FullName);
            }

            return recentActivity.ToString();
        }


        /// <summary>
        /// Opens a file from a file name.
        /// </summary>
        /// <param name="file"></param>
        protected virtual object OpenFile(System.IO.FileInfo file)
        {
            System.Diagnostics.Process.Start(file.FullName);
            return file;
        }

        /// <summary>
        /// Opens a document based on the document object passed.
        /// </summary>
        /// <param name="document">The document to be opened.</param>
        /// <param name="mode">The open mode of the document.</param>
        /// <returns>The document object that has been opened.</returns>
        protected abstract object InternalDocumentOpen(Document document, FetchResults fetchData, OpenSettings settings);


        /// <summary>
        /// Opens a precedent based on the precedent object passed.
        /// </summary>
        /// <param name="precedent">The precedent to be opened.</param>
        /// <param name="mode">The open mode of the precedent.</param>
        /// <returns>The document object that has been opened.</returns>
        public override object Open(Precedent precedent, DocOpenMode mode)
        {
            if (precedent == null)
                throw new ArgumentNullException("precedent");

            OpenSettings settings = OpenSettings.Default;
            settings.Mode = mode;
            return Open((IStorageItem)precedent, settings, Common.TriState.Null);
        }

        /// <summary>
        /// Opens a precedent based on the precedent object passed.
        /// </summary>
        /// <param name="precedent">The precedent to be opened.</param>
        /// <param name="mode">The open mode of the precedent.</param>
        /// <returns>The document object that has been opened.</returns>
        protected abstract object InternalPrecedentOpen(Precedent precedent, FetchResults fetchData, OpenSettings settings);

        #endregion

        #region Job Processing Routines

        /// <summary>
        /// Passes a file info object top the derived class so that it knows how to insert a text precedent.
        /// </summary>
        /// <param name="obj">The object / document having text inserted into.</param>
        /// <param name="preclink">The precedent link info.</param>
        protected abstract void InsertText(object obj, PrecedentLink preclink);

        /// <summary>
        /// Processes a job.
        /// </summary>
        /// <param name="precjob"></param>
        /// <returns></returns>
        public override ProcessJobStatus ProcessJob(FWBS.OMS.PrecedentJob precjob)
        {
            object obj;
            return ProcessJob(precjob, out obj);
        }

        /// <summary>
        /// Process Job.
        /// </summary>
        public ProcessJobStatus ProcessJob(FWBS.OMS.PrecedentJob precjob, out object obj)
        {
            //Create of activate the word application if it does not already exist.
            obj = null;

            try
            {
                ActivateApplication();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
                return ProcessJobStatus.Error;
            }

            System.Windows.Forms.IWin32Window owner = ActiveWindow;
            obj = this;


            //Check the precedent is Text Only?
            if (precjob.Precedent.TextOnly)
            {

                try
                {
                    CheckObjectIsDoc(ref obj);
                }
                catch
                {
                    obj = null;
                }

                if (obj != null && precjob.AsNewTemplate == false)
                {
                    if (precjob.Associate == null)
                        precjob.Associate = GetCurrentAssociate(obj);

                    // Check the Precedent still doesn't have an associate
                    // If not prompt for the UI For Associate information...

                    if (precjob.Associate == null)
                        precjob.Associate = FWBS.OMS.UI.Windows.Services.SelectAssociate(owner);


                    //Check the active doc type and whether is has the TYPE variable incase a default value has been used.
                    if (GetActiveDocType(obj) == precjob.Precedent.PrecedentType && HasDocVariable(obj, TYPE))
                    {
                        if (precjob.Associate == null)
                        {
                            precjob.Associate = FWBS.OMS.UI.Windows.Services.SelectAssociate(owner);
                            //Associate still not set so quit function
                            if (precjob.Associate == null)
                                return ProcessJobStatus.Error;
                        }

                        //Already have a base precedent
                        InsertText(obj, precjob);
                    }
                    else
                    {
                        //Check the associate objects are set if not pick?
                        if (precjob.Associate == null)
                        {
                            precjob.Associate = FWBS.OMS.UI.Windows.Services.SelectAssociate(owner);
                            //Associate still not set so quit function
                            if (precjob.Associate == null)
                                return ProcessJobStatus.Error;
                        }
                        //Create the Base Precedent First.
                        obj = TemplateStart(this, precjob.Precedent.PrecedentType, precjob.Associate);

                        InsertText(obj, precjob);
                    }
                }
                else
                {
                    //Check the associate objects are set if not pick?
                    if (precjob.Associate == null)
                    {
                        precjob.Associate = FWBS.OMS.UI.Windows.Services.SelectAssociate(owner);
                        //Associate still not set so quit function
                        if (precjob.Associate == null)
                            return ProcessJobStatus.Error;

                    }
                    //Base Precedent Needed First.
                    obj = TemplateStart(this, precjob.Precedent.PrecedentType, precjob.Associate);
                    System.Windows.Forms.Application.DoEvents();
                    InsertText(obj, precjob);
                }
            }
            else
            {
                //Check the associate objects are set if not pick?
                if (precjob.Associate == null)
                {
                    precjob.Associate = FWBS.OMS.UI.Windows.Services.SelectAssociate(owner);
                    //Associate still not set so quit function
                    if (precjob.Associate == null)
                        return ProcessJobStatus.Error;

                }
                obj = TemplateStart(this, precjob);
            }

            // DMB 19/02/2004 check if object is null and stop and get out if it is
            if (obj == null)
                return ProcessJobStatus.PauseJobs;
            else
                CheckObjectIsDoc(ref obj);

            try
            {
                SetDocVariable(obj, PROCESSING, true);


                DettachPrecedentVars(obj);

                LinkDocument(obj, precjob);
                // RJA 04/02/2009 Moved OnJobProcessed here to avoid documents that have autosave enabled being closed before OnJobProcessed is called
                //Raise the event which tells the consuming object that a particular job has completed.
                OnJobProcessed();

                if (precjob.SaveMode != PrecSaveMode.None)
                {
                    SaveSettings settings = SaveSettings.Default;
                    settings.Printing.Mode = precjob.PrintMode;
                    settings.Mode = precjob.SaveMode;
                    settings.UseDefaultAssociate = false;
                    settings.ContinueOnError = false;

                    if (BeginSave(obj, settings) == DocSaveStatus.Error)
                    {
                        return ProcessJobStatus.PauseJobs;
                    }
                }
                else
                {
                    if (precjob.PrintMode != PrecPrintMode.None)
                    {
                        PrintSettings settings = PrintSettings.Default;
                        settings.Mode = precjob.PrintMode;
                        BeginPrint(new object[1] { obj }, settings);
                    }
                }

            }
            finally
            {
                try
                {
                    RemoveDocVariable(obj, PROCESSING);
                }
                catch
                {
                    //Document could be closed therfore Word causing
                    //An object deleted COM exception
                }
            }

            return ProcessJobStatus.Finished;
        }


        #endregion

        #region Template Routines

        /// <summary>
        /// Start a template passing a string for the precedent through, this will then lookup
        /// to see whether the precedent is a system precedent such as letterhead
        /// </summary>
        /// <param name="obj">Object to be base normally Word, Outlook</param>
        /// <param name="precName">Precedent String "LETTERHEAD", "MEMO"</param>
        /// <param name="assoc">Associate or to be worked with</param>
        /// <returns>Object created</returns>
        public override object TemplateStart(object obj, string precName, Associate assoc)
        {
            Precedent precobj = Precedent.GetDefaultPrecedent(precName, assoc);

            if (precobj == null)
            {
                throw new OMSException(HelpIndexes.PrecedentNotFound, precName);
            }

            if (assoc == null)
                return null;

            PrecedentLink preclink = new PrecedentLink(precobj, assoc);

            object doc = TemplateStart(obj, preclink);


            DettachPrecedentVars(doc);

            LinkDocument(doc, preclink);

            ActivateDocument(doc);

            return doc;
        }

        public override object TemplateStart(object obj, Precedent prec, Associate assoc)
        {
            if (prec == null)
                throw new ArgumentNullException("prec");

            if (assoc == null)
                return null;

            PrecedentLink preclink = new PrecedentLink(prec, assoc);

            object doc = TemplateStart(obj, preclink);

            DettachPrecedentVars(doc);

            LinkDocument(doc, preclink);

            ActivateDocument(doc);

            return doc;
        }

        /// <summary>
        /// Start a template passing a string for the precedent through, this will then lookup
        /// to see whether the precedent is a system precedent such as letterhead
        /// </summary>
        /// <param name="obj">Object to be base normally Word, Outlook</param>
        /// <param name="prec">Precedent / Associate Link Object.</param>
        /// <returns>Object created</returns>
        protected abstract object TemplateStart(object obj, PrecedentLink preclink);


        #endregion

        #region Properties


        /// <summary>
        /// Gets the module name.
        /// </summary>
        public abstract string ModuleName { get; }

        /// <summary>
        /// Gets the active window handle, if any.
        /// </summary>
        public abstract System.Windows.Forms.IWin32Window ActiveWindow { get; }


        #endregion

        #region Document Key Routines

        protected virtual void SetDocKey(object obj, string key)
        {
        }

        protected virtual string[] GetDocKey(object obj)
        {
            return new string[0];
        }

        public static string GenerateDocKey(long docid, long compid, long serialno, string datakey)
        {
            if (docid != 0 && compid != 0 && serialno != 0)
            {
                try
                {
                    const string CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string text = compid.ToString() + ";" + serialno.ToString() + ";" + docid.ToString() + ";" + datakey;
                    int length = CHARS.Length + 2;
                    string ret = "";

                    text = text.Replace("-", "Z");
                    text = text.Replace(";", "X");

                    string serial = text.ToUpper();

                    Random rnd = new Random();
                    int key = rnd.Next(CHARS.Length - 1) + 1;
                    ret += CHARS[key - 1];

                    for (int ctr = serial.Length - 1; ctr >= 0; ctr--)
                    {
                        int ach = CHARS.IndexOf(serial[ctr]) + key;
                        if (ach > CHARS.Length - 1)
                        {
                            do
                            {
                                ach -= CHARS.Length;
                                if (ach < 0) ach = 0;
                            }
                            while (ach > (CHARS.Length - 1));
                        }
                        ret += CHARS[ach];
                    }

                    for (int g = ret.Length; g <= (length - 2); g++)
                    {
                        int ach = rnd.Next(CHARS.Length - 1);
                        ret += CHARS[ach];
                    }

                    ret += CHARS[serial.Length - 1];

                    return ret;
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
        /// <summary>
        /// Creates a key value based on the document id of the current document.
        /// </summary>
        private string GenerateDocKey(object obj)
        {
            CheckObjectIsDoc(ref obj);
            long docid = GetDocVariable(obj, DOCUMENT, 0);
            long compid = GetDocVariable(obj, COMPANY, 0);
            long serialno = GetDocVariable(obj, BRANCH, 0);
            string datakey = GetDocVariable(obj, DATAKEY, "");

            return GenerateDocKey(docid, compid, serialno, datakey);
        }

        /// <summary>
        /// Resolves a document key value based on the key provided.
        /// </summary>
        private Document ResolveDocKey(string[] keys)
        {
            try
            {
                const string CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                foreach (string key in keys)
                {
                    try
                    {

                        if (key == null || key == String.Empty) continue;

                        string serial = key.ToUpper();

                        int k = CHARS.IndexOf(serial[0]) + 1;
                        serial = serial.Substring(1, CHARS.IndexOf(serial[serial.Length - 1]) + 1);
                        string ret = "";

                        for (int ctr = serial.Length - 1; ctr >= 0; ctr--)
                        {
                            int ach = CHARS.IndexOf(serial[ctr]) - k;
                            if (ach < 0)
                            {
                                do
                                    ach += (CHARS.Length);
                                while (ach < 0);
                            }
                            ret += CHARS[ach];
                        }

                        ret = ret.Replace("Z", "-");
                        ret = ret.Replace("X", ";");

                        string[] res = ret.Split(';');

                        long compid = Convert.ToInt64(res[0]);
                        long serialno = Convert.ToInt64(res[1]);
                        long docid = Convert.ToInt64(res[2]);
                        string datakey = "";

                        //Cater for the datakey that may not exist.
                        if (res.Length > 3)
                            datakey = res[3].Trim();

                        if (compid == Session.CurrentSession.CompanyID &&
                            (String.IsNullOrEmpty(datakey) || datakey.ToUpperInvariant().Equals(Session.CurrentSession.DataKey.ToUpperInvariant())))
                            return Document.GetDocument(docid);
                        else
                            continue;
                    }
                    catch
                    {
                        continue;
                    }
                }

                return null;

            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region RUN COMMANDS

        /// <summary>
        /// Run Command to allow for commands to be ran which aren't in base interface.
        /// </summary>
        /// <param name="obj">Object being manipulated.</param>
        /// <param name="cmd">Command to be ran.</param>
        public override void RunCommand(object obj, string cmd)
        {
            CheckUserLastUpdate();
            CheckFeeEarnerLastUpdate();

            IOMSApp app;
            if (string.IsNullOrEmpty(cmd))
                return;

            string[] pars;
            pars = cmd.Split(';');

            string lastElement = pars[pars.GetUpperBound(0)];

            if (lastElement == "SAVE")
            {
                string additionalSaveCommands = Session.CurrentSession.CurrentUser.AdditionalDocumentSaveCommands;
                if (string.IsNullOrEmpty(additionalSaveCommands))
                    additionalSaveCommands = Session.CurrentSession.AdditionalDocumentSaveCommands;

                if (!string.IsNullOrEmpty(additionalSaveCommands))
                {

                    cmd = cmd + ";" + additionalSaveCommands;

                    pars = cmd.Split(';');
                    lastElement = pars[pars.GetUpperBound(0)];
                }
            }



            bool quick = lastElement == "QUICK";


            int formcount = Services.VisibleForms.Length;

            try
            {

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                if (pars.Length < 2)
                {
                    MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("INVALIDRUNCMD", "Invalid run command!", "", false), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    return;
                }

                switch (pars[1])
                {
                    case "ABOUT":
                        FWBS.OMS.UI.Windows.Services.ShowAbout();
                        break;
                    case "CONNECT":    //Connects to a session, log in if need to.
                        if (pars.Length > 3 && pars[2] == "FORCE")
                        {
                            try
                            {
                                Session.CurrentSession.LogOn(Convert.ToInt32(pars[3]), Environment.UserName, "");
                                Services.ShowUserAutoWindows(ActiveWindow);
                            }
                            catch
                            {

                                FWBS.OMS.UI.Windows.Services.CheckLogin();
                            }
                        }
                        else
                        {
                            try
                            {
                                Session.CurrentSession.Connect();
                            }
                            catch
                            {

                                FWBS.OMS.UI.Windows.Services.CheckLogin();
                            }
                        }

                        //Set the default parent window to the currently active window after logon
                        //just incase the command centre form is being shown.
                        ActivateWindow(ActiveWindow);
                        break;
                    case "DISCONNECT":    //Logs off the currently logged in session.
                        Session.CurrentSession.Disconnect();
                        goto DoNotCheckJobList;
                    case "CLIENTINFO":    //Displays the client information screen by using a client picker screen first.
                        {
                            ShowClientInfoCommand(pars);
                        }
                        goto DoNotCheckJobList;
                    case "FILEINFO":    //Diplays a selected clients file information by using a client / file picker screen.
                        {
                            ShowFileInfoCommand(pars);
                        }
                        goto DoNotCheckJobList;
                    case "UFNINFORMATION":
                        FWBS.OMS.OMSFile newfile3a = FWBS.OMS.UI.Windows.Services.SelectFile();
                        if (newfile3a != null)
                        {
                            try
                            {
                                object test = newfile3a.ExtendedData["EXTFILLA"].Code;
                                object result = FWBS.OMS.UI.Windows.Services.ShowOMSItem("SCRFILUFNEDIT", newfile3a, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, false, new FWBS.Common.KeyValueCollection());
                            }
                            catch (Exception ex)
                            {
                                throw new OMSException2("ERRUFN", "There was a Problem showing the UFN Information, this %FILE% may not hold UFN Information!", ex, true);
                            }
                        }
                        goto DoNotCheckJobList;
                    case "COMMANDCENTRE":
                        FWBS.OMS.UI.Windows.Services.ShowCommandCentre(ActiveWindow, "");
                        goto DoNotCheckJobList;
                    case "CONTACTMANAGER":
                        FWBS.OMS.UI.Windows.Services.ShowSearchManager(ActiveWindow, FWBS.OMS.UI.Windows.SearchManager.ContactManager);
                        goto DoNotCheckJobList;
                    case "USERSETTINGS":
                        FWBS.OMS.UI.Windows.Services.DisplayCurrentUserSettings();
                        break;
                    case "RELINKFIELDS":
                        RelinkFields(obj);
                        break;
                    case "REFRESHFIELDS":
                        RefreshFields(obj);
                        break;
                    case "PRINT":
                        Print(obj);
                        break;
                    case "SAVEAS":
                        SaveAs(obj, false);
                        break;
                    case "SAVEQUICK":
                        SaveQuick(obj);
                        break;
                    case "SAVE":
                        {
                            SaveSettings settings = SaveSettings.Default;

                            if (quick)
                            {
                                //settings.UseDefaultAssociate = true; Removed so the System setting is picked up - Will only see a change in behavior on save as
                                settings.Printing.Mode = PrecPrintMode.None;
                                settings.Mode = PrecSaveMode.Quick;
                            }

                            if (pars.Length > 2)
                            {
                                switch (pars[2])
                                {
                                    case "CONTINUE":
                                        {
                                            settings.ContinueEditing = true;
                                            Save(obj, settings);
                                        }
                                        break;
                                    default:
                                        Save(obj, settings);
                                        break;
                                }
                            }
                            else
                                Save(obj, settings);

                        }
                        break;
                    case "TEMPLATESTART":
                        {
                            DisableVisibleFormsCheck = TemplateStartCommand(pars);
                        }
                        break;
                    case "TABLE":
                        {
                            Associate assoc = GetCurrentAssociate(obj);
                            FieldParser parser = new FieldParser(assoc);
                            switch (pars[2])
                            {
                                case "DATALIST":
                                    {
                                        BuildTable(obj, pars[3], (DataView)parser.Parse(FieldParser.FieldPrefixDataList + pars[3]), false);
                                    }
                                    break;
                                case "SEARCH":
                                    {
                                        BuildTable(obj, pars[3], (DataView)parser.Parse(FieldParser.FieldPrefixSearchList + pars[3]), true);
                                    }
                                    break;
                                default:
                                    goto case "DATALIST";
                            }
                        }
                        break;
                    case "NEWCLIENT":
                        FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
                        break;
                    case "NEWPRECLIENT":
                        FWBS.OMS.UI.Windows.Services.Wizards.CreatePreClient();
                        break;
                    case "NEWFILE":
                        FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(true);
                        goto DoNotCheckJobList;
                    case "NEWASSOC":
                        FWBS.OMS.UI.Windows.Services.Wizards.CreateAssociate(null);
                        break;
                    case "GOTONEXTSTOPCODE":
                        if (pars.Length < 3 || pars[2].ToUpper() == "TRUE")
                            GotoNextStopCode(true);
                        else
                            GotoNextStopCode(false);
                        break;
                    case "PRECEDENTS":
                        if (pars.Length < 3 || pars[2].ToUpper() == "MAIN")
                            FWBS.OMS.UI.Windows.Services.ShowPrecedentLibrary(ActiveWindow, this, null, DefaultDocType, "");
                        else if (pars[2].ToUpper() == "QUICK")
                        {

                            string ret = InputBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("PICKPREC", "Please enter a %PRECEDENT% title to use.", "", true).Text);
                            if (ret == InputBox.CancelText || ret.Trim() == "")
                                return;
                            else
                            {

                                Precedent prec = null;
                                Associate assoc = Services.SelectAssociate(ActiveWindow);
                                if (assoc != null)
                                {
                                    prec = Precedent.GetPrecedent(ret, assoc);
                                    if (prec != null)
                                    {
                                        PrecedentJob job = new PrecedentJob(prec);
                                        job.Associate = assoc;
                                        Services.ProcessJob(this, job);
                                        if (job.HasError)
                                            MessageBox.Show(ActiveWindow, job.ErrorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }

                            }

                        }
                        else
                        {
                            try
                            {    // Try the Active Window if present else go for default.
                                FWBS.OMS.UI.Windows.Services.ShowPrecedentLibrary(ActiveWindow, this, GetCurrentAssociate(obj), DefaultDocType, "");
                            }
                            catch
                            {
                                FWBS.OMS.UI.Windows.Services.ShowPrecedentLibrary(this, null, "");
                            }
                        }

                        goto DoNotCheckJobList;


                    case "SAVEASPREC":
                        SaveAs(obj, true);
                        break;
                    case "OPEN":
                        {
                            OpenCommand(obj, pars);
                        }
                        break;
                    case "OPENDOCUMENT":
                        {
                            OpenDocumentCommand(obj, pars);
                        }
                        break;
                    case "COMPARE":
                        {
                            DocumentCompareType type = DocumentCompareType.Latest;

                            if (pars.Length > 2)
                            {
                                switch (pars[2])
                                {
                                    case "VERSION":
                                        type = DocumentCompareType.Specific;
                                        break;
                                    case "LATEST":
                                        type = DocumentCompareType.Latest;
                                        break;
                                    case "FILE":
                                        type = DocumentCompareType.File;
                                        break;
                                    case "CLIENT":
                                        type = DocumentCompareType.Client;
                                        break;
                                    case "RECENT":
                                        type = DocumentCompareType.Recent;
                                        break;
                                    case "ANY":
                                        type = DocumentCompareType.Any;
                                        break;
                                    default:
                                        type = DocumentCompareType.Latest;
                                        break;
                                }
                            }

                            CompareDocument(obj, type);
                        }
                        break;
                    case "LOCKDOC":
                        {
                            IStorageItem doc = GetCurrentDocument(obj);

                            if (doc != null)
                            {
                                if (pars.Length > 2)
                                {
                                    switch (pars[2])
                                    {
                                        case "IN":
                                            CheckIn(obj, SaveSettings.Default);
                                            break;
                                        case "OUT":
                                            CheckOut(obj);
                                            break;
                                        case "UNDO":
                                            var lockable = (IStorageItemLockable)doc;
                                            bool isCheckedOutByCurrentUserPriorToUndo = lockable.IsCheckedOutByCurrentUser && FWBS.Common.Functions.IsCurrentComputer(lockable.CheckedOutMachine);
                                            object reopenedDocObject = UndoCheckOut(obj, true, false);

                                            if (reopenedDocObject == null)
                                            {
                                                break;
                                            }

                                            if (isCheckedOutByCurrentUserPriorToUndo)
                                            {
                                                CheckOut(reopenedDocObject);
                                            }

                                            break;
                                    }
                                }
                            }
                            else
                                CheckOut(obj);

                        }
                        break;
                    case "DOCPROPS":
                        {
                            Document doc = GetCurrentDocument(obj);
                            if (doc != null)
                            {
                                Services.ShowDocument(ActiveWindow, doc);
                            }
                        }
                        break;
                    case "GETLATESTVERSION":
                        {
                            GetLatestVersion(obj);
                        }
                        break;
                    case "ADDFIELD":
                        CheckObjectIsDoc(ref obj);
                        Services.AddField(this.ActiveWindow, this);
                        break;
                    case "DELETEFIELD":
                        CheckObjectIsDoc(ref obj);
                        Services.DeleteField(this.ActiveWindow, this);
                        break;
                    case "SHOWDOCVARS":
                        DisplayDocVariables(obj);
                        break;
                    case "DETACHDOCVARS":
                        {
                            CheckObjectIsDoc(ref obj);
                            DettachDocumentVars(obj);
                        }
                        break;
                    case "VIEWCURRENTFILE":
                    case "F_CONTEXTVIEWFILE":
                        if (GetDocVariable(obj, FILE, -1) != -1)
                        {
                            FWBS.OMS.OMSFile newfile = FWBS.OMS.OMSFile.GetFile(GetDocVariable(obj, FILE, -1));
                            FWBS.OMS.UI.Windows.Services.ShowFile(ActiveWindow, newfile, "");
                            goto DoNotCheckJobList;
                        }
                        goto case "FILEINFO";
                    case "VIEWCURRENTCLIENT":
                    case "F_CONTEXTVIEWCLIENT":


                        if (GetDocVariable(obj, CLIENT, -1) != -1)
                        {
                            FWBS.OMS.Client newclient = FWBS.OMS.Client.GetClient(GetDocVariable(obj, CLIENT, -1));
                            FWBS.OMS.UI.Windows.Services.ShowClient(ActiveWindow, newclient, "");
                            goto DoNotCheckJobList;
                        }
                        else
                            goto case "CLIENTINFO";

                    case "VIEWCURRENTASSOCIATE":
                        if (GetDocVariable(obj, ASSOCIATE, -1) != -1)
                        {
                            //UTCFIX: DM - 30//11/06 - Make sure that comparison is the same.
                            DateTime tmp = DateTime.Now;
                            FWBS.OMS.Associate newassociate = FWBS.OMS.Associate.GetAssociate(GetDocVariable(obj, ASSOCIATE, -1));

                            FWBS.OMS.UI.Windows.Services.ShowAssociate(null, newassociate, "");
                            if (newassociate.TrackingStamp.Updated.ToLocalTime() > tmp)
                            {
                                if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ASSOCUPDATE2", "Would you like to Relink Fields?", "", false), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    RelinkFields(obj);
                                }
                            }
                        }
                        else
                            MessageBox.Show(Session.CurrentSession.Resources.GetMessage("NOCURASSOC", "There is no Current Document for Viewing the Active Associate!", "", false), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case "VIEWUFNINFO":
                        if (pars.Length > 2)
                        {
                            FWBS.OMS.OMSFile newfile3;
                            try
                            {
                                newfile3 = FWBS.OMS.OMSFile.GetFile(Convert.ToInt64(pars[2]));
                                object result = FWBS.OMS.UI.Windows.Services.ShowOMSItem("SCRFILUFNEDIT", newfile3, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, false, new FWBS.Common.KeyValueCollection());
                            }
                            catch
                            {
                                MessageBox.Show(Session.CurrentSession.Resources.GetMessage("MSGERRPRMINC", "Error UFNInfo Params Incorrect!", "").Text);
                            }
                        }
                        else
                            if (FWBS.OMS.Session.CurrentSession.CurrentFile != null)
                        {
                            object result = FWBS.OMS.UI.Windows.Services.ShowOMSItem("SCRFILUFNEDIT", FWBS.OMS.Session.CurrentSession.CurrentFile, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, false, new FWBS.Common.KeyValueCollection());
                        }
                        break;
                    case "VIEWUFNINFOQUESTION":
                        FWBS.OMS.OMSFile newfile2;
                        newfile2 = FWBS.OMS.UI.Windows.Services.SelectFile();
                        if (newfile2 != null)
                        {
                            object result = FWBS.OMS.UI.Windows.Services.ShowOMSItem("SCRFILUFNEDIT", newfile2, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, false, new FWBS.Common.KeyValueCollection());
                        }
                        break;
                    case "CREATETIME":
                        FWBS.OMS.OMSFile omfile;
                        if (pars.Length > 2)
                        {
                            omfile = FWBS.OMS.OMSFile.GetFile(Convert.ToInt64(pars[2]));
                        }
                        else
                        {
                            omfile = FWBS.OMS.UI.Windows.Services.SelectFile();
                        }
                        if (omfile != null)
                        {
                            FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(FWBS.OMS.Session.CurrentSession.DefaultSystemForm(FWBS.OMS.SystemForms.ManualTimeWizard), omfile, null);
                        }
                        break;

                    case "VIEWINBOX":
                        var currentManager = Apps.ApplicationManager.CurrentManager;
                        if (currentManager.AppsByName.ContainsKey("OUTLOOK"))
                        {
                            currentManager.AppsByName.Remove("OUTLOOK");
                        }
                        app = currentManager.GetApplicationInstance("OUTLOOK", true);
                        app.RunCommand(obj, cmd);
                        DisableVisibleFormsCheck = true;
                        goto DoNotCheckJobList;
                    case "VIEWTASKS":
                        goto case "VIEWINBOX";
                    case "VIEWCALENDAR":
                        goto case "VIEWINBOX";
                    default:
                        if (pars[0] == "SYSTEM")    //Just so that uncptured SYSTEM commands like SYSTEM;TOOLBARSCHANGED do not fire the process job list question.
                            goto DoNotCheckJobList;
                        break;

                }

                //If the session is logged in then check the precedent job list and
                //ask to process the list.
                if (Session.CurrentSession.IsLoggedIn)
                {
                    Services.CheckJobList(this);
                }

            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }


        DoNotCheckJobList:

            //The form count must be the same as when the RunCommand started.
            //This allows modaless forms to stay in focus and modal forms to give
            //focus back to the calling app.
            if (DisableVisibleFormsCheck)
            {
                DisableVisibleFormsCheck = false;
                return;
            }

            if (Services.VisibleForms.Length == formcount)
                ActivateWindow(ActiveWindow);

            if (_activeDoc != null)
                ActivateDocument(_activeDoc);

            _activeDoc = null;

            return;
        }

        /// <summary>
        /// Creates a modeless Wizard Form for a specific Command.
        /// </summary>
        /// <param name="cmd">Command to be ran.</param>
        /// <param name="wizardStyle">Parameter indicating Wizard style.</param>
        /// <param name="param">Optional additional parameter, e.g. existing Client or Matter</param>
        /// <returns>Wizard Form</returns>
        public static Form CreateModelessWizard(string cmd, WizardStyle wizardStyle, object param = null)
        {
            Form frm = null;
            if (Services.CheckLogin())
            {
                try
                {
                    OMSFile file;
                    switch (cmd)
                    {
                        case "NEWFILE":
                            Client client = (param is Client) ? (Client)param : Services.SelectClient();
                            if (client != null)
                            {
                                new ClientPermission(client, StandardPermissionType.CreateFile).Check();
                                new ClientActivity(client, ClientStatusActivityType.FileCreation).Check();
                                frm = new frmFileWizard(client, null, wizardStyle);
                            }
                            break;

                        case "NEWASSOC":
                            new SystemPermission(StandardPermissionType.CreateAssociate).Check();
                            file = (param is OMSFile) ? (OMSFile)param : Services.SelectFile();
                            if (file != null)
                            {
                                new FilePermission(file, StandardPermissionType.CreateAssociate).Check();
                                new FileActivity(file, FileStatusActivityType.AssociateCreation).Check();
                                frm = new frmAssociateWizard(file, null, false, wizardStyle);
                            }
                            break;

                        case "NEWCLIENT":
                            new SystemPermission(StandardPermissionType.CreateClient).Check();
                            frm = new frmClientTakeonWizard(null, false, false, null, wizardStyle);
                            break;

                        case "NEWPRECLIENT":
                            frm = new frmWizard("SCRCLIPRENEW", null, EnquiryEngine.EnquiryMode.Add, false, new KeyValueCollection(), wizardStyle);
                            break;

                        case "CREATEPRECLIENTCORPORATE":
                            frm = new frmWizard("SCRCLIPRECRPNEW", null, EnquiryEngine.EnquiryMode.Add, false, new KeyValueCollection(), wizardStyle);
                            break;

                        case "CREATECONTACT":
                            new SystemPermission(StandardPermissionType.CreateContact).Check();
                            frm = new frmContactWizard(null, wizardStyle);
                            break;

                        case "NEWUNDERTAKING":
                            file = (param is OMSFile) ? (OMSFile)param : Services.SelectFile();
                            if (file != null)
                            {
                                frm = new frmWizard("SCRFILUNDERTAKI", file, EnquiryEngine.EnquiryMode.Add, false, new KeyValueCollection(), wizardStyle);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ex);
                }
            }
            return frm;
        }

        private void CheckFeeEarnerLastUpdate()
        {
            CheckFeeEarnerLastUpdate(false);
        }
        private void CheckFeeEarnerLastUpdate(bool force)
        {

            try
            {
                //Check the last date the User was updated, and see if the user record needs updating.
                if (FWBS.OMS.Session.CurrentSession != null)
                {
                    if (FWBS.OMS.Session.CurrentSession.CurrentFeeEarner != null && FWBS.OMS.Session.CurrentSession.CurrentConnection != null)
                    {

                        IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                        List<IDataParameter> parList = new List<IDataParameter>();
                        parList.Add(connection.CreateParameter("FEEUSRID", FWBS.OMS.Session.CurrentSession.CurrentFeeEarner.ID));
                        DataTable data = connection.ExecuteSQL("SELECT DBFEEEARNER.UPDATED AS FEEUPDATED , DBUSER.UPDATED AS FEEUSERUPDATED FROM DBFEEEARNER INNER JOIN DBUSER ON DBFEEEARNER.FEEUSRID = DBUSER.USRID WHERE FEEUSRID = @FEEUSRID", parList);

                        DateTimeNULL checkDateFeeEarner = Convert.ToDateTime(data.Rows[0][0]);
                        DateTimeNULL checkDateFeeEarnerUser = Convert.ToDateTime(data.Rows[0][1]);


                        if (force)
                        {
                            FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerLastUpdated = checkDateFeeEarner;
                            FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerUserLastUpdated = checkDateFeeEarnerUser;
                            FWBS.OMS.Session.CurrentSession.CurrentFeeEarner = FWBS.OMS.Session.CurrentSession.CurrentUser.WorksFor;
                            FWBS.OMS.Session.CurrentSession.CurrentFeeEarner.Refresh();
                        }
                        else
                        {
                            if (checkDateFeeEarner > FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerLastUpdated | checkDateFeeEarnerUser > FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerUserLastUpdated)
                            {


                                if (System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("CUS_REFRESHFEE", "Your fee earner's record has been updated.  Would you like to refresh the fee earner's details for this application?", "").Text, Session.CurrentSession.Resources.GetResource("CUS_REFRESHFECP", "Refresh Fee Earner Details", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerLastUpdated = checkDateFeeEarner;
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerUserLastUpdated = checkDateFeeEarnerUser;
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarners.Clear(); //Clear here as the FeeEarner object does not refresh the FeeEarner part of a user
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarner = FWBS.OMS.Session.CurrentSession.CurrentUser.WorksFor;
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarner.Refresh();
                                }
                                else
                                {
                                    //Even if user does not update the user record, they won't want a warning all of the time, so set the updated _currentUserLastUpdated to the date checked
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerLastUpdated = checkDateFeeEarner;
                                    FWBS.OMS.Session.CurrentSession.CurrentFeeEarnerUserLastUpdated = checkDateFeeEarnerUser;
                                    System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("CUS_APPCLOSEFEE", "If you close this application fee earner record will be updated automatically when you next open it.", "").Text, Session.CurrentSession.Resources.GetResource("CUS_APPCLOSEFCP", "Update Fee Earner Details", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Error raised in CheckFeeEarnerLastUpdate : {0}", ex.Message));
            }
        }


        private void CheckUserLastUpdate()
        {
            try
            {
                //Check the last date the User was updated, and see if the user record needs updating.
                if (FWBS.OMS.Session.CurrentSession != null)
                {
                    if (FWBS.OMS.Session.CurrentSession.CurrentUser != null && FWBS.OMS.Session.CurrentSession.CurrentConnection != null)
                    {
                        IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                        List<IDataParameter> parList = new List<IDataParameter>();
                        parList.Add(connection.CreateParameter("USRID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID));
                        DataTable data = connection.ExecuteSQL("SELECT UPDATED FROM DBUSER WHERE USRID = @USRID", parList);

                        DateTimeNULL checkDate = Convert.ToDateTime(data.Rows[0][0]).ToLocalTime();

                        System.Diagnostics.Debug.WriteLine(string.Format("CurrentUserLastUpdated : {0}{2}From Database : {1}", FWBS.OMS.Session.CurrentSession.CurrentUserLastUpdated, checkDate, Environment.NewLine));

                        if (checkDate > FWBS.OMS.Session.CurrentSession.CurrentUserLastUpdated)
                        {
                            if (System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("CUS_REFRESH", "Your user record has been updated. Would you like to refresh your user details for this application?", "").Text, Session.CurrentSession.Resources.GetResource("CUS_REFRESHCP", "Refresh User Details", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                FWBS.OMS.Session.CurrentSession.CurrentUser.Refresh(); //TODO - do we need to pass true argument here?
                                FWBS.OMS.Session.CurrentSession.CurrentUserLastUpdated = checkDate;
                                FWBS.OMS.Session.CurrentSession.CurrentFeeEarner = FWBS.OMS.Session.CurrentSession.CurrentUser.WorksFor;
                                FWBS.OMS.Session.CurrentSession.CurrentFeeEarner.Refresh();
                                CheckFeeEarnerLastUpdate(true);
                            }
                            else
                            {
                                //Even if user does not update the user record, they won't want a warning all of the time, so set the updated _currentUserLastUpdated to the date checked
                                FWBS.OMS.Session.CurrentSession.CurrentUserLastUpdated = checkDate;
                                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("CUS_APPCLOSE", "If you close this application your user record will be updated automatically when you next open it.", "").Text, Session.CurrentSession.Resources.GetResource("CUS_APPCLOSECP", "Update User Details", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Error raised in CheckUserLastUpdate : {0}", ex.Message));
            }

        }

        protected bool DisableVisibleFormsCheck { get; set; }

        #endregion

        #region Locking & Versioning

        protected virtual System.IO.FileInfo GetCurrentFileLocation(object obj)
        {
            return null;
        }

        public void GetLatestVersion(object obj)
        {
            CheckObjectIsDoc(ref obj);

            Document doc = GetCurrentDocument(obj);
            if (doc != null)
            {
                OpenSettings settings = OpenSettings.Default;
                settings.Mode = DocOpenMode.Edit;
                settings.LatestVersion = false;
                Open(((IStorageItemVersionable)doc).GetLatestVersion(), settings, Common.TriState.True);
            }
            else
                ThrowUnsavedDocumentActionException();
        }

        public void CheckIn(object obj, SaveSettings settings)
        {
            CheckObjectIsDoc(ref obj);

            if (settings == null)
                settings = SaveSettings.Default;

            if (Session.CurrentSession.QuickSaveOnCheckIn)
                settings.Mode = PrecSaveMode.Quick;

            Document doc = GetCurrentDocument(obj);

            if (doc != null)
            {
                IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);

                if (lockdoc.IsCheckedOutByCurrentUser)
                {
                    if (BeginSave(obj, settings) == DocSaveStatus.Success)
                    {
                        lockdoc.CheckIn();

                        MessageBox.ShowInformation("MSGCHECKEDIN", "The document has successfully been checked in.");
                    }
                }
                else
                    lockdoc.CheckIn();
            }
            else
                ThrowUnsavedDocumentActionException();
        }

        public void CheckOut(object obj)
        {
            CheckObjectIsDoc(ref obj);

            Document doc = GetCurrentDocument(obj);

            if (doc != null)
            {
                IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);

                lockdoc.CheckOut(GetCurrentFileLocation(obj));

                MessageBox.ShowInformation("MSGCHECKEDOUT", "The document has successfully been checked out.");
            }
            else
                ThrowUnsavedDocumentActionException();

        }

        public void UndoCheckOut(object obj)
        {
            UndoCheckOut(obj, true);
        }

        public void UndoCheckOut(object obj, bool reopen)
        {
            UndoCheckOut(obj, reopen, false);
        }


        public object UndoCheckOut(object obj, bool reopen, bool silentUndoCheckout = false)
        {
            object reopenedDocObject = null;

            CheckObjectIsDoc(ref obj);

            IStorageItem doc = GetCurrentDocumentVersion(obj);

            if (doc == null)
                doc = GetCurrentDocument(obj);

            if (doc != null)
            {
                DialogResult undoCheckout = (silentUndoCheckout)
                                                    ? DialogResult.Yes
                                                    : MessageBox.ShowYesNoQuestion("MSGUNDOCHNGSQ1",
                                                                                   "Are you sure you want to undo? You will lose any changes that have been made.");

                if (undoCheckout == DialogResult.Yes)
                {
                    IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);

                    if (lockdoc.CanUndo)
                    {
                        lockdoc.UndoCheckOut();
                    }

                    Close(obj);

                    if (!silentUndoCheckout)
                    {
                        MessageBox.ShowInformation("MSGUNDOCHANGES", "The changes to the document have successfully been undone.");
                    }

                    if (reopen)
                    {
                        OpenSettings settings = OpenSettings.Default;
                        settings.Mode = DocOpenMode.Edit;
                        settings.LatestVersion = false;
                        settings.CheckOut = false;
                        reopenedDocObject = Open(doc, settings, Common.TriState.True);
                    }
                }
            }
            else
                ThrowUnsavedDocumentActionException();

            return reopenedDocObject;
        }


        protected void ThrowUnsavedDocumentActionException()
        {
            throw new InvalidOperationException(OMS.Global.GetResString("UnsavedDocCannotPerformAction",false));
        }

        protected void OnDocumentClose(object doc, System.ComponentModel.CancelEventArgs args)
        {
            if (doc == null)
                return;

            if (!Session.CurrentSession.IsLoggedIn)
            {
                FWBS.Common.Reg.ApplicationSetting appsettings = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "SuppressNotConnectedWarning");
                if (Convert.ToBoolean(appsettings.GetSetting("False")) == true)
                    return;

                if (IsDocumentSaved(doc) == true)
                    return;
                var text=Session.CurrentSession.RegistryRes("NotLoggedIn",OMS.Global.GetResString("NotLoggedIn",false));
                MessageBox.ShowInformation(text);

                return;
            }

            if (!FWBS.OMS.Session.CurrentSession.IsLicensedFor("DM"))
                return;

            try
            {
                bool processed = false;
                IStorageItem omsdoc = GetCurrentDocumentVersion(doc);

                if (omsdoc == null)
                    omsdoc = GetCurrentDocument(doc);

                if (omsdoc == null)
                {
                    if (IsDocumentSaved(doc) == false && FWBS.OMS.Session.CurrentSession.IsLicensedFor("DM"))
                    {
                        DialogResult inval;
                        inval = MessageBox.ShowYesNoCancel("MSGNOTSAVED",
                                                           "Would you like to save this document? Clicking No will lose your changes and close the document.");
                        if (inval == DialogResult.Yes)
                        {
                            args.Cancel = true;
                            Save(doc);
                        }

                        if (inval == DialogResult.No)
                        {
                            SetDocumentAsSaved(doc);
                            args.Cancel = false;
                        }

                        if (inval == DialogResult.Cancel)
                        {
                            args.Cancel = true;
                        }
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                IStorageItemLockable lockeddoc = omsdoc.GetStorageProvider().GetLockableItem(omsdoc);
                if (lockeddoc != null)
                {
                    // Change logic to work with a number of resolutions for the Machine Name 
                    if (lockeddoc.IsCheckedOutByCurrentUser && FWBS.Common.Functions.IsCurrentComputer(lockeddoc.CheckedOutMachine))
                    {
                        bool autocheckin = false;

                        switch (FWBS.OMS.Session.CurrentSession.CurrentUser.AutoCheckInUnchangedDocuments)
                        {
                            case FWBS.Common.TriState.True:
                                autocheckin = true;
                                break;
                            case FWBS.Common.TriState.Null:
                                autocheckin = FWBS.OMS.Session.CurrentSession.AutoCheckInUnchangedDocuments;
                                break;
                        }

                        if (autocheckin && IsDocumentSaved(doc))
                        {
                            lockeddoc.UndoCheckOut();
                            processed = true;
                        }
                        else
                        {

                            MessageBox msg = new MessageBox(Session.CurrentSession.Resources.GetMessage("MSGWORDOCCLOSE5", "You are about to close a document that is checked out.  What would you like to do?" + Environment.NewLine + "Pressing Close will cause all changes to be lost.", "", omsdoc.DisplayID));

                            System.Collections.Generic.List<string> buttons = new System.Collections.Generic.List<string>();

                            if (!string.IsNullOrEmpty(Session.CurrentSession.Resources.GetResource("BTNCLSCHECKIN", "Check &In", "").Text))
                                buttons.Add("BTNCLSCHECKIN");

                            if (!string.IsNullOrEmpty(Session.CurrentSession.Resources.GetResource("BTNCLSUNDO", "&Undo", "").Text))
                                buttons.Add("BTNCLSUNDO");

                            string undoCloseButton = Session.CurrentSession.Resources.GetResource("BTNCLSUNDOCLS", "", "").Text;

                            if (!string.IsNullOrEmpty(undoCloseButton) && undoCloseButton != "BTNCLSUNDOCLS")
                                buttons.Add("BTNCLSUNDOCLS");

                            if (!string.IsNullOrEmpty(Session.CurrentSession.Resources.GetResource("BTNCLSCLOSE", "&Close", "").Text))
                                buttons.Add("BTNCLSCLOSE");

                            if (!string.IsNullOrEmpty(Session.CurrentSession.Resources.GetResource("BTNCLSCANCEL", "Cance&l", "").Text))
                                buttons.Add("BTNCLSCANCEL");

                            msg.Buttons = buttons.ToArray();
                            msg.Icon = MessageBoxIcon.Question;

                            string res = msg.Show();

                            switch (res)
                            {
                                case "BTNCLSCHECKIN":
                                    args.Cancel = true;
                                    CheckIn(doc, SaveSettings.Default);
                                    break;
                                case "BTNCLSUNDO":
                                    args.Cancel = true;
                                    object reopenedDocObject = UndoCheckOut(doc, true, false);
                                    if (reopenedDocObject == null)
                                    {
                                        break;
                                    }
                                    CheckOut(reopenedDocObject);
                                    break;
                                case "BTNCLSCLOSE":
                                    args.Cancel = true;
                                    SetDocumentAsSaved(doc);
                                    UndoCheckOut(doc, reopen: false, silentUndoCheckout: true);
                                    break;
                                case "BTNCLSUNDOCLS":
                                    args.Cancel = true;
                                    UndoCheckOut(doc, false);
                                    break;
                                case "BTNCLSCANCEL":
                                    args.Cancel = true;
                                    return;

                            }
                            processed = true;
                        }
                    }
                }

                //MNW Wrappered error to validate if the document is closed then the doc variable will be deleted.
                try
                {
                    if (!IsDocumentSaved(doc) && !processed)
                    {
                        DialogResult res = MessageBox.ShowYesNoCancel("MSGWORDOCCLOSE1", "You are about to close a document attached to %1%. Would you like to save now?" + Environment.NewLine + "Pressing No will cause all changes to be lost.", FWBS.OMS.Branding.APPLICATION_NAME);
                        switch (res)
                        {
                            case DialogResult.Yes:
                                args.Cancel = true;
                                if (!Save(doc))
                                    return;
                                break;
                            case DialogResult.No:
                                SetDocumentAsSaved(doc);
                                break;
                            case DialogResult.Cancel:
                                args.Cancel = true;
                                return;
                        }
                    }
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        protected void UnlockPrecedent(string precID)
        {
            if (Session.CurrentSession.ObjectLocking)
            {
                LockState ls = new LockState();
                ls.UnlockPrecedentObject(precID);
            }
        }

        protected virtual bool IsDocumentSaved(object doc)
        {
            throw new NotImplementedException();
        }

        protected virtual void SetDocumentAsSaved(object doc)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Comparison


        protected virtual bool CanCompare
        {
            get
            {
                return false;
            }
        }


        private void CompareDocument(object current, DocumentCompareType type)
        {
            CheckObjectIsDoc(ref current);

            bool cancel = false;
            IStorageItem comparison = PickDocumentToCompare(current, type, ref cancel);

            if (cancel)
                return;

            if (comparison == null)
            {
                comparison = PickDocumentToCompare(current, DocumentCompareType.Any, ref cancel);
                if (cancel)
                    return;

                if (comparison == null)
                {
                    MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DOCNOCOMPARISON", "The document may need to be saved to the system before comparison can be applied.", ""), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                    CompareDocument(current, comparison);
            }
            else
                CompareDocument(current, comparison);
        }

        private void CompareDocument(object current, IStorageItem comparison)
        {
            CheckObjectIsDoc(ref current);

            DialogResult ismaster = MessageBox.ShowYesNoCancel("MSGCURDOCISORIG", "Would you like the current document to be the original document in the comparison?");

            object master = null;
            object child = null;

            OpenSettings oset = OpenSettings.Default;
            oset.Mode = DocOpenMode.View;
            oset.LatestVersion = false;
            object newdoc;

            switch (ismaster)
            {
                case DialogResult.Yes:
                    master = current;
                    child = Open(comparison, oset, Common.TriState.True);
                    newdoc = CompareDocument(master, child);
                    break;
                case DialogResult.No:
                    master = Open(comparison, oset, Common.TriState.True);
                    child = current;
                    newdoc = CompareDocument(master, child);
                    break;
                default:
                    return;
            }
            if (newdoc != null)
            {
                _activeDoc = newdoc;
                ActivateDocument(newdoc);
            }
        }

        private IStorageItem PickDocumentToCompare(object current, DocumentCompareType type, ref bool cancel)
        {
            Document current_doc = GetCurrentDocument(current);


            switch (type)
            {
                case DocumentCompareType.Specific:
                    {
                        if (current_doc != null)
                        {
                            if (current_doc.IsNew)
                                throw new OMSException2("94500000", "The document must be stored before comparing with a specific latest version.");

                            IStorageItem current_si = current_doc;
                            IStorageItemVersionable current_versionable = current_doc;

                            using (DocumentManagement.Storage.StorageSettingsForm settings = new DocumentManagement.Storage.StorageSettingsForm(current_doc, SettingsType.Fetch))
                            {
                                if (settings.ShowDialog(ActiveWindow, typeof(FWBS.OMS.DocumentManagement.Storage.VersionFetchSettings).Name) == DialogResult.Cancel)
                                {
                                    cancel = true;
                                    return null;
                                }

                                var specversion = current_doc.GetSettings()
                                    .GetSettings<VersionFetchSettings>()
                                    .VersionLabel;

                                return current_doc.GetVersion(specversion);
                            }
                        }
                        else
                        {
                            ThrowUnsavedDocumentActionException();
                            return null;
                        }
                    }
                case DocumentCompareType.Latest:
                    {
                        if (current_doc != null)
                        {
                            if (current_doc.IsNew)
                                throw new OMSException2("94500000", "The document must be stored before comparing with a specific latest version.");

                            return current_doc.GetLatestVersion();
                        }
                        else
                        {
                            ThrowUnsavedDocumentActionException();
                            return null;
                        }
                    }
                case DocumentCompareType.File:
                    {
                        if (current_doc != null)
                        {
                            DocumentManagement.DocumentPicker picker = new DocumentManagement.DocumentPicker();

                            picker.Type = DocumentManagement.DocumentPickerType.File;

                            OMSDocument[] docs = picker.Show(ActiveWindow);
                            if (docs != null && docs.Length > 0)
                            {
                                return docs[0].GetLatestVersion();
                            }
                            else
                            {
                                cancel = true;
                                return null;
                            }

                        }
                        else
                        {
                            ThrowUnsavedDocumentActionException();
                            return null;
                        }
                    }
                case DocumentCompareType.Client:
                    {
                        if (current_doc != null)
                        {
                            DocumentManagement.DocumentPicker picker = new DocumentManagement.DocumentPicker();

                            picker.Type = DocumentManagement.DocumentPickerType.Client;

                            OMSDocument[] docs = picker.Show(ActiveWindow);
                            if (docs != null && docs.Length > 0)
                            {
                                return docs[0].GetLatestVersion();
                            }
                            else
                            {
                                cancel = true;
                                return null;
                            }
                        }
                        else
                        {
                            ThrowUnsavedDocumentActionException();
                            return null;
                        }
                    }
                case DocumentCompareType.Recent:
                    {
                        DocumentManagement.DocumentPicker picker = new DocumentManagement.DocumentPicker();

                        picker.Type = DocumentManagement.DocumentPickerType.LatestOpen;

                        OMSDocument[] docs = picker.Show(ActiveWindow);
                        if (docs != null && docs.Length > 0)
                        {
                            return docs[0].GetLatestVersion();
                        }
                        else
                        {
                            cancel = true;
                            return null;
                        }
                    }
                case DocumentCompareType.Any:
                    {
                        DocumentManagement.DocumentPicker picker = new DocumentManagement.DocumentPicker();

                        picker.Type = DocumentManagement.DocumentPickerType.Search;

                        OMSDocument[] docs = picker.Show(ActiveWindow);
                        if (docs != null && docs.Length > 0)
                        {
                            return docs[0].GetLatestVersion();
                        }
                        else
                        {
                            cancel = true;
                            return null;
                        }
                    }
            }

            return null;

        }

        protected virtual object CompareDocument(object master, object child)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGDCMCMPNSP", "Document Comparing Not Supported", "").Text);
        }

        private enum DocumentCompareType
        {
            Latest,
            Specific,
            File,
            Client,
            Recent,
            Any
        }

        #endregion

        #region Link Documents

        /// <summary>
        /// Links the document to the specified client data.
        /// </summary>
        /// <param name="obj">The document object.</param>
        /// <param name="preclink">The precedent and associate to link to.</param>
        /// <param name="mergeFields">If true, will attempt to merge the fields.</param>
        public void LinkDocument(object obj, PrecedentLink preclink, bool mergeFields)
        {
            //Activate the Application
            try
            {
                ActivateApplication();
            }
            catch { }

            Associate assoc = preclink.Associate;
            Precedent prec = preclink.Precedent;

            AttachDocumentVars(obj, false, assoc);

            //If the base precedent has not yet been set then set it.
            long baseprec = GetDocVariable(obj, BASEPRECEDENT, 0);
            if (baseprec == 0)
            {
                baseprec = prec.ID;
                SetDocVariable(obj, BASEPRECEDENT, prec.ID);
            }

            //If the base precedent has not yet been set then set it.
            long lastprec = GetDocVariable(obj, PRECEDENT, 0);
            if (baseprec != 0)
            {
                if (baseprec != prec.ID)
                {
                    lastprec = prec.ID;
                    SetDocVariable(obj, PRECEDENT, prec.ID);
                }
            }

            //Only do the following field refresh and base type property setting
            //if the precedent is NOT text only.
            if (prec.TextOnly == false)  //Is Master Template
            {
                SetDocVariable(obj, TYPE, prec.PrecedentType);

                //Try and position the cursor.
                PositionCursor();

            }

            if (mergeFields)
            {
                //Added in relation to WI 7707 - Outlook Precedent with prompt fields ask 3 times - does not occur in Word/Excel
                if (obj.GetType().Name == "OutlookMail" && Convert.ToString(GetDocVariable(obj, "FIRST_TIME_MERGE_DONE")) == "True")
                {
                    //Dont link the document again!
                }
                else
                {
                    UpdateDocFields(obj, preclink);
                    if (obj.GetType().Name == "OutlookMail")
                        SetDocVariable(obj, "FIRST_TIME_MERGE_DONE", "True");
                }
            }

            SetWindowCaption(obj, GetWindowCaption(obj));
        }

        /// <summary>
        /// Link the document only to the client.
        /// </summary>
        /// <param name="obj">The document object.</param>
        /// <param name="preclink">The precedent and associate to link to.</param>
        public virtual void LinkDocument(object obj, PrecedentLink preclink)
        {
            LinkDocument(obj, preclink, true);
        }

        #endregion

        #region Captured Events

        /// <summary>
        /// Captures the field parsers UI propmp event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _fieldParser_Prompt(FieldParser sender, FieldParserPromptEventArgs e)
        {
            string existing = Convert.ToString(GetDocVariable(this, e.FieldName, "")).Trim();
            if (existing == "" || Refreshing == true)
            {
                RefreshedFields.Add(e.FieldName, true);
                if (e.Required)
                {
                    string tmp = "";
                    while (tmp == "" || tmp == InputBox.CancelText)
                    {
                        tmp = InputBox.Show(ActiveWindow, e.Message, FWBS.OMS.Global.ApplicationName, existing, 0, e.Required);
                        if (tmp == "" || tmp == InputBox.CancelText)
                        {
                            MessageBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("REQUIREDINFO", "You Must enter a Value this is a Required Field!", ""), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                        }

                    }

                    e.Result = tmp;
                }
                else
                {
                    string tmp = "";
                    tmp = InputBox.Show(ActiveWindow, e.Message, FWBS.OMS.Global.ApplicationName, existing, 0, e.Required);

                    if (tmp == InputBox.CancelText) tmp = "";
                    e.Result = tmp;
                }
            }
            else
                e.Result = existing;
        }

        /// <summary>
        /// Captures the field parsers UI propmp event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _fieldParser_ListPrompt(FieldParser sender, FieldParserListPromptEventArgs e)
        {
            try
            {
                bool refreshing = false;
                bool pick = false;

                SearchEngine.SearchList sch = null;
                Services.Searches schfrm = null;
                FWBS.Common.KeyValueCollection ret = null;

                if (e.Code == FieldParser.FieldPrefixAppointment)
                {
                    Appointment apnt = null;
                    refreshing = (RefreshedFields.Contains(APPID + SEP + e.Key) == false && Refreshing == true);
                    int apntid = Convert.ToInt32(GetDocVariable(this, APPID + SEP + e.Key, -1));
                    if (refreshing)
                    {
                        RefreshedFields.Add(APPID + SEP + e.Key, true);
                        pick = true;
                    }

                    if (apntid == -1) pick = true;

                    if (apntid != 0)
                    {
                        if (pick)
                        {
                            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
                            pars.Add(new FWBS.Common.KeyValueItem("CboType", DBNull.Value));
                            pars.Add(new FWBS.Common.KeyValueItem("CCboAllDay", DBNull.Value));
                            pars.Add(new FWBS.Common.KeyValueItem("DteStartDate", DBNull.Value));
                            pars.Add(new FWBS.Common.KeyValueItem("DteEndDate", DBNull.Value));
                            sch = new SearchEngine.SearchList(FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Appointments), sender.CurrentFile, pars);
                            sch.Search(false);
                            if (sch.ResultCount == 1)
                            {
                                ret = sch.Select(0);
                            }
                            else
                            {
                                schfrm = new Services.Searches(sch);
                                schfrm.AsType = false;
                                schfrm.Parent = sender.CurrentFile;
                                schfrm.Message = e.Message;
                                ret = schfrm.Show(ActiveWindow);
                            }

                            if (ret != null && ret.Count > 0)
                            {
                                try
                                {
                                    apntid = Convert.ToInt32(ret["APPID"].Value);
                                    apnt = Appointment.GetAppointment(apntid);
                                }
                                catch (Exception ex)
                                {
                                    ErrorBox.Show(ActiveWindow, ex);
                                    apnt = null;
                                }
                            }

                        }
                        else
                            apnt = Appointment.GetAppointment(apntid);

                        if (apnt == null)
                        {
                            if (apntid == -1)
                                apntid = 0;
                            else
                                apnt = Appointment.GetAppointment(apntid);

                            SetDocVariable(this, APPID + SEP + e.Key, apntid);
                        }
                        else
                            SetDocVariable(this, APPID + SEP + e.Key, apnt.ID);
                    }
                    e.Result = apnt;
                }
                else if (e.Code == FieldParser.FieldPrefixAssociate)
                {

                    Associate assoc = null;
                    refreshing = (RefreshedFields.Contains(ASSOCIATE + SEP + e.Key) == false && Refreshing == true);
                    long associd = GetDocVariable(this, ASSOCIATE + SEP + e.Key, -1);
                    if (refreshing)
                    {
                        RefreshedFields.Add(ASSOCIATE + SEP + e.Key, true);
                        pick = true;
                    }

                    if (associd == -1) pick = true;

                    if (associd != 0)
                    {
                        if (pick)
                        {
                            FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
                            pars.Add("CONTTYPE", e.Filter[0]);
                            pars.Add("ASSOCTYPE", e.Filter[1]);
                            sch = new SearchEngine.SearchList(FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.AssociateFilter), sender.CurrentFile, pars);
                            sch.Search(false);
                            if (sch.ResultCount == 1)
                            {
                                ret = sch.Select(0);
                            }
                            else
                            {
                                schfrm = new Services.Searches(sch);
                                schfrm.Message = e.Message;
                                schfrm.Parent = sender.CurrentFile;
                                schfrm.AsType = false;
                                ret = schfrm.Show(ActiveWindow);
                            }

                            if (ret != null && ret.Count > 0)
                            {
                                try
                                {
                                    associd = Convert.ToInt64(ret["ASSOCID"].Value);
                                    assoc = Associate.GetAssociate(associd);
                                }
                                catch (Exception ex)
                                {
                                    ErrorBox.Show(ActiveWindow, ex);
                                    assoc = null;
                                }
                            }

                            //## DMB 12/8/2003 if associate is inactive prompt to see if they want to make active
                            if (assoc != null && assoc.Active == false)
                            {
                                DialogResult res = MessageBox.ShowYesNoQuestion("ASSOCMAKEACTIVE", "Selected associate is inactive, would you like to make it active.", null);

                                if (res == DialogResult.Yes)
                                {
                                    assoc.SetExtraInfo("assocActive", true);
                                    assoc.Update();
                                }
                            }
                            //## DMB remove up to here if there are problems
                        }
                        else
                        {
                            assoc = Associate.GetAssociate(associd);
                        }

                        if (assoc == null)
                        {
                            if (associd == -1)
                                associd = 0;
                            else
                                assoc = Associate.GetAssociate(associd);

                            SetDocVariable(this, ASSOCIATE + SEP + e.Key, associd);

                        }
                        else
                            SetDocVariable(this, ASSOCIATE + SEP + e.Key, assoc.ID);

                    }
                    e.Result = assoc;
                }
                else
                {
                    refreshing = (RefreshedFields.Contains(e.Code + SEP + e.Key) == false && Refreshing == true);
                    string xml = GetDocVariable(this, e.Code + SEP + e.Key, "");
                    if (refreshing)
                    {
                        RefreshedFields.Add(e.Code + SEP + e.Key, true);
                        xml = "";
                    }
                    if (xml == "")
                    {
                        FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
                        sch = new SearchEngine.SearchList(e.Code, sender.CurrentFile, pars);
                        if (e.Filter.Any())
                        {
                            sch.ExternalFilter = e.Filter[0].ToString();
                        }
                        sch.Search(false);
                        if (sch.ResultCount == 1)
                        {
                            ret = sch.Select(0);
                        }
                        else
                        {
                            schfrm = new Services.Searches(sch);
                            schfrm.AsType = false;
                            schfrm.Parent = sender.CurrentFile;
                            schfrm.Message = e.Message;
                            ret = schfrm.Show(ActiveWindow);
                        }

                        if (ret == null)
                            SetDocVariable(this, e.Code + SEP + e.Key, "");
                        else
                        {
                            System.IO.TextWriter txt = new System.IO.StringWriter();
                            System.Xml.XmlWriter wr = new System.Xml.XmlTextWriter(txt);
                            wr.WriteStartElement("KeyValues");

                            for (int ctr = 0; ctr < ret.Count; ctr++)
                            {
                                wr.WriteStartElement("KeyValue");
                                wr.WriteAttributeString("key", ret[ctr].Key);
                                wr.WriteAttributeString("value", Convert.ToString(ret[ctr].Value));
                                wr.WriteEndElement();
                            }

                            wr.WriteEndElement();

                            SetDocVariable(this, e.Code + SEP + e.Key, txt.ToString());

                            wr.Close();
                            txt.Close();
                        }

                    }
                    else
                    {
                        System.IO.TextReader txt = new System.IO.StringReader(xml);
                        System.Xml.XmlReader rdr = new System.Xml.XmlTextReader(txt);
                        ret = new FWBS.Common.KeyValueCollection();

                        while (rdr.Read())
                        {
                            if (rdr.Name == "KeyValue")
                            {
                                string key = rdr.GetAttribute("key");
                                string val = rdr.GetAttribute("value");
                                ret.Add(key, val);
                            }
                        }

                        rdr.Close();
                        txt.Close();
                    }
                    e.Result = ret;
                }
            }
            catch (Exception ex)
            {
                e.Result = null;
                ErrorBox.Show(ex);
            }
        }

        #endregion

        #region Table Routines

        public virtual void BuildTable(object obj, string code, DataView vw, bool includeHeader)
        {
            CheckObjectIsDoc(ref obj);

            System.Text.StringBuilder tbl = new System.Text.StringBuilder();

            System.Globalization.NumberFormatInfo numfmt = Session.CurrentSession.DefaultCurrencyFormat;
            System.Globalization.DateTimeFormatInfo datefmt = Session.CurrentSession.DefaultDateTimeFormat;

            Associate assoc = GetCurrentAssociate(obj);
            if (assoc != null)
            {
                numfmt = assoc.OMSFile.CurrencyFormat;
                datefmt = assoc.OMSFile.DateTimeFormat;
            }

            char tab = (char)9;
            string ret = Environment.NewLine;

            if (includeHeader)
            {
                int ctr = 0;
                foreach (DataColumn col in vw.Table.Columns)
                {
                    ctr++;
                    tbl.Append((col.Caption == "" ? col.ColumnName : col.Caption));
                    if (ctr < vw.Table.Columns.Count) tbl.Append(tab);
                }
            }

            tbl.Append(ret);

            foreach (DataRowView row in vw)
            {
                int ctr = 0;
                foreach (DataColumn col in vw.Table.Columns)
                {
                    ctr++;
                    string alignment = Convert.ToString(col.ExtendedProperties["ALIGNMENT"]);
                    string format = Convert.ToString(col.ExtendedProperties["FORMAT"]);
                    tbl.Append(Convert.ToString(FieldParser.GetFormattedValue(row[col.ColumnName], numfmt, datefmt, format)));
                    if (ctr < vw.Table.Columns.Count) tbl.Append(tab);
                }

                if (ctr < vw.Count) tbl.Append(ret);
            }

            SetBookmark(obj, code, tbl.ToString());
        }

        #endregion

        #region Refactored Commands

        private void ShowFileInfoCommand(string[] pars)
        {
            if (!Services.CheckLogin())
                return;

            var DisplayTab = string.Empty;
            OMSFile file = null;

            if (pars.Length > 2)
                DisplayTab = pars[2];
            if (pars.Length > 3)
            {
                long id;
                if (long.TryParse(pars[3], out id))
                    file = OMSFile.GetFile(id);
            }

            if (file == null)
                FWBS.OMS.UI.Windows.Services.ShowFile(ActiveWindow, true, DisplayTab);
            else
                FWBS.OMS.UI.Windows.Services.ShowFile(ActiveWindow, file, DisplayTab);

        }

        private void ShowClientInfoCommand(string[] pars)
        {
            if (!Services.CheckLogin())
                return;

            var DisplayTab = string.Empty;
            Client client = null;

            if (pars.Length > 2)
                DisplayTab = pars[2];

            if (pars.Length > 3)
            {
                long id;
                if (long.TryParse(pars[3], out id))
                {
                    try
                    {
                        client = Client.GetClient(id);
                    }
                    catch (OMSException2)
                    {
                    }
                }

                if (client == null)
                    client = Client.GetClient(pars[3]);
            }

            if (client == null)
                FWBS.OMS.UI.Windows.Services.ShowClient(ActiveWindow, true, DisplayTab);
            else
                FWBS.OMS.UI.Windows.Services.ShowClient(ActiveWindow, client, DisplayTab);

        }


        private bool TemplateStartCommand(string[] pars)
        {
            if (!Services.CheckLogin())
                return false;

            Associate assoc = null;
            Precedent prec = null;
            var picker = String.Empty;
            var prectype = String.Empty;

            if (pars.Length > 2)
            {
                prectype = pars[2];

                long id;
                if (long.TryParse(pars[2], out id))
                {
                    try
                    {
                        prec = Precedent.GetPrecedent(id);
                    }
                    catch (OMSException2)
                    {
                    }
                }
            }

            if (pars.Length > 3)
            {
                picker = pars[3];
            }

            if (pars.Length > 4)
            {
                long id;
                if (long.TryParse(pars[4], out id))
                {
                    try
                    {
                        if (picker == "CLIENT")
                        {
                            var file = OMSFile.GetFile(id);
                            assoc = file.DefaultAssociate;
                        }
                        else
                            assoc = Associate.GetAssociate(id);
                    }
                    catch (OMSException2)
                    {
                    }
                }
            }

            if (assoc == null)
            {
                if (picker == "CLIENT")
                    assoc = FWBS.OMS.UI.Windows.Services.SelectDefaultAssociate();
                else
                    assoc = FWBS.OMS.UI.Windows.Services.SelectAssociate();
            }

            if (assoc == null)
                return false;

            Common.KeyValueCollection jobpars = new Common.KeyValueCollection();

            if (Array.IndexOf<string>(pars, "TO", 2) > -1)
                jobpars.Add("TO", true);

            if (Array.IndexOf<string>(pars, "CC", 2) > -1)
                jobpars.Add("CC", true);

            if (Array.IndexOf<string>(pars, "BCC", 2) > -1)
                jobpars.Add("BCC", true);

            PrecedentJob job = null;

            if (prec == null)
                job = FWBS.OMS.UI.Windows.Services.TemplateStart(this, pars[2].ToString(), assoc, jobpars);
            else
                job = FWBS.OMS.UI.Windows.Services.TemplateStart(this, prec, assoc, jobpars);

            if (job == null)
                return false;

            if (job.HasError)
            {
                FWBS.OMS.UI.Windows.MessageBox.Show(job.ErrorMessage, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void OpenDocumentCommand(object obj, string[] pars)
        {
            if (!Services.CheckLogin())
                return;

            if (pars.Length <= 2)
            {
                OpenCommand(obj, pars);
                return;
            }

            long docid;

            string sdocid = pars[2].Trim();
            string label = String.Empty;

            if (pars.Length > 3)
                label = pars[3];

            int versionseploc = sdocid.IndexOf('.');
            if (versionseploc > -1)
            {
                label = sdocid.Substring(versionseploc + 1).Trim();
                sdocid = sdocid.Substring(0, versionseploc).Trim();
            }

            if (!long.TryParse(sdocid, out docid))
            {
                OpenCommand(obj, pars);
                return;
            }
            OMSDocument doc = OMSDocument.GetDocument(docid);

            if (!String.IsNullOrEmpty(label))
            {
                DocumentVersion version = (DocumentVersion)doc.GetVersion(label);
                if (version == null)
                    throw new FWBS.OMS.DocumentManagement.Storage.StorageException("MSGDOCVERINV", "Version '%1%' does not exist for document '%2%'.", null, label, sdocid);

                StorageSettingsCollection settings = doc.GetStorageProvider().GetDefaultSettings(version, SettingsType.Fetch);
                VersionFetchSettings versettings = settings.GetSettings<FWBS.OMS.DocumentManagement.Storage.VersionFetchSettings>();

                //Make sure specific version is specified otherwise the default OpenDocument 
                //class will use latest version.
                versettings.Version = VersionFetchSettings.FetchAs.Specific;
                versettings.VersionLabel = label;
                version.ApplySettings(settings);
                FWBS.OMS.UI.Windows.Services.OpenDocument(version, DocOpenMode.Edit);
            }
            else
            {
                FWBS.OMS.UI.Windows.Services.OpenDocument(doc, DocOpenMode.Edit);
                return;
            }

        }

        private void OpenCommand(object obj, string[] pars)
        {
            if (pars.Length > 2)
            {

                switch (pars[2])
                {
                    case "VERSION":
                        {
                            Document doc = GetCurrentDocument(obj);
                            if (doc != null)
                                Services.OpenDocument(doc, DocOpenMode.Edit, false);
                            else
                                ThrowUnsavedDocumentActionException();
                        }
                        break;
                    case "LOCAL":
                        {
                            bool changes = false;
                            if (pars.Length > 3)
                            {
                                switch (pars[3])
                                {
                                    case "CHANGES":
                                        changes = true;
                                        break;
                                }
                            }
                            DocumentManagement.DocumentPicker picker = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker();
                            System.IO.FileInfo[] files = picker.ShowLocal(ActiveWindow, changes);
                            if (files != null)
                            {
                                foreach (System.IO.FileInfo file in files)
                                {
                                    if (System.IO.File.Exists(file.FullName))
                                    {
                                        Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", file), InputValidation.ValidateOpenFileInput);
                                    }
                                }
                            }

                        }
                        break;
                    default:
                        {
                            Services.ShowOpenDocument(this.ActiveWindow, this);
                        }

                        break;
                }
            }
            else
                Services.ShowOpenDocument(this.ActiveWindow, this);
        }


        #endregion
    }

}
