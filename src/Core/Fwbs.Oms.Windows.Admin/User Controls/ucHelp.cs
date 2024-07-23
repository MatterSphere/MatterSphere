using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmHelp.
    /// </summary>
    public class ucHelp : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel pnlHelp;
		private System.Windows.Forms.Label labTitle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		public FWBS.OMS.UI.Windows.ucSearchControl Index;

		private int _helpid = -1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnExpand;
		private string _helpsearch = "";
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.RichTextBox rcHelp;
		private System.Windows.Forms.Panel panel1;
		private frmMain _parentmain = null;

		public ucHelp()
		{
			InitializeComponent();
		}
			
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public frmMain MainParent
		{
			get
			{
				return _parentmain;
			}
			set
			{
				_parentmain = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HelpSearch
		{
			get
			{
				return _helpsearch;
			}
			set
			{
				_helpsearch = value;
				FWBS.Common.KeyValueCollection kv = new FWBS.Common.KeyValueCollection();
				kv.Add("search",_helpsearch);
				Index.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Help),null,kv);
				Index.Search();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int HelpID
		{
			get
			{
				return _helpid;
			}
			set
			{
				_helpid = value;
				FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists("DSHELP");
				FWBS.Common.KeyValueCollection kv = new FWBS.Common.KeyValueCollection();
				kv.Add("helpid",_helpid);
				dl.ChangeParameters(kv);
				DataTable dt = dl.Run(false,false) as DataTable;
				if (dt.Rows.Count > 0)
				{
					labTitle.Text = Convert.ToString(dt.Rows[0]["Title"]).Trim();
					rcHelp.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(Convert.ToString(dt.Rows[0]["RTF"]));
				}
				else
				{
					btnExpand.Text = ">>>";
					btnExpand_Click(this,EventArgs.Empty);
				}
			}
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlHelp = new System.Windows.Forms.Panel();
            this.labTitle = new System.Windows.Forms.Label();
            this.Index = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExpand = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rcHelp = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnlHelp.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHelp
            // 
            this.pnlHelp.BackColor = System.Drawing.SystemColors.Desktop;
            this.pnlHelp.Controls.Add(this.labTitle);
            this.pnlHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHelp.Location = new System.Drawing.Point(0, 0);
            this.pnlHelp.Name = "pnlHelp";
            this.pnlHelp.Padding = new System.Windows.Forms.Padding(8);
            this.pnlHelp.Size = new System.Drawing.Size(540, 42);
            this.pnlHelp.TabIndex = 1;
            // 
            // labTitle
            // 
            this.labTitle.AutoSize = true;
            this.labTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTitle.Font = new System.Drawing.Font("Tahoma", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTitle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labTitle.Location = new System.Drawing.Point(8, 8);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(65, 23);
            this.labTitle.TabIndex = 0;
            this.labTitle.Text = "Index";
            this.labTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Index
            // 
            this.Index.BackColor = System.Drawing.Color.White;
            this.Index.BackGroundColor = System.Drawing.Color.White;
            this.Index.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Index.DoubleClickAction = "None";
            this.Index.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Index.Location = new System.Drawing.Point(2, 2);
            this.Index.Name = "Index";
            this.Index.NavCommandPanel = null;
            this.Index.Padding = new System.Windows.Forms.Padding(18, 5, 5, 5);
            this.Index.RefreshOnEnquiryFormRefreshEvent = false;
            this.Index.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.Index.SearchListCode = "";
            this.Index.SearchListType = "";
            this.Index.Size = new System.Drawing.Size(8, 429);
            this.Index.TabIndex = 3;
            this.Index.ToBeRefreshed = false;
            this.Index.TypeSelectorVisible = false;
            this.Index.ItemSelected += new System.EventHandler(this.Index_ItemSelected);
            this.Index.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.Index_ItemHover);
            this.Index.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this.Index_SearchCompleted);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnExpand);
            this.panel2.Controls.Add(this.Index);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(14, 435);
            this.panel2.TabIndex = 4;
            // 
            // btnExpand
            // 
            this.btnExpand.BackColor = System.Drawing.Color.White;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.Location = new System.Drawing.Point(-1, -1);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(14, 69);
            this.btnExpand.TabIndex = 4;
            this.btnExpand.Text = ">>>";
            this.btnExpand.UseVisualStyleBackColor = false;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rcHelp);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 42);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(540, 435);
            this.panel3.TabIndex = 5;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // rcHelp
            // 
            this.rcHelp.BackColor = System.Drawing.Color.White;
            this.rcHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rcHelp.BulletIndent = 5;
            this.rcHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rcHelp.Location = new System.Drawing.Point(19, 6);
            this.rcHelp.Name = "rcHelp";
            this.rcHelp.ReadOnly = true;
            this.rcHelp.ShowSelectionMargin = true;
            this.rcHelp.Size = new System.Drawing.Size(521, 429);
            this.rcHelp.TabIndex = 1;
            this.rcHelp.Text = "";
            this.rcHelp.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rcHelp_LinkClicked);
            this.rcHelp.Enter += new System.EventHandler(this.rcHelp_Enter);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(19, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 6);
            this.panel1.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Window;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(19, 435);
            this.panel4.TabIndex = 0;
            // 
            // ucHelp
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnlHelp);
            this.Name = "ucHelp";
            this.Size = new System.Drawing.Size(540, 477);
            this.pnlHelp.ResumeLayout(false);
            this.pnlHelp.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void rcHelp_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void btnExpand_Click(object sender, System.EventArgs e)
		{
			if (btnExpand.Text == ">>>")
			{
				panel2.Width = 250;
				btnExpand.Text = "<<<";
			}
			else
			{
				panel2.Width = btnExpand.Width;
				btnExpand.Text = ">>>";
			}
		}

		private void rcHelp_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			if (e.LinkText.StartsWith("http://action?"))
			{
				DataView dv = _parentmain.MenuDataTable.DefaultView;
				dv.RowFilter="admnuID = " + e.LinkText.Substring(14);
				if (dv.Count > 0)
				{
					DataRowView drv = dv[0];
					string caption = FWBS.OMS.Session.CurrentSession.Terminology.Parse(Convert.ToString(drv["menudesc"]),true);
					int index = Convert.ToInt32(drv["admnuImageIndex"]);
					MenuEventArgs lvi;
					if (Convert.ToString(drv["admnuID"]) == "")
						lvi = new MenuEventArgs(Convert.ToString(drv["admnuID"]), Convert.ToString(drv["admnuCode"]), caption, index,Convert.ToBoolean(drv["admnuIncFav"]),true,Convert.ToInt32(drv["admnuID"]),Convert.ToString(drv["admnuRoles"]));
					else
						lvi = new MenuEventArgs(Convert.ToString(drv["admnuSearchListCode"]), Convert.ToString(drv["admnuCode"]), caption, index,Convert.ToBoolean(drv["admnuIncFav"]),false,Convert.ToInt32(drv["admnuID"]),Convert.ToString(drv["admnuRoles"]));
					_parentmain.frmMain_MenuActioned(this,lvi);
					foreach(Form frm in _parentmain.MdiChildren)
					{
						if (frm != this.ParentForm)
						{
							frm.Location = new Point(0,0);
							frm.Size = new Size(((_parentmain.Width / 3) * 2)-5,_parentmain.pnlHeight.Height-5);
							frm.WindowState = FormWindowState.Normal;
						}
						else
						{
							frm.Location = new Point(((_parentmain.Width / 3) * 2)-5,0);
							frm.Size = new Size((_parentmain.Width / 3)-10,_parentmain.pnlHeight.Height-5);
							frm.WindowState = FormWindowState.Normal;
						}
					}
				}
			}
			if (e.LinkText.StartsWith("http://help?"))
			{
				this.HelpID = Convert.ToInt32(e.LinkText.Substring(12));
			}
			if (e.LinkText.StartsWith("http://bookmark?"))
			{
				int indexToText = rcHelp.Find(e.LinkText.Substring(16) + " ");
				rcHelp.SelectionStart = indexToText;
				rcHelp.SelectionLength =0;
			}		
		}

		private void Index_SearchCompleted(object sender, FWBS.OMS.UI.Windows.SearchCompletedEventArgs e)
		{
			if (Index.DataTable.Rows.Count == 1)
			{
				Index.SelectRowItem();
			}
			else
			{
				btnExpand.Text = ">>>";
				btnExpand_Click(this,EventArgs.Empty);
			}
		}

		private void Index_ItemHover(object sender, FWBS.OMS.UI.Windows.SearchItemHoverEventArgs e)
		{
			labTitle.Text = Convert.ToString(Index.ReturnValues["Title"].Value).Trim();					
			rcHelp.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(Convert.ToString(Index.ReturnValues["RTF"].Value));	
		}

		private void Index_ItemSelected(object sender, System.EventArgs e)
		{
			btnExpand.Text = "<<<";
			btnExpand_Click(sender,e);
		}

		private void rcHelp_Enter(object sender, System.EventArgs e)
		{
			btnExpand.Text = "<<<";
			btnExpand_Click(sender,e);
		}

		private void panel3_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}
	}
}
