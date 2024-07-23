using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fwbs.Documents.Preview.Msg
{
    using Aspose.Email;
    using Handlers;
    using Microsoft.Win32;

    #region WebBrowserExtensions
    public class WebBrowserExtensions
	{
		public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
	"Html",
	typeof(string),
	typeof(WebBrowserExtensions),
	new FrameworkPropertyMetadata(OnHtmlChanged));

		[AttachedPropertyBrowsableForType(typeof(WebBrowser))]
		public static string GetHtml(WebBrowser d)
		{
			return (string)d.GetValue(HtmlProperty);
		}

		public static void SetHtml(WebBrowser d, string value)
		{
			d.SetValue(HtmlProperty, value);
		}

		static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WebBrowser wb = d as WebBrowser;
			string html = (string)e.NewValue;

			if (html != null)
			{
				byte[] htmlBytes = GetEncodedBytes(html);

				if (wb != null)
					wb.NavigateToString(Encoding.UTF8.GetString(htmlBytes));
			}
		}

		/// <summary>
		/// Encode the html string in UTF8 and add the UTF8 Preamble (Byte Order Marker BOM) unless disabled in registry
		/// Without doing this the WebBrowser.NavigateToString methods fails to recognise some of the extended characters 
		/// that Outlook add for quotes etc.
		/// </summary>
		private static byte[] GetEncodedBytes(string html)
		{
			byte[] preamble = GetPreamble();
			byte[] htmlBytes = Encoding.UTF8.GetBytes(html);

			htmlBytes = preamble.Concat(htmlBytes).ToArray();
			return htmlBytes;
		}

		private static byte[] GetPreamble()
		{
			if (includePreamble())
				return Encoding.UTF8.GetPreamble();

			return new byte[0];
		}

		private static bool includePreamble()
		{
			const string policiesKey = @"SOFTWARE\POLICIES\FWBS\OMS\2.0\DocumentPreviewer";
			const string softwareKey = @"SOFTWARE\FWBS\OMS\2.0\DocumentPreviewer";
			const string keyValue = @"UseUTF8Preamble";
			// the values in the arrays define precedence and must match
			const int MAX_KEYS = 4;
			RegistryHive[] hives = new RegistryHive[MAX_KEYS] { RegistryHive.LocalMachine, RegistryHive.CurrentUser, RegistryHive.LocalMachine, RegistryHive.CurrentUser };
			string[] subKeyNames = new string[MAX_KEYS] { policiesKey, policiesKey, softwareKey, softwareKey };

			bool retValue = true; 

			RegistryKey key = null;
			RegistryKey subKey = null;

			try
			{
				for (int i = 0; i < MAX_KEYS; i++)
				{
					key = RegistryKey.OpenBaseKey(hives[i], RegistryView.Default);
					subKey = key.OpenSubKey(subKeyNames[i]);
					if (subKey != null)
					{
						// found it
						retValue = Convert.ToBoolean(subKey.GetValue(keyValue));
						break;
					}
					key.Close();
					key = null;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("MsgPreviewer error accessing registry to get UseUTF8Preamble setting. Default to True. Exception: {0}.", ex.Message), "MsgPreviewer");
			}
			finally
			{
				// clean up any resource
				if (subKey != null)
				{
					subKey.Close();
					subKey = null;
				}
				if (key != null)
				{
					key.Close();
					key = null;
				}
			}
			return retValue;
		}

	}
	#endregion

	#region RecipientWrapper
	public class RecipientWrapper
	{

		public string Name { get; set; }
		public string Address { get; set; }

	}
	#endregion

	#region AttachmentWrapper
	public class AttachmentWrapper
	{
		public string Name { get; set; }
		public string Size { get; set; }
		public string Extension { get; set; }
		public bool IsMessage { get; set; }
		public string Location { get; set; }
		public string Id { get; set; }
	}
	#endregion
	
	#region MessageWrapper
	public class MessageWrapper : INotifyPropertyChanged ,IDisposable
	{
		public MessageWrapper()
		{
			_to = new ObservableCollection<RecipientWrapper>();
			_cc = new ObservableCollection<RecipientWrapper>();
			_attachments = new ObservableCollection<AttachmentWrapper>();
		}

		private string msgFailMessage = "Unable to preview - Email has been secured. Please Open In Outlook";
		private Aspose.Email.MailMessage msg = null;
		private string file;
		internal void Initialise(string file)
		{
			this.file = file;
			// get rid of previous one properly
			if (this.msg != null)
			{
				this.msg.Dispose();
			}
            this.msg = Aspose.Email.MailMessage.Load(file, new Aspose.Email.MsgLoadOptions());
			SetupProperties(this.msg);
		}

		private Dictionary<string, string> additionalProperties;
		internal void SetAdditionalProperties(Dictionary<string, string> additionalProperties)
		{
			this.additionalProperties = additionalProperties;
		}

		public void Dispose()
		{
			if (this.msg != null)
			{
				this.msg.Dispose();
				this.msg = null;
			}
		}

		private string _subject;
		public string Subject
		{
			get { return _subject; }
			set
			{
				_subject = value;
				RaisePropertyChanged("Subject");
			}
		}
		private string _from;
		public string From
		{
			get { return _from; }
			set { _from = value;
			RaisePropertyChanged("From");
			}
		}

		private string _sentDate;
		public string SentDate
		{
			get { return _sentDate; }
			set
			{
				_sentDate = value;
				RaisePropertyChanged("SentDate");
			}
		}

		private ObservableCollection<RecipientWrapper> _to;
		public ObservableCollection<RecipientWrapper> To
		{
			get { return _to; }
		}

		private ObservableCollection<RecipientWrapper> _cc;
		public ObservableCollection<RecipientWrapper> CC
		{
			get { return _cc; }
		}

		private ObservableCollection<AttachmentWrapper> _attachments;
		public ObservableCollection<AttachmentWrapper> Attachments
		{
			get { return _attachments; }
		}


		private string _body;
		public string Body
		{
			get { return _body; }
			set
			{
				_body = value;
				RaisePropertyChanged("Body");
			}
		}

		internal string GetAttachmentLocation(AttachmentWrapper attachment)
		{
			if (!string.IsNullOrWhiteSpace(attachment.Location))
			{
				if (System.IO.File.Exists(attachment.Location))
					return attachment.Location;
			}

			var attach = msg.Attachments.FirstOrDefault(a => a.ContentId == attachment.Id);
			if (attach == null)
				return null;

			var dir = CreateTempDirectory(file);

			string location = GetLocalAttatchmentFile(dir, attach, attachment.Extension);
		  
			attachment.Location = location;
			return attachment.Location;
		}

		internal void SetupProperties(MailMessage msgLocal)
		{         
			if (msgLocal.IsSigned)
			{
				throw new NotSupportedException(msgFailMessage);
			}

			Subject = msgLocal.Subject;
			string body = msgLocal.IsBodyHtml ? msgLocal.HtmlBody
                            : string.Format("<html><head><meta charset=\"utf-16\"/></head><body><pre>{0}</pre></body></html>", System.Net.WebUtility.HtmlEncode(msgLocal.Body));

			string sendersAddress = null;
            if (msgLocal.From != null)
            {
                sendersAddress = msgLocal.From.Address;

                if (string.IsNullOrEmpty(sendersAddress) || sendersAddress.StartsWith("/o"))
                    From = msgLocal.From.DisplayName;
                else
                    From = string.Format("{0} [{1}]", msgLocal.From.DisplayName, sendersAddress);
            }
			SentDate = null;
            if (msgLocal.Date != DateTime.MinValue)
            {
                if (msgLocal.Date == DateTime.FromOADate(0))
                {
                    if (additionalProperties != null && additionalProperties.ContainsKey("SentDate"))
                    {
                        SentDate = additionalProperties["SentDate"];
                    }
                }
                else
                {
                    if (msgLocal.Date.Kind == DateTimeKind.Utc)
                        this.SentDate = msgLocal.Date.ToLocalTime().ToString();
                    else
                        SentDate = msgLocal.Date.ToString();
                }
            }

			To.Clear();
			CC.Clear();

			foreach (var recip in msgLocal.To)
			{
				var w = new RecipientWrapper();
				w.Name = recip.DisplayName;
				w.Address = recip.Address;
				To.Add(w);
			}

			foreach (var recip in msgLocal.CC)
			{
				var w = new RecipientWrapper();
				w.Name = recip.DisplayName;
				w.Address = recip.Address;
				CC.Add(w);
			}

			System.IO.DirectoryInfo dir = CreateTempDirectory(file);
			foreach (var linkedDoc in msgLocal.LinkedResources)
			{
				string searchvalue = string.Format("cid:{0}", linkedDoc.ContentId);
				string attfile = GetLocalAttatchmentFile(dir, linkedDoc);
				body = body.Replace(searchvalue, new Uri(attfile).AbsoluteUri);
			}

			Body = body;

			this.Attachments.Clear();
			AddMsgAttachment();

			foreach (var attatchment in msgLocal.Attachments)
			{
				AttachmentWrapper w = new AttachmentWrapper();

				if (attatchment.Name.Contains('.'))
				{
					var parts = attatchment.Name.Split('.');
					w.Extension = "."+parts.Last();
				}
				else
					w.Extension = attatchment.Name;

				w.Name = attatchment.Name;
				w.Size = Utils.FileSize(attatchment.ContentStream.Length);
				w.Id = attatchment.ContentId;
				Attachments.Add(w);
		   }

			RaisePropertyChanged("Attachments");
		 }

		public void AddMsgAttachment()
		{
			AttachmentWrapper w = new AttachmentWrapper();
			w.Extension = ".msg";
			w.Name = "Message";
			w.IsMessage = true;
			Attachments.Add(w);
		 }

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged(string property)
		{
			var ev = PropertyChanged;
			if (ev != null)
				ev(this, new PropertyChangedEventArgs(property));
		}
	   
		private static string GetLocalAttatchmentFile(System.IO.DirectoryInfo dir, Aspose.Email.Attachment att, string extension)
		{
			string attfile = System.IO.Path.Combine(dir.FullName, att.ContentId);
			attfile = System.IO.Path.ChangeExtension(attfile, extension);
			if (!System.IO.File.Exists(attfile))
				att.Save(attfile);
			return attfile;
		}

		private static string GetLocalAttatchmentFile(System.IO.DirectoryInfo dir, Aspose.Email.LinkedResource att)
		{
			string attfile = System.IO.Path.Combine(dir.FullName, att.ContentId);
			if (!System.IO.File.Exists(attfile))
				att.Save(attfile);
			return attfile;
		}

		private static System.IO.DirectoryInfo CreateTempDirectory(string file)
		{
			string checksum = GenerateChecksum(file);
			string temp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "FWBS", "OMS", checksum);
			if (!System.IO.Directory.Exists(temp))
				System.IO.Directory.CreateDirectory(temp);

			return new System.IO.DirectoryInfo(temp);
		}

		private static string GenerateChecksum(string text)
		{
			System.Text.UnicodeEncoding enc = new System.Text.UnicodeEncoding();
			byte[] input = enc.GetBytes(text);

			using (System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
			{
				byte[] hash = sha1.ComputeHash(input);
				System.Text.StringBuilder buff = new System.Text.StringBuilder();
				foreach (byte hashByte in hash)
				{
					buff.AppendFormat("{0:X1}", hashByte);
				}

				return buff.ToString();
			}
		}
	}
	#endregion

	#region MessagePreviewerVM
	public class MessagePreviewerVM : IPreviewHandlerInfo , IDisposable, INotifyPropertyChanged
	{
		private AttachmentWrapper _selectedAttachment;

		public AttachmentWrapper SelectedAttachment
		{
			get { return _selectedAttachment; }
			set
			{
				_selectedAttachment = value;
				RaisePropertyChanged("SelectedAttachment");
				OnSelectedAttachmentChanged();
			}
		}

		private string _attachmentLocation;
		public string AttachmentLocation
		{
			get { return _attachmentLocation; }
			set
			{
				_attachmentLocation = value;
				RaisePropertyChanged("AttachmentLocation");
			}
		}

		private void OnSelectedAttachmentChanged()
		{
			if (SelectedAttachment == null || SelectedAttachment.IsMessage)
			{
				AttachmentLocation = null;
				return;
			}

			AttachmentLocation = Wrapper.GetAttachmentLocation(SelectedAttachment);
		}

		private MessageWrapper _wrapper;
		public MessageWrapper Wrapper
		{
			get { return _wrapper; }
			set 
			{
				// get rid of previous one properly
				if (this._wrapper != null)
				{
					this._wrapper.Dispose();
				}
				this._wrapper = value;
				RaisePropertyChanged("Wrapper");
			}
		}

		public MessagePreviewerVM()
		{
			Wrapper = new MessageWrapper();
			Sent = "Sent:";
			To = "To:";
			Cc = "CC:";
			Attachments = "Attachments:";
		}

		private string _sent;
		public string Sent
		{
			get { return _sent; }
			set { _sent = value;
			RaisePropertyChanged("Sent");
			}
		}

		private string _to;
		public string To
		{
			get { return _to; }
			set { _to = value;
			RaisePropertyChanged("To");
			}
		}

		private string _cc;
		public string Cc
		{
			get { return _cc; }
			set
			{
				_cc = value;
				RaisePropertyChanged("Cc");
			}
		}

		private string _attachments;
		public string Attachments
		{
			get { return _attachments; }
			set { _attachments = value;
			RaisePropertyChanged("Attachments");
			}
		}

		public void DoPreview(string file)
		{
			Wrapper.Initialise(file);
			SelectedAttachment = Wrapper.Attachments[0];//Always atleast the message
		}

		#region IPreviewHandlerInfo
	   
		

		public void SetCultureData(Dictionary<string, string> cultureData)
		{
			if (cultureData.ContainsKey("Sent:"))
				Sent = cultureData["Sent:"];

			if (cultureData.ContainsKey("To:"))
				To = cultureData["To:"];

			if (cultureData.ContainsKey("Cc:"))
				Cc = cultureData["Cc:"];

			if (cultureData.ContainsKey("Attachments:"))
				Attachments = cultureData["Attachments:"];
		}

		private bool supportsFullPreview = true;
		public void SetPreviewSupport(bool fullPreviewSupport)
		{
			this.supportsFullPreview = fullPreviewSupport;
		}

		public void SetAdditionalProperties(Dictionary<string, string> additionalProperties)
		{
			Wrapper.SetAdditionalProperties(additionalProperties);
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged(string property)
		{
			var ev = PropertyChanged;
			if (ev != null)
				ev(this, new PropertyChangedEventArgs(property));
		}

		public void Dispose()
		{
			if (this.Wrapper != null)
			{
				// setting the property will trigger Dispose()
				this.Wrapper = null;
			}
		}
	}
	#endregion
	
	#region IsVisibleConverter
	public class IsVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return Visibility.Hidden;
			var att = value as AttachmentWrapper;
			if (att != null)
			{
				bool isMsg = false;
				bool.TryParse(parameter.ToString(), out isMsg);

				if (isMsg)
				{
					if (!att.IsMessage)
						return Visibility.Hidden;
					else
						return Visibility.Visible;
					
				}

				if (att.IsMessage)
					return Visibility.Hidden;
			}
			else
			{
				var val = value as System.Collections.IEnumerable;
				if (val != null)
				{
					long min = 0;
					if (parameter != null)
					{
						long param = 0;
						if (long.TryParse(parameter.ToString(), out param))
							min = param;
					}

					if (val.Count() <= min)
						return Visibility.Collapsed;
				}
			}

			
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	#endregion

	#region ExtensionToImageConverter
	public class ExtensionToImageConverter : IValueConverter
	{
        // cache the icons? These will NOT be released until the end of the app, however they will always be used...
        private static Dictionary<string, System.Windows.Media.ImageSource> icons = new Dictionary<string, System.Windows.Media.ImageSource>();

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var fileExtension = value as string;

            if (icons.ContainsKey(fileExtension))
            {
                return icons[fileExtension];
            }

			var icon = IconReader.GetFileIcon(fileExtension, IconReader.IconSize.Small, false);
			System.Windows.Media.ImageSource img = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
							icon.Handle,					// native icon handle
							new Int32Rect(0, 0, 16, 16),	// small size
							System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
			// get rid of icon object
			icon.Dispose();

            // freeze it since it will not change
            if (img.CanFreeze)
            {
                img.Freeze();
            }

            icons.Add(fileExtension, img);
			return img;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}   
	#endregion
}
