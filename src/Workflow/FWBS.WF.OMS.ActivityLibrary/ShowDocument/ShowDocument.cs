#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowDocument.ShowDocumentToolboxIcon.bmp")]
	[Description("Shows a Document")]
	[Designer(typeof(ShowDocumentDesigner))]
	public sealed class ShowDocument : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.OMSDocument> Document { get; set; }
		#endregion

		#region Constructor
		public ShowDocument()
		{
			this.DisplayName = "Show Document";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Document", typeof(FWBS.OMS.OMSDocument), ArgumentDirection.In, true);
			metadata.Bind(this.Document, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.OMSDocument document = Document.Get(context);

			if (document == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Document' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.UI.Windows.Services.ShowDocument(document);
			}
		}
		#endregion
	}
}
