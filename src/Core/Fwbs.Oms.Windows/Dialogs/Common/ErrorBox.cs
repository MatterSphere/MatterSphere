using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FWBS.OMS.Data.Exceptions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom message box that displays exception / error information, stack tracing.
    /// OMS exceptions will also display a help index.
    /// </summary>
    internal sealed class frmErrorBox : BaseForm
	{
		#region Fields 

		/// <summary>
		/// Extra error information.
		/// </summary>
		private System.Windows.Forms.TextBox txtErrors;
		/// <summary>
		/// Help activator button.
		/// </summary>
		private System.Windows.Forms.Button cmdHelp;
		/// <summary>
		/// OK and close button.
		/// </summary>
		private System.Windows.Forms.Button cmdOK;
		/// <summary>
		/// Advanced information toggle button.
		/// </summary>
		private System.Windows.Forms.Button cmdAdvanced;

		/// <summary>
		/// The help provider used on the errors.
		/// </summary>
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Panel pnlMessage;
		private System.Windows.Forms.PictureBox picError;
		private System.Windows.Forms.PictureBox picPermission;
        private ResourceLookup resourceLookup1;
        private IContainer components;
        private Label lblMessage;
        private Panel pnlText;
        private Panel panel6;
        private Panel pnlImage;
        private Panel panel3;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyMessageToolStripMenuItem;
        private Exception _exception;
        private ToolStripMenuItem EmailToolStripMenuItem;

		/// <summary>
		/// Minimum height of this form, used for expanding the advanced information.
		/// </summary>
		private int minHeight = 0;
        /// <summary>
        /// Maximum height of this form, used for expanding the advanced information.
        /// </summary>
        private int maxHeight = 0;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new error box.
		/// </summary>
		internal frmErrorBox() : base()
		{
			InitializeComponent();
            MinimumSize = Size;
            minHeight = this.ClientSize.Height;
            maxHeight = minHeight + LogicalToDeviceUnits(100);
			cmdAdvanced.Tag = true;

		}

		/// <summary>
		/// Creates a new error box, passing the exception to be shown.
		/// </summary>
		/// <param name="exception">Exception to interigate.</param>
		internal frmErrorBox(Exception exception) : this(exception,null)
		{
		}
		
		/// <summary>
		/// Creates a new error box, passing the exception to be shown.
		/// </summary>
		/// <param name="exception">Exception to interigate.</param>
		internal frmErrorBox(Exception exception, ArrayList extratrace) : this()
		{
			picPermission.Visible = false;

			System.Text.StringBuilder msg = new System.Text.StringBuilder();
			this.Text = FWBS.OMS.Global.ApplicationName;
		
			cmdHelp.Visible = false;

			if (exception != null)
			{
                _exception = exception;
                //If the exception is an OMS exception then display the help button.
				if (exception is OMSException)
				{
					if (exception is Security.SecurityException)
					{
						picPermission.Visible = true;
						picError.Visible = false;
					}
					else
					{
						picPermission.Visible = false;
						picError.Visible = true;
					}
				}
				else if (exception is System.Reflection.TargetInvocationException)
				{
					exception = exception.InnerException;
				}
				else if (exception is DataException)
				{
				}
				else
				{
					cmdOK.Left = (this.Width - cmdOK.Width) / 2;
				}

				cmdOK.Left = (this.Width - cmdOK.Width) / 2;

				//Display the message.
				lblMessage.Text = exception.Message;
                minHeight = this.ClientSize.Height;
                maxHeight = minHeight + LogicalToDeviceUnits(100);

				Exception ex = exception.InnerException;
				
				//If an inner exception is available then display the advanced button
				//and its stack trace and any exception information.
				//if (ex == null)

				while (ex != null)
				{
					msg.Append(Environment.NewLine);
					msg.Append(ex.Message);
					msg.Append(Environment.NewLine);
					ex = ex.InnerException;
				}
				
				msg.Append(Environment.NewLine);
				msg.Append(exception.StackTrace ?? exception.InnerException?.StackTrace);
				msg.Append(Environment.NewLine);
				if (extratrace != null)
				{
					msg.Append(Environment.NewLine + "OMS Type Trace Info" + Environment.NewLine+ Environment.NewLine);
					foreach (string et in extratrace)
					{
						msg.Append(et + Environment.NewLine);
					}
				}

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


		}

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            float scaleFactor = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
            Size minSize = MinimumSize;
            MinimumSize = Size.Empty;
            base.OnDpiChanged(e);
            minHeight = Convert.ToInt32(minHeight * scaleFactor);
            maxHeight = Convert.ToInt32(maxHeight * scaleFactor);
            MinimumSize = new Size(Convert.ToInt32(minSize.Width * scaleFactor), Convert.ToInt32(minSize.Height * scaleFactor));
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorBox));
            this.cmdOK = new System.Windows.Forms.Button();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdAdvanced = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.pnlText = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EmailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picError = new System.Windows.Forms.PictureBox();
            this.picPermission = new System.Windows.Forms.PictureBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.pnlText.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlImage.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPermission)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Location = new System.Drawing.Point(162, 8);
            this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdOK", "OK", ""));
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // txtErrors
            // 
            this.txtErrors.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrors.Location = new System.Drawing.Point(0, 82);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrors.Size = new System.Drawing.Size(480, 0);
            this.txtErrors.TabIndex = 1;
            this.txtErrors.Visible = false;
            this.txtErrors.WordWrap = false;
            // 
            // cmdHelp
            // 
            this.cmdHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdHelp.Location = new System.Drawing.Point(244, 8);
            this.resourceLookup1.SetLookup(this.cmdHelp, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdHelp", "Help", ""));
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 24);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.VisibleChanged += new System.EventHandler(this.frmErrorBox_Resize);
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // cmdAdvanced
            // 
            this.cmdAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdAdvanced.Location = new System.Drawing.Point(394, 8);
            this.resourceLookup1.SetLookup(this.cmdAdvanced, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdAdvanced", "Advanced", ""));
            this.cmdAdvanced.Name = "cmdAdvanced";
            this.cmdAdvanced.Size = new System.Drawing.Size(75, 24);
            this.cmdAdvanced.TabIndex = 4;
            this.cmdAdvanced.Text = "Advanced";
            this.cmdAdvanced.Click += new System.EventHandler(this.cmdAdvanced_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.cmdAdvanced);
            this.pnlButtons.Controls.Add(this.cmdOK);
            this.pnlButtons.Controls.Add(this.cmdHelp);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(0, 81);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(480, 42);
            this.pnlButtons.TabIndex = 0;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Controls.Add(this.pnlText);
            this.pnlMessage.Controls.Add(this.pnlImage);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMessage.Location = new System.Drawing.Point(0, 0);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Padding = new System.Windows.Forms.Padding(20, 9, 10, 9);
            this.pnlMessage.Size = new System.Drawing.Size(480, 82);
            this.pnlMessage.TabIndex = 8;
            // 
            // pnlText
            // 
            this.pnlText.Controls.Add(this.lblMessage);
            this.pnlText.Controls.Add(this.panel6);
            this.pnlText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlText.Location = new System.Drawing.Point(76, 9);
            this.pnlText.Name = "pnlText";
            this.pnlText.Size = new System.Drawing.Size(394, 64);
            this.pnlText.TabIndex = 10;
            // 
            // lblMessage
            // 
            this.lblMessage.ContextMenuStrip = this.contextMenuStrip1;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(0, 17);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(394, 47);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "1\r\n2";
            this.lblMessage.TextChanged += new System.EventHandler(this.lblMessage_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMessageToolStripMenuItem,
            this.EmailToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // copyMessageToolStripMenuItem
            // 
            this.copyMessageToolStripMenuItem.Name = "copyMessageToolStripMenuItem";
            this.copyMessageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyMessageToolStripMenuItem.Text = "&Copy Message";
            this.copyMessageToolStripMenuItem.Click += new System.EventHandler(this.copyMessageDiagnosticInformationToolStripMenuItem_Click);
            // 
            // EmailToolStripMenuItem
            // 
            this.EmailToolStripMenuItem.Name = "EmailToolStripMenuItem";
            this.EmailToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.EmailToolStripMenuItem.Text = "&Email Message";
            this.EmailToolStripMenuItem.Visible = false;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(394, 17);
            this.panel6.TabIndex = 0;
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.panel3);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlImage.Location = new System.Drawing.Point(20, 9);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(56, 64);
            this.pnlImage.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.picError);
            this.panel3.Controls.Add(this.picPermission);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(12);
            this.panel3.Size = new System.Drawing.Size(56, 63);
            this.panel3.TabIndex = 10;
            // 
            // picError
            // 
            this.picError.Dock = System.Windows.Forms.DockStyle.Left;
            this.picError.Image = global::FWBS.OMS.UI.Properties.Resources.ErrorIcon;
            this.picError.Location = new System.Drawing.Point(44, 12);
            this.picError.Name = "picError";
            this.picError.Size = new System.Drawing.Size(32, 39);
            this.picError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picError.TabIndex = 5;
            this.picError.TabStop = false;
            // 
            // picPermission
            // 
            this.picPermission.Dock = System.Windows.Forms.DockStyle.Left;
            this.picPermission.Image = ((System.Drawing.Image)(resources.GetObject("picPermission.Image")));
            this.picPermission.Location = new System.Drawing.Point(12, 12);
            this.picPermission.Name = "picPermission";
            this.picPermission.Size = new System.Drawing.Size(32, 39);
            this.picPermission.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPermission.TabIndex = 7;
            this.picPermission.TabStop = false;
            // 
            // frmErrorBox
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cmdOK;
            this.ClientSize = new System.Drawing.Size(480, 123);
            this.ControlBox = false;
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlMessage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmErrorBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ErrorBox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmErrorBox_Load);
            this.RightToLeftChanged += new System.EventHandler(this.ErrorBox_RightToLeftChanged);
            this.Resize += new System.EventHandler(this.frmErrorBox_Resize);
            this.pnlButtons.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            this.pnlText.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlImage.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPermission)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

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
                this.ClientSize = new Size(this.ClientSize.Width, maxHeight);
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


		/// <summary>
		/// Captures the help button click to show help information using the OMSExceptions
		/// help id.  This will link to a website help subject or built in help files.
		/// </summary>
		/// <param name="exception">Exception object to validate.</param>
		private void cmdHelp_Click(object sender, System.EventArgs e)
		{
			if (cmdHelp.Tag != null)
			{
				if (cmdHelp.Tag.ToString() == "")
				{
					MessageBox.ShowInformation("NOHELPAVAILABLE","No Help available for this message!",null);
				}
				else
				{
					try
					{
						System.Diagnostics.Process.Start(cmdHelp.Tag.ToString());
					}
					catch
					{
						MessageBox.ShowInformation("NOHELPAVAILABLE","No Help available for this message!",null);
					}
				}
			}
		}

		/// <summary>
		/// Captures the RightToLeft property changed event.
		/// </summary>
		/// <param name="sender">This form that calls the method.</param>
		/// <param name="e">Empty event arguments.</param>
		private void ErrorBox_RightToLeftChanged(object sender, System.EventArgs e)
		{
			Global.RightToLeftFormConverter(this);
		}

		#endregion

		private void frmErrorBox_Resize(object sender, System.EventArgs e)
		{
			if (cmdHelp.Visible)
			{
                int padding = LogicalToDeviceUnits(4);
                cmdOK.Left = ((pnlButtons.Width - cmdOK.Width) / 2) - (cmdOK.Width / 2) - padding;
				cmdHelp.Left = ((pnlButtons.Width - cmdHelp.Width) / 2) + (cmdHelp.Width / 2) + padding;
			}
			else
			{
				cmdOK.Left = ((pnlButtons.Width - cmdOK.Width) / 2);
			}
		}

        private void lblMessage_TextChanged(object sender, System.EventArgs e)
        {
            using (Graphics g = Graphics.FromHwnd(lblMessage.Handle))
            {
                SizeF sizeMsg = g.MeasureString(lblMessage.Text, lblMessage.Font, lblMessage.Width);
                sizeMsg.Height += (lblMessage.Font.GetHeight() * 3);

                Size sizeWnd = new Size(Convert.ToInt32(sizeMsg.Width) + pnlImage.Right + this.DockPadding.Left + this.DockPadding.Right,
                    Convert.ToInt32(sizeMsg.Height) + pnlButtons.Height + this.DockPadding.Top + this.DockPadding.Bottom);

                if (sizeWnd.Height > this.ClientSize.Height && sizeWnd.Width > this.ClientSize.Width)
                {
                    pnlMessage.Size = Size.Round(sizeMsg);
                    this.ClientSize = sizeWnd;
                }
                else if (sizeWnd.Width > this.ClientSize.Width && sizeWnd.Height < this.ClientSize.Height)
                {
                    pnlMessage.Size = Size.Round(sizeMsg);
                    this.ClientSize = new Size(sizeWnd.Width, this.ClientSize.Height);
                }
                else if (sizeWnd.Width < this.ClientSize.Width && sizeWnd.Height > this.ClientSize.Height)
                {
                    pnlMessage.Size = Size.Round(sizeMsg);
                    this.ClientSize = new Size(this.ClientSize.Width, sizeWnd.Height);
                }
            }
        }

		private void frmErrorBox_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (Session.CurrentSession.Resources != null)
                {
                    this.copyMessageToolStripMenuItem.Text = Session.CurrentSession.Resources.GetResource("CopyMessage", "&Copy Message", "").Text;
                    this.EmailToolStripMenuItem.Text = Session.CurrentSession.Resources.GetResource("EmailMessage", "&Email Message", "").Text;
                }
            }
            catch { }

            cmdAdvanced.Visible = Session.CurrentSession.IsInDebug;

		}

        private void cmdOK_Click(object sender, EventArgs e)
        {

        }

        private void copyMessageDiagnosticInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.ApplicationSetting aei = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Debug", "EncryptErrorInfo", "false");
            StringBuilder s = BuildErrorMessage(aei.ToBoolean());
            Clipboard.SetText(s.ToString());
        }

        private StringBuilder BuildErrorMessage(bool encrypt)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            StringBuilder s = new StringBuilder();
            StringBuilder ee = new StringBuilder();
            s.AppendLine(lblMessage.Text);
            if (cmdAdvanced.Visible)
            {
                s.AppendLine();
                s.AppendLine("-== Diagnostics Information ==-");
                s.AppendLine();
                s.AppendLine("###");
                if (encrypt == false)
                    ee = s;
                if (Session.CurrentSession.CurrentUser != null)
                {
                    ee.Append("UserName : ");
                    ee.AppendLine(Session.CurrentSession.CurrentUser.FullName);
                    ee.Append("User ID : ");
                    ee.AppendLine(Session.CurrentSession.CurrentUser.ID.ToString());
                }
                else
                {
                    ee.Append("UserName : ");
                    ee.AppendLine(Environment.UserName);
                }
                ee.Append("Date/Time : ");
                ee.AppendLine(DateTime.Now.ToString());
                ee.Append("Computer Name : ");
                ee.AppendLine(Environment.MachineName);
                ee.AppendLine("-=OMS=-");
                ee.Append("Database Version : ");
                ee.AppendLine(Session.CurrentSession.DatabaseVersion.ToString());
                ee.Append("Engine Version : ");
                ee.AppendLine(Session.CurrentSession.EngineVersion.ToString());
                ee.AppendLine();
                ee.AppendLine("-=Message=-");
                ee.AppendLine(lblMessage.Text);
                Exception ex = _exception;
                while (ex.InnerException != null)
                {
                    ee.AppendLine();
                    ee.AppendLine("-=Inner Message=-");
                    ee.AppendLine(String.Format("[{0}]", ex.InnerException.GetType().Name));
                    ee.Append(ex.InnerException.Message);
                    ex = ex.InnerException;
                }
                ee.AppendLine();
                ee.AppendLine();
                ee.AppendLine("-=Stack Trace=-");
                ee.Append(_exception.StackTrace);
                if (encrypt)
                    s.AppendLine(Convert.ToBase64String(encoding.GetBytes(ee.ToString())));
                s.AppendLine("###");
            }
            return s;
        }
    }
}
