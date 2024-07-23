using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eLockIcon.
    /// </summary>
    public class eLockIcon : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.PictureBox picLock;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private EnquiryForm _parent;
        private Button btnProtect;
        private TableLayoutPanel tableLayoutPanel;
        private FWBS.OMS.PasswordProtectedBase _protected;

		public eLockIcon()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            if (Session.CurrentSession.AdvancedSecurity)
            {
                picLock.Visible = false;
                btnProtect.Visible = false;
            }
		}

        private void SetLockPicture(bool isLocked, bool showEnabled = true)
        {
            int imageId = 112;
            if (isLocked)
                imageId = 86;

            var image = Images.GetCoolButton(imageId, Images.IconSize.Size32);
            picLock.Image = image.ToBitmap();
            picLock.Enabled = showEnabled;

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

                if (_protected != null)
                {
                    _protected.PasswordChanged -= new EventHandler(protected_PasswordChanged);
                    _protected = null;
                }

                _parent = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eLockIcon));
            this.picLock = new System.Windows.Forms.PictureBox();
            this.btnProtect = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // picLock
            // 
            this.picLock.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picLock.Image = ((System.Drawing.Image)(resources.GetObject("picLock.Image")));
            this.picLock.Location = new System.Drawing.Point(1, 4);
            this.picLock.Margin = new System.Windows.Forms.Padding(1);
            this.picLock.Name = "picLock";
            this.picLock.Size = new System.Drawing.Size(32, 32);
            this.picLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLock.TabIndex = 0;
            this.picLock.TabStop = false;
            // 
            // btnProtect
            // 
            this.btnProtect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnProtect.AutoSize = true;
            this.btnProtect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnProtect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProtect.Location = new System.Drawing.Point(37, 7);
            this.btnProtect.Name = "btnProtect";
            this.btnProtect.Padding = new System.Windows.Forms.Padding(2);
            this.btnProtect.Size = new System.Drawing.Size(108, 26);
            this.btnProtect.TabIndex = 2;
            this.btnProtect.Text = "Password Protect";
            this.btnProtect.UseVisualStyleBackColor = true;
            this.btnProtect.Click += new System.EventHandler(this.btnProtect_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.picLock, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnProtect, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(154, 40);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // eLockIcon
            // 
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "eLockIcon";
            this.Size = new System.Drawing.Size(154, 40);
            this.Load += new System.EventHandler(this.eLockIcon_Load);
            this.ParentChanged += new System.EventHandler(this.eLockIcon_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        [Browsable(false)]
        public PasswordProtectedBase ProtectedObject
        {
            get
            {
                return _protected;
            }
            set
            {
                if (value != _protected)
                {
                    if (_protected != null)
                        _protected.PasswordChanged -= new EventHandler(protected_PasswordChanged);

                    _protected = value;
                    if (Session.CurrentSession.AdvancedSecurity == false)
                        RefreshProtection();
                }
            }
        }

		private void eLockIcon_ParentChanged(object sender, System.EventArgs e)
		{
            if (Session.CurrentSession.AdvancedSecurity == false)
            {
                if (this.Parent != null)
                {
                    if (this.Parent is EnquiryForm)
                    {
                        _parent = this.Parent as EnquiryForm;
                        if (_parent.Enquiry.Object is FWBS.OMS.PasswordProtectedBase)
                        {
                            _protected = _parent.Enquiry.Object as FWBS.OMS.PasswordProtectedBase;
                            _protected.PasswordChanged += new EventHandler(protected_PasswordChanged);
                            SetLockPicture(_protected.HasPassword);
                           
                        }
                        else
                        {
                            SetLockPicture(false, false);
                        }
                    }
                    else
                    {
                        picLock.Visible = false;
                    }
                }
                else
                {
                    if (_protected != null)
                        _protected.PasswordChanged -= new EventHandler(protected_PasswordChanged);
                    _parent = null;
                    _protected = null;
                }
            }
		}


        private void RefreshProtection()
        {
            if (_protected != null)
            {
                _protected.PasswordChanged += new EventHandler(protected_PasswordChanged);
                SetLockPicture(_protected.HasPassword);
            }
            else
            {
                SetLockPicture(false, false);

            }
        }

		private void protected_PasswordChanged(object sender, EventArgs e)
		{
            FWBS.OMS.Interfaces.IUpdateable up = _protected as FWBS.OMS.Interfaces.IUpdateable;
            if (up != null)
                up.Update();

            SetLockPicture(_protected.HasPassword);
		}

        private void btnProtect_Click(object sender, EventArgs e)
        {
            FWBS.OMS.UI.Windows.Services.ChangePassword(this, _protected);
        }

        private void eLockIcon_Load(object sender, EventArgs e)
        {
            if (Session.CurrentSession.IsConnected)
            {
                this.btnProtect.Text = Session.CurrentSession.Resources.GetResource("btnPassProtect", this.btnProtect.Text, "").Text;
            }
        }
	}
}
