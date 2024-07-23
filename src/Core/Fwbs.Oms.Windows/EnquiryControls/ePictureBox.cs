using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ePictureBox.
    /// </summary>
    public class ePictureBox : UserControl, FWBS.Common.UI.IListEnquiryControl
    {
        #region Fields
        /// <summary>
        /// The Top Containing Panel
        /// </summary>
        private System.Windows.Forms.Panel pnlTop;
        /// <summary>
        /// The Browse Button
        /// </summary>
        private FWBS.OMS.UI.Windows.ePictureBox.FlatButton btnBrowse;
        /// <summary>
        /// A Hidden Text Box for the Value
        /// </summary>
        private System.Windows.Forms.TextBox txtValue;
        /// <summary>
        /// A Combo Box for the Value
        /// </summary>
        private System.Windows.Forms.ComboBox cmbValues;
        /// <summary>
        /// The Caption
        /// </summary>
        private System.Windows.Forms.Label labCaption;
        /// <summary>
        /// The Preview View
        /// </summary>
        private System.Windows.Forms.PictureBox picImage;
        /// <summary>
        /// File Dialog to Browse for a Image
        /// </summary>
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.IContainer components;
        /// <summary>
        /// The Image Object to Store The Picture and any naration of the Image
        /// </summary>
        private FWBS.OMS.Images _image = null;
        /// <summary>
        /// The Image Value Index
        /// </summary>
        private object _value = DBNull.Value;
        /// <summary>
        /// The Enquiry form Leached from the Parent
        /// </summary>
        private EnquiryForm _form = null;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuItem mnuFullScreen;
        private System.Windows.Forms.ContextMenu popMenu;
        private object _last = DBNull.Value;
        private System.Windows.Forms.MenuItem mnPrint;
        private FWBS.OMS.UI.Windows.ePictureBox.FlatButton btnDel;
        private readonly ComponentResourceManager _resources = new ComponentResourceManager(typeof(ePictureBox));
        #endregion

        #region Constructors & Despose
        public ePictureBox() : base()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            try
            {
                _image = new FWBS.OMS.Images();
            }
            catch { }

            if (Session.CurrentSession.IsConnected)
            {
                Res res = Session.CurrentSession.Resources;
                mnuFullScreen.Text = res.GetResource("MNUFULLSCREEN", "&Full Screen", "").Text;
                mnPrint.Text = res.GetResource("MNUPRINT", "&Print...", "").Text;
                openFileDialog1.Title = res.GetResource("BRWSFRIMG", "Browse for Image ...", "").Text;
                toolTip1.SetToolTip(picImage, res.GetResource("RGTHCLKCFOROPTS", "Right Click for Options", "").Text);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ePictureBox));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cmbValues = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new FWBS.OMS.UI.Windows.ePictureBox.FlatButton();
            this.btnDel = new FWBS.OMS.UI.Windows.ePictureBox.FlatButton();
            this.popMenu = new System.Windows.Forms.ContextMenu();
            this.mnuFullScreen = new System.Windows.Forms.MenuItem();
            this.mnPrint = new System.Windows.Forms.MenuItem();
            this.labCaption = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picImage = new System.Windows.Forms.PictureBox();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.AutoSize = true;
            this.pnlTop.Controls.Add(this.txtValue);
            this.pnlTop.Controls.Add(this.cmbValues);
            this.pnlTop.Controls.Add(this.btnBrowse);
            this.pnlTop.Controls.Add(this.btnDel);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 21);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.pnlTop.Size = new System.Drawing.Size(256, 53);
            this.pnlTop.TabIndex = 0;
            // 
            // txtValue
            // 
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtValue.Location = new System.Drawing.Point(0, 21);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(192, 20);
            this.txtValue.TabIndex = 2;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.txtValue.Leave += new System.EventHandler(this.txtValue_Leave);
            // 
            // cmbValues
            // 
            this.cmbValues.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValues.Location = new System.Drawing.Point(0, 0);
            this.cmbValues.Name = "cmbValues";
            this.cmbValues.Size = new System.Drawing.Size(192, 21);
            this.cmbValues.TabIndex = 1;
            this.cmbValues.Visible = false;
            this.cmbValues.SelectionChangeCommitted += new System.EventHandler(this.cmbValues_SelectionChangeCommitted);
            this.cmbValues.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.cmbValues.Leave += new System.EventHandler(this.cmbValues_Leave);
            // 
            // btnBrowse
            // 
            this.btnBrowse.DisabledImageName = "upload_disabled";
            this.btnBrowse.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(192, 0);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.SelectedImageName = "upload_selected";
            this.btnBrowse.Size = new System.Drawing.Size(32, 41);
            this.btnBrowse.StandardImageName = "upload";
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnDel
            // 
            this.btnDel.DisabledImageName = "delete_disabled";
            this.btnDel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDel.Enabled = false;
            this.btnDel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnDel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDel.Location = new System.Drawing.Point(224, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.SelectedImageName = "delete_selected";
            this.btnDel.Size = new System.Drawing.Size(32, 41);
            this.btnDel.StandardImageName = "delete";
            this.btnDel.TabIndex = 4;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // popMenu
            // 
            this.popMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFullScreen,
            this.mnPrint});
            // 
            // mnuFullScreen
            // 
            this.mnuFullScreen.Enabled = false;
            this.mnuFullScreen.Index = 0;
            this.mnuFullScreen.Text = "&Full Screen";
            this.mnuFullScreen.Click += new System.EventHandler(this.mnuFullScreen_Click);
            // 
            // mnPrint
            // 
            this.mnPrint.Enabled = false;
            this.mnPrint.Index = 1;
            this.mnPrint.Text = "&Print...";
            this.mnPrint.Visible = false;
            this.mnPrint.Click += new System.EventHandler(this.mnPrint_Click);
            // 
            // labCaption
            // 
            this.labCaption.AutoSize = true;
            this.labCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labCaption.Location = new System.Drawing.Point(0, 0);
            this.labCaption.Name = "labCaption";
            this.labCaption.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.labCaption.Size = new System.Drawing.Size(0, 21);
            this.labCaption.TabIndex = 0;
            this.labCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labCaption.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = resources.GetString("openFileDialog1.Filter");
            // 
            // picImage
            // 
            this.picImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picImage.ContextMenu = this.popMenu;
            this.picImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImage.Location = new System.Drawing.Point(0, 74);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(256, 198);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 2;
            this.picImage.TabStop = false;
            this.picImage.DoubleClick += new System.EventHandler(this.mnuFullScreen_Click);
            // 
            // ePictureBox
            // 
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.labCaption);
            this.Name = "ePictureBox";
            this.Size = new System.Drawing.Size(256, 272);
            this.VisibleChanged += new System.EventHandler(this.ePictureBox_VisibleChanged);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Private
        /// <summary>
        /// Browse Button Click
        /// </summary>
        /// <param name="sender">Self</param>
        /// <param name="e">Empty</param>
        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            SetInitialPathForDialog(openFileDialog1);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                toolTip1.RemoveAll();
                toolTip1.SetToolTip(picImage, $"{openFileDialog1.FileName}\n\n{Session.CurrentSession.Resources.GetResource("RGTHCLKCFOROPTS", "Right Click for Options", "").Text}");

                btnDel.Enabled = mnuFullScreen.Enabled = true;
                try
                {
                    if (_form == null)
                    {
                        _image.Image = System.Drawing.Image.FromFile(openFileDialog1.FileName);
                        picImage.Image = _image.Image;
                        _image.Update();
                        this.Value = _image.ID;
                    }
                    else
                    {
                        if (_value is Signature)
                        {
                            this.Value = new Signature(new System.IO.FileInfo(openFileDialog1.FileName));
                            picImage.Image = ((Signature)_value).ToBitmap();
                        }
                        else
                        {
                            Image temp = System.Drawing.Image.FromFile(openFileDialog1.FileName);
                            picImage.Image = temp;
                            _image = new FWBS.OMS.Images();
                            _image.Image = temp;
                        }

                        OnActiveChanged();
                        OnChanged();
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, new OMSException2("IMGTYPERR","Error Invalid Image Type. Please try again",ex));
                }
            }
        }
        

        private void mnuFullScreen_Click(object sender, System.EventArgs e)
        {
            using (FWBS.Common.UI.Windows.frmFullScreen frmView = new FWBS.Common.UI.Windows.frmFullScreen())
            {
                if (_value is Signature)
                {
                    if (((Signature)_value).ToBitmap() != null)
                        frmView.DisplayImage(((Signature)_value).ToBitmap());
                }
                else if (_image.Image != null)
                    frmView.DisplayImage(_image.Image);
                else
                    return;
                frmView.ShowDialog();
            }
        }

        private void _form_Updating(object sender, CancelEventArgs e)
        {
            if (IsDirty)
            {
                if (_value is Signature == false)
                {
                    if (picImage.Image != null)
                    {
                        _image.Text = cmbValues.DataSource == null
                            ? txtValue.Text
                            : cmbValues.Text;
                        _image.Update();
                        this.Value = _image.ID;
                    }
                    else
                    {
                        if (_image.IsNew == false)
                        {
                            _image.Delete();
                            _image.Create();
                        }
                    }
                }
            }
        }
        
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is EnquiryForm)
            {
                if (_form != null)
                {
                    _form.Updating -= new CancelEventHandler(_form_Updating);
                    _form.Finishing -= new CancelEventHandler(_form_Updating);
                }

                _form = Parent as EnquiryForm;
                _form.Updating += new CancelEventHandler(_form_Updating);
                _form.Finishing += new CancelEventHandler(_form_Updating);
            }
            else if (Parent == null && _form != null)
            {
                _form.Updating -= new CancelEventHandler(_form_Updating);
                _form.Finishing -= new CancelEventHandler(_form_Updating);
            }
        }

        private void mnPrint_Click(object sender, System.EventArgs e)
        {
        }

        private void txtValue_TextChanged(object sender, System.EventArgs e)
        {
            OnActiveChanged();
            _form.IsDirty=true;
        }

        private void cmbValues_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            OnActiveChanged();
            _form.IsDirty=true;
        }

        private void btnDel_Click(object sender, System.EventArgs e)
        {
            if (picImage.Image != null)
            {
                if (MessageBox.ShowYesNoQuestion("RUSUREDELETE","Are you sure you wish to delete?") == DialogResult.Yes)
                {
                    toolTip1.RemoveAll();
                    toolTip1.SetToolTip(picImage, Session.CurrentSession.Resources.GetResource("RGTHCLKCFOROPTS", "Right Click for Options", "").Text);

                    btnDel.Enabled = mnuFullScreen.Enabled = false;
                    picImage.Image = null;
                    if (_value is Signature)
                    {
                        this.Value = Signature.Empty;
                    }
                    else
                    {
                        this.Value = DBNull.Value;
                        if (cmbValues.Items.Count > 0)
                        {
                            cmbValues.SelectedIndex = 0;
                        }
                        txtValue.Text = "";
                    }
                    _image = new FWBS.OMS.Images();
                    OnActiveChanged();
                    OnChanged();
                }
            }
        }

        #endregion

        #region IBasicEnquiryControl2 Members
        /// <summary>
        /// readonly variable
        /// </summary>
        private bool _readonly = false;
        /// <summary>
        /// required variable 
        /// </summary>
        private bool _required = false;
        /// <summary>
        /// in design mode variable
        /// </summary>
        private bool _designonly = false;
        /// <summary>
        /// Is Dirty variable
        /// </summary>
        private bool _isdirty = false;

        [Browsable(false)]
        public int CaptionWidth
        {
            get { return -1; }
            set { }
        }

        [Browsable(false)]
        [DefaultValue(true)]
        public bool CaptionTop
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Property to return the Default Control
        /// in this case I am return null
        /// </summary>    
        public object Control => null;

        /// <summary>
        /// Read Only Property
        /// </summary>
        [Browsable(false)]
        public bool ReadOnly
        {
            get
            {
                return _readonly;
            }
            set
            {
                btnBrowse.Enabled = !value;
                btnDel.Enabled = picImage.Image != null;
                txtValue.Enabled = !value;
                cmbValues.Enabled = !value;
                _readonly = value;
            }
        }

        /// <summary>
        /// The Value object
        /// </summary>
        [Browsable(false)]
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (value == DBNull.Value)
                    {
                        picImage.Image = null;
                        _image = new FWBS.OMS.Images();
                        cmbValues.SelectedIndex = -1;
                        txtValue.Text = "";
                        _last = null;
                    }
                    else
                        ePictureBox_VisibleChanged(this, EventArgs.Empty);

                    if (this.Parent != null)
                    {
                        IsDirty = true;
                        OnChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Get or Set Property to Store the Required 
        /// </summary>
        [Browsable(false)]
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


        [Browsable(false)]
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
        public bool LockHeight => false;

        [Browsable(false)]
        public bool omsDesignMode
        {
            get
            {
                return _designonly;
            }
            set
            {
                _designonly = value;
            }
        }

        public event EventHandler ActiveChanged;
        public event EventHandler Changed;

        /// <summary>
        /// Public Method to launch ActiveChange Event
        /// </summary>
        public void OnActiveChanged()
        {
            IsDirty = true;
            ActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnChanged()
        {
            if (Changed != null && IsDirty)
                Changed(this, EventArgs.Empty);
        }

        private void ePictureBox_VisibleChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (this.Parent != null && Visible && _last != _value)
                {
                    btnDel.Enabled = mnuFullScreen.Enabled = false;
                    if (_value is Signature)
                    {
                        Bitmap temp = ((Signature)_value).ToBitmap();
                        if (temp != null)
                        {
                            picImage.Image = temp;
                            btnDel.Enabled = mnuFullScreen.Enabled = true;
                        }

                        txtValue.Visible = false;
                        cmbValues.Visible = false;

                        labCaption.Parent = pnlTop;
                        btnBrowse.SendToBack();
                        btnDel.SendToBack();
                        
                        labCaption.Visible = true;
                    }
                    else if (_image != null)
                    {
                        labCaption.Parent = this;
                        labCaption.Visible = !string.IsNullOrEmpty(labCaption.Text);

                        if (cmbValues.DataSource != null)
                        {
                            cmbValues.Visible = true;
                            txtValue.Visible = false;
                        }
                        else
                        {
                            cmbValues.Visible = false;
                            txtValue.Visible = true;
                        }

                        try
                        {
                            _image.GetImage(Convert.ToInt64(_value));
                            txtValue.Text = _image.Text;
                            cmbValues.Text = _image.Text;
                            picImage.Image = _image.Image;
                            btnDel.Enabled = mnuFullScreen.Enabled = true;
                            _last = _value;
                        }
                        catch
                        {
                            _image.Create();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void cmbValues_Leave(object sender, System.EventArgs e)
        {
            if (_form == null && IsDirty)
            {
                _image.Text = cmbValues.Text;
                _value = _image.ID;
            }
            OnChanged();
        }

        private void txtValue_Leave(object sender, System.EventArgs e)
        {
            if (_form == null && IsDirty)
            {
                _image.Text = txtValue.Text;
                _value = _image.ID;
            }
            OnChanged();
        }

        public override string Text
        {
            get
            {
                return this.labCaption.Text;
            }
            set
            {
                this.labCaption.Text = value;
                this.labCaption.Visible = !string.IsNullOrEmpty(value);
            }
        }

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
                return cmbValues.Text;
            }
            set
            {
                cmbValues.Text = Convert.ToString(value);
            }
        }

        /// <summary>
        /// Assigns an individual bound value and a display value for a combo box.
        /// </summary>
        /// <param name="Value">Invisible bound value.</param>
        /// <param name="displayText">Visible display text.</param>
        public void AddItem(object Value, string displayText)
        {
            ComboBox cbo = cmbValues;
            cbo.BeginUpdate();
            FWBS.Common.UI.EnquiryListItem itm = new FWBS.Common.UI.EnquiryListItem(Value, displayText);
            cbo.Items.Add(itm);
            cbo.EndUpdate();
        }

        /// <summary>
        /// Assigns a data table object to the datasource of the combo box which uses
        /// the first column as bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataTable">Bound data table.</param>
        public void AddItem(DataTable dataTable)
        {
            ComboBox cbo = cmbValues;
            cbo.BeginUpdate();
            if (dataTable.Columns.Count > 0)
            {
                cbo.ValueMember = dataTable.Columns[0].ColumnName;
                if (dataTable.Columns.Count > 1)
                    cbo.DisplayMember = dataTable.Columns[1].ColumnName;
                else
                    cbo.DisplayMember = dataTable.Columns[0].ColumnName;
            }
            cbo.DataSource= dataTable;
            cbo.EndUpdate();
            txtValue.Visible=false;
            cbo.Visible =true;
        }

        /// <summary>
        /// Assigns a data table object to the datasource of the combo box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataTable">Bound data table.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public void AddItem(DataTable dataTable, string valueMember, string displayMember)
        {
            ComboBox cbo = cmbValues;
            cbo.BeginUpdate();
            if (dataTable.Columns.Contains(valueMember))
            {
                cbo.ValueMember = dataTable.Columns[valueMember].ColumnName;
            }
            if (dataTable.Columns.Contains(displayMember))
            {
                cbo.DisplayMember = dataTable.Columns[displayMember].ColumnName;
            }
            cbo.DataSource = dataTable;
            cbo.EndUpdate();
            txtValue.Visible=false;
            cbo.Visible =true;
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
                ComboBox cbo = cmbValues;
                cbo.BeginUpdate();
                if (dataView.Table.Columns.Count > 0)
                {
                    cbo.ValueMember = dataView.Table.Columns[0].ColumnName;
                    if (dataView.Table.Columns.Count > 1)
                        cbo.DisplayMember = dataView.Table.Columns[1].ColumnName;
                    else
                        cbo.DisplayMember = dataView.Table.Columns[0].ColumnName;
                }
                cbo.DataSource= dataView;
                cbo.EndUpdate();
                txtValue.Visible=false;
                cbo.Visible =true;
            }
        }

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataTable">Bound data view.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public void AddItem(DataView dataView, string valueMember, string displayMember)
        {
            if (dataView.Table != null)
            {
                ComboBox cbo = cmbValues;
                cbo.BeginUpdate();
                if (dataView.Table.Columns.Contains(valueMember))
                {
                    cbo.ValueMember = dataView.Table.Columns[valueMember].ColumnName;
                }
                if (dataView.Table.Columns.Contains(displayMember))
                {
                    cbo.DisplayMember = dataView.Table.Columns[displayMember].ColumnName;
                }
                cbo.DataSource = dataView;
                cbo.EndUpdate();
                txtValue.Visible=false;
                cbo.Visible =true;
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
                return cmbValues.Items.Count;
            }
        }

        /// <summary>
        /// Filters the bound result set withe the specified Filter String
        /// </summary>
        /// <param name="FilterString">Filter String</param>
        public bool Filter (string FilterString)
        {
            DataTable dt = null;
            ComboBox cbo = cmbValues;
            if (cbo.DataSource is DataView)
            {
                dt = ((DataView)cbo.DataSource).Table;
            }
            else if (cbo.DataSource is DataTable)
            {
                dt = (DataTable)cbo.DataSource;
            }

            if (dt != null)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = FilterString + " or " + cbo.ValueMember + " is null";
                cbo.DataSource = dv;
            }
            if (cbo.Items.Count > 0)
                cbo.SelectedIndex = 0;
            return true;

        }

        /// <summary>
        /// Filters the bound result set with the specified value under the specified field.
        /// </summary>
        /// <param name="fieldName">The field name to filter with.</param>
        /// <param name="Value">The value to filter for.</param>
        public bool Filter (string fieldName, object Value)
        {
            // Removed Value == null from get out clause MNW
            if (fieldName == null) return false;
            ComboBox cbo = cmbValues;
            DataTable dt = null;
            if (cbo.DataSource is DataView)
            {
                dt = ((DataView)cbo.DataSource).Table;
            }
            else if (cbo.DataSource is DataTable)
            {
                dt = (DataTable)cbo.DataSource;
            }
            if (dt != null)
            {
                try
                {
                    DataView dv = new DataView(dt);
                    string filter;
                    // Check for NULL Value 
                    if (Value != System.DBNull.Value)
                        filter = fieldName + " = '" + Convert.ToString(Value).Replace("'", "''") + "' or " + cbo.ValueMember + " is null";
                    else
                        filter = fieldName + " is null";

                    //Re-apply the value after the filter has been performed.
                    object val = this.Value;
                    dv.RowFilter = filter;
                    cbo.DataSource = dv;
                    this.Value = val;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Session.CurrentSession.Resources.GetMessage("ERRORSHOW", "Error", "").Text, MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            return true;
        }
        #endregion

        private void SetInitialPathForDialog(OpenFileDialog openFileDialog1)
        {
            if (openFileDialog1 != null)
            {
                string path = Convert.ToString(Session.CurrentSession.GetXmlProperty("ImageDirectory", ""));
                if (path != "" && System.IO.Directory.Exists(path))
                {
                    openFileDialog1.InitialDirectory = path;
                }
            }
        }

        private class FlatButton : Button
        {

            private bool _isHovered;

            private bool _isSelected;

            public FlatButton()
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            }

            public override Color BackColor
            {
                get
                {
                    if (_isSelected)
                    {
                        return FlatAppearance.MouseDownBackColor;
                    }

                    if (_isHovered)
                    {
                        return FlatAppearance.MouseOverBackColor;
                    }

                    return base.BackColor;
                }
                set { base.BackColor = value; }
            }

            [DefaultValue(null)]
            public string SelectedImageName { get; set; }

            [DefaultValue(null)]
            public string DisabledImageName { get; set; }

            [DefaultValue(null)]
            public string StandardImageName { get; set; }

            private Image FlatImage
            {
                get { return Images.GetCommonIcon(DeviceDpi, ImageName); }
            }

            private string ImageName
            {
                get
                {
                    if (_isSelected)
                    {
                        return SelectedImageName;
                    }

                    if (!Enabled)
                    {
                        return DisabledImageName;
                    }

                    return StandardImageName;
                }
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                _isHovered = true;
                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _isHovered = false;
                base.OnMouseLeave(e);
            }

            protected override void OnMouseUp(MouseEventArgs mevent)
            {
                _isSelected = false;
                base.OnMouseUp(mevent);
            }

            protected override void OnMouseDown(MouseEventArgs mevent)
            {
                _isSelected = true;
                base.OnMouseDown(mevent);
            }

            protected override void OnKeyDown(KeyEventArgs kevent)
            {
                _isSelected = true;
                base.OnKeyDown(kevent);
            }

            protected override void OnKeyUp(KeyEventArgs kevent)
            {
                _isSelected = false;
                base.OnKeyUp(kevent);
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                using (var brush = new SolidBrush(BackColor))
                using (var image = FlatImage)
                {
                    pevent.Graphics.FillRectangle(brush, ClientRectangle);
                    pevent.Graphics.DrawImage(image, (Width - image.Width) / 2, (Height - image.Height) / 2);
                }

                if (Focused)
                {
                    var rect = ClientRectangle;
                    rect.Inflate(-FlatAppearance.BorderSize, -FlatAppearance.BorderSize);
                    ControlPaint.DrawFocusRectangle(pevent.Graphics, rect);
                }
            }

        }

    }
}


