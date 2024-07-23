using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    public class eDataSelector : UserControl, IBasicEnquiryControl2, IListEnquiryControl, ITextEditorEnquiryControl, ISupportRightToLeft
    {
        #region Fields
        /// <summary>
        /// Hidden buttons that will be used to contain the original locations of the Default Property
        /// </summary>
        private Button _btnaccept;
        /// <summary>
        /// Internal Text Box used to by this control
        /// </summary>
        protected internal System.Windows.Forms.TextBox txtSelection;
        /// <summary>
        /// Internal Button used to by this control
        /// </summary>
        protected internal System.Windows.Forms.Button btnSearch;
        /// <summary>
        /// Some s**t added by .NET
        /// </summary>
        private System.ComponentModel.IContainer components;
        /// <summary>
        /// Flag to mark the Control as required
        /// </summary>
        private bool _required = false;
        /// <summary>
        /// Width of the Caption in this control
        /// </summary>
        private int _captionwidth = 150;
        /// <summary>
        /// Do not raise Change Events when Filtering
        /// </summary>
        private bool _filtering = false;

        private bool _isdirty = false;
        private bool _updating = false;
        private object _datasouce = null;
        protected System.Windows.Forms.Panel pnlContainer;

        private string _filterString = string.Empty;

        private object _value = null;

        private string _filterName = null;

        private object _filterValue = null;

        protected internal System.Windows.Forms.ErrorProvider errorProvider1;

        private string _formText = "Search";

        private string _searchlistcode;

        private bool _loaded = false;

        #endregion

        #region Constructors
        public eDataSelector()
        {
            InitializeComponent();
        }


        private void Selection_Load(object sender, EventArgs e)
        {
            if (this.TabStop && this.ReadOnly)
                this.TabStop = false;
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eDataSelector));
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.txtSelection = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.txtSelection);
            this.pnlContainer.Controls.Add(this.btnSearch);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(356, 23);
            this.pnlContainer.TabIndex = 0;
            // 
            // txtSelection
            // 
            this.txtSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorProvider1.SetIconAlignment(this.txtSelection, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.errorProvider1.SetIconPadding(this.txtSelection, 2);
            this.txtSelection.Location = new System.Drawing.Point(0, 0);
            this.txtSelection.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.txtSelection.Name = "txtSelection";
            this.txtSelection.ReadOnly = true;
            this.txtSelection.Size = new System.Drawing.Size(330, 20);
            this.txtSelection.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(330, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(26, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "...";
            this.btnSearch.UseCompatibleTextRendering = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // eDataSelector
            // 
            this.Controls.Add(this.pnlContainer);
            this.DoubleBuffered = true;
            this.Name = "eDataSelector";
            this.Size = new System.Drawing.Size(356, 23);
            this.Load += new System.EventHandler(this.Selection_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Selection_Paint);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Private Functionality
        /// <summary>
        /// Draws the Caption Text on the Main Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selection_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            eBase2.PaintCaption(e, this, txtSelection, _captionwidth);
        }

        /// <summary>
        /// Process the Key Mnenonic
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns></returns>
        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.Visible && System.Windows.Forms.Control.IsMnemonic(charCode, this.Text))
            {
                this.txtSelection.Focus();
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Public

        /// <summary>
        /// Filters the bound result set with the specified value under the specified field.
        /// </summary>
        /// <param name="fieldName">The field name to filter with.</param>
        /// <param name="Value">The value to filter for.</param>
        public bool Filter(string fieldName, object Value)
        {
            if (Value == null || fieldName == null || _datasouce == null) return false;

            DataTable dt = null;
            if (_datasouce is DataTable)
                dt = (DataTable)_datasouce;

            if (Value != System.DBNull.Value)
                _filterString = "(" + fieldName + " = '" + Convert.ToString(Value).Replace("'", "''") + "' OR " + fieldName + " IS NULL)";
            else
                _filterString = fieldName + " IS NULL";

            dt.DefaultView.RowFilter = _filterString;

            _filterName = fieldName;
            _filterValue = Value;

            SetValue(_value);

            return true;
        }

        public bool Filter(string FilterString)
        {
            return true;
        }

        /// <summary>
        /// Disable the Events
        /// </summary>
        public void BeginUpdate()
        {

        }

        /// <summary>
        /// Enables the Events and Sets the Value
        /// </summary>
        public void EndUpdate()
        {

        }

        #endregion

        #region Properties

        [DefaultValue("")]
        [Browsable(true)]
        [LocCategory("Data")]
        [Lookup("DSSLCODE")]
        [Editor(typeof(FWBS.OMS.UI.Windows.SearchListUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSearchLists", NullValue = "", UseNull = false, DisplayMember = "cdDesc", ValueMember = "schCode")]
        public string SearchListCode
        {
            get
            {
                return _searchlistcode;
            }
            set
            {
                _searchlistcode = value;
                if (!string.IsNullOrEmpty(value))
                    this.btnSearch.Enabled = true;
            }
        }

        private bool _omsdesignmode = false;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool omsDesignMode
        {
            get
            {
                return _omsdesignmode;
            }
            set
            {
                _omsdesignmode = value;
            }
        }

        [Browsable(true)]
        public object Control
        {
            get
            {
                return txtSelection;
            }
        }

        /// <summary>
        /// Gets or Sets the Object for the Return Value
        /// </summary>
        [Category("OMS")]
        [DefaultValue(null)]
        public virtual object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _updating = true;
                SetValue(value);
                _updating = false;
                IsDirty = false;
                if (this.Parent != null)
                {
                    IsDirty = true;
                    OnChanged();
                }
            }
        }

        [Browsable(false)]
        public virtual bool IsDirty
        {
            get
            {
                return _isdirty;
            }
            set
            {
                _isdirty = value;
            }
        }

        /// <summary>
        /// Gets or Set the string for the Controls Caption
        /// </summary>
        [Category("Appearance")]
        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or Sets the object for the DataSource 
        /// </summary>
        [Category("Data")]
        [DefaultValue(null)]
        public object DataSource
        {
            get
            {
                return _datasouce;
            }
            set
            {
                _datasouce = value;
                if (_datasouce is DataView)
                    _datasouce = ((DataView)_datasouce).ToTable();
            }
        }

        /// <summary>
        /// Gets or Set a int for the Caption Label Width
        /// </summary>
        [Category("OMS")]
        [DefaultValue(150)]
        public int CaptionWidth
        {
            get
            {
                return _captionwidth;
            }
            set
            {
                _captionwidth = value;
                if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                {
                    this.Padding = new System.Windows.Forms.Padding(0, 0, _captionwidth, 0);
                }
                else
                {
                    this.Padding = new System.Windows.Forms.Padding(_captionwidth, 0, 0, 0);
                }
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

        /// <summary>
        /// Gets or Sets a int for the Max Length
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(32767)]
        public int MaxLength
        {
            get
            {
                return txtSelection.MaxLength;
            }
            set
            {
                txtSelection.MaxLength = value;
            }
        }

        /// <summary>
        /// Gets whether the current control can be stretched by its Y co-ordinate.
        /// This is a design mode property and is set to true.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(true)]
        public virtual bool LockHeight
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or Sets the editable format of the control.  By default the whole control toggles it's enable property.
        /// </summary>
        private bool _readonly;

        [DefaultValue(false)]
        [Category("Behavior")]
        public virtual bool ReadOnly
        {
            get
            {
                return _readonly;
            }
            set
            {
                this.pnlContainer.Enabled = !value;
                _readonly = value;
            }
        }

        /// <summary>
        /// Gets or Sets the control as required.  This is then used by the rendering form to display the
        /// control as required by its own definition.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool Required
        {
            get
            {
                return _required;
            }
            set
            {
                _required = value;
            }
        }

        #endregion

        #region IListEnquiryControl Implementation
        /// <summary>
        /// Gets or Sets the display text section of the combo box.
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DisplayValue
        {
            get
            {
                return this.Value;
            }
            set
            {
            }
        }

        /// <summary>
        /// Assigns an individual bound value and a display value for a combo box.
        /// </summary>
        /// <param name="Value">Invisible bound value.</param>
        /// <param name="displayText">Visible display text.</param>
        public virtual void AddItem(object Value, string displayText)
        {
            EnquiryListItem itm = new EnquiryListItem(Value, displayText);
        }

        /// <summary>
        /// Assigns a data table object to the datasource of the combo box which uses
        /// the first column as bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataTable">Bound data table.</param>
        public virtual void AddItem(DataTable dataTable)
        {
            this.BeginUpdate();
            if (dataTable == null)
            {
                return;
            }
            if (this.DataSource != null) this.DataSource = null;
            this.DataSource = dataTable;
            _formText = CodeLookup.GetLookup("ENQDATALIST", dataTable.TableName);
            _formText = FWBS.OMS.Session.CurrentSession.Terminology.Parse(_formText, true);
            this.btnSearch.Enabled = true;
            this.EndUpdate();
        }

        /// <summary>
        /// Assigns an ArrayList object to the datasource of the combo box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataArrayList">Bound ArrayList.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public virtual void AddItem(ArrayList dataArrayList, string valueMember, string displayMember)
        {
            try
            {
                this.BeginUpdate();
                if (this.DataSource != null) this.DataSource = null;
                this.DataSource = dataArrayList;
                this.EndUpdate();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Assigns a data table object to the datasource of the combo box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataTable">Bound data table.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public virtual void AddItem(DataTable dataTable, string valueMember, string displayMember)
        {
            if (this.DataSource != null) this.DataSource = null;
            this.DataSource = dataTable;
        }

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the first column as the bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataView">Bound data view.</param>
        public virtual void AddItem(DataView dataView)
        {
            if (dataView.Table != null)
            {
                if (this.DataSource != null) this.DataSource = null;
                this.DataSource = dataView;
            }
        }

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataView">Bound data view.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public virtual void AddItem(DataView dataView, string valueMember, string displayMember)
        {
            if (dataView.Table != null)
            {
                if (this.DataSource != null) this.DataSource = null;
                this.DataSource = dataView;
            }
        }

        /// <summary>
        /// Gets the number of items within the list section of the control.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(0)]
        public int Count
        {
            get
            {
                return ((DataTable)_datasouce).Rows.Count;
            }
        }

        #endregion

        #region Deligates
        /// <summary>
        /// Executes the changed event.
        /// </summary>
        public void OnChanged()
        {
            if (Changed != null && _filtering == false && IsDirty)
            {
                IsDirty = false;
                Changed(this, EventArgs.Empty);
            }
        }

        public void OnActiveChanged()
        {
            IsDirty = true;
            if (ActiveChanged != null)
                ActiveChanged(this, EventArgs.Empty);
        }

        public bool OnDoesNotExist()
        {
            CancelEventArgs cancel = new CancelEventArgs();
            if (DoesNotExist != null)
                DoesNotExist(this, cancel);
            return cancel.Cancel;
        }

        /// <summary>
        /// Raises the changed event within the base control.
        /// </summary>
        protected void RaiseChangedEvent(object sender, System.EventArgs e)
        {
            OnChanged();
        }

        /// <summary>
        /// The changed event is used to determine when a major change has happended within the
        /// user control.  This will tend to be used when the internal editing control has changed
        /// in some way or another.
        /// </summary>
        [Category("Action")]
        public virtual event EventHandler Changed;
        [Category("Action")]
        public virtual event CancelEventHandler DoesNotExist;
        [Category("Action")]
        public event EventHandler ActiveChanged;

        #endregion

        #region ICharacterCasingControl

        /// <summary>
        /// Gets or Sets the character casing of the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(CharacterCasing.Normal)]
        public CharacterCasing Casing
        {
            get
            {
                return CharacterCasing.Normal;
            }
            set
            {
            }
        }

        #endregion

        /// <summary>
        /// Disables the Cancel And Return Buttons of the Form
        /// </summary>
        protected void StopCancelAndReturn()
        {
            if (this.ParentForm != null)
            {
                if (this.ParentForm.AcceptButton != null)
                {
                    _btnaccept = (Button)this.ParentForm.AcceptButton;
                    this.ParentForm.AcceptButton = null;
                }
            }
        }

        /// <summary>
        /// Enables the Cancel and Return Buttons on the Form
        /// </summary>
        protected void StartCancelAndReturn()
        {
            if (this.ParentForm != null)
            {
                if (_btnaccept != null)
                {
                    this.ParentForm.AcceptButton = _btnaccept;
                    _btnaccept = null;
                }
            }
        }

        private void SetValue(object value)
        {
            _updating = true;

            _value = value;

            if (this.DataSource == null)
            {
                if (string.IsNullOrWhiteSpace(this.SearchListCode))
                {
                    return;
                }
                else
                {
                    if (_value != null && !_loaded)
                    {
                        GetDisplayValueOnLoad(_value);
                        _loaded = true;
                    }
                }
            }
            else
            {
                string _text = getText(value);

                if (_text == null)
                {
                    _value = value = DBNull.Value;
                    _text = getText(value);
                }

                txtSelection.Text = Convert.ToString(_text);
                OnActiveChanged();
                _updating = false;
            }
        }


        private string getText(object value)
        {
            if (_filterValue != null && _filterName != null)
            {
                return (from DataRow dr in ((DataTable)this.DataSource).Rows
                        where ((Convert.ToString(dr[0]).ToUpper().Trim() == Convert.ToString(value).ToUpper().Trim()) &&
                         ((Convert.ToString(dr[_filterName]).ToUpper().Trim() == Convert.ToString(_filterValue).ToUpper().Trim()) || (dr[_filterName] == System.DBNull.Value)))
                        select Convert.ToString(dr[1])).FirstOrDefault();
            }
            else
            {
                return (from DataRow dr in ((DataTable)this.DataSource).Rows
                        where Convert.ToString(dr[0]).ToUpper().Trim() == Convert.ToString(value).ToUpper().Trim()
                        select Convert.ToString(dr[1])).FirstOrDefault();
            }
        }


        private void btnSearch_click(object sender, EventArgs e)
        {
            if (this.DataSource == null)
            {
                if (string.IsNullOrWhiteSpace(this.SearchListCode))
                    return;
                else
                    searchSearchList();
            }
            else
            {
                searchDataList();
            }
        }


        private void searchDataList()
        {
            DataTable dt = null;
            if (_datasouce is DataTable)
                dt = (DataTable)_datasouce;

            using (searchDataList form = new searchDataList(dt, _filterString))
            {
                form.Text = _formText;
                form.Value = _value;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _value = form.Value;
                    this.txtSelection.Text = form.Description;
                    if (this.Parent != null)
                    {
                        IsDirty = true;
                        OnChanged();
                    }
                    dt.DefaultView.RowFilter = _filterString;
                    OnActiveChanged();
                }
                else
                {
                    dt.DefaultView.RowFilter = _filterString;
                }
            }
        }


        private void searchSearchList()
        {
            try
            {
                FWBS.Common.KeyValueCollection keys = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(this.Parent, _searchlistcode, false, new System.Drawing.Size(640, 480), null, PopulateKeyValueCollection(DBNull.Value));

                if (keys == null)
                    return;

                if (!string.IsNullOrWhiteSpace(Convert.ToString(keys[0].Value)) && !string.IsNullOrWhiteSpace(Convert.ToString(keys[1].Value)))
                    SetControlText(keys[0].Value, Convert.ToString(keys[1].Value));
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        private void GetDisplayValueOnLoad(object value)
        {
            try
            {
                if (this.omsDesignMode)
                    return;

                if (value == null)
                {
                    return;
                }

                using (FWBS.OMS.SearchEngine.SearchList sl = new FWBS.OMS.SearchEngine.SearchList(_searchlistcode, null, PopulateKeyValueCollection(value)))
                {
                    DataTable result = sl.Run(false) as DataTable;
                    txtSelection.Text = result.Rows[0][1].ToString();
                }

                if (_loaded)
                {
                    OnActiveChanged();
                    OnChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }

        }


        public event EventHandler<BeforeSearchListEventArgs> BeforeSearchListSearches;


        public class BeforeSearchListEventArgs : EventArgs
        {
            private KeyValueCollection _kvc;

            public KeyValueCollection SearchListParameters
            {
                get
                {
                    return _kvc;
                }
                set
                {
                    _kvc = value;
                }
            }

            public BeforeSearchListEventArgs(KeyValueCollection kvc)
            {
                _kvc = kvc;
            }
        }


        private void OnBeforeSearchListSearches(BeforeSearchListEventArgs e)
        {
            if (BeforeSearchListSearches != null)
                BeforeSearchListSearches(this, e);
        }



        private FWBS.Common.KeyValueCollection PopulateKeyValueCollection(object passedValue)
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection();
            kvc.Add("value", passedValue);
            OnBeforeSearchListSearches(new BeforeSearchListEventArgs(kvc));
            if (!kvc.Keys.Contains("value"))
                throw new SearchListParameterMissingException(Session.CurrentSession.Resources.GetMessage(
                    "ERRMSGPRMSL",
                    "The parameters for the Search List configured for the control %1% must contain a value key.",
                    "",
                    this.Name).Text);
            return kvc;
        }


        private class SearchListParameterMissingException : Exception
        {
            public SearchListParameterMissingException(string message) : base() { }
        }



        private void SetControlText(object value, string text)
        {
            try
            {
                _loaded = true;
                Value = value;
                txtSelection.Text = text;
                OnActiveChanged();
                OnChanged();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        public void SetRTL(Form parentform)
        {
            eBase2.SetRTL(this, txtSelection, _captionwidth, btnSearch);
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (omsDesignMode && this.Height < txtSelection.PreferredSize.Height)
            {
                this.Height = txtSelection.PreferredSize.Height;
            }
            base.OnSizeChanged(e);
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (omsDesignMode && this.Height < txtSelection.PreferredSize.Height)
            {
                this.Height = txtSelection.PreferredSize.Height;
            }
            
        }
    }
}
