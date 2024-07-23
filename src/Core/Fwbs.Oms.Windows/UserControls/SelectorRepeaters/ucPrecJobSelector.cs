using System;
using System.ComponentModel;
using System.Data;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Multi precedent selector user control.
    /// </summary>
    public class ucPrecJobSelector : ucSelectorClass
	{
		#region Controls

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.CheckBox chkAsNew;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btnAddAssoc;
		private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup SaveMode;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkEmail;
		private System.Windows.Forms.CheckBox chkFax;
		private System.Windows.Forms.CheckBox chkPrintDialog;
		private System.Windows.Forms.CheckBox chkAutoPrint;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblto;
		private System.Windows.Forms.ComboBox cbAssoc;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lblClient;
		private System.Windows.Forms.Button btnChange;
		private System.Windows.Forms.Label lblClientInfo;
		private System.Windows.Forms.Panel pnlClientInfoBk;

		#endregion

		#region Fields

		/// <summary>
		/// The precedent job used for this instance of the control.
		/// </summary>
		private PrecedentJob _precjob = null;


		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ucPrecJobSelector()
		{
			InitializeComponent();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.chkAsNew = new System.Windows.Forms.CheckBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.chkEmail = new System.Windows.Forms.CheckBox();
            this.chkFax = new System.Windows.Forms.CheckBox();
            this.chkPrintDialog = new System.Windows.Forms.CheckBox();
            this.chkAutoPrint = new System.Windows.Forms.CheckBox();
            this.lblto = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.lblClient = new System.Windows.Forms.Label();
            this.btnAddAssoc = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SaveMode = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbAssoc = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlClientInfoBk = new System.Windows.Forms.Panel();
            this.lblClientInfo = new System.Windows.Forms.Label();
            this.pnlTitle.SuspendLayout();
            this.border.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlClientInfoBk.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCloseAlert
            // 
            this.btnCloseAlert.Location = new System.Drawing.Point(544, 2);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(560, 20);
            // 
            // border
            // 
            this.border.Controls.Add(this.pnlClientInfoBk);
            this.border.Controls.Add(this.panel2);
            this.border.Controls.Add(this.panel3);
            this.border.Controls.Add(this.btnAddAssoc);
            this.border.Controls.Add(this.panel1);
            this.border.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.border.Size = new System.Drawing.Size(560, 72);
            this.border.Paint += new System.Windows.Forms.PaintEventHandler(this.border_Paint);
            // 
            // labTitle
            // 
            this.labTitle.Size = new System.Drawing.Size(542, 16);
            // 
            // chkAsNew
            // 
            this.chkAsNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAsNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAsNew.Location = new System.Drawing.Point(5, 36);
            this.resourceLookup1.SetLookup(this.chkAsNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkAsNew", "As New Template", ""));
            this.chkAsNew.Name = "chkAsNew";
            this.chkAsNew.Size = new System.Drawing.Size(121, 19);
            this.chkAsNew.TabIndex = 3;
            this.chkAsNew.Text = "As New Template";
            this.chkAsNew.CheckedChanged += new System.EventHandler(this.Control_LostFocus);
            // 
            // chkEmail
            // 
            this.chkEmail.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkEmail.Location = new System.Drawing.Point(5, 51);
            this.resourceLookup1.SetLookup(this.chkEmail, new FWBS.OMS.UI.Windows.ResourceLookupItem("EMAIL", "Email", ""));
            this.chkEmail.Name = "chkEmail";
            this.chkEmail.Size = new System.Drawing.Size(97, 17);
            this.chkEmail.TabIndex = 17;
            this.chkEmail.Text = "Email";
            this.chkEmail.CheckedChanged += new System.EventHandler(this.Control_LostFocus);
            this.chkEmail.Leave += new System.EventHandler(this.Control_LostFocus);
            // 
            // chkFax
            // 
            this.chkFax.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkFax.Location = new System.Drawing.Point(5, 34);
            this.resourceLookup1.SetLookup(this.chkFax, new FWBS.OMS.UI.Windows.ResourceLookupItem("FAX", "Fax", ""));
            this.chkFax.Name = "chkFax";
            this.chkFax.Size = new System.Drawing.Size(97, 17);
            this.chkFax.TabIndex = 16;
            this.chkFax.Text = "Fax";
            this.chkFax.CheckedChanged += new System.EventHandler(this.Control_LostFocus);
            // 
            // chkPrintDialog
            // 
            this.chkPrintDialog.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPrintDialog.Location = new System.Drawing.Point(5, 17);
            this.resourceLookup1.SetLookup(this.chkPrintDialog, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkPrintDialog", "Print Dialog", ""));
            this.chkPrintDialog.Name = "chkPrintDialog";
            this.chkPrintDialog.Size = new System.Drawing.Size(97, 17);
            this.chkPrintDialog.TabIndex = 15;
            this.chkPrintDialog.Text = "Print Dialog";
            this.chkPrintDialog.CheckedChanged += new System.EventHandler(this.chkPrintDialog_CheckedChanged);
            // 
            // chkAutoPrint
            // 
            this.chkAutoPrint.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAutoPrint.Location = new System.Drawing.Point(5, 0);
            this.resourceLookup1.SetLookup(this.chkAutoPrint, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkAutoPrint", "Auto Print", ""));
            this.chkAutoPrint.Name = "chkAutoPrint";
            this.chkAutoPrint.Size = new System.Drawing.Size(97, 17);
            this.chkAutoPrint.TabIndex = 14;
            this.chkAutoPrint.Text = "Auto Print";
            this.chkAutoPrint.CheckedChanged += new System.EventHandler(this.chkAutoPrint_CheckedChanged);
            // 
            // lblto
            // 
            this.lblto.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblto.Location = new System.Drawing.Point(0, 3);
            this.resourceLookup1.SetLookup(this.lblto, new FWBS.OMS.UI.Windows.ResourceLookupItem("TO", "To", ""));
            this.lblto.Name = "lblto";
            this.lblto.Size = new System.Drawing.Size(44, 21);
            this.lblto.TabIndex = 10;
            this.lblto.Text = "To";
            this.lblto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChange
            // 
            this.btnChange.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChange.Location = new System.Drawing.Point(264, 2);
            this.resourceLookup1.SetLookup(this.btnChange, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHANGE", "C&hange", ""));
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(54, 26);
            this.btnChange.TabIndex = 12;
            this.btnChange.Text = "C&hange";
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // lblClient
            // 
            this.lblClient.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClient.Location = new System.Drawing.Point(0, 2);
            this.resourceLookup1.SetLookup(this.lblClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("CLIENT", "%CLIENT% :", ""));
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(44, 26);
            this.lblClient.TabIndex = 10;
            this.lblClient.Text = "%CLIENT% :";
            this.lblClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddAssoc
            // 
            this.btnAddAssoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAssoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAddAssoc.Location = new System.Drawing.Point(4548, 29);
            this.btnAddAssoc.Name = "btnAddAssoc";
            this.btnAddAssoc.Size = new System.Drawing.Size(60, 23);
            this.btnAddAssoc.TabIndex = 7;
            this.btnAddAssoc.Text = "Add...";
            // 
            // SaveMode
            // 
            this.SaveMode.ActiveSearchEnabled = false;
            this.SaveMode.AddNotSet = false;
            this.SaveMode.CaptionWidth = 0;
            this.SaveMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.SaveMode.IsDirty = false;
            this.SaveMode.Location = new System.Drawing.Point(5, 3);
            this.SaveMode.Name = "SaveMode";
            this.SaveMode.NotSetCode = "NOTSET";
            this.SaveMode.NotSetType = "RESOURCE";
            this.SaveMode.Size = new System.Drawing.Size(121, 33);
            this.SaveMode.TabIndex = 9;
            this.SaveMode.TerminologyParse = false;
            this.SaveMode.Type = "PrecSaveMode";
            this.SaveMode.ActiveChanged += new System.EventHandler(this.SaveMode_Changed);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkEmail);
            this.panel1.Controls.Add(this.chkFax);
            this.panel1.Controls.Add(this.chkPrintDialog);
            this.panel1.Controls.Add(this.chkAutoPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(454, 2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(102, 68);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbAssoc);
            this.panel2.Controls.Add(this.lblto);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(136, 2);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 6);
            this.panel2.Size = new System.Drawing.Size(318, 30);
            this.panel2.TabIndex = 11;
            // 
            // cbAssoc
            // 
            this.cbAssoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbAssoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAssoc.Location = new System.Drawing.Point(44, 3);
            this.cbAssoc.Name = "cbAssoc";
            this.cbAssoc.Size = new System.Drawing.Size(274, 21);
            this.cbAssoc.TabIndex = 9;
            this.cbAssoc.SelectionChangeCommitted += new System.EventHandler(this.Control_LostFocus);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkAsNew);
            this.panel3.Controls.Add(this.SaveMode);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(4, 2);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5, 3, 6, 0);
            this.panel3.Size = new System.Drawing.Size(132, 68);
            this.panel3.TabIndex = 12;
            // 
            // pnlClientInfoBk
            // 
            this.pnlClientInfoBk.Controls.Add(this.lblClientInfo);
            this.pnlClientInfoBk.Controls.Add(this.btnChange);
            this.pnlClientInfoBk.Controls.Add(this.lblClient);
            this.pnlClientInfoBk.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlClientInfoBk.Location = new System.Drawing.Point(136, 32);
            this.pnlClientInfoBk.Name = "pnlClientInfoBk";
            this.pnlClientInfoBk.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pnlClientInfoBk.Size = new System.Drawing.Size(318, 30);
            this.pnlClientInfoBk.TabIndex = 13;
            // 
            // lblClientInfo
            // 
            this.lblClientInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClientInfo.Location = new System.Drawing.Point(44, 2);
            this.lblClientInfo.Name = "lblClientInfo";
            this.lblClientInfo.Size = new System.Drawing.Size(220, 26);
            this.lblClientInfo.TabIndex = 11;
            this.lblClientInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucPrecJobSelector
            // 
            this.Name = "ucPrecJobSelector";
            this.Size = new System.Drawing.Size(560, 100);
            this.pnlTitle.ResumeLayout(false);
            this.border.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlClientInfoBk.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region ISelectorRepeater Implementation

	
		/// <summary>
		/// Checks to see if this type of selector control supports certain methods.
		/// </summary>
		/// <param name="methodType">Method type to check for.</param>
		/// <returns>A true / false value.</returns>
		public override bool HasMethod(SelectorRepeaterMethods methodType)
		{
			return false;
		}

		/// <summary>
		/// Runs the specific type of method.
		/// </summary>
		/// <param name="methodType">>Method type to check for.</param>
		public override void RunMethod(SelectorRepeaterMethods methodType)
		{
		}

		/// <summary>
		/// Gets or sets the current contact object for this control.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public override object Object
		{
			get
			{
				return _precjob;
			}
			set
			{
				_precjob = value as PrecedentJob;
				GetData();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets all the values
		/// </summary>
		private void SetData()
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
				if (_precjob != null)
				{
					_precjob.SaveMode = (FWBS.OMS.PrecSaveMode)Convert.ToInt16(SaveMode.Value);
			
					_precjob.PrintMode = PrecPrintMode.None;

					if (chkAutoPrint.Checked && chkAutoPrint.Visible)
						_precjob.PrintMode = _precjob.PrintMode | PrecPrintMode.Print;

					if (chkPrintDialog.Checked && chkPrintDialog.Visible)
						_precjob.PrintMode = _precjob.PrintMode | PrecPrintMode.Dialog;

					if (chkFax.Checked && chkFax.Visible)
						_precjob.PrintMode = _precjob.PrintMode | PrecPrintMode.Fax;

					if (chkEmail.Checked && chkEmail.Visible)
						_precjob.PrintMode = _precjob.PrintMode | PrecPrintMode.Email;

					if (_precjob.Associate != null && _precjob.Associate.ID != Convert.ToInt64(cbAssoc.SelectedValue)) 
						_precjob.Associate = FWBS.OMS.Associate.GetAssociate(Convert.ToInt64(cbAssoc.SelectedValue));

					_precjob.AsNewTemplate = chkAsNew.Checked;
				}
			}
		}

		/// <summary>
		/// Gets all the values
		/// </summary>
		private void GetData()
		{
			// Load up the precedent information
			if (_precjob != null)
			{
				if (Session.CurrentSession.IsLoggedIn)
				{
					SaveMode.ActiveChanged -= new EventHandler(this.SaveMode_Changed);
					cbAssoc.SelectionChangeCommitted -= new EventHandler(this.Control_LostFocus);
					chkAsNew.CheckedChanged -= new EventHandler(this.Control_LostFocus);
					chkAutoPrint.CheckedChanged -= new EventHandler(this.chkAutoPrint_CheckedChanged);
					chkPrintDialog.CheckedChanged -= new EventHandler(this.chkPrintDialog_CheckedChanged);
					chkEmail.CheckedChanged -= new EventHandler(this.Control_LostFocus);
					chkFax.CheckedChanged -= new EventHandler(this.Control_LostFocus);

					this.Text = _precjob.Precedent.PrecedentType + " (" + _precjob.Precedent.Title + ") " + _precjob.Precedent.Description;
					if (_precjob.Precedent.TextOnly && _precjob.Precedent.PrecedentType != "")
					{
						toolTip1.SetToolTip(chkAsNew,_precjob.Precedent.PrecedentType);
						chkAsNew.Visible = true;
					}
					else
					{
						chkAsNew.Visible = false;
					}
					chkAsNew.Checked = _precjob.AsNewTemplate;

					SaveMode.Value = Convert.ToString((int)_precjob.SaveMode);
				
					if (_precjob.Associate != null)
					{ // Populate the Drop down Combo
						lblClientInfo.Text =  _precjob.Associate.OMSFile.ToString();
						cbAssoc.DataSource = null;
						DataView assoc = _precjob.Associate.OMSFile.Associates.GetActiveAssociates();
						cbAssoc.DataSource = assoc;
						cbAssoc.DisplayMember = assoc.Table.Columns["contname"].ColumnName;
						cbAssoc.ValueMember = assoc.Table.Columns["associd"].ColumnName;
						cbAssoc.SelectedValue = _precjob.Associate.ID;
					}

					
					//Set the print options.
					if (_precjob.PrintMode == PrecPrintMode.None)
					{
						chkAutoPrint.Checked = false;
						chkPrintDialog.Checked = false;
						chkEmail.Checked = false;
						chkFax.Checked = false;
					}
					else
					{
						chkAutoPrint.Checked = ((_precjob.PrintMode | PrecPrintMode.Print) == _precjob.PrintMode);
						chkPrintDialog.Checked = ((_precjob.PrintMode | PrecPrintMode.Dialog) == _precjob.PrintMode);
						chkEmail.Checked = ((_precjob.PrintMode | PrecPrintMode.Email) == _precjob.PrintMode);
						chkFax.Checked = ((_precjob.PrintMode | PrecPrintMode.Fax) == _precjob.PrintMode);
					}

					SaveMode.ActiveChanged += new EventHandler(this.SaveMode_Changed);
					cbAssoc.SelectionChangeCommitted += new EventHandler(this.Control_LostFocus);
					chkAsNew.CheckedChanged += new EventHandler(this.Control_LostFocus);
					chkAutoPrint.CheckedChanged += new EventHandler(this.chkAutoPrint_CheckedChanged);
					chkPrintDialog.CheckedChanged += new EventHandler(this.chkPrintDialog_CheckedChanged);
					chkEmail.CheckedChanged += new EventHandler(this.Control_LostFocus);
					chkFax.CheckedChanged += new EventHandler(this.Control_LostFocus);

				
				}
			}
		}



		private void SaveMode_Changed(object sender, System.EventArgs e)
		{
			try
			{
				PrecSaveMode mode = (FWBS.OMS.PrecSaveMode)Convert.ToInt16(SaveMode.Value);
				if ((mode | PrecSaveMode.Quick) == mode)
				{
					chkAutoPrint.Checked = true;
				}
				if ((mode| PrecSaveMode.Save) == mode)
				{
					chkPrintDialog.Checked = true;
				}
				if (mode == PrecSaveMode.None)
				{
					chkEmail.Checked = false;
				}
				SetData();
			
			}
			catch
			{
			}
		}

		private void chkAutoPrint_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkAutoPrint.Checked == false)
				chkPrintDialog.Checked = false;
			SetData();
		}

		private void chkPrintDialog_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkPrintDialog.Checked == true)
				chkAutoPrint.Checked = true;
			SetData();
		}


		private void lblprecdesc_Click(object sender, System.EventArgs e)
		{
			this.OnClick(e);
		}

		private void btnChange_Click(object sender, System.EventArgs e)
		{
			OMSFile of = FWBS.OMS.UI.Windows.Services.SelectFile();
			if (of != null)
			{
				Associate assoc =  of.GetBestFitAssociate(_precjob.Precedent.ContactType, _precjob.Precedent.AssocType);
                if (assoc == null)
                    return;

                _precjob.Associate = assoc;
				GetData();
			}

		}

		private void Control_LostFocus(object sender, System.EventArgs e)
		{
			if (sender == chkEmail)
			{
				if (chkEmail.Checked == true)
				{
					PrecSaveMode mode = (FWBS.OMS.PrecSaveMode)Convert.ToInt16(SaveMode.Value);
					if (mode == PrecSaveMode.None)
						SaveMode.Value = Convert.ToInt32(PrecSaveMode.Quick);


				}
			}
			SetData();
		}


		#endregion

		private void border_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}


	}


}
