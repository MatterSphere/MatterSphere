#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	//
	// THE ICON IN THE TOOLBOX IS DETERMINED BY ToolboxBitmap ATTRIBUTE
	// THE ICON ON THE DESIGNER IS DETERMINED BY Designer ATTRIBUTE
	//
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.MessageBox.MessageBoxToolboxIcon.bmp")]
	[Description("Display a Message Box")]
	[Designer(typeof(MessageBoxDesigner))]
	public sealed class MessageBox : CodeActivity<DialogResult>
	{
		#region Constructor
		public MessageBox()
		{
			this.DisplayName = "Message Box";
		}
		#endregion

		#region Arguments
		[RequiredArgument]
		public InArgument<string> Text { get; set; }

		public InArgument<string> Caption { get; set; }

		public MessageBoxButtons Buttons { get; set; }

		public ActivityMessageBoxIcon Icon { get; set; }

		public MessageBoxDefaultButton DefaultButton { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Text", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.Text, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Caption", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Caption, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override DialogResult Execute(CodeActivityContext context)
		{
			string text = this.Text.Get(context);
			string caption = this.Caption.Get(context);

			// Validate Arguments
			if (text == null)
			{
				text = string.Empty;
			}

			return FWBS.OMS.UI.Windows.MessageBox.Show(text, caption, this.Buttons, (MessageBoxIcon)this.Icon, this.DefaultButton);
		}
		#endregion
	}
}
