#region References
using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.RunWizard.RunWizardToolboxIcon.png")]
	[Description("Run a Wizard")]
	[DisplayName("Run Wizard")]
	[Designer(typeof(RunWizardDesigner))]
	public sealed class RunWizard : CodeActivity<object>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> WizardCode { get; set; }

		public InArgument<object> ParentObject { get; set; }

		[Browsable(true)]
		[Editor(typeof(ParametersPropertyValueEditor), typeof(DialogPropertyValueEditor))]
		public ObservableCollection<KeyParameters> Parameters { get; set; }
		#endregion

		#region Constructor
		public RunWizard()
		{
			this.DisplayName = "Run Wizard";

			if (Parameters == null)
			{
				Parameters = new System.Collections.ObjectModel.ObservableCollection<KeyParameters>();
			}
		}
		#endregion

		#region Overrides
		/// <summary>
		/// CacheMetadata
		/// </summary>
		/// <param name="metadata"></param>
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("WizardCode", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.WizardCode, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("ParentObject", typeof(object), ArgumentDirection.In);
			metadata.Bind(this.ParentObject, argument);
			metadata.AddArgument(argument);

			foreach (var item in this.Parameters)
			{
				RuntimeArgument runTimeArg = new RuntimeArgument(item.Key, item.Value.ArgumentType, item.Value.Direction, false);
				metadata.Bind(item.Value, runTimeArg);
				metadata.AddArgument(runTimeArg);
			}
		}

		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override object Execute(CodeActivityContext context)
		{
			string wizardCode = this.WizardCode.Get(context);
			object parent = this.ParentObject.Get(context);
			FWBS.OMS.EnquiryEngine.EnquiryMode mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Add;
			ObservableCollection<KeyParameters> parameters = this.Parameters;

			// Validate arguments
			if (string.IsNullOrEmpty(wizardCode))
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.WizardCode' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				// Map parameters
				FWBS.Common.KeyValueCollection replacementParameters = new Common.KeyValueCollection();
				if (Parameters != null)
				{
					foreach (var item in parameters)
					{
						object obj = context.GetValue(item.Value);
						replacementParameters.Add(item.Key, obj);
					}
				}
				
				//  Requires reference to System.Windows.Forms to be added
				return FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(wizardCode, parent, mode, replacementParameters);
			}
		}
		#endregion
	}
}
