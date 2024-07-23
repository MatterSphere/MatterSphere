#region References
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
#endregion

namespace FWBS.OMS.Workflow
{
    public partial class WorkflowDesignActivityDesigner
	{
		#region Constants
		private const string HOST_PROPERTY_NAME_CODE = "Code";				// name of 'code' property in the host i.e. the activity
		private const string HOST_PROPERTY_NAME_ARGUMENTS = "Arguments";	// name of 'arguments' property in the host i.e. the activity
		#endregion

		#region Constructor
		public WorkflowDesignActivityDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(WorkflowDesignActivityDesigner_Loaded);
		}
		#endregion

		#region WorkflowDesignActivityDesigner_Loaded
		void WorkflowDesignActivityDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.ModelItem != null)
			{
				// check whether this an edit of a saved workflow i.e. loading an existing one which will have its code and arguments set
				//	we check the 'Code' rather than an empty 'Arguments' since the workflow may not have any arguments!
				string code = this.ModelItem.Properties[HOST_PROPERTY_NAME_CODE].ComputedValue as string;
				if ((code == null) || string.IsNullOrWhiteSpace(code))
				{
					// brand new drag and drop so ContextItemWorkflow should contain the 'LATEST' code and xaml from the selected item in toolbox
					ContextItemWorkflow ciw = this.ModelItem.GetEditingContext().Items.GetValue<ContextItemWorkflow>();
					Dictionary<string, Argument> arguments = this.SetupArguments(ciw.Xaml);

					this.ModelItem.Properties["DisplayName"].SetValue(ciw.Code);				// sets the title in the designer
					this.ModelItem.Properties[HOST_PROPERTY_NAME_CODE].SetValue(ciw.Code);
					this.ModelItem.Properties[HOST_PROPERTY_NAME_ARGUMENTS].SetValue(arguments);
				}
			}
		}
		#endregion

		#region SetupArguments
		private Dictionary<string, Argument> SetupArguments(string xml)
		{
			Dictionary<string, Argument> Arguments = new Dictionary<string, Argument>();
			XNamespace aw = "http://schemas.microsoft.com/winfx/2006/xaml";
			XDocument doc = XDocument.Parse(xml);
			IEnumerable<XElement> members = from el in doc.Descendants(aw + "Members") select el;
			var nodes = members.Nodes();
			// <x:Property Name="argument1" Type="InArgument(x:String)" 
			// <x:Property Name="argument2" Type="InArgument(fo:OMSFile)" 
			foreach (XElement xmlArg in nodes)
			{
				string argName = string.Empty;		// name of attribute
				string argWFType = string.Empty;	// InArgument/InOutArgument/OutArgument
				string argType = string.Empty;		// argument type

				// get name
				argName = xmlArg.Attribute("Name").Value;

				// get workflow argument type
				string typeStr = xmlArg.Attribute("Type").Value;
				int index = typeStr.IndexOf('(');
				argWFType = typeStr.Substring(0, index);

				// get argument type
				int index2 = typeStr.IndexOf(')');
				string tmpStr = typeStr.Substring(index + 1, index2 - index - 1);
				index = tmpStr.IndexOf(':');
				string prefix = tmpStr.Substring(0, index);
				string type = tmpStr.Substring(index + 1, tmpStr.Length - index - 1);
				XNamespace nm = xmlArg.GetNamespaceOfPrefix(prefix);
				bool add = true;
				if (nm.NamespaceName.StartsWith("http"))
				{
					argType = "System." + type;
				}
				else
				{
					tmpStr = nm.NamespaceName;
					index = tmpStr.IndexOf(':');
					index2 = tmpStr.IndexOf(';');
					string typeNamespace = tmpStr.Substring(index + 1, index2 - index - 1);
					argType = typeNamespace + "." + type;


					index = tmpStr.IndexOf('=');
					string typeAssembly = tmpStr.Substring(index + 1, tmpStr.Length - index - 1);

					System.Reflection.Assembly ass = FWBS.OMS.Session.CurrentSession.AssemblyManager.TryLoadFrom(typeAssembly);
					if (ass == null)
					{
						ass = FWBS.OMS.Session.CurrentSession.AssemblyManager.TryLoad(typeAssembly);
					}
					if (ass == null)
					{
						// TODO: Error, cannot load the assembly containing the type
						add = false;
					}
				}

				if (add)
				{
					ArgumentDirection argDir = argWFType.StartsWith("Out") ? ArgumentDirection.Out : (argWFType.StartsWith("InOut") ? ArgumentDirection.InOut : ArgumentDirection.In);
					Argument arg = Argument.Create(Type.GetType(argType), argDir);
					Arguments.Add(argName, arg);
				}
			}

			return Arguments;
		}
		#endregion

		#region buttonArguments_Click
		private void buttonArguments_Click(object sender, RoutedEventArgs e)
		{
			DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
			{
				Title = "Enter Argument Expressions"
			};

			ModelItem modelItem = this.ModelItem.Properties[HOST_PROPERTY_NAME_ARGUMENTS].Dictionary;
			using (ModelEditingScope change = modelItem.BeginEdit("ChildArgumentEditing"))
			{
				if (DynamicArgumentDialog.ShowDialog(this.ModelItem, modelItem, Context, this.ModelItem.View, options))
				{
					change.Complete();
				}
				else
				{
					change.Revert();
				}
			}
		}
		#endregion

		#region buttonRefresh_Click
		private void buttonRefresh_Click(object sender, RoutedEventArgs e)
		{
			SplashWindow wnd = new SplashWindow("Refreshing...");
			wnd.Show();
			System.Threading.Thread.Sleep(500);

			try
			{
				string code = this.ModelItem.Properties[HOST_PROPERTY_NAME_CODE].ComputedValue as string;
				if ((code != null) && !string.IsNullOrWhiteSpace(code))
				{
					FWBS.WF.Packaging.WorkflowXaml wf = new WF.Packaging.WorkflowXaml();
					if (wf.Exists(code))
					{
						wf.Fetch(code);
						Dictionary<string, Argument> arguments = this.SetupArguments(wf.Xaml);
						this.ModelItem.Properties[HOST_PROPERTY_NAME_ARGUMENTS].SetValue(arguments);
					}
					else
					{
						// TODO: Code lookup for message
						FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("Workflow {0} does not exist!", code));
					}
				}
			}
			finally
			{
				wnd.Close();
			}
		}
		#endregion

		#region buttonEditWorkflow_Click
		private void buttonEditWorkflow_Click(object sender, RoutedEventArgs e)
		{
			string code = this.ModelItem.Properties[HOST_PROPERTY_NAME_CODE].ComputedValue as string;
			if ((code != null) && !string.IsNullOrWhiteSpace(code))
			{
				FWBS.WF.Packaging.WorkflowXaml wf = new WF.Packaging.WorkflowXaml();
				if (wf.Exists(code))
				{
					Admin.WorkflowForm form = Admin.WorkflowForm.CreateForm(code);
					form.Show();
					form.Focus();
				}
				else
				{
					// TODO: Code lookup for message
					FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("Workflow {0} does not exist!", code));
				}
			}
		}
		#endregion
	}
}
