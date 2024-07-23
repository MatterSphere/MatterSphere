using System;
using System.Collections.Generic;
using FWBS.OMS.FormsPortalService;

namespace FWBS.OMS
{
    public class FormDataEngine
    {
        private FormData formdata;
        private FWBS.OMS.Interfaces.IExtendedDataCompatible obj;
        private FWBS.OMS.Interfaces.IEnquiryCompatible ienqobj;
        private bool runSync = true;

        public FormDataEngine(FormData formData, FWBS.OMS.Interfaces.IExtendedDataCompatible obj, bool RunAsync)
        {
            formdata = formData;
            this.obj = obj;
            ienqobj = obj as FWBS.OMS.Interfaces.IEnquiryCompatible;
            runSync = RunAsync;
        }

        public event EventHandler<FormServiceEventArgs> FormServiceCalled;

  

        public FormServiceEventArgs RequestForm()
        {
            FWBS.OMS.FormsPortalService.FormsPortalService svc = FormDefinition.GetService(formdata.WebserviceUrl,formdata.WebserviceUser,formdata.WebservicePassword);
            svc.AddFormRequestCompleted += new AddFormRequestCompletedEventHandler(svc_AddFormRequestCompleted);
            OnlineFormRequest req = new OnlineFormRequest();
            req.Description = formdata.Description;
            req.DocumentType = DownloadType.XML;
            req.EmailAddress = formdata.Email;
            req.FormID = formdata.FormID;

            OnLineForm data = new OnLineForm();
            List<Field> fields = new List<Field>();
            foreach (KeyValuePair<string, object> val in formdata.Data)
            {
                if (String.IsNullOrEmpty(Convert.ToString(val.Value)) == false)
                {
                    Field f1 = new Field();
                    f1.Name = val.Key;
                    f1.Value = Convert.ToString(val.Value);
                    fields.Add(f1);
                }
            }
            data.Fields = fields.ToArray();
            req.FormData = data;
            req.OurReference = formdata.OurRef;
            req.TheirReference = formdata.YourRef;
            if (formdata.CampaignID != 0)
                req.CampaignID = formdata.CampaignID;

            if (runSync)
            {
                svc.AddFormRequestAsync(req);
                return null;
            }
            else
                return AddFormRequestCompleted(svc.AddFormRequest(req));
        }

        private FormServiceEventArgs AddFormRequestCompleted(ServiceResponse e)
        {
            if (e.ErrorCollection.Length > 0)
            {
                List<string> errs = new List<string>();
                foreach (ServiceError ee in e.ErrorCollection)
                    errs.Add(ee.Description);
                var evt = new FormServiceEventArgs(FormServiceType.Request, errs.ToArray());
                if (FormServiceCalled != null)
                    FormServiceCalled(this, evt);
                return evt;
            }
            else
            {
                formdata.FormGuid = (Guid?)new Guid(e.ResponseToken);
                formdata.Requested = DateTime.Now;
                formdata.Refreshed = DateTime.Now;
                formdata.Update();
                if (FormServiceCalled != null)
                    FormServiceCalled(this, FormServiceEventArgs.EmptyRequest);
                return FormServiceEventArgs.EmptyRequest;
            }
        }

        private void svc_AddFormRequestCompleted(object sender, AddFormRequestCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (FormServiceCalled != null)
                    FormServiceCalled(this, new FormServiceEventArgs(FormServiceType.Request, e.Error));
            }
            else
            {
                AddFormRequestCompleted(e.Result);
            }
        }

        public FormServiceEventArgs RefreshForm()
        {
            if (formdata.FormGuid == null)
                throw new ArgumentNullException("FormGuid on the FormData Object is required");

            FWBS.OMS.FormsPortalService.FormsPortalService svc = FormDefinition.GetService(formdata.WebserviceUrl, formdata.WebserviceUser, formdata.WebservicePassword);
            svc.GetFormRequestCompleted += new GetFormRequestCompletedEventHandler(svc_GetFormRequestCompleted);

            if (runSync == false)
                return GetFormRequestedCompleted(svc.GetFormRequest(Convert.ToString(formdata.FormGuid)));
            else
            {
                svc.GetFormRequestAsync(Convert.ToString(formdata.FormGuid));
                return null;
            }
        }

        private FormServiceEventArgs GetFormRequestedCompleted(OnlineFormResponse e)
        {
            if (e.ErrorCollection.Length > 0)
            {
                List<string> errs = new List<string>();
                foreach (ServiceError ee in e.ErrorCollection)
                    errs.Add(ee.Description);
                var evt = new FormServiceEventArgs(FormServiceType.Refresh, errs.ToArray());
                if (FormServiceCalled != null)
                    FormServiceCalled(this, evt);
                return evt;
            }
            else
            {
                OnLineForm frm = e.FormData;
                if (frm != null)
                {
                    Field[] fields = frm.Fields;
                    foreach (Field f in fields)
                    {
                        formdata.SetExtraInfo(f.Name, ConvertCR2CRLF(f.Value));
                    }
                }
                if (e.Completed != null)
                    formdata.Completed = e.Completed;
                formdata.Refreshed = DateTime.Now;
                formdata.Update();
                if (FormServiceCalled != null)
                    FormServiceCalled(this, FormServiceEventArgs.EmptyRefresh);
                return FormServiceEventArgs.EmptyRefresh;
            }
        }

        private void svc_GetFormRequestCompleted(object sender, GetFormRequestCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                var evt = new FormServiceEventArgs(FormServiceType.Refresh, e.Error);
                if (FormServiceCalled != null)
                    FormServiceCalled(this, evt);
            }
            else
            {
                GetFormRequestedCompleted(e.Result);
            }
        }

        private string ConvertCR2CRLF(string text)
        {
            if (text == null) return null;
            string output = text;
            output = output.Replace("\r\n", "\n");
            output = output.Replace("\r", "\n");
            output = output.Replace("\n", "\r\n");
            return output;
        }

        public FormServiceEventArgs CancelRequest(string reason)
        {
            if (formdata.FormGuid == null)
                throw new ArgumentNullException("FormGuid on the FormData Object is required");
            
            if (String.IsNullOrEmpty(reason))
                throw new ArgumentNullException("reason cannot be null or empty");

            FWBS.OMS.FormsPortalService.FormsPortalService svc = FormDefinition.GetService(formdata.WebserviceUrl, formdata.WebserviceUser, formdata.WebservicePassword);
            svc.CancelFormRequestCompleted += new CancelFormRequestCompletedEventHandler(svc_CancelFormRequestCompleted);

            if (runSync == false)
                return GetCancelRequestedCompleted(svc.CancelFormRequest(Convert.ToString(formdata.FormGuid),reason,false,""));
            else
            {
                svc.CancelFormRequestAsync(Convert.ToString(formdata.FormGuid),reason,false,"");
                return null;
            }
        }

        private void svc_CancelFormRequestCompleted(object sender, CancelFormRequestCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                var evt = new FormServiceEventArgs(FormServiceType.Refresh, e.Error);
                if (FormServiceCalled != null)
                    FormServiceCalled(this, evt);
            }
            else
            {
                GetCancelRequestedCompleted(e.Result);
            }
        }

        private FormServiceEventArgs GetCancelRequestedCompleted(OnlineFormResponse e)
        {
            if (e.ErrorCollection.Length > 0)
            {
                List<string> errs = new List<string>();
                foreach (ServiceError ee in e.ErrorCollection)
                    errs.Add(ee.Description);
                var evt = new FormServiceEventArgs(FormServiceType.Refresh, errs.ToArray());
                if (FormServiceCalled != null)
                    FormServiceCalled(this, evt);
                return evt;
            }
            else
            {
                formdata.ClearData();
                if (FormServiceCalled != null)
                    FormServiceCalled(this, FormServiceEventArgs.EmptyCancel);
                return FormServiceEventArgs.EmptyCancel;
            }
        }
    }
}
