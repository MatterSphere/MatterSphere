using System;
using System.Data;
using FWBS.Common;

namespace FWBS.OMS
{
    /// <summary>
    /// Summary description for CommandBarItem.
    /// </summary>
    public class CommandBarItem  : CommonObject
	{
		#region Static
		/// <summary>
		/// Gets an CommandBarItem from the CtrlID and assigns an Event Handler to the Change Event
		/// </summary>
		/// <param name="CtrlID">The Control ID</param>
		/// <param name="Changed">A Standard EventHandler to capture UI Changes</param>
		/// <returns>Returns a CommandBarItem</returns>
		public static CommandBarItem GetCommandBarItem(int CtrlID, EventHandler Changed)
		{
			CommandBarItem bar = new CommandBarItem();
			bar.Fetch(CtrlID);
			if (Changed != null) bar.Changed += Changed;
			return bar;
		}

		/// <summary>
		/// Creates a new CommandBarItem
		/// </summary>
		/// <param name="ToolbarCode">The Code of the CommandBar Menu</param>
		/// <param name="Parent">A Parent Menu Code ("") if a main menu</param>
		/// <param name="Order">The Order in which the items should be displayed</param>
		/// <param name="Level">What Popout Level e.g. 0 is the Main Menu 1 is any items below the main menu and so on</param>
		/// <param name="Type">The Office Code type either {msoControlButton,msoControlPopup}</param>
		/// <param name="Changed">A Standard EventHandler to capure UI changes</param>
		/// <returns>Returns a CommandBarItem</returns>
		public static CommandBarItem CreateCommandBarItem(string ToolbarCode, string Parent, int Order, int Level, string Type, EventHandler Changed)
		{
			CommandBarItem bar = new CommandBarItem();
			bar.Create();
			bar.CommandBar = ToolbarCode;
			bar.ParentItem = Parent;
			bar.Level = Level;
			bar.Order = Order;
			bar.Type = Type;
			bar.Changed += Changed;
			bar.SetExtraInfo("ctrlFilter","*");
			return bar;
		}
		#endregion

		#region Fields
		/// <summary>
		/// The Caption of the Menu which will be stored in the Codelookups
		/// </summary>
		private string _caption = "";
		/// <summary>
		/// Any conditions that must be met if the menu is to become visible
		/// </summary>
		private string[] _conditions;
		/// <summary>
		/// A object to store data for the Tag Property 
		/// </summary>
		private object _object = null;
		/// <summary>
		/// The Event Handler for the UI Changes
		/// </summary>
		public event EventHandler Changed;
		/// <summary>
		/// The roles for this menu Item
		/// </summary>
		private CodeLookupDisplayMulti _usrroles;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor to Create a Empty CommandBarItem you must run Create or Fetch Methods
		/// </summary>
		public CommandBarItem()
		{
			_usrroles = new CodeLookupDisplayMulti("USRROLES");
		}
		#endregion

		#region Public
		public void Fetch(int CtrlID)
		{
			base.Fetch (CtrlID);
			_conditions = Convert.ToString(GetExtraInfo("ctrlCondition")).Split(Environment.NewLine[0]);
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i] = _conditions[i].Trim(Environment.NewLine[1]);

			_caption = CodeLookup.GetLookup("CBCCAPTIONS",this.Code);
			_usrroles.Codes = Convert.ToString(GetExtraInfo("ctrlRole"));

		}

		public bool IsCodeUsed(string Code)
		{
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code",Code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("ID",this.CtrlID);
			DataTable _data = Session.CurrentSession.Connection.ExecuteSQLTable(SelectStatement + " where ctrlID <> @ID AND ctrlCode = @Code", "EXISTS", paramlist);
			if ((_data == null) || (_data.Rows.Count == 0))
				return false;
			else
				return true;
		}

		
		public override void Delete()
		{
			if (_data.Columns.Contains(FieldActive))
			{
				SetExtraInfo(FieldActive, false);
			}
			else
			{
				_data.Rows[0].Delete();
			}
		}
		#endregion

		#region Overrides
		/// <summary>
		/// No Enquiry form is used to edit this object
		/// </summary>
		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The Primary Field
		/// </summary>
		public override string FieldPrimaryKey
		{
			get
			{
				return "ctrlID";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "CMDBARITEM";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbCommandBarControl";
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Returns the CtrlID
		/// </summary>
		[LocCategory("(DETAILS)")]
		public int CtrlID
		{
			get
			{
				return ConvertDef.ToInt32(GetExtraInfo("CtrlID"),-1);
			}
		}

		/// <summary>
		/// Gets and Sets the Code this can be changed anytime as long as it does not already
		/// exist
		/// </summary>
		[LocCategory("(DETAILS)")]
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("CtrlCode"));
			}
			set
			{
				if (IsCodeUsed(value))
					throw new OMSException2("ERRCODEAEXTS","The Command Bar Button Code '%1%' has already been used. Please enter another.","",new Exception(),true,value);
				else
				{
					SetExtraInfo("ctrlCode",value.ToUpper());
					MenuCaption = MenuCaption;
				}
			}
		}

		/// <summary>
		/// Gets the name of the Main Command Bar sets only allowed in code
		/// </summary>
		[LocCategory("(DETAILS)")]
		[System.ComponentModel.ReadOnly(true)]
		public string CommandBar
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ctrlCommandBar"));
			}
			set
			{
				SetExtraInfo("ctrlCommandBar",value);
			}
		}

		/// <summary>
		/// Get and Sets the Menu Caption to be stored in the CodeLookups
		/// </summary>
		[LocCategory("(DETAILS)")]
		public string MenuCaption
		{
			get
			{
				return _caption;
			}
			set
			{
				if (this.Code == "")
					throw new OMSException2("ERRCODENOTSET","The Code must be set before the Menu Caption.","",new Exception(),true,value);

				_caption = value;
				CodeLookup.Create("CBCCAPTIONS",this.Code,value,"",CodeLookup.DefaultCulture,true,true,true);

				if (Changed != null)
					Changed(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets the Order of Display Set is allowed only in code
		/// </summary>
		[LocCategory("DATA")]
		[System.ComponentModel.ReadOnly(true)]
		public int Order
		{
			get
			{
				return ConvertDef.ToInt32(GetExtraInfo("ctrlOrder"),0);
			}
			set
			{
				SetExtraInfo("ctrlOrder",value);
			}
		}

		/// <summary>
		/// Gets the Popout Level 0 being the Main Menu 1 being any items in that menu 2 and above being Popouts
		/// </summary>
		[LocCategory("DATA")]
		[System.ComponentModel.ReadOnly(true)]
		public int Level
		{
			get
			{
				return ConvertDef.ToInt32(GetExtraInfo("ctrlLevel"),0);
			}
			set
			{
				SetExtraInfo("ctrlLevel",value);
			}
		}

		/// <summary>
		/// The Special Filter used hide or show menu items depending on the Application
		/// </summary>
		[LocCategory("DATA")]
		public string Filter
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ctrlFilter"));
			}
			set
			{
				if (value == "")
					SetExtraInfo("ctrlFilter","*");
				else
					SetExtraInfo("ctrlFilter",value);
			}
		}

		
		/// <summary>
		/// Gets the Parent Code for a Menu Item that is not a main menu
		/// </summary>
		[LocCategory("DATA")]
		[System.ComponentModel.ReadOnly(true)]
		public string ParentItem
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ctrlParent"));
			}
			set
			{
				if (value == "")
					SetExtraInfo("ctrlParent",DBNull.Value);
				else
					SetExtraInfo("ctrlParent",value);
			}
		}

		/// <summary>
		/// The Office type of button either {msoControlButton,msoControlPopup}
		/// </summary>
		[LocCategory("DATA")]
		[System.ComponentModel.ReadOnly(true)]
		public string Type
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ctrlType"));
			}
			set
			{
				SetExtraInfo("ctrlType",value);
			}
		}

		/// <summary>
		/// Starts or adds above a Seperator
		/// </summary>
		[LocCategory("DATA")]
		public bool BeginGroup
		{
			get
			{
				return ConvertDef.ToBoolean(GetExtraInfo("ctrlBeginGroup"),false);
			}
			set
			{
				SetExtraInfo("ctrlBeginGroup",value);
				if (Changed != null)
					Changed(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Hides the Menu Item
		/// </summary>
		[LocCategory("DATA")]
		public bool Hide
		{
			get
			{
				return ConvertDef.ToBoolean(GetExtraInfo("ctrlHide"),false);
			}
			set
			{
				SetExtraInfo("ctrlHide",value);
			}
		}

		/// <summary>
		/// Gets or Sets the Image Index Number for the Image in the Office Application
		/// </summary>
		[LocCategory("DATA")]
		public int Icon
		{
			get
			{
				return ConvertDef.ToInt32(GetExtraInfo("ctrlIcon"),-1);
			}
			set
			{
				if (value == -1)
					SetExtraInfo("ctrlIcon",DBNull.Value);
				else
					SetExtraInfo("ctrlIcon",value);
				if (Changed != null)
					Changed(this,EventArgs.Empty);

			}
		}

		/// <summary>
		/// Gets or Sets the Roles allow to see the menu item
		/// </summary>
		[LocCategory("SYSTEM")]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti UserRoles
		{
			get
			{
				return _usrroles;
			}
			set
			{
				_usrroles = value;
				SetExtraInfo("ctrlRole",value.Codes);
			}
		}

		/// <summary>
		/// Gets or Sets the Conditions that can be set e.g. IsPackageInstalled("")
		/// </summary>
		[LocCategory("SYSTEM")]
		public string[] Conditions
		{
			get
			{
				return _conditions;
			}
			set
			{
				_conditions = value;
				SetExtraInfo("ctrlCondition",String.Join(Environment.NewLine, value));
			}
		}

		/// <summary>
		/// Gets or Set the OMS MACRO Command or the Script Method in the MENU Script
		/// </summary>
		[LocCategory("ACTIONS")]
		[System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.ScriptMenuCommands,omsadmin")]
		public string RunCommand
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ctrlRunCommand"));
			}
			set
			{
				SetExtraInfo("ctrlRunCommand",value);
			}
		}

		
		/// <summary>
		/// Misc Tag Value to store what ever
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public object Tag
		{
			get
			{
				return _object;
			}
			set
			{
				_object = value;
			}
		}
		#endregion

		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		public override object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}
