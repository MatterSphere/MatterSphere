#region References
using System;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class OpenDocumentDesigner
	{      
		#region Constants
		private const string HOST_PROPERTY_NAME_MODE = "Mode";
		#endregion

		#region Constructor
		public OpenDocumentDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(OpenDocumentDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region OpenDocumentDesigner_Loaded
		void OpenDocumentDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			FWBS.OMS.DocOpenMode mode = (FWBS.OMS.DocOpenMode)this.ModelItem.Properties[HOST_PROPERTY_NAME_MODE].ComputedValue;
			cmbDocOpenMode.SelectedItem = mode;

			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTOPDOBDTT", "Open a Document Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTOPDOINTT", "A required Document as input", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}
		}
		#endregion

		#region ModeTypeFilter
		public bool ModeTypeFilter(Type type)
		{
			if (type == null)
			{
				return false;
			}

			if (typeof(FWBS.OMS.DocOpenMode).IsAssignableFrom(type))
			{
				return true;
			}

			return false;
		}
		#endregion

		#region cmbDocOpenMode_SelectionChanged
		private void cmbDocOpenMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.ModelItem.Properties[HOST_PROPERTY_NAME_MODE].ComputedValue = cmbDocOpenMode.SelectedItem;
		}
		#endregion
	}
}
