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
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.GetWizard.GetWizardToolboxIcon.png")]
	[Description("Get Wizard")]
	[Designer(typeof(GetWizardDesigner))]
	public sealed class GetWizard : CodeActivity<object>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> WizardCode { get; set; }

		public InArgument<object> Parent { get; set; }

		public FWBS.OMS.EnquiryEngine.EnquiryMode Mode { get; set; }

		[Browsable(true)]
		[Editor(typeof(ParametersPropertyValueEditor), typeof(DialogPropertyValueEditor))]
		public ObservableCollection<KeyParameters> Parameters { get; set; }
		#endregion

		#region Constructor
		public GetWizard()
		{
			this.DisplayName = "Get Wizard";

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

			argument = new RuntimeArgument("Parent", typeof(object), ArgumentDirection.In);
			metadata.Bind(this.Parent, argument);
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
			var wizardCode = WizardCode.Get(context);
			var parent = Parent.Get(context);
			ObservableCollection<KeyParameters> parameters = this.Parameters;

			if (wizardCode == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.WizardCode' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				if (parameters == null)
				{
					return FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(wizardCode, parent, this.Mode, new FWBS.Common.KeyValueCollection());
				}
				else
				{
					FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection();

					foreach (var item in parameters)
					{
						object obj = context.GetValue(item.Value);
						kvc.Add(item.Key, obj);
					}

					return FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(wizardCode, parent, this.Mode, kvc);
				}
			}
		}
		#endregion
	}
}
