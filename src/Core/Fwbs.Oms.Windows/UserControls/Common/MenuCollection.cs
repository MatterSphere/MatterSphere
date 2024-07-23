using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.Common.UI.Windows
{
    public enum ButtonStyle {mbSmall,mbSmallToggle,mbLarge}
	/// <summary>
	/// Summary description for MenuCollection.
	/// </summary>
	[ToolboxItem(true)]
	public class MenuCollection : System.Windows.Forms.UserControl
	{
		#region Fields
        private int hightdif => LogicalToDeviceUnits(24);
		private int cheightdif => LogicalToDeviceUnits(4);
		private bool _localise = true;
		private string _buttondown = "";
		
		private ButtonStyle _buttonstyle = ButtonStyle.mbLarge;
		
		/// <summary>
		/// Pointer to the ImageList for the Icon Menu Bar and Favorites
		/// </summary>
		public ImageList _imagelist;

		/// <summary>
		/// Pointer to the Panel that will contain the Favorites
		/// </summary>
		public ContainerControl _pnlfav = null;

		/// <summary>
		/// Array of ButtonItems used to create the menu system
		/// </summary>
		/// 
		private EventArrayList _item = new EventArrayList();
		/// <summary>
		/// Sorted List to store the Headers and Buttons used in the Icon Menu Bar
		/// </summary>
		private SortedList _headers = new SortedList();
		private SortedList _buttons = new SortedList();

		/// <summary>
		/// _menus = Hashtable to store the ButtonItem agains the MenuItem
		/// </summary>
		private Hashtable _menus;

		/// <summary>
		/// activebar = When Menu in the Menu Bar is expanded
		/// </summary>
		private MenuBar activebar = null;

		/// <summary>
		/// Pointer to the MainMenu
		/// </summary>
		private System.Windows.Forms.MainMenu _mainmenu = null;

		/// <summary>
		/// String containing the ActiveBar name
		/// </summary>
		private string _activebarname;

		/// <summary>
		/// Container for the Nav Button Image Lists
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Image list for the Icon Menu Bar navigation
		/// </summary>
		public System.Windows.Forms.ImageList NavIcons;

		//
		// Events
		//
		new public event ButtonEventHandler Click;
		new public event ButtonEventHandler DoubleClick;
		#endregion

		#region Public Methods
		public MenuButton GetMenuButton(string HeaderName,string ButtonName)
		{
			return _buttons[HeaderName + "|" + ButtonName] as MenuButton;
		}

		public MenuButton GetMenuButton(string HeaderPipeButton)
		{
			return _buttons[HeaderPipeButton] as MenuButton;
		}

		public MenuButton GetMenuButton()
		{
			for(int t = 0; t < _buttons.Count; t++)
			{
				Console.WriteLine(_buttons.GetKey(t));
			}
			return _buttons[_buttondown] as MenuButton;
		}
		#endregion
		//
		// Designer Events
		//
		/// <summary>
		/// public enum ChangePropTypes{ImageListChange,MainHeaderChange}
		/// public class ChangePropEventArgs : EventArgs
		/// private delegate void PropChangedEvent(object sender, ChangePropEventArgs e);
		/// protected virtual void OnPropChanged(ChangePropEventArgs e) 
		/// private void IPropChanged(object sender, ChangePropEventArgs e)
		/// private void AddItemEvent(object sender,ChangeEventArgs e)
		/// private void IntAddItemEvent(string command, object item)
		/// </summary>
		//
		#region Designer Events
		public enum ChangePropTypes{ImageListChange,MainHeaderChange}
		private event PropChangedEvent PropChanged;
		public class ChangePropEventArgs : EventArgs 
		{  
			private ChangePropTypes _what;
			public ChangePropEventArgs(ChangePropTypes What) 
			{_what = What;}
			public ChangePropTypes What{get{return _what;}}
		}
		private delegate void PropChangedEvent(object sender, ChangePropEventArgs e);
		
		protected virtual void OnPropChanged(ChangePropEventArgs e) 
		{
			if (PropChanged != null)
				PropChanged(this, e);
		}

		private void IPropChanged(object sender, ChangePropEventArgs e)
		{
			if (e.What == ChangePropTypes.MainHeaderChange)
			{
				if (_headers.Count>0)
				{
					object _testbar = _headers[_activebarname];
					if (_testbar != null)
					{
						activebar = (MenuBar)_testbar;
						Header_Click(_testbar,EventArgs.Empty);
					}
				}
			}
			if (e.What == ChangePropTypes.ImageListChange)
			{
				if (_buttons.Count>0)
					for(int t = 0; t < _buttons.Count; t++)
					{
						object item = _buttons.GetByIndex(t);
						if (item is MenuButton)
						{
							MenuButton i = (MenuButton)item;
							i.ButtonImageList = _imagelist;
						}
					}
			}
		}

		private void AddItemEvent(object sender,ChangeEventArgs e)
		{
			IntAddItemEvent(e.What,e.Item);
		}

		public new void Refresh()
		{
			for(int t = 0; t < _buttons.Count; t++)
			{
				object item = _buttons.GetByIndex(t);
				if (item is MenuButton)
				{
					MenuButton i = (MenuButton)item;
					i.ButtonText = ProcessResStrings(i.CodeLookup,i.ButtonText);
				}
			}
			
			for(int t = 0; t < _headers.Count; t++)
			{
				object item = _headers.GetByIndex(t);
				if (item is MenuBar)
				{
					MenuBar i = (MenuBar)item;
					i.HeaderText = ProcessResStrings(i.CodeLookup,i.HeaderText);
				}
			}
		}

		private void IntAddItemEvent(string command, object item)
		{
			string resheader="";
			string rescaption="";
			if (command == "clear")
			{
				_buttons.Clear();
				_headers.Clear();
				FWBS.OMS.UI.Windows.Global.RemoveAndDisposeControls(this);
				if (_pnlfav != null)
					FWBS.OMS.UI.Windows.Global.RemoveAndDisposeControls(_pnlfav);
			}
			if (command == "add")
			{
				ButtonItems i = (ButtonItems)item;
				i.IntimageList = _imagelist;
				if (i.HeaderCaption != "" && i.VisibleInSideBar)
				{
					if (_localise)
					{
						rescaption = ProcessResStrings(i.ButtonName,i.ButtonCaption);
						resheader = ProcessResStrings(i.HeaderName, i.HeaderCaption);
					}
					else
					{
						rescaption = i.ButtonCaption;
						resheader = i.HeaderCaption;
					}
					if (_headers[i.HeaderName] == null)
					{
						MenuBar bar = new MenuBar();
						bar.Left = 0;
						bar.CodeLookup = i.HeaderName;
						bar.HeaderText = resheader;
						Controls.Add(bar, true);
						bar.Dock = DockStyle.Top;
						if (!this.DesignMode)
							bar.HeaderButtonClick += new HeaderButtonClickEventHandler(Header_Click);
						Controls.SetChildIndex(bar,0);
						MenuButton but = new MenuButton(rescaption,i.ReturnKey,i.ImageIndex,i.Include,i.VisibleInSideBar,i.ButtonName,_buttonstyle);
						but.ButtonImageList = _imagelist;
						bar.Controls.Add(but, true);
						but.Dock = DockStyle.Top;
						_buttons.Add(i.HeaderName + "|" + i.ButtonName,but);
						if (!this.DesignMode)
						{
							but.Click += new ButtonEventHandler(Button_Click);
							but.MouseMove += new MouseEventHandler(Button_MouseMove);
							but.MouseDown += new MouseEventHandler(Button_MouseDown);
							but.MouseUp += new MouseEventHandler(Button_MouseUp);
							but.DoubleClick += new ButtonEventHandler(Button_DoubleClick);
						}
						bar.Controls.SetChildIndex(but,0);
						bar.TotalButtonHeight = but.Height;
						bar.ButtonHeight = but.Height;
						_headers.Add(i.HeaderName,bar);
						bar.Height = Height - ((_headers.Count-1) * 21);
						if (_activebarname == null)
							this.ActiveBar = resheader;
						if (resheader == _activebarname)
							activebar = bar;
					}
					else
					{
						if (_buttons[i.HeaderName + "|" + i.ButtonName] == null)
						{
							MenuBar bar = (MenuBar)_headers[i.HeaderName];
							MenuButton but = new MenuButton(rescaption,i.ReturnKey,i.ImageIndex,i.Include,i.VisibleInSideBar,i.ButtonName,_buttonstyle);
							but.ButtonImageList = _imagelist;
							bar.Controls.Add(but, true);
							if (!this.DesignMode)
							{
								but.Click += new ButtonEventHandler(Button_Click);
								but.MouseMove += new MouseEventHandler(Button_MouseMove);
								but.MouseDown += new MouseEventHandler(Button_MouseDown);
								but.MouseUp += new MouseEventHandler(Button_MouseUp);
								but.DoubleClick += new ButtonEventHandler(Button_DoubleClick);
							}
							but.Dock = DockStyle.Top;
							bar.Controls.SetChildIndex(but,0);
							_buttons.Add(i.HeaderName + "|" + i.ButtonName,but);
							bar.TotalButtonHeight = bar.TotalButtonHeight + but.Height;
						}
					}
				}
			}
		}
		#endregion

		//
		// Constructor
		//
		#region Constructor
		public MenuCollection()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            this.NavIcons = FWBS.OMS.UI.Windows.ExpandCollapseIconSelector.GetExpandCollapseIcons();
            _item.Changed += new ChangedEventHandler(AddItemEvent);
			this.PropChanged += new PropChangedEvent(IPropChanged);
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
                    if (_imagelist != null)
                    {
                        _imagelist.Dispose();
                        _imagelist = null;
                    }
                    if (this.NavIcons != null)
                    {
                        this.NavIcons.Dispose();
                        this.NavIcons = null;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // MenuCollection
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "MenuCollection";
            this.Padding = new System.Windows.Forms.Padding(2, 2, 4, 1);
            this.Size = new System.Drawing.Size(96, 848);
            this.Load += new System.EventHandler(this.MenuCollection_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MenuCollection_Paint);
            this.Resize += new System.EventHandler(this.MenuCollection_Resize_1);
            this.ResumeLayout(false);

		}
		#endregion
		#endregion

		//
		// Internal Calliung Events
		//
		/// <summary>
		/// protected virtual void OnButtonClick(MenuButtonEventArgs e) 
		/// private void Menu_Click(object sender, EventArgs e)
		/// private void Button_Click(object sender, MenuButtonEventArgs e)
		/// private void Header_Click(object sender, System.EventArgs e)
		/// private void MenuCollection_Resize_1(object sender, System.EventArgs e)
		/// private void MenuCollection_Load(object sender, System.EventArgs e)
		/// private void MenuCollection_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		/// private void pnlFavoritesControlAdded(object sender, ControlEventArgs e)
		/// </summary>
		//
		#region Internal Calling Events

        /// <summary>
		/// Raise Button Click Event
		/// </summary>
		/// <param name="e">MenuButtonArgs</param>
		protected virtual void OnButtonDoubleClick(MenuButtonEventArgs e) 
		{
			if (DoubleClick != null)
				DoubleClick(this, e);
		}

		protected virtual void OnButtonClick(MenuButtonEventArgs e) 
		{
			if (_buttonstyle == ButtonStyle.mbSmallToggle)
			{
				for(int t = 0; t < _buttons.Count; t++)
				{
					object item = _buttons.GetByIndex(t);
					if (item is MenuButton)
					{
						MenuButton i = (MenuButton)item;
						if (i.ReturnKey != e.ReturnKey.ToString() && e.ButtonCaption != i.ButtonText)
							i.Toggle = false;
						else
						{
							i.Toggle = true;
							_buttondown = _activebarname + "|" + e.ButtonName;
						}
					}
				}
			}
			
			if (Click != null)
				Click(this, e);
		}

		/// <summary>
		/// Internally run by the Drop Down Menu system
		/// </summary>
		/// <param name="sender">Menu Item clicked</param>
		/// <param name="e">Empty</param>
		private void Menu_Click(object sender, EventArgs e)
		{
			ButtonItems n = (ButtonItems)_menus[sender];
			MenuButtonEventArgs ee = new MenuButtonEventArgs(n.ReturnKey.ToString(),ProcessResStrings(n.ButtonName,n.ButtonCaption),n.ImageIndex,n.Include,n.ButtonName);
			Button_Click(sender, ee);
		}

		/// <summary>
		/// Internally run when a Icon Menu Button is Clicked and raises the OnButtonClick Event
		/// </summary>
		/// <param name="sender">Icon Button Clicked</param>
		/// <param name="e">MenuButtonEventArgs</param>
		private void Button_Click(object sender, MenuButtonEventArgs e)
		{
			// Rasise the OnButtonClick Event
			OnButtonClick(e);

			// If the Favorites panel is assigned and the Button can be Inclueded in Favorites
			if (_pnlfav != null && e.IncFavorites)
			{
				FWBS.OMS.Favourites fw = new FWBS.OMS.Favourites("MENUBARFAV",e.ReturnKey.ToString());
				if (fw.Count == 0)
					fw.AddFavourite(e.ReturnKey.ToString(),e.ImageIndex.ToString(),e.ButtonCaption,e.ButtonName,"","0");
				else
				{
					Int64 n = fw.Param4(0);
					fw.Param4(0,++n);
				}
				fw.Update();
				RefreshFavorites();
			}
		}

		private void Button_DoubleClick(object sender, MenuButtonEventArgs e)
		{
			OnButtonDoubleClick(e);
		}

		private void Button_MouseDown(object sender, MouseEventArgs e)
		{
			OnMouseDown(e);
		}

		private void Button_MouseMove(object sender, MouseEventArgs e)
		{
			OnMouseMove(e);
		}
		
		private void Button_MouseUp(object sender, MouseEventArgs e)
		{
			OnMouseUp(e);
		}
		
		/// <summary>
		/// Changes the Icon Menu Bar Panel
		/// </summary>
		/// <param name="sender">Which Icon Menu Panel HeaderCaption button</param>
		/// <param name="e">Empty</param>
		private void Header_Click(object sender, System.EventArgs e)
		{
			foreach (Control i in this.Controls)
			{
				if (i is MenuBar)
				{
					((MenuBar)i).Height = hightdif;
				}
			}
			activebar = (MenuBar)sender;
			_activebarname = activebar.CodeLookup;
			MenuCollection_Resize_1(this, EventArgs.Empty);
		}

		/// <summary>
		/// Resizes the Icon Menu Bar if the Container is resized
		/// </summary>
		/// <param name="sender">Not important</param>
		/// <param name="e">Empty</param>
		private void MenuCollection_Resize_1(object sender, System.EventArgs e)
		{
			if (Parent != null && activebar != null)
			{
				activebar.Height = Height - (((_headers.Count - 1) * hightdif) + cheightdif);

				for (int i = activebar.HiddenButtons; i > 0; i--)
				{
					if (activebar.TotalButtonHeight + activebar.ButtonHeight <= activebar.Height - hightdif)
						activebar.ShowButton();
					else
						break;
				}

				activebar.DownButton = (activebar.TotalButtonHeight + hightdif > activebar.Height);
			}
			Invalidate();
		}

		public void RefreshFavorites()
		{
			// Load the Favorites from the Registry
			if (_pnlfav != null)
			{
				FWBS.OMS.UI.Windows.Global.RemoveAndDisposeControls(_pnlfav);
				FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites("MENUBARFAV");
				DataView dv = fav.GetDataView();
				dv.Sort = "usrFavObjParam4 DESC";
				foreach (DataRowView favKey in dv)
				{
					_pnlfav.Controls.Add(new FWBS.Common.UI.Windows.ucFavorites(_imagelist,Convert.ToInt32(favKey["usrFavGlyph"]),ProcessResStrings(Convert.ToString(favKey["usrFavObjParam2"]),Convert.ToString(favKey["usrFavObjParam1"])),Convert.ToString(favKey["usrFavDesc"]),Convert.ToInt32(favKey["usrFavObjParam4"]),Convert.ToString(favKey["usrFavObjParam2"])));
				}
			}
		}


		/// <summary>
		/// Menu Collection Load Event
		/// </summary>
		/// <param name="sender">Not important</param>
		/// <param name="e">Empty</param>
		private void MenuCollection_Load(object sender, System.EventArgs e)
		{
			// Activate the Selected Menu Bar
			if (activebar != null)
			{
				activebar.Height = Height - (((_headers.Count-1) * hightdif) + cheightdif);
				Header_Click(activebar,System.EventArgs.Empty);
				this.ButtonDown = this.ButtonDown;
			}

			RefreshFavorites();

			// If the Main Menu is Connected
			if (_mainmenu != null)
			{
				Hashtable menuitemsarray = new Hashtable();
				_menus = new Hashtable();
				foreach(object obj in _item)
				{
					if (obj is ButtonItems)
					{
						ButtonItems but = (ButtonItems)obj;
						if (but.MenuRoot != "")
						{
							MenuItem _menuitem = null;
							string rootkey = "";
							if (menuitemsarray[but.MenuRoot] ==  null)
							{
								string [] sections = but.MenuRoot.Split("/".ToCharArray());
								foreach(string section in sections)
								{
									rootkey = rootkey + section;
									if (menuitemsarray[rootkey] == null)
									{
										if (_menuitem == null)
											_menuitem = _mainmenu.MenuItems.Add(ProcessResStrings(section,section));
										else
											_menuitem = _menuitem.MenuItems.Add(ProcessResStrings(section,section));
										menuitemsarray.Add(rootkey,_menuitem);
										rootkey = rootkey + "/";
									}
									else
									{
										_menuitem = menuitemsarray[rootkey] as MenuItem;
									}
								}
							}
							else
							{
								_menuitem = menuitemsarray[but.MenuRoot] as MenuItem;
							}
							MenuItem finalitem = new MenuItem(ProcessResStrings(but.ButtonName, but.ButtonCaption),new EventHandler(Menu_Click));
							_menuitem.MenuItems.Add(finalitem);
							_menus.Add(finalitem,but);
						}
					}
				}
				menuitemsarray.Clear();
				MenuCollection_Resize_1(sender,e);
			}
		}

		/// <summary>
		/// Draws the 3D Bar around the Icon Menu Bar
		/// </summary>
		/// <param name="sender">Not Important</param>
		/// <param name="e">PaintEventArgs</param>
		private void MenuCollection_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ControlPaint.DrawBorder3D(e.Graphics,0,0,this.Width-2,this.Height-2,Border3DStyle.Sunken,Border3DSide.All);
		}

		/// <summary>
		/// Attach the ButtonClick event to the Favorites as they are added to the Favorites Panel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlFavoritesControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control is FWBS.Common.UI.Windows.ucFavorites)
				((FWBS.Common.UI.Windows.ucFavorites)e.Control).ButtonClick += new FWBS.Common.UI.Windows.ButtonEventHandler(Button_Click);
		}
		#endregion

		// 
		// Internal Private Procedures
		//
		#region Internal Private Procedures
		private string ProcessResStrings(string input, string Default)
		{
			try
			{
				if (FWBS.OMS.Session.CurrentSession.IsLoggedIn && input != "")
					return FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText(input);
				else
					return Default;
			}
			catch
			{
				return Default;
			}
		}
		#endregion

		//
		// Properties
		/// <summary>
		/// [Category("Favorites")] Panel
		/// [Category("Menus")] MainMenu
		/// [Category("Buttons")] ActiveBar
		/// [Category("Buttons")] ButtonImageList
		/// [Category("Buttons")] Items
		/// </summary>
		//
		#region Properties
		[Category("Favorites")]
		[Description("Link the the Panel that will contain the Favorite Buttons this panel needs to be blanked else it will be cleared on the Load Event")]
		[DefaultValue(null)]
		public ContainerControl Panel
		{
			get
			{
				return _pnlfav;
			}
			set
			{
				if (_pnlfav != value)
				{
					_pnlfav = value;
					if (_pnlfav != null)
					{
						_pnlfav.ControlAdded += new ControlEventHandler(pnlFavoritesControlAdded);
					}
				}
			}
		}

		[Category("Buttons")]
		[DefaultValue("")]
		[Description("Returns or Sets the Return Key of the button which has the down state")]
		[TypeConverter(typeof(ButtonItems.ButtonLister))]
		public string ButtonDown
		{
			get
			{
				return _buttondown;
			}
			set
			{
				if (value.IndexOf("|") == -1)
					value = _activebarname + "|" + value;
				else
				{
					string barname = value.Split('|')[0];
					this.ActiveBar = barname ;
				}
				_buttondown = value;
				
				for(int t = 0; t < _buttons.Count; t++)
				{
					object item = _buttons.GetByIndex(t);
					if (item is MenuButton)
					{
						MenuButton i = (MenuButton)item;
						i.Toggle = false;
					}
				}				
				try
				{
					MenuButton b = (MenuButton)_buttons[value];
					b.Toggle = true;
				}
				catch
				{
				}
			}
		}
		
		
		[Category("Buttons")]
		[DefaultValue(true)]
		public bool UseLocalisation
		{
			get
			{
				return _localise;
			}
			set
			{
				_localise = value;
			}
		}

		[Category("Buttons")]
		[DefaultValue(ButtonStyle.mbLarge)]
		public ButtonStyle ButtonStyle
		{
			get
			{
				return _buttonstyle;
			}
			set
			{
				_buttonstyle = value;
				int bth = 0;
				for(int t = 0; t < _buttons.Count; t++)
				{
					object item = _buttons.GetByIndex(t);
					if (item is MenuButton)
					{
						MenuButton i = (MenuButton)item;
						i.ButtonStyle = value;
						bth = i.Height;
					}
				}
				for(int t = 0; t < _headers.Count; t++)
				{
					object item = _headers.GetByIndex(t);
					if (item is MenuBar)
					{
						MenuBar i = (MenuBar)item;
						i.ButtonHeight = bth;
					}
				}
			}
		}

		[Category("Menus")]
		[Description("Link the the MainMenu that will contain ButtonItems that have a MenuRoot")]
		[DefaultValue(null)]
		public System.Windows.Forms.MainMenu MainMenu
		{
			get
			{
				return _mainmenu;
			}
			set
			{
				_mainmenu = value;
			}
		}

		[Category("Buttons")]
		[Description("Name of the Active Bar if a Resource Name e.g. %ResName% then use this.")]
		[TypeConverter(typeof(ButtonItems.HeaderLister))]
		[DefaultValue("")]
		public string ActiveBar
		{
			get
			{
				return _activebarname;
			}
			set
			{
				if (_activebarname != value)
				{
					_activebarname = value;
				}
				ChangePropEventArgs e = new ChangePropEventArgs(ChangePropTypes.MainHeaderChange);
				OnPropChanged(e);
			}
		}

		[Category("Buttons")]
		[Description("Link to a ImageList containing the Images for the Icon Menu Bar and Favorites")]
		[DefaultValue(null)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		public ImageList ButtonImageList
		{
			get
			{
				return _imagelist;
			}
			set
			{
				if (_imagelist != value)
				{
					_imagelist = value;
					ChangePropEventArgs e = new ChangePropEventArgs(ChangePropTypes.ImageListChange);
					OnPropChanged(e);
				}
			}
		}

		[Category("Buttons")]
		[Description("A Collection of menu items used for the Creation of the Icon Menu Bar and Drop Down Menus")]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(MenuItemEditor),typeof(UITypeEditor))]
		[DefaultValue(null)]
		public EventArrayList Items
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}		
		}
		#endregion

		//
		// Collection Desinger
		//
		/// <summary>
		/// internal class MenuItemEditor : CollectionEditor
		/// </summary>
		//
		#region Collection Editor of ButtonItems
		/// <summary>
		/// MenuItem Collection Editor of ButtonItems
		/// </summary>
		internal class MenuItemEditor : CollectionEditor
		{
			public MenuItemEditor() : base (typeof(ArrayList)) 
			{
			}

			protected override System.Type CreateCollectionItemType ( )
			{
				return typeof (ButtonItems);
			}

			protected override object CreateInstance(System.Type t)
			{
				return new ButtonItems("","","",0,false,true,"","","");
			}

			protected override Type[] CreateNewItemTypes ( )
			{
				return new Type[] {typeof(ButtonItems)};
			}
		}
		
		#endregion

		//
		// Button Item Class
		/// <summary>
		/// public ButtonItems(string HeaderCaption,string ButtonCaption, string ReturnKey, int ImageIndex, bool incFavorites, bool VisibleInSideBar, string MenuRoot)
		/// public override string ToString ( )
		/// public ImageList ButtonImageList
		/// [Category("Menus")] MenuRoot
		/// [Category("Group")] HeaderCaption
		/// [Category("Button")] ButtonCaption
		/// [Category("Favorites")] Include
		/// [Category("Buttons")] VisibleInSideBar
		/// [Category("Button")] ReturnKey
		/// [Category("Button")] ImageIndex
		/// internal class IconDisplayEditor : UITypeEditor
		//
		#region ButtonItems
		/// <summary>
		/// Summary description for ButtonItems.
		/// </summary>
		[Serializable()] 
			[TypeConverter(typeof(ButtonItemConverter))]
			public class ButtonItems
		{
			private string _header;
			private string _caption;
			private string _returnkey;
			private int _imageindex;
			public ImageList IntimageList;
			private bool _incfav;
			private bool _sidebarvisible;
			private string _menuroot;
			private string _butcodelookup;
			private string _hedcodelookup;
			private ButtonStyle _buttonstyle = ButtonStyle.mbLarge;
			private FWBS.Common.UI.Windows.MenuButton _button = new FWBS.Common.UI.Windows.MenuButton();
			
			public ButtonItems(string HeaderCaption,string ButtonCaption, string ReturnKey, int ImageIndex, bool incFavorites, bool VisibleInSideBar, string MenuRoot, string HeaderName, string ButtonName) : this (HeaderCaption,ButtonCaption,ReturnKey,ImageIndex,incFavorites,VisibleInSideBar,MenuRoot,HeaderName,ButtonName,ButtonStyle.mbLarge)
			{
			}
				
			public ButtonItems(string HeaderCaption,string ButtonCaption, string ReturnKey, int ImageIndex, bool incFavorites, bool VisibleInSideBar, string MenuRoot, string HeaderName, string ButtonName, ButtonStyle buttonStyle)
			{
				_header = HeaderCaption;
				_caption = ButtonCaption;
				_returnkey = ReturnKey;
				_imageindex = ImageIndex;
				_incfav  = incFavorites;
				_sidebarvisible = VisibleInSideBar;
				_menuroot = MenuRoot;
				_butcodelookup = ButtonName;
				_hedcodelookup = HeaderName;
				_buttonstyle = buttonStyle;
			}

			public override string ToString ( )
			{
				return _header + " | " + _caption;
			}

			[Browsable(false)]
			public ButtonStyle ButtonStyle
			{
				get
				{
					return _buttonstyle;
				}
				set
				{
					_buttonstyle = value;
				}
			}
			
			[Browsable(false)]
			public ImageList ButtonImageList
			{
				get
				{
					return IntimageList;
				}
			}
		
			[Category("Menus")]
			public string MenuRoot
			{
				get
				{
					return _menuroot;
				}
				set
				{
					_menuroot=value;
				}
			}
			
			[Category("Header")]
			public string HeaderCaption
			{
				get
				{
					return _header;
				}
				set
				{
					_header=value;
				}
			}
			
			[Category("Header")]
			public string HeaderName
			{
				get
				{
					return _hedcodelookup;
				}
				set
				{
					_hedcodelookup = value;
				}
			}

			[Category("Button")]
			public string ButtonName
			{
				get
				{
					return _butcodelookup;
				}
				set
				{
					_butcodelookup = value;
				}
			}

			[Category("Button")]
			public string ButtonCaption
			{
				get
				{
					return _caption;
				}
				set
				{
					_caption = value;
				}
			}

			[Category("Favorites")]
			public bool Include
			{
				get
				{
					return _incfav;
				}
				set
				{
					_incfav = value;
				}
			}

			[Category("Buttons")]
			public bool VisibleInSideBar
			{
				get
				{
					return _sidebarvisible;
				}
				set
				{
					_sidebarvisible = value;
				}
			}

			[Category("Button")]
			public string ReturnKey
			{
				get
				{
					return _returnkey;
				}
				set
				{
					_returnkey = value;
				}
			}

			[Category("Button")]
			[TypeConverter(typeof(ImageIndexConverter))]
			[Editor(typeof(IconDisplayEditor),typeof(UITypeEditor))]
			public int ImageIndex
			{
				get
				{
					return _imageindex;
				}
				set
				{
					_imageindex = value;
				}
			}

			internal class ButtonLister : StringConverter
			{
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
				{
					return true;
				}
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
				{
					EventArrayList buttons = (EventArrayList)TypeDescriptor.GetProperties(context.Instance)["Items"].GetValue(context.Instance);
					ArrayList controls = new ArrayList();
					foreach (ButtonItems bt in buttons)
						controls.Add(bt.HeaderName + "|" + bt.ButtonName);
					string[] vals;
					vals = new string[controls.Count];
					controls.CopyTo(vals);
					return new StandardValuesCollection(vals);
				}

			}
			
			internal class HeaderLister : StringConverter
			{
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
				{
					return true;
				}
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
				{
					EventArrayList buttons = (EventArrayList)TypeDescriptor.GetProperties(context.Instance)["Items"].GetValue(context.Instance);
					ArrayList controls = new ArrayList();
					foreach (ButtonItems bt in buttons)
						if (controls.Contains(bt.HeaderName) == false)
							controls.Add(bt.HeaderName);
					string[] vals;
					vals = new string[controls.Count];
					controls.CopyTo(vals);
					return new StandardValuesCollection(vals);
				}
			}

			internal class IconDisplayEditor : UITypeEditor
			{
				public IconDisplayEditor()
				{
				}

				public override bool GetPaintValueSupported ( ITypeDescriptorContext ctx )
				{
					return true ;
				}

				public override void PaintValue ( PaintValueEventArgs e )
				{
					if (e != null && e.Context != null) 
					{
						// Find the ImageList property on the parent...
						//
						PropertyDescriptor imageProp = null;
						foreach(PropertyDescriptor pd in
							TypeDescriptor.GetProperties(e.Context.Instance)) 
						{
							if (typeof(ImageList).IsAssignableFrom(pd.PropertyType)) 
							{
								imageProp = pd;
								break;
							}
						}
						if (imageProp != null) 
						{
							try
							{
								ImageList imageList = (ImageList)imageProp.GetValue(e.Context.Instance);
								Image image = imageList.Images[Convert.ToInt32(Convert.ChangeType(e.Value,typeof(System.Int32)))];
								e.Graphics.DrawImage(image, e.Bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
							}
							catch
							{}
						}
					}
				}
			}
		}

		/// <summary>
		/// ButonItemConvert Code Writer for the Collection
		/// </summary>
		internal class ButtonItemConverter : ExpandableObjectConverter
		{
			public ButtonItemConverter()
			{
			}

			public override bool CanConvertTo ( ITypeDescriptorContext ctx , Type destinationType )
			{
				if (destinationType == typeof(InstanceDescriptor))
					return true;
				else
					return base.CanConvertTo (ctx, destinationType);
			}

			public override object ConvertTo ( ITypeDescriptorContext ctx , CultureInfo culture , object value , Type destinationType )
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					ButtonItems gf = value as ButtonItems;
					if (gf != null)
					{
						Type[] parms = new Type[] {typeof(String),typeof(String),typeof(String),typeof(int),typeof(bool),typeof(bool),typeof(String),typeof(String),typeof(String)};
						ConstructorInfo	ci = typeof(ButtonItems).GetConstructor(parms);
						return new InstanceDescriptor (ci,new object[]{gf.HeaderCaption, gf.ButtonCaption, gf.ReturnKey, gf.ImageIndex, gf.Include, gf.VisibleInSideBar,gf.MenuRoot, gf.HeaderName, gf.ButtonName});
					}
					else
						return base.ConvertTo(ctx, culture, value, destinationType); 
				}
				else
					return base.ConvertTo(ctx, culture, value, destinationType ) ;
			}

		}

		#endregion

	}
}
