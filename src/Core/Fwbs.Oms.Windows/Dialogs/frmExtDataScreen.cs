using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmExtDataScreen.
    /// </summary>
    internal class frmExtDataScreen : frmDialog
	{
		private System.Windows.Forms.Panel panel1;
		public FWBS.OMS.UI.Windows.ExtendedDataForm extendedDataForm1;
		private FWBS.OMS.Interfaces.IEnquiryCompatible _enq;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private frmExtDataScreen() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

        protected override void SetResources()
        {
            base.SetResources();
            this.Text = Session.CurrentSession.Resources.GetResource("frmExtDataScn", "Extended Data Form", "").Text;
        }

		/// <summary>
		/// Initialises an Extended Enquiry Form.
		/// </summary>
		internal frmExtDataScreen(string ExtendedDataCode, FWBS.OMS.Interfaces.IEnquiryCompatible enq) : this()
		{
			extendedDataForm1.ExtendedData = ((FWBS.OMS.Interfaces.IExtendedDataCompatible)enq).ExtendedData[ExtendedDataCode];
			extendedDataForm1.Dock = DockStyle.Fill;
			_enq = enq;
			ucFormStorage1.UniqueID = @"Forms\Dialogs\" + ExtendedDataCode;
			Text = FWBS.OMS.CodeLookup.GetLookup("EXTENDEDDATA",ExtendedDataCode);
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.extendedDataForm1 = new FWBS.OMS.UI.Windows.ExtendedDataForm();
            this.pnlTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 90;
            this.ucFormStorage1.DefaultPercentageWidth = 60;
            // 
            // tbLeft
            // 
            this.tbLeft.Size = new System.Drawing.Size(182, 42);
            // 
            // tbRight
            // 
            this.tbRight.Size = new System.Drawing.Size(234, 42);
            this.tbRight.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbRight_ButtonClick);
            // 
            // pnlTop
            // 
            this.pnlTop.Size = new System.Drawing.Size(566, 42);
            // 
            // quickOK
            // 
            this.quickOK.Size = new System.Drawing.Size(19, 32);
            // 
            // quickCancel
            // 
            this.quickCancel.Size = new System.Drawing.Size(19, 32);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.extendedDataForm1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12);
            this.panel1.Size = new System.Drawing.Size(566, 482);
            this.panel1.TabIndex = 10;
            // 
            // extendedDataForm1
            // 
            this.extendedDataForm1.AutoScroll = true;
            this.extendedDataForm1.ExtendedData = null;
            this.extendedDataForm1.Location = new System.Drawing.Point(208, 187);
            this.extendedDataForm1.Name = "extendedDataForm1";
            this.extendedDataForm1.Size = new System.Drawing.Size(150, 150);
            this.extendedDataForm1.TabIndex = 11;
            this.extendedDataForm1.ToBeRefreshed = false;
            // 
            // frmExtDataScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(566, 524);
            this.Controls.Add(this.panel1);
            this.Name = "frmExtDataScreen";
            this.Text = "Extended Data Form";
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.pnlTop.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void tbRight_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == cmdOK)
			{
				try
				{
					extendedDataForm1.UpdateItem();
					DialogResult = DialogResult.OK;
				}
				catch (Exception ex)
				{
					ErrorBox.Show(this, ex);
				}

			}
			else if (e.Button == cmdSave)
			{
				try
				{
					extendedDataForm1.UpdateItem();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(this, ex);
				}
			}
			else if (e.Button == cmdCancel)
				DialogResult = DialogResult.Cancel;
		}
	}
}
