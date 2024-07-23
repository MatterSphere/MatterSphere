namespace FWBS.OMS.UI.UserControls.ClientDetails
{
    partial class ucClientDetailsInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Email = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.Mobile = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.Fax = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.Telephone = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.Salutation = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.DefaultContact = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.ClientName = new FWBS.OMS.UI.UserControls.Common.ucLabelField();
            this.LayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 4;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Controls.Add(this.Email, 0, 2);
            this.LayoutPanel.Controls.Add(this.Mobile, 2, 1);
            this.LayoutPanel.Controls.Add(this.Fax, 1, 1);
            this.LayoutPanel.Controls.Add(this.Telephone, 0, 1);
            this.LayoutPanel.Controls.Add(this.Salutation, 2, 0);
            this.LayoutPanel.Controls.Add(this.DefaultContact, 1, 0);
            this.LayoutPanel.Controls.Add(this.ClientName, 0, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 4;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.LayoutPanel.Size = new System.Drawing.Size(500, 180);
            this.LayoutPanel.TabIndex = 0;
            // 
            // Email
            // 
            this.Email.AutoSize = true;
            this.Email.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Email.Location = new System.Drawing.Point(3, 111);
            this.Email.MinimumSize = new System.Drawing.Size(150, 50);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(150, 50);
            this.Email.TabIndex = 6;
            // 
            // Mobile
            // 
            this.Mobile.AutoSize = true;
            this.Mobile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mobile.Location = new System.Drawing.Point(315, 57);
            this.Mobile.MinimumSize = new System.Drawing.Size(150, 50);
            this.Mobile.Name = "Mobile";
            this.Mobile.Size = new System.Drawing.Size(150, 50);
            this.Mobile.TabIndex = 5;
            // 
            // Fax
            // 
            this.Fax.AutoSize = true;
            this.Fax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Fax.Location = new System.Drawing.Point(159, 57);
            this.Fax.MinimumSize = new System.Drawing.Size(150, 50);
            this.Fax.Name = "Fax";
            this.Fax.Size = new System.Drawing.Size(150, 50);
            this.Fax.TabIndex = 4;
            // 
            // Telephone
            // 
            this.Telephone.AutoSize = true;
            this.Telephone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Telephone.Location = new System.Drawing.Point(3, 57);
            this.Telephone.MinimumSize = new System.Drawing.Size(150, 50);
            this.Telephone.Name = "Telephone";
            this.Telephone.Size = new System.Drawing.Size(150, 50);
            this.Telephone.TabIndex = 3;
            // 
            // Salutation
            // 
            this.Salutation.AutoSize = true;
            this.Salutation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Salutation.Location = new System.Drawing.Point(315, 3);
            this.Salutation.MinimumSize = new System.Drawing.Size(150, 50);
            this.Salutation.Name = "Salutation";
            this.Salutation.Size = new System.Drawing.Size(150, 50);
            this.Salutation.TabIndex = 2;
            // 
            // DefaultContact
            // 
            this.DefaultContact.AutoSize = true;
            this.DefaultContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DefaultContact.Location = new System.Drawing.Point(159, 3);
            this.DefaultContact.MinimumSize = new System.Drawing.Size(150, 50);
            this.DefaultContact.Name = "DefaultContact";
            this.DefaultContact.Size = new System.Drawing.Size(150, 50);
            this.DefaultContact.TabIndex = 1;
            // 
            // ClientName
            // 
            this.ClientName.AutoSize = true;
            this.ClientName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientName.Location = new System.Drawing.Point(3, 3);
            this.ClientName.MinimumSize = new System.Drawing.Size(150, 50);
            this.ClientName.Name = "ClientName";
            this.ClientName.Size = new System.Drawing.Size(150, 50);
            this.ClientName.TabIndex = 0;
            // 
            // ucClientDetailsInfo
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.LayoutPanel);
            this.Name = "ucClientDetailsInfo";
            this.Size = new System.Drawing.Size(500, 180);
            this.LayoutPanel.ResumeLayout(false);
            this.LayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private Common.ucLabelField ClientName;
        private Common.ucLabelField Email;
        private Common.ucLabelField Mobile;
        private Common.ucLabelField Fax;
        private Common.ucLabelField Telephone;
        private Common.ucLabelField Salutation;
        private Common.ucLabelField DefaultContact;
    }
}
