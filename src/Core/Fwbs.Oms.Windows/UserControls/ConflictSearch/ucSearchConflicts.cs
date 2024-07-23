using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using FWBS.Common;
using FWBS.Common.UI;
using FWBS.OMS.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.UI.UserControls;
using FWBS.OMS.UI.UserControls.ConflictSearch;

using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This user control is used for searching and listing conflicts for existing client / contact within the system.
    /// </summary>
    public partial class ucSearchConflicts : UserControl, IConflictSearcher
    {
        #region Fields

        private bool _captureException = true;

        private Enquiry _enquirydisconnect;

        private Hashtable _incsearch = new Hashtable();

        private KeyValueCollection _parameters = null;

        private object _parent = null;

        private SearchList _searchList = null;

        private string _searchlistcode = string.Empty;

        private IBasicEnquiryControl2 _searchQueryControl;

        private string _searchQueryControlName;

        private DataTable dtCT = new DataTable();

        private Label lblCaption;

        private KeyValueCollection _returnValues;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ucSearchConflicts"/> class.
        /// </summary>
        public ucSearchConflicts()
        {
            this.InitializeComponent();
            this.EnquiryForm = new EnquiryForm()
            {
                IsDirty = false,
                Location = new System.Drawing.Point(0, 0),
                Name = "EnquirySearchForm",
                Size = new System.Drawing.Size(500, 150),
                TabIndex = 0,
                Tag = "InternalEnquirySearch",
                ToBeRefreshed = false
            };
        }

        #endregion

        #region Events

        /// <summary>
        /// An event that gets raised when the search is completed.
        /// </summary>
        public event SearchCompletedEventHandler SearchCompleted = null;

        /// <summary>
        /// An event that gets raised when the search list object is loaded.
        /// </summary>
        public event EventHandler SearchListLoad = null;

        /// <summary>
        /// This event gets raised when Open button is clicked.
        /// </summary>
        public event EventHandler OpenButtonClicked = null;

        /// <summary>
        /// This event gets raised when an item gets successfully picked form the search list.
        /// </summary>
        public event EventHandler ItemSelected = null;

        public event SearchStateChangedEventHandler StateChanged;

        public event SearchButtonEventHandler SearchButtonCommands;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current search type descriptions.
        /// </summary>
        [Browsable(false)]
        public string CurrentSearchText
        {
            get
            {
                if (this._searchList == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this._searchList.Description;
                }
            }
        }

        /// <summary>
        /// Access to the Returned Data Table.
        /// </summary>
        [Browsable(false)]
        public DataTable DataTable => this.dtCT;

        /// <summary>
        /// Gets a reference to the enquiry form that is being used as the criteria form.
        /// </summary>
        public EnquiryForm EnquiryForm
        {
            get; private set;
        }

        /// <summary>
        /// Indicates whether the EnquiryForm is refreshed on Refresh event.
        /// </summary>
        [DefaultValue(false), Category("Auto Load")]
        public bool RefreshOnEnquiryFormRefreshEvent { get; set; }

        /// <summary>
        /// Gets the search list object associated to the control.
        /// </summary>
        [Browsable(false)]
        public SearchList SearchList => this._searchList;

        /// <summary>
        /// Gets or sets a code of a search list object associated to the control.
        /// </summary>
        [DefaultValue(null), Category("Auto Load")]
        public string SearchListCode
        {
            get
            {
                return this._searchlistcode;
            }
            set
            {
                this._searchlistcode = value;
            }
        }

        /// <summary>
        /// Control to search for existing clients / contacts within the system.
        /// </summary>
        [Category("OMS")]
        [Description("Use to Set the Search Conflict Control")]
        [DefaultValue(null)]
        public IBasicEnquiryControl2 SearchQueryControl
        {
            get
            {
                return _searchQueryControl;
            }
            set
            {
                if (_searchQueryControl != value)
                {
                    if (_searchQueryControl != null)
                    {
                        _searchQueryControl.ActiveChanged -= SearchQueryControl_TextChanged;
                    }
                    _searchQueryControl = value;
                    if (_searchQueryControl != null)
                    {
                        _searchQueryControl.ActiveChanged += SearchQueryControl_TextChanged;
                    }
                }
            }
        }

        [Category("OMS")]
        [Description("Use to Set the Search Conflict Control name")]
        [DefaultValue(null)]
        public string SearchQueryControlName
        {
            get
            {
                return _searchQueryControlName;
            }
            set
            {
                _searchQueryControlName = value;
            }
        }

        /// <summary>
        /// Gets Id of selected conflict contact / client.
        /// </summary>
        public string SelectedId => this.groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)?.Name.Split('_')[1];

        /// <summary>
        /// Gets the current selected values that have been picked from the searched list.
        /// </summary>
        [Browsable(false)]
        public FWBS.Common.KeyValueCollection ReturnValues => _returnValues;

        [Browsable(false)]
        public SearchState State
        {
            get
            {
                return SearchState.Search;
            }
        }
        
        public Button cmdSearch
        {
            get { return null; }
        }

        public Button cmdSelect
        {
            get { return null; }
        }

        private UltraPeekPopup Popup { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Cancels the search process.
        /// </summary>
        public void CancelSearch()
        {
            this._searchList?.CancelSearch();
        }

        /// <summary>
        /// Performs the search method.
        /// </summary>
        public void Search()
        {
            this.Search(true, true);
        }

        /// <summary>
        /// Performs the search method.
        /// </summary>
        public void Search(bool captureError)
        {
            this.Search(captureError, true);
        }

        /// <summary>
        /// Performs the search method.
        /// </summary>
        public void Search(bool captureError, bool Async)
        {
            try
            {
                if (this._searchList == null)
                {
                    return;
                }

                this.EnquiryForm.ReBind();
                this._captureException = captureError;

                if (this._searchList.SaveSearch == SaveSearchType.Always.ToString())
                {
                    string _obj = string.Empty;
                    long? _objID = 0;
                    SavedSearches.Tools.GetParentObjectTypeAndID(
                        this.SearchList.Code,
                        this.SearchList.Parent,
                        ref _obj,
                        ref _objID);
                    SavedSearches.SaveForcedSearch(
                        SavedSearches.Tools.BuildSearchCriteriaXML(this.EnquiryForm),
                        this.SearchList.Code,
                        "SEARCHLIST",
                        _obj,
                        _objID);
                }

                this._searchList.Search(Async);
            }
            catch (Exception ex)
            {
                Application.DoEvents();
                if (captureError)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets / changes the replacement parameters.
        /// </summary>
        /// <param name="parameters">The parameter values to use.</param>
        public void SetParameters(KeyValueCollection parameters)
        {
            this._parameters = parameters;
            this._searchList?.ChangeParameters(this._parameters);
        }

        /// <summary>
        /// Sets the current search list object.
        /// </summary>
        /// <param name="code">The code of a search list object.</param>
        /// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
        /// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
        public void SetSearchList(string code, object parent, KeyValueCollection parameters)
        {
            this.SetSearchList(null, code, parent, parameters);
        }

        /// <summary>
        /// Sets the current search list object.
        /// </summary>
        /// <param name="searchList">Search list object to be used.</param>
        /// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
        public void SetSearchList(SearchList searchList, KeyValueCollection parameters)
        {
            this.SetSearchList(searchList, string.Empty, null, parameters);
        }

        /// <summary>
        /// Populates the search type.
        /// </summary>
        /// <param name="type">Search list types.  For instance client related searches will only be displayed in the combo box.</param>
        /// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
        /// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
        public void SetSearchListType(string type, object parent, FWBS.Common.KeyValueCollection parameters)
        {
            SetSearchListType(type, parent, parameters, true);
        }

        /// <summary>
        /// Performs the select method.
        /// </summary>
        /// <returns>A boolean flag that indicates whether a valid item has been picked from the list.</returns>
        public bool SelectRowItem(int index)
        {
            try
            {
                if (_searchList == null) return false;
                _returnValues = _searchList.Select(index);
            }
            catch
            {
                return false;
            }

            if (_returnValues == null || _returnValues.Count == 0)
                return false;
            else
            {
                OnItemSelected(this, new ConflictSelectedEventArgs(DataTable.Rows[index]));
                return true;
            }
        }

        public bool SelectRowItem()
        {
            return false;
        }

        #endregion

        #region Non-public methods

        /// <summary>
        /// Populates the search type.
        /// </summary>
        /// <param name="type">Search list types.  For instance client related searches will only be displayed in the combo box.</param>
        /// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
        /// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
        /// <param name="force">Forces the types to be refreshed.</param>
        internal void SetSearchListType(string type, object parent, FWBS.Common.KeyValueCollection parameters, bool force)
        {
            _parent = parent;
            _parameters = parameters;

            DataView dv = FWBS.OMS.SearchEngine.SearchList.GetSearchLists(type).DefaultView;
            dv.Sort = "schdesc";
            if (dv.Count == 0)
                throw new OMSException2("ERRSGRPNOTFND", "Search Group '%1%' not found...", new Exception(), true, type);
            SetSearchList(Convert.ToString(dv[0]["schcode"]), _parent, _parameters);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.components?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// A method used to raise the SearchCompleted event.
        /// </summary>
        protected void OnSearchCompleted()
        {
            if (this.SearchCompleted != null)
            {
                int count = 0;
                System.Text.StringBuilder ext = new System.Text.StringBuilder();
                if (this._searchList != null && this._searchList.CriteriaForm != null)
                {
                    DataTable questions = this._searchList.CriteriaForm.Source.Tables["QUESTIONS"];
                    DataTable criteria = this._searchList.CriteriaForm.Source.Tables["DATA"];

                    foreach (DataRow q in questions.Rows)
                    {
                        try
                        {
                            string val = Convert.ToString(criteria.Rows[0][Convert.ToString(q["quname"])]);
                            if (val == string.Empty)
                            {
                                continue;
                            }

                            ext.Append(Convert.ToString(q["qudesc"]));
                            ext.Append(" - ");
                            ext.Append(val);
                            ext.Append(Environment.NewLine);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                count = this.SearchList.ResultCount;
                this.SearchCompleted(this, new SearchCompletedEventArgs(count, ext.ToString()));
            }
        }

        /// <summary>
        /// A method used to raise the OpenButtonClicked event.
        /// </summary>
        protected void OnOpenButtonClicked(object sender, EventArgs e)
        {
            PopupClose();
            this.OpenButtonClicked?.Invoke(sender, e);
        }

        /// <summary>
        /// A method used to raise the ItemSelected event.
        /// </summary>
        protected void OnItemSelected(object sender, ConflictSelectedEventArgs e)
        {
            PopupClose();
            this.ItemSelected?.Invoke(sender, e);
        }

        /// <summary>
        /// A method used to raise the search list load event
        /// </summary>
        protected void OnSearchListLoad()
        {
            this.SearchListLoad?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, new SearchStateEventArgs(this.State));
        }

        private void But_OnClick(object sender, EventArgs e)
        {
            OnOpenButtonClicked(sender, e);
        }

        private void CheckResultsVisibility()
        {
            if (this.SearchQueryControl != null && string.IsNullOrWhiteSpace(this.SearchQueryControl.Text))
            {
                this.lblWarning.Visible = false;
                this.lblCaption.Text = string.Empty;
                this.pnlResults.Visible = false;
            }
            else if (this._searchList.ResultCount <= 0)
            {
                this.lblWarning.Visible = false;
                this.pnlResults.Visible = false;
            }
            else
            {
                this.lblWarning.Visible = true;
                this.pnlResults.Visible = true;
            }
        }

        private void Enquiry_Refreshed(object sender, EventArgs e)
        {
            Trace.WriteLine($"Enquiry Form Refreshed {this.SearchList.Code}");
            this.Search(true, true);
        }

        private void GenerateResults()
        {
            int itemHeight = 0;
            this.groupBox1.Controls.Clear();
            this.pnlOpens.Controls.Clear();
            this.pnlLabels.Controls.Clear();

            foreach (DataRow dataRow in this.DataTable.Rows)
            {
                string itemName = string.Empty;

                if (this._searchList.Code == "SCHCLISEACHADV")
                {
                    string query =
                        $"select clname, cladd1 as addLine1 from vwDBClientHeader where clid = '{dataRow[0]}'";

                    DataTableExecuteParameters pars = new DataTableExecuteParameters { Sql = query };
                    DataTable dtbl = Session.CurrentSession.CurrentConnection.Execute(pars);

                    itemName = dataRow[7] + " - " + dtbl.Rows[0]["addLine1"];
                }

                if (this._searchList.Code == "SCHCONSRCHALL")
                {
                    string query =
                        $"select contName, adr.addLine1 from dbContact left join dbAddress adr on contDefaultAddress = adr.addID where contId = '{dataRow[0]}'";

                    DataTableExecuteParameters pars = new DataTableExecuteParameters { Sql = query };
                    DataTable dtbl = Session.CurrentSession.CurrentConnection.Execute(pars);

                    itemName = dataRow[1] + " - " + dtbl.Rows[0]["addLine1"];
                }

                var rad = new RadioButton
                {
                    Name = $"Radio_{dataRow[0]}",
                    AutoSize = true,
                    Location = new Point(15, itemHeight),
                    Padding = new Padding(0, 0, 0, 6),
                    TabStop = true,
                };
                rad.CheckedChanged += this.rad_OnCheckedChanged;

                var lbl = new Label
                {
                    Name = $"Label_{dataRow[0]}",
                    Cursor = Cursors.Hand,
                    AutoSize = true,
                    Text = itemName,
                    Font = new Font(
                                      "Segoe UI",
                                      9F,
                                      FontStyle.Underline),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, itemHeight)
                };
                lbl.Click += this.lbl_OnClick;

                var but = new LinkLabel
                {
                    ActiveLinkColor =
                                      Color.FromArgb(
                                          ((int)(((byte)(32)))),
                                          ((int)(((byte)(81)))),
                                          ((int)(((byte)(137))))),
                    LinkBehavior = LinkBehavior.HoverUnderline,
                    LinkColor = Color.FromArgb(
                                      ((int)(((byte)(32)))),
                                      ((int)(((byte)(81)))),
                                      ((int)(((byte)(137))))),
                    Location = new Point(15, itemHeight),
                    Name = $"LinkLabel_{dataRow[0]}",
                    AutoSize = true,
                    TabStop = true,
                    Text = Session.CurrentSession.Resources.GetResource("OPENBUT", "Open", "").Text,
                    TextAlign = ContentAlignment.MiddleCenter,
                    VisitedLinkColor = Color.FromArgb(
                                      ((int)(((byte)(32)))),
                                      ((int)(((byte)(81)))),
                                      ((int)(((byte)(137)))))
                };
                but.Click += this.But_OnClick;

                itemHeight += rad.Height;
                this.groupBox1.Controls.Add(rad, true);
                this.pnlLabels.Controls.Add(lbl, true);
                this.pnlOpens.Controls.Add(but, true);
            }

            this.groupBox1.Height = this.pnlLabels.Height;
        }

        private void rad_OnCheckedChanged(object sender, EventArgs e)
        {
            var obj = sender as RadioButton;
            if (obj.Checked)
            {
                SelectRowItem(obj.TabIndex);
            }
        }

        private void lbl_OnClick(object sender, EventArgs e)
        {
            var obj = sender as Label;
            string id = obj.Name.Split('_')[1];

            Popup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Content = new ucPopupInfo(this._searchList.Code, id),
                Appearance = new Infragistics.Win.Appearance { BorderColor = SystemColors.ActiveBorder }
            };


            Popup.Show(new Point(MousePosition.X, MousePosition.Y), Infragistics.Win.Peek.PeekLocation.AboveItem);
        }

        /// <summary>
        /// The main thread on error captured event.
        /// </summary>
        /// <param name="sender">The search list.</param>
        /// <param name="e">Message event arguments.</param>
        private void Main_searchList_Error(object sender, MessageEventArgs e)
        {
            Application.DoEvents();

            if (this._captureException)
            {
                ErrorBox.Show(
                    ParentForm,
                    new Exception(
                        Session.CurrentSession.Resources.GetMessage("SLERROR", "Unexpected error in Search List ''%1%''. Please contact support.", "", this.SearchList.Code).Text,
                        e.Exception));
            }
            else
            {
                throw e.Exception;
            }
        }

        /// <summary>
        /// Captures the main threads search lists searched event, when the search has finished.
        /// </summary>
        /// <param name="sender">The search list calling.</param>
        /// <param name="e">Searched event arguments.</param>
        private void Main_searchList_Searched(object sender, SearchedEventArgs e)
        {
            this.Searched(e.Data);
            if ((this.Parent is EnquiryForm) == false)
            {
                ((Control)SearchQueryControl).Focus();
            }

            OnStateChanged();
        }

        private void Searched(DataTable dt)
        {
            this.dtCT = dt;
            this.SetCaption();
            this.CheckResultsVisibility();
            this.OnSearchCompleted();
            this.GenerateResults();
        }

        /// <summary>
        /// Captures an error from the search object whilst threading.
        /// </summary>
        /// <param name="sender">The search object.</param>
        /// <param name="e">Message event arguments.</param>
        private void searchList_Error(object sender, MessageEventArgs e)
        {
            MessageEventHandler err = this.Main_searchList_Error;
            try
            {
                //Invoke across threads using the forms invoke method.
                this.Invoke(err, new object[2] { sender, e });
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "searchList_Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Captures the search lists searched event, when the search has finished.
        /// </summary>
        /// <param name="sender">The search list calling.</param>
        /// <param name="e">Searched event arguments.</param>
        private void searchList_Searched(object sender, SearchedEventArgs e)
        {
            try
            {
                SearchedEventHandler sch = new SearchedEventHandler(this.Main_searchList_Searched);
                this.Invoke(sch, new object[2] { sender, e });
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "searchList_Searched: " + ex.Message);
            }
        }

        /// <summary>
        /// Search Query Control on Text Changed.
        /// </summary>
        private void SearchQueryControl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblCaption.Text = Session.CurrentSession.Resources.GetResource("CONFCSEARCH", "Searching...", "").Text;

                IBasicEnquiryControl2 searchCriteria = null;
                if (this._searchList.Code == "SCHCLISEACHADV")
                {
                    searchCriteria = this.EnquiryForm.GetIBasicEnquiryControl2("txtSearchName");
                }
                else if (this._searchList.Code == "SCHCONSRCHALL")
                {
                    searchCriteria = this.EnquiryForm.GetIBasicEnquiryControl2("@CONTNAME");
                }

                searchCriteria.Value = this.SearchQueryControl.Value;

                this.Search();
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(
                    Global.LogSwitch.TraceError,
                    ex.Message,
                    "SearchQueryControl_TextChanged: " + ex.Message);
            }
        }

        private void SetCaption()
        {
            if (this._searchList == null)
            {
                return;
            }

            this.lblCaption.Text = $"{this._searchList.ResultCount} " + Session.CurrentSession.Resources.GetResource("CONFFOUND", "Found", "").Text;
        }

        private void SetParameterHandler(string name, out object value)
        {
            if (name == "#UI")
            {
                value = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }
            else
            {
                value = DBNull.Value;
            }
        }

        /// <summary>
        /// Sets the current search list object.
        /// </summary>
        /// <param name="searchList">Search list object to be used.</param>
        /// <param name="code">Search list to be used.</param>
        /// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
        /// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
        private void SetSearchList(SearchList searchList, string code, object parent, KeyValueCollection parameters)
        {
            this.SuspendLayout();

            //Set the parameter array.
            this._parent = parent;
            this._parameters = parameters;

            //Check to see if the current search list has a script to dispose before you change it.
            if (this._searchList != null)
            {
                if (this._searchList.HasScript)
                {
                    this._searchList.Script.Dispose();
                }
            }

            if (searchList != null) this._searchList = searchList;
            else
            {
                //Create the search list object and fetch the search enquiry form.
                if (this._searchList == null)
                {
                    this._searchList = new SearchList(code, parent, parameters);
                }
                else
                {
                    this._searchList.Dispose();
                    this._searchList = new SearchList(code, parent, parameters);
                }
            }

            if (this._searchList.Code != "SCHCLISEACHADV" && this._searchList.Code != "SCHCONSRCHALL")
            {
                throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("SLNOTSUPP", "Searchlist ''%1%'' is not supported.", "", this.SearchList.Code).Text);
            }

            // Set the Events for the Thread Search
            this._searchList.Error += new MessageEventHandler(this.searchList_Error);
            this._searchList.Searched += new SearchedEventHandler(this.searchList_Searched);

            this._incsearch.Clear();

            //Loop through the rows within the list view table
            //and create the columns to be displayed.
            DataTable dt = this._searchList.ListView;
            bool _first = false;
            User _user = Session.CurrentSession.CurrentUser;

            bool warned = false;
            foreach (DataRow row in dt.Rows)
            {
                if (dt.Columns.Contains("roles") == false || _user.IsInRoles(Convert.ToString(row["roles"]))
                    && (dt.Columns.Contains("conditions") == false || Session.CurrentSession.ValidateConditional(
                            this._parent,
                            Convert.ToString(row["conditions"]).Split(Environment.NewLine.ToCharArray()))))
                {
                    if (_first == false)
                    {
                        if (row.Table.Columns.Contains("displayAs") == false
                            || row.Table.Columns.Contains("sourceIs") == false)
                            if (warned == false)
                            {
                                MessageBox.ShowInformation(
                                    "WARSTPOD1",
                                    "There is a System Stored Procedure out of date that needs updating to maintain full compatibility, please contact your system administrator to report this code “%1%”, the system will try and continue operation as normal but functionality maybe reduced.",
                                    "sprSearchListBuilder");
                                warned = true;
                            }
                    }
                }
            }

            //Get the criteria enquiry form if the seach style is a search style.
            if (this._searchList.Style == SearchListStyle.Search || this._searchList.Style == SearchListStyle.Filter)
            {
                this.EnquiryForm.Enquiry = this._searchList.CriteriaForm;
            }
            else
            {
                this.EnquiryForm.Enquiry = null;
            }

            this.OnSearchListLoad();
            this.ResumeLayout();
        }

        private void ucSearchConflicts_ParentChanged(object sender, EventArgs e)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                try
                {
                    if (this.Parent is EnquiryForm && this._enquirydisconnect == null)
                    {
                        if (this.RefreshOnEnquiryFormRefreshEvent)
                        {
                            EnquiryForm _enqform = this.Parent as EnquiryForm;
                            if (_enqform.Enquiry != null)
                            {
                                _enqform.Enquiry.Refreshed += new EventHandler(this.Enquiry_Refreshed);
                                this._enquirydisconnect = _enqform.Enquiry;
                            }
                        }
                    }
                    else if (this.Parent == null && this._enquirydisconnect != null)
                    {
                        if (this.RefreshOnEnquiryFormRefreshEvent)
                        {
                            this._enquirydisconnect.Refreshed -= new EventHandler(this.Enquiry_Refreshed);
                        }
                    }

                    if (this.Parent is EnquiryForm)
                    {
                        EnquiryForm _enqform = this.Parent as EnquiryForm;
                        if (_enqform != null)
                        {
                            SearchQueryControl = _enqform.GetIBasicEnquiryControl2(_searchQueryControlName, EnquiryControlMissing.None);

                            if (_enqform.Enquiry != null)
                            {
                                _enqform.Enquiry.Refreshed += new EventHandler(this.Enquiry_Refreshed);
                                this._enquirydisconnect = _enqform.Enquiry;
                            }

                            if (_searchlistcode != string.Empty)
                            {
                                this.SetSearchList(_searchlistcode,
                                    _enqform.Enquiry.Parent,
                                    _enqform.Enquiry.ReplacementParameters);
                                if (_enqform.Enquiry.InDesignMode)
                                {
                                    this._searchList.ParameterHandler +=
                                        new SourceEngine.SetParameterHandler(this.SetParameterHandler);
                                }
                            }
                        }
                        else
                        {
                            _searchQueryControl = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
        }

        private void ucSearchConflicts_MouseLeave(object sender, EventArgs e)
        {
            PopupClose();
        }

        private void PopupClose()
        {
            if (Popup != null && Popup.Visible)
            {
                Popup.Close();
                Popup = null;
            }
        }

        #endregion
    }
}