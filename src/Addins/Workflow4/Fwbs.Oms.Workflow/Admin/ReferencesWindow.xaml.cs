#region References
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FWBS.WF.Packaging;
using Microsoft.Win32;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    #region ReferencesWindow
    internal partial class ReferencesWindow : Window
	{
		#region Constants
		private const string ROLE_ADMIN = "ADMIN";			// role name for modifying system items

		// datagrid related - columnn headers displayed
		private const string COLUMN_HEADER_SYSTEM = "System";		// column header name for system i.e. global for everyone
		private const string COLUMN_HEADER_NAME1 = "File Name";		// column header name for file names
		private const string COLUMN_HEADER_NAME2 = "Script Code";	// column header name for scripts

		// datatable related ...
		private const string COLUMN_SYSTEM = "IsSystem";	// column name for system i.e. global for everyone
		private const string COLUMN_NAME = "Name";			// column name for the item's name (filename for references/distributions and script code for scripts)
		private const string COLUMN_ENABLED = "IsEnabled";	// indicates whether the system column should be enabled or not for editing
		private const int COLUMN_SYSTEM_INDEX = 0;			// index for system column - for performance
		private const int COLUMN_NAME_INDEX = 1;			// index for name column - for performance
		private const int COLUMN_ENABLED_INDEX = 2;			// index for enabled column - for performance

		// Tab indexes for the tab items defined in the XAML!
		private const int TAB_REFERENCES_INDEX = 0;								// references
		private const int TAB_DISTRIBUTIONS_INDEX = TAB_REFERENCES_INDEX +1;	// distributions
		private const int TAB_SCRIPTS_INDEX = TAB_DISTRIBUTIONS_INDEX + 1;		// workflow scripts
		private const int TAB_SYSTEMSCRIPTS_INDEX = TAB_SCRIPTS_INDEX + 1;		// system scripts
		#endregion

		#region Fields
		private HashSet<string> removedItems = new HashSet<string>();		// removed items from references/distributions/scripts

		private HashSet<string> distribution = new HashSet<string>();
		private HashSet<string> references = new HashSet<string>();
		private HashSet<string> scripts = new HashSet<string>();
		private HashSet<string> sysDistribution = new HashSet<string>();
		private HashSet<string> sysReferences = new HashSet<string>();
		private HashSet<string> sysScripts = new HashSet<string>();

		private DataTable referencesTable = new DataTable();	// References
		private DataTable distributionsTable = new DataTable();	// Distributed assemblies
		private DataTable scriptsTable = new DataTable();		// Workflow Scripts

		private bool isAdmin = false;							// indicates whether this user can add/remove system/global items
		#endregion

		#region Constructors
		private ReferencesWindow()
		{
			InitializeComponent();

			// set button properties
			this.buttonAddDA.Visibility = System.Windows.Visibility.Hidden;

			// check whether user has privilige to manipulate system/global items
			string[] roles = FWBS.OMS.Session.CurrentSession.CurrentUser.Roles.Split(',');
			foreach(string str in roles)
			{
				if (str == ROLE_ADMIN)
				{
					this.isAdmin = true;
					break;
				}
			}

			#region Setup data tables
			this.referencesTable.Columns.Add(COLUMN_SYSTEM, typeof(bool)).SetOrdinal(COLUMN_SYSTEM_INDEX);
			this.referencesTable.Columns.Add(COLUMN_NAME, typeof(string)).SetOrdinal(COLUMN_NAME_INDEX);
			this.referencesTable.Columns.Add(COLUMN_ENABLED, typeof(bool)).SetOrdinal(COLUMN_ENABLED_INDEX);
			this.referencesTable.Columns[COLUMN_NAME_INDEX].ReadOnly = true;

			this.distributionsTable.Columns.Add(COLUMN_SYSTEM, typeof(bool)).SetOrdinal(COLUMN_SYSTEM_INDEX);
			this.distributionsTable.Columns.Add(COLUMN_NAME, typeof(string)).SetOrdinal(COLUMN_NAME_INDEX);
			this.distributionsTable.Columns.Add(COLUMN_ENABLED, typeof(bool)).SetOrdinal(COLUMN_ENABLED_INDEX);
			this.distributionsTable.Columns[COLUMN_NAME_INDEX].ReadOnly = true;

			this.scriptsTable.Columns.Add(COLUMN_SYSTEM, typeof(bool)).SetOrdinal(COLUMN_SYSTEM_INDEX);
			this.scriptsTable.Columns.Add(COLUMN_NAME, typeof(string)).SetOrdinal(COLUMN_NAME_INDEX);
			this.scriptsTable.Columns.Add(COLUMN_ENABLED, typeof(bool)).SetOrdinal(COLUMN_ENABLED_INDEX);
			this.scriptsTable.Columns[COLUMN_NAME_INDEX].ReadOnly = true;
			#endregion
		}

		//
		// Constructor when used for Toolbox
		//
		internal ReferencesWindow(WorkflowStartupAssemblies startupConfig, WorkflowStartupAssemblies startupUserConfig)
			: this()
		{
			#region Save data
			this.sysReferences = startupConfig.GetReferences();
			this.sysDistribution = startupConfig.GetDistributions();
			this.sysScripts = startupConfig.GetScriptCodes();

			this.references = startupUserConfig.GetReferences();
			this.distribution = startupUserConfig.GetDistributions();
			this.scripts = startupUserConfig.GetScriptCodes();
			#endregion

			this.SetupDataGrids(false);
		}
		#endregion

		#region Properties
		// These properties MUST be set before Show() is called ...
		internal HashSet<string> RemovedItems
		{
			get
			{
				return this.removedItems;
			}
		}

		internal HashSet<string> Distribution
		{
			get
			{
				return this.distribution;
			}
			set
			{
				if (this.IsLoaded)
				{
					throw new InvalidOperationException();
				}
				// make a copy so that manipulations won't effect the source
				this.distribution = new HashSet<string>(value);
			}
		}

		internal HashSet<string> SysDistribution
		{
			get
			{
				return this.sysDistribution;
			}
			set
			{
				if (this.IsLoaded)
				{
					throw new InvalidOperationException();
				}
				// make a copy so that manipulations won't effect the source
				this.sysDistribution = new HashSet<string>(value);
			}
		}

		internal HashSet<string> References
		{
			get
			{
				return this.references;
			}
			set
			{
				if (this.IsLoaded)
				{
					throw new InvalidOperationException();
				}
				// make a copy so that manipulations won't effect the source
				this.references = new HashSet<string>(value);
			}
		}

		internal HashSet<string> SysReferences
		{
			get
			{
				return this.sysReferences;
			}
			set
			{
				if (this.IsLoaded)
				{
					throw new InvalidOperationException();
				}
				// make a copy so that manipulations won't effect the source
				this.sysReferences = new HashSet<string>(value);
			}
		}

		internal HashSet<string> Scripts
		{
			get
			{
				return this.scripts;
			}
			set
			{
				if (this.IsLoaded)
				{
					throw new InvalidOperationException();
				}
				// make a copy so that manipulations won't effect the source
				this.scripts = new HashSet<string>(value);
			}
		}

		internal HashSet<string> SysScripts
		{
			get
			{
				return this.sysScripts;
			}
			set
			{
				if (this.IsLoaded)
				{
					throw new InvalidOperationException();
				}
				// make a copy so that manipulations won't effect the source
				this.sysScripts = new HashSet<string>(value);
			}
		}
		#endregion

		#region Loaded event
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.referencesTable.BeginLoadData();
			this.referencesTable.Rows.Clear();
			this.PopulateDataTable(this.referencesTable, this.sysReferences, true);
			this.PopulateDataTable(this.referencesTable, this.references, false);
			this.referencesTable.EndLoadData();
			this.referencesTable.AcceptChanges();
			this.dataGridReferences.ItemsSource = this.referencesTable.DefaultView;

			this.distributionsTable.BeginLoadData();
			this.distributionsTable.Rows.Clear();
			this.PopulateDataTable(this.distributionsTable, this.sysDistribution, true);
			this.PopulateDataTable(this.distributionsTable, this.distribution, false);
			this.distributionsTable.EndLoadData();
			this.distributionsTable.AcceptChanges();
			this.dataGridDistribution.ItemsSource = this.distributionsTable.DefaultView;

			this.scriptsTable.BeginLoadData();
			this.scriptsTable.Rows.Clear();
			this.PopulateDataTable(this.scriptsTable, this.sysScripts, true);
			this.PopulateDataTable(this.scriptsTable, this.scripts, false);
			this.scriptsTable.EndLoadData();
			this.scriptsTable.AcceptChanges();
			this.dataGridScripts.ItemsSource = this.scriptsTable.DefaultView;
		}
		#endregion

		#region LoadingRow event
		void dataGrids_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			// disable the row if the enable column is false
			DataRowView rowView = e.Row.Item as DataRowView;
			if (rowView != null)
			{
				if ((bool)rowView.Row[COLUMN_ENABLED_INDEX] == false)
				{
					e.Row.IsEnabled = false;
				}
			}
		}
		#endregion

		#region OK button click
		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			this.references.Clear();
			this.sysReferences.Clear();
			this.PopulateHashSet(this.referencesTable, this.references, this.sysReferences, this.removedItems);

			this.distribution.Clear();
			this.sysDistribution.Clear();
			this.PopulateHashSet(this.distributionsTable, this.distribution, this.sysDistribution, this.removedItems);

			this.scripts.Clear();
			this.sysScripts.Clear();
			this.PopulateHashSet(this.scriptsTable, this.scripts, this.sysScripts, this.removedItems);

			this.DialogResult = true;
		}
		#endregion

		#region Browse Distribution button click handler
		private void buttonBrowse_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedTabIndex = this.tabControl1.SelectedIndex;

				switch (selectedTabIndex)
				{
					case -1:						// none selected, do nothing
						break;

					case TAB_REFERENCES_INDEX:		// references
					case TAB_DISTRIBUTIONS_INDEX:	// distributed assemblies
						{
							OpenFileDialog dlg = new OpenFileDialog();
							dlg.DefaultExt = "dll";
							dlg.Filter = "Assembly (*.dll)|*.dll|All Files (*.*)|*.*";
							dlg.Title = "Browse for Assembly";
							dlg.Multiselect = true;
							dlg.CheckFileExists = true;
							dlg.ValidateNames = true;

							if (dlg.ShowDialog(this) == true)
							{
								foreach (string fileName in dlg.FileNames)
								{
									// validate filename - will throw an exception
									System.IO.FileInfo info = new System.IO.FileInfo(fileName);

									// check whether the file is already in references/distributions list
									bool add = true;
									DataRow[] rows = this.distributionsTable.Select(string.Format("{0} = '{1}'", COLUMN_NAME, info.FullName));
									if (rows.Length == 0)
									{
										// check whether the short name exists
										rows = this.distributionsTable.Select(string.Format("{0} = '{1}'", COLUMN_NAME, info.Name));
									}
									if (rows.Length > 0)
									{
										// Display message this is in DAs and will not be added
										FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("{0} already exists as a Distributed Assembly!", info.Name));
										add = false;
									}
									else
									{
										rows = this.referencesTable.Select(string.Format("{0} = '{1}'", COLUMN_NAME, info.Name));
										if (rows.Length > 0)
										{
											// Display message this is already in references and will not be added
											FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("{0} already exists as a reference!", info.Name));
											add = false;
										}
									}

									if (add)
									{
										if (selectedTabIndex == TAB_DISTRIBUTIONS_INDEX)
										{
											this.AddNewRow(this.distributionsTable, info.FullName);
										}
										else
										{
											this.AddNewRow(this.referencesTable, info.Name);
										}
									}
								}
							}
						}
						break;

					case TAB_SCRIPTS_INDEX:		// scripts
						{
							ScriptsWindow dlg = new ScriptsWindow(this.scriptsTable);
							if (dlg.ShowDialog() == true)
							{
								foreach (string code in dlg.ScriptCodes)
								{
									// check whether the name is already in references/distributions list
									bool add = true;
									DataRow[] rows = this.distributionsTable.Select(string.Format("{0} = '{1}.dll'", COLUMN_NAME, code));
									if (rows.Length > 0)
									{
										// Display message this is in DAs and will not be added
										FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("{0} already exists as a Distributed Assembly!", code));
										add = false;
									}
									else
									{
										rows = this.referencesTable.Select(string.Format("{0} = '{1}.dll'", COLUMN_NAME, code));
										if (rows.Length > 0)
										{
											// Display message this is already in references and will not be added
											FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("{0} already exists as a reference!", code));
											add = false;
										}
									}

									if (add)
									{
										this.AddNewRow(this.scriptsTable, code);
									}
								}
							}
						}
						break;

				}
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
		}
		#endregion

		#region Remove button click handler
		private void buttonRemove_Click(object sender, RoutedEventArgs e)
		{
			System.Collections.IList selectedItems = null;
			if (this.tabReferences.IsSelected)
			{
				selectedItems = this.dataGridReferences.SelectedItems;
			}
			else if (this.tabDistribution.IsSelected)
			{
				selectedItems = this.dataGridDistribution.SelectedItems;
			}
			else if (this.tabScripts.IsSelected)
			{
				selectedItems = this.dataGridScripts.SelectedItems;
			}

			if (selectedItems != null)
			{
				// The deletion is done top down. This is because as the row is deleted, it is also deleted from the this list
				for (int i = selectedItems.Count; i > 0; i--)
				{
					DataRowView rowView = selectedItems[i - 1] as DataRowView;
					rowView.Delete();
				}
			}
		}
		#endregion

		#region Add DA button click handler
		private void buttonAddDA_Click(object sender, RoutedEventArgs e)
		{
			DistributedAssembliesWindow dlg = new DistributedAssembliesWindow(this.distributionsTable);
			if (dlg.ShowDialog() == true)
			{
				// add to the list
				foreach (string file in dlg.DistributedAssemblies)
				{
					this.AddNewRow(this.distributionsTable, file);
				}
			}
		}
		#endregion

		#region PopulateDataTable
		private void PopulateDataTable(DataTable dt, HashSet<string> refs, bool isSystem)
		{
			foreach (string str in refs)
			{
				this.AddNewRow(dt, str);
			}
		}
		#endregion

		#region Add new row to datatable
		private void AddNewRow(DataTable table, string name)
		{
			bool isSystem = false;
			DataRow row = table.NewRow();
			row[COLUMN_SYSTEM_INDEX] = isSystem;
			row[COLUMN_ENABLED_INDEX] = !isSystem || (isSystem && this.isAdmin);
			row[COLUMN_NAME_INDEX] = name;
			table.Rows.Add(row);
		}
		#endregion

		#region PopulateHashSet
		private void PopulateHashSet(
			DataTable dt,
			HashSet<string> local,
			HashSet<string> global,
			HashSet<string> removed)
		{
			foreach (DataRow row in dt.Rows)
			{
				switch (row.RowState)
				{
					case DataRowState.Added:		// New row
					case DataRowState.Modified:
					case DataRowState.Unchanged:
						{
							string filename = row[COLUMN_NAME_INDEX].ToString();
							if ((bool)row[COLUMN_SYSTEM_INDEX])
							{
								global.Add(filename);
							}
							else
							{
								local.Add(filename);
							}
						}
						break;

					case DataRowState.Deleted:		// Existing row deleted
						{
							object obj = row[COLUMN_NAME_INDEX, DataRowVersion.Original];
							if (obj != null)
							{
								removed.Add(obj as string);
							}
						}
						break;

					case DataRowState.Detached:		// New row added then deleted
						break;
				}
			}
		}
		#endregion

		#region tabControl1_SelectionChanged
		private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// display/hide the button depending on the selected tab...
			this.buttonAddDA.Visibility = this.tabDistribution.IsSelected ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
		}
		#endregion

		#region SetupDataGrids
		private void SetupDataGrids(bool isScript)
		{
			DataGridCheckBoxColumn col1 = new DataGridCheckBoxColumn();
			col1.Header = COLUMN_HEADER_SYSTEM;
			col1.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
			col1.Binding = new Binding(COLUMN_SYSTEM);
			if (!isScript)
			{
				this.dataGridReferences.Columns.Add(col1);
			}

			DataGridTextColumn col2 = new DataGridTextColumn();
			col2.Header = COLUMN_HEADER_NAME1;
			col2.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			col2.Binding = new Binding(COLUMN_NAME);
			col2.IsReadOnly = true;
			this.dataGridReferences.Columns.Add(col2);

			col1 = new DataGridCheckBoxColumn();
			col1.Header = COLUMN_HEADER_SYSTEM;
			col1.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
			col1.Binding = new Binding(COLUMN_SYSTEM);
			if (!isScript)
			{
				this.dataGridDistribution.Columns.Add(col1);
			}

			col2 = new DataGridTextColumn();
			col2.Header = COLUMN_HEADER_NAME1;
			col2.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			col2.Binding = new Binding(COLUMN_NAME);
			col2.IsReadOnly = true;
			this.dataGridDistribution.Columns.Add(col2);

			col1 = new DataGridCheckBoxColumn();
			col1.Header = COLUMN_HEADER_SYSTEM;
			col1.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
			col1.Binding = new Binding(COLUMN_SYSTEM);
			this.dataGridScripts.Columns.Add(col1);

			col2 = new DataGridTextColumn();
			col2.Header = COLUMN_HEADER_NAME2;
			col2.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			col2.Binding = new Binding(COLUMN_NAME);
			col2.IsReadOnly = true;
			this.dataGridScripts.Columns.Add(col2);

			this.dataGridReferences.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrids_LoadingRow);
			this.dataGridDistribution.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrids_LoadingRow);
			this.dataGridScripts.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrids_LoadingRow);
		}
		#endregion
	}
	#endregion
}
