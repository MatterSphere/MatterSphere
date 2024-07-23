using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucVertialContactDisplay.
    /// </summary>
    public class ucVerticalContactEx : ucVerticalContact
	{
		private FWBS.Common.UI.Windows.eLabel2 txtHome;
		private FWBS.Common.UI.Windows.eLabel2 txtBusiness;
		private System.Windows.Forms.Panel pnlSeperator2;
		private System.Windows.Forms.Label txtPostCode;
		private System.Windows.Forms.Label txtLine5;
		private System.Windows.Forms.Label txtLine4;
		private System.Windows.Forms.Label txtLine3;
		private System.Windows.Forms.Label txtLine2;
		private System.Windows.Forms.Label txtLine1;
		private System.Windows.Forms.Panel pnlSeperator1;
		private System.Windows.Forms.Label txtName;
		private FWBS.Common.UI.Windows.eLabel2 labDOB;
		private FWBS.Common.UI.Windows.eLabel2 labIsClient;
		private System.Windows.Forms.Panel pnlSeperator3;

		#region Control Fields

		#endregion

		#region Fields
		#endregion

		#region Constructors & Dispose
		public ucVerticalContactEx(string Name,string Line1,string Line2,string Line3,string Line4,string Line5,string PostCode,string Home,string Business, string DOB, string Client) : this()
		{
			txtName.Text = Name;
			if (Line1 != "")
				this.AddLine1 = Line1;
			if (Line2 != "")
				this.AddLine2 = Line2;
			if (Line3 != "")
				this.AddLine3 = Line3;
			if (Line4 != "")
				this.AddLine4 = Line4;
			if (Line5 != "")
				this.AddLine5 = Line5;
			if (PostCode != "")
				this.PostCode = PostCode;
			if (Home != "")
				this.HomeTel = Home;
			if (Business != "")
				this.BusinessTel = Business;
			if (Client != "")
				this.IsClient = Client;
			if (DOB != "")
				this.DOB = DOB;
		}
		
		public ucVerticalContactEx() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}


		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.labDOB = new FWBS.Common.UI.Windows.eLabel2();
            this.labIsClient = new FWBS.Common.UI.Windows.eLabel2();
            this.pnlSeperator3 = new System.Windows.Forms.Panel();
            this.txtHome = new FWBS.Common.UI.Windows.eLabel2();
            this.txtBusiness = new FWBS.Common.UI.Windows.eLabel2();
            this.pnlSeperator2 = new System.Windows.Forms.Panel();
            this.txtPostCode = new System.Windows.Forms.Label();
            this.txtLine5 = new System.Windows.Forms.Label();
            this.txtLine4 = new System.Windows.Forms.Label();
            this.txtLine3 = new System.Windows.Forms.Label();
            this.txtLine2 = new System.Windows.Forms.Label();
            this.txtLine1 = new System.Windows.Forms.Label();
            this.pnlSeperator1 = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Window;
            this.pnlMain.Controls.Add(this.labDOB);
            this.pnlMain.Controls.Add(this.labIsClient);
            this.pnlMain.Controls.Add(this.pnlSeperator3);
            this.pnlMain.Controls.Add(this.txtHome);
            this.pnlMain.Controls.Add(this.txtBusiness);
            this.pnlMain.Controls.Add(this.pnlSeperator2);
            this.pnlMain.Controls.Add(this.txtPostCode);
            this.pnlMain.Controls.Add(this.txtLine5);
            this.pnlMain.Controls.Add(this.txtLine4);
            this.pnlMain.Controls.Add(this.txtLine3);
            this.pnlMain.Controls.Add(this.txtLine2);
            this.pnlMain.Controls.Add(this.txtLine1);
            this.pnlMain.Controls.Add(this.pnlSeperator1);
            this.pnlMain.Controls.Add(this.txtName);
            this.pnlMain.Padding = new System.Windows.Forms.Padding(2);
            this.pnlMain.Size = new System.Drawing.Size(151, 208);
            // 
            // labDOB
            // 
            this.labDOB.CaptionWidth = 50;
            this.labDOB.Dock = System.Windows.Forms.DockStyle.Top;
            this.labDOB.Format = "";
            this.labDOB.IsDirty = false;
            this.labDOB.Location = new System.Drawing.Point(2, 194);
            this.resourceLookup1.SetLookup(this.labDOB, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOB", "DOB :", "Date of Birth"));
            this.labDOB.Name = "labDOB";
            this.labDOB.ReadOnly = true;
            this.labDOB.Size = new System.Drawing.Size(147, 14);
            this.labDOB.TabIndex = 25;
            this.labDOB.Text = "DOB :";
            this.labDOB.Value = null;
            this.labDOB.Visible = false;
            // 
            // labIsClient
            // 
            this.labIsClient.CaptionWidth = 50;
            this.labIsClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.labIsClient.Format = "";
            this.labIsClient.IsDirty = false;
            this.labIsClient.Location = new System.Drawing.Point(2, 180);
            this.resourceLookup1.SetLookup(this.labIsClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("CLIENT", "%CLIENT% : ", ""));
            this.labIsClient.Name = "labIsClient";
            this.labIsClient.ReadOnly = true;
            this.labIsClient.Size = new System.Drawing.Size(147, 14);
            this.labIsClient.TabIndex = 24;
            this.labIsClient.Text = "%CLIENT% : ";
            this.labIsClient.Value = null;
            this.labIsClient.Visible = false;
            // 
            // pnlSeperator3
            // 
            this.pnlSeperator3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeperator3.Location = new System.Drawing.Point(2, 175);
            this.pnlSeperator3.Name = "pnlSeperator3";
            this.pnlSeperator3.Size = new System.Drawing.Size(147, 5);
            this.pnlSeperator3.TabIndex = 23;
            this.pnlSeperator3.Visible = false;
            // 
            // txtHome
            // 
            this.txtHome.CaptionWidth = 50;
            this.txtHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtHome.Format = "";
            this.txtHome.IsDirty = false;
            this.txtHome.Location = new System.Drawing.Point(2, 157);
            this.resourceLookup1.SetLookup(this.txtHome, new FWBS.OMS.UI.Windows.ResourceLookupItem("HOMETEL", "Tel Home : ", ""));
            this.txtHome.Name = "txtHome";
            this.txtHome.ReadOnly = true;
            this.txtHome.Size = new System.Drawing.Size(147, 18);
            this.txtHome.TabIndex = 21;
            this.txtHome.Text = "Tel Home : ";
            this.txtHome.Value = null;
            this.txtHome.Visible = false;
            // 
            // txtBusiness
            // 
            this.txtBusiness.CaptionWidth = 50;
            this.txtBusiness.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBusiness.Format = "";
            this.txtBusiness.IsDirty = false;
            this.txtBusiness.Location = new System.Drawing.Point(2, 139);
            this.resourceLookup1.SetLookup(this.txtBusiness, new FWBS.OMS.UI.Windows.ResourceLookupItem("BusinessTel", "Tel Business : ", ""));
            this.txtBusiness.Name = "txtBusiness";
            this.txtBusiness.ReadOnly = true;
            this.txtBusiness.Size = new System.Drawing.Size(147, 18);
            this.txtBusiness.TabIndex = 20;
            this.txtBusiness.Text = "Tel Business : ";
            this.txtBusiness.Value = null;
            this.txtBusiness.Visible = false;
            // 
            // pnlSeperator2
            // 
            this.pnlSeperator2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeperator2.Location = new System.Drawing.Point(2, 134);
            this.pnlSeperator2.Name = "pnlSeperator2";
            this.pnlSeperator2.Size = new System.Drawing.Size(147, 5);
            this.pnlSeperator2.TabIndex = 19;
            this.pnlSeperator2.Visible = false;
            // 
            // txtPostCode
            // 
            this.txtPostCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPostCode.Location = new System.Drawing.Point(2, 116);
            this.txtPostCode.Name = "txtPostCode";
            this.txtPostCode.Size = new System.Drawing.Size(147, 18);
            this.txtPostCode.TabIndex = 22;
            this.txtPostCode.Visible = false;
            // 
            // txtLine5
            // 
            this.txtLine5.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLine5.Location = new System.Drawing.Point(2, 98);
            this.txtLine5.Name = "txtLine5";
            this.txtLine5.Size = new System.Drawing.Size(147, 18);
            this.txtLine5.TabIndex = 18;
            this.txtLine5.UseMnemonic = false;
            this.txtLine5.Visible = false;
            // 
            // txtLine4
            // 
            this.txtLine4.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLine4.Location = new System.Drawing.Point(2, 80);
            this.txtLine4.Name = "txtLine4";
            this.txtLine4.Size = new System.Drawing.Size(147, 18);
            this.txtLine4.TabIndex = 17;
            this.txtLine4.UseMnemonic = false;
            this.txtLine4.Visible = false;
            // 
            // txtLine3
            // 
            this.txtLine3.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLine3.Location = new System.Drawing.Point(2, 62);
            this.txtLine3.Name = "txtLine3";
            this.txtLine3.Size = new System.Drawing.Size(147, 18);
            this.txtLine3.TabIndex = 16;
            this.txtLine3.UseMnemonic = false;
            this.txtLine3.Visible = false;
            // 
            // txtLine2
            // 
            this.txtLine2.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLine2.Location = new System.Drawing.Point(2, 44);
            this.txtLine2.Name = "txtLine2";
            this.txtLine2.Size = new System.Drawing.Size(147, 18);
            this.txtLine2.TabIndex = 15;
            this.txtLine2.UseMnemonic = false;
            this.txtLine2.Visible = false;
            // 
            // txtLine1
            // 
            this.txtLine1.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLine1.Location = new System.Drawing.Point(2, 26);
            this.txtLine1.Name = "txtLine1";
            this.txtLine1.Size = new System.Drawing.Size(147, 18);
            this.txtLine1.TabIndex = 13;
            this.txtLine1.UseMnemonic = false;
            this.txtLine1.Visible = false;
            // 
            // pnlSeperator1
            // 
            this.pnlSeperator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeperator1.Location = new System.Drawing.Point(2, 21);
            this.pnlSeperator1.Name = "pnlSeperator1";
            this.pnlSeperator1.Size = new System.Drawing.Size(147, 5);
            this.pnlSeperator1.TabIndex = 14;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtName.Location = new System.Drawing.Point(2, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(147, 19);
            this.txtName.TabIndex = 12;
            this.txtName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucVerticalContactEx
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Name = "ucVerticalContactEx";
            this.Size = new System.Drawing.Size(161, 218);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            txtName.Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold);
        }

		#region Properties
		[DefaultValue("")]
		public string Heading
		{
			get
			{
				return txtName.Text;
			}
			set
			{
				txtName.Text = value;
			}
		}

		[DefaultValue("")]
		public string AddLine1
		{
			get
			{
				return txtLine1.Text;
			}
			set
			{
				txtLine1.Text = value;
				txtLine1.Visible=true;
			}
		}

		[DefaultValue("")]
		public string AddLine2
		{
			get
			{
				return txtLine2.Text;
			}
			set
			{
				txtLine2.Text = value;
				txtLine2.Visible=true;
			}
		}

		[DefaultValue("")]
		public string AddLine3
		{
			get
			{
				return txtLine3.Text;
			}
			set
			{
				txtLine3.Text = value;
				txtLine3.Visible=true;
			}
		}

		[DefaultValue("")]
		public string AddLine4
		{
			get
			{
				return txtLine4.Text;
			}
			set
			{
				txtLine4.Text = value;
				txtLine4.Visible=true;
			}
		}

		[DefaultValue("")]
		public string AddLine5
		{
			get
			{
				return txtLine5.Text;
			}
			set
			{
				txtLine5.Text = value;
				txtLine5.Visible=true;
			}
		}

		[DefaultValue("")]
		public string PostCode
		{
			get
			{
				return txtPostCode.Text;
			}
			set
			{
				txtPostCode.Text = value;
				txtPostCode.Visible=true;
			}
		}

		[DefaultValue(null)]
		public string HomeTel
		{
			get
			{
				return txtHome.Value.ToString();
			}
			set
			{
				txtHome.Value = value;
				txtHome.Visible=true;
				pnlSeperator2.Visible=true;
			}
		}


		[DefaultValue(null)]
		public string BusinessTel
		{
			get
			{
				return txtBusiness.Value.ToString();
			}
			set
			{
				txtBusiness.Value = value;
				txtBusiness.Visible=true;
				pnlSeperator2.Visible=true;
			}
		}


		[DefaultValue(null)]
		public string DOB
		{
			get
			{
				return labDOB.Value.ToString();
			}
			set
			{
				labDOB.Value = value;
				labDOB.Visible=true;
				pnlSeperator3.Visible=true;
			}
		}


		[DefaultValue(null)]
		public string IsClient
		{
			get
			{
				return labIsClient.Value.ToString();
			}
			set
			{
				labIsClient.Value = value;
				labIsClient.Visible=true;
				pnlSeperator3.Visible=true;
			}
		}

		#endregion
	}
}
