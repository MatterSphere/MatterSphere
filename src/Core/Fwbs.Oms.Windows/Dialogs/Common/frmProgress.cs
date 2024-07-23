using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.UI.Dialogs.Common;

namespace FWBS.OMS.UI.Windows
{
    internal class frmProgress : BaseForm
    {
        #region Events

        public event EventHandler Cancelled = null;

        #endregion

        #region Fields

        public bool Cancel = false;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labInfo;

        #endregion
        private ResourceLookup resourceLookup1;
        private Panel pnlBackground;
        private IContainer components;

        #region Constructors
        public frmProgress()
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labInfo = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(378, 105);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(8, 83);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(446, 16);
            this.progressBar1.TabIndex = 2;
            // 
            // labInfo
            // 
            this.labInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labInfo.Location = new System.Drawing.Point(8, 8);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(446, 66);
            this.labInfo.TabIndex = 3;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.labInfo);
            this.pnlBackground.Controls.Add(this.progressBar1);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(8);
            this.pnlBackground.Size = new System.Drawing.Size(462, 138);
            this.pnlBackground.TabIndex = 0;
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(462, 138);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmProgress", "Progress", ""));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progress";
            this.pnlBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties
        [Browsable(false)]
        public string Label
        {
            get
            {
                return labInfo.Text;
            }
            set
            {
                labInfo.Text = value;
            }
        }

        public string CancelMessageBoxText { get; set; }

        public string CancelMessageBoxTitle { get; set; }

        [Browsable(false)]
        public ProgressBar ProgressBar
        {
            get
            {
                return progressBar1;
            }
        }

        public bool CanCancel
        {
            set
            {
                if (value)
                {
                    if (btnCancel.Visible == false)
                        btnCancel.Visible = true;
                }
                btnCancel.Enabled = value;
            }
        }

        #endregion

        #region Protected

        protected void OnCancelled()
        {
            this.Cancel = true;
            if (Cancelled != null)
                Cancelled(this,EventArgs.Empty);
        }
        #endregion

        #region Private

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (!HideCancellationConfirmationDialog())
            {
                ShowAreYouSureMessageBox();
            }
            else
            {
                OnCancelled();
            }
        }


        private bool HideCancellationConfirmationDialog()
        {
            if (Session.CurrentSession.CurrentUser.HideCancellationConfirmationDialog == FWBS.Common.TriState.True)
            {
                return true;   //don't show the dialog
            }

            if (Session.CurrentSession.CurrentUser.HideCancellationConfirmationDialog == FWBS.Common.TriState.False)
            {
                return false;   // show the dialog
            }

            if (Session.CurrentSession.HideCancellationConfirmationDialog)
            {
                return true;    // don't show the dialog
            }
            else
            {
                return false;    // show the dialog
            }
        }


        private void ShowAreYouSureMessageBox()
        {
            CheckedMessageBox dlg = new CheckedMessageBox();

            DialogResult dr = dlg.Show(DialogResult.OK,
                                       Session.CurrentSession.Resources.GetResource("CHKMSGCANCEL1", "Don't show this again", "").Text,
                                       CancelMessageBoxText ?? Session.CurrentSession.Resources.GetResource("CHKMSGCANCEL2", "Are you sure you want to cancel this save?", "").Text,
                                       CancelMessageBoxTitle ?? Session.CurrentSession.Resources.GetResource("CHKMSGCANCEL3", "Multiple Document Save", "").Text,
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);

            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                OnCancelled();
            }
        }
        #endregion
    }
}
