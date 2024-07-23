using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom zoom box that displays a larger view of the specified text.
    /// This is handly for memo columns in data grids.
    /// </summary>
    internal sealed class frmZoomBox : BaseForm
	{
		#region Fields

		/// <summary>
		/// OK Button.
		/// </summary>
		private System.Windows.Forms.Button cmdOK;
		/// <summary>
		/// Cancel the text edit.
		/// </summary>
		private System.Windows.Forms.Button cmdCancel;
		/// <summary>
		/// Text window.
		/// </summary>
        internal System.Windows.Forms.TextBox txtText;

		#endregion
        private ResourceLookup resourceLookup1;
        private Panel pnlBackground;
        private IContainer components;

		#region Constructors & Destructors

		/// <summary>
		/// Creates an instance of this form.
		/// </summary>
		internal frmZoomBox() : base()
		{
			InitializeComponent();
			txtText.Focus();
			txtText.SelectAll();
		}

		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.txtText = new System.Windows.Forms.TextBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Location = new System.Drawing.Point(240, 104);
            this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdOK", "OK", ""));
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 24);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.Location = new System.Drawing.Point(321, 104);
            this.resourceLookup1.SetLookup(this.cmdCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdCancel", "Cancel", ""));
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 24);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            // 
            // txtText
            // 
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Location = new System.Drawing.Point(4, 4);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtText.Size = new System.Drawing.Size(392, 94);
            this.txtText.TabIndex = 0;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.txtText);
            this.pnlBackground.Controls.Add(this.cmdOK);
            this.pnlBackground.Controls.Add(this.cmdCancel);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(4, 4, 4, 36);
            this.pnlBackground.Size = new System.Drawing.Size(400, 134);
            this.pnlBackground.TabIndex = 3;
            // 
            // frmZoomBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(400, 134);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "frmZoomBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmZoom";
            this.RightToLeftChanged += new System.EventHandler(this.ZoomBox_RightToLeftChanged);
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


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

		#region Methods

		/// <summary>
		/// Captures the RightToLeft property changed event.
		/// </summary>
		/// <param name="sender">This form that calls the method.</param>
		/// <param name="e">Empty event arguments.</param>
		private void ZoomBox_RightToLeftChanged(object sender, System.EventArgs e)
		{
			Global.RightToLeftFormConverter(this);
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		#endregion
	}
}
