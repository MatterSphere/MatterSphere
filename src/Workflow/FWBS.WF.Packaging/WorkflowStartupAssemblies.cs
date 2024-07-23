#region References
using System;
using System.Data;
#endregion

namespace FWBS.WF.Packaging
{
    #region WorkflowStartupAssemblies
    public sealed class WorkflowStartupAssemblies : WorkflowCommonObject
	{
		#region Constants
		// SQL Related
		private const string DBS_TABLENAME = "dbWorkflowConfig";			// SQl table name for workflow
		private const string DBS_SELECT_STMT = "select * from " + DBS_TABLENAME;	// select all from the SQL table
		private const string DBS_PRIMARYKEY = DBS_CODE_FIELD;					// Primary key of the table
		private const string DBS_CODE_FIELD = "wfCode";						// 'code' column
		private const string DBS_VERSION_FIELD = "wfVersion";				// 'version' column
		private const string DBS_ASSEMBLIES_FIELD = "wfConfig";				// assemblies to load for toolbox at startup - Distributed Assemblies
		#endregion

		#region Fields
		/// <summary>
		/// An xml representation of the referenced assemblies.
		/// </summary>
		private Common.ConfigSetting settings = null;
		#endregion

		#region Constructors
		public WorkflowStartupAssemblies()
			: base()
		{
		}
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
		#endregion

		#region CommonObject
		protected override string SelectStatement
		{
			get
			{
				return DBS_SELECT_STMT;
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
			// Check if there are any changes made before setting the updated and updated by properties then update. - what?
			this.SetExtraInfo(DBS_ASSEMBLIES_FIELD, this.Settings.DocObject.OuterXml);

			if (this._data.GetChanges() != null)
			{
				this.SetExtraInfo(DBS_VERSION_FIELD, this.Version + 1);
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
