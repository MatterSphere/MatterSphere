#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ProcessJob.ProcessJobToolboxIcon.png")]
	[Description("Process a Job")]
	[Designer(typeof(ProcessJobDesigner))]
	public sealed class ProcessJob : CodeActivity<FWBS.OMS.ProcessJobStatus>
	{
		#region Constructor
		public ProcessJob()
		{
			this.DisplayName = "Process Job";
		}
		#endregion

		#region Arguments
        [RequiredArgument]
        public InArgument<FWBS.OMS.PrecedentJob> PrecedentJob { get; set; }
        
		public InArgument<FWBS.OMS.Interfaces.IOMSApp> IOMSApp { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
            RuntimeArgument argument = new RuntimeArgument("PrecedentJob", typeof(FWBS.OMS.PrecedentJob), ArgumentDirection.In, true);
            metadata.Bind(this.PrecedentJob, argument);
            metadata.AddArgument(argument);

            argument = new RuntimeArgument("IOMSApp", typeof(FWBS.OMS.Interfaces.IOMSApp), ArgumentDirection.In);
			metadata.Bind(this.IOMSApp, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.ProcessJobStatus Execute(CodeActivityContext context)
		{
			FWBS.OMS.Interfaces.IOMSApp controlApp = this.IOMSApp.Get(context);
			FWBS.OMS.PrecedentJob precedentJob = this.PrecedentJob.Get(context);

			if (precedentJob == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.PrecedentJob' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
                if (controlApp == null)
                {
                    return FWBS.OMS.UI.Windows.Services.ProcessJob(null, precedentJob);
                }
                else
                {
                    return FWBS.OMS.UI.Windows.Services.ProcessJob(controlApp, precedentJob);
                }
			}
		}
		#endregion
	}
}
