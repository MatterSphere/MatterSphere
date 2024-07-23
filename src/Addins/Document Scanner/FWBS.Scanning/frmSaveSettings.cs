using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FWBS.Scanning
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmSaveSettings : FWBS.OMS.UI.Windows.BaseForm
    {
        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.RadioButton rdoNothing;
		private System.Windows.Forms.RadioButton rdoDelete;
		private System.Windows.Forms.RadioButton rdoMove;
		private System.Windows.Forms.TextBox txtMovePath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private IContainer components;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.FolderBrowserDialog fldBrowse;
		private SaveSettingsResults _results = SaveSettingsResults.ssrNothing;
        private GroupBox groupBox1;
        private TextBox txtRegSearchString;
        private Button btnReset;
        private TextBox txtSample;
        private Button btnTest;
        private TextBox txtResults;
		private ucScanning _scanning = null;
        private string moveto;

		public frmSaveSettings(ucScanning Scanning, string MoveTo)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_scanning = Scanning;
            moveto = MoveTo;
            txtRegSearchString.Text = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetXmlProperty("ocrregex", @"([a-z]\d{1,15}[%delimiter%]\d{1,15})"));
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
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtMovePath = new System.Windows.Forms.TextBox();
            this.rdoMove = new System.Windows.Forms.RadioButton();
            this.rdoDelete = new System.Windows.Forms.RadioButton();
            this.rdoNothing = new System.Windows.Forms.RadioButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtRegSearchString = new System.Windows.Forms.TextBox();
            this.txtSample = new System.Windows.Forms.TextBox();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.fldBrowse = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlButtons.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(10, 398);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(322, 32);
            this.pnlButtons.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(167, 8);
            this.resLookup.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(247, 8);
            this.resLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cance&l";
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnBrowse);
            this.grpSettings.Controls.Add(this.txtMovePath);
            this.grpSettings.Controls.Add(this.rdoMove);
            this.grpSettings.Controls.Add(this.rdoDelete);
            this.grpSettings.Controls.Add(this.rdoNothing);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpSettings.Location = new System.Drawing.Point(10, 10);
            this.resLookup.SetLookup(this.grpSettings, new FWBS.OMS.UI.Windows.ResourceLookupItem("grpPRMSettings", "What to do", ""));
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(322, 147);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "What to do";
            this.grpSettings.Enter += new System.EventHandler(this.grpSettings_Enter);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(233, 113);
            this.resLookup.SetLookup(this.btnBrowse, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnBrowse", "&Browse", ""));
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 24);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtMovePath
            // 
            this.txtMovePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMovePath.Enabled = false;
            this.errorProvider1.SetIconAlignment(this.txtMovePath, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtMovePath.Location = new System.Drawing.Point(30, 114);
            this.txtMovePath.Name = "txtMovePath";
            this.txtMovePath.Size = new System.Drawing.Size(200, 23);
            this.txtMovePath.TabIndex = 3;
            // 
            // rdoMove
            // 
            this.rdoMove.Location = new System.Drawing.Point(14, 84);
            this.resLookup.SetLookup(this.rdoMove, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoMoveToFolder", "Move Image to folder", ""));
            this.rdoMove.Name = "rdoMove";
            this.rdoMove.Size = new System.Drawing.Size(293, 24);
            this.rdoMove.TabIndex = 2;
            this.rdoMove.Text = "Move Image to folder";
            this.rdoMove.CheckedChanged += new System.EventHandler(this.rdoMove_CheckedChanged);
            // 
            // rdoDelete
            // 
            this.rdoDelete.Location = new System.Drawing.Point(14, 53);
            this.resLookup.SetLookup(this.rdoDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoDeleteImage", "Delete Image", ""));
            this.rdoDelete.Name = "rdoDelete";
            this.rdoDelete.Size = new System.Drawing.Size(293, 24);
            this.rdoDelete.TabIndex = 1;
            this.rdoDelete.Text = "Delete Image";
            this.rdoDelete.CheckedChanged += new System.EventHandler(this.rdoMove_CheckedChanged);
            // 
            // rdoNothing
            // 
            this.rdoNothing.Checked = true;
            this.rdoNothing.Location = new System.Drawing.Point(14, 25);
            this.resLookup.SetLookup(this.rdoNothing, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoNothing", "Nothing", ""));
            this.rdoNothing.Name = "rdoNothing";
            this.rdoNothing.Size = new System.Drawing.Size(294, 24);
            this.rdoNothing.TabIndex = 0;
            this.rdoNothing.TabStop = true;
            this.rdoNothing.Text = "Nothing";
            this.rdoNothing.CheckedChanged += new System.EventHandler(this.rdoMove_CheckedChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // txtRegSearchString
            // 
            this.txtRegSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider1.SetIconAlignment(this.txtRegSearchString, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtRegSearchString.Location = new System.Drawing.Point(14, 19);
            this.txtRegSearchString.Name = "txtRegSearchString";
            this.txtRegSearchString.Size = new System.Drawing.Size(294, 23);
            this.txtRegSearchString.TabIndex = 4;
            // 
            // txtSample
            // 
            this.txtSample.AcceptsReturn = true;
            this.txtSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider1.SetIconAlignment(this.txtSample, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtSample.Location = new System.Drawing.Point(14, 46);
            this.txtSample.Multiline = true;
            this.txtSample.Name = "txtSample";
            this.txtSample.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSample.Size = new System.Drawing.Size(294, 130);
            this.txtSample.TabIndex = 6;
            this.txtSample.Text = "Mr N Smith\r\n1 High Street\r\n\r\nTel : 01509 666123\r\n\r\nYour Ref : DCT/N10000-1/13815\r" +
    "\n\r\nDear Mr Smith\r\n";
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider1.SetIconAlignment(this.txtResults, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtResults.Location = new System.Drawing.Point(94, 180);
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(213, 23);
            this.txtResults.TabIndex = 7;
            // 
            // fldBrowse
            // 
            this.fldBrowse.Description = "After Save Move Image to";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.txtResults);
            this.groupBox1.Controls.Add(this.txtSample);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.txtRegSearchString);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox1.Location = new System.Drawing.Point(10, 157);
            this.resLookup.SetLookup(this.groupBox1, new FWBS.OMS.UI.Windows.ResourceLookupItem("groupBoxOCR", "OCR RegEx Search String", ""));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 241);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OCR RegEx Search String";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(14, 179);
            this.resLookup.SetLookup(this.btnTest, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnTest", "&Test", ""));
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 24);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "&Test";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(233, 207);
            this.resLookup.SetLookup(this.btnReset, new FWBS.OMS.UI.Windows.ResourceLookupItem("RESET", "&Reset", ""));
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 24);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "&Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // frmSaveSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(342, 440);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("OPTIONS", "Options", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSaveSettings";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmSaveSettings_Load);
            this.pnlButtons.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void grpSettings_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void rdoMove_CheckedChanged(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtMovePath,"");
			txtMovePath.Enabled = rdoMove.Checked;
			btnBrowse.Enabled = rdoMove.Checked;
			if (rdoNothing.Checked) _results = SaveSettingsResults.ssrNothing;
			else if (rdoDelete.Checked) _results = SaveSettingsResults.ssrDelete;
			else if (rdoMove.Checked) _results = SaveSettingsResults.ssrMove;
		}

		private void frmSaveSettings_Load(object sender, System.EventArgs e)
		{
			string val = Convert.ToString(FWBS.Common.RegistryAccess.GetSetting("",Microsoft.Win32.RegistryHive.CurrentUser,@"\Software\FWBS\OMS\2.0\OMSDocumentImporter","AfterSave"));
			_results = (SaveSettingsResults)FWBS.Common.ConvertDef.ToEnum(val,SaveSettingsResults.ssrNothing);
			if (_results == SaveSettingsResults.ssrNothing) rdoNothing.Checked = true;
			else if (_results == SaveSettingsResults.ssrDelete) rdoDelete.Checked = true;
			else if (_results == SaveSettingsResults.ssrMove) rdoMove.Checked = true;
			try
			{
				object n = FWBS.OMS.Session.CurrentSession.CurrentUser.GetExtraInfo("usrImageFolderDone");
                txtMovePath.Text = moveto;
				btnBrowse.Visible = false;
                txtMovePath.ReadOnly = true;
                txtMovePath.Width = btnBrowse.Right - txtMovePath.Left;
            }
			catch
			{
				txtMovePath.Text = Convert.ToString(FWBS.Common.RegistryAccess.GetSetting("",Microsoft.Win32.RegistryHive.CurrentUser,@"\Software\FWBS\OMS\2.0\OMSDocumentImporter","MovePath"));
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
            try
            {
                string delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("/config/clientSearch/clientDelimiter", "-./:");
                string pattern = txtRegSearchString.Text.Replace("%delimiter%", delimiter);
                Regex search = new Regex(pattern, RegexOptions.IgnoreCase);
                Match results = search.Match(txtSample.Text);
                FWBS.OMS.Session.CurrentSession.CurrentBranch.SetXmlProperty("ocrregex", txtRegSearchString.Text);
                FWBS.OMS.Session.CurrentSession.CurrentBranch.Update();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                return;
            }
            if (_results == SaveSettingsResults.ssrMove && txtMovePath.Text == "")
				errorProvider1.SetError(txtMovePath,"You must specify a location to move the image to...");
			else
				DialogResult = DialogResult.OK;
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			string val = Convert.ToString(FWBS.Common.RegistryAccess.GetSetting("",Microsoft.Win32.RegistryHive.CurrentUser,@"\Software\FWBS\OMS\2.0\OMSDocumentImporter","Location"));
			if (txtMovePath.Text == "")
				fldBrowse.SelectedPath = val;
			else
				fldBrowse.SelectedPath = txtMovePath.Text;
			if (fldBrowse.ShowDialog(this) == DialogResult.OK)
			{
				if (fldBrowse.SelectedPath.ToUpper() == val.ToUpper())
					MessageBox.Show("The Destination is the same as the current location of the Image","OMS Validation",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				else
					txtMovePath.Text = fldBrowse.SelectedPath;
			}
		}

		public SaveSettingsResults Result
		{
			get
			{
				return _results;
			}
		}

		public string MovePath
		{
			get
			{
				if (_results == SaveSettingsResults.ssrMove)
					return txtMovePath.Text;
				else
					return String.Empty;
			}
		}

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtRegSearchString.Text = @"([a-z]\d{1,15}[%delimiter%]\d{1,15})";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("/config/clientSearch/clientDelimiter", "-./:");
                string pattern = txtRegSearchString.Text.Replace("%delimiter%", delimiter);
                Regex search = new Regex(pattern, RegexOptions.IgnoreCase);
                Match results = search.Match(txtSample.Text);
                txtResults.Text = results.Value;
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                return;
            }
        }
	}

	public enum SaveSettingsResults {ssrNothing, ssrDelete, ssrMove};
}
