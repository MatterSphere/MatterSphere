using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucNavCommands.
    /// </summary>
    public delegate void LinkEventHandler(ucNavCmdButtons LinkButton);
	
	[Designer(typeof(FWBS.OMS.UI.Windows.Design.ucNavCmdDesigner))]
	public class ucNavCommands : ucNavPanel
	{
		#region Fields
		/// <summary>
		/// The Image List for the Command Buttons
		/// </summary>
		private ImageList _imagelist;
		/// <summary>
		/// The Image Enum for a selection of Image Lists
		/// </summary>
		private omsImageLists _omsimagelists = omsImageLists.None;
		#endregion
	
		#region Constructors
		public ucNavCommands()
		{
			/// <summary>
			/// Required for Windows.Forms Class Composition Designer support
			/// </summary>
			InitializeComponent();
			this.ControlAdded += new ControlEventHandler(MyControlsAdded);
			this.ControlRemoved += new ControlEventHandler(MyControlsRemoved);
		}

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (parent != null)
                    {
                        parent.Load -= new EventHandler(ucNavCommands_Load);
                        parent = null;
                    } 
                    
                    foreach (Control cmd in this.Controls)
                    {
                        if (cmd is ucNavCmdButtons)
                        {
                            ucNavCmdButtons n = cmd as ucNavCmdButtons;
                            n.ImageList = null;
                        }
                    }
                    if (_imagelist != null)
                        _imagelist.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ucNavCommands
			// 
			this.ParentChanged += new System.EventHandler(this.ucNavCommands_ParentChanged);
            this.TabStop = false;
		}
		#endregion

		#endregion

		#region Events
		public event LinkEventHandler LinkClicked;

		protected virtual void OnLinkClicked(object sender, EventArgs e) 
		{
			if (LinkClicked != null)
				LinkClicked((ucNavCmdButtons)sender);
		}
		#endregion
		
		#region Private
		private void MyControlsAdded(object sender, ControlEventArgs e)
		{
			e.Control.Dock = DockStyle.Bottom;
			e.Control.SendToBack();
			if (e.Control is ucNavCmdButtons)
			{
				Global.RightToLeftControlConverter(e.Control, ParentForm);
				((ucNavCmdButtons)e.Control).LinkClicked += new EventHandler(ucNavCommands_LinkClicked);
                ((ucNavCmdButtons)e.Control).ModernStyle = ModernStyle;
			}
		}

		private void MyControlsRemoved(object sender, ControlEventArgs e)
		{
			if (e.Control is ucNavCmdButtons)
			{
				((ucNavCmdButtons)e.Control).LinkClicked -= new EventHandler(ucNavCommands_LinkClicked);
			}
		}

		private void ucNavCommands_LinkClicked(object sender, System.EventArgs e)
		{
			OnLinkClicked(sender,e);
		}

        private ucPanelNav parent;

		private void ucNavCommands_ParentChanged(object sender, System.EventArgs e)
		{
            if (Parent != null)
            {
                parent = Parent as ucPanelNav;
                parent.Load += new EventHandler(ucNavCommands_Load);
            }
            else if (parent != null)
            {
                parent.Load -= new EventHandler(ucNavCommands_Load);
                parent = null;
            }
		}

		private void ucNavCommands_Load(object sender, System.EventArgs e)
		{
			Refresh();
		}
		#endregion

		#region Public
		public new void Refresh()
		{
			this.Refresh(false);
		}
			
		/// <summary>
		/// Refreshes the To Command Bar to Resize if necessary
		/// </summary>
		/// <param name="All">All including any Invisible Menu Commands</param>
		public void Refresh(bool All)
		{
			foreach (Control cmd in this.Controls)
			{
				if (cmd is ucNavCmdButtons)
					((ucNavCmdButtons)cmd).ImageList = _imagelist;
			}
			if (Parent != null)
			{
				int h =0;
				ucPanelNav n = (ucPanelNav)Parent;
				h= n.labHeader.Height + this.DockPadding.Top + this.DockPadding.Bottom + n.pnlSpace.Height;
				if (this.Controls.Count > 0)
				{
                    foreach (Control ctrl in this.Controls)
                    {
                        ucNavCmdButtons btn = ctrl as ucNavCmdButtons;
                        if (btn != null)
                        {
                            if (btn.Visible || All)
                                h = h + ctrl.Height;
                        }
                        else if (ctrl.Visible || All)
                            h = h + ctrl.Height;
                    }
				}
				else
					h = n.labHeader.Height + n.pnlSpace.Height;

				if (n.Expanded)
				{
					n.ExpandedHeight = h;
					n.Height =h;
				}
				else
				{
					n.ExpandedHeight = h;
				}
			}
		}
		#endregion

		#region Properties
		[Category("Appearance")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return _imagelist;
			}
			set
			{
				_imagelist = value;
				foreach (Control cmd in this.Controls)
				{
					if (cmd is ucNavCmdButtons)
						((ucNavCmdButtons)cmd).ImageList = value;
				}

			}
		}

		[Category("Appearance")]
		[DefaultValue(omsImageLists.None)]
		public omsImageLists Resources
		{
			get
			{
				return _omsimagelists;
			}
			set
			{
				if (_omsimagelists != value)
				{
					switch (value)
					{
						case omsImageLists.AdminMenu16:
						{
							ImageList = Images.AdminMenu16();
							break;
						}
						case omsImageLists.AdminMenu32:
						{
							ImageList = Images.AdminMenu32();
							break;
						}
						case omsImageLists.Arrows:
						{
							ImageList = Images.Arrows;
							break;
						}
                        case omsImageLists.PlusMinus:
                        {
                            ImageList = Images.PlusMinus;
                            break;
                        }
						case omsImageLists.CoolButtons16:
						{
							ImageList = Images.CoolButtons16();
							break;
						}
						case omsImageLists.CoolButtons24:
						{
                            ImageList = Images.GetCoolButtons24();
							break;
						}
						case omsImageLists.Developments16:
						{
							ImageList = Images.Developments();
							break;
						}
						case omsImageLists.Entities16:
						{
							ImageList = Images.Entities();
							break;
						}
						case omsImageLists.Entities32:
						{
							ImageList = Images.Entities32();
							break;
						}
						case omsImageLists.imgFolderForms16:
						{
                            ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size16);
							break;
						}
						case omsImageLists.imgFolderForms32:
						{
                            ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size32);
							break;
						}
						case omsImageLists.None:
						{
							ImageList = null;
							break;
						}
					}
				}
				_omsimagelists = value;
			}
		}
		#endregion
	}
}
