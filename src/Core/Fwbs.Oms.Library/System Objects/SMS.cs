using System;
using System.Data;

namespace FWBS.OMS
{
    /// <summary>
    /// A class that accepts a message and a mobile phone number so that it can be sent as a text message.
    /// </summary>
	[Obsolete("This class has been deprecated in V10.1")]
    public class SMS : SimpleDoc
	{
		#region Fields

		private bool _cancel = false;

		#endregion

		#region Constructors

		private SMS(){}

		[Obsolete("This method has been deprecated in V10.1")]
		public SMS (System.Xml.XmlDocument xml,System.IO.FileInfo file) : base (xml, file)
		{
		}

		[Obsolete("This method has been deprecated in V10.1")]
		public SMS(Associate associate) : base()
		{
			Session.CurrentSession.CheckLoggedIn();
			Associate = associate;
			Number = associate.DefaultMobile;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the associate to send the SMS to.
		/// </summary>
		[Obsolete("This property has been deprecated in V10.1")]
		public Associate Associate
		{
			get
			{
				long assocId = Convert.ToInt64(GetExtraInfo("_assoc", -1));
				if (assocId == -1)
					return null;
				else
					return Associate.GetAssociate(assocId);
			}
			set
			{
				if (value == null)
					SetExtraInfo("_assoc", -1);
				else
				{
					SetExtraInfo("_assoc", value.ID);
					Number = value.DefaultMobile;
				}

			}
		}

		/// <summary>
		/// Gets or Sets the Mobile number to use.
		/// </summary>
		[Obsolete("This property has been deprecated in V10.1")]
		public string Number
		{
			get
			{
				return Convert.ToString(GetExtraInfo("_number", ""));
			}
			set
			{
				SetExtraInfo("_number", value);
			}
		}

		///<summary>
		/// Gets or Sets whether the SMS will be sent or not.
		/// </summary>
		[Obsolete("This property has been deprecated in V10.1")]
		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}


		/// <summary>
		/// Gets a validation flag that determines that the SMS is ready to be sent.
		/// </summary>
		[Obsolete("This property has been deprecated in V10.1")]
		public bool IsValid
		{
			get
			{
				try
				{
					Validate();
					return true;
				}
				catch
				{
					return false;
				}
			}
		}


		#endregion

		#region Methods

		/// <summary>
		/// Gets a dataview of available mobile numbers of the the associate.
		/// </summary>
		/// <returns></returns>
		[Obsolete("This method has been deprecated in V10.1")]
		public DataView GetAvailableNumbers()
		{
			return Associate.GetMobileNumbers(); 
		}

		/// <summary>
		/// Raises an exception if the SMS does not pass validation.
		/// </summary>
		[Obsolete("This method has been deprecated in V10.1")]
		public void Validate()
		{
			Session.CurrentSession.ValidateLicensedFor("SMS");

			System.IO.DirectoryInfo dir = Session.CurrentSession.GetDirectory("SMS");
			if (dir.Exists == false)
				dir.Create();

			
			if (Associate == null || Number == "")
			{
				throw new SMSException(this, "SMSSEND", "Please ensure that a mobile number and associate are specified.");
			}
			else
			{
				if (Associate.Contact.HasNumber(Number, "MOBILE", "") == false)
					throw new SMSException(this, "ASSOCNOMOB", "The specified associate contact does not have the '%1%' as a mobile number.", null, false, Number);
			}
		}

		/// <summary>
		/// Parses the specified text with current Associate, Client, File information.
		/// </summary>
		[Obsolete("This method has been deprecated in V10.1")]
		public string Parse(string text)
		{
			FieldParser parser = new FieldParser(Associate);
			return parser.ParseString(text);
		}

		/// <summary>
		/// Parses the SMS message with current Associate, Client, File information.
		/// </summary>
		[Obsolete("This method has been deprecated in V10.1")]
		public void Parse()
		{
			Text = Parse(Text);
		}

		/// <summary>
		/// Sends the SMS message ready for the SMS server (POPBOT).
		/// </summary>
		[Obsolete("This method has been deprecated in V10.1")]
		public void Send()
		{
            if (Cancel == false && String.IsNullOrEmpty(base.Text))
                Cancel = true;
            
            if (_cancel == false)
			{
				Validate();
				CreateGMail();
			}
		}


		/// <summary>
		/// Backward compatible method fo an INI file format.
		/// </summary>
		private void CreateGMail()
		{
			//Add Body
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();

			string fullmessage = base.Text;
			
			long start = 0;
			long end = 0;
			System.IO.DirectoryInfo dir = Session.CurrentSession.GetDirectory("SMS");

            string _fileName;
            string _startName = Guid.NewGuid().ToString();
            int p = 1;
            while (fullmessage != "")
			{
                buffer = new System.Text.StringBuilder(); 
                string message;
				
				if (fullmessage.Length > SMS.MaxLength)
				{
					message = fullmessage.Substring(0, SMS.MaxLength);
					fullmessage = fullmessage.Substring(SMS.MaxLength); 
				}
				else
				{
					message = fullmessage;
					fullmessage = "";
				}

                _fileName = dir.FullName + System.IO.Path.DirectorySeparatorChar + _startName + p.ToString() + ".sms";

				buffer.Append("[HEADER]");
				buffer.Append(Environment.NewLine); 
				buffer.Append("To=SMS@fwbs.net");
				buffer.Append(Environment.NewLine);
				buffer.Append("From=");
				buffer.Append(Session.CurrentSession.CurrentUser.Initials);
				buffer.Append(Environment.NewLine);
				buffer.Append("CC=");
				buffer.Append(Environment.NewLine);
				buffer.Append("BCC=");
				buffer.Append(Environment.NewLine);
				buffer.Append("Subject=");
				buffer.Append(message);
				buffer.Append("/");
				buffer.Append(Number);
				buffer.Append(Environment.NewLine);
				buffer.Append("[STATUS]");
				buffer.Append(Environment.NewLine);
				buffer.Append("ReadyToSend=NO");
				buffer.Append(Environment.NewLine);
				buffer.Append("[ATTACHMENTS]");
				buffer.Append(Environment.NewLine);
				buffer.Append("Att0=|body.txt");
				buffer.Append(Environment.NewLine);
				buffer.Append("AttPos0=000000000000,000000000000");
				buffer.Append(Environment.NewLine);
				buffer.Append("Count=1");
				buffer.Append(Environment.NewLine);
				buffer.Append(Environment.NewLine);
			
				buffer.Append("[B-->]");
				start = buffer.Length;
				buffer.Append("<SMSSPEC>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<CUSTOMER>FWBS</CUSTOMER>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<MESSAGE>");
				buffer.Append(message);
				buffer.Append("</MESSAGE>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<RECIPIENT>");
				buffer.Append(Number);
				buffer.Append("</RECIPIENT>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<REFERENCE>%LOGID%</REFERENCE>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<DATESENT>");
				buffer.Append(DateTime.Today);
				buffer.Append("</DATESENT>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<TIMESENT>");
				buffer.Append(System.DateTime.Now.TimeOfDay);
				buffer.Append("</TIMESENT>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<SOURCE>");
				buffer.Append("OMS.NET");
				buffer.Append("</SOURCE>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<CONTACT>");
				buffer.Append(Session.CurrentSession.CurrentFeeEarner.Initials);
				buffer.Append("</CONTACT>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<CREATEDBY>");
				buffer.Append(Session.CurrentSession.CurrentUser.Initials);
				buffer.Append("</CREATEDBY>");
				buffer.Append(Environment.NewLine);
				buffer.Append("<MATTERID>");
				buffer.Append(this.Associate.OMSFileID);
				buffer.Append("</MATTERID>");
				buffer.Append(Environment.NewLine);
				buffer.Append("</SMSSPEC>");
				end = buffer.Length;

				//Write File
				System.IO.FileInfo file= new System.IO.FileInfo(_fileName);
				System.IO.TextWriter w = file.CreateText();
				w.Write(buffer);
				w.Close();

				FWBS.Common.INIFile.SetSetting(_fileName, "ATTACHMENTS", "AttPos0", Buff(start) + "," + Buff(end - start));
				FWBS.Common.INIFile.SetSetting(_fileName, "STATUS", "ReadyToSend", "OK");
                p++;
			}
		}

		private string Buff(long val)
		{
			string buff = "000000000000" + val.ToString();
			return buff.Substring(val.ToString().Length);
		}

		#endregion

		#region Static Methods

		[Obsolete("This property has been deprecated in V10.1")]
		public static int MaxLength
		{
			get
			{
				return 160;
			}
		}

		#endregion
	}
}
