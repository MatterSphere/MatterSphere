using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom message box which will allow the use of custom buttons and icons.
    /// </summary>
    internal class frmMessageBox : BaseForm
	{
		#region Fields

		/// <summary>
		/// Custom dialog result.
		/// </summary>
		private string _result = "";

		#endregion

		#region Controls

		private System.Windows.Forms.Panel pnlContent;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Panel pnlButtonContainer;
		private System.Windows.Forms.Panel pnlButtons;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructors

		private frmMessageBox(){}

		public frmMessageBox(string text, string caption, string [] buttons, MessageBoxIcon icon, string defaultButton, string cancelButton) : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            pnlContent.Font = pnlButtonContainer.Font = SystemFonts.MessageBoxFont;
            txtMessage.BackColor = ExtColor.ConvertToRGB(this.BackColor);

			this.Text = caption;
			txtMessage.Text = text;
            picIcon.Tag = icon;
            picIcon.Image = ConvertMessageBoxIconToImage(icon);

			int left = 0;
			int ctr = 0;
			foreach (string btn in buttons)
			{
				Button b = new Button() { Tag = btn };
				b.Text = Session.CurrentSession.Resources.GetResource(btn, btn, "").Text;
				b.FlatStyle = FlatStyle.System;
				pnlButtons.Controls.Add(b, true);
				b.Location = new Point(left, 0);
				left = left + b.Width + LogicalToDeviceUnits(8);
				b.TabIndex = ctr;
				ctr++;
				if (btn == defaultButton) this.AcceptButton = b;
				if (btn == cancelButton) this.CancelButton = b;
				b.Click += new EventHandler(b_Click);
			}
			pnlButtons.Width = left;
			_result = cancelButton;
			FWBS.OMS.UI.Windows.Global.RightToLeftFormConverter(this);

			if (this.AcceptButton  != null) ((Button)this.AcceptButton).Select();
	 
			pnlButtonContainer_Resize(null, EventArgs.Empty);
		}

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            picIcon.Image = ConvertMessageBoxIconToImage((MessageBoxIcon)picIcon.Tag);
        }

        private Image ConvertMessageBoxIconToImage(MessageBoxIcon icon)
        {
            Icon iconImage = null;
            Images.IconSize iconSize = (Images.IconSize)LogicalToDeviceUnits(32);
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    iconImage = Images.GetDevelopmentIcon(14, iconSize);
                    break;
                case MessageBoxIcon.Question:
                    iconImage = Images.GetDevelopmentIcon(12, iconSize);
                    break;
                case MessageBoxIcon.Exclamation:
                    iconImage = Images.GetDevelopmentIcon(13, iconSize);
                    break;
                case MessageBoxIcon.Information:
                    iconImage = Images.GetDevelopmentIcon(11, iconSize);
                    break;
                case MessageBox.MessageBoxIconGear:
                    iconImage = Images.GetAdminMenuItem(18, iconSize);
                    break;
            }
            return iconImage != null ? iconImage.ToBitmap() : null;
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
            this.pnlButtonContainer = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.pnlButtonContainer.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtonContainer.Controls.Add(this.pnlButtons);
            this.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonContainer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtonContainer.Location = new System.Drawing.Point(0, 81);
            this.pnlButtonContainer.Name = "pnlButtonContainer";
            this.pnlButtonContainer.Size = new System.Drawing.Size(434, 40);
            this.pnlButtonContainer.TabIndex = 0;
            this.pnlButtonContainer.Resize += new System.EventHandler(this.pnlButtonContainer_Resize);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(112, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(200, 24);
            this.pnlButtons.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.txtMessage);
            this.pnlContent.Controls.Add(this.picIcon);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(10);
            this.pnlContent.Size = new System.Drawing.Size(434, 81);
            this.pnlContent.TabIndex = 2;
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(48, 10);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(376, 61);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.TabStop = false;
            this.txtMessage.Text = "1\r\n2\r\n3\r\n4";
            // 
            // picIcon
            // 
            this.picIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.picIcon.Location = new System.Drawing.Point(10, 10);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(38, 61);
            this.picIcon.TabIndex = 2;
            this.picIcon.TabStop = false;
            // 
            // frmMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(434, 121);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlButtonContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmMessageBox";
            this.pnlButtonContainer.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion



		#endregion

		#region Methods

		private void pnlButtonContainer_Resize(object sender, System.EventArgs e)
		{
			pnlButtons.Left = (this.Width - pnlButtons.Width) / 2;
		}

		private void b_Click(object sender, EventArgs e)
		{
			_result = Convert.ToString(((Button)sender).Tag);
			this.DialogResult = DialogResult.OK;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the custom dialog result.
		/// </summary>
		public string CustomDialogResult
		{
			get
			{
				return _result;
			}
		}

		#endregion


	}
}
