using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eCollectionSelector.
    /// </summary>
    public class eCLCollectionSelector : System.Windows.Forms.UserControl, FWBS.Common.UI.IBasicEnquiryControl2, FWBS.Common.UI.IListEnquiryControl
    {
        #region Auto Fields
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel pnlNavButtons;
        private System.Windows.Forms.Button btnSelectItems;
        private System.Windows.Forms.Button btnUnselectAll;
        private System.Windows.Forms.Button btnUnselectItems;
        private System.Windows.Forms.Button btnSelectAll;
        private FWBS.Common.UI.Windows.ImageListBox Source;
        private FWBS.Common.UI.Windows.ImageListBox Destination;
        private System.Windows.Forms.Panel pnlSourceFilter;
        private System.Windows.Forms.TextBox txtSourceFilter;
        private System.Windows.Forms.Label lblSourceFilter;
        private System.Windows.Forms.Panel pnlDestinationFilter;
        private System.Windows.Forms.Label lblDestinationFilter;
        private System.Windows.Forms.TextBox txtDestinationFilter;
        private TableLayoutPanel tableLayoutPanel1;

        /// <summary>
        /// The Image Enum for a selection of Image Lists
        /// </summary>
        private omsImageLists _omsimagelists = omsImageLists.None;
        private IContainer components;

        public eCLCollectionSelector()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
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

        public event EventHandler <BeforeeCLCollectionChangeEventArgs> Changing;
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblSourceFilter = new System.Windows.Forms.Label();
            this.lblDestinationFilter = new System.Windows.Forms.Label();
            this.pnlSourceFilter = new System.Windows.Forms.Panel();
            this.txtSourceFilter = new System.Windows.Forms.TextBox();
            this.pnlNavButtons = new System.Windows.Forms.Panel();
            this.btnSelectItems = new System.Windows.Forms.Button();
            this.btnUnselectItems = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnUnselectAll = new System.Windows.Forms.Button();
            this.pnlDestinationFilter = new System.Windows.Forms.Panel();
            this.txtDestinationFilter = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Source = new FWBS.Common.UI.Windows.ImageListBox();
            this.Destination = new FWBS.Common.UI.Windows.ImageListBox();
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlSourceFilter.SuspendLayout();
            this.pnlNavButtons.SuspendLayout();
            this.pnlDestinationFilter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSourceFilter
            // 
            this.lblSourceFilter.AutoSize = true;
            this.lblSourceFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSourceFilter.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup.SetLookup(this.lblSourceFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblFilter", "Filter : ", ""));
            this.lblSourceFilter.Name = "lblSourceFilter";
            this.lblSourceFilter.Size = new System.Drawing.Size(38, 13);
            this.lblSourceFilter.TabIndex = 1;
            this.lblSourceFilter.Text = "Filter : ";
            this.lblSourceFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDestinationFilter
            // 
            this.lblDestinationFilter.AutoSize = true;
            this.lblDestinationFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDestinationFilter.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup.SetLookup(this.lblDestinationFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblFilter", "Filter : ", ""));
            this.lblDestinationFilter.Name = "lblDestinationFilter";
            this.lblDestinationFilter.Size = new System.Drawing.Size(38, 13);
            this.lblDestinationFilter.TabIndex = 5;
            this.lblDestinationFilter.Text = "Filter : ";
            this.lblDestinationFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSourceFilter
            // 
            this.pnlSourceFilter.AutoSize = true;
            this.pnlSourceFilter.Controls.Add(this.txtSourceFilter);
            this.pnlSourceFilter.Controls.Add(this.lblSourceFilter);
            this.pnlSourceFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSourceFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlSourceFilter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSourceFilter.Name = "pnlSourceFilter";
            this.pnlSourceFilter.Size = new System.Drawing.Size(232, 20);
            this.pnlSourceFilter.TabIndex = 0;
            // 
            // txtSourceFilter
            // 
            this.txtSourceFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSourceFilter.Location = new System.Drawing.Point(38, 0);
            this.txtSourceFilter.Margin = new System.Windows.Forms.Padding(0);
            this.txtSourceFilter.Name = "txtSourceFilter";
            this.txtSourceFilter.Size = new System.Drawing.Size(194, 20);
            this.txtSourceFilter.TabIndex = 2;
            this.txtSourceFilter.TextChanged += new System.EventHandler(this.txtFilterAvailable_TextChanged);
            // 
            // pnlNavButtons
            // 
            this.pnlNavButtons.Controls.Add(this.btnSelectItems);
            this.pnlNavButtons.Controls.Add(this.btnUnselectItems);
            this.pnlNavButtons.Controls.Add(this.btnSelectAll);
            this.pnlNavButtons.Controls.Add(this.btnUnselectAll);
            this.pnlNavButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNavButtons.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pnlNavButtons.Location = new System.Drawing.Point(232, 20);
            this.pnlNavButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlNavButtons.Name = "pnlNavButtons";
            this.pnlNavButtons.Size = new System.Drawing.Size(48, 244);
            this.pnlNavButtons.TabIndex = 8;
            // 
            // btnSelectItems
            // 
            this.btnSelectItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectItems.Location = new System.Drawing.Point(8, 0);
            this.btnSelectItems.Name = "btnSelectItems";
            this.btnSelectItems.Size = new System.Drawing.Size(32, 23);
            this.btnSelectItems.TabIndex = 9;
            this.btnSelectItems.Text = ">";
            this.btnSelectItems.Click += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // btnUnselectItems
            // 
            this.btnUnselectItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnselectItems.Location = new System.Drawing.Point(8, 32);
            this.btnUnselectItems.Name = "btnUnselectItems";
            this.btnUnselectItems.Size = new System.Drawing.Size(32, 23);
            this.btnUnselectItems.TabIndex = 10;
            this.btnUnselectItems.Text = "<";
            this.btnUnselectItems.Click += new System.EventHandler(this.btnUnselectItems_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectAll.Location = new System.Drawing.Point(8, 64);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(32, 23);
            this.btnSelectAll.TabIndex = 11;
            this.btnSelectAll.Text = ">>";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnUnselectAll
            // 
            this.btnUnselectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnselectAll.Location = new System.Drawing.Point(8, 96);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(32, 23);
            this.btnUnselectAll.TabIndex = 12;
            this.btnUnselectAll.Text = "<<";
            this.btnUnselectAll.Click += new System.EventHandler(this.btnUnselectAll_Click);
            // 
            // pnlDestinationFilter
            // 
            this.pnlDestinationFilter.AutoSize = true;
            this.pnlDestinationFilter.Controls.Add(this.txtDestinationFilter);
            this.pnlDestinationFilter.Controls.Add(this.lblDestinationFilter);
            this.pnlDestinationFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDestinationFilter.Location = new System.Drawing.Point(280, 0);
            this.pnlDestinationFilter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDestinationFilter.Name = "pnlDestinationFilter";
            this.pnlDestinationFilter.Size = new System.Drawing.Size(232, 20);
            this.pnlDestinationFilter.TabIndex = 4;
            // 
            // txtDestinationFilter
            // 
            this.txtDestinationFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDestinationFilter.Location = new System.Drawing.Point(38, 0);
            this.txtDestinationFilter.Margin = new System.Windows.Forms.Padding(0);
            this.txtDestinationFilter.Name = "txtDestinationFilter";
            this.txtDestinationFilter.Size = new System.Drawing.Size(194, 20);
            this.txtDestinationFilter.TabIndex = 6;
            this.txtDestinationFilter.TextChanged += new System.EventHandler(this.txtFilterAvailable2_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Source, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Destination, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlSourceFilter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlDestinationFilter, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlNavButtons, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(512, 264);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // Source
            // 
            this.Source.DisplayMember = "cddesc";
            this.Source.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Source.ImageColumnName = "";
            this.Source.IntegralHeight = false;
            this.Source.Location = new System.Drawing.Point(0, 20);
            this.Source.Margin = new System.Windows.Forms.Padding(0);
            this.Source.Name = "Source";
            this.Source.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Source.Size = new System.Drawing.Size(232, 244);
            this.Source.TabIndex = 3;
            this.Source.ValueMember = "cdcode";
            this.Source.DoubleClick += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // Destination
            // 
            this.Destination.DisplayMember = "cddesc";
            this.Destination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Destination.ImageColumnName = "";
            this.Destination.IntegralHeight = false;
            this.Destination.Location = new System.Drawing.Point(280, 20);
            this.Destination.Margin = new System.Windows.Forms.Padding(0);
            this.Destination.Name = "Destination";
            this.Destination.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Destination.Size = new System.Drawing.Size(232, 244);
            this.Destination.TabIndex = 7;
            this.Destination.ValueMember = "cdcode";
            this.Destination.DoubleClick += new System.EventHandler(this.btnUnselectItems_Click);
            // 
            // eCLCollectionSelector
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "eCLCollectionSelector";
            this.Size = new System.Drawing.Size(512, 264);
            this.Load += new System.EventHandler(this.eCLCollectionSelector_Load);
            this.ParentChanged += new System.EventHandler(this.eCLCollectionSelector_ParentChanged);
            this.pnlSourceFilter.ResumeLayout(false);
            this.pnlSourceFilter.PerformLayout();
            this.pnlNavButtons.ResumeLayout(false);
            this.pnlDestinationFilter.ResumeLayout(false);
            this.pnlDestinationFilter.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Fields
        private string _codetype = "";
        private string _value = null;
        private bool _required = false;
        private bool _omsdesign = false;
        private bool _isdirty = false;
        private string _valuesplit = ";";
        private bool _teminologyparse = false;
        private DataTable _source = null;
        private DataTable _destin = null;
        private string _displaymember = "";
        private string _valuemember = "";
        private string _existingsourcefilter = "";
        #endregion

        #region Properties

        [Browsable(false)]
        public int HighlightedItems
        {
            get
            {
                return this.Source.SelectedItems.Count;
            }
        }

        [Browsable(false)]
        public int AlreadySelectedItems
        {
            get
            {
                return this.Destination.Items.Count;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        public string ImageIndexColumnName
        {
            get
            {
                return Source.ImageColumnName;
            }
            set
            {
                Source.ImageColumnName = value;
                Destination.ImageColumnName = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public ImageList ImageList
        {
            get
            {
                return Source.ImageList;
            }
            set
            {
                Source.ImageList = value;
                Destination.ImageList = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(omsImageLists.None)]
        public omsImageLists Resources
        {
            get
            {
                return _omsimagelists;
            }
            set
            {
                if (_omsimagelists != value)
                {
                    switch (value)
                    {
                        case omsImageLists.AdminMenu16:
                            {
                                ImageList = Images.AdminMenu16();
                                break;
                            }
                        case omsImageLists.AdminMenu32:
                            {
                                ImageList = Images.AdminMenu32();
                                break;
                            }
                        case omsImageLists.Arrows:
                            {
                                ImageList = Images.Arrows;
                                break;
                            }
                        case omsImageLists.PlusMinus:
                            {
                                ImageList = Images.PlusMinus;
                                break;
                            }
                        case omsImageLists.CoolButtons16:
                            {
                                ImageList = Images.CoolButtons16();
                                break;
                            }
                        case omsImageLists.CoolButtons24:
                            {
                                ImageList = Images.GetCoolButtons24();
                                break;
                            }
                        case omsImageLists.Developments16:
                            {
                                ImageList = Images.Developments();
                                break;
                            }
                        case omsImageLists.Entities16:
                            {
                                ImageList = Images.Entities();
                                break;
                            }
                        case omsImageLists.Entities32:
                            {
                                ImageList = Images.Entities32();
                                break;
                            }
                        case omsImageLists.imgFolderForms16:
                            {
                                ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size16);
                                break;
                            }
                        case omsImageLists.imgFolderForms32:
                            {
                                ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size32);
                                break;
                            }
                        case omsImageLists.None:
                            {
                                ImageList = null;
                                break;
                            }
                    }
                }
                _omsimagelists = value;
            }
        }

        [LocCategory("SETTINGS")]
        [DefaultValue("")]
        public string CodeType
        {
            get
            {
                return _codetype;
            }
            set
            {
                if (_codetype != value)
                {
                    _codetype = value;
                    eCLCollectionSelector_ParentChanged(this, EventArgs.Empty);
                }
            }
        }

        [LocCategory("SETTINGS")]
        [DefaultValue(";")]
        public string ValueSplit
        {
            get
            {
                return _valuesplit;
            }
            set
            {
                if (_valuesplit != value)
                {
                    _valuesplit = value;
                    eCLCollectionSelector_ParentChanged(this, EventArgs.Empty);
                }
            }
        }

        [LocCategory("SETTINGS")]
        [Description("Parse the Description through the OMS Terminolgy Parser e.g. %FILE% to File")]
        [DefaultValue(false)]
        public bool TerminologyParse
        {
            get
            {
                return _teminologyparse;
            }
            set
            {
                if (_teminologyparse != value)
                {
                    _teminologyparse = value;
                    eCLCollectionSelector_ParentChanged(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable SelectionList
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                //changed DMB 19/4/2004 if there was no incoming filter it would break later on
                if (value.DefaultView.RowFilter != "")
                    _existingsourcefilter = "(" + value.DefaultView.RowFilter + ") AND ";
                _destin = value.Clone();
                Source.DataSource = _source;
                if (_displaymember == "")
                    _displaymember = value.Columns[1].ColumnName;
                Source.DisplayMember = _displaymember;
                if (_valuemember == "")
                    _valuemember = value.Columns[0].ColumnName;
                Source.ValueMember = _valuemember;
                _source.DefaultView.Sort = _displaymember;
            }
        }

        [Browsable(false)]
        public DataTable SelectedItems
        {
            get
            {
                return (DataTable)Destination.DataSource;
            }
        }
        #endregion

        #region Methods
        public virtual void OnChanging(BeforeeCLCollectionChangeEventArgs e)
        {
            if (Changing != null)
                Changing(this, e);
        }
        #endregion

        #region Private
        private void eCLCollectionSelector_ParentChanged(object sender, System.EventArgs e)
        {
            if (this.Parent != null && _codetype != "")
            {
                _source = FWBS.OMS.CodeLookup.GetLookups(_codetype);
                if (_teminologyparse)
                {
                    Terminology terminology = Session.CurrentSession.Terminology;
                    DataView tp = new DataView(_source, "cddesc like '%[%]%'", "", DataViewRowState.CurrentRows);
                    for (int ctr = tp.Count - 1; ctr >= 0; ctr--)
                    {
                        tp[ctr]["cddesc"] = terminology.Parse(Convert.ToString(tp[ctr]["cddesc"]), true);
                    }
                    _source.AcceptChanges();
                }
                _destin = _source.Clone();
                _valuemember = "cdcode";
            }
            if (this.Parent != null && _value != null && _source != null && _destin != null)
            {
                string[] _values = _value.Split(new char[] { _valuesplit[0]}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string vl in _values)
                {
                    DataView temp;
                    try
                    {
                        temp = new DataView(_source, _valuemember + " = '" + vl + "'", "", DataViewRowState.CurrentRows);
                    }
                    catch
                    {
                        temp = new DataView(_source, _valuemember + " = " + vl, "", DataViewRowState.CurrentRows);
                    }
                    if (temp.Count > 0)
                    {
                        _destin.Rows.Add(temp[0].Row.ItemArray);
                        temp[0].Delete();
                    }
                }
            }
        }

        private void btnSelectItems_Click(object sender, System.EventArgs e)
        {
            if (!CheckChanging(SelectionType.SelectOne))
            {
                Destination.BeginUpdate();
                Source.BeginUpdate();
                ArrayList temp = new ArrayList();
                foreach (object obj in Source.SelectedItems)
                {
                    DataRowView vw = obj as DataRowView;
                    _destin.Rows.Add(vw.Row.ItemArray);
                    temp.Add(vw);
                }
                for (int i = temp.Count - 1; i > -1; i--)
                    ((DataRowView)temp[i]).Delete();
                Destination.EndUpdate();
                Source.EndUpdate();
                Refresh();
            }
        }

        private void btnUnselectItems_Click(object sender, System.EventArgs e)
        {
            Destination.BeginUpdate();
            Source.BeginUpdate();
            ArrayList temp = new ArrayList();
            foreach (object obj in Destination.SelectedItems)
            {
                DataRowView vw = obj as DataRowView;
                _source.Rows.Add(vw.Row.ItemArray);
                temp.Add(vw);
            }
            for (int i = temp.Count - 1; i > -1; i--)
                ((DataRowView)temp[i]).Delete();
            Destination.EndUpdate();
            Source.EndUpdate();
            Refresh();
        }

        private void btnSelectAll_Click(object sender, System.EventArgs e)
        {
            if (!CheckChanging(SelectionType.SelectAll))
            {
                Destination.BeginUpdate();
                Source.BeginUpdate();
                ArrayList temp = new ArrayList();
                foreach (object obj in Source.Items)
                {
                    DataRowView vw = obj as DataRowView;
                    _destin.Rows.Add(vw.Row.ItemArray);
                    temp.Add(vw);
                }
                for (int i = temp.Count - 1; i > -1; i--)
                    ((DataRowView)temp[i]).Delete();
                Destination.EndUpdate();
                Source.EndUpdate();
                Refresh();
            }
        }

        private bool CheckChanging(SelectionType selectionType)
        {
            var ea = new BeforeeCLCollectionChangeEventArgs();
            ea.Direction = Direction.Add;
            ea.SelectionType = selectionType;
            ea.Cancel = false;
            OnChanging(ea);
            return ea.Cancel;
        }
        
        private void btnUnselectAll_Click(object sender, System.EventArgs e)
        {
            Destination.BeginUpdate();
            Source.BeginUpdate();
            ArrayList temp = new ArrayList();
            foreach (object obj in Destination.Items)
            {
                DataRowView vw = obj as DataRowView;
                _source.Rows.Add(vw.Row.ItemArray);
                temp.Add(vw);
            }
            for (int i = temp.Count - 1; i > -1; i--)
                ((DataRowView)temp[i]).Delete();
            Destination.EndUpdate();
            Source.EndUpdate();
            Refresh();
        }

        public new void Refresh()
        {
            Refresh(true);
        }

        public void Refresh(bool Dirty)
        {
            Destination.BeginUpdate();
            Source.BeginUpdate();
            Destination.DataSource = _destin;
            Source.DataSource = _source;
            if (_valuemember == "")
            {
                Destination.ValueMember = "cdcode";
                Source.ValueMember = "cdcode";
            }
            else
            {
                Destination.ValueMember = _valuemember;
                Source.ValueMember = _valuemember;
            }
            if (_displaymember == "")
            {
                Destination.DisplayMember = "cddesc";
                Source.DisplayMember = "cddesc";
            }
            else
            {
                Destination.DisplayMember = _displaymember;
                Source.DisplayMember = _displaymember;
            }
            Destination.EndUpdate();
            Source.EndUpdate();
            _value = "";
            string oldfilter = "";
            if (Destination.DataSource is DataTable)
            {
                oldfilter = ((DataTable)Destination.DataSource).DefaultView.RowFilter;
                ((DataTable)Destination.DataSource).DefaultView.RowFilter = "";
            }
            foreach (DataRowView vw in Destination.Items)
            {
                if (_valuemember == "")
                    _value += Convert.ToString(vw["cdcode"]) + _valuesplit;
                else
                    _value += Convert.ToString(vw[_valuemember]) + _valuesplit;
            }
            _value = _value.Trim(_valuesplit.ToCharArray());
            if (Destination.DataSource is DataTable)
            {
                ((DataTable)Destination.DataSource).DefaultView.RowFilter = oldfilter;
            }
            if (Dirty)
            {
                OnActiveChanged();
                OnChanged();
            }
        }
        #endregion

        #region IBasicEnquiryControl2 Members
        [Browsable(false)]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return !this.Enabled;
            }
            set
            {
                this.Enabled = !value;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
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

        public void OnActiveChanged()
        {
            IsDirty = true;
            if (ActiveChanged != null)
                ActiveChanged(this, EventArgs.Empty);
        }

        [Browsable(false)]
        public object Control
        {
            get
            {
                return null;
            }
        }

        [Browsable(false)]
        [DefaultValue(null)]
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (string.IsNullOrEmpty(_value) || _value != Convert.ToString(value))
                {
                    _value = Convert.ToString(value);
                    eCLCollectionSelector_ParentChanged(this, EventArgs.Empty);
                    Refresh();
                }
            }
        }

        [Browsable(false)]
        public bool LockHeight
        {
            get
            {
                return false;
            }
        }

        [Browsable(false)]
        [DefaultValue(0)]
        public int CaptionWidth
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

        public void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        private void eCLCollectionSelector_Load(object sender, System.EventArgs e)
        {
            Refresh(false);
        }

        private void txtFilterAvailable_TextChanged(object sender, System.EventArgs e)
        {
            string searchstring = FWBS.Common.SQLRoutines.RemoveRubbish(txtSourceFilter.Text);
            searchstring = searchstring.Replace("[", "ÿþýÊ");
            searchstring = searchstring.Replace("]", "Êÿýþ");
            searchstring = searchstring.Replace("ÿþýÊ", "[[]");
            searchstring = searchstring.Replace("Êÿýþ", "[]]");
            searchstring = searchstring.Replace("%", "[%]");
            if(Source.DataSource != null)
                ((DataTable)Source.DataSource).DefaultView.RowFilter = _existingsourcefilter + Source.DisplayMember + " Like '*" + searchstring + "*'";
        }

        private void txtFilterAvailable2_TextChanged(object sender, System.EventArgs e)
        {
            string searchstring = FWBS.Common.SQLRoutines.RemoveRubbish(txtDestinationFilter.Text);
            searchstring = searchstring.Replace("[", "ÿþýÊ");
            searchstring = searchstring.Replace("]", "Êÿýþ");
            searchstring = searchstring.Replace("ÿþýÊ", "[[]");
            searchstring = searchstring.Replace("Êÿýþ", "[]]");
            searchstring = searchstring.Replace("%", "[%]");
            if(Destination.DataSource != null)
                ((DataTable)Destination.DataSource).DefaultView.RowFilter = _existingsourcefilter + Destination.DisplayMember + " Like '*" + searchstring + "*'";
        }

        public event System.EventHandler ActiveChanged;

        [Browsable(false)]
        [DefaultValue(false)]
        public bool omsDesignMode
        {
            get
            {
                return _omsdesign;
            }
            set
            {
                _omsdesign = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsDirty
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

        [Browsable(false)]
        public override string Text
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public string DisplayMember
        {
            set
            {
                _displaymember = value;
            }
        }

        public string ValueMember
        {
            set
            {
                _valuemember = value;
            }
        }

        public event System.EventHandler Changed;
        #endregion

        #region IListEnquiryControl Implementation
        /// <summary>
        /// Gets or Sets the display text section of the combo box.
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
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
        public void AddItem(object Value, string displayText)
        {

        }

        /// <summary>
        /// Assigns a data table object to the datasource of the combo box which uses
        /// the first column as bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataTable">Bound data table.</param>
        public void AddItem(DataTable dataTable)
        {
            if (dataTable.Columns.Count > 0)
            {
                ValueMember = dataTable.Columns[0].ColumnName;
                if (dataTable.Columns.Count > 1)
                    DisplayMember = dataTable.Columns[1].ColumnName;
                else
                    DisplayMember = dataTable.Columns[0].ColumnName;
            }
            _source = dataTable;
            _destin = _source.Clone();
            if (this.Parent != null) Refresh();
        }

        /// <summary>
        /// Assigns a data table object to the datasource of the combo box which uses
        /// the a specified value and display column.
        /// </summary>
        /// <param name="dataTable">Bound data table.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public void AddItem(DataTable dataTable, string valueMember, string displayMember)
        {
            if (dataTable.Columns.Contains(valueMember))
            {
                ValueMember = dataTable.Columns[valueMember].ColumnName;
            }
            if (dataTable.Columns.Contains(displayMember))
            {
                DisplayMember = dataTable.Columns[displayMember].ColumnName;
            }
            _source = dataTable;
            _destin = _source.Clone();
            if (this.Parent != null) Refresh();
        }

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the first column as the bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataTable">Bound data view.</param>
        public void AddItem(DataView dataView)
        {
            if (dataView.Table != null)
            {
                if (dataView.Table.Columns.Count > 0)
                {
                    ValueMember = dataView.Table.Columns[0].ColumnName;
                    if (dataView.Table.Columns.Count > 1)
                        DisplayMember = dataView.Table.Columns[1].ColumnName;
                    else
                        DisplayMember = dataView.Table.Columns[0].ColumnName;
                }
                _source = dataView.Table;
                _destin = _source.Clone();
                if (this.Parent != null) Refresh();
            }
        }

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the a specified value and display column.
        /// </summary>
        /// <param name="dataTable">Bound data view.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public void AddItem(DataView dataView, string valueMember, string displayMember)
        {
            if (dataView.Table != null)
            {
                if (dataView.Table.Columns.Contains(valueMember))
                {
                    ValueMember = dataView.Table.Columns[valueMember].ColumnName;
                }
                if (dataView.Table.Columns.Contains(displayMember))
                {
                    DisplayMember = dataView.Table.Columns[displayMember].ColumnName;
                }
                _source = dataView.Table;
                _destin = _source.Clone();
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
                return Source.Items.Count;
            }
        }

        /// <summary>
        /// Filters the bound result set with the specified Filter String.
        /// </summary>
        /// <param name="FilterString">Filter String</param>
        public bool Filter(string FilterString)
        {
            DataTable dt = null;
            if (Source.DataSource is DataView)
            {
                dt = ((DataView)Source.DataSource).Table;
            }
            else if (Source.DataSource is DataTable)
            {
                dt = (DataTable)Source.DataSource;
            }
            if (dt != null)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = FilterString + " or " + Source.ValueMember + " is null";
                Source.DataSource = dv;
            }
            if (Source.Items.Count > 0)
                Source.SelectedIndex = 0;
            return true;
        }

        /// <summary>
        /// Filters the bound result set with the specified value under the specified field.
        /// </summary>
        /// <param name="fieldName">The field name to filter with.</param>
        /// <param name="Value">The value to filter for.</param>
        public bool Filter(string fieldName, object Value)
        {
            // Removed Value == null from get out clause MNW
            if (fieldName == null) return false;
            DataTable dt = null;
            if (Source.DataSource is DataView)
            {
                dt = ((DataView)Source.DataSource).Table;
            }
            else if (Source.DataSource is DataTable)
            {
                dt = (DataTable)Source.DataSource;
            }
            if (dt != null)
            {
                try
                {
                    DataView dv = new DataView(dt);
                    string filter;
                    // Check for NULL Value 
                    if (Value != System.DBNull.Value)
                        filter = fieldName + " = '" + Convert.ToString(Value).Replace("'", "''") + "' or " + Source.ValueMember + " is null";
                    else
                        filter = fieldName + " is null";
                    //Re-apply the value after the filter has been performed.
                    object val = this.Value;
                    dv.RowFilter = filter;
                    Source.DataSource = dv;
                    this.Value = val;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Session.CurrentSession.Resources.GetMessage("ERRORSHOW", "Error", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return true;
        }
        #endregion
    }
}
