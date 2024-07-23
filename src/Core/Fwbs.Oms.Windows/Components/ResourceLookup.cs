using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A component that can be used to localize controls on a form to the entries
    /// within the codelookup library database.
    /// </summary>
    [ProvideProperty("LookupCode", typeof(object))]
	[ProvideProperty("LookupDefault", typeof(object))]
	[ProvideProperty("Lookup",typeof(object))]
	public class ResourceLookup : System.ComponentModel.Component, IExtenderProvider
	{
		#region Fields
		private System.ComponentModel.IContainer components;
		/// <summary>
		/// The controls collection with the values being looked up.
		/// </summary>
		private Hashtable _itmstest = new Hashtable();
		private bool _refreshing = false;
		public System.Windows.Forms.ToolTip ToolTip;
		/// <summary>
		/// Checks login only once.
		/// </summary>
		private bool _checkLogin = true;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Constructor used to add this component to the container it is to sit on.
		/// </summary>
		/// <param name="container">Calling container.</param>
		public ResourceLookup(System.ComponentModel.IContainer container)
		{
			/// <summary>
			/// Required for Windows.Forms Class Composition Designer support
			/// </summary>
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Default constructor to create component id code with no container involved.
		/// </summary>
		public ResourceLookup()
		{
			/// <summary>
			/// Required for Windows.Forms Class Composition Designer support
			/// </summary>
			InitializeComponent();
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);

		}
		#endregion

		#endregion

		#region IExtenderProvider Implmentation

		/// <summary>
		/// Checks whether the specified control is valid to be extended.
		/// </summary>
		/// <param name="extendee">Object control extended.</param>
		/// <returns>A boolean value of the object being able to be extended.</returns>
		public bool CanExtend (object extendee)
		{
			if ((extendee is Control || extendee is MenuItem || extendee is DataGridColumnStyle || extendee is ToolBarButton || extendee is ToolStripItem) && !(extendee is ResourceLookup)) 
				return true;
			else
				return false;
		}

		#endregion

		#region Static
		public static string GetLookupText(string code)
		{
			return GetLookupText(code, "","",new string[0]);
		}

		public static string GetLookupHelp(string code)
		{
			if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
			{
				ResourceItem res = FWBS.OMS.Session.CurrentSession.Resources.GetResource(code,"","");
				return res.Help;
			}
			else
				return "";
		}

		public static string GetLookupText(string code, string defaulttext, string defaulthelp, params string [] param)
		{
			return GetLookupText(code,defaulttext,defaulthelp,true,param);
		}
					
		public static string GetLookupText(string code, string defaulttext, string defaulthelp, bool parse, params string [] param)
		{
			if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
			{
				ResourceItem res = FWBS.OMS.Session.CurrentSession.Resources.GetResource(code,defaulttext,defaulthelp,parse,param);
				return res.Text;
			}
			else
			{
				if (defaulttext != "")
					return defaulttext;
				else
					return code;
			}
		}
		#endregion
		
		#region Methods

		[DefaultValue(null)]
		[Editor(typeof(ResourceLookupItemEditor),typeof(UITypeEditor))]
		public ResourceLookupItem GetLookup(object itm)
		{
			try
			{
				ResourceLookupItem res = _itmstest[itm] as ResourceLookupItem;
				return res;
			}
			catch
			{
				return null;
			}
		}

		[DefaultValue(null)]
		[Editor(typeof(ResourceLookupItemEditor),typeof(UITypeEditor))]
		public void SetLookup(object itm, ResourceLookupItem value)
		{
			try
			{
				if (!_refreshing)
				{
					if (!_itmstest.Contains(itm))
						_itmstest.Add(itm,value);
					else
						_itmstest[itm] = value;
				}

				if (value.Code != String.Empty)
				{
					if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
					{
						if (_checkLogin) FWBS.OMS.UI.Windows.Services.CheckLogin();
						_checkLogin = false;
						ResourceItem res = FWBS.OMS.Session.CurrentSession.Resources.GetResource(value.Code,value.Description,value.Help);
						if (itm is Control)
						{
							Control control = (Control)itm;
							control.Text = res.Text;
							ToolTip.SetToolTip(control, res.Help);
							if (_refreshing==false) control.TextChanged += new EventHandler(TextChanged);
						}
						else if (itm is MenuItem)
						{
							MenuItem menu = (MenuItem)itm;
							menu.Text = res.Text;
							if (menu.Parent is MainMenu == false && _refreshing==false)
							{
								menu.OwnerDraw=true;
								menu.MeasureItem += new MeasureItemEventHandler(MeasureItem);
							}
						}
						else if (itm is DataGridColumnStyle)
						{
							DataGridColumnStyle col = (DataGridColumnStyle)itm;
							col.HeaderText = res.Text;
							if (_refreshing==false) 
								col.HeaderTextChanged += new EventHandler(TextChanged);
						}
						else if (itm is ToolBarButton)
						{
							ToolBarButton tb = (ToolBarButton)itm;
							tb.Text = res.Text;
							tb.ToolTipText = res.Help;
						}
                        else if (itm is ToolStripItem)
                        {
                            ToolStripItem tsi = (ToolStripItem)itm;
                            tsi.Text = res.Text;
                            tsi.ToolTipText = res.Help;
                            if (_refreshing == false)
                                tsi.TextChanged += new EventHandler(TextChanged);
                        }
					}
				}
			}
			catch
			{}
		}

	
		private ResourceLookupItem GetLookupItem(object value)
		{
			return (ResourceLookupItem)_itmstest[value];
		}

		/// <summary>
		/// Refreshes all the resources controls and reapplies the resource.
		/// </summary>
		public void Refresh()
		{
			Refresh(false);
		}

		/// <summary>
		/// Refreshes all the resources controls and with refreshed resources from the database.
		/// </summary>
		/// <param name="refreshLookups">True to refresh the lookups from the database.</param>
		public void Refresh(bool refreshLookups)
		{
			if (refreshLookups)
				Session.CurrentSession.Resources.Refresh();
			_refreshing = true;
			foreach (object itm in _itmstest.Keys)
			{
				SetLookup(itm,(ResourceLookupItem)_itmstest[itm]);
			}
			_refreshing = false;
		}

		/// <summary>
		/// Captures the controls text change event incase the forms initialize code
		/// has set the text after the text has been set within this component.
		/// </summary>
		/// <param name="sender">The control whose text has changed.</param>
		/// <param name="e">Empty event arguments.</param>
		private void TextChanged (object sender, EventArgs e)
		{
			ResourceLookupItem ires = GetLookupItem(sender);
			ResourceItem res = FWBS.OMS.Session.CurrentSession.Resources.GetResource(ires.Code,ires.Description,ires.Help);
			if (sender is Control)
			{
				Control control = (Control)sender;
				control.TextChanged -= new EventHandler(TextChanged);
				control.Text = res.Text;
				ToolTip.SetToolTip(control, res.Help);
			}
			else if (sender is DataGridColumnStyle)
			{
				DataGridColumnStyle col = (DataGridColumnStyle)sender;
				col.HeaderTextChanged -= new EventHandler(TextChanged);
				col.HeaderText = res.Text;
			}
            else if (sender is ToolStripItem)
            {
                ToolStripItem tsi = (ToolStripItem)sender;
                tsi.TextChanged -= new EventHandler(TextChanged);
                tsi.Text = res.Text;
            }
		}

		/// <summary>
		/// Captures the menus measure item event incase the forms initialize code
		/// has set the text after the text has been set within this component.
		/// </summary>
		/// <param name="sender">The menu whose text has changed.</param>
		/// <param name="e">Empty event arguments.</param>
		private void MeasureItem (object sender, MeasureItemEventArgs e)
		{
			ResourceLookupItem ires = GetLookupItem(sender);
			ResourceItem res = FWBS.OMS.Session.CurrentSession.Resources.GetResource(ires.Code,ires.Description,ires.Help);
			if (sender is MenuItem)
			{
				MenuItem menu = (MenuItem)sender;
				menu.Text = res.Text;
				menu.OwnerDraw = false;
				menu.MeasureItem -= new MeasureItemEventHandler(MeasureItem);
			}
		}
		#endregion
	}
}
