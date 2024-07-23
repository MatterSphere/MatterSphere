namespace FWBS.OMS.UI.Elasticsearch
{
    partial class FilterTypesPopup
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
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblEntities = new System.Windows.Forms.Label();
            this.btnUnselectAll = new System.Windows.Forms.Button();
            this.lblDocFiltering = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.pnlEntities = new System.Windows.Forms.Panel();
            this.chbTask = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbPrecedent = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbNote = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbFile = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbEmail = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbDocument = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbClient = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbContact = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbAppointment = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.chbAssociate = new FWBS.OMS.UI.Elasticsearch.BlueCheckBox();
            this.pnlDocDates = new System.Windows.Forms.Panel();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblSpace = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDivider = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlEntities.SuspendLayout();
            this.pnlDocDates.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.btnSelectAll.FlatAppearance.BorderSize = 0;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Location = new System.Drawing.Point(12, 6);
            this.resourceLookup1.SetLookup(this.btnSelectAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNSELECTALL", "Select All", ""));
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(80, 25);
            this.btnSelectAll.TabIndex = 21;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lblEntities
            // 
            this.lblEntities.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblEntities.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.lblEntities, new FWBS.OMS.UI.Windows.ResourceLookupItem("ENTITIES", "Entities", ""));
            this.lblEntities.Name = "lblEntities";
            this.lblEntities.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblEntities.Size = new System.Drawing.Size(180, 24);
            this.lblEntities.TabIndex = 1;
            this.lblEntities.Text = "Entities";
            // 
            // btnUnselectAll
            // 
            this.btnUnselectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.btnUnselectAll.FlatAppearance.BorderSize = 0;
            this.btnUnselectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnselectAll.Location = new System.Drawing.Point(98, 6);
            this.resourceLookup1.SetLookup(this.btnUnselectAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNUNSELECTALL", "Unselect All", ""));
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(80, 25);
            this.btnUnselectAll.TabIndex = 22;
            this.btnUnselectAll.Text = "Unselect All";
            this.btnUnselectAll.UseVisualStyleBackColor = false;
            this.btnUnselectAll.Click += new System.EventHandler(this.btnUnselectAll_Click);
            // 
            // lblDocFiltering
            // 
            this.lblDocFiltering.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblDocFiltering.Location = new System.Drawing.Point(188, 8);
            this.resourceLookup1.SetLookup(this.lblDocFiltering, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOCFILTERING", "Document Filtering", ""));
            this.lblDocFiltering.Name = "lblDocFiltering";
            this.lblDocFiltering.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblDocFiltering.Size = new System.Drawing.Size(180, 24);
            this.lblDocFiltering.TabIndex = 13;
            this.lblDocFiltering.Text = "Document Filtering";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnSearch.Enabled = false;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(282, 6);
            this.resourceLookup1.SetLookup(this.btnSearch, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNSEARCH", "Search", ""));
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 25);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "";
            this.dtpEndDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.dtpEndDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(8, 75);
            this.dtpEndDate.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(164, 23);
            this.dtpEndDate.TabIndex = 19;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "";
            this.dtpStartDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.dtpStartDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(8, 24);
            this.dtpStartDate.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(164, 23);
            this.dtpStartDate.TabIndex = 16;
            this.dtpStartDate.Value = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // pnlEntities
            // 
            this.pnlEntities.Controls.Add(this.chbTask);
            this.pnlEntities.Controls.Add(this.chbPrecedent);
            this.pnlEntities.Controls.Add(this.chbNote);
            this.pnlEntities.Controls.Add(this.chbFile);
            this.pnlEntities.Controls.Add(this.chbEmail);
            this.pnlEntities.Controls.Add(this.chbDocument);
            this.pnlEntities.Controls.Add(this.chbClient);
            this.pnlEntities.Controls.Add(this.chbContact);
            this.pnlEntities.Controls.Add(this.chbAppointment);
            this.pnlEntities.Controls.Add(this.chbAssociate);
            this.pnlEntities.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.pnlEntities.Location = new System.Drawing.Point(8, 32);
            this.pnlEntities.Name = "pnlEntities";
            this.pnlEntities.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.pnlEntities.Size = new System.Drawing.Size(180, 240);
            this.pnlEntities.TabIndex = 2;
            // 
            // chbTask
            // 
            this.chbTask.BackColor = System.Drawing.Color.White;
            this.chbTask.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbTask.Location = new System.Drawing.Point(8, 211);
            this.resourceLookup1.SetLookup(this.chbTask, new FWBS.OMS.UI.Windows.ResourceLookupItem("Task", "Task", ""));
            this.chbTask.Name = "chbTask";
            this.chbTask.Size = new System.Drawing.Size(164, 23);
            this.chbTask.TabIndex = 12;
            this.chbTask.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Task;
            this.chbTask.Text = "Task";
            this.chbTask.UseVisualStyleBackColor = false;
            this.chbTask.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbPrecedent
            // 
            this.chbPrecedent.BackColor = System.Drawing.Color.White;
            this.chbPrecedent.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbPrecedent.Location = new System.Drawing.Point(8, 188);
            this.resourceLookup1.SetLookup(this.chbPrecedent, new FWBS.OMS.UI.Windows.ResourceLookupItem("Precedent", "Precedent", ""));
            this.chbPrecedent.Name = "chbPrecedent";
            this.chbPrecedent.Size = new System.Drawing.Size(164, 23);
            this.chbPrecedent.TabIndex = 11;
            this.chbPrecedent.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Precedent;
            this.chbPrecedent.Text = "Precedent";
            this.chbPrecedent.UseVisualStyleBackColor = false;
            this.chbPrecedent.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbNote
            // 
            this.chbNote.BackColor = System.Drawing.Color.White;
            this.chbNote.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbNote.Location = new System.Drawing.Point(8, 165);
            this.resourceLookup1.SetLookup(this.chbNote, new FWBS.OMS.UI.Windows.ResourceLookupItem("Note", "Note", ""));
            this.chbNote.Name = "chbNote";
            this.chbNote.Size = new System.Drawing.Size(164, 23);
            this.chbNote.TabIndex = 10;
            this.chbNote.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Note;
            this.chbNote.Text = "Note";
            this.chbNote.UseVisualStyleBackColor = false;
            this.chbNote.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbFile
            // 
            this.chbFile.BackColor = System.Drawing.Color.White;
            this.chbFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbFile.Location = new System.Drawing.Point(8, 142);
            this.resourceLookup1.SetLookup(this.chbFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("FILEFILTER", "%FILE%", ""));
            this.chbFile.Name = "chbFile";
            this.chbFile.Size = new System.Drawing.Size(164, 23);
            this.chbFile.TabIndex = 9;
            this.chbFile.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.File;
            this.chbFile.Text = "Matter";
            this.chbFile.UseVisualStyleBackColor = false;
            this.chbFile.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbEmail
            // 
            this.chbEmail.BackColor = System.Drawing.Color.White;
            this.chbEmail.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbEmail.Location = new System.Drawing.Point(8, 119);
            this.resourceLookup1.SetLookup(this.chbEmail, new FWBS.OMS.UI.Windows.ResourceLookupItem("Email", "Email", ""));
            this.chbEmail.Name = "chbEmail";
            this.chbEmail.Size = new System.Drawing.Size(164, 23);
            this.chbEmail.TabIndex = 8;
            this.chbEmail.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Email;
            this.chbEmail.Text = "Email";
            this.chbEmail.UseVisualStyleBackColor = false;
            this.chbEmail.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbDocument
            // 
            this.chbDocument.BackColor = System.Drawing.Color.White;
            this.chbDocument.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbDocument.Location = new System.Drawing.Point(8, 96);
            this.resourceLookup1.SetLookup(this.chbDocument, new FWBS.OMS.UI.Windows.ResourceLookupItem("Document", "Document", ""));
            this.chbDocument.Name = "chbDocument";
            this.chbDocument.Size = new System.Drawing.Size(164, 23);
            this.chbDocument.TabIndex = 7;
            this.chbDocument.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Document;
            this.chbDocument.Text = "Document";
            this.chbDocument.UseVisualStyleBackColor = false;
            this.chbDocument.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbClient
            // 
            this.chbClient.BackColor = System.Drawing.Color.White;
            this.chbClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbClient.Location = new System.Drawing.Point(8, 73);
            this.resourceLookup1.SetLookup(this.chbClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("Client", "Client", ""));
            this.chbClient.Name = "chbClient";
            this.chbClient.Size = new System.Drawing.Size(164, 23);
            this.chbClient.TabIndex = 6;
            this.chbClient.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Client;
            this.chbClient.Text = "Client";
            this.chbClient.UseVisualStyleBackColor = false;
            this.chbClient.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbContact
            // 
            this.chbContact.BackColor = System.Drawing.Color.White;
            this.chbContact.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbContact.Location = new System.Drawing.Point(8, 50);
            this.resourceLookup1.SetLookup(this.chbContact, new FWBS.OMS.UI.Windows.ResourceLookupItem("Contact", "Contact", ""));
            this.chbContact.Name = "chbContact";
            this.chbContact.Size = new System.Drawing.Size(164, 23);
            this.chbContact.TabIndex = 5;
            this.chbContact.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Contact;
            this.chbContact.Text = "Contact";
            this.chbContact.UseVisualStyleBackColor = false;
            this.chbContact.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbAppointment
            // 
            this.chbAppointment.BackColor = System.Drawing.Color.White;
            this.chbAppointment.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbAppointment.Location = new System.Drawing.Point(8, 27);
            this.resourceLookup1.SetLookup(this.chbAppointment, new FWBS.OMS.UI.Windows.ResourceLookupItem("Appointment", "Appointment", ""));
            this.chbAppointment.Name = "chbAppointment";
            this.chbAppointment.Size = new System.Drawing.Size(164, 23);
            this.chbAppointment.TabIndex = 4;
            this.chbAppointment.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Appointment;
            this.chbAppointment.Text = "Appointment";
            this.chbAppointment.UseVisualStyleBackColor = false;
            this.chbAppointment.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // chbAssociate
            // 
            this.chbAssociate.BackColor = System.Drawing.Color.White;
            this.chbAssociate.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbAssociate.Location = new System.Drawing.Point(8, 4);
            this.resourceLookup1.SetLookup(this.chbAssociate, new FWBS.OMS.UI.Windows.ResourceLookupItem("Associate", "Associate", ""));
            this.chbAssociate.Name = "chbAssociate";
            this.chbAssociate.Size = new System.Drawing.Size(164, 23);
            this.chbAssociate.TabIndex = 3;
            this.chbAssociate.Tag = FWBS.Common.Elasticsearch.EntityTypeEnum.Associate;
            this.chbAssociate.Text = "Associate";
            this.chbAssociate.UseVisualStyleBackColor = false;
            this.chbAssociate.CheckedChanged += new System.EventHandler(this.chbEntity_CheckedChanged);
            // 
            // pnlDocDates
            // 
            this.pnlDocDates.Controls.Add(this.dtpEndDate);
            this.pnlDocDates.Controls.Add(this.lblEndDate);
            this.pnlDocDates.Controls.Add(this.lblSpace);
            this.pnlDocDates.Controls.Add(this.dtpStartDate);
            this.pnlDocDates.Controls.Add(this.lblStartDate);
            this.pnlDocDates.Enabled = false;
            this.pnlDocDates.Location = new System.Drawing.Point(188, 32);
            this.pnlDocDates.Name = "pnlDocDates";
            this.pnlDocDates.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.pnlDocDates.Size = new System.Drawing.Size(180, 240);
            this.pnlDocDates.TabIndex = 14;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEndDate.Location = new System.Drawing.Point(8, 55);
            this.resourceLookup1.SetLookup(this.lblEndDate, new FWBS.OMS.UI.Windows.ResourceLookupItem("ENDDATE", "End Date", ""));
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(164, 20);
            this.lblEndDate.TabIndex = 18;
            this.lblEndDate.Text = "End Date";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSpace
            // 
            this.lblSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSpace.Location = new System.Drawing.Point(8, 47);
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Size = new System.Drawing.Size(164, 8);
            this.lblSpace.TabIndex = 17;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStartDate.Location = new System.Drawing.Point(8, 4);
            this.resourceLookup1.SetLookup(this.lblStartDate, new FWBS.OMS.UI.Windows.ResourceLookupItem("STARTDATE", "Start Date", ""));
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(164, 20);
            this.lblStartDate.TabIndex = 15;
            this.lblStartDate.Text = "Start Date";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnSelectAll);
            this.pnlButtons.Controls.Add(this.btnUnselectAll);
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnSearch);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 272);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(376, 44);
            this.pnlButtons.TabIndex = 20;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(196, 6);
            this.resourceLookup1.SetLookup(this.btnClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("CLOSE", "Close", ""));
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 25);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDivider
            // 
            this.lblDivider.AutoSize = true;
            this.lblDivider.Location = new System.Drawing.Point(188, 300);
            this.lblDivider.Name = "lblDivider";
            this.lblDivider.Size = new System.Drawing.Size(0, 15);
            this.lblDivider.TabIndex = 0;
            // 
            // FilterTypesPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblDivider);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblEntities);
            this.Controls.Add(this.pnlEntities);
            this.Controls.Add(this.lblDocFiltering);
            this.Controls.Add(this.pnlDocDates);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.MinimumSize = new System.Drawing.Size(120, 155);
            this.Name = "FilterTypesPopup";
            this.Size = new System.Drawing.Size(376, 316);
            this.pnlEntities.ResumeLayout(false);
            this.pnlDocDates.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDivider;
        private System.Windows.Forms.Label lblEntities;
        private System.Windows.Forms.Panel pnlEntities;
        private System.Windows.Forms.Panel pnlButtons;
        private BlueCheckBox chbAssociate;
        private BlueCheckBox chbAppointment;
        private BlueCheckBox chbFile;
        private BlueCheckBox chbClient;
        private BlueCheckBox chbContact;
        private BlueCheckBox chbDocument;
        private BlueCheckBox chbEmail;
        private BlueCheckBox chbPrecedent;
        private BlueCheckBox chbNote;
        private BlueCheckBox chbTask;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnUnselectAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblDocFiltering;
        private System.Windows.Forms.Panel pnlDocDates;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblSpace;
        private Windows.ResourceLookup resourceLookup1;
    }
}
