using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eToolbars.
    /// </summary>
    public class eToolbars : System.Windows.Forms.ToolBar, ISupportRightToLeft
	{
		#region Fields
		private omsImageLists _omsimagelists = omsImageLists.None;
		private omsImageLists _omspnlimagelists = omsImageLists.None;
		private ImageList _pnlimagelist = null;
		private string _buttons = "";
		private ucNavCommands _navcommands = null;
		private System.Windows.Forms.Panel pnlLineDark;
		private System.Windows.Forms.Panel pnlLineLight;
		private DataTable _text = null;
		private DataTable _pnltext = null;
		private ContextMenu _contextmenu = null;
		private OMSToolBarButton _selectedbutton = null;
		private bool _online = false;
        private System.Collections.Generic.Dictionary<string, OMSToolBarButton> buttons = new System.Collections.Generic.Dictionary<string, OMSToolBarButton>();

        #endregion

        #region Events
        [Category("Behavior")]
		public event OMSToolBarButtonClickEventHandler OMSButtonClick;
		#endregion

		#region Contructors
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public eToolbars()
		{
			base.Appearance = ToolBarAppearance.Flat;
			base.TextAlign = ToolBarTextAlign.Right;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    RemovePanelButtons();
                    if (_pnlimagelist != null)
                    {
                        if (_navcommands != null && _navcommands.ImageList != base.ImageList)
                        {
                            _pnlimagelist.Dispose();
                            _navcommands.ImageList = null;
                        }

                        _pnlimagelist = null;
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlLineDark = new System.Windows.Forms.Panel();
            this.pnlLineLight = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlLineDark
            // 
            this.pnlLineDark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlLineDark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLineDark.Location = new System.Drawing.Point(0, 38);
            this.pnlLineDark.Name = "pnlLineDark";
            this.pnlLineDark.Size = new System.Drawing.Size(100, 1);
            this.pnlLineDark.TabIndex = 13;
            this.pnlLineDark.Visible = false;
            // 
            // pnlLineLight
            // 
            this.pnlLineLight.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlLineLight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLineLight.Location = new System.Drawing.Point(0, 39);
            this.pnlLineLight.Name = "pnlLineLight";
            this.pnlLineLight.Size = new System.Drawing.Size(100, 1);
            this.pnlLineLight.TabIndex = 14;
            this.pnlLineLight.Visible = false;
            // 
            // eToolbars
            // 
            this.Controls.Add(this.pnlLineDark);
            this.Controls.Add(this.pnlLineLight);
            this.Divider = false;
            this.Size = new System.Drawing.Size(100, 40);
            this.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.eToolbars_ButtonClick);
            this.ParentChanged += new System.EventHandler(this.eToolbars_ParentChanged);
            this.ResumeLayout(false);

		}
        #endregion

        #region Native Code

        private const ushort DDARROW_WIDTH = 15;
        private const uint TBIF_SIZE = 0x00000040;

        private const int TB_SETBUTTONINFOW = 0x0440;
        private const int TB_SETBUTTONINFOA = 0x0442;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct TBBUTTONINFO
        {
            public uint cbSize;
            public uint dwMask;
            public int idCommand;
            public int iImage;
            public byte fsState;
            public byte fsStyle;
            public ushort cx;
            public IntPtr lParam;
            public IntPtr pszText;
            public int cchTest;
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == TB_SETBUTTONINFOW || m.Msg == TB_SETBUTTONINFOA) && DropDownArrows && (DeviceDpi != 96))
            {
                TBBUTTONINFO tbButtonInfo = (TBBUTTONINFO)m.GetLParam(typeof(TBBUTTONINFO));
                if ((tbButtonInfo.dwMask & TBIF_SIZE) != 0)
                {
                    ToolBarButton button = Buttons[m.WParam.ToInt32()];
                    if (button.Style == ToolBarButtonStyle.DropDownButton)
                    {
                        tbButtonInfo.cx -= DDARROW_WIDTH-1;
                        tbButtonInfo.cx += (ushort)LogicalToDeviceUnits(DDARROW_WIDTH+1);
                        Marshal.StructureToPtr(tbButtonInfo, m.LParam, true);
                    }
                }
            }

            base.WndProc(ref m);
        }

		#endregion

		#region Properties

		[Browsable(false)]
		public ContextMenu ContextMenuOutput
		{
			get
			{
				if (_contextmenu == null)
					_contextmenu = new ContextMenu();
				
                _contextmenu.MenuItems.Clear();
                
                foreach (OMSToolBarButton omstb in buttons.Values)
                {
                    
                    omstb.CreateContextMenu();

                    IconMenuItem mnuitem = omstb.ContextMenuItem;

                    if (mnuitem != null)
                    {
                        _contextmenu.MenuItems.Add(mnuitem);
                    }
                }
				return _contextmenu;
			}
		}

		[Category("Appearance")]
		public bool TopDivider
		{
			get
			{
				return this.Divider;
			}
			set
			{
			}
		}

		[Category("Appearance")]
		public bool BottomDivider
		{
			get
			{
				return pnlLineDark.Visible;
			}
			set
			{
				pnlLineLight.Visible = value;
				pnlLineDark.Visible = value;
			}
		}
		
		[Category("Panel")]
		public ucNavCommands NavCommandPanel
		{
			get
			{
				return _navcommands;
			}
			set
			{
				_navcommands = value;
                if (this.Parent != null)
                {
                    this.Refresh();
                }
			}
		}

		[Category("Panel")]
		[DefaultValue(omsImageLists.None)]
		public omsImageLists PanelImageLists
		{
			get
			{
				return _omspnlimagelists;
			}
			set
			{
				if (_omspnlimagelists != value)
				{
					switch (value)
					{
						case omsImageLists.AdminMenu16:
						{
							this.PanelImageList = Images.AdminMenu16();
							break;
						}
						case omsImageLists.AdminMenu32:
						{
							this.PanelImageList = Images.AdminMenu32();
							break;
						}
						case omsImageLists.Arrows:
						{
							this.PanelImageList = Images.Arrows;
							break;
						}
                        case omsImageLists.PlusMinus:
                        {
                            this.PanelImageList = Images.PlusMinus;
                            break;
                        }
						case omsImageLists.CoolButtons16:
						{
							this.PanelImageList = Images.CoolButtons16();
							break;
						}
						case omsImageLists.CoolButtons24:
						{
                            this.PanelImageList = Images.GetCoolButtons24();
							break;
						}
						case omsImageLists.Developments16:
						{
							this.PanelImageList = Images.Developments();
							break;
						}
						case omsImageLists.Entities16:
						{
							this.PanelImageList = Images.Entities();
							break;
						}
						case omsImageLists.Entities32:
						{
							this.PanelImageList = Images.Entities32();
							break;
						}
						case omsImageLists.imgFolderForms16:
						{
                            this.PanelImageList = Images.GetFolderFormsIcons(Images.IconSize.Size16);
							break;
						}
						case omsImageLists.imgFolderForms32:
						{
                            this.PanelImageList = Images.GetFolderFormsIcons(Images.IconSize.Size32);
							break;
						}
						case omsImageLists.None:
						{
							this.PanelImageList = null;
							break;
						}
					}
				}
				_omspnlimagelists = value;
			}
		}

		[Category("Panel")]
		[DefaultValue(null)]
		public ImageList PanelImageList
		{
			get
			{
				return _pnlimagelist;
			}
			set
			{
				_pnlimagelist = value;
				if (this.Parent != null) this.Refresh();
			}

		}

		[Editor(typeof(ToolBarButtonEditor),typeof(UITypeEditor))]
		[Category("Data")]
		public string ButtonsXML
		{
			get
			{
				return _buttons;
			}
			set
			{
				_buttons = value;
				if (this.Parent != null) this.Refresh();
			}
		}

		[Browsable(false)]
		public new System.Windows.Forms.ToolBar.ToolBarButtonCollection Buttons
		{
			get
			{
				return base.Buttons;
			}
		}
		
		[Category("Buttons")]
		[DefaultValue(omsImageLists.None)]
		public omsImageLists ImageLists
		{
			get
			{
				return _omsimagelists;
			}
			set
			{
				if (_omsimagelists != value)
				{
                    this.ImageList = GetOmsImageList(value);
                    _omsimagelists = value;
                }
			}
		}

        private ImageList GetOmsImageList(omsImageLists value)
        {
            ImageList imageList = null;
            switch (value)
            {
                case omsImageLists.AdminMenu16:
                {
                    imageList = Images.GetAdminMenuList((Images.IconSize)LogicalToDeviceUnits(16));
                    break;
                }
                case omsImageLists.AdminMenu32:
                {
                    imageList = Images.GetAdminMenuList((Images.IconSize)LogicalToDeviceUnits(32));
                    break;
                }
                case omsImageLists.Arrows:
                {
                    imageList = Images.Arrows;
                    if (DeviceDpi != 96) imageList.ImageSize = LogicalToDeviceUnits(imageList.ImageSize);
                    break;
                }
                case omsImageLists.PlusMinus:
                {
                    imageList = Images.PlusMinus;
                    if (DeviceDpi != 96) imageList.ImageSize = LogicalToDeviceUnits(imageList.ImageSize);
                    break;
                }
                case omsImageLists.CoolButtons16:
                {
                    imageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
                    break;
                }
                case omsImageLists.CoolButtons24:
                {
                    imageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(24));
                    break;
                }
                case omsImageLists.Developments16:
                {
                    imageList = Images.GetDevelopmentList((Images.IconSize)LogicalToDeviceUnits(16));
                    break;
                }
                case omsImageLists.Entities16:
                {
                    imageList = Images.GetEntitiesList((Images.IconSize)LogicalToDeviceUnits(16));
                    break;
                }
                case omsImageLists.Entities32:
                {
                    imageList = Images.GetEntitiesList((Images.IconSize)LogicalToDeviceUnits(32));
                    break;
                }
                case omsImageLists.imgFolderForms16:
                {
                    imageList = Images.GetFolderFormsIcons((Images.IconSize)LogicalToDeviceUnits(16));
                    break;
                }
                case omsImageLists.imgFolderForms32:
                {
                    imageList = Images.GetFolderFormsIcons((Images.IconSize)LogicalToDeviceUnits(32));
                    break;
                }
                case omsImageLists.None:
                {
                    imageList = null;
                    break;
                }
            }
            return imageList;
        }

		[Category("Buttons")]
		[DefaultValue(null)]
		public new ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}
			set
			{
				base.ImageList = value;
				if (this.Parent != null) this.Refresh();
			}

		}

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);

            // Workaround that forces menu update
            foreach (OMSToolBarButton omstb in buttons.Values)
            {
                if (omstb.Style == ToolBarButtonStyle.DropDownButton && omstb.DropDownMenu != null)
                {
                    var menuItems = omstb.DropDownMenu.MenuItems;
                    menuItems.Remove(menuItems.Add(string.Empty));
                }
            }

            if (_contextmenu != null)
            {
                var menuItems = _contextmenu.MenuItems;
                menuItems.Remove(menuItems.Add(string.Empty));
            }
        }
        
        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            if (_omsimagelists != omsImageLists.None)
            {
                base.ImageList = GetOmsImageList(_omsimagelists);
            }
            
            base.OnDpiChangedAfterParent(e);
        }

		[DefaultValue(ToolBarAppearance.Flat)]
		public new ToolBarAppearance Appearance
		{
			get
			{
				return base.Appearance;
			}
			set
			{
				base.Appearance = value;
			}
		}

		[Browsable(false)]
		public new bool Divider
		{
			get
			{
				return base.Divider;
			}
			set
			{
				base.Divider = value;
			}
		}

		[DefaultValue(ToolBarTextAlign.Right)]
		public new ToolBarTextAlign TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		[Browsable(false)]
		public OMSToolBarButton ButtonClicked
		{
			get
			{
				return _selectedbutton;
			}
		}
        
        private ucActionsBlock _actionsBlock;
        [Browsable(false)]
        public ucActionsBlock ActionsBlock
        {
            get
            {
                if (_actionsBlock == null)
                {
                    _actionsBlock = new ucActionsBlock(this);
                }

                return _actionsBlock;
            }
            set { _actionsBlock = value; }
        }
        #endregion

        #region Public

        public void Clear()
        {
            Buttons.Clear();
            buttons.Clear();
        }

        public OMSToolBarButton GetButton(string name)
        {
            OMSToolBarButton button = null;
            if (!string.IsNullOrEmpty(name))
            {
                buttons.TryGetValue(name.ToUpperInvariant(), out button);
            }
            return button;
        }

        internal void RefreshParent(OMSToolBarButton btn)
        {
            if (btn == null)
                return;

            if (String.IsNullOrEmpty(btn.ParentCode))
                return;

            OMSToolBarButton parentbtn = GetButton("_____" + btn.ParentCode);
            OMSToolBarButton defaultbtn = null;
            OMSToolBarButton activebtn = null;

            
            foreach (OMSToolBarButton b in buttons.Values)
            {

                if (btn.ParentCode == b.ParentCode)
                {
                    if (defaultbtn == null)
                        defaultbtn = b;

                    if (b.Enabled && b.Visible)
                    {
                        activebtn = b;
                        break;
                    }

                }
            }

            if (parentbtn != null)
            {

                if (activebtn == null)
                {
                    parentbtn.ImageIndex = defaultbtn.ImageIndex;
                    parentbtn.Name = defaultbtn.Name;
                    parentbtn.Text = defaultbtn.Text;
                    parentbtn.ToolTipText = defaultbtn.ToolTipText;
                    parentbtn.Enabled = false;
                }
                else
                {
                    parentbtn.ImageIndex = activebtn.ImageIndex;
                    parentbtn.Name = activebtn.Name;
                    parentbtn.Text = activebtn.Text;
                    parentbtn.ToolTipText = activebtn.ToolTipText;
                    parentbtn.Visible = true;
                    parentbtn.Enabled = true;
                }

            }
            
        }

        public void AddButton(OMSToolBarButton button)
        {
            if (button == null)
                throw new ArgumentNullException("button");

            OMSToolBarButton parent = null;

            if (!String.IsNullOrEmpty(button.ParentCode))
            {
                string parentname = "_____" + button.ParentCode.ToUpperInvariant();

                if (buttons.ContainsKey(parentname))
                    parent = (OMSToolBarButton)buttons[parentname];

                else
                {
                    parent = new OMSToolBarButton(this);
                    parent.Name = parentname;
                    parent.Text = button.Text;
                    parent.Style = ToolBarButtonStyle.DropDownButton;
                    parent.ImageIndex = button.ImageIndex;
                    parent.DropDownMenu = new ContextMenu();
                    Buttons.Add(parent);
                    buttons.Add(parentname, parent);

                }
            }


            if (parent == null)
                Buttons.Add(button);
            else
            {
                button.CreateSubMenuItem();
                parent.DropDownMenu.MenuItems.Add(button.SubMenuItem);
            }

            int ctr = 1;
            if (String.IsNullOrEmpty(button.Name))
            {
                do
                {
                    button.Name = String.Format("BUTTON{0}", ctr);
                    ctr++;
                }while (buttons.ContainsKey(button.Name));

            }

            if (!String.IsNullOrEmpty(button.Name))
                buttons.Add(button.Name.ToUpperInvariant(), button);

            if (DeviceDpi != 96)
            {
                button.PanelButton.Scale(new System.Drawing.SizeF(DeviceDpi / 96F, DeviceDpi / 96F));
            }
        }

		protected override bool ProcessMnemonic(char charCode)
		{
			foreach (OMSToolBarButton tb in buttons.Values)
				if (IsMnemonic(charCode, tb.Text))
				{
					eToolbars_ButtonClick(this,new ToolBarButtonClickEventArgs(tb));
					return true;
				}
			return false;
		}


		public void RemovePanelButtons()
		{
			if (this.NavCommandPanel != null)
			{
				foreach (OMSToolBarButton omstb in buttons.Values)
				{
                    this.NavCommandPanel.Controls.Remove(omstb.PanelButton);
				}
				this.NavCommandPanel.Refresh();
                ActionsBlock.Clear();
            }
		}

		public void ShowPanelButtons()
		{
            if (this.NavCommandPanel!=null)
            {
                this.NavCommandPanel.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();

                foreach (OMSToolBarButton omstb in buttons.Values)
                {
                    if (omstb.PanelButtonVisible)
                        this.NavCommandPanel.Controls.Add(omstb.PanelButton);
                }
                this.NavCommandPanel.Refresh();

                ActionsBlock.Clear();
                foreach (var control in this.NavCommandPanel.Controls)
                {
                    if (control is ucNavCmdButtons)
                    {
                        ActionsBlock.AddButton(control as ucNavCmdButtons);
                    }
                }
            }
        }

		public new void Refresh()
		{
			try
			{
				if (_online == false)
				{
					if (Session.CurrentSession.IsLoggedIn)
					{
						_text = CodeLookup.GetLookups("SLBUTTON");
						_pnltext = CodeLookup.GetLookups("SLPBUTTON");
						_online = true;
					}
				}

				if (_contextmenu != null)
					this.ContextMenuOutput.MenuItems.Clear();
				FWBS.Common.ConfigSetting _buttonsxml = new FWBS.Common.ConfigSetting(this.ButtonsXML);
                ActionsBlock.Clear();
                base.Buttons.Clear();
                buttons.Clear();
				if (_navcommands != null) 
				{
					if (_pnlimagelist == null) _navcommands.ImageList = this.ImageList; else _navcommands.ImageList = _pnlimagelist; 
					Global.RemoveAndDisposeControls(_navcommands);
				}

				foreach(FWBS.Common.ConfigSettingItem itmbtn in _buttonsxml.CurrentChildItems)
				{
                    OMSToolBarButton but = new OMSToolBarButton(this);
					but.Enabled = Convert.ToBoolean(itmbtn.GetString("Enabled","true"));
					but.ImageIndex = Convert.ToInt32(itmbtn.GetString("ImageIndex","0"));
					but.Pushed = Convert.ToBoolean(itmbtn.GetString("Pushed","false"));
					but.Style = (ToolBarButtonStyle)FWBS.Common.ConvertDef.ToEnum(itmbtn.GetString("Style","PushButton"),ToolBarButtonStyle.PushButton);
					but.Tag = itmbtn.GetString("Tag","");
					GetToolTextHelp(itmbtn.GetString("Text",""),but);
					but.PanelButtonCaption = GetPanelText(itmbtn.GetString("PanelButtonCaption",""));
                    but.Visible = Convert.ToBoolean(itmbtn.GetString("Visible", "true"));
                    but.PanelButtonImageIndex = Convert.ToInt32(itmbtn.GetString("PanelButtonImageIndex", "-1"));
                    but.PanelButtonVisible = Convert.ToBoolean(itmbtn.GetString("PanelButtonVisible", "false"));
					but.Name = itmbtn.GetString("Name","");
					but.Group = itmbtn.GetString("Group","");

                    AddButton(but);

					if (_navcommands != null && but.PanelButtonVisible)
						_navcommands.Controls.Add(but.PanelButton);

                    ActionsBlock.AddButton(but);
                }
				if (_navcommands != null)
					_navcommands.Refresh();
			}
			catch
			{
			}
		}

        public void GroupVisible(string Group, bool value)
		{
			foreach (OMSToolBarButton omstb in buttons.Values)
			{
				if (omstb.Group == Group)
						omstb.Visible = value;
			}
		}

		public void GroupEnabled(string Group, bool value)
		{
            foreach (OMSToolBarButton omstb in buttons.Values)
            {
                if (omstb.Group == Group)
                    omstb.Enabled = value;
            }
		}


		#endregion

		#region Private
		private void GetToolTextHelp(string Code, OMSToolBarButton Button)
		{
			if (Code=="") return;
			try
			{
				if (_text != null)
				{
					_text.DefaultView.RowFilter = "cdCode = '" + Code.Split('¬')[0] + "'";
					if (_text.DefaultView.Count > 0)
					{
						Button.Text = Convert.ToString(_text.DefaultView[0]["cddesc"]);
						Button.ToolTipText = Convert.ToString(_text.DefaultView[0]["cdhelp"]);
					}
					else
					{
						string [] output = Code.Split('¬');
						if (output.Length == 1) Button.Text = output[0];
						if (output.Length > 1) Button.Text = output[1];
						if (output.Length > 2) Button.ToolTipText = output[2];
						if (output.Length > 1) CodeLookup.Create("SLBUTTON", output[0],Button.Text,Button.ToolTipText,CodeLookup.DefaultCulture,true,true,true);
					}
				}
				else
				{
					string [] output = Code.Split('¬');
					if (output.Length == 1) Button.Text = output[0];
					if (output.Length > 1) Button.Text = output[1];
					if (output.Length > 2) Button.ToolTipText = output[2];
				}
			}
			catch
			{
			}
		}

		private string GetPanelText(string Code)
		{
			if (Code=="") return "";
			try
			{
				if (_pnltext != null)
				{
					_pnltext.DefaultView.RowFilter = "cdCode = '" + Code.Split('¬')[0] + "'";
					if (_pnltext.DefaultView.Count > 0)
						return Convert.ToString(_pnltext.DefaultView[0]["cddesc"]);
					else
					{
						string [] output = Code.Split('¬');
						if (output.Length > 1) 
						{
							CodeLookup.Create("SLPBUTTON", output[0],output[1],"",CodeLookup.DefaultCulture,true,true,true);
							return output[1];
						}
						else
							return output[0];
					}
				}
				else
				{
					string [] output = Code.Split('¬');
					if (output.Length > 1) 
						return output[1];
					else
						return output[0];
				}
			}
			catch
			{
				return "~" + Code + "~";
			}
		}

		private void eToolbars_ParentChanged(object sender, System.EventArgs e)
		{
			if (this.Parent != null)
                Refresh();
		}

		public void OnLinkButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			OnButtonClick(e);
		}

		private void eToolbars_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			// Used in Scripting because of the limitation of the Current Event Args System
            _selectedbutton = GetButton(e.Button.Name);

            if (_selectedbutton != null)
            {

                this.OnClick(EventArgs.Empty);

                if (OMSButtonClick != null)
                    OMSButtonClick(this, new OMSToolBarButtonClickEventArgs(_selectedbutton));
            }
		}
		#endregion

        public void SetRTL(Form parentform)
        {
            Global.ConvertToolBarRTL(this);
        }
    }

	public class OMSToolBarButton : ToolBarButton
	{
		private bool _panelbuttonvisible = false;
		private ucNavCmdButtons _panelbutton = new ucNavCmdButtons();
		private int _pnlimageindex = -1;
		private bool _createcontextmenuitem = false;
		private IconMenuItem ctxmnuitm = null;
        private IconMenuItem mnuitem = null;
		private string _group;
        private eToolbars toolbar;


        public OMSToolBarButton()
        {
            _panelbutton.LinkClicked += new EventHandler(LinkClicked);

        }

        protected override void Dispose(bool disposing)
        {
            toolbar = null;

            if (disposing)
            {
                if (_panelbutton != null)
                {
                    _panelbutton.LinkClicked -= new EventHandler(LinkClicked);
                    _panelbutton.Dispose();
                    _panelbutton = null;
                }
            }
            base.Dispose(disposing);
        }

        public OMSToolBarButton(eToolbars toolbar) : this()
        {
            if (toolbar == null)
                throw new ArgumentNullException("toolbar");

            this.toolbar = toolbar;
        }

		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
                if (base.Enabled != value)
                {
                    base.Enabled = value;
                    if (ctxmnuitm != null)
                        ctxmnuitm.Enabled = value;
                    if (mnuitem != null)
                        mnuitem.Enabled = value;
                    if (_panelbutton != null)
                        _panelbutton.Enabled = value;
                    if (Parent != null)
                        Parent.RefreshParent(this);
                }
			}
		}

        public new bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;
                    if (mnuitem != null)
                        mnuitem.Visible = value;

                    Parent.RefreshParent(this);
                }
            }
        }


		public new ToolBarButtonStyle Style
		{
			get
			{
				return base.Style;
			}
			set
			{
				base.Style = value;
				if (value == ToolBarButtonStyle.Separator)
				{
					this.Text = "";
					this.ImageIndex = -1;
				}
			}
		}
		
		public string Group
		{
			get
			{
				return _group;
			}
			set
			{
				_group = value;
			}
		}


        public string ParentCode{get;set;}
        

        [DefaultValue(false)]
		public bool HasContextMenu
		{
			get
			{
				return _createcontextmenuitem;
			}
			set
			{
				_createcontextmenuitem = value;
			}
		}

        internal void CreateContextMenu()
        {
            if (ctxmnuitm != null)
            {
                ctxmnuitm.Dispose();
                ctxmnuitm = null;
            }

            if (HasContextMenu)
                ctxmnuitm = CreateMenuItem();
        }

        internal void CreateSubMenuItem()
        {
            if (mnuitem != null)
                mnuitem.Dispose();

            mnuitem = CreateMenuItem();
        }

        private IconMenuItem CreateMenuItem()
        {
            IconMenuItem mnuitem = new IconMenuItem();
            mnuitem.ImageList = ImageList;

            if (ImageIndex > -1)
                mnuitem.ImageIndex = ImageIndex;
            else
                mnuitem.ImageIndex = PanelButtonImageIndex;

            if (Text != "")
                mnuitem.Text = Text;
            else
                mnuitem.Text = PanelButtonCaption;

            mnuitem.Enabled = Enabled;
            mnuitem.ImageIndex = ImageIndex;

            mnuitem.Click += new EventHandler(LinkClicked);

            return mnuitem;
        }

		[Browsable(false)]
		public IconMenuItem MenuItem
		{
			get
			{
				return ctxmnuitm;
			}
		}

        [Browsable(false)]
        public IconMenuItem ContextMenuItem
        {
            get
            {
                return ctxmnuitm;
            }
        }

        [Browsable(false)]
        internal IconMenuItem SubMenuItem
        {
            get
            {
                return mnuitem;
            }
        }


		public bool PanelButtonVisible
		{
			get
			{
				return _panelbuttonvisible;
			}
			set
			{
                if (_panelbuttonvisible != value)
                {
                    _panelbuttonvisible = value;
                    this.PanelButton.Visible = value;
                }
                if (string.IsNullOrEmpty(PanelButtonCaption) && _panelbuttonvisible)
                {
                    _panelbuttonvisible = false;
                    this.PanelButton.Visible = false;
                }
			}
		}

		public string PanelButtonCaption
		{
			get
			{
				return this.PanelButton.Text;
			}
			set
			{
				this.PanelButton.Text = value;
				if (OMS.Session.CurrentSession.IsLoggedIn)
				{
					this.PanelButton.Text = Session.CurrentSession.Terminology.Parse(this.PanelButton.Text,true);
				}
            }
		}

		public ImageList ImageList
		{
			get
			{
				return this.Parent.ImageList;
			}
		}

        public new eToolbars Parent
        {
            get
            {
                eToolbars parent = base.Parent as eToolbars;
                if (parent == null)
                    return toolbar;
                
                return parent;
            }
        }

		public int PanelButtonImageIndex
		{
			get
			{
				return _pnlimageindex;
			}
			set
			{
				_pnlimageindex = value;
				_panelbutton.ImageIndex = value;
				if (ctxmnuitm != null)
					ctxmnuitm.ImageIndex = value;
                if (mnuitem != null)
                    mnuitem.ImageIndex = value;
			}
		}

		[Browsable(false)]
		public ucNavCmdButtons PanelButton
		{
			get
			{
				return _panelbutton;
			}

		}

		private void LinkClicked(object sender, EventArgs e)
		{           
			this.Parent.OnLinkButtonClick(this.Parent,new ToolBarButtonClickEventArgs(this));
		}
	}

	/// <summary>
	/// A delegate that describes a method that is used when a search has finished.
	/// </summary>
	public delegate void OMSToolBarButtonClickEventHandler (object sender, OMSToolBarButtonClickEventArgs e);

	
	public class OMSToolBarButtonClickEventArgs : EventArgs
	{
		OMSToolBarButton _button = null;

		public OMSToolBarButtonClickEventArgs(OMSToolBarButton button)
		{
			_button = button;
		}

		public OMSToolBarButton Button
		{
			get
			{
				return _button;
			}
		}
	}

	internal class ToolBarButtonEditor : UITypeEditor
	{
		public ToolBarButtonEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService iWFES;
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			FWBS.OMS.UI.Windows.Design.frmToolbarButtonEditor frmButtonEditor = new FWBS.OMS.UI.Windows.Design.frmToolbarButtonEditor();
            ImageList imageList = (ImageList)(TypeDescriptor.GetProperties(context.Instance)["ImageList"].GetValue(context.Instance));
            if (imageList != null) imageList.Tag = TypeDescriptor.GetProperties(context.Instance)["DeviceDpi"].GetValue(context.Instance);
            frmButtonEditor.ImageList = imageList;
            frmButtonEditor.PanelImageList = (ImageList)(TypeDescriptor.GetProperties(context.Instance)["PanelImageList"].GetValue(context.Instance));
			frmButtonEditor.ButtonsXML = Convert.ToString(value);
			iWFES.ShowDialog(frmButtonEditor);
			if (frmButtonEditor.DialogResult == DialogResult.OK)
			{
				value = frmButtonEditor.ButtonsXML;
			}
			frmButtonEditor.Dispose();
			return value;
		}
	}
}
