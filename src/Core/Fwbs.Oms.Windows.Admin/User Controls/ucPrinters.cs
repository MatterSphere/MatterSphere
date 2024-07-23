using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucOMSTypes.
    /// </summary>
    public class ucPrinters : ucEditBase2	
	{
		#region Fields
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.DataGridTextBoxColumn dgcDescription;
		private System.Windows.Forms.DataGridTextBoxColumn gdcLocation;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel pnlBackPrinterName;
		private System.Windows.Forms.Label labPrinterName;
		private System.Windows.Forms.Panel pnlPrinterUIObjectBack;
		private System.Windows.Forms.Panel pnlPrinterUIObject;
		private System.Windows.Forms.Panel pnlPrinterLocation;
		private System.Windows.Forms.Label labPrinterLocation;
		private System.Windows.Forms.Label labLocation;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labTray;
		private System.Windows.Forms.Label label3;
		private FWBS.OMS.Printer _currentobj = null;
		#endregion

		#region Constructors
	
		public ucPrinters()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

        public ucPrinters(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent, editparent, Params)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPrinters));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlPrinterUIObjectBack = new System.Windows.Forms.Panel();
            this.pnlPrinterUIObject = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labTray = new System.Windows.Forms.Label();
            this.labLocation = new System.Windows.Forms.Label();
            this.pnlPrinterLocation = new System.Windows.Forms.Panel();
            this.labPrinterLocation = new System.Windows.Forms.Label();
            this.pnlBackPrinterName = new System.Windows.Forms.Panel();
            this.labPrinterName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dgcDescription = new System.Windows.Forms.DataGridTextBoxColumn();
            this.gdcLocation = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.pnlPrinterUIObjectBack.SuspendLayout();
            this.pnlPrinterUIObject.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlPrinterLocation.SuspendLayout();
            this.pnlBackPrinterName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.pnlPrinterUIObjectBack);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.propertyGrid1);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Size = new System.Drawing.Size(663, 441);
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.propertyGrid1, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlPrinterUIObjectBack, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Size = new System.Drawing.Size(663, 50);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Size = new System.Drawing.Size(663, 22);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Size = new System.Drawing.Size(663, 26);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbClose
            // 
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // pnlToolbarContainer
            // 
            this.pnlToolbarContainer.Size = new System.Drawing.Size(663, 26);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(410, 50);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(253, 391);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(407, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 391);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // pnlPrinterUIObjectBack
            // 
            this.pnlPrinterUIObjectBack.Controls.Add(this.pnlPrinterUIObject);
            this.pnlPrinterUIObjectBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPrinterUIObjectBack.Location = new System.Drawing.Point(0, 50);
            this.pnlPrinterUIObjectBack.Name = "pnlPrinterUIObjectBack";
            this.pnlPrinterUIObjectBack.Size = new System.Drawing.Size(407, 391);
            this.pnlPrinterUIObjectBack.TabIndex = 3;
            this.pnlPrinterUIObjectBack.SizeChanged += new System.EventHandler(this.pnlPrinterUIObjectBack_SizeChanged);
            // 
            // pnlPrinterUIObject
            // 
            this.pnlPrinterUIObject.Controls.Add(this.label3);
            this.pnlPrinterUIObject.Controls.Add(this.panel1);
            this.pnlPrinterUIObject.Controls.Add(this.labLocation);
            this.pnlPrinterUIObject.Controls.Add(this.pnlPrinterLocation);
            this.pnlPrinterUIObject.Controls.Add(this.pnlBackPrinterName);
            this.pnlPrinterUIObject.Controls.Add(this.pictureBox1);
            this.pnlPrinterUIObject.Location = new System.Drawing.Point(24, 23);
            this.pnlPrinterUIObject.Name = "pnlPrinterUIObject";
            this.pnlPrinterUIObject.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlPrinterUIObject.Size = new System.Drawing.Size(362, 353);
            this.pnlPrinterUIObject.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 276);
            this.BresourceLookup1.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("Trays", "Trays : ", ""));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Trays : ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labTray);
            this.panel1.Location = new System.Drawing.Point(55, 275);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.panel1.Size = new System.Drawing.Size(35, 18);
            this.panel1.TabIndex = 5;
            // 
            // labTray
            // 
            this.labTray.BackColor = System.Drawing.SystemColors.Info;
            this.labTray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTray.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTray.Location = new System.Drawing.Point(0, 0);
            this.labTray.Name = "labTray";
            this.labTray.Size = new System.Drawing.Size(33, 16);
            this.labTray.TabIndex = 0;
            this.labTray.Text = "0";
            this.labTray.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(22, 328);
            this.BresourceLookup1.SetLookup(this.labLocation, new FWBS.OMS.UI.Windows.ResourceLookupItem("Location", "Location : ", ""));
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(55, 16);
            this.labLocation.TabIndex = 4;
            this.labLocation.Text = "Location : ";
            // 
            // pnlPrinterLocation
            // 
            this.pnlPrinterLocation.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlPrinterLocation.Controls.Add(this.labPrinterLocation);
            this.pnlPrinterLocation.Location = new System.Drawing.Point(80, 327);
            this.pnlPrinterLocation.Name = "pnlPrinterLocation";
            this.pnlPrinterLocation.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.pnlPrinterLocation.Size = new System.Drawing.Size(244, 18);
            this.pnlPrinterLocation.TabIndex = 3;
            // 
            // labPrinterLocation
            // 
            this.labPrinterLocation.BackColor = System.Drawing.SystemColors.Info;
            this.labPrinterLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labPrinterLocation.Location = new System.Drawing.Point(0, 0);
            this.labPrinterLocation.Name = "labPrinterLocation";
            this.labPrinterLocation.Size = new System.Drawing.Size(242, 16);
            this.labPrinterLocation.TabIndex = 0;
            this.labPrinterLocation.Text = "Printer Name";
            this.labPrinterLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlBackPrinterName
            // 
            this.pnlBackPrinterName.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlBackPrinterName.Controls.Add(this.labPrinterName);
            this.pnlBackPrinterName.Location = new System.Drawing.Point(70, 17);
            this.pnlBackPrinterName.Name = "pnlBackPrinterName";
            this.pnlBackPrinterName.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.pnlBackPrinterName.Size = new System.Drawing.Size(221, 18);
            this.pnlBackPrinterName.TabIndex = 2;
            // 
            // labPrinterName
            // 
            this.labPrinterName.BackColor = System.Drawing.SystemColors.Info;
            this.labPrinterName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labPrinterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labPrinterName.Location = new System.Drawing.Point(0, 0);
            this.labPrinterName.Name = "labPrinterName";
            this.labPrinterName.Size = new System.Drawing.Size(219, 16);
            this.labPrinterName.TabIndex = 0;
            this.labPrinterName.Text = "Printer Name";
            this.labPrinterName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(362, 343);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // dgcDescription
            // 
            this.dgcDescription.Format = "";
            this.dgcDescription.FormatInfo = null;
            this.dgcDescription.HeaderText = "Description";
            this.BresourceLookup1.SetLookup(this.dgcDescription, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
            this.dgcDescription.MappingName = "printDescription";
            this.dgcDescription.ReadOnly = true;
            this.dgcDescription.Width = 400;
            // 
            // gdcLocation
            // 
            this.gdcLocation.Format = "";
            this.gdcLocation.FormatInfo = null;
            this.gdcLocation.HeaderText = "Location";
            this.BresourceLookup1.SetLookup(this.gdcLocation, new FWBS.OMS.UI.Windows.ResourceLookupItem("Location", "Location", ""));
            this.gdcLocation.MappingName = "printLocation";
            this.gdcLocation.ReadOnly = true;
            this.gdcLocation.Width = 200;
            // 
            // ucPrinters
            // 
            this.Name = "ucPrinters";
            this.Size = new System.Drawing.Size(697, 471);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.pnlPrinterUIObjectBack.ResumeLayout(false);
            this.pnlPrinterUIObject.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlPrinterLocation.ResumeLayout(false);
            this.pnlBackPrinterName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Override

		protected override void NewData()
		{
			_currentobj = new FWBS.OMS.Printer();
			_currentobj.Dirty +=new EventHandler(OnDirty);
			propertyGrid1.SelectedObject = _currentobj;
			ShowEditor(true);
		}

		protected override string SearchListName
		{
			get
			{
				return "ADMPRINTERS";
			}
		}

        protected override void DeleteData(string Code)
        {
            Printer currentPrinter = Printer.GetPrinter(Convert.ToInt32(Code));

            if (currentPrinter.IsPrinterAssignedToUsers)
            {
                DataTable dt = currentPrinter.GetUsersTable();
                int length = Math.Min(5, dt.Rows.Count);
                bool addDots = dt.Rows.Count > 5;
                System.Text.StringBuilder assignedUsers = new System.Text.StringBuilder();

                assignedUsers.AppendLine();

                for(int n = 0; n< length; n++)
                {
                    
                    if (addDots && n == length -1)
                        assignedUsers.AppendLine(dt.Rows[n]["usrFullName"]+"...");    
                    else
                        assignedUsers.AppendLine(dt.Rows[n]["usrFullName"].ToString());  
                }

                if (MessageBox.ShowYesNoQuestion("MOVEPRINTUSRS", "The printer cannot be deleted until the following users have been reassigned: %1%Would you like to choose a new printer now?", assignedUsers.ToString()) == DialogResult.No)
                    return;

                
                FWBS.OMS.SearchEngine.SearchList sl = new FWBS.OMS.SearchEngine.SearchList("ADMPRINTSEL", null, null);
                sl.Filter("printID <> " + Code);
                sl.ApplyFilters();

                Services.Searches search = new Services.Searches(sl);
                search.AsType = false;
 
                FWBS.Common.KeyValueCollection searchRes = search.Show(this);

                if (searchRes == null || searchRes.Count <= 0)
                    return;

                int newPrinter = Convert.ToInt32(searchRes["printID"].Value);

                if (newPrinter == currentPrinter.ID)
                    return;

                currentPrinter.MoveUsersTo(newPrinter);

            }
            currentPrinter.DeleteCurrentPrinter();
            currentPrinter.Update();
        }

		protected override void LoadSingleItem(string Code)
		{
			_currentobj = FWBS.OMS.Printer.GetPrinter(Convert.ToInt32(Code));		
			_currentobj.Dirty +=new EventHandler(OnDirty);
			propertyGrid1.SelectedObject = _currentobj;
			labPrinterName.Text =  _currentobj.PrinterName;
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Printer", "Printer", ""), _currentobj.PrinterName);
			labPrinterLocation.Text = _currentobj.Location;
			labTray.Text = _currentobj.Trays.ToString();
			ShowEditor(false);
		}

		protected override bool UpdateData()
		{
            if (_currentobj.PrinterName == "")
            {
                MessageBox.ShowInformation("REQPRINTNAME", "The Printer Name is required.");
                return false;
            }
            else
            {
                _currentobj.Update();
                this.IsDirty = false;
                return true;
            }
		}

		protected override void Clone(string Code)
		{
			_currentobj = FWBS.OMS.Printer.Clone(Convert.ToInt32(Code));		
			_currentobj.Dirty +=new EventHandler(OnDirty);
			propertyGrid1.SelectedObject = _currentobj;
			labPrinterName.Text =  _currentobj.PrinterName;
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Printer", "Printer", ""), _currentobj.PrinterName);
			labPrinterLocation.Text = _currentobj.Location;
			labTray.Text = _currentobj.Trays.ToString();
			ShowEditor(true);
		}

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    base.ShowList();
                }
            }
            else
            {
                base.ShowList();
            }
        }

		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			labPrinterName.Text =  _currentobj.PrinterName;
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Printer", "Printer", ""), _currentobj.PrinterName);
			labPrinterLocation.Text = _currentobj.Location;
			labTray.Text = _currentobj.Trays.ToString();
		}

		private void pnlPrinterUIObjectBack_SizeChanged(object sender, System.EventArgs e)
		{
			Point centre = new Point((pnlPrinterUIObjectBack.Width - pnlPrinterUIObject.Width) / 2,(pnlPrinterUIObjectBack.Height - pnlPrinterUIObject.Height) / 2);
			pnlPrinterUIObject.Location = centre;
		}

		private void OnDirty(object sender, EventArgs e)
		{
			this.IsDirty=true;
		}
	}
}
