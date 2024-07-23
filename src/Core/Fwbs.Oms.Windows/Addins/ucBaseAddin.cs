using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A base user control for OMS addins.
    /// </summary>
    public class ucBaseAddin : System.Windows.Forms.UserControl, Interfaces.IOMSTypeAddin
	{
		#region Fields

		/// <summary>
		/// The panel that holds navigational panels.
		/// </summary>
		protected System.Windows.Forms.Panel pnlDesign;
			
		protected FWBS.OMS.UI.Windows.ucPanelNav pnlActions;

        protected FWBS.OMS.UI.Windows.ucNavCommands navCommands;
        private IContainer components;

		/// <summary>
		/// Global panels to manipulate.
		/// </summary>
		private FWBS.OMS.UI.Windows.ucPanelNav [] _globalpanels = null;

		/// <summary>
		/// Panels to manipulate.
		/// </summary>
		private FWBS.OMS.UI.Windows.ucPanelNav [] _panels = null;
		protected FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;

		/// <summary>
		/// To Be Refreshed
		/// </summary>
		private bool _toberefresh = false;


		#endregion

		#region Constructors & Destructors

		public ucBaseAddin()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            this.components = new System.ComponentModel.Container();
            this.pnlDesign = new System.Windows.Forms.Panel();
            this.pnlActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.navCommands = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(122)))), ((int)(((byte)(215)))));
            this.pnlDesign.Controls.Add(this.pnlActions);
            this.pnlDesign.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlDesign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlDesign.Location = new System.Drawing.Point(0, 0);
            this.pnlDesign.Name = "pnlDesign";
            this.pnlDesign.Padding = new System.Windows.Forms.Padding(8);
            this.pnlDesign.Size = new System.Drawing.Size(168, 490);
            this.pnlDesign.TabIndex = 8;
            this.pnlDesign.Visible = false;
            // 
            // pnlActions
            // 
            this.pnlActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(217)))), ((int)(((byte)(238)))));
            this.pnlActions.BlendColor1 = System.Drawing.Color.Empty;
            this.pnlActions.BlendColor2 = System.Drawing.Color.Empty;
            this.pnlActions.Brightness = 70;
            this.pnlActions.Controls.Add(this.navCommands);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.ExpandedHeight = 31;
            this.pnlActions.HeaderColor = System.Drawing.Color.Empty;
            this.pnlActions.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(152, 31);
            this.pnlActions.TabIndex = 1;
            this.pnlActions.Text = "Actions";
            this.pnlActions.Visible = false;
            this.pnlActions.ModernStyle = FWBS.OMS.UI.Windows.ucPanelNav.NavStyle.NoHeader;
            // 
            // navCommands
            // 
            this.navCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navCommands.Location = new System.Drawing.Point(0, 24);
            this.navCommands.Name = "navCommands";
            this.navCommands.Padding = new System.Windows.Forms.Padding(5);
            this.navCommands.PanelBackColor = System.Drawing.Color.Empty;
            this.navCommands.Resources = FWBS.OMS.UI.Windows.omsImageLists.ToolsButton16;
            this.navCommands.Size = new System.Drawing.Size(152, 0);
            this.navCommands.TabIndex = 15;
            this.navCommands.ModernStyle = true;
            // 
            // ucBaseAddin
            // 
            this.Controls.Add(this.pnlDesign);
            this.Name = "ucBaseAddin";
            this.Size = new System.Drawing.Size(840, 490);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Events

		/// <summary>
		/// An event that gets raised when a new OMS type object needs to be opened in
		/// a navigational format on the dialog form.
		/// </summary>
		public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;

		/// <summary>
		/// Dirty Event
		/// </summary>
		public event EventHandler Dirty = null;

		#endregion

		#region IOMSTypeAddin Implementation

		/// <summary>
		/// A method that gets called when the form first loads.
		/// </summary>
		/// <param name="obj">OMS Configurable type object to use.</param>
		public virtual void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
		{
		}

		/// <summary>
		/// Allows the calling OMS dialog to connect to the addin for the configurable type object.
		/// </summary>
		/// <param name="obj">OMS Configurable type object to use.</param>
		/// <returns>A flag that tells the dialog that the connection has been successfull.</returns>
		public virtual bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
		{
			return false;
		}

        /// <summary>
        /// Updates the contents of the addin, if there is any at all.
        /// </summary>
        public virtual void UpdateItem()
        {
        }

        /// <summary>
        /// Refreshes the addin visual look and data contents.
        /// </summary>
        public virtual void RefreshItem()
        {
        }

        /// <summary>
        /// Cancels any updates.
        /// </summary>
        public virtual void CancelItem()
        {
        }

		/// <summary>
		/// Method that is called when the owning tab has been selected.
		/// </summary>
		public virtual void SelectItem()
		{
			if (ToBeRefreshed) RefreshItem();
		}

        /// <summary>
        /// Returns the UI Element to be displayed on the Tab
        /// </summary>
        public virtual Control UIElement
        {
            get
            {
                return this;
            }
        }

		/// <summary>
		/// Method used get the panels that the addin may need to use.
		/// </summary>
		/// <returns>An array of panels to add.</returns>
		[Browsable(false)]
		[DefaultValue(null)]
		public virtual FWBS.OMS.UI.Windows.ucPanelNav[] Panels 
		{

			get
			{
				if (_panels == null)
				{
					if (!DesignMode)
					{
						Hashtable navs = new Hashtable();
						int ctr = 0;
						foreach (Control ctrl in pnlDesign.Controls)
						{
							ucPanelNav pnl = ctrl as ucPanelNav;
							if (pnl != null)
							{
								if (pnl.GlobalScope == false)
								{
									navs.Add(ctr, ctrl);
									ctr++;
								}
							}
						}
						_panels = new ucPanelNav[navs.Count];
						navs.Values.CopyTo(_panels, 0);
					}
				}
				return _panels;
			}
		}

		/// <summary>
		/// Property used get the global panels that the addin may need to use.
		/// </summary>
		/// <returns>An array of panels to add.</returns>
		[Browsable(false)]
		[DefaultValue(null)]
		public virtual FWBS.OMS.UI.Windows.ucPanelNav[] GlobalPanels 
		{

			get
			{
				if (_globalpanels == null)
				{
					if (!DesignMode)
					{
						Hashtable navs = new Hashtable();
						int ctr = 0;
						foreach (Control ctrl in pnlDesign.Controls)
						{
							ucPanelNav pnl = ctrl as ucPanelNav;
							if (pnl != null)
							{
								if (pnl.GlobalScope)
								{
									navs.Add(ctr, ctrl);
									ctr++;
								}
							}
						}
						_globalpanels = new ucPanelNav[navs.Count];
						navs.Values.CopyTo(_globalpanels, 0);
					}
				}
				return _globalpanels;
			}
		}

		
		/// <summary>
		/// Gets a boolean value that indicates whether this class is holding any
		/// unsaved dirty data.
		/// </summary>
		public virtual bool IsDirty
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Refresh the Control next time it become active
		/// </summary>
		[Browsable(false)]
		public bool ToBeRefreshed
		{
			get
			{
				return _toberefresh;
			}
			set
			{
				_toberefresh = value;
			}
		}

		/// <summary>
		/// Overrides the tab text.  Return null to keep the original text.
		/// </summary>
		public virtual string AddinText 
		{
			get
			{
				return null;
			}
		}


		#endregion

		#region Event Methods

		protected internal void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
		{
			if (NewOMSTypeWindow != null)
				NewOMSTypeWindow(this, e);
            else
                Services.ShowOMSType(e.OMSObject, e.DefaultPage);
		}

        public void OnNewOMSTypeWindow(NewOMSTypeWindowEventArgs e)
        {
            this.OnNewOMSTypeWindow(this, e);
        }

		protected void OnDirty(object sender, EventArgs e)
		{
			if (Dirty != null)
				Dirty(this,EventArgs.Empty);
		}

        protected void OnDirty()
        {
            this.OnDirty(this, EventArgs.Empty);
        }

		#endregion

		#region Properties

        //WARNING: This is a bit of a bodge to get the file management sharing of panels done correctly.
        protected ucPanelNav GetPanel(string key)
        {
            ucPanelNav pnlActions = null;
            if (string.IsNullOrEmpty(key))
                return pnlActions;

            Panel pnl = CurrentDisplay?.Panels;
            if (pnl != null)
            {
                foreach (Control ctrl in pnl.Controls)
                {
                    ucPanelNav pnltemp = ctrl as ucPanelNav;
                    if (pnltemp != null)
                    {
                        //NOTE: Perhaps check that the calling assembly is also the same as the found panel, incase duplicate names in separate addins.
                        if (pnltemp.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                        {
                            pnlActions = pnltemp;
                            break;
                        }
                    }
                }
            }

            return pnlActions;
        }

		/// <summary>
		/// Gets the Main Display windows.
		/// </summary>
		protected Interfaces.IOMSTypeWindow MainWindow
		{
			get
			{
				Control parent = this.Parent;
				while (parent != null)
				{
					if (parent is Interfaces.IOMSTypeWindow)
						return (Interfaces.IOMSTypeWindow)parent;
					else
						parent = parent.Parent;
				}

				return parent as Interfaces.IOMSTypeWindow;
			}
		}


		/// <summary>
		/// Gets the Current Display windows.
		/// </summary>
		protected Interfaces.IOMSTypeDisplay CurrentDisplay
		{
			get
			{
				Control parent = this.Parent;
				while (parent != null)
				{
					if (parent is Interfaces.IOMSTypeDisplay)
						return (Interfaces.IOMSTypeDisplay)parent;
					else
						parent = parent.Parent;
				}

				return parent as Interfaces.IOMSTypeDisplay;
			}
		}

        [Browsable(false)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        #endregion
    }
	
}
