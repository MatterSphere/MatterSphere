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
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.RunEnquiryForm.RunEnquiryCommandToolboxIcon.png")]
	[Description("Run an Enquiry Command")]
	[Designer(typeof(RunEnquiryCommandDesigner))]
	public sealed class RunEnquiryCommand : CodeActivity<object>
	{
		#region Arguments
		[RequiredArgument]
        public InArgument<string> EnquiryCommand { get; set; }

        public InArgument<string> Parent { get; set; }

		[Browsable(true)]
		[Editor(typeof(ParametersPropertyValueEditor), typeof(DialogPropertyValueEditor))]
		public ObservableCollection<KeyParameters> Parameters { get; set; }
		#endregion

		#region Constructor
		public RunEnquiryCommand()
		{
			this.DisplayName = "Run Enquiry Command";

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
			RuntimeArgument argument = new RuntimeArgument("EnquiryCommand", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.EnquiryCommand, argument);
			metadata.AddArgument(argument);

            argument = new RuntimeArgument("Parent", typeof(string), ArgumentDirection.In, false);
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
			string cmd = this.EnquiryCommand.Get(context);
			ObservableCollection<KeyParameters> parameters = this.Parameters;
            object parent = Parent.Get<object>(context);

			// Validate arguments
			if (string.IsNullOrEmpty(cmd))
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.EnquiryCommand' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}

            FWBS.Common.KeyValueCollection kvc = ConvertParameters(context, parameters);

            return FWBS.OMS.UI.Windows.Services.Run(cmd, parent, kvc);
		}

        private static FWBS.Common.KeyValueCollection ConvertParameters(CodeActivityContext context, ObservableCollection<KeyParameters> parameters)
        {
            // Map parameters
            FWBS.Common.KeyValueCollection kvc = new Common.KeyValueCollection();
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    object obj = context.GetValue(item.Value);
                    kvc.Add(item.Key, obj);
                }
            }
            return kvc;
        }
		#endregion
	}
}
