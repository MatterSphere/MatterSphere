#region References
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    internal partial class DistributedAssembliesWindow : Window
	{
		#region Constructors
		internal DistributedAssembliesWindow()
		{
			InitializeComponent();
		}

		internal DistributedAssembliesWindow(DataTable currentList)
			: this()
		{
			try
			{
				FWBS.OMS.Workflow.Admin.DistributedAssemblies da = new FWBS.OMS.Workflow.Admin.DistributedAssemblies();
				System.Data.DataTable dt = da.GetList();

				HashSet<string> fileNames = new HashSet<string>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					string file = dt.Rows[i][0].ToString();	// we can 0 as index since we only have a single column!
					DataRow[] rows = currentList.Select(string.Format("Name = '{0}'", file));
					if (rows.Length < 1)
					{
						fileNames.Add(file);
					}
				}
				this.listBoxDistribution.ItemsSource = fileNames;
			}
			catch (Exception ex)
			{
				// Any exception display message and exit
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
				this.DialogResult = false;
			}
		}	
		#endregion

		#region Properties
		internal HashSet<string> DistributedAssemblies
		{
			get
			{
				HashSet<string> selectedItems = new HashSet<string>();
				foreach (string str in this.listBoxDistribution.SelectedItems)
				{
					selectedItems.Add(str);
				}
				return selectedItems;
			}
		}
		#endregion

		#region OK button click
		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
		#endregion
	}

	internal partial class DistributedAssemblies : CommonObject
	{
		#region Constants
		//
		// SQL Related
		//
		private const string DBS_TABLENAME = "dbAssembly";					// SQl table name for workflow
		private const string DBS_SELECT_STMT = "select distinct " + DBS_ASSEMBLY_FIELD + " from " + DBS_TABLENAME;	// select all from the SQL table

		private const string DBS_PRIMARYKEY = DBS_ID_FIELD;					// Primary key of the table
		private const string DBS_ID_FIELD = "ID";							// 'id' column
		private const string DBS_ASSEMBLY_FIELD = "Assembly";				// 'assembly' column
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

		public override string FieldPrimaryKey
		{
			get
			{
				return DBS_PRIMARYKEY;
			}
		}
		#endregion
	}
}
