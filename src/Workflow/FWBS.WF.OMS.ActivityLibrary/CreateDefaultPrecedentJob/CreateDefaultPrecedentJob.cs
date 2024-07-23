using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using FWBS.OMS;

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateDefaultPrecedentJob.CreateDefaultPrecedentJobToolboxIcon.png")]
    [Description("Create Default Precedent Job")]
    [Designer(typeof(CreateDefaultPrecedentJobDesigner))]
    public sealed class CreateDefaultPrecedentJob : CodeActivity<PrecedentJob>
    {
        #region Constructor
        public CreateDefaultPrecedentJob()
		{
            this.DisplayName = "Create Default Precedent Job";
            AutoProcessJob = true;
		}
		#endregion
        
        #region Arguments       
        [RequiredArgument]
        public InArgument<string> Type {get;set;}

        [RequiredArgument]
        public InArgument<FWBS.OMS.Associate> Associate { get; set; }

        [RequiredArgument]
        public InArgument<bool> AutoProcessJob { get; set; }

        #endregion

        #region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            RuntimeArgument argument = new RuntimeArgument("Associate", typeof(FWBS.OMS.Associate), ArgumentDirection.In, true);
            metadata.Bind(this.Associate, argument);
            metadata.AddArgument(argument);

            argument = new RuntimeArgument("Type", typeof(string), ArgumentDirection.In, true);
            metadata.Bind(this.Type, argument);
            metadata.AddArgument(argument);

            argument = new RuntimeArgument("AutoProcessJob", typeof(bool), ArgumentDirection.In, true);
            metadata.Bind(this.AutoProcessJob, argument);
            metadata.AddArgument(argument);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        protected override PrecedentJob Execute(CodeActivityContext context)
        {
            FWBS.OMS.Associate associate = Associate.Get(context);
            string type = Type.Get(context);
            bool autoProcessJob = AutoProcessJob.Get(context);

            PrecedentJob job = null;

            Precedent precedent = Precedent.GetDefaultPrecedent(type, associate); 

            if (precedent == null)
            {
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Precedent' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
            }
            else
            {
                if (associate == null)
                {
                    string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Precedent' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
                    throw new ArgumentNullException(errMsg);
                }
                else
                {
                    job = new FWBS.OMS.PrecedentJob(precedent);
                    job.Associate = associate;
                    job.SaveMode = FWBS.OMS.PrecSaveMode.None;
                    job.PrintMode = FWBS.OMS.PrecPrintMode.None;                                     

                    if (autoProcessJob)
                    {
                        ProcessJobStatus status = FWBS.OMS.UI.Windows.Services.ProcessJob(null, job);

                        if (status == ProcessJobStatus.Error)
                        {
                        }
                    }
                }
            }

            return job;
        }
        #endregion       
    }
}
