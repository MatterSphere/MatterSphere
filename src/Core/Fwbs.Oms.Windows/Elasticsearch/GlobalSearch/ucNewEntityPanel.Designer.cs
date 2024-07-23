namespace FWBS.OMS.UI.Elasticsearch.GlobalSearch
{
    partial class ucNewEntityPanel
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
            this.components = new System.ComponentModel.Container();
            this.Line = new System.Windows.Forms.Panel();
            this.btnNewContact = new System.Windows.Forms.Button();
            this.container = new System.Windows.Forms.Panel();
            this.btnNewClient = new System.Windows.Forms.Button();
            this.EmptySpace = new System.Windows.Forms.Panel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // Line
            // 
            this.Line.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
            this.Line.Dock = System.Windows.Forms.DockStyle.Top;
            this.Line.Location = new System.Drawing.Point(0, 0);
            this.Line.Margin = new System.Windows.Forms.Padding(0);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(600, 1);
            this.Line.TabIndex = 0;
            // 
            // btnNewContact
            // 
            this.btnNewContact.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnNewContact.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNewContact.FlatAppearance.BorderSize = 0;
            this.btnNewContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewContact.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnNewContact.ForeColor = System.Drawing.Color.White;
            this.btnNewContact.Location = new System.Drawing.Point(456, 15);
            this.resourceLookup1.SetLookup(this.btnNewContact, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnNewContact", "New Contact", ""));
            this.btnNewContact.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewContact.Name = "btnNewContact";
            this.btnNewContact.Size = new System.Drawing.Size(120, 33);
            this.btnNewContact.TabIndex = 1;
            this.btnNewContact.Text = "New Contact";
            this.btnNewContact.UseVisualStyleBackColor = false;
            this.btnNewContact.Click += new System.EventHandler(this.btnNewContact_Click);
            // 
            // container
            // 
            this.container.Controls.Add(this.btnNewClient);
            this.container.Controls.Add(this.EmptySpace);
            this.container.Controls.Add(this.btnNewContact);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.Padding = new System.Windows.Forms.Padding(0, 15, 24, 15);
            this.container.Size = new System.Drawing.Size(600, 63);
            this.container.TabIndex = 2;
            // 
            // btnNewClient
            // 
            this.btnNewClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnNewClient.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNewClient.FlatAppearance.BorderSize = 0;
            this.btnNewClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewClient.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnNewClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnNewClient.Location = new System.Drawing.Point(312, 15);
            this.resourceLookup1.SetLookup(this.btnNewClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnNewClient", "New Client", ""));
            this.btnNewClient.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewClient.Name = "btnNewClient";
            this.btnNewClient.Size = new System.Drawing.Size(120, 33);
            this.btnNewClient.TabIndex = 2;
            this.btnNewClient.Text = "New Client";
            this.btnNewClient.UseVisualStyleBackColor = false;
            this.btnNewClient.Click += new System.EventHandler(this.btnNewClient_Click);
            // 
            // Margin
            // 
            this.EmptySpace.Dock = System.Windows.Forms.DockStyle.Right;
            this.EmptySpace.Location = new System.Drawing.Point(432, 15);
            this.EmptySpace.Name = "Margin";
            this.EmptySpace.Size = new System.Drawing.Size(24, 33);
            this.EmptySpace.TabIndex = 3;
            // 
            // ucNewEntityPanel
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Line);
            this.Controls.Add(this.container);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucNewEntityPanel";
            this.Size = new System.Drawing.Size(600, 63);
            this.container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Line;
        private System.Windows.Forms.Button btnNewContact;
        private System.Windows.Forms.Panel container;
        private System.Windows.Forms.Button btnNewClient;
        private System.Windows.Forms.Panel EmptySpace;
        private Windows.ResourceLookup resourceLookup1;
    }
}
