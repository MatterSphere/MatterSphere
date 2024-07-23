using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using FWBS.Common.UI.Windows.Common;
using FWBS.OMS;


namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eXPComboBox.
    /// </summary>
    public class eXPComboBox : System.Windows.Forms.UserControl, IBasicEnquiryControl2, IListEnquiryControl, ITextEditorEnquiryControl, ISupportRightToLeft
    {
        protected class CustomComboBox : CueComboBox
        {
            private static readonly PropertyInfo _windowTextProp = typeof(Control).GetProperty("WindowText", BindingFlags.Instance | BindingFlags.NonPublic);

            internal string EditText
            {
                get { return (string)_windowTextProp.GetValue(this); }
                set { _windowTextProp.SetValue(this, value); }
            }

            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                base.SetBoundsCore(x, System.Math.Max(0, y), width, height, specified);
            }
        }

        private const string CUETEXT_CODELOOKUPGROUPNAME = "ENQQUESTCUETXT";

        #region Fields
        /// <summary>
        /// Hidden buttons that will be used to contain the original locations of the Default Property
        /// </summary>
        private Button _btnaccept;
        /// <summary>
        /// Internal Combo Box used to by this control
        /// </summary>
        protected CustomComboBox comboBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Flag to mark the Control as required
        /// </summary>
        private bool _required = false;
        /// <summary>
        /// Flat to mark the Control as Limited to List thus no input that is not in the list will be accepted
        /// </summary>
        private bool _limittolist = true;
        /// <summary>
        /// Width of the Caption in this control
        /// </summary>
        private int _captionWidth = 150;
        /// <summary>
        /// Display caption on top.
        /// </summary>
        private bool _captionTop = false;
        /// <summary>
        /// Flag to Allow Nulls
        /// </summary>
        private bool _allownull = false;
        /// <summary>
        /// Flag to enable or disable the Active Search key
        /// </summary>
        private bool _activesearch = true;
        /// <summary>
        /// Do not raise Change Events when Filtering
        /// </summary>
        private bool _filtering = false;

        private bool _isdirty = false;

        protected object lastgoodchoice;
        private object escapevalue = null;

        private string _displaymember = "";

        private string _valuemember = "";
        private bool _updating = false;
        private object _datasource = null;

        private Color _defaultForeColor = Color.Transparent;

        private CodeLookupDisplay _cueText;
        private string _cueTextCode;

        /// <summary>
        /// Caption Label Height
        /// </summary>
        private int _captionTopHeight = 0;
        #endregion

        #region Constructors
        public eXPComboBox()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ValidateCtrlHeight();
            base.OnSizeChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (_captionTop)
            {
                _captionTopHeight = CalcCaptionTopHeight(true);
                eBase2.SetRTL(this, comboBox1, _captionWidth, null, _captionTop, _captionTopHeight);
            }

            ValidateCtrlHeight();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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
            this.comboBox1 = new FWBS.Common.UI.Windows.eXPComboBox.CustomComboBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.Location = new System.Drawing.Point(150, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(206, 23);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            this.comboBox1.Enter += new System.EventHandler(this.comboBox1_Enter);
            this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
            this.comboBox1.LostFocus += new System.EventHandler(this.RaiseLeaveEvent);
            // 
            // eXPComboBox
            // 
            this.Controls.Add(this.comboBox1);
            this.DoubleBuffered = true;
            this.Name = "eXPComboBox";
            this.Size = new System.Drawing.Size(356, 23);
            this.ResumeLayout(false);

        }
        #endregion

        #region Private Functionality

        /// <summary>
        /// Draws the Caption Text on the Main Control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            eBase2.PaintCaption(e, this, comboBox1, _captionWidth, _captionTop, _captionTopHeight);
        }

        /// <summary>
        /// Process the Key Mnenonic
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns></returns>
        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.Visible && IsMnemonic(charCode, this.Text))
            {
                this.comboBox1.Focus();
                return true;
            }
            else
                return false;
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            if (_captionTop)
            {
                _captionTopHeight = CalcCaptionTopHeight(false);
                eBase2.SetRTL(this, comboBox1, _captionWidth, null, _captionTop, _captionTopHeight);
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (factor.Height != 1 && (specified & BoundsSpecified.Height) != 0)
            {
                _captionTopHeight = Convert.ToInt32(_captionTopHeight * factor.Height);
            }
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
            if (Value == null || fieldName == null)
                return false;
            else if (Value != DBNull.Value)
                return Filter(string.Format("{0} = '{1}'", fieldName, SQLRoutines.RemoveRubbish(Convert.ToString(Value))));
            else
                return ApplyFilter(fieldName + " is null");
        }

        public bool Filter(string FilterString)
        {
            return ApplyFilter(string.Format("({0} or {1} is null)", FilterString, comboBox1.ValueMember));
        }

        private bool ApplyFilter(string rowFilter)
        {
            try
            {
                _filtering = true;
                object oldvalue = comboBox1.SelectedValue;
                DataTable dt = _datasource as DataTable;
                if (dt != null)
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = rowFilter;
                    comboBox1.DisplayMember = _displaymember;
                    comboBox1.ValueMember = _valuemember;
                    comboBox1.DataSource = FixDuplicateDescriptions(dv.ToTable(true, _valuemember, _displaymember));
                    dv.Dispose();
                }
                SetValue(oldvalue);
                return (comboBox1.SelectedValue == DBNull.Value);
            }
            finally
            {
                _filtering = false;
            }
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

        public void Clear()
        {
            comboBox1.Items.Clear();
        }

        #endregion

        #region Properties

        [Category("OMS Appearance")]
        [CodeLookupSelectorTitle("CUETEXT", "Cue Text")]
        [DefaultValue(null)]
        [Description("Localised code of the Controls CueText"), LocCategory("Design")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MergableProperty(false)]
        public virtual CodeLookupDisplay CueText
        {
            get { return _cueText ?? (_cueText = new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME)); }

            set
            {
                if (_cueText != value)
                {
                    _cueText = value;
                    ((CustomComboBox)comboBox1).CueText = _cueText.Description;
                    IsDirty = true;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string CueTextCode
        {
            get { return _cueTextCode; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals(_cueTextCode))
                {
                    _cueTextCode = value;
                    CueText = new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME)
                    {
                        Code = value,
                        Description = CodeLookup.GetLookup(CUETEXT_CODELOOKUPGROUPNAME, value),
                        UICulture = Thread.CurrentThread.CurrentCulture.Name,
                        Help = CodeLookup.GetLookupHelp(CUETEXT_CODELOOKUPGROUPNAME, value)
                    };
                }
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
                return comboBox1;
            }
        }

        /// <summary>
        /// Gets or Sets a Boolean to Limit the Users choice to the List Provided
        /// </summary>
        [Category("Behavior")]
        [Description("Limits the Users Choice to the List")]
        [DefaultValue(true)]
        public bool LimitToList
        {
            get
            {
                return _limittolist;
            }
            set
            {
                _limittolist = value;
            }
        }

        [Browsable(false)]
        public bool ActiveSearchEnabled
        {
            get
            {
                return _activesearch;
            }
            set
            {
                _activesearch = value;
            }
        }

        private object retainvalue = null;

        /// <summary>
        /// Gets or Sets the Object for the Return Value
        /// </summary>
        [Category("OMS")]
        [DefaultValue(null)]
        public virtual object Value
        {
            get
            {
                if (_limittolist == false && comboBox1.SelectedValue == null)
                    return comboBox1.Text;
                else
                    return comboBox1.SelectedValue;
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

        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public System.Windows.Forms.ComboBox.ObjectCollection Items
        {
            get
            {
                return comboBox1.Items;
            }
            set
            {

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
                return _datasource;
            }
            set
            {
                _datasource = value;
                if (_datasource is DataView)
                    _datasource = ((DataView)_datasource).ToTable();
                _updating = true;
                comboBox1.DataSource = FixDuplicateDescriptions(_datasource);
                _updating = false;
            }
        }

        /// <summary>
        /// Gets or Sets the string for Display Member
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        public string DisplayMember
        {
            get
            {
                return comboBox1.DisplayMember;
            }
            set
            {
                _displaymember = value;
                _updating = true;
                comboBox1.DisplayMember = value;
                _updating = false;
            }
        }

        /// <summary>
        /// Gets or Sets a Boolean to AllowNull the Control to be left Blank
        /// </summary>
        [Category("Behavior")]
        [Description("Determines if the Control can be Blank")]
        [DefaultValue(false)]
        public bool AllowNull
        {
            get
            {
                return _allownull;
            }
            set
            {
                _allownull = value;
            }
        }

        /// <summary>
        /// Gets or Sets the string for the Value Member
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        public string ValueMember
        {
            get
            {
                return comboBox1.ValueMember;
            }
            set
            {
                _valuemember = value;
                _updating = true;
                comboBox1.ValueMember = value;
                _updating = false;
            }
        }

        /// <summary>
        /// Gets or Set a int for the Caption Label Width
        /// </summary>
        [Browsable(false)]
        [Category("OMS")]
        [DefaultValue(150)]
        public int CaptionWidth
        {
            get
            {
                return _captionWidth;
            }
            set
            {
                _captionWidth = _captionTop ? 0 : value;
                eBase2.SetRTL(this, comboBox1, _captionWidth, null, _captionTop, _captionTopHeight);
            }
        }

        /// <summary>
        /// Gets or Set a bool for the Caption location - on the top or not
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public virtual bool CaptionTop
        {
            get
            {
                return _captionTop;
            }
            set
            {
                _captionTop = value;
                _captionWidth = value ? 0 : eBase2.DefaultCaptionWidth;
                _captionTopHeight = value ? CalcCaptionTopHeight(true) : 0;
                eBase2.SetRTL(this, comboBox1, _captionWidth, null, _captionTop, _captionTopHeight);
                ValidateCtrlHeight();
            }
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
                return comboBox1.MaxLength;
            }
            set
            {
                comboBox1.MaxLength = value;
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
                return true;
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
                comboBox1.Enabled = !value;
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

        /// <summary>
        /// Gets the Selected Text
        /// </summary>
        [Browsable(false)]
        public string SelectedText
        {
            get
            {
                return comboBox1.Text;
            }
        }


        /// <summary>
        /// Gets the Selected Item
        /// </summary>
        [Browsable(false)]
        public object SelectedItem
        {
            get
            {
                return comboBox1.SelectedItem;
            }
        }

        /// <summary>
        /// Gets the ValueMember of the combo box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get
            {
                return comboBox1.SelectedValue;
            }
            set
            {
                if (comboBox1.SelectedValue != value)
                {
                    SetValue(value);
                    escapevalue = value;
                }
            }
        }

        /// <summary>
        /// Gets the ValueMember of the combo box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                return comboBox1.SelectedIndex;
            }
            set
            {
                if (comboBox1.SelectedIndex != value)
                    comboBox1.SelectedIndex = value;
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
                return comboBox1.Text;
            }
            set
            {
                comboBox1.Text = Convert.ToString(value);
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
            comboBox1.Items.Add(itm);
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
            if (this.DataSource != null) this.DataSource = null;
            if (dataTable.Columns.Count > 0)
            {
                this.ValueMember = dataTable.Columns[0].ColumnName;
                if (dataTable.Columns.Count > 1)
                    this.DisplayMember = dataTable.Columns[1].ColumnName;
                else
                    this.DisplayMember = dataTable.Columns[0].ColumnName;
            }
            this.DataSource = dataTable;
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
                this.DisplayMember = displayMember;
                this.ValueMember = valueMember;
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
            if (dataTable.Columns.Contains(valueMember))
            {
                this.ValueMember = dataTable.Columns[valueMember].ColumnName;
            }
            if (dataTable.Columns.Contains(displayMember))
            {
                this.DisplayMember = dataTable.Columns[displayMember].ColumnName;
            }
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
                if (dataView.Table.Columns.Count > 0)
                {
                    this.ValueMember = dataView.Table.Columns[0].ColumnName;
                    if (dataView.Table.Columns.Count > 1)
                        this.DisplayMember = dataView.Table.Columns[1].ColumnName;
                    else
                        this.DisplayMember = dataView.Table.Columns[0].ColumnName;
                }
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
                if (dataView.Table.Columns.Contains(valueMember))
                {
                    this.ValueMember = dataView.Table.Columns[valueMember].ColumnName;
                }
                if (dataView.Table.Columns.Contains(displayMember))
                {
                    this.DisplayMember = dataView.Table.Columns[displayMember].ColumnName;
                }
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
                return comboBox1.Items.Count;
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
            lastgoodchoice = comboBox1.SelectedValue;
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

        protected void RaiseLeaveEvent(object sender, System.EventArgs e)
        {
            _updating = true;
            if (_activesearch)
            {
                comboBox1.LostFocus -= RaiseLeaveEvent;
                try
                {
                    if ((!_limittolist || _limittolist && _allownull) && comboBox1.Text == "")
                    {
                        comboBox1.SelectedIndex = -1;
                    }
                    else if (_limittolist)
                    {
                        if (comboBox1.FindStringExact(comboBox1.Text) == -1 && !comboBox1.IsCueTextShown)
                        {
                            if (OnDoesNotExist())
                            {
                                // Run if the Deligate say it now has been added
                                // or some other thing
                                comboBox1.SelectedIndex = comboBox1.FindStringExact(comboBox1.Text);
                                comboBox1.SelectAll();
                                comboBox1.Focus();
                                OnActiveChanged();
                            }
                            else
                            {
                                // Simply test again reset to last best option
                                // Note: SelectedValue setter may throw an exception.
                                comboBox1.SelectedValue = lastgoodchoice;
                                comboBox1.SelectAll();
                                comboBox1.Focus();
                                OnActiveChanged();
                            }
                        }
                        else if (comboBox1.SelectedValue == null)
                        {
                            comboBox1.SelectedIndex = comboBox1.FindStringExact(comboBox1.Text);
                            OnActiveChanged();
                        }
                        // Workaround: sometimes text in ComboBox and its internal Edit may be out of sync, so fix it.
                        if (comboBox1.EditText != comboBox1.Text)
                        {
                            comboBox1.EditText = comboBox1.Text;
                            OnActiveChanged();
                        }
                    }
                }
                catch { }
                finally
                {
                    if (this.ReadOnly == false)
                    {
                        OnChanged();
                    }
                    StartCancelAndReturn();
                }
                comboBox1.LostFocus += RaiseLeaveEvent;
            }
            _updating = false;
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
        [Category("Action")]
        public event EventHandler SelectionCommited;

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.Visible && comboBox1.DataSource != null)
                {
                    comboBox1.Select(0, 0);
                }
            }
            catch
            { }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            try
            {
                if (this.Visible && comboBox1.DataSource != null)
                {
                    comboBox1.Select(0, 0);
                }
            }
            catch
            { }
        }

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
            if (_btnaccept != null)
            {
                Form parentForm = this.ParentForm;
                if (parentForm != null)
                {
                    parentForm.AcceptButton = _btnaccept;
                    _btnaccept = null;
                }
            }
        }


        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.PageUp && e.KeyCode != Keys.PageDown)
                comboBox1.DroppedDown = false;

            if (e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = false;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                SetValue(escapevalue);
                StartCancelAndReturn();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                RaiseLeaveEvent(this, e);
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (_defaultForeColor != Color.Transparent)
            {
                comboBox1.ForeColor = _defaultForeColor;
            }

            StopCancelAndReturn();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_updating == false)
                {
                    OnActiveChanged();
                    OnChanged();
                }
            }
            catch (OMSException ex)
            {
                if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                {
                    throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                }
            }
        }

        private void SetValue(object value)
        {
            if (Convert.ToString(comboBox1.SelectedValue) != Convert.ToString(value))
            {
                _updating = true;
                try
                {
                    comboBox1.SelectedValue = value;
                    if (this.Visible == false && comboBox1.SelectedValue == null) retainvalue = value;
                    if (comboBox1.SelectedValue == null && this.Visible)
                        comboBox1.SelectedValue = DBNull.Value;
                    OnActiveChanged();
                }
                catch (OMSException ex)
                {
                    if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                    {
                        _updating = false;
                        throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                    }
                }
                catch { }
                _updating = false;
            }
        }

        private object FixDuplicateDescriptions(object data)
        {
            DataView dv = null;
            if (data is DataTable && this.DisplayMember != "")
            {
                dv = new DataView(((DataTable)data).Copy());
                dv.Sort = "[" + this.DisplayMember + "] ASC";
            }

            if (dv != null)
            {
                string last = null;
                int count = 1;
                foreach (DataRowView drv in dv)
                {
                    if (last == Convert.ToString(drv[this.DisplayMember]))
                    {
                        drv[this.DisplayMember] = Convert.ToString(drv[this.DisplayMember]) + " (" + count.ToString() + ")";
                        count++;
                    }
                    else
                    {
                        count = 1;
                        last = Convert.ToString(drv[this.DisplayMember]);
                    }
                }
                dv.Sort = "";
                return dv;
            }
            else
                return _datasource;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            comboBox1.Select(0, 0);
            if (this.Visible && AllowNull == false && LimitToList && comboBox1.Items.Count > 0 && comboBox1.SelectedIndex == -1 && retainvalue == DBNull.Value)
            {
                    comboBox1.SelectedIndex = 0;
                    retainvalue = null;
                    OnActiveChanged();
                    OnChanged();
            }
            else if (retainvalue != null && this.Visible && comboBox1.DataSource != null)
            {
                comboBox1.SelectedValue = retainvalue;
                retainvalue = null;
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (lastgoodchoice == null)
                lastgoodchoice = comboBox1.SelectedValue;
        }



        public void SetRTL(Form parentform)
        {
            eBase2.SetRTL(this, comboBox1, _captionWidth, null, _captionTop, _captionTopHeight);
        }

        /// <summary>
        /// Calculates preffered control height taking into account caption alignment
        /// </summary>
        [Browsable(false)]
        public int PreferredHeight
        {
            get { return comboBox1.PreferredSize.Height + (_captionTop ? _captionTopHeight : 0); }
        }

        private void ValidateCtrlHeight()
        {
            if (omsDesignMode)
            {
                int preferredHeight = PreferredHeight;
                if (Height < preferredHeight)
                {
                    Height = preferredHeight;
                }
            }
        }

        /// <summary>
        /// Calculates caption top height in depends of font.
        /// </summary>
        /// <param name="isDeviceDpiRequired">Flag, should include device dpi recalculating</param>
        /// <returns>Caption top height</returns>
        private int CalcCaptionTopHeight(bool isDeviceDpiRequired)
        {
            using (var graphics = this.CreateGraphics())
            {
                return isDeviceDpiRequired
                    ? Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height * 96 / (omsDesignMode || Font == DefaultFont ? 96 : DeviceDpi)))
                    : Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height));
            }
        }
    }
}