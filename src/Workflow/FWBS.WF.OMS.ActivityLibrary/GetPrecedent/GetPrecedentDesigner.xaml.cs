#region References
using System;
using System.Windows;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	// Interaction logic for GetPrecedentDesigner.xaml
	internal sealed partial class GetPrecedentDesigner
	{
		#region Constants
		private const string HOST_PROPERTY_NAME_TITLE = "Title";
		private const string HOST_PROPERTY_NAME_TYPE = "Type";
		private const string HOST_PROPERTY_NAME_LIBRARY = "Library";
		private const string HOST_PROPERTY_NAME_CATEGORY = "Category";
		private const string HOST_PROPERTY_NAME_SUBCATEGORY = "Subcategory";
		private const string HOST_PROPERTY_NAME_LANGUAGE = "Language";
		#endregion

		#region Constructor
		public GetPrecedentDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(GetPrecedentDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region GetPrecedentDesigner_Loaded
		void GetPrecedentDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETPREC", "Get a Precedent", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETPREC1", "Title of Precedent", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETPREC2", "Type of Precedent", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETPRECTT", "A Precedent output result", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}
		}
		#endregion

		#region btnSearchPrecedent_Click
		private void btnSearchPrecedent_Click(object sender, RoutedEventArgs e)
		{
			FWBS.OMS.Precedent precedent = FWBS.OMS.UI.Windows.Services.Searches.FindPrecedent();

			if (precedent != null)
			{
				//  Title
				System.Activities.InArgument<string> title = new System.Activities.InArgument<string>();
				title.Expression = precedent.Title;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_TITLE].ComputedValue = title;

				//  Type
				System.Activities.InArgument<string> type = new System.Activities.InArgument<string>();
				type.Expression = precedent.PrecedentType;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_TYPE].ComputedValue = type;

				//  Library
				System.Activities.InArgument<string> library = new System.Activities.InArgument<string>();
				library.Expression = precedent.Library;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_LIBRARY].ComputedValue = library;

				//  Category
				System.Activities.InArgument<string> category = new System.Activities.InArgument<string>();
				category.Expression = precedent.Category;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_CATEGORY].ComputedValue = category;

				//  Subcategory
				System.Activities.InArgument<string> subcategory = new System.Activities.InArgument<string>();
				subcategory.Expression = precedent.SubCategory;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_SUBCATEGORY].ComputedValue = subcategory;

				//  Language
				System.Activities.InArgument<string> language = new System.Activities.InArgument<string>();
				language.Expression = precedent.Language;
				this.ModelItem.Properties[HOST_PROPERTY_NAME_LANGUAGE].ComputedValue = language;
			}
		}
		#endregion
	}
}
