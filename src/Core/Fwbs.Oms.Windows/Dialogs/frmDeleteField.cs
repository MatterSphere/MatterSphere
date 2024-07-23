using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// The form that allows you to pick document variable fields etc.. that are used
    /// by the field parser.
    /// </summary>
    internal class frmDeleteField : BaseForm
	{
		#region Controls

		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlPicker;
		private System.Windows.Forms.Button btnDelete;
		private FWBS.OMS.UI.Windows.ResourceLookup _res;
		private System.Windows.Forms.ComboBox cboCode;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Fields

		private FWBS.OMS.Interfaces.IOMSApp _controlApp = null;

		#endregion

		#region Constructors 

		private frmDeleteField()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			MinimumSize = Size;
			MaximumSize = new Size(Size.Width * 2, Size.Height);
        }

		internal frmDeleteField(FWBS.OMS.Interfaces.IOMSApp controlApp) : this()
		{
			_controlApp = controlApp;
			PopulateFields();

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlPicker = new System.Windows.Forms.Panel();
            this.cboCode = new System.Windows.Forms.ComboBox();
            this._res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons.SuspendLayout();
            this.pnlPicker.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnDelete);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(403, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10);
            this.pnlButtons.Size = new System.Drawing.Size(95, 71);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(10, 33);
            this._res.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCLOSE", "&Close", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Close";
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Location = new System.Drawing.Point(10, 10);
            this._res.SetLookup(this.btnDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("DELETE", "&Delete", ""));
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlPicker
            // 
            this.pnlPicker.Controls.Add(this.cboCode);
            this.pnlPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPicker.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlPicker.Location = new System.Drawing.Point(0, 0);
            this.pnlPicker.Name = "pnlPicker";
            this.pnlPicker.Padding = new System.Windows.Forms.Padding(24);
            this.pnlPicker.Size = new System.Drawing.Size(403, 71);
            this.pnlPicker.TabIndex = 4;
            // 
            // cboCode
            // 
            this.cboCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCode.Location = new System.Drawing.Point(24, 24);
            this.cboCode.Name = "cboCode";
            this.cboCode.Size = new System.Drawing.Size(355, 23);
            this.cboCode.TabIndex = 0;
            // 
            // frmDeleteField
            // 
            this.AcceptButton = this.btnDelete;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(498, 71);
            this.ControlBox = false;
            this.Controls.Add(this.pnlPicker);
            this.Controls.Add(this.pnlButtons);
            this._res.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmDeleteField", "Delete Field From Active Document", ""));
            this.Name = "frmDeleteField";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delete Field From Active Document";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDeleteField_Closing);
            this.Load += new System.EventHandler(this.frmDeleteField_Load);
            this.pnlButtons.ResumeLayout(false);
            this.pnlPicker.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Event Methods

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            MaximumSize = Size.Empty;
            base.OnDpiChanged(e);
            MaximumSize = new Size(MinimumSize.Width * 2, MinimumSize.Height);
        }

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (_controlApp == null)
			{
				if (cboCode.Text != "")
					DialogResult = DialogResult.OK;
			}
			else
			{
				if (cboCode.Text != "")
				{
					_controlApp.DeleteField(_controlApp, SelectedField);
					PopulateFields();
				}
			}
		}

		private void frmDeleteField_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_controlApp != null) _controlApp.RunCommand(_controlApp, "SYSTEM;SHOWFIELDCODES;FALSE");
		}

		private void frmDeleteField_Load(object sender, System.EventArgs e)
		{
			if (_controlApp != null) _controlApp.RunCommand(_controlApp, "SYSTEM;SHOWFIELDCODES;TRUE");
		}

		#endregion

		#region Methods

		private void PopulateFields()
		{
			if (_controlApp != null)
			{
				int count = _controlApp.GetFieldCount(_controlApp);
				cboCode.Items.Clear();
				for (int ctr = 0; ctr < count;ctr++)
				{
					string name = _controlApp.GetFieldName(_controlApp, ctr);
					cboCode.Items.Add(name);
				}
				if (cboCode.Items.Count > 0) cboCode.SelectedIndex = 0;
			}
		}

		#endregion



		#region Properties

		/// <summary>
		/// Gets the currently selected field.
		/// </summary>
		public string SelectedField
		{
			get
			{
				return Convert.ToString(cboCode.Text);
			}
		}


		#endregion

	}
}
