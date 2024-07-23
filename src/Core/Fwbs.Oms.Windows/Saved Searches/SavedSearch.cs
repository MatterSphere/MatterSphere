using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using FWBS.Common;
using FWBS.Common.UI;
using FWBS.OMS.UI.Windows;



namespace FWBS.OMS
{
    public class SavedSearch : CommonObject
    {
        #region Properties
        /// <summary>
        /// Gets the unique OMS id for the task item.
        /// </summary>
        public long ID
        {
            get
            {
                return Convert.ToInt64(UniqueID);
            }
        }

        public string Type
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ssType"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("ssType", DBNull.Value);
                else
                {
                    SetExtraInfo("ssType", value);
                }
            }
        }

        public string OMSObjectCode
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ssOMSObjectCode"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("ssOMSObjectCode", DBNull.Value);
                else
                {
                    SetExtraInfo("ssOMSObjectCode", value);
                }
            }
        }

        public string Name
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ssName"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("ssName", DBNull.Value);
                else
                {
                    SetExtraInfo("ssName", value);
                }
            }
        }


        public string Object
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ssObject"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("ssObject", DBNull.Value);
                else
                {
                    SetExtraInfo("ssObject", value);
                }
            }
        }
        /// <summary>
        /// Currently not used - possibility for the future
        /// </summary>
        public long? ObjectID
        {
            //Always set as NULL - this value is not used and asks questions around whether a saved search could relate to, say, ClientID 1 on SearchList X

            get
            {
                return Convert.ToInt64(GetExtraInfo("ssObjectID"));
            }
            set
            {
                SetExtraInfo("ssObjectID", DBNull.Value);
            }
        }

        public string CriteriaXML
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ssCriteriaXML"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("ssCriteriaXML", DBNull.Value);
                else
                {
                    SetExtraInfo("ssCriteriaXML", value);
                }
            }
        }

        public User CreatedBy
        {
            get
            {
                return User.GetUser(CreatedByID);
            }
        }

        /// <summary>
        /// Gets the fee earner identifier.
        /// </summary>
        public int CreatedByID
        {
            get
            {
                return Common.ConvertDef.ToInt32(GetExtraInfo("createdby"), -1);
            }
        }


        public Common.DateTimeNULL Created
        {
            get
            {
                return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("created"), DBNull.Value);
            }
        }

        public bool ForcedSearch
        {
            get
            {
                return Convert.ToBoolean(GetExtraInfo("ssForcedSave"));
            }
            set
            {
                //If Forced Search, then the ssName is set to be a Guid
                if (value == true)
                {
                    SetExtraInfo("ssName", Guid.NewGuid());
                }

                SetExtraInfo("ssForcedSave", value);
                
            }
        }


        public bool GlobalSearch
        {
            get
            {
                return Convert.ToBoolean(GetExtraInfo("ssGlobalsave"));
            }
            set
            {                

                SetExtraInfo("ssGlobalsave", value);

            }
        }

        
        #endregion

        #region CommonObject Implementation

        // FieldActive and DefaultForm not yet implemented

        public override string FieldPrimaryKey
        {
            get
            {
                return "ssID";
            }
        }

        protected override string PrimaryTableName
        {
            get
            {
                return "SAVEDSEARCH";
            }
        }

        protected override string SelectStatement
        {
            get
            {
                return "select * from dbSavedSearches";
            }
        }

        #endregion

        #region "Methods"

        public SavedSearch()
            : base()
        {
            SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
            SetExtraInfo("Created", DateTime.Now);
        }

        internal SavedSearch(long id)
            : base(id)
        {
        }


        #region Static Methods

        /// <summary>
        /// Gets a saved search based on a saved search identifier.
        /// </summary>
        /// <param name="id">The identifier of the saved search.</param>
        /// <returns>A save search object.</returns>
        public static SavedSearch GetSavedSearch(long id)
        {
            return new SavedSearch(id);
        }


        



        #endregion

        public override void Update()
        {
            SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
            SetExtraInfo("Updated", DateTime.Now);
            base.Update();
        }


        #endregion
    }
    
    public class SavedSearches
    {
        private static string _enterDescriptionMsg = Session.CurrentSession.Resources.GetMessage("MSGSSENTERDESC", "Please enter a Save Search Description", "").Text;
        private static ResourceItem _globalSearchRes = Session.CurrentSession.Resources.GetResource("MSGSSGLBSAVE", "Would you like to save this as a global search?", "");
        private static string _controlErrorMsg = Session.CurrentSession.Resources.GetMessage("MSGSSCTRLERR", "One or more controls could not be found. This could be due design change etc.", "").Text;
        private static string _updateLoadedSearchMsg = Session.CurrentSession.Resources.GetMessage("MSGSSUPDLSTLOAD", "Do you want to update the last loaded search?", "").Text; 

        #region "Static Methods"

        #region "Saving"
        public static void SaveForcedSearch(string criteriaXML, string omsObjectCode, string type, string parentObject, long? parentObjectID)
        {
            SaveSearch(Guid.NewGuid().ToString(), criteriaXML, omsObjectCode, type, parentObject, parentObjectID, false, true);
        }

        public static void SaveSearch(string description, string criteriaXML, string omsObjectCode, string type, string parentObject, long? parentObjectID, bool global)
        {
            SaveSearch(description, criteriaXML, omsObjectCode, type, parentObject, parentObjectID, global, false);
        }

        private static void SaveSearch(string description, string criteriaXML, string omsObjectCode, string type, string parentObject, long? parentObjectID, bool global, bool forced)
        {
            SavedSearch s = new SavedSearch();
            s.Type = type;
            s.Object = parentObject;
            s.OMSObjectCode = omsObjectCode;
            s.ObjectID = parentObjectID;
            s.CriteriaXML = criteriaXML;
            s.ForcedSearch = forced;
            s.GlobalSearch = global;
            s.Name = description;
            s.Update();
        }
        #endregion
        
        #region "Opening"
        public static SavedSearch OpenSavedSearch(IWin32Window owner, string omsObjectCode)
        {
            //Present user with searches
            KeyValueCollection keys = new KeyValueCollection();
            keys.Add("OMSObjectCode", omsObjectCode);
            KeyValueCollection kvc = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(owner, "GRPSAVEDSEARCH", null, keys);
            if (kvc == null)
                return null;

            //Create Saved Search from ID returned from SearchList
            return FWBS.OMS.SavedSearch.GetSavedSearch(Convert.ToInt64(kvc["ssID"].Value));
        }

        public static void OpenSavedSearchAndPopulateForm(IWin32Window owner, string omsObjectCode, EnquiryForm searchForm, ref long lastOpenedSearch)
        {
            SavedSearch _openedSearch = SavedSearches.OpenSavedSearch(owner, omsObjectCode);

            if (_openedSearch == null)
            {

                lastOpenedSearch = -1;
                return;

            }

            //Reset the enquiryform when loading so any populated values are removed
            searchForm.RefreshItem();
            //Set the last opened search for this search list, so this can be used on a Save.          
            lastOpenedSearch = _openedSearch.ID;
            SavedSearches.Tools.PopulateFormWithSavedSearchCriteria(searchForm, _openedSearch);
        }

        #endregion

        #endregion

        #region "Tools"
        public class Tools
        {
            private static int CompareTabIndex(Control c1, Control c2)
            {
                return c1.TabIndex.CompareTo(c2.TabIndex);
            }
            /// <summary>
            /// Generates the XML string for the entered Search criteria on the Enquiry Form
            /// </summary>
            /// <param name="searchForm"></param>
            /// <returns></returns>
            ///         
            
            public static string BuildSearchCriteriaXML(EnquiryForm searchForm)
            {
                string xml = string.Format(@"<SavedSearch enquiryForm = ""{0}"" version = ""{1}""><Controls>", searchForm.Code, searchForm.Version);
                IBasicEnquiryControl2 _temp;

                /* controls not saving to xml based on tabindex so it's creating problems when reloading*/

                List<Control> Controls = new List<Control>();

                foreach (Control c in searchForm.Controls)
                {
                    Controls.Add(c); 
                }

                Controls.Sort(new Comparison<Control>(CompareTabIndex));  



                foreach (Control c in Controls)
                {
                    try
                    {
                        _temp = searchForm.GetIBasicEnquiryControl2(c.Name, EnquiryControlMissing.Exception);
                    }
                    catch (FWBS.OMS.OMSException2)
                    {
                        continue;
                    }

                    if (_temp.Value != DBNull.Value)
                        xml += string.Format(@"<Control name = ""{0}"">{1}</Control>", c.Name, _temp.Value);
                }

                xml += @" </Controls></SavedSearch>";
                return xml;
            }


            public static void PopulateFormWithSavedSearchCriteria(EnquiryForm searchForm, SavedSearch savedSearch)
            {
                PopulateFormWithSavedSearchCriteria(searchForm, savedSearch.CriteriaXML);
            }

            private static void PopulateFormWithSavedSearchCriteria(EnquiryForm searchForm, string criteriaXML)
            {
                //Obtain the 'Controls' node in the XML for the SavedSearch object
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(criteriaXML);
                XmlNode xmlControls = xml.DocumentElement.SelectSingleNode("/SavedSearch/Controls");

                //Populate enquiry form with values obtained from XML
                string _errors = "";
                IBasicEnquiryControl2 _temp;
                foreach (XmlNode control in xmlControls.ChildNodes)
                {
                    string _controlName = control.Attributes["name"].Value;
                    object _value = control.InnerXml;
                    _temp = searchForm.GetIBasicEnquiryControl2(_controlName);
                    if (_temp == null)
                        _errors += _controlName;
                    else
                        _temp.Value = _value;
                }
                
                //Report on any errors
                if (!String.IsNullOrWhiteSpace(_errors))
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(_controlErrorMsg + "\n\n" + _errors);
                }
            }

            /// <summary>
            /// Prompts user to enter a description for the Saved Search
            /// </summary>
            /// <param name="owner"></param>
            /// <returns></returns>
            public static string SaveSearchDescription(IWin32Window owner)
            {
                string _desc = InputBox.Show(owner, _enterDescriptionMsg, "", "", 100, true, true);
                if (_desc == InputBox.CancelText)
                    return "";
                else 
                    return _desc;
            }

            /// <summary>
            /// Prompts user to enter a description for the Saved Search
            /// </summary>
            /// <param name="owner"></param>
            /// <returns></returns>
            public static string SaveSearchDescription(IWin32Window owner, string defaultValue)
            {
                string _desc = InputBox.Show(owner, _enterDescriptionMsg, "", defaultValue, 100, true, true);
                if (_desc == InputBox.CancelText)
                    return "";
                else
                    return _desc;
            }
            
            public static bool IsGlobalSearchRequired()
            {
                return (FWBS.OMS.UI.Windows.MessageBox.Show(null, _globalSearchRes, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
            }

            public static bool IsUpdateToLastLoadedSearchRequired()
            {
                return (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion(_updateLoadedSearchMsg) == DialogResult.Yes);       
            }
            
            /// <summary>
            /// Returns the object type and ID
            /// </summary>
            /// <param name="omsObjectCode">Code of the OMS object</param>
            /// <param name="parent">Parent Object to be used</param>
            /// <param name="objectType">Returns the OMS Object type (FILE, CLIENT etc.)</param>
            /// <param name="objectID">Returns the Parent ID</param>
            public static void GetParentObjectTypeAndID(string omsObjectCode, object parent, ref string objectType, ref long? objectID)
            {

                if (parent== null)
                {
                    objectType = "NOPARENT";
                    objectID = null;
                }
                else if (parent.GetType() == typeof(OMSFile))
                {
                    objectType = "FILE";
                    objectID = ((FWBS.OMS.OMSFile)parent).ID;
                }
                else if (parent.GetType() == typeof(Client))
                {
                    objectType = "CLIENT";
                    objectID = ((FWBS.OMS.Client)parent).ID;
                }
                else if (parent.GetType() == typeof(Associate))
                {
                    objectType = "ASSOCIATE";
                    objectID = ((FWBS.OMS.Associate)parent).ID;
                }
                else if (parent.GetType() == typeof(Contact))
                {
                    objectType = "CONTACT";
                    objectID = ((FWBS.OMS.Contact)parent).ID;
                }
                else if (parent.GetType() == typeof(User))
                {
                    objectType = "USER";
                    objectID = ((FWBS.OMS.User)parent).ID;
                }
                else if (parent.GetType() == typeof(FeeEarner))
                {
                    objectType = "FEEEARNER";
                    objectID = ((FWBS.OMS.FeeEarner)parent).ID;
                }

                else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("MSGNTSUPP", "Object (%1%) not supported for save searching", "", omsObjectCode).Text);
            }

        }
        #endregion
    }
}
