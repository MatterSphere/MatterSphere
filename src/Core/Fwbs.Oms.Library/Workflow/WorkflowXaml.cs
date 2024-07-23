﻿#region References
using System;
using System.Collections.Generic;
using System.Data;
#endregion

namespace FWBS.WF.Packaging
{
    #region DistributedEventArgs
    public class DistributedEventArgs : EventArgs
	{
		public DistributedEventArgs(Exception exception)
		{
			Exception = exception;
		}

		public Exception Exception { get; private set; }
	}
	#endregion

	#region WorkflowXaml class
	public sealed class WorkflowXaml : WorkflowCommonObject
	{
		#region Constants
		//
		// SQL Related
		//
		private const string DBS_TABLENAME = "dbWorkflow";					// SQl table name for workflow
		private const string DBS_SELECT_STMT = "select " + DBS_CODE_FIELD + "," +	DBS_VERSION_FIELD + "," + DBS_XAML_FIELD + "," + 
			DBS_ASSEMBLIES_FIELD + "," + DBS_GROUP_FIELD + "," + DBS_NOTES_FIELD + "," + DBS_ATTRIBUTES_FIELD + "," + DBS_CODELOOKUP_FIELD + "," + 
			DBS_CREATED_FIELD + "," + DBS_CREATEDBY_FIELD + "," + DBS_UPDATED_FIELD + "," + DBS_UPDATEDBY_FIELD + 
			", dbo.GetUser(" + DBS_CREATEDBY_FIELD + ", 'USRFULLNAME') as " + DBS_UPDATEDBYUSER_FIELD + 
			", dbo.GetUser(" + DBS_UPDATEDBY_FIELD + ", 'USRFULLNAME') as " + DBS_UPDATEDBYUSER_FIELD +
			" from " + DBS_TABLENAME;
		private const string DBS_SELECT_GROUPS_STMT = "select distinct " + DBS_GROUP_FIELD + " from " + DBS_TABLENAME;	// select distinct groups from the SQL table
		private const string DBS_SELECT_TOOLBOX_STMT = "select " + DBS_CODE_FIELD + "," + DBS_VERSION_FIELD + "," + DBS_XAML_FIELD + "," +
			DBS_ASSEMBLIES_FIELD + "," + DBS_GROUP_FIELD + "," + DBS_NOTES_FIELD + "," + DBS_ATTRIBUTES_FIELD + "," + DBS_CODELOOKUP_FIELD + "," +
			DBS_CREATED_FIELD + "," + DBS_CREATEDBY_FIELD + "," + DBS_UPDATED_FIELD + "," + DBS_UPDATEDBY_FIELD +
			", dbo.GetUser(" + DBS_CREATEDBY_FIELD + ", 'USRFULLNAME') as " + DBS_UPDATEDBYUSER_FIELD +
			", dbo.GetUser(" + DBS_UPDATEDBY_FIELD + ", 'USRFULLNAME') as " + DBS_UPDATEDBYUSER_FIELD +
			" from " + DBS_TABLENAME +
			" where (" + DBS_ATTRIBUTES_FIELD + " & 0x0000000000000004) <> 0";

		private const string DBS_PRIMARYKEY = DBS_CODE_FIELD;				// Primary key of the table
		private const string DBS_CODE_FIELD = "wfCode";						// 'code' column
		private const string DBS_VERSION_FIELD = "wfVersion";				// 'version' column
		private const string DBS_XAML_FIELD = "wfXaml";						// 'xaml' column
		private const string DBS_ASSEMBLIES_FIELD = "wfAssemblies";			// 'assemblies' of this workflow - Distributed Assemblies
		private const string DBS_GROUP_FIELD = "wfGroup";					// 'group' column
		private const string DBS_NOTES_FIELD = "wfNotes";					// 'notes' column
		private const string DBS_ATTRIBUTES_FIELD = "wfAttributes";			// 'attributes' column
		private const string DBS_CODELOOKUP_FIELD = "wfCodelookup";			// 'code for code lookup' column
		private const string DBS_CREATED_FIELD = "created";					// 'created' column
		private const string DBS_CREATEDBY_FIELD = "createdby";				// 'createdby' column
		private const string DBS_CREATEDBYUSER_FIELD = "createdByUser";		// 'createdByuser' column - full name of user that created it
		private const string DBS_UPDATED_FIELD = "updated";					// 'updated' column
		private const string DBS_UPDATEDBY_FIELD = "updatedby";				// 'updatedby' column
		private const string DBS_UPDATEDBYUSER_FIELD = "updatedByUser";		// 'updatedByuser' column - full name of user that last updated it
		//
		// Attributes related bit masks
		//
		private const long MASK_READONLY = 0x0000000000000001;				// readonly - workflow cannot be modified, properties can though
		private const long MASK_SERVER = 0x0000000000000002;				// can be run on server i.e. no UI and no persist within certain bits
		private const long MASK_VISIBLEINTOOLBOX = 0x0000000000000004;		// visible in the toolbox - Note select stmt above
		private const long MASK_VISIBLEINPICKER = 0x0000000000000008;		// visible in a picker like screen designer for the user to select
		private const long MASK_SYSTEM = 0x0000000000000010;				// system workflow supplied by us
		#endregion

		#region Fields
		private Common.ConfigSetting settings = null;
		// Flag to hack the CommonObject SelectStatement property to return the correct data
		private bool isGettingGroupList = false;
		private bool isGettingToolboxList = false;
		#endregion

		#region Constructor
		public WorkflowXaml()
			: base()
		{ }
		#endregion

		#region Properties
		#region Code
		public string Code
		{
			get
			{
				return Convert.ToString(this.GetExtraInfo(DBS_CODE_FIELD));
			}
			set
			{
				if (this._data.Rows[0].RowState == DataRowState.Added)
				{
					this.SetExtraInfo(DBS_CODE_FIELD, value);
				}
			}
		}
		#endregion

		#region Version
		public long Version
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_VERSION_FIELD), 0);
			}
		}
		#endregion

		#region Xaml
		public string Xaml
		{
			get
			{
				return Convert.ToString(this.GetExtraInfo(DBS_XAML_FIELD));
			}
			set
			{
				this.SetExtraInfo(DBS_XAML_FIELD, value);
			}
		}
		#endregion

		#region Group
		public string Group
		{
			get
			{
				return Convert.ToString(this.GetExtraInfo(DBS_GROUP_FIELD));
			}
			set
			{
				this.SetExtraInfo(DBS_GROUP_FIELD, value);
			}
		}
		#endregion

		#region Notes
		public string Notes
		{
			get
			{
				return Convert.ToString(this.GetExtraInfo(DBS_NOTES_FIELD));
			}
			set
			{
				this.SetExtraInfo(DBS_NOTES_FIELD, value);
			}
		}
		#endregion

		#region UpdatedDate
		public DateTime UpdatedDate
		{
			get
			{
				return FWBS.Common.ConvertDef.ToDateTime(this.GetExtraInfo(DBS_UPDATED_FIELD), DateTime.MinValue);
			}
		}
		#endregion

		#region UpdatedBy
		public string UpdatedBy
		{
			get
			{
				object obj = this.GetExtraInfo(DBS_UPDATEDBYUSER_FIELD);
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return Convert.ToString(obj);
			}
		}
		#endregion

		#region CreatedDate
		public DateTime CreatedDate
		{
			get
			{
				return FWBS.Common.ConvertDef.ToDateTime(this.GetExtraInfo(DBS_CREATED_FIELD), DateTime.MinValue);
			}
		}
		#endregion

		#region CreatedBy
		public string CreatedBy
		{
			get
			{
				object obj = this.GetExtraInfo(DBS_CREATEDBYUSER_FIELD);
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return Convert.ToString(obj);
			}
		}
		#endregion

		#region Settings
		protected override FWBS.Common.ConfigSetting Settings
		{
			get
			{
				if (this.settings == null)
				{
					this.settings = new Common.ConfigSetting(this._data.Rows[0], DBS_ASSEMBLIES_FIELD);
				}
				return this.settings;
			}
		}
		#endregion

		#region IsVisibleInToolbox
		public bool IsVisibleInToolbox
		{
			get
			{
				return (FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & MASK_VISIBLEINTOOLBOX) != 0;
			}
			set
			{
				if (value)
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) | MASK_VISIBLEINTOOLBOX);
				}
				else
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & ~MASK_VISIBLEINTOOLBOX);
				}
			}
		}

		public bool IsVisibleInPicker
		{
			get
			{
				return (FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & MASK_VISIBLEINPICKER) != 0;
			}

			set
			{
				if (value)
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) | MASK_VISIBLEINPICKER);
				}
				else
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & ~MASK_VISIBLEINPICKER);
				}
			}
		}
		#endregion

		#region IsReadOnly
		public bool IsReadOnly
		{
			get
			{
				return (FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & MASK_READONLY) != 0;
			}

			set
			{
				if (value)
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) | MASK_READONLY);
				}
				else
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & ~MASK_READONLY);
				}
			}
		}
		#endregion

		#region IsServerWorkflow
		public bool IsServerWorkflow
		{
			get
			{
				return (FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & MASK_SERVER) != 0;
			}

			set
			{
				if (value)
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) | MASK_SERVER);
				}
				else
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & ~MASK_SERVER);
				}
			}
		}
		#endregion

		#region IsSystem
		public bool IsSystem
		{
			get
			{
				return (FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & MASK_SYSTEM) != 0;
			}

			set
			{
				if (value)
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) | MASK_SYSTEM);
					this.IsReadOnly = true;
				}
				else
				{
					this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0) & ~MASK_SYSTEM);
				}
			}
		}
		#endregion

		#region Attributes
		public long Attributes
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_ATTRIBUTES_FIELD), 0);
			}
			private set
			{
				this.SetExtraInfo(DBS_ATTRIBUTES_FIELD, value);
			}
		}
		#endregion

		#region Codelookup
		public string Codelookup
		{
			get
			{
				object obj = this.GetExtraInfo(DBS_CODELOOKUP_FIELD);
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return Convert.ToString(obj);
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					this.SetExtraInfo(DBS_CODELOOKUP_FIELD, DBNull.Value);
				}
				this.SetExtraInfo(DBS_CODELOOKUP_FIELD, value);
			}
		}
		#endregion

		#region CodeColumnName
		internal static string CodeColumnName
		{
			get { return DBS_CODE_FIELD; }
		}
		#endregion

		#region GroupColumnName
		internal static string GroupColumnName
		{
			get { return DBS_GROUP_FIELD; }
		}
		#endregion

		#region XamlColumnName
		internal static string XamlColumnName
		{
			get { return DBS_XAML_FIELD; }
		}
		#endregion
		#endregion

		#region ToString
		public override string ToString()
		{
			return this.Code;
		}
		#endregion

		#region WorkflowCopy
		internal WorkflowXaml WorkflowCopy()
		{
			WorkflowXaml wfDest = new WorkflowXaml();
			return this.WorkflowCopy(wfDest);
		}

		internal WorkflowXaml WorkflowCopy(WorkflowXaml wfDest)
		{
			wfDest.Xaml = this.Xaml;							// set the workflow xaml
			wfDest.SetDistribution(this.GetDistributions());	// set the dependent assemblies 
			wfDest.SetReferences(this.GetReferences());			// set dependent references
			wfDest.SetScriptCodes(this.GetScriptCodes());		// set dependent scripts
			wfDest.Attributes = this.Attributes;				// set attributes - isreadonly, isserver etc
			wfDest.Codelookup = this.Codelookup;				// set codelookup code - will have the same description
			wfDest.Group = this.Group;							// same group
			wfDest.Notes = this.Notes;							// same notes

			return wfDest;
		}
		#endregion

		#region GetGroups
		public HashSet<string> GetGroups()
		{
			HashSet<string> groups = new HashSet<string>();

			this.isGettingGroupList = true;
			DataTable dt = this.GetList();
			this.isGettingGroupList = false;

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				string val = (string)dt.Rows[i][0];
				groups.Add(val);
			}
			return groups;
		}
		#endregion

		#region GetToolboxList
		public DataTable GetToolboxList()
		{
			this.isGettingToolboxList = true;
			DataTable dt = this.GetList();
			this.isGettingToolboxList = false;

			return dt;
		}
		#endregion

		#region CommonObject
		protected override string SelectStatement
		{
			get
			{
				return this.isGettingGroupList ? DBS_SELECT_GROUPS_STMT : (this.isGettingToolboxList ? DBS_SELECT_TOOLBOX_STMT : DBS_SELECT_STMT);
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return DBS_TABLENAME;
			}
		}

		protected override string DefaultForm
		{
			get
			{
				return string.Empty;
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return DBS_PRIMARYKEY;
			}
		}

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		public void Fetch(string code)
		{
			Dispose();

			base.Fetch(code);
		}

		public override void Create()
		{
			Dispose();

			base.Create();
		}

		public override void Update()
		{
			if (this.State != FWBS.OMS.ObjectState.Deleted)
			{
				// Check if there are any changes made before setting the updated and updated by properties then update. - what?
				this.SetExtraInfo(DBS_ASSEMBLIES_FIELD, this.Settings.DocObject.OuterXml);
			}

			if (this._data.GetChanges() != null)
			{
				if (this.State != FWBS.OMS.ObjectState.Deleted)
				{
					this.SetExtraInfo(DBS_VERSION_FIELD, this.Version + 1);
				}
				base.Update();
			}
		}

		public override void Cancel()
		{
			base.Cancel();

			Dispose();
		}
		#endregion
	}
	#endregion
}