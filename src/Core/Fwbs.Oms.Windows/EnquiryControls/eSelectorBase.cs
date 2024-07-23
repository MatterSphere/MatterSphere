using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A base control for a one to many data relationship with add, remove and find facilities.
    /// </summary>
    public class eSelectorBase: UserControl, IBasicEnquiryControl2
	{

		#region Events

		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler Changed;

		[Category("Action")]
		public event EventHandler ActiveChanged;
		#endregion

		#region Fields

		/// <summary>
		/// A variable that tells the control that it is in design mode.
		/// </summary>
		private bool _omsdesignmode = false;

		/// <summary>
		/// Marks the editing control as required.
		/// </summary>
		private bool _required = false;

		/// <summary>
		/// Holds a value that indicates whether the control is currently read only.
		/// </summary>
		private bool _readOnly = false;

		private bool _isdirty = false;

        /// <summary>
        /// Displays caption on top.
        /// </summary>
        private bool _captionTop = false;

        /// <summary>
        /// Displays icons for actions instead of labels.
        /// </summary>
        private bool _iconButtons = false;

        protected System.Windows.Forms.ComboBox cboInfoSelector;
		#endregion

		#region Controls Specific Fields

		private System.Windows.Forms.LinkLabel lnkAdd;
		private System.Windows.Forms.LinkLabel lnkEdit;
		private System.Windows.Forms.LinkLabel lnkRemove;
		private System.Windows.Forms.LinkLabel lnkFind;
		protected System.Windows.Forms.Panel pnlContainer;
		private System.Windows.Forms.Label lblCaption;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel pnlSpace;
        private Panel pnlIconButtons;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnRemove;
        private Button btnFind;
        private Panel pnlLinkLabels;
        private ToolTip toolTip;
        private FWBS.OMS.UI.Windows.ResourceLookup res;

		#endregion
		
		#region Constructors


		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the database to display in the countries combo box.
		/// </summary>
		public eSelectorBase() : base()
		{
			InitializeComponent();
			TabIndex = 0;
			Width = 300;
            SetIconButtonsToolTip();
        }

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.cboInfoSelector = new System.Windows.Forms.ComboBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lnkAdd = new System.Windows.Forms.LinkLabel();
            this.lnkEdit = new System.Windows.Forms.LinkLabel();
            this.lnkRemove = new System.Windows.Forms.LinkLabel();
            this.lnkFind = new System.Windows.Forms.LinkLabel();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.pnlIconButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.pnlLinkLabels = new System.Windows.Forms.Panel();
            this.res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlContainer.SuspendLayout();
            this.pnlIconButtons.SuspendLayout();
            this.pnlLinkLabels.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboInfoSelector
            // 
            this.cboInfoSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboInfoSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInfoSelector.Location = new System.Drawing.Point(0, 0);
            this.cboInfoSelector.Margin = new System.Windows.Forms.Padding(0);
            this.cboInfoSelector.Name = "cboInfoSelector";
            this.cboInfoSelector.Size = new System.Drawing.Size(0, 21);
            this.cboInfoSelector.TabIndex = 0;
            this.cboInfoSelector.SelectedIndexChanged += new System.EventHandler(this.cboInfoSelector_SelectedIndexChanged);
            this.cboInfoSelector.SelectionChangeCommitted += new System.EventHandler(this.Info_Changed);
            this.cboInfoSelector.DataSourceChanged += new System.EventHandler(this.cboInfoSelector_DataSourceChanged);
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(150, 25);
            this.lblCaption.TabIndex = 0;
            // 
            // lnkAdd
            // 
            this.lnkAdd.AutoSize = true;
            this.lnkAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lnkAdd.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lnkAdd.Location = new System.Drawing.Point(0, 0);
            this.res.SetLookup(this.lnkAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("lnkAdd", "Add", ""));
            this.lnkAdd.Name = "lnkAdd";
            this.lnkAdd.Size = new System.Drawing.Size(26, 13);
            this.lnkAdd.TabIndex = 1;
            this.lnkAdd.TabStop = true;
            this.lnkAdd.Text = "Add";
            this.lnkAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkAdd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Add);
            this.lnkAdd.EnabledChanged += new System.EventHandler(this.lbl_EnabledChanged);
            // 
            // lnkEdit
            // 
            this.lnkEdit.AutoSize = true;
            this.lnkEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lnkEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lnkEdit.Location = new System.Drawing.Point(26, 0);
            this.res.SetLookup(this.lnkEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("lnkEdit", "Edit", ""));
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.Size = new System.Drawing.Size(25, 13);
            this.lnkEdit.TabIndex = 2;
            this.lnkEdit.TabStop = true;
            this.lnkEdit.Text = "Edit";
            this.lnkEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Edit);
            this.lnkEdit.EnabledChanged += new System.EventHandler(this.lbl_EnabledChanged);
            // 
            // lnkRemove
            // 
            this.lnkRemove.AutoSize = true;
            this.lnkRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lnkRemove.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lnkRemove.Location = new System.Drawing.Point(51, 0);
            this.res.SetLookup(this.lnkRemove, new FWBS.OMS.UI.Windows.ResourceLookupItem("lnkRemove", "Del", ""));
            this.lnkRemove.Name = "lnkRemove";
            this.lnkRemove.Size = new System.Drawing.Size(23, 13);
            this.lnkRemove.TabIndex = 3;
            this.lnkRemove.TabStop = true;
            this.lnkRemove.Text = "Del";
            this.lnkRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkRemove.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Remove);
            this.lnkRemove.EnabledChanged += new System.EventHandler(this.lbl_EnabledChanged);
            // 
            // lnkFind
            // 
            this.lnkFind.AutoSize = true;
            this.lnkFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lnkFind.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lnkFind.Location = new System.Drawing.Point(74, 0);
            this.res.SetLookup(this.lnkFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("lnkFind", "Find", ""));
            this.lnkFind.Name = "lnkFind";
            this.lnkFind.Size = new System.Drawing.Size(27, 13);
            this.lnkFind.TabIndex = 4;
            this.lnkFind.TabStop = true;
            this.lnkFind.Text = "Find";
            this.lnkFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkFind.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Find);
            this.lnkFind.EnabledChanged += new System.EventHandler(this.lbl_EnabledChanged);
            // 
            // pnlContainer
            // 
            this.pnlContainer.AutoSize = true;
            this.pnlContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlContainer.Controls.Add(this.cboInfoSelector);
            this.pnlContainer.Controls.Add(this.pnlSpace);
            this.pnlContainer.Controls.Add(this.pnlIconButtons);
            this.pnlContainer.Controls.Add(this.pnlLinkLabels);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContainer.Location = new System.Drawing.Point(150, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(156, 23);
            this.pnlContainer.TabIndex = 0;
            // 
            // pnlSpace
            // 
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpace.Location = new System.Drawing.Point(0, 21);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(0, 2);
            this.pnlSpace.TabIndex = 5;
            // 
            // pnlIconButtons
            // 
            this.pnlIconButtons.AutoSize = true;
            this.pnlIconButtons.Controls.Add(this.btnAdd);
            this.pnlIconButtons.Controls.Add(this.btnEdit);
            this.pnlIconButtons.Controls.Add(this.btnRemove);
            this.pnlIconButtons.Controls.Add(this.btnFind);
            this.pnlIconButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlIconButtons.Location = new System.Drawing.Point(-89, 0);
            this.pnlIconButtons.Name = "pnlIconButtons";
            this.pnlIconButtons.Size = new System.Drawing.Size(144, 23);
            this.pnlIconButtons.TabIndex = 5;
            this.pnlIconButtons.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(36, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Click += new System.EventHandler(this.Add);
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Location = new System.Drawing.Point(36, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(36, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Click += new System.EventHandler(this.Edit);
            // 
            // btnRemove
            // 
            this.btnRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRemove.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Location = new System.Drawing.Point(72, 0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(36, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Click += new System.EventHandler(this.Remove);
            // 
            // btnFind
            // 
            this.btnFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFind.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnFind.FlatAppearance.BorderSize = 0;
            this.btnFind.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Location = new System.Drawing.Point(108, 0);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(36, 23);
            this.btnFind.TabIndex = 4;
            this.btnFind.Click += new System.EventHandler(this.Find);
            // 
            // pnlLinkLabels
            // 
            this.pnlLinkLabels.AutoSize = true;
            this.pnlLinkLabels.Controls.Add(this.lnkAdd);
            this.pnlLinkLabels.Controls.Add(this.lnkEdit);
            this.pnlLinkLabels.Controls.Add(this.lnkRemove);
            this.pnlLinkLabels.Controls.Add(this.lnkFind);
            this.pnlLinkLabels.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlLinkLabels.Location = new System.Drawing.Point(55, 0);
            this.pnlLinkLabels.Name = "pnlLinkLabels";
            this.pnlLinkLabels.Size = new System.Drawing.Size(101, 23);
            this.pnlLinkLabels.TabIndex = 5;
            // 
            // eSelectorBase
            // 
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.lblCaption);
            this.Name = "eSelectorBase";
            this.Size = new System.Drawing.Size(306, 25);
            this.VisibleChanged += new System.EventHandler(this.eSelectorBase_VisibleChanged);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.pnlIconButtons.ResumeLayout(false);
            this.pnlLinkLabels.ResumeLayout(false);
            this.pnlLinkLabels.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void eSelectorBase_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Value is IConvertible)
            {
                EnquiryForm parent = this.Parent as EnquiryForm;
                if (parent != null)
                {
                    if (parent.Style == EnquiryStyle.Wizard)
                    {
                        if (Combo.SelectedValue != this.Value)
                            Combo.SelectedValue = this.Value;
                        OnActiveChanged();
                        OnChanged();
                    }
                }
            }
        }

		#endregion

		#region IBasicEnquiryControl2 Implementation

		/// <summary>
		/// Gets or Sets the desgin mode property of the control.
		/// </summary>
		[Browsable(false)]
		public bool omsDesignMode
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

		/// <summary>
		/// Gets the enquiry controls internal control.
		/// </summary>
		[Browsable(false)]
		public object Control
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Gets whether the current control can be stretched by its Y co-ordinate.
		/// This is a design mode property and is set to true.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		public bool LockHeight 
		{
			get
			{
				return false;
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
		/// Gets or Sets the editable format of the control.  By default the whole control toggles it's enable property.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool ReadOnly 
		{
			get
			{
				return _readOnly;
			}
			set
			{
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is FWBS.Common.UI.IBasicEnquiryControl2)
						((FWBS.Common.UI.IBasicEnquiryControl2)ctrl).ReadOnly = value;
					else
						ctrl.Enabled = !value;
				}
				_readOnly = value;
			}
		}


		/// <summary>
		/// Gets or Sets the caption width of a control, leaving the rest of the width of the control
		/// to be the width of the internal editing control.
		/// </summary>
		[DefaultValue(150)]
        [Browsable(false)]
		public virtual int CaptionWidth
		{
			get
			{
				return lblCaption.Width;
			}
			set
			{
				lblCaption.Width = value;
			}
		}

        /// <summary>
        /// Gets or Set a bool for the Caption location - on the top or not.
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool CaptionTop
        {
            get
            {
                return _captionTop;
            }
            set
            {
                _captionTop = value;
                lblCaption.Dock = _captionTop ? DockStyle.Top : DockStyle.Left;
                ValidateCtrlSize();
                lblCaption.Height = CalcCaptionHeight(true);
                lblCaption.Width = Common.UI.Windows.eBase2.DefaultCaptionWidth;
            }
        }

        protected virtual int PreferredHeight()
        {
            var captionTopHeight = _captionTop ? CalcCaptionHeight(false) : 0;
            return cboInfoSelector.PreferredHeight + pnlSpace.Height + captionTopHeight;
        }

        protected void ValidateCtrlSize()
        {
            if (omsDesignMode)
            {
                Height = PreferredHeight();
            }
        }

        private int CalcCaptionHeight(bool isDeviceDpiRequired)
        {
            using (var graphics = lblCaption.CreateGraphics())
            {
                return isDeviceDpiRequired
                    ? Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height * 96 / (omsDesignMode || Font == DefaultFont ? 96 : DeviceDpi)))
                    : Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height));
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            SetIcons();
            if (_captionTop)
            {
                lblCaption.Height = CalcCaptionHeight(false);
            }
        }

        protected override void OnFontChanged(EventArgs eventArgs)
        {
            base.OnFontChanged(eventArgs);
            if (CaptionTop)
            {
                lblCaption.Height = CalcCaptionHeight(true);
            }
            ValidateCtrlSize();
        }

		/// <summary>
		/// Gets or Sets the controls value.  This must be overriden by derived classes to make their
		/// own representation of the value using the internal editing control..
		/// </summary>
		[Browsable(false)]
		public virtual object Value
		{
			get
			{
				return null;
			}
			set
			{
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
		/// Gets or Sets the controls value.  This must be overriden by derived classes to make their
		/// own representation of the value using the internal editing control..
		/// </summary>
		[Browsable(true)]
		[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public override string Text
		{
			get
			{
				return lblCaption.Text;
			}
			set
			{
				lblCaption.Text = value;
			}
		}

		#endregion

		#region Event Methods

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public virtual void OnChanged()
		{
			if (Changed!= null && this.IsDirty)
				Changed(this, EventArgs.Empty);
		}

		public virtual void OnActiveChanged()
		{
			this.IsDirty=true;
			if (ActiveChanged!= null)
				ActiveChanged(this, EventArgs.Empty);
		}
		#endregion
		
		#region Captured Events

		/// <summary>
		/// Captures the combo box change event.
		/// </summary>
		/// <param name="sender">The address selector combo box control.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void Info_Changed(object sender, System.EventArgs e)
		{
            FWBS.OMS.UI.Windows.MessageBox.Show("Changed ...");
        }

		/// <summary>
		/// Adds an item.
		/// </summary>
		/// <param name="sender">The add link.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void Add(object sender, LinkLabelLinkClickedEventArgs e)
		{
            FWBS.OMS.UI.Windows.MessageBox.Show("Add ...");
        }

		/// <summary>
		/// Adds an item.
		/// </summary>
		/// <param name="sender">The add link.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void Edit(object sender, LinkLabelLinkClickedEventArgs e)
		{
            FWBS.OMS.UI.Windows.MessageBox.Show("Edit ...");
        }

		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <param name="sender">The remove link.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void RemoveConfirm(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Remove(sender,e);
		}
		
		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <param name="sender">The remove link.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void Remove(object sender, LinkLabelLinkClickedEventArgs e)
		{
            FWBS.OMS.UI.Windows.MessageBox.Show("Remove ...");
        }

		/// <summary>
		/// Finds an item.
		/// </summary>
		/// <param name="sender">The find link.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void Find(object sender, LinkLabelLinkClickedEventArgs e)
		{
            FWBS.OMS.UI.Windows.MessageBox.Show("Find ...");
        }

        /// <summary>
        /// Synchronizes button's enable state with label enable state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_EnabledChanged(object sender, EventArgs e)
        {
            var lnkLabel = sender as LinkLabel;
            var isEnabled = lnkLabel.Enabled;
            switch (lnkLabel.Name)
            {
                case "lnkAdd":
                    btnAdd.Enabled = isEnabled;
                    break;
                case "lnkEdit":
                    btnEdit.Enabled = isEnabled;
                    break;
                case "lnkRemove":
                    btnRemove.Enabled = isEnabled;
                    break;
                case "lnkFind":
                    btnFind.Enabled = isEnabled;
                    break;
                default:
                    break;
            }
        }

        #endregion

		private void cboInfoSelector_DataSourceChanged(object sender, System.EventArgs e)
		{
			UIUpdate();
		}
		
		protected void UIUpdate()
		{
            if (Combo.DataSource == null)
            {
                lnkEdit.Enabled = false;
                lnkRemove.Enabled = false;
                cboInfoSelector.Focus();
            }
            else if (Combo.DataSource is DataView)
            {
                lnkEdit.Enabled = (((DataView)Combo.DataSource).Count != 0) && Combo.SelectedItem != null && EditLinkEnabled;
                lnkRemove.Enabled = (((DataView)Combo.DataSource).Count != 0) && Combo.SelectedItem != null;
            }
            else if (Combo.DataSource is DataTable)
            {
                lnkEdit.Enabled = (((DataTable)Combo.DataSource).Rows.Count != 0) && Combo.SelectedItem != null && EditLinkEnabled;
                lnkRemove.Enabled = (((DataTable)Combo.DataSource).Rows.Count != 0) && Combo.SelectedItem != null;
            }   
		}
		


		#region Properties

		/// <summary>
		/// Gets a reference to the combo box.
		/// </summary>
		internal ComboBox Combo
		{
			get
			{
				return cboInfoSelector;
			}
		}

		/// <summary>
		/// Gets or Sets the Add link button visibility attribute.
		/// </summary>
		[DefaultValue(true)]
		public bool AddLinkVisible
		{
			get
			{
                return _iconButtons ? btnAdd.Visible : lnkAdd.Visible;
			}
			set
			{
				lnkAdd.Visible = value;
                btnAdd.Visible = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Remove link button visibility attribute.
		/// </summary>
		[DefaultValue(true)]
		public bool DeleteLinkVisible
		{
			get
			{
				return _iconButtons ? btnRemove.Visible : lnkRemove.Visible;
			}
			set
			{
				lnkRemove.Visible = value;
                btnRemove.Visible = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Find link button visibility attribute.
		/// </summary>
		[DefaultValue(true)]
		public bool FindLinkVisible
		{
			get
			{
				return _iconButtons ? btnFind.Visible : lnkFind.Visible;
			}
			set
			{
				lnkFind.Visible = value;
                btnFind.Visible = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Edit link button visibility attribute.
		/// </summary>
		[DefaultValue(true)]
		public bool EditLinkVisible
		{
			get
			{
				return _iconButtons ? btnEdit.Visible : lnkEdit.Visible;
			}
			set
			{
				lnkEdit.Visible = value;
                btnEdit.Visible = value;
			}
		}

        private bool _editlinkenabled = true;

        /// <summary>
        /// Gets or Sets the Edit link button visibility attribute.
        /// </summary>
        [DefaultValue(true)]
        public bool EditLinkEnabled
        {
            get
            {
                return _editlinkenabled;
            }
            set
            {
                _editlinkenabled = value;
                lnkEdit.Enabled = value;
            }
        }

		#endregion

        private void cboInfoSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UIUpdate();
        }

        #region Configurable UI Customizations

        /// <summary>
        /// Gets or Set a bool for icon buttons instead of linklabel actions.
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool IconButtons
        {
            get
            {
                return _iconButtons;
            }
            set
            {
                _iconButtons = value;
                if (_iconButtons)
                {
                    SetIcons();
                }
                pnlLinkLabels.Visible = !_iconButtons;
                pnlIconButtons.Visible = _iconButtons;
            }
        }

        private void Find(object sender, EventArgs e)
        {
            Find(this, new LinkLabelLinkClickedEventArgs(GetLink(lnkFind)));
        }

        private void Remove(object sender, EventArgs e)
        {
            Remove(this, new LinkLabelLinkClickedEventArgs(GetLink(lnkRemove)));
        }

        private void Edit(object sender, EventArgs e)
        {
            Edit(this, new LinkLabelLinkClickedEventArgs(GetLink(lnkEdit)));
        }

        private void Add(object sender, EventArgs e)
        {
            Add(this, new LinkLabelLinkClickedEventArgs(GetLink(lnkAdd)));
        }

        private LinkLabel.Link GetLink(LinkLabel label)
        {
            return label.Links.Count > 0 ? label.Links[0] : new LinkLabel.Link();
        }

        private void SetIconButtonsToolTip()
        {
            Res res = Session.CurrentSession.Resources;
            if (res != null)
            {
                toolTip.SetToolTip(btnAdd, res.GetResource("LNKADD", "Add", "").Text);
                toolTip.SetToolTip(btnEdit, res.GetResource("LNDEDIT", "Edit", "").Text);
                toolTip.SetToolTip(btnRemove, res.GetResource("DELETE", "Delete", "").Text);
                toolTip.SetToolTip(btnFind, res.GetResource("LNKFIND", "Find", "").Text);
            }
        }

        private void SetIcons()
        {
            btnAdd.Image = Images.GetCommonIcon(DeviceDpi, "add");
            btnEdit.Image = Images.GetCommonIcon(DeviceDpi, "edit");
            btnRemove.Image = Images.GetCommonIcon(DeviceDpi, "delete");
            btnFind.Image = Images.GetCommonIcon(DeviceDpi, "grey_magnifier");
        }

        #endregion
    }
}
