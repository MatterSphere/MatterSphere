using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows.Design
{
    internal class PrinterLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList ar = new ArrayList();
			try { ar.AddRange(PrinterSettings.InstalledPrinters); }
			catch { }
			return new StandardValuesCollection(ar);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
	}

	internal class ScriptMenuCommands : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
            List<FWBS.OMS.Script.ScriptGen> scripts = new List<FWBS.OMS.Script.ScriptGen>(FWBS.OMS.Script.ScriptGen.GetMenuScripts());

            ArrayList list = new ArrayList();

            foreach (FWBS.OMS.Script.ScriptGen script in scripts)
            {
                FWBS.Common.ConfigSettingItem[] staticprocs = script.GetStaticProcedures();

                foreach (FWBS.Common.ConfigSettingItem configSetting in staticprocs)
                {
                    string scriptCode = "SCRIPT;" + configSetting.GetString("name", "");
                    if (!list.Contains(scriptCode))
                        list.Add(scriptCode);
                }
            }

            return new StandardValuesCollection(list);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
	}

	/// <summary>
	/// Summary description for AssociatedFormatsEditor.
	/// </summary>
	public class AssociatedFormatsEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public AssociatedFormatsEditor(){}


		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			FWBS.OMS.UI.Windows.Design.frmAssociatedFormats frmAssociatedFormats1 = new frmAssociatedFormats();
			FWBS.OMS.ContactType.AssociatedFormats af = value as FWBS.OMS.ContactType.AssociatedFormats;
			frmAssociatedFormats1.SelectionList.InputDisplayMember = "typedesc";
			frmAssociatedFormats1.SelectionList.InputValueMember = "typecode";
			frmAssociatedFormats1.SelectionList.OutputDisplayMember = "typedesc";
			frmAssociatedFormats1.SelectionList.OutputValueMember = "typecode";
			frmAssociatedFormats1.SelectionList.InputData = af.GetAllAssociatedTypes;
			frmAssociatedFormats1.SelectionList.OutputData = af.GetAssociatedTypes;
			frmAssociatedFormats1.SelectionList.RefreshData();
			iWFES.ShowDialog(frmAssociatedFormats1);
			if (frmAssociatedFormats1.DialogResult == DialogResult.OK)
			{
				DataTable dt = af.GetAssociatedTypes;
				foreach (DataRow sl in frmAssociatedFormats1.SelectionList.SelectedItems.Rows)
				{
					bool found = false;
					foreach (DataRow dr in dt.Rows)
					{
						if (Convert.ToString(sl["Value"]).ToUpper() == Convert.ToString(dr["assocType"]).ToUpper()) 
						{
							found = true;
							break;
						}
					}
					if (!found)
					{
						DataRow dr = dt.NewRow();
						dr["contType"] = af.ContactTypeCode;
						dr["assocType"] = sl["Value"];
						dt.Rows.Add(dr);
					}
				}
				
				foreach (DataRow dr in dt.Rows)
				{
					bool found = false;
					foreach (DataRow sl in frmAssociatedFormats1.SelectionList.SelectedItems.Rows)
					{
						if (Convert.ToString(sl["Value"]).ToUpper() == Convert.ToString(dr["assocType"]).ToUpper()) 
						{
							found = true;
							break;
						}
					}
					if (!found) dr.Delete();
				}


				af.Update();
				MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("ASSTYPEUP","Associated Types have been Updated...",""), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Information);
				return af;
			}
			else
			return value;
		}
	}

	public class StorageProviderEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public StorageProviderEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			FWBS.OMS.StorageProvider _input = value as FWBS.OMS.StorageProvider;
			
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmListSelector frmListSelector1 = new frmListSelector();
			frmListSelector1.Text = OMS.Session.CurrentSession.Resources.GetResource("STORAGEPRO","Storage Providers","").Text;
			FWBS.OMS.EnquiryEngine.DataLists _data = new DataLists("DSSTOREPROVS");
			DataTable _storage = _data.Run() as DataTable;
			frmListSelector1.List.DisplayMember = "spdesc";
			frmListSelector1.List.ValueMember = "spid";
			frmListSelector1.List.DataSource = _storage;

			if (Convert.ToString(_input.ID) != "")
				frmListSelector1.List.SelectedValue = _input.ID;
			frmListSelector1.ShowHelp = true;
			iWFES.ShowDialog(frmListSelector1);
			if (frmListSelector1.DialogResult == DialogResult.OK)
			{
				FWBS.OMS.StorageProvider output = new FWBS.OMS.StorageProvider(Convert.ToInt16(frmListSelector1.List.SelectedValue));
				value = output;
			}
			return value;
		}
	}

    public class ApplicationEditor : UITypeEditor
    {
        private IWindowsFormsEditorService iWFES;
        public ApplicationEditor() { }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext ctx)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            FWBS.OMS.RegisteredApplication _input = value as FWBS.OMS.RegisteredApplication;

            iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            frmListSelector frmListSelector1 = new frmListSelector();
            frmListSelector1.Text = OMS.Session.CurrentSession.Resources.GetResource("APPLICATIONS", "Applications", "").Text;
            FWBS.OMS.EnquiryEngine.DataLists _data = new DataLists("DSAPPLICATIONS");
            DataTable _apps = _data.Run() as DataTable;
            frmListSelector1.List.DisplayMember = "appname";
            frmListSelector1.List.ValueMember = "appid";
            frmListSelector1.List.DataSource = _apps;

            if (Convert.ToString(_input.ID) != "")
                frmListSelector1.List.SelectedValue = _input.ID;
            frmListSelector1.ShowHelp = true;
            iWFES.ShowDialog(frmListSelector1);
            if (frmListSelector1.DialogResult == DialogResult.OK)
            {
                FWBS.OMS.RegisteredApplication output = FWBS.OMS.RegisteredApplication.GetApplication(Convert.ToInt16(frmListSelector1.List.SelectedValue));
                value = output;
            }
            return value;
        }
    }


	/// <summary>
	/// Summary description for CurrencyLister
	/// </summary>
	internal class CurrencyLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			Currency cr = new Currency();
			DataTable dt = cr.GetList();
			ArrayList ar = new ArrayList();
			foreach(DataRow row in dt.Rows)
				ar.Add(Convert.ToString(row[0]));
			cr.Dispose();
			return new StandardValuesCollection(ar);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
	}
	
	/// <summary>
	/// Summary description for ScriptLister
	/// </summary>
	public class ScriptLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string scripttype = "";
			DataTable dt;
			try
			{
				scripttype = ((FWBS.OMS.ScriptTypeParamAttribute)context.PropertyDescriptor.Attributes[typeof(FWBS.OMS.ScriptTypeParamAttribute)]).Code;
			}
			catch{}
			if (scripttype == "")
			{
				return new StandardValuesCollection(new string[0]);
			}
			else
			{
				dt = FWBS.OMS.Script.ScriptGen.GetScripts(scripttype);
				ArrayList ar = new ArrayList();
				foreach(DataRow row in dt.Rows)
					ar.Add(Convert.ToString(row[0]));
				return new StandardValuesCollection(ar);
			}
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
	}
	
	/// <summary>
	/// Summary description for SearchListButtonerLister
	/// </summary>
	internal class SearchListButtonLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			FWBS.OMS.UI.Windows.Design.ButtonsCollection slbuttons = (FWBS.OMS.UI.Windows.Design.ButtonsCollection)(TypeDescriptor.GetProperties(context.Instance)["Buttons"].GetValue(context.Instance));
			ArrayList ar = new ArrayList();
			ar.Add("None");
			foreach(FWBS.OMS.UI.Windows.Design.Buttons bt in slbuttons)
				ar.Add(bt.Name);
			return new StandardValuesCollection(ar);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
	}
	

	/// <summary>
	/// Summary description for FieldNamesLister
	/// </summary>
	internal class FieldNamesLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string table = "";
			DataTable dt;
			table = ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)context.Instance).ExtendedData.Code;
			if (table != "")
			{
				FWBS.OMS.UI.Windows.Design.ExtendedDataEditor ext = new FWBS.OMS.UI.Windows.Design.ExtendedDataEditor(table);
				return new StandardValuesCollection(ext.DataBuilder.TestAndGetFields());
			}
			else
			{
				
				table = ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)context.Instance).BoundTable;
				if (table == "") 
				{
					MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("PLZBNDTBL","Please select a Table from the Bound Table Property above",""),FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return new StandardValuesCollection(new string[0]{});
				}
                dt = ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)context.Instance).EnquiryForm.Enquiry.GetColumns(table);
				ArrayList ar = new ArrayList();
				foreach(DataRow row in dt.Rows)
					ar.Add(Convert.ToString(row[0]));
				return new StandardValuesCollection(ar);
			}
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
	}

	/// <summary>
	/// Summary description for EnquryControlStaticDefaults.
	/// </summary>
	internal class EnquryControlStaticDefaults : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string[] vals = new string[2]{"%#UI%","%#NOW%"};
			return new StandardValuesCollection(vals);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

	}
	
	/// <summary>
	/// Summary description for EnquryControlTypeTypeEditor.
	/// </summary>
	internal class DataTypesLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string[] vals;
			Array arr = Enum.GetValues(typeof(System.TypeCode));
			vals = new string[arr.Length];
			for (int ctr = 0; ctr < arr.Length; ctr++)
			{
				vals[ctr] = "System." + Enum.GetName(typeof(System.TypeCode), arr.GetValue(ctr));
			}
			return new StandardValuesCollection(vals);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

	}

	/// <summary>
	/// Summary description for BusinessMappedPropertiesLister.
	/// </summary>
	internal class BusinessMappedPropertiesLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			if (context.Instance is FWBS.OMS.OMSType.Panel)
			{
				FWBS.OMS.OMSType.Panel pnl = context.Instance as FWBS.OMS.OMSType.Panel;
                EnquiryEngine.EnquiryPropertyCollection props = Enquiry.GetObjectProperties(pnl.OMSObjectType);
                
                List<string> vals = new List<string>();
                foreach (EnquiryEngine.EnquiryProperty prop in props)
                    vals.Add(prop.Name);
					
				return new StandardValuesCollection(vals.ToArray());
			}
			else if (context.Instance is FWBS.OMS.UI.Windows.Admin.EnquiryControl)
			{
				FWBS.OMS.UI.Windows.Admin.EnquiryControl enc = context.Instance as FWBS.OMS.UI.Windows.Admin.EnquiryControl;
				string enq = Convert.ToString(enc.EnquiryForm.Enquiry.Source.Tables["ENQUIRY"].Rows[0]["enqSource"]);
				try
				{
					EnquiryEngine.EnquiryPropertyCollection props = Enquiry.GetObjectProperties(enq);
                    List<string> vals = new List<string>();
                    foreach (EnquiryEngine.EnquiryProperty prop in props)
                        vals.Add(prop.Name);
					return new StandardValuesCollection(vals.ToArray());
				}
				catch
				{
					return new StandardValuesCollection(new string[0]);
				}
			}
			else
			{
				return new StandardValuesCollection(new string[3]{"Client","File","FileDetailed"});
			}
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
	}

	/// <summary>
	/// Implements a custom type editor for selecting a in a list
	/// </summary>
	internal class FlagsEditor : UITypeEditor
	{
		/// <summary>
		/// Internal class used for storing custom data in listviewitems
		/// </summary>
		internal class clbItem
		{
			/// <summary>
			/// Creates a new instance of the <c>clbItem</c>
			/// </summary>
			/// <param name="str">The string to display in the <c>ToString</c> method. 
			/// It will contains the name of the flag</param>
			/// <param name="value">The integer value of the flag</param>
			/// <param name="tooltip">The tooltip to display in the <see cref="CheckedListBox"/></param>
			public clbItem(string str, int value, string tooltip)
			{
				this.str = str;
				this.value = value;
				this.tooltip = tooltip;
			}

			private string str;

			private int value;
			/// <summary>
			/// Gets the int value for this item
			/// </summary>
			public int Value
			{
				get{return value;}
			}

			private string tooltip;

			/// <summary>
			/// Gets the tooltip for this item
			/// </summary>
			public string Tooltip
			{
				get{return tooltip;}
			}

			/// <summary>
			/// Gets the name of this item
			/// </summary>
			/// <returns>The name passed in the constructor</returns>
			public override string ToString()
			{
				return str;
			}
		}

		private IWindowsFormsEditorService edSvc = null;
		private CheckedListBox clb;
		private ToolTip tooltipControl;

		/// <summary>
		/// Overrides the method used to provide basic behaviour for selecting editor.
		/// Shows our custom control for editing the value.
		/// </summary>
		/// <param name="context">The context of the editing control</param>
		/// <param name="provider">A valid service provider</param>
		/// <param name="value">The current value of the object to edit</param>
		/// <returns>The new value of the object</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null
				&& context.Instance != null
				&& provider != null) 
			{

				edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

				if (edSvc != null) 
				{					
					// Create a CheckedListBox and populate it with all the enum values
					clb = new CheckedListBox();
					clb.BorderStyle = BorderStyle.FixedSingle;
					clb.CheckOnClick = true;
					clb.MouseDown += new MouseEventHandler(this.OnMouseDown);
					clb.MouseMove += new MouseEventHandler(this.OnMouseMoved);

					tooltipControl = new ToolTip();
					tooltipControl.ShowAlways = true;

                    //Sorts the enum names
                    string[] names = Enum.GetNames(context.PropertyDescriptor.PropertyType);
                    Array.Sort<string>(names);

					foreach(string name in names)
					{
						// Get the enum value
						object enumVal = Enum.Parse(context.PropertyDescriptor.PropertyType, name);
						// Get the int value 
						int intVal = (int) Convert.ChangeType(enumVal, typeof(int));
						if (intVal > 0)
						{
							// Get the description attribute for this field
							System.Reflection.FieldInfo fi = context.PropertyDescriptor.PropertyType.GetField(name);
							DescriptionAttribute[] attrs = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

							// Store the the description
							string tooltip = attrs.Length > 0 ? attrs[0].Description : string.Empty;

							// Get the int value of the current enum value (the one being edited)
							int intEdited = (int) Convert.ChangeType(value, typeof(int));

							// Creates a clbItem that stores the name, the int value and the tooltip
							clbItem item = new clbItem(enumVal.ToString(), intVal, tooltip);

							// Get the checkstate from the value being edited
							bool checkedItem = false;
						
							if ((intEdited | intVal) == intEdited)
							{
								checkedItem = true;
							}
							
							// Add the item with the right check state
							clb.Items.Add(item, checkedItem);
						}
					}					

					// Show our CheckedListbox as a DropDownControl. 
					// This methods returns only when the dropdowncontrol is closed
					edSvc.DropDownControl(clb);

					// Get the sum of all checked flags
					int result = 0;
					foreach(clbItem obj in clb.CheckedItems)
					{
						result |= obj.Value;
					}
					
					// return the right enum value corresponding to the result
					return Enum.ToObject(context.PropertyDescriptor.PropertyType, result);
				}
			}

			return value;
		}

		/// <summary>
		/// Shows a dropdown icon in the property editor
		/// </summary>
		/// <param name="context">The context of the editing control</param>
		/// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return UITypeEditorEditStyle.DropDown;			
		}

		private bool handleLostfocus = false;

		/// <summary>
		/// When got the focus, handle the lost focus event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseDown(object sender, MouseEventArgs e) 
		{
			if(!handleLostfocus && clb.ClientRectangle.Contains(clb.PointToClient(new Point(e.X, e.Y))))
			{
				clb.LostFocus += new EventHandler(this.ValueChanged);
				handleLostfocus = true;
			}
		}

		/// <summary>
		/// Occurs when the mouse is moved over the checkedlistbox. 
		/// Sets the tooltip of the item under the pointer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseMoved(object sender, MouseEventArgs e) 
		{			
			int index = clb.IndexFromPoint(e.X, e.Y);
			if(index >= 0)
				tooltipControl.SetToolTip(clb, ((clbItem) clb.Items[index]).Tooltip);
		}

		/// <summary>
		/// Close the dropdowncontrol when the user has selected a value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ValueChanged(object sender, EventArgs e) 
		{
			if (edSvc != null) 
			{
				edSvc.CloseDropDown();
			}
		}
	}

	
	/// <summary>
	/// Summary description for IListEnquiryControlSourceLister
	/// </summary>
	internal class IListEnquiryControlSourceLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
	
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList fields = new ArrayList();
			try
			{
				FWBS.OMS.UI.Windows.EnquiryForm enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance);
				string ctl = TypeDescriptor.GetProperties(context.Instance)["Control"].GetValue(context.Instance).ToString();
				foreach(Control ctrl in enq.Controls)
				{
					if (ctrl.Name == ctl)
					{
						FWBS.Common.UI.IBasicEnquiryControl2 list = (FWBS.Common.UI.IBasicEnquiryControl2)ctrl;
						if (list.Control is ComboBox)
						{
							if (((ComboBox)list.Control).DataSource is DataTable)
							{
								DataTable data = (DataTable)((ComboBox)list.Control).DataSource;
								foreach(DataColumn cm in data.Columns)
								{
									fields.Add(cm.ColumnName);
								}
							}
							else if (((ComboBox)list.Control).DataSource is DataView)
							{
								DataView data = (DataView)((ComboBox)list.Control).DataSource;
								foreach(DataColumn cm in data.Table.Columns)
								{
									fields.Add(cm.ColumnName);
								}
							}
						}
						else if (list.Control is ListBox)
						{
							if (((ListBox)list.Control).DataSource is DataTable)
							{
								DataTable data = (DataTable)((ListBox)list.Control).DataSource;
								foreach(DataColumn cm in data.Columns)
								{
									fields.Add(cm.ColumnName);
								}
							}
							else if (((ListBox)list.Control).DataSource is DataView)
							{
								DataView data = (DataView)((ListBox)list.Control).DataSource;
								foreach(DataColumn cm in data.Table.Columns)
								{
									fields.Add(cm.ColumnName);
								}
							}
						}
						else if (list.Control is FWBS.Common.UI.Windows.eXPComboBox)
						{
							if (((FWBS.Common.UI.Windows.eXPComboBox)list.Control).DataSource is DataTable)
							{
								DataTable data = (DataTable)((FWBS.Common.UI.Windows.eXPComboBox)list.Control).DataSource;
								foreach(DataColumn cm in data.Columns)
								{
									fields.Add(cm.ColumnName);
								}
							}
							else if (((FWBS.Common.UI.Windows.eXPComboBox)list.Control).DataSource is DataView)
							{
								DataView data = (DataView)((FWBS.Common.UI.Windows.eXPComboBox)list.Control).DataSource;
								foreach(DataColumn cm in data.Table.Columns)
								{
									fields.Add(cm.ColumnName);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ex);
			}
			string[] vals;
			vals = new string[fields.Count];
			fields.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}
	}
		
	/// <summary>
	/// Summary description for IListEnquiryControlLister
	/// </summary>
	internal class IListEnquiryControlLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList controls = new ArrayList();
			try
			{
				FWBS.OMS.UI.Windows.EnquiryForm enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance);
				string ctl = TypeDescriptor.GetProperties(context.Instance)["ControlName"].GetValue(context.Instance).ToString();
				foreach(Control ctrl in enq.Controls)
				{
					if (ctrl is FWBS.Common.UI.IListEnquiryControl)
					{
						if (ctrl.Name != ctl)
							controls.Add(ctrl.Name);
					}
				}
			}
			catch
			{}

			string[] vals;
			vals = new string[controls.Count];
			controls.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

	}

	/// <summary>
	/// Summary description for Mile Stone Plan Lister
	/// </summary>
	internal class MileStonePlanLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList msplans = new ArrayList();
			msplans.Add("");
			try
			{
				foreach(DataRow dr in Milestones_OMS2K.GetMilestonePlans(true).Rows)
					msplans.Add(Convert.ToString(dr["MSCode"]));
			}
			catch
			{}

			string[] vals;
			vals = new string[msplans.Count];
			msplans.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

	}

	/// <summary>
	/// Summary description for Page Lister
	/// </summary>
	internal class PageLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList pages = new ArrayList();
			try
			{
				FWBS.OMS.UI.Windows.EnquiryForm enq = null;
				object contextSingleInstance = null;

				if (context.Instance is System.Array)
				{
					contextSingleInstance = ((System.Array)context.Instance).GetValue(0);
					enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(contextSingleInstance)["EnquiryForm"].GetValue(contextSingleInstance);
				}
				else
				{
					enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance);
				}
				foreach(DataRow dr in enq.Enquiry.Source.Tables["PAGES"].Rows)
					pages.Add(Convert.ToString(dr["pgeName"]));
				pages.Add("NONE");
			}
			catch
			{}

			string[] vals;
			vals = new string[pages.Count];
			pages.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

	}

	/// <summary>
	/// Summary description for CoolButtonLister.
	/// </summary>
	internal class CoolButtonLister : Int16Converter
	{
        private Int32[] s;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
            if (s == null)
            {
                ImageList img = FWBS.OMS.UI.Windows.Images.CoolButtons16();
                s = new Int32[img.Images.Count + 1];
                for (int i = -1; i <= img.Images.Count - 1; i++)
                    s[i + 1] = i;
            }
			return new StandardValuesCollection(s);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

	}

    /// <summary>
    /// Summary description for NavigationIconsLister.
    /// </summary>
    internal class NavigationIconsLister : Int16Converter
    {
        private Int32[] s;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (s == null)
            {
                ImageList img = FWBS.OMS.UI.Windows.Images.GetNavigationIconsBySize(Images.IconSize.Size16);
                s = new Int32[img.Images.Count + 1];
                for (int i = -1; i <= img.Images.Count - 1; i++)
                {
                    s[i + 1] = i;
                }
            }

            return new StandardValuesCollection(s);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    /// <summary>
    /// Summary description for DialogIconsLister
    /// </summary>
    internal class DialogIconsLister : Int16Converter
    {
        private Int32[] s;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (s == null)
            {
                ImageList img = FWBS.OMS.UI.Windows.Images.GetDialogIconsBySize(Images.IconSize.Size16);
                s = new Int32[img.Images.Count + 1];
                for (int i = -1; i <= img.Images.Count - 1; i++)
                {
                    s[i + 1] = i;
                }
            }

            return new StandardValuesCollection(s);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    /// <summary>
    /// Summary description for EntitiesLister.
    /// </summary>
    internal class EntitiesLister : Int16Converter
	{
        int ec = 0;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
            if (ec == 0) ec = FWBS.OMS.UI.Windows.Images.Entities().Images.Count;
            Int32[] s = new Int32[ec + 1];
            for (int i = -1; i <= ec - 1; i++)
				s[i+1] = i;
			return new StandardValuesCollection(s);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

	}
	
	/// <summary>
	/// Summary description for EnquryControlTypeTypeEditor.
	/// </summary>
	internal class BoundTableLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string _table = "";
			try
			{
				_table = Convert.ToString(((FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance)).Enquiry.Source.Tables["ENQUIRY"].Rows[0]["enqCall"]);
			}
			catch
			{}
			if (_table == "")
			{
				MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("NODTBUILD","No Data Builder has been set or the Data Binding is not in Bound Mode",""), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return null;
			}
			string[] vals;
			vals = new string[2];
			vals[0] = "";
			vals[1] = _table;
			return new StandardValuesCollection(vals);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

	}


	public class IconDisplayEditor : UITypeEditor
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
                        if (imageList != null)
                        {
                            int index = Convert.ToInt32(e.Value);
                            if (index < imageList.Images.Count && index > -1)
                            {
                                Image image = imageList.Images[index];
                                e.Graphics.DrawImage(image, e.Bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                            }
                        }
					}
					catch
					{}
				}
			}
		}
	}
}
