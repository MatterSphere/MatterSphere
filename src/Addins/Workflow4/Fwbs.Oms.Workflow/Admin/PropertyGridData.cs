using System;
using System.ComponentModel;

namespace FWBS.OMS.Workflow.Admin
{
    public class PropertyGridData : Observable
	{
		private const int MaxCodeLength = 100;
		private const int MaxCodeLookupLength = 15;
        private const string ROLE_ADMIN = "ADMIN";			// role name for modifying system items

		private string _code;
		private bool _isEditMode;
        private bool isAdmin = false;							// indicates whether this user can add/remove system/global items

		public PropertyGridData()
		{
			//  Default to EditMode ON
			isEditMode = true;      

			Code = string.Empty;
			Description = new CodeLookupDisplay(FWBS.OMS.Workflow.Constants.CODELOOKUPTYPE);
			IsServerWorkflow = false;
			IsReadOnly = false;
			IsSystem = false;
			Group = new CodeLookupDisplay(FWBS.OMS.Workflow.Constants.CODELOOKUP_GROUPTYPE);
			Notes = null;
			Updated = DateTime.MinValue;
			UpdatedBy = string.Empty;

            string[] roles = FWBS.OMS.Session.CurrentSession.CurrentUser.Roles.Split(',');
            foreach (string str in roles)
            {
                if (str == ROLE_ADMIN)
                {
                    this.isAdmin = true;
                    break;
                }
            }
		}

		/// <summary>
		/// Is Edit Mode On/Off
		/// </summary>
		[Browsable(false)]
		public bool isEditMode
		{
			get { return _isEditMode; }
			set { _isEditMode = value; }
		}
		
		[LocCategory("(Details)")]
		[DisplayName("Code")]
		[Description("Code to identify the Workflow.")]
		public string Code
		{
			get { return _code; }
			set
			{
				if (isEditMode)
				{
					if (value != null && value.Length > MaxCodeLength)
					{
						throw new ArgumentException("Code must be " + MaxCodeLength + " characters or less");
					}

					Set(ref _code, value, "Code");
				}
			}
		}

		private CodeLookupDisplay _description;

		[LocCategory("(Details)")]
		[DisplayName("Description")]
		[Description("Description of the Workflow.")]
		public CodeLookupDisplay Description
		{   
			get { return _description; }
			set
			{
				Set<CodeLookupDisplay>(ref _description, value, "Description");
			}
		}

		private bool isserverworkflow;
		
		[LocCategory("(Details)")]
		[DisplayName("Is Server Workflow")]
		[Description("Indicates the Workflow can be run on a Server.")]
		[ReadOnly(true)]
		public bool IsServerWorkflow
		{
			get { return isserverworkflow; }
			set
			{
				Set<bool>(ref isserverworkflow, value, "IsServerWorkflow");
			}
		}

		private bool isreadonly;

		[LocCategory("(Details)")]
		[DisplayName("Is Read Only")]
		[Description("Indicates the Workflow can be edited.")]
		public bool IsReadOnly
		{
			get { return isreadonly; }
			set
			{
                if (isAdmin)
                {
                    Set<bool>(ref isreadonly, value, "IsReadOnly");
                }
			}
		}

		private DateTime updated;

		[LocCategory("(Details)")]
		[DisplayName("Updated")]
		[Description("The date this Workflow was last updated.")]
		[ReadOnly(true)]
		public DateTime Updated
		{
			get { return updated; }
			set
			{
				Set<DateTime>(ref updated, value, "Updated");
			}
		}

		private string updatedby;

		[LocCategory("(Details)")]
		[DisplayName("Updated By")]
		[Description("The person who last updated this Workflow.")]
		[ReadOnly(true)]
		public string UpdatedBy
		{
			get { return updatedby; }
			set
			{
				Set<string>(ref updatedby, value, "UpdatedBy");
			}
		}

		private string[] notes;

		[LocCategory("(Details)")]
		[DisplayName("Notes")]
		[Description("Notes that are attached to this Workflow.")]
		public string[] Notes
		{
			get { return notes; }
			set
			{
				Set<string[]>(ref notes, value, "Notes");
			}
		}

		private CodeLookupDisplay group;

		[LocCategory("(Details)")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("GROUPS", "Groups")]
		[Description("Sets the Group this workflow belongs.")]
		public CodeLookupDisplay Group
		{
			get { return group; }
			set
			{
				Set<CodeLookupDisplay>(ref group, value, "Group");
			}
		}

		private bool isvisibleintoolbox;

		[LocCategory("(Details)")]
		[DisplayName("Is Visible In Toolbox")]
		[Description("Indicates whether this Workflow can be added to the Toolbox.")]
		public bool IsVisibleInToolbox
		{
			get { return isvisibleintoolbox; }
			set
			{
				Set<bool>(ref isvisibleintoolbox, value, "IsVisibleInToolbox");
			}
		}

		private bool isvisibleinpicker;

		[LocCategory("(Details)")]
		[DisplayName("Is Visible In Picker:")]
		[Description("Indicates whether this Workflow is displayed in a list to be picked by a user.")]
		public bool IsVisibleInPicker
		{
			get { return isvisibleinpicker; }
			set
			{
				Set<bool>(ref isvisibleinpicker, value, "IsVisibleInPicker");
			}
		}

		private bool isSystem;

		[LocCategory("(Details)")]
		[DisplayName("Is System Workflow:")]
		[Description("Indicates Workflow is a builtin Workflow.")]
		[ReadOnly(true)]
		public bool IsSystem
		{
			get { return isSystem; }
			set
			{
				Set<bool>(ref isSystem, value, "IsSystem");
				if (value)
				{
					Set<bool>(ref isSystem, value, "IsSystem");
				}
			}
		}
	}
}
