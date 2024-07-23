#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowContact.ShowContactToolboxIcon.bmp")]
	[Description("Shows a Contact")]
	[Designer(typeof(ShowContactDesigner))]
	public sealed class ShowContact : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.Contact> Contact { get; set; }
		#endregion

		#region Constructor
		public ShowContact()
		{
			this.DisplayName = "Show Contact";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Contact", typeof(FWBS.OMS.Contact), ArgumentDirection.In, true);
			metadata.Bind(this.Contact, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.Contact contact = Contact.Get(context);

			if (contact == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Contact' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.UI.Windows.Services.ShowContact(contact);
			}
		}
		#endregion
	}
}
