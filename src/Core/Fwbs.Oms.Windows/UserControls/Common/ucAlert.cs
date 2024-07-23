using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    public class ucAlert : System.Windows.Forms.UserControl
	{
		#region Controls

		private System.Windows.Forms.Panel pnlAlert;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;
		private CloseButton btnCloseAlert;
        private Panel pnlSpace;

		#endregion

		#region Fields

		private EnquiryForm _parent = null;
        
		#endregion
        
		#region Constructors

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucAlert()
		{
			InitializeComponent();
            
            if (Session.CurrentSession.IsLoggedIn)
                this.label2.Text = Session.CurrentSession.Resources.GetResource("Alerts", "Alerts", "").Text;
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
            this.pnlAlert = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCloseAlert = new FWBS.OMS.UI.Windows.ucAlert.CloseButton();
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAlert
            // 
            this.pnlAlert.BackColor = System.Drawing.Color.White;
            this.pnlAlert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAlert.Location = new System.Drawing.Point(0, 19);
            this.pnlAlert.Name = "pnlAlert";
            this.pnlAlert.Size = new System.Drawing.Size(688, 53);
            this.pnlAlert.TabIndex = 21;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnCloseAlert);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(688, 19);
            this.panel1.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(668, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Alerts";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCloseAlert
            // 
            this.btnCloseAlert.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCloseAlert.Location = new System.Drawing.Point(670, 2);
            this.btnCloseAlert.Name = "btnCloseAlert";
            this.btnCloseAlert.Size = new System.Drawing.Size(14, 13);
            this.btnCloseAlert.TabIndex = 4;
            this.btnCloseAlert.Click += new System.EventHandler(this.btnCloseAlert_Click);
            // 
            // pnlSpace
            // 
            this.pnlSpace.BackColor = System.Drawing.Color.Transparent;
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpace.Location = new System.Drawing.Point(0, 72);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(688, 5);
            this.pnlSpace.TabIndex = 22;
            // 
            // ucAlert
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlAlert);
            this.Controls.Add(this.pnlSpace);
            this.Controls.Add(this.panel1);
            this.Name = "ucAlert";
            this.Size = new System.Drawing.Size(688, 77);
            this.VisibleChanged += new System.EventHandler(this.ucAlert_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.ucAlert_ParentChanged);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion Constructors

        #region Events

        private void btnCloseAlert_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
        }


        private void ucAlert_ParentChanged(object sender, System.EventArgs e)
        {
            if (Parent is EnquiryForm)
            {
                _parent = Parent as EnquiryForm;
                if (_parent != null && _parent.Enquiry.InDesignMode == false)
                {
                    FWBS.OMS.Interfaces.IAlert alert = _parent.Enquiry.Object as FWBS.OMS.Interfaces.IAlert;
                    if (alert != null) SetAlerts(alert.Alerts);
                }
            }
        }


        private void ucAlert_VisibleChanged(object sender, System.EventArgs e)
        {
            if (Visible)
            {
                if (pnlAlert.Controls.Count == 0)
                {
                    if (_parent != null && _parent.Enquiry.InDesignMode == true)
                        return;

                    Visible = false;
                }
            }
        }

        #endregion Events

        #region Methods

        public void SetAlerts(Alert [] alerts)
		{
		
			int ctrl_count = pnlAlert.Controls.Count;

			int height = panel1.Height;

			for (int alert_ctr = 0; alert_ctr < alerts.Length; alert_ctr++)
			{
                Control ctrl = null;
				if (ctrl_count >= (alert_ctr + 1))
				{
                    ctrl = pnlAlert.Controls[alert_ctr] as Control;			
					ctrl = SetAlert(alerts[alert_ctr], ctrl);
				}
				else
					ctrl = SetAlert(alerts[alert_ctr], null);

				height += ctrl.Height;
			}
            height += pnlSpace.Height;

			for (int ctr = ctrl_count - 1; ctr> alerts.Length - 1; ctr --)
			{
				pnlAlert.Controls.RemoveAt(ctr);
			}

			if (alerts.Length > 0)
			{
				this.Height = height;
				this.Visible = true;
			}
			else
				this.Visible = false;
		}


        private Control SetAlert(Alert alert, Control control)
        {
            Label alertLabel = PopulateAlertLabelProperties(alert, control);            

            if (!AlertLabelIsVisible(alertLabel))
            {
                return alertLabel;
            }
            
            ucAlertLabel alertLabelV2 = SetVersion2UIAlert(alert, control, alertLabel);
            return alertLabelV2;
        }

       
        private bool AlertLabelIsVisible(Label alertLabel)
        {
            return alertLabel.Visible;
        }


        private Label PopulateAlertLabelProperties(Alert alert, Control control)
        {
            Label alertLabel = (control == null)
                ? NewAlertLabel()
                : ((ucAlertLabel)control).AlertLabel;

            alertLabel = SetLabelFormatting(alert, alertLabel);
            alertLabel.Visible = SetAlertLabelVisibility(alert);
            alertLabel.Text = alert.Message;
            return alertLabel;
        }


        private static Label NewAlertLabel()
        {
            Label lblAlert = new Label();
            lblAlert.Dock = System.Windows.Forms.DockStyle.Top;
            lblAlert.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            lblAlert.ForeColor = System.Drawing.Color.White;
            lblAlert.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lblAlert.Location = new System.Drawing.Point(0, 19);
            lblAlert.Name = "lblAlert";
            lblAlert.Size = new System.Drawing.Size(600, 40);
            lblAlert.TabIndex = 0;
            lblAlert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            return lblAlert;
        }


        private Label SetLabelFormatting(Alert alert, Label label)
        {
            IAlertFormatting alertFormat = new Version2UIAlertFormatting(alert);
            label.BackColor = alertFormat.BackColor;
            label.ForeColor = alertFormat.ForeColor;
            label.TextAlign = alertFormat.TextAlign;
            label.Dock = alertFormat.DockStyle;
            label.Font = alertFormat.Font;
            return label;
        }


        private bool SetAlertLabelVisibility(Alert alert)
        {
            return (alert.Status == Alert.AlertStatus.Off) ? false : true;
        }


        private ucAlertLabel SetVersion2UIAlert(Alert alert, Control control, Label alertLabel)
        {
            Image alertImage = AlertIconSelector.GetAlertIcon(alert);

            var alertLabelV2 = (control == null)
                                        ? new ucAlertLabel(alertLabel, alertImage)
                                        : (ucAlertLabel)control;

            alertLabelV2.AlertLabel.Text = alert.Message;
            alertLabelV2.AlertImage = alertImage;
            alertLabelV2.Dock = System.Windows.Forms.DockStyle.Top;

            if (control == null)
            {
                pnlAlert.Controls.Add(alertLabelV2, true);
            }

            return alertLabelV2;
        }


		public void RemoveAlert()
		{
			this.Visible = false;
		}

        #endregion Methods

        #region CloseButton

        private class CloseButton : Button
        {
            protected override void OnPaint(PaintEventArgs e)
            {
               base.OnPaint(e);

               bool pressed = (Control.MouseButtons == MouseButtons.Left) && RectangleToScreen(ClientRectangle).Contains(Control.MousePosition);
               ControlPaint.DrawCaptionButton(e.Graphics, ClientRectangle, CaptionButton.Close, pressed ? ButtonState.Pushed : ButtonState.Normal);
            }
        }

        #endregion CloseButton
    }
}

