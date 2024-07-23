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
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.OpenReport.OpenReportToolboxIcon.png")]
	[Description("Open a Report")]
	[Designer(typeof(OpenReportDesigner))]
	public sealed class OpenReport : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> ReportCode { get; set; }

		public InArgument<object> Parent { get; set; }

		[Browsable(true)]
		[Editor(typeof(ParametersPropertyValueEditor), typeof(DialogPropertyValueEditor))]
		public ObservableCollection<KeyParameters> Parameters { get; set; }

		public InArgument<bool> RunNow { get; set; }
		#endregion

		#region Constructor
		public OpenReport()
		{
			this.DisplayName = "Open Report";

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
			RuntimeArgument argument = new RuntimeArgument("ReportCode", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.ReportCode, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Parent", typeof(object), ArgumentDirection.In);
			metadata.Bind(this.Parent, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("RunNow", typeof(bool), ArgumentDirection.In);
			metadata.Bind(this.RunNow, argument);
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
		protected override void Execute(CodeActivityContext context)
		{
			var reportCode = this.ReportCode.Get(context);
			var parent = this.Parent.Get(context);
			var runNow = this.RunNow.Get(context);
			ObservableCollection<KeyParameters> parameters = this.Parameters;

			if (reportCode == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.ReportCode' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}

            FWBS.Common.KeyValueCollection replacementParameters = ConvertParameters(context, parameters);							
            
            FWBS.OMS.UI.Windows.Services.Reports.OpenReport(reportCode, parent, replacementParameters, runNow);
		}

        private static FWBS.Common.KeyValueCollection ConvertParameters(CodeActivityContext context, ObservableCollection<KeyParameters> parameters)
        {
            FWBS.Common.KeyValueCollection replacementParameters = null;
            if (parameters != null)
            {
                // Map parameters    
                replacementParameters = new Common.KeyValueCollection();
                foreach (var item in parameters)
                {
                    object obj = context.GetValue(item.Value);
                    replacementParameters.Add(item.Key, obj);
                }
            }
            return replacementParameters;
        }
		#endregion
	}
}
