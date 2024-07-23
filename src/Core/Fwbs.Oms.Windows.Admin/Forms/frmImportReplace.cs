using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmImportReplace.
    /// </summary>
    public class frmImportReplace : FWBS.OMS.UI.Windows.BaseForm
	{
		#region Fields
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnOK;
		private FWBS.OMS.UI.Windows.DataGridEx dgReplace;
		private System.Windows.Forms.DataGridTableStyle gdsFieldReplace;
		private FWBS.Common.UI.Windows.DataGridLabelColumn dgcFieldDesc;
		private System.Windows.Forms.Panel panel4;
		private FWBS.Common.UI.Windows.eCaptionLine eCaptionLine2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label labCategory;
		private FWBS.OMS.Design.Export.FieldReplacer _fieldreplacer = null;
		private System.Windows.Forms.ComboBox cmbCategory;
		private DataView filtered = null;
		private System.Windows.Forms.Label labhelp;
		private System.Windows.Forms.ComboBox cmbChangeToo;
		private FWBS.Common.UI.Windows.eXPFrame eXPFrame1;
		private System.Windows.Forms.RadioButton rdoEmpty;
		private System.Windows.Forms.RadioButton rdoImport;
		private System.Windows.Forms.RadioButton rdoOverride;
		private System.Windows.Forms.DataGridTextBoxColumn dgcFieldValue;
		private System.Windows.Forms.DataGridTextBoxColumn dgcFieldValueDesc;
		private System.Windows.Forms.Panel panel5;
		private FWBS.Common.UI.Windows.eXPFrame expNullFilter;
		private System.Windows.Forms.RadioButton rdoOn;
        private System.Windows.Forms.RadioButton rdoOff;
		#endregion
        protected ResourceLookup resourceLookup1;
        private IContainer components;

		#region Constructors
		public frmImportReplace()
		{
			//
			// Required for Windows Form Designer support
			//
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
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportReplace));
            this.panel1 = new System.Windows.Forms.Panel();
            this.expNullFilter = new FWBS.Common.UI.Windows.eXPFrame();
            this.rdoOff = new System.Windows.Forms.RadioButton();
            this.rdoOn = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.dgReplace = new FWBS.OMS.UI.Windows.DataGridEx();
            this.gdsFieldReplace = new System.Windows.Forms.DataGridTableStyle();
            this.dgcFieldDesc = new FWBS.Common.UI.Windows.DataGridLabelColumn();
            this.dgcFieldValue = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcFieldValueDesc = new System.Windows.Forms.DataGridTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.eXPFrame1 = new FWBS.Common.UI.Windows.eXPFrame();
            this.rdoOverride = new System.Windows.Forms.RadioButton();
            this.rdoImport = new System.Windows.Forms.RadioButton();
            this.rdoEmpty = new System.Windows.Forms.RadioButton();
            this.cmbChangeToo = new System.Windows.Forms.ComboBox();
            this.labhelp = new System.Windows.Forms.Label();
            this.eCaptionLine2 = new FWBS.Common.UI.Windows.eCaptionLine();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.labCategory = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            this.expNullFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgReplace)).BeginInit();
            this.panel4.SuspendLayout();
            this.eXPFrame1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.expNullFilter);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(473, 7);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.panel1.Size = new System.Drawing.Size(99, 386);
            this.panel1.TabIndex = 3;
            // 
            // expNullFilter
            // 
            this.expNullFilter.Controls.Add(this.rdoOff);
            this.expNullFilter.Controls.Add(this.rdoOn);
            this.expNullFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.expNullFilter.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.expNullFilter.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.expNullFilter.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.expNullFilter.Location = new System.Drawing.Point(11, 70);
            this.resourceLookup1.SetLookup(this.expNullFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("EmptyValue", "Empty Value", ""));
            this.expNullFilter.Name = "expNullFilter";
            this.expNullFilter.Padding = new System.Windows.Forms.Padding(9, 16, 4, 4);
            this.expNullFilter.Size = new System.Drawing.Size(84, 60);
            this.expNullFilter.TabIndex = 4;
            this.expNullFilter.Text = "Empty Value";
            // 
            // rdoOff
            // 
            this.rdoOff.Checked = true;
            this.rdoOff.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoOff.Location = new System.Drawing.Point(9, 34);
            this.resourceLookup1.SetLookup(this.rdoOff, new FWBS.OMS.UI.Windows.ResourceLookupItem("OFF", "Off", ""));
            this.rdoOff.Name = "rdoOff";
            this.rdoOff.Size = new System.Drawing.Size(71, 18);
            this.rdoOff.TabIndex = 1;
            this.rdoOff.TabStop = true;
            this.rdoOff.Text = "Off";
            // 
            // rdoOn
            // 
            this.rdoOn.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoOn.Location = new System.Drawing.Point(9, 16);
            this.resourceLookup1.SetLookup(this.rdoOn, new FWBS.OMS.UI.Windows.ResourceLookupItem("ON", "On", ""));
            this.rdoOn.Name = "rdoOn";
            this.rdoOn.Size = new System.Drawing.Size(71, 18);
            this.rdoOn.TabIndex = 0;
            this.rdoOn.Text = "On";
            this.rdoOn.CheckedChanged += new System.EventHandler(this.NullFilter_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(11, 57);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(84, 13);
            this.panel5.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(11, 32);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(11, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(84, 7);
            this.panel2.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(11, 0);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            // 
            // dgReplace
            // 
            this.dgReplace.BackColor = System.Drawing.Color.White;
            this.dgReplace.BackgroundColor = System.Drawing.Color.White;
            this.dgReplace.CaptionVisible = false;
            this.dgReplace.DataMember = "";
            this.dgReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgReplace.GridLineColor = System.Drawing.Color.Silver;
            this.dgReplace.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgReplace.Location = new System.Drawing.Point(7, 35);
            this.dgReplace.Name = "dgReplace";
            this.dgReplace.ReadOnly = true;
            this.dgReplace.RowHeaderWidth = 16;
            this.dgReplace.Size = new System.Drawing.Size(466, 212);
            this.dgReplace.TabIndex = 1;
            this.dgReplace.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.gdsFieldReplace});
            this.dgReplace.CurrentCellChanged += new System.EventHandler(this.dgReplace_CurrentCellChanged);
            // 
            // gdsFieldReplace
            // 
            this.gdsFieldReplace.DataGrid = this.dgReplace;
            this.gdsFieldReplace.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dgcFieldDesc,
            this.dgcFieldValue,
            this.dgcFieldValueDesc});
            this.gdsFieldReplace.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gdsFieldReplace.MappingName = "FIELDREPLACER";
            this.gdsFieldReplace.RowHeaderWidth = 16;
            // 
            // dgcFieldDesc
            // 
            this.dgcFieldDesc.AllowMultiSelect = false;
            this.dgcFieldDesc.DisplayDateAs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dgcFieldDesc.Format = "";
            this.dgcFieldDesc.FormatInfo = null;
            this.dgcFieldDesc.HeaderText = "Description";
            this.dgcFieldDesc.ImageColumn = "";
            this.dgcFieldDesc.ImageIndex = -1;
            this.dgcFieldDesc.ImageList = null;
            this.dgcFieldDesc.MappingName = "FieldDesc";
            this.dgcFieldDesc.ReadOnly = true;
            this.dgcFieldDesc.SearchList = null;
            this.dgcFieldDesc.SourceDateIs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dgcFieldDesc.Width = 200;
            // 
            // dgcFieldValue
            // 
            this.dgcFieldValue.Format = "";
            this.dgcFieldValue.FormatInfo = null;
            this.dgcFieldValue.HeaderText = "Value";
            this.dgcFieldValue.MappingName = "FieldValue";
            this.dgcFieldValue.Width = 110;
            // 
            // dgcFieldValueDesc
            // 
            this.dgcFieldValueDesc.Format = "";
            this.dgcFieldValueDesc.FormatInfo = null;
            this.dgcFieldValueDesc.HeaderText = "Value Lookup";
            this.dgcFieldValueDesc.MappingName = "FieldValueDesc";
            this.dgcFieldValueDesc.Width = 135;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.eXPFrame1);
            this.panel4.Controls.Add(this.labhelp);
            this.panel4.Controls.Add(this.eCaptionLine2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(7, 247);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(466, 146);
            this.panel4.TabIndex = 3;
            // 
            // eXPFrame1
            // 
            this.eXPFrame1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eXPFrame1.Controls.Add(this.rdoOverride);
            this.eXPFrame1.Controls.Add(this.rdoImport);
            this.eXPFrame1.Controls.Add(this.rdoEmpty);
            this.eXPFrame1.Controls.Add(this.cmbChangeToo);
            this.eXPFrame1.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eXPFrame1.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.eXPFrame1.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eXPFrame1.Location = new System.Drawing.Point(9, 77);
            this.resourceLookup1.SetLookup(this.eXPFrame1, new FWBS.OMS.UI.Windows.ResourceLookupItem("ChangeTo", "Change To", ""));
            this.eXPFrame1.Name = "eXPFrame1";
            this.eXPFrame1.Size = new System.Drawing.Size(449, 60);
            this.eXPFrame1.TabIndex = 2;
            this.eXPFrame1.Text = "Change To";
            // 
            // rdoOverride
            // 
            this.rdoOverride.Location = new System.Drawing.Point(189, 26);
            this.resourceLookup1.SetLookup(this.rdoOverride, new FWBS.OMS.UI.Windows.ResourceLookupItem("ThisValue", "This Value", ""));
            this.rdoOverride.Name = "rdoOverride";
            this.rdoOverride.Size = new System.Drawing.Size(82, 25);
            this.rdoOverride.TabIndex = 2;
            this.rdoOverride.Text = "This Value";
            this.rdoOverride.CheckedChanged += new System.EventHandler(this.rdoOverride_CheckedChanged);
            // 
            // rdoImport
            // 
            this.rdoImport.Checked = true;
            this.rdoImport.Location = new System.Drawing.Point(15, 26);
            this.resourceLookup1.SetLookup(this.rdoImport, new FWBS.OMS.UI.Windows.ResourceLookupItem("ImportValue", "Import Value", ""));
            this.rdoImport.Name = "rdoImport";
            this.rdoImport.Size = new System.Drawing.Size(93, 25);
            this.rdoImport.TabIndex = 0;
            this.rdoImport.TabStop = true;
            this.rdoImport.Text = "Import Value";
            this.rdoImport.CheckedChanged += new System.EventHandler(this.rdoOverride_CheckedChanged);
            // 
            // rdoEmpty
            // 
            this.rdoEmpty.Location = new System.Drawing.Point(115, 26);
            this.resourceLookup1.SetLookup(this.rdoEmpty, new FWBS.OMS.UI.Windows.ResourceLookupItem("Empty", "Empty", ""));
            this.rdoEmpty.Name = "rdoEmpty";
            this.rdoEmpty.Size = new System.Drawing.Size(66, 25);
            this.rdoEmpty.TabIndex = 1;
            this.rdoEmpty.Text = "Empty";
            this.rdoEmpty.CheckedChanged += new System.EventHandler(this.rdoOverride_CheckedChanged);
            // 
            // cmbChangeToo
            // 
            this.cmbChangeToo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbChangeToo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChangeToo.Enabled = false;
            this.cmbChangeToo.ImeMode = System.Windows.Forms.ImeMode.On;
            this.cmbChangeToo.Location = new System.Drawing.Point(279, 26);
            this.cmbChangeToo.Name = "cmbChangeToo";
            this.cmbChangeToo.Size = new System.Drawing.Size(163, 23);
            this.cmbChangeToo.TabIndex = 5;
            this.cmbChangeToo.SelectionChangeCommitted += new System.EventHandler(this.cmbChangeToo_SelectionChangeCommitted);
            this.cmbChangeToo.TextChanged += new System.EventHandler(this.cmbChangeToo_TextChanged);
            // 
            // labhelp
            // 
            this.labhelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labhelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labhelp.Location = new System.Drawing.Point(9, 26);
            this.labhelp.Name = "labhelp";
            this.labhelp.Size = new System.Drawing.Size(449, 51);
            this.labhelp.TabIndex = 8;
            this.labhelp.Text = "Source Help";
            // 
            // eCaptionLine2
            // 
            this.eCaptionLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eCaptionLine2.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine2.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine2.Location = new System.Drawing.Point(9, 9);
            this.resourceLookup1.SetLookup(this.eCaptionLine2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Help", "Help", ""));
            this.eCaptionLine2.Name = "eCaptionLine2";
            this.eCaptionLine2.Size = new System.Drawing.Size(449, 18);
            this.eCaptionLine2.TabIndex = 9999;
            this.eCaptionLine2.Text = "Help";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbCategory);
            this.panel3.Controls.Add(this.labCategory);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(7, 7);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 4, 5);
            this.panel3.Size = new System.Drawing.Size(466, 28);
            this.panel3.TabIndex = 13;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.ImeMode = System.Windows.Forms.ImeMode.On;
            this.cmbCategory.Location = new System.Drawing.Point(57, 0);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(405, 23);
            this.cmbCategory.TabIndex = 0;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // labCategory
            // 
            this.labCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.labCategory.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.labCategory, new FWBS.OMS.UI.Windows.ResourceLookupItem("Category:", "Category : ", ""));
            this.labCategory.Name = "labCategory";
            this.labCategory.Size = new System.Drawing.Size(57, 23);
            this.labCategory.TabIndex = 15;
            this.labCategory.Text = "Category : ";
            this.labCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmImportReplace
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(579, 400);
            this.Controls.Add(this.dgReplace);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportReplace";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Field Replacer";
            this.Load += new System.EventHandler(this.frmImportReplace_Load);
            this.panel1.ResumeLayout(false);
            this.expNullFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgReplace)).EndInit();
            this.panel4.ResumeLayout(false);
            this.eXPFrame1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void cmbCategory_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			filtered = new DataView(FieldReplacer.Source);
			NullFilter_CheckedChanged(sender,e);
			filtered.Sort = "FieldDesc";
			dgReplace.DataSource = filtered;
			dgReplace_CurrentCellChanged(sender,e);
			
		}

		private void dgReplace_CurrentCellChanged(object sender, System.EventArgs e)
		{
			eXPFrame1.Tag = "OFFLINE";
			DataRowView dr = filtered[dgReplace.CurrentRowIndex];
			FieldReplacer.Goto(dr);
			labhelp.Text = FieldReplacer.FieldHelp;

			if (FieldReplacer.Select != "")
			{
				DataTable _source = FieldReplacer.DataSource;
				cmbChangeToo.DropDownStyle = ComboBoxStyle.DropDownList;
				cmbChangeToo.BeginUpdate();
				cmbChangeToo.DataSource = _source.Copy();
				if (FieldReplacer.DataSource.Columns.Count > 1)
				{
					cmbChangeToo.ValueMember = _source.Columns[0].ColumnName;
					cmbChangeToo.DisplayMember = _source.Columns[1].ColumnName;
				}
				else
				{
					cmbChangeToo.ValueMember = _source.Columns[0].ColumnName;
					cmbChangeToo.DisplayMember = _source.Columns[0].ColumnName;
				}
				try
				{
					cmbChangeToo.SelectedValue = FieldReplacer.FieldValue;
				}
				catch
				{
				}
				cmbChangeToo.EndUpdate();

			}
			else
			{
				cmbChangeToo.DropDownStyle = ComboBoxStyle.DropDown;
				cmbChangeToo.DataSource = null;
				cmbChangeToo.Items.Clear();
			}


			rdoEmpty.Enabled = FieldReplacer.AllowNull;


			switch(FieldReplacer.ChangeType)
			{
				case 0:
					rdoImport.Checked = true;
					break;
				case 1:
					rdoEmpty.Checked = true;
					break;
				case 2:
					rdoOverride.Checked = true;
					if (cmbChangeToo.DropDownStyle == ComboBoxStyle.DropDownList)
						cmbChangeToo.SelectedValue = FieldReplacer.ChangeValue;
					else
						cmbChangeToo.Text = Convert.ToString(FieldReplacer.ChangeValue);
					break;
			}

			eXPFrame1.Tag = null;

		}

		private void rdoOverride_CheckedChanged(object sender, System.EventArgs e)
		{
			if (Convert.ToString(eXPFrame1.Tag) != "OFFLINE")
			{
				Control ctrl = sender as Control;
				if (ctrl != null)
					switch (ctrl.Name)
					{
						case "rdoEmpty":
							FieldReplacer.ChangeType = 0;
							break;
						case "rdoImport":
							FieldReplacer.ChangeType = 1;
							break;
						case "rdoOverride":
							FieldReplacer.ChangeType = 2;
							break;
					}
			}
			cmbChangeToo.Enabled = rdoOverride.Checked;
		}

		private void frmImportReplace_Load(object sender, System.EventArgs e)
		{
			dgReplace.Focus();
		}

		private void cmbImportValue_Enter(object sender, System.EventArgs e)
		{
			eXPFrame1.Focus();
		}

		private void cmbChangeToo_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (cmbChangeToo.DropDownStyle == ComboBoxStyle.DropDownList)
				FieldReplacer.ChangeValue = cmbChangeToo.SelectedValue;
		}

		private void cmbChangeToo_TextChanged(object sender, System.EventArgs e)
		{
			if (Convert.ToString(eXPFrame1.Tag) != "OFFLINE")
				if (cmbChangeToo.DropDownStyle == ComboBoxStyle.DropDown)
					FieldReplacer.ChangeValue = cmbChangeToo.Text;
		
		}

		private void NullFilter_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rdoOn.Checked)
				filtered.RowFilter = "[Category] = '" + cmbCategory.SelectedItem + "'";
			else if (rdoOff.Checked)
				 filtered.RowFilter = "[Category] = '" + cmbCategory.SelectedItem + "' AND FieldValue <> ''";
		}

		#region Properties
		public FWBS.OMS.Design.Export.FieldReplacer FieldReplacer
		{
			get
			{
				return _fieldreplacer;
			}
			set
			{
				_fieldreplacer = value;
				cmbCategory.Items.Clear();
				DataView _category = new DataView(_fieldreplacer.Source);
				_category.Sort = "Category";
				string oldcat = "";
				foreach (DataRowView rw in _category)
					if (oldcat != Convert.ToString(rw["Category"]))
					{
						oldcat = Convert.ToString(rw["Category"]);
						cmbCategory.Items.Add(oldcat);
					}
				if (cmbCategory.Items.Count > 0)
					cmbCategory.SelectedIndex =0;
			}
		}
		#endregion
	}
}
