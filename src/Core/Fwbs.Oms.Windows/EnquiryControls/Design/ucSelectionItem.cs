using System;
using System.ComponentModel;
using System.Data;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for ucSelectionItem.
    /// </summary>
    public class ucSelectionItem : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler Changed;
		public event EventHandler ListDoubleClick;
		
		public System.Windows.Forms.ListBox List;
		private System.Windows.Forms.Label labItem;
		private System.Windows.Forms.Splitter splItem;
		private FWBS.Common.UI.Windows.eXPPanel pnlHelp;
		private System.Windows.Forms.Label labHelpDesc;
		private System.Windows.Forms.Label labHelpCaption;
		private DataTable _codelookups = null;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucSelectionItem()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnChanged()
		{
			if (Changed!= null)
				Changed(this, EventArgs.Empty);
		}


		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnListDoubleClick()
		{
			if (ListDoubleClick!= null)
				ListDoubleClick(this, EventArgs.Empty);
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.List = new System.Windows.Forms.ListBox();
            this.labItem = new System.Windows.Forms.Label();
            this.splItem = new System.Windows.Forms.Splitter();
            this.pnlHelp = new FWBS.Common.UI.Windows.eXPPanel();
            this.labHelpDesc = new System.Windows.Forms.Label();
            this.labHelpCaption = new System.Windows.Forms.Label();
            this.pnlHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // List
            // 
            this.List.AllowDrop = true;
            this.List.Dock = System.Windows.Forms.DockStyle.Fill;
            this.List.IntegralHeight = false;
            this.List.Location = new System.Drawing.Point(0, 16);
            this.List.Name = "List";
            this.List.Size = new System.Drawing.Size(150, 49);
            this.List.TabIndex = 3;
            this.List.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
            this.List.DoubleClick += new System.EventHandler(this.List_DoubleClick);
            // 
            // labItem
            // 
            this.labItem.BackColor = System.Drawing.Color.White;
            this.labItem.Dock = System.Windows.Forms.DockStyle.Top;
            this.labItem.Location = new System.Drawing.Point(0, 0);
            this.labItem.Name = "labItem";
            this.labItem.Size = new System.Drawing.Size(150, 16);
            this.labItem.TabIndex = 2;
            this.labItem.Text = "#Selection List Heading";
            // 
            // splItem
            // 
            this.splItem.BackColor = System.Drawing.SystemColors.Control;
            this.splItem.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splItem.Location = new System.Drawing.Point(0, 65);
            this.splItem.Name = "splItem";
            this.splItem.Size = new System.Drawing.Size(150, 3);
            this.splItem.TabIndex = 10;
            this.splItem.TabStop = false;
            // 
            // pnlHelp
            // 
            this.pnlHelp.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlHelp.BorderLine = true;
            this.pnlHelp.Controls.Add(this.labHelpDesc);
            this.pnlHelp.Controls.Add(this.labHelpCaption);
            this.pnlHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHelp.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlHelp.Location = new System.Drawing.Point(0, 68);
            this.pnlHelp.Name = "pnlHelp";
            this.pnlHelp.Padding = new System.Windows.Forms.Padding(3);
            this.pnlHelp.Size = new System.Drawing.Size(150, 82);
            this.pnlHelp.TabIndex = 9;
            // 
            // labHelpDesc
            // 
            this.labHelpDesc.BackColor = System.Drawing.Color.White;
            this.labHelpDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labHelpDesc.Location = new System.Drawing.Point(3, 17);
            this.labHelpDesc.Name = "labHelpDesc";
            this.labHelpDesc.Size = new System.Drawing.Size(144, 62);
            this.labHelpDesc.TabIndex = 1;
            // 
            // labHelpCaption
            // 
            this.labHelpCaption.BackColor = System.Drawing.Color.White;
            this.labHelpCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labHelpCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labHelpCaption.Location = new System.Drawing.Point(3, 3);
            this.labHelpCaption.Name = "labHelpCaption";
            this.labHelpCaption.Size = new System.Drawing.Size(144, 14);
            this.labHelpCaption.TabIndex = 0;
            // 
            // ucSelectionItem
            // 
            this.Controls.Add(this.List);
            this.Controls.Add(this.labItem);
            this.Controls.Add(this.splItem);
            this.Controls.Add(this.pnlHelp);
            this.Name = "ucSelectionItem";
            this.pnlHelp.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		[Category("Main")]
		public string Heading
		{
			get
			{
				return labItem.Text;
			}
			set
			{
				labItem.Text = value;
			}
		}

        private string _unknownText;
        private string UnknownText
        {
            get
            {
                if (_unknownText == null)
                    _unknownText = Session.CurrentSession.Resources.GetResource("UNKNOWN", "Unknown", "").Text;
                return _unknownText;
            }
        }

		private string _codetype;

		private void List_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (List.Focused) OnChanged();
			if (!DesignMode)
			{
				if (List.DataSource == null || List.DisplayMember == "" || List.ValueMember == "")
					return;

				if (_codelookups != null && _codelookups.DefaultView != null)
				{
					try
					{
						_codelookups.DefaultView.RowFilter = string.Format("[cdcode] = '{0}'", List.SelectedValue);
						if (_codelookups.DefaultView.Count > 0)
						{
							if (List.DisplayMember == List.ValueMember)
							{
								try{labHelpCaption.Text = Session.CurrentSession.Terminology.Parse(_codelookups.DefaultView[0]["cddesc"].ToString(),true);}
								catch{labHelpCaption.Text = UnknownText;}
							}
							else
								labHelpCaption.Text = List.SelectedValue.ToString();
							try{labHelpDesc.Text = Session.CurrentSession.Terminology.Parse(_codelookups.DefaultView[0]["cdhelp"].ToString(),true);}
							catch{labHelpDesc.Text = UnknownText;}
						}
						else
						{
							labHelpDesc.Text = UnknownText;
							labHelpCaption.Text = UnknownText;
						}
					}
					catch
					{
					}
				}
			}
		}

		private void List_DoubleClick(object sender, System.EventArgs e)
		{
			OnListDoubleClick();
		}
	
		[Category("Main")]
		[DefaultValue(null)]
		public string CodeType
		{
			get
			{
				return _codetype;
			}
			set
			{
				_codetype = value;
				if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
					_codelookups = FWBS.OMS.CodeLookup.GetLookups(_codetype);
			}
		}

		[Category("Main")]
		public bool ShowHelp
		{
			get
			{
				return pnlHelp.Visible;
			}
			set
			{
				pnlHelp.Visible=value;
				splItem.Visible=value;
			}
		}

	}
}
