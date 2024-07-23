#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateAssociate.CreateAssociateToolboxIcon.bmp")]
	[Description("Creates a new Associate")]
	[Designer(typeof(CreateAssociateDesigner))]
	public sealed class CreateAssociate : CodeActivity<FWBS.OMS.Associate>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }

        public InArgument<FWBS.OMS.Contact> Contact { get; set; }
		#endregion

		#region Constructor
		public CreateAssociate()
		{
			this.DisplayName = "Create Associate";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("File", typeof(FWBS.OMS.OMSFile), ArgumentDirection.In, true);
			metadata.Bind(this.File, argument);
			metadata.AddArgument(argument);

            argument = new RuntimeArgument("Contact", typeof(FWBS.OMS.Contact), ArgumentDirection.In, false);
            metadata.Bind(this.Contact, argument);
            metadata.AddArgument(argument);

		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Associate Execute(CodeActivityContext context)
		{
			FWBS.OMS.OMSFile file = this.File.Get(context);
            FWBS.OMS.Contact cont = this.Contact.Get(context);

			if (file == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.File' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
                return FWBS.OMS.UI.Windows.Services.Wizards.CreateAssociate(file,cont,false);
			}
		}
		#endregion
	}
}
