using System;
using System.Diagnostics;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    class ucDocumentPreviewer : Fwbs.Documents.Preview.PreviewerControl
	{
		public ucDocumentPreviewer() : base()
		{
			if (!Global.IsInDesignMode())
			{
				//Only change the value if the default value is the same.  Could have been changed on the control itself.
				if (UnloadWaitFor == 2000)
				{
					Common.ApplicationSetting timeout = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "WaitForPreviewUnloadTimeout", 2000);
					UnloadWaitFor = Common.ConvertDef.ToInt32(timeout.GetSetting(), 2000);
				}
				
				this.Container = Session.CurrentSession.Container;
			}

			Common.ApplicationSetting ext = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "ForceUnloadPreviewExt", "");
			string unloadExtensions = Convert.ToString(ext.GetSetting(), System.Globalization.CultureInfo.InvariantCulture);
			if (!string.IsNullOrEmpty(unloadExtensions))
				ForceUnloadExtensions = unloadExtensions.ToLower();
		}

		public void PreviewFile(IStorageItem item,bool SpecificVersion=false)
		{
			try
			{

				AdditionalProperties.Clear();

				if (item == null)
				{
					return;
				}
				SetupCodeLookups();

				OMSDocument doc = item as OMSDocument;
				if (doc == null)
				{
					FWBS.OMS.DocumentManagement.DocumentVersion vers = item as FWBS.OMS.DocumentManagement.DocumentVersion;
					if (vers != null)
						doc = vers.ParentDocument;
				}


				if (doc != null && doc.DocumentType == "EMAIL")
				{
					FWBS.OMS.DocumentManagement.EmailDocument email = null;
					try
					{
						email = new FWBS.OMS.DocumentManagement.EmailDocument(doc);
						if (email != null && email.Sent.HasValue)
							this.AdditionalProperties.Add("SentDate", email.Sent.Value.ToLocalTime().ToString());
					}
					catch (MissingCommonObjectException)
					{
					}
					finally
					{
						if (email != null)
						{
							email.Dispose();
						}
					}
				}

				StorageSettingsCollection settings = item.GetSettings();
				if (settings == null || settings.Count == 0)
					settings = item.GetStorageProvider().GetDefaultSettings(item, SettingsType.Fetch);

				LockableFetchSettings fetch = settings.GetSettings<LockableFetchSettings>();
				if (fetch != null)
					fetch.CheckOut = false;

				item.ApplySettings(settings);

				ValidateExtension(item.Extension);

				FetchResults results = item.GetStorageProvider().Fetch(item, false, settings, Common.TriState.Null, SpecificVersion);

				if (results != null)
				{
					PreviewFile(results.LocalFile);
				}
				else
				{
					// since this file does not exist we will get appropriate message displayed in exception catcher
					PreviewFile(new System.IO.FileInfo("qwerty"));
				}
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (!string.IsNullOrWhiteSpace(ex.Message))
				{
                    ShowMessage(Session.CurrentSession.Resources.GetResource("DP_NOPREVIEW", "No Preview Available", "").Text);// (ex.Message);
				}
				else
				{
					if (CultureProperties.ContainsKey("NoPreview") && !string.IsNullOrEmpty(CultureProperties["NoPreview"]))
					{
						ShowMessage(CultureProperties["NoPreview"]);
					}
				} 
				Trace.TraceError(ex.Message);
			}
		}


		private void SetupCodeLookups()
		{
			if (this.CultureProperties.Count > 0)
				return;

			this.CultureProperties.Add("To:", Session.CurrentSession.Resources.GetResource("TO:", "To:", "").Text);
			this.CultureProperties.Add("Cc:", Session.CurrentSession.Resources.GetResource("CC:", "Cc:", "").Text);
			this.CultureProperties.Add("Sent:", Session.CurrentSession.Resources.GetResource("Sent:", "Sent:", "").Text);
			this.CultureProperties.Add("Attachments:", Session.CurrentSession.Resources.GetResource("Attachments:", "Attachments:", "").Text);
			this.CultureProperties.Add("Column:", Session.CurrentSession.Resources.GetResource("Column:", "Column:", "").Text);
			this.CultureProperties.Add("NoPreview", Session.CurrentSession.Resources.GetResource("NoPreviewAvail", "Unable To Preview The Document", "").Text);
			this.CultureProperties.Add("ProcPlzWait", Session.CurrentSession.Resources.GetResource("ProcPlzWait", "Processing Please Wait", "").Text);
			this.CultureProperties.Add("SecureEmail", Session.CurrentSession.Resources.GetResource("PRVWSECEMAIL", "Unable to preview - Email has been secured. Please Open In Outlook", "").Text);

		}

	}
}
