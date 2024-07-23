#region References
using System;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class ShowOMSItemDesigner
	{
		#region Constants
		private const string HOST_PROPERTY_NAME_CODE = "Code";
		#endregion

		#region Constructor
		public ShowOMSItemDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(ShowOMSItemDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region ShowOMSItemDesigner_Loaded
		void ShowOMSItemDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWITBDTT", "Show OMS Item", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWITINTT", "An Enquiry Name as input", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}
		}
		#endregion

		#region btnSearchScreen_Click
		private void btnSearchScreen_Click(object sender, RoutedEventArgs e)
		{
			FWBS.OMS.UI.Windows.OpenSaveEnquiry OpenSaveEnquiry1 = new FWBS.OMS.UI.Windows.OpenSaveEnquiry();
			OpenSaveEnquiry1.AllowDelete = false;
			OpenSaveEnquiry1.AllowNewFolder = false;
			OpenSaveEnquiry1.AllowRename = false;
			OpenSaveEnquiry1.Code = "CODE";
			OpenSaveEnquiry1.OpenSaveForm();
			
			if (OpenSaveEnquiry1.Execute() == System.Windows.Forms.DialogResult.OK)
			{
				System.Activities.InArgument<string> litStr = new System.Activities.InArgument<string>();
				litStr.Expression = OpenSaveEnquiry1.Code;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_CODE].ComputedValue = litStr;
			}
		}
		#endregion
	}
}
