using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmAdditionalText.
    /// </summary>
    internal class frmAdditionalText : BaseForm
	{
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.ComponentModel.IContainer components;

		private string _firstlookup = "";
		private string _secondlookup = "";
		private string _thirdlookup = "";
		private bool _slogan = false;
		private string _delimiter = "";
		private FieldParser parser = new FieldParser();
		private FWBS.OMS.Interfaces.IOMSApp _appController = null;
		private object _doc = null;

		private System.Windows.Forms.Panel pnlMain;
		private FWBS.Common.UI.Windows.eXPFrame gbSecond;
		private CustomCheckedListBox chkLstSecond;
		private System.Windows.Forms.Button btnOK;
		private FWBS.Common.UI.Windows.eXPFrame gbFirst;
		private CustomCheckedListBox chkLstFirst;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Panel pnlFirst;
		private System.Windows.Forms.Panel pnlThird;
		private System.Windows.Forms.Panel pnlSecond;
		private FWBS.Common.UI.Windows.eXPFrame gbThird;
		private System.Windows.Forms.Panel pnlENC;
		private System.Windows.Forms.TextBox txtENC;
		private System.Windows.Forms.Label lblENC;
		private System.Windows.Forms.Panel pnlCC;
		private System.Windows.Forms.TextBox txtCC;
		private System.Windows.Forms.Label lblCC;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlHelp;
		private FWBS.Common.UI.Windows.eInformation eInformation1;
		private System.Windows.Forms.Panel pnlOther;
		private System.Windows.Forms.TextBox txtOther;
		private System.Windows.Forms.Label lblOther;
		private CustomCheckedListBox chkLstThird;


		public frmAdditionalText(FWBS.OMS.Interfaces.IOMSApp appController, object doc, string delimiter,string firstselection, string secondselection, string thirdselection) : base()
		{
			_appController = appController;
			_doc = doc;
			_firstlookup = firstselection;
			_secondlookup = secondselection;
			_thirdlookup = thirdselection;
			_delimiter = delimiter;

            var ver = appController.GetCurrentDocumentVersion(doc);

            if (ver == null)
                parser.ChangeObject(ver);
            else
                parser.ChangeObject(appController.GetCurrentAssociate(doc));

			// If Delimiter is blank or null then set as default return value
			if ((_delimiter == null) | (_delimiter == ""))
			{
				_delimiter = Environment.NewLine;
			}
			
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdditionalText));
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.lblOther = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbSecond = new FWBS.Common.UI.Windows.eXPFrame();
            this.chkLstSecond = new FWBS.OMS.UI.Windows.frmAdditionalText.CustomCheckedListBox();
            this.gbFirst = new FWBS.Common.UI.Windows.eXPFrame();
            this.chkLstFirst = new FWBS.OMS.UI.Windows.frmAdditionalText.CustomCheckedListBox();
            this.gbThird = new FWBS.Common.UI.Windows.eXPFrame();
            this.chkLstThird = new FWBS.OMS.UI.Windows.frmAdditionalText.CustomCheckedListBox();
            this.eInformation1 = new FWBS.Common.UI.Windows.eInformation();
            this.lblENC = new System.Windows.Forms.Label();
            this.lblCC = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlThird = new System.Windows.Forms.Panel();
            this.pnlSecond = new System.Windows.Forms.Panel();
            this.pnlFirst = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlENC = new System.Windows.Forms.Panel();
            this.txtENC = new System.Windows.Forms.TextBox();
            this.pnlCC = new System.Windows.Forms.Panel();
            this.txtCC = new System.Windows.Forms.TextBox();
            this.pnlHelp = new System.Windows.Forms.Panel();
            this.pnlOther = new System.Windows.Forms.Panel();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.gbSecond.SuspendLayout();
            this.gbFirst.SuspendLayout();
            this.gbThird.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlThird.SuspendLayout();
            this.pnlSecond.SuspendLayout();
            this.pnlFirst.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlENC.SuspendLayout();
            this.pnlCC.SuspendLayout();
            this.pnlHelp.SuspendLayout();
            this.pnlOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOther
            // 
            this.lblOther.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblOther.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblOther.Location = new System.Drawing.Point(16, 3);
            this.resourceLookup1.SetLookup(this.lblOther, new FWBS.OMS.UI.Windows.ResourceLookupItem("OTHER", "Other", ""));
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(100, 16);
            this.lblOther.TabIndex = 0;
            this.lblOther.Text = "lblOther";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(6, 6);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(6, 34);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cance&l";
            // 
            // gbSecond
            // 
            this.gbSecond.Controls.Add(this.chkLstSecond);
            this.gbSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSecond.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbSecond.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.gbSecond.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbSecond.Location = new System.Drawing.Point(1, 1);
            this.resourceLookup1.SetLookup(this.gbSecond, new FWBS.OMS.UI.Windows.ResourceLookupItem("gbSecond", "Additional Text of Letter", ""));
            this.gbSecond.Name = "gbSecond";
            this.gbSecond.Padding = new System.Windows.Forms.Padding(10, 17, 10, 10);
            this.gbSecond.Size = new System.Drawing.Size(426, 118);
            this.gbSecond.TabIndex = 100;
            this.gbSecond.Text = "Additional Text of Letter";
            // 
            // chkLstSecond
            // 
            this.chkLstSecond.CheckOnClick = true;
            this.chkLstSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLstSecond.IntegralHeight = false;
            this.chkLstSecond.Location = new System.Drawing.Point(10, 17);
            this.chkLstSecond.Name = "chkLstSecond";
            this.chkLstSecond.Size = new System.Drawing.Size(406, 91);
            this.chkLstSecond.TabIndex = 1;
            this.chkLstSecond.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLstFirst_ItemCheck);
            this.chkLstSecond.Enter += new System.EventHandler(this.chkLstFirst_Enter);
            // 
            // gbFirst
            // 
            this.gbFirst.Controls.Add(this.chkLstFirst);
            this.gbFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFirst.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbFirst.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.gbFirst.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbFirst.Location = new System.Drawing.Point(1, 1);
            this.resourceLookup1.SetLookup(this.gbFirst, new FWBS.OMS.UI.Windows.ResourceLookupItem("gbFirst", "Additional Header Text of Letter", ""));
            this.gbFirst.Name = "gbFirst";
            this.gbFirst.Padding = new System.Windows.Forms.Padding(10, 17, 10, 10);
            this.gbFirst.Size = new System.Drawing.Size(426, 118);
            this.gbFirst.TabIndex = 50;
            this.gbFirst.Text = "Additional Header Text of Letter";
            // 
            // chkLstFirst
            // 
            this.chkLstFirst.CheckOnClick = true;
            this.chkLstFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLstFirst.IntegralHeight = false;
            this.chkLstFirst.Location = new System.Drawing.Point(10, 17);
            this.chkLstFirst.Name = "chkLstFirst";
            this.chkLstFirst.Size = new System.Drawing.Size(406, 91);
            this.chkLstFirst.TabIndex = 0;
            this.chkLstFirst.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLstFirst_ItemCheck);
            this.chkLstFirst.Enter += new System.EventHandler(this.chkLstFirst_Enter);
            // 
            // gbThird
            // 
            this.gbThird.Controls.Add(this.chkLstThird);
            this.gbThird.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbThird.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbThird.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.gbThird.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbThird.Location = new System.Drawing.Point(1, 1);
            this.resourceLookup1.SetLookup(this.gbThird, new FWBS.OMS.UI.Windows.ResourceLookupItem("gbThird", "Additional Text of Letter", ""));
            this.gbThird.Name = "gbThird";
            this.gbThird.Padding = new System.Windows.Forms.Padding(10, 17, 10, 10);
            this.gbThird.Size = new System.Drawing.Size(426, 105);
            this.gbThird.TabIndex = 200;
            this.gbThird.Text = "Additional Text of Letter";
            // 
            // chkLstThird
            // 
            this.chkLstThird.CheckOnClick = true;
            this.chkLstThird.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLstThird.IntegralHeight = false;
            this.chkLstThird.Location = new System.Drawing.Point(10, 17);
            this.chkLstThird.Name = "chkLstThird";
            this.chkLstThird.Size = new System.Drawing.Size(406, 78);
            this.chkLstThird.TabIndex = 2;
            this.chkLstThird.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLstFirst_ItemCheck);
            this.chkLstThird.Enter += new System.EventHandler(this.chkLstFirst_Enter);
            // 
            // eInformation1
            // 
            this.eInformation1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.eInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eInformation1.Location = new System.Drawing.Point(5, 5);
            this.resourceLookup1.SetLookup(this.eInformation1, new FWBS.OMS.UI.Windows.ResourceLookupItem("eInformation1", "eInformation1", ""));
            this.eInformation1.Name = "eInformation1";
            this.eInformation1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.eInformation1.Size = new System.Drawing.Size(428, 62);
            this.eInformation1.TabIndex = 0;
            this.eInformation1.Text = "eInformation1";
            this.eInformation1.Title = "Help Bar";
            // 
            // lblENC
            // 
            this.lblENC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblENC.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblENC.Location = new System.Drawing.Point(16, 4);
            this.resourceLookup1.SetLookup(this.lblENC, new FWBS.OMS.UI.Windows.ResourceLookupItem("ENC", "ENC", ""));
            this.lblENC.Name = "lblENC";
            this.lblENC.Size = new System.Drawing.Size(100, 16);
            this.lblENC.TabIndex = 0;
            this.lblENC.Text = "lblENC";
            // 
            // lblCC
            // 
            this.lblCC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCC.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCC.Location = new System.Drawing.Point(16, 3);
            this.resourceLookup1.SetLookup(this.lblCC, new FWBS.OMS.UI.Windows.ResourceLookupItem("CC", "CC", ""));
            this.lblCC.Name = "lblCC";
            this.lblCC.Size = new System.Drawing.Size(100, 16);
            this.lblCC.TabIndex = 0;
            this.lblCC.Text = "lblCC";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlThird);
            this.pnlMain.Controls.Add(this.pnlSecond);
            this.pnlMain.Controls.Add(this.pnlFirst);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5);
            this.pnlMain.Size = new System.Drawing.Size(438, 355);
            this.pnlMain.TabIndex = 4;
            this.pnlMain.Resize += new System.EventHandler(this.pnlMain_Resize);
            // 
            // pnlThird
            // 
            this.pnlThird.Controls.Add(this.gbThird);
            this.pnlThird.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlThird.Location = new System.Drawing.Point(5, 245);
            this.pnlThird.Name = "pnlThird";
            this.pnlThird.Padding = new System.Windows.Forms.Padding(1);
            this.pnlThird.Size = new System.Drawing.Size(428, 107);
            this.pnlThird.TabIndex = 2;
            // 
            // pnlSecond
            // 
            this.pnlSecond.Controls.Add(this.gbSecond);
            this.pnlSecond.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSecond.Location = new System.Drawing.Point(5, 125);
            this.pnlSecond.Name = "pnlSecond";
            this.pnlSecond.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSecond.Size = new System.Drawing.Size(428, 120);
            this.pnlSecond.TabIndex = 1;
            // 
            // pnlFirst
            // 
            this.pnlFirst.Controls.Add(this.gbFirst);
            this.pnlFirst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFirst.Location = new System.Drawing.Point(5, 5);
            this.pnlFirst.Name = "pnlFirst";
            this.pnlFirst.Padding = new System.Windows.Forms.Padding(1);
            this.pnlFirst.Size = new System.Drawing.Size(428, 120);
            this.pnlFirst.TabIndex = 0;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(438, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(86, 502);
            this.pnlButtons.TabIndex = 5;
            // 
            // pnlENC
            // 
            this.pnlENC.Controls.Add(this.txtENC);
            this.pnlENC.Controls.Add(this.lblENC);
            this.pnlENC.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlENC.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlENC.Location = new System.Drawing.Point(0, 405);
            this.pnlENC.Name = "pnlENC";
            this.pnlENC.Padding = new System.Windows.Forms.Padding(20, 1, 1, 1);
            this.pnlENC.Size = new System.Drawing.Size(438, 25);
            this.pnlENC.TabIndex = 12;
            this.pnlENC.Visible = false;
            this.pnlENC.VisibleChanged += new System.EventHandler(this.pnlMain_Resize);
            // 
            // txtENC
            // 
            this.txtENC.Location = new System.Drawing.Point(120, 1);
            this.txtENC.Name = "txtENC";
            this.txtENC.Size = new System.Drawing.Size(278, 23);
            this.txtENC.TabIndex = 3;
            // 
            // pnlCC
            // 
            this.pnlCC.Controls.Add(this.txtCC);
            this.pnlCC.Controls.Add(this.lblCC);
            this.pnlCC.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCC.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlCC.Location = new System.Drawing.Point(0, 355);
            this.pnlCC.Name = "pnlCC";
            this.pnlCC.Padding = new System.Windows.Forms.Padding(15, 1, 1, 1);
            this.pnlCC.Size = new System.Drawing.Size(438, 25);
            this.pnlCC.TabIndex = 13;
            this.pnlCC.Visible = false;
            this.pnlCC.VisibleChanged += new System.EventHandler(this.pnlMain_Resize);
            // 
            // txtCC
            // 
            this.txtCC.Location = new System.Drawing.Point(120, 0);
            this.txtCC.Name = "txtCC";
            this.txtCC.Size = new System.Drawing.Size(278, 23);
            this.txtCC.TabIndex = 4;
            // 
            // pnlHelp
            // 
            this.pnlHelp.Controls.Add(this.eInformation1);
            this.pnlHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHelp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlHelp.Location = new System.Drawing.Point(0, 430);
            this.pnlHelp.Name = "pnlHelp";
            this.pnlHelp.Padding = new System.Windows.Forms.Padding(5);
            this.pnlHelp.Size = new System.Drawing.Size(438, 72);
            this.pnlHelp.TabIndex = 15;
            // 
            // pnlOther
            // 
            this.pnlOther.Controls.Add(this.txtOther);
            this.pnlOther.Controls.Add(this.lblOther);
            this.pnlOther.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOther.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlOther.Location = new System.Drawing.Point(0, 380);
            this.pnlOther.Name = "pnlOther";
            this.pnlOther.Padding = new System.Windows.Forms.Padding(15, 1, 1, 1);
            this.pnlOther.Size = new System.Drawing.Size(438, 25);
            this.pnlOther.TabIndex = 16;
            this.pnlOther.Visible = false;
            // 
            // txtOther
            // 
            this.txtOther.Location = new System.Drawing.Point(120, 0);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(278, 23);
            this.txtOther.TabIndex = 4;
            // 
            // frmAdditionalText
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(524, 502);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlCC);
            this.Controls.Add(this.pnlOther);
            this.Controls.Add(this.pnlENC);
            this.Controls.Add(this.pnlHelp);
            this.Controls.Add(this.pnlButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmAddText", "Additional Text...", ""));
            this.Name = "frmAdditionalText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.frmAdditionalText_Activated);
            this.Load += new System.EventHandler(this.frmAdditionalText_Load);
            this.gbSecond.ResumeLayout(false);
            this.gbFirst.ResumeLayout(false);
            this.gbThird.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlThird.ResumeLayout(false);
            this.pnlSecond.ResumeLayout(false);
            this.pnlFirst.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlENC.ResumeLayout(false);
            this.pnlENC.PerformLayout();
            this.pnlCC.ResumeLayout(false);
            this.pnlCC.PerformLayout();
            this.pnlHelp.ResumeLayout(false);
            this.pnlOther.ResumeLayout(false);
            this.pnlOther.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmAdditionalText_Load(object sender, System.EventArgs e)
		{
			PopulateList(chkLstFirst, _firstlookup);
			PopulateList(chkLstSecond, _secondlookup);
			PopulateList(chkLstThird, _thirdlookup);
			pnlMain_Resize(sender,e);

            eInformation1.Title = Session.CurrentSession.Resources.GetResource("HelpBarTitle", eInformation1.Title, "").Text;

		}

		private void PopulateList(CheckedListBox lb, string lookup)
		{
			if (lookup == "")
			{
				lb.Visible = false;
			}
			else
			{

				lb.Parent.Text = FWBS.OMS.CodeLookup.GetLookup("DOCUMENTS",lookup);
				lb.Parent.Tag = FWBS.OMS.CodeLookup.GetLookupHelp("DOCUMENTS",lookup);
				lb.BeginUpdate();
				DataTable dt = FWBS.OMS.CodeLookup.GetLookups(lookup);
				
				if (dt.Columns.Contains("cddescorder") == false)
					dt.Columns.Add("cddescorder");

				foreach (DataRow row in dt.Rows)
				{
					string txt = parser.ParseString(Convert.ToString(row["cddesc"]));
					row["cddesc"] = txt;
					row["cddescorder"] = txt.Replace("&", "");
				}

				dt.DefaultView.Sort = "cddescorder";
				lb.Tag = _firstlookup;
				lb.DataSource = dt;
				lb.DisplayMember = "cddesc";
				lb.ValueMember = "cdcode";
				lb.EndUpdate();
			}
		}

		private void SetCheckItem(CheckedListBox lst, string code, bool val)
		{
			object current = lst.SelectedValue;
			DataTable tmpdt = (DataTable)lst.DataSource;
			tmpdt.PrimaryKey = new DataColumn [] {tmpdt.Columns["cdcode"]};
			DataRow dr = tmpdt.Rows.Find(new object [] {code});
			if (dr != null)
			{
				lst.SelectedValue = code;
				lst.SetItemChecked(lst.SelectedIndex,val);
				lst.SelectedValue = current;
			}

		}

        private void chkLstFirst_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				CheckedListBox lst = (CheckedListBox)sender;

				DataRowView r = lst.Items[e.Index] as DataRowView;
				
				string txt;
				if (r == null)
					txt = Convert.ToString(lst.SelectedValue);
				else
					txt = Convert.ToString(r[0]);

				switch(txt.ToUpper())
				{
					case "1STCLASS":
					{
						if (e.NewValue==CheckState.Checked)
						{		
							SetCheckItem(lst, "2NDCLASS", false);
						}
					}
						break;
					case "2NDCLASS":
					{
						if (e.NewValue==CheckState.Checked)
						{
							SetCheckItem(lst, "1STCLASS", false);
						}
					}
						break;
					case "ENC":
					{
						if (e.NewValue==CheckState.Checked)
						{
							lblENC.Text = Convert.ToString(r[1]);
							pnlENC.Visible = true;
							pnlENC.Focus();
						}
						else
						{
							pnlENC.Visible = false;
						}
					}
						break;
					case "CC":
					{
						if (e.NewValue==CheckState.Checked)
						{
							lblCC.Text = Convert.ToString(r[1]);
							pnlCC.Visible = true;
							pnlCC.Focus();
						}
						else
						{
							pnlCC.Visible = false;
						}
					}
						break;
					case "OTHER":
					{
						if (e.NewValue==CheckState.Checked)
						{
							lblOther.Text = Convert.ToString(r[1]);
							pnlOther.Visible = true;
							pnlOther.Focus();
						}
						else
						{
							pnlOther.Visible = false;
						}
					}
						break;
					case "FAX&POST":
					{
						if (e.NewValue==CheckState.Checked)
						{
							string old = _appController.GetDocVariable(_doc, "FAXTIME", "");
							_appController.SetDocVariable(_doc, "FAXTIME", "DONTFAX");

							if (_appController is OMSApp)
							{
								if (((OMSApp)_appController).ShowFaxOptions(this, _doc) == false)
								{
									_appController.SetDocVariable(_doc, "FAXTIME", old);
									e.NewValue = CheckState.Unchecked;
								}
								else
								{
									SetCheckItem(lst, "FAX", false);
									SetCheckItem(lst, "FAX&DX", false);
								}
							}
						}
					}
						break;
					case "FAX":
					{
						if (e.NewValue==CheckState.Checked)
						{
							string old = _appController.GetDocVariable(_doc, "FAXTIME", "");
							_appController.SetDocVariable(_doc, "FAXTIME", "NOW");

							if (_appController is OMSApp)
							{
								if (((OMSApp)_appController).ShowFaxOptions(this, _doc) == false)
								{
									_appController.SetDocVariable(_doc, "FAXTIME", old);
									e.NewValue = CheckState.Unchecked;
								}
								else
								{
									SetCheckItem(lst, "FAX&POST", false);
									SetCheckItem(lst, "FAX&DX", false);
								}
							}
							
						}

					}
						break;
					case "FAX&DX":
					{
						if (e.NewValue==CheckState.Checked)
						{
							string old = _appController.GetDocVariable(_doc, "FAXTIME", "");
							_appController.SetDocVariable(_doc, "FAXTIME", "DONTFAX");

							if (_appController is OMSApp)
							{
								if (((OMSApp)_appController).ShowFaxOptions(this, _doc) == false)
								{
									_appController.SetDocVariable(_doc, "FAXTIME", old);
									e.NewValue = CheckState.Unchecked;
								}
								else
								{
									SetCheckItem(lst, "FAX", false);
									SetCheckItem(lst, "FAX&POST", false);
								}
							}
						}
					}
						break;
				}
			}
			catch (Exception ex)
			{
				e.NewValue = CheckState.Unchecked;
				MessageBox.Show(this, ex);
			}
			finally 
			{
				Cursor = Cursors.Default;
			}
		}

		private void frmAdditionalText_Activated(object sender, System.EventArgs e)
		{
			if (chkLstFirst.Visible == true)
				chkLstFirst.TabIndex = 0;
			if (chkLstSecond.Visible == true)
				chkLstSecond.TabIndex = 1;
			if (chkLstThird.Visible == true)
				chkLstThird.TabIndex = 2;

			chkLstFirst.Focus();
		}

		private void pnlMain_Resize(object sender, System.EventArgs e)
		{
			pnlFirst.Height = (pnlMain.Height - 10) / 3;
			pnlSecond.Height = (pnlMain.Height - 10) / 3;
			pnlThird.Height = (pnlMain.Height - 10) / 3;
		}

		/// <summary>
		/// Returns the String of the Extra Text information to be related to Panel One
		/// </summary>
		public string PanelOneText
		{
			get
			{
				return GetText(chkLstFirst );
			}
		}

		/// <summary>
		/// Will Return the Concatenated Text of the Panel Text Two, 
		/// </summary>
		public string PanelTwoText
		{
			get
			{
				return GetText(chkLstSecond);
			}		
		}

		/// <summary>
		/// Will Return the Concatenated Text of the Panel Text Three, 
		/// </summary>
		public string PanelThreeText
		{
			get
			{
				return GetText(chkLstThird);
			}
	
		}

		private string GetText(CheckedListBox lb)
		{
			if (lb.CheckedItems.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(DataRowView t in lb.CheckedItems)
				{
					switch (Convert.ToString(t["cdcode"]).ToUpper())
					{
						case "ENC":
						{
							sb.Append(Convert.ToString(t["cddesc"]));
							if (txtENC.Text != "") // Add the Enclosures Text
							{
								sb.Append(_delimiter);
								sb.Append(txtENC.Text);
							}
						}
							break;
						case "CC":
						{
							sb.Append(Convert.ToString(t["cddesc"]));
							if (txtCC.Text != "") // Add the Enclosures Text
							{
								sb.Append(_delimiter);
								sb.Append(txtCC.Text);
							}
						}
							break;
						case "OTHER":
						{
							if (txtOther.Text != "") // Add the Others Text
							{
								sb.Append(_delimiter);
								sb.Append(txtOther.Text);
							}
						}
							break;
						case "SLOGAN":
							_slogan = true;
							break;
						case "FAX":
						{
							sb.Append(Convert.ToString(t["cdhelp"]).Replace("%FAXNUMBER%", _appController.GetDocVariable(_doc, "FAXNUMBER", "")));
							sb.Append(_delimiter);
						}
							break;
						case "FAX&POST":
							goto case "FAX";
						case "FAX&DX":
							goto case "FAX";
						default:
						{
							sb.Append(Convert.ToString(t["cddesc"]));
						}
							break;
					}
						
					sb.Append(_delimiter);
				}
				return sb.ToString();
			}
			else
			{
				return "";
			}
		}

		private void chkLstFirst_Enter(object sender, System.EventArgs e)
		{
			eInformation1.Text = Convert.ToString(((Control)sender).Parent.Tag);
		}


		/// <summary>
		/// Gets a value indicating whether a slogan is to be added.
		/// </summary>
		public bool Slogan
		{
			get
			{
				return _slogan;
			}
		}




		private class CustomCheckedListBox : CheckedListBox
		{
			private Hashtable _hotkeys = new Hashtable();

			protected override void OnDrawItem(DrawItemEventArgs e)
			{
				Size iconSize = LogicalToDeviceUnits(new Size(16, 16));

				if (base.DesignMode)
				{
					base.OnDrawItem(e);
				}
				else
				{
					if (e.Index>-1)
					{
						bool rightToLeft = (this.RightToLeft == RightToLeft.Yes);

						DataRowView item = (DataRowView)base.Items[e.Index];

						// no flickering: we create a shadow bitmap to draw 
						// into and only transfer once we've completed:
						Bitmap bmp = new Bitmap(e.Bounds.Width, e.Bounds.Height);
						Graphics gr = Graphics.FromImage(bmp);
               
						// fill the background:
						SolidBrush brBack = new SolidBrush(this.BackColor);
						gr.FillRectangle(brBack,0, 0,e.Bounds.Width, e.Bounds.Height);
						brBack.Dispose();

						// evaluate the indent:
						int indent = 0;
						Rectangle rcItem;
						if (rightToLeft)
						{
							rcItem = new Rectangle(0, 0,e.Bounds.Width - indent, e.Bounds.Height);
						}
						else
						{
							rcItem = new Rectangle(indent, 0,e.Bounds.Width - indent, e.Bounds.Height);
						}

						// Draw selection rectangle if necessary:
						if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
						{
							gr.FillRectangle(SystemBrushes.Highlight, rcItem.Left, rcItem.Top, rcItem.Width, rcItem.Height);
						}

						// Draw the Check box:
						int pos = LogicalToDeviceUnits(2);
						if (rightToLeft)
						{
							pos = rcItem.Right - iconSize.Width - pos;
						}

						ButtonState state = ButtonState.Normal;

						switch (GetItemCheckState(e.Index))
						{
							case CheckState.Checked:
								state = ButtonState.Checked;
								break;
							case CheckState.Indeterminate:
								state = ButtonState.Inactive;
								break;
						}
					
						state |= ButtonState.Flat;
						ControlPaint.DrawCheckBox(gr, pos, rcItem.Top, iconSize.Width, iconSize.Height, state);
						pos += (rightToLeft ? -(iconSize.Width) : iconSize.Width);
					
						// Draw the image onto the control, not the
						// offscreen bitmap:
						pos += (rightToLeft ? 0 : iconSize.Width);

						// Draw the Text:
						Brush brText = null;
						bool noDispose = false;
						if (((e.State & DrawItemState.Selected) != DrawItemState.Selected))
						{
							brText = new SolidBrush(base.ForeColor);
							noDispose = false;
						}
						else
						{
							brText = SystemBrushes.HighlightText;
							noDispose = true;
						}
						RectangleF textRect;
						if (rightToLeft)
						{
							textRect = new RectangleF(0, rcItem.Top, pos, rcItem.Height);
						}
						else
						{
							textRect = new RectangleF(pos, rcItem.Top,rcItem.Right - pos,rcItem.Height);
						}

						StringFormat textFormat = new StringFormat()
						{
							HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show, //base.ShowKeyboardCues;
							Trimming = StringTrimming.EllipsisCharacter,
							Alignment = rightToLeft ? StringAlignment.Far : StringAlignment.Near
						};
						gr.DrawString(Convert.ToString(item[1]), base.Font, brText, textRect, textFormat);
						textFormat.Dispose();

						if (!noDispose)
						{
							brText.Dispose();
						}

						// focus rectangle?
						if (((e.State & DrawItemState.Selected) == DrawItemState.Selected) & ((e.State & DrawItemState.Focus ) == DrawItemState.Focus))
						{
							ControlPaint.DrawFocusRectangle(gr, rcItem);
						}

						// Swap shadow bitmap to display:
						e.Graphics.DrawImage(bmp, e.Bounds, 0, 0, e.Bounds.Width, e.Bounds.Height, GraphicsUnit.Pixel);

						// clear up:
						bmp.Dispose();
						gr.Dispose();
					}
					else
					{
						base.OnDrawItem(e);
					}
				}

			}

			protected override bool ProcessMnemonic(char charCode)
			{
				int ctr = 0;
				foreach (DataRowView r in Items)
				{
					if (IsMnemonic(charCode, Convert.ToString(r[1])))
					{
						SetItemChecked(ctr, (!GetItemChecked(ctr)));
						return true;
					}
					ctr++;
				}

				return false;
			}


		}

	}

	
}
