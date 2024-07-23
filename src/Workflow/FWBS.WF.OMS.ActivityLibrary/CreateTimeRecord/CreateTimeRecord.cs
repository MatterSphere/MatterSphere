#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateTimeRecord.CreateTimeRecordToolboxIcon.png")]
	[Description("Creates a new Time Record")]
	[Designer(typeof(CreateTimeRecordDesigner))]
	public sealed class CreateTimeRecord : CodeActivity<bool>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }
		
		#endregion

		#region Constructor
		public CreateTimeRecord()
		{
			this.DisplayName = "Create Time Record";
		}
		#endregion

		#region Overrides
		/// <summary>
		/// CacheMetadata
		/// </summary>
		/// <param name="metadata"></param>
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			System.Collections.ObjectModel.Collection<RuntimeArgument> arguments = new System.Collections.ObjectModel.Collection<RuntimeArgument>();

            RuntimeArgument argument = new RuntimeArgument("File", typeof(FWBS.OMS.OMSFile), ArgumentDirection.In, true);
			metadata.Bind(this.File, argument);
			arguments.Add(argument);			

			metadata.SetArgumentsCollection(arguments);
		}

		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
        protected override bool Execute(CodeActivityContext context)
        {
            var omsfile = File.Get(context);

            if (omsfile == null)
            {
                string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Object' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
                throw new ArgumentNullException(errMsg);
            }

            object obj = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(FWBS.OMS.Session.CurrentSession.DefaultSystemForm(FWBS.OMS.SystemForms.ManualTimeWizard), omsfile, null);
            return false;
        }
		#endregion
	}
}
