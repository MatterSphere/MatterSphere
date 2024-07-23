using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Data.Exceptions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom message box that displays exception / error information, stack tracing.
    /// OMS exceptions will also display a help index.
    /// </summary>
    internal sealed class ucErrorBox : System.Windows.Forms.UserControl
	{
		#region Fields 

		/// <summary>
		/// Extra error information.
		/// </summary>
		private System.Windows.Forms.TextBox txtErrors;
		/// <summary>
		/// Advanced information toggle button.
		/// </summary>
		private System.Windows.Forms.Button cmdAdvanced;
		/// <summary>
		/// The message label.
		/// </summary>
		private System.Windows.Forms.Label lblMessage;

		/// <summary>
		/// The help provider used on the errors.
		/// </summary>
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Panel pnlMessage;
		private System.Windows.Forms.PictureBox picError;

		/// <summary>
		/// Minimum height of this form, used for expanding the advanced information.
		/// </summary>
		private int minHeight = 0;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new error box.
		/// </summary>
		public ucErrorBox() 
		{
			InitializeComponent();

            //Captures the current scale.
            minHeight = this.ClientSize.Height;
			cmdAdvanced.Tag = true;
		}

		/// <summary>
		/// Creates a new error box, passing the exception to be shown.
		/// </summary>
		/// <param name="exception">Exception to interigate.</param>
		public void SetErrorBox(Exception exception)
		{
            if (!Convert.ToBoolean(cmdAdvanced.Tag))
            {   // Shrink control to minimal height 
                cmdAdvanced_Click(this, EventArgs.Empty);
            }
		
			if (exception != null)
			{
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
				//If the exception is an OMS exception then display the help button.
				if (exception is OMSException || exception is OMSException2)
				{
					cmdAdvanced.Visible = (exception.InnerException != null);
                    picError.Visible = !(exception is Security.SecurityException);
				}
				else
				{
					cmdAdvanced.Visible = !(exception is DataException);
                    picError.Visible = true;
                }

				//Display the message.
				lblMessage.Text = exception.Message;

				Exception ex = exception.InnerException;
				
				while (ex != null)
				{
					msg.Append(Environment.NewLine);
					msg.Append(ex.Message);
					msg.Append(Environment.NewLine);
					ex = ex.InnerException;
				}
				
				msg.Append(Environment.NewLine);
				msg.Append(exception.StackTrace ?? exception.InnerException?.StackTrace);

				txtErrors.Text = msg.ToString();
			
                if (Session.CurrentSession.IsInDebug)
                {
                    try
                    {
                        // Create the source, if it does not already exist.
                        if (!EventLog.SourceExists("OMSDOTNET"))
                            EventLog.CreateEventSource("OMSDOTNET", "Application");

                        // Create an EventLog instance and assign its source.
                        using (EventLog myLog = new EventLog())
                        {
                            myLog.Source = "OMSDOTNET";
                            myLog.WriteEntry(lblMessage.Text + Environment.NewLine + "-----------------------------------------------------------------------------------------------" + txtErrors.Text, EventLogEntryType.Error);
                        }
                    }
                    catch { }
                }
			}
            else
            {
                lblMessage.Text = string.Empty;
                txtErrors.Text = string.Empty;
                cmdAdvanced.Visible = false;
                picError.Visible = false;
            }
		}

        #endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucErrorBox));
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.cmdAdvanced = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.picError = new System.Windows.Forms.PictureBox();
            this.pnlButtons.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).BeginInit();
            this.SuspendLayout();
            // 
            // txtErrors
            // 
            this.txtErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrors.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.txtErrors.Location = new System.Drawing.Point(0, 72);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrors.Size = new System.Drawing.Size(440, 0);
            this.txtErrors.TabIndex = 1;
            this.txtErrors.Visible = false;
            this.txtErrors.WordWrap = false;
            // 
            // cmdAdvanced
            // 
            this.cmdAdvanced.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdAdvanced.Location = new System.Drawing.Point(356, 8);
            this.cmdAdvanced.Name = "cmdAdvanced";
            this.cmdAdvanced.Size = new System.Drawing.Size(76, 24);
            this.cmdAdvanced.TabIndex = 4;
            this.cmdAdvanced.Text = "Advanced";
            this.cmdAdvanced.VisibleChanged += new System.EventHandler(this.cmdAdvanced_VisibleChanged);
            this.cmdAdvanced.Click += new System.EventHandler(this.cmdAdvanced_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(44, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(388, 56);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "1\r\n2\r\n3\r\n4";
            this.lblMessage.TextChanged += new System.EventHandler(this.lblMessage_TextChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.cmdAdvanced);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(0, 72);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(8);
            this.pnlButtons.Size = new System.Drawing.Size(440, 40);
            this.pnlButtons.TabIndex = 0;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Controls.Add(this.lblMessage);
            this.pnlMessage.Controls.Add(this.picError);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMessage.Location = new System.Drawing.Point(0, 0);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Padding = new System.Windows.Forms.Padding(44, 8, 8, 8);
            this.pnlMessage.Size = new System.Drawing.Size(440, 72);
            this.pnlMessage.TabIndex = 8;
            // 
            // picError
            // 
            this.picError.Image = ((System.Drawing.Image)(resources.GetObject("picError.Image")));
            this.picError.Location = new System.Drawing.Point(8, 8);
            this.picError.Name = "picError";
            this.picError.Size = new System.Drawing.Size(32, 32);
            this.picError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picError.TabIndex = 5;
            this.picError.TabStop = false;
            // 
            // ucErrorBox
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.pnlMessage);
            this.Controls.Add(this.pnlButtons);
            this.Name = "ucErrorBox";
            this.Size = new System.Drawing.Size(440, 112);
            this.pnlButtons.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (factor.Height != 1 && (specified & BoundsSpecified.Height) != 0)
            {
                minHeight = Convert.ToInt32(minHeight * factor.Height);
            }
        }

		#endregion

		#region Methods
		/// <summary>
		/// Captures the advanced button click.  This will expand or attract the form
		/// size depending on current boolean toggle of the expansion process.
		/// </summary>
		/// <param name="sender">Command button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdAdvanced_Click(object sender, System.EventArgs e)
		{
            SuspendLayout();
            if (Convert.ToBoolean(cmdAdvanced.Tag))
            {
                this.ClientSize = new Size(this.ClientSize.Width, minHeight + LogicalToDeviceUnits(100));
                txtErrors.Visible = true;
                cmdAdvanced.Tag = false;
            }
            else
            {
                this.ClientSize = new Size(this.ClientSize.Width, minHeight);
                txtErrors.Visible = false;
                cmdAdvanced.Tag = true;
            }
            ResumeLayout();
        }

        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            float factor = (Parent == null) ? 96F / DeviceDpi : 1F;
            Size messageSize = new Size(Convert.ToInt32(factor * lblMessage.PreferredSize.Width), Convert.ToInt32(factor * lblMessage.PreferredSize.Height));

            Size clientSize = new Size(messageSize.Width + pnlMessage.Padding.Left + pnlMessage.Padding.Right,
                messageSize.Height + pnlMessage.Padding.Top + pnlMessage.Padding.Bottom + pnlButtons.Height);

            this.ClientSize = new Size(Math.Max(this.ClientSize.Width, clientSize.Width),
                Math.Max(this.ClientSize.Height, clientSize.Height));

            pnlMessage.Height = this.ClientSize.Height - pnlButtons.Height;
            minHeight = this.ClientSize.Height;
        }

		private void cmdAdvanced_VisibleChanged(object sender, EventArgs e)
		{
            try
            {
                txtErrors.Visible = cmdAdvanced.Visible;
            }
            catch { }
		}

		#endregion

    }
}
