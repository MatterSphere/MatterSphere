#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    //
    // THE ICON IN THE TOOLBOX IS DETERMINED BY ToolboxBitmap ATTRIBUTE
    // THE ICON ON THE DESIGNER IS DETERMINED BY Designer ATTRIBUTE
    //
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SendEMail.SendEMailToolboxIcon.bmp")]
	[Description("Sends an Email")]
	[Designer(typeof(SendEMailDesigner))]
	public sealed class SendEMail : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> From { get; set; }

		[RequiredArgument]
		public InArgument<string> To { get; set; }

		[DefaultValue("")]
		public InArgument<string> Subject { get; set; }

		[DefaultValue("")]
		public InArgument<string> Body { get; set; }
		#endregion

		#region Constructor
		public SendEMail()
		{
			this.DisplayName = "Send Email";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("From", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.From, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("To", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.To, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Subject", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Subject, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Body", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Body, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			// Validate arguments
			string from = this.From.Get(context);
			string to = this.To.Get(context);

			if (!IsValidEmailAddress(to) || !IsValidEmailAddress(from))
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.From/To' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentException(errMsg);
			}
			else
			{
				string subject = this.Subject.Get(context);
				string body = this.Body.Get(context);

				if (subject == null)
				{
					subject = string.Empty;
				}
				if (body == null)
				{
					body = string.Empty;
				}
				FWBS.OMS.Session.CurrentSession.SendMail(from, to, subject, body);
			}
		}

		#endregion

		#region IsValidEmailAddress
        /// <summary>
        /// Is Valid Email Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
		private bool IsValidEmailAddress(string address)
		{
			// must only proceed with validation if we have data to validate
			if (string.IsNullOrEmpty(address))
			{
				return false;
			}

			Regex rx = new Regex(@"[^A-Za-z0-9@\-_.]", RegexOptions.Compiled);
			MatchCollection matches = rx.Matches(address);

			if (matches.Count > 0)
			{
				return false;
			}

			// Must have an '@' character
			int i = address.IndexOf('@');

			// Must be at least three chars after the @
			if (i <= 0 || i >= address.Length - 3)
			{
				return false;
			}

			// Must only be one '@' character
			if (address.IndexOf('@', i + 1) >= 0)
			{
				return false;
			}

			// Find the last . in the address
			int j = address.LastIndexOf('.');

			// The dot can't be before or immediately after the @ char
			if (j >= 0 && j <= i + 1)
			{
				return false;
			}

			return true;
		}
		#endregion
	}
}
