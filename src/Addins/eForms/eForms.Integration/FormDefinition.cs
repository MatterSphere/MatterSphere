using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FWBS.OMS.FormsPortalService;

namespace FWBS.OMS
{
    /// <summary>

    /// </summary>
    public class FormDefinition : FWBS.OMS.CommonObject
    {
        #region Fields
        private bool tobeRefreshed = false;
        private string webserviceurl;
        private string webserviceuser;
        private string webservicepass;
        #endregion

        private FormDefinition()
        {
        }

        public FormDefinition(Int32 ID,string url,string user, string password)
        {
            webserviceurl = url;
            webserviceuser = user;
            webservicepass = password;
            Fetch(ID);
        }

        public override void Refresh()
        {
            tobeRefreshed = true;
            base.Refresh();
        }

        protected override void Fetch(object id)
        {
            if (this.Exists(id) == false || tobeRefreshed)
            {
                FWBS.OMS.FormsPortalService.FormsPortalService svc = GetService(webserviceurl,webserviceuser,webservicepass);
                OnlineFormResponse resp = svc.GetOnlineForm(Convert.ToInt32(id));
                if (resp.ErrorCollection.Length > 0)
                    throw new FormDataException(resp.ErrorCollection[0].Description);

                OnLineForm form = resp.FormData;
                
                List<Control> ctrls = new List<Control>();
                foreach (Page p in form.Pages)
                    foreach (Control c in p.Controls)
                        ctrls.Add(c);
                this.UniqueID = id;
                this.Name = form.Caption;
                serializeDefinition(ctrls);
                this.Update();
            }
            else
                base.Fetch(id);
        }

        #region Private
        private void serializeDefinition(List<Control> control)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter write = new XmlTextWriter(stream, Encoding.ASCII))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<Control>));
                    ser.Serialize(write, control);
                    stream.Position = 0;
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    SetExtraInfo("Definition", enc.GetString(buffer));
                }
            }
        }

        private List<Control> deserializeDefinition()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] buffer = encoding.GetBytes(Convert.ToString(GetExtraInfo("Definition")));
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;
                XmlSerializer ser = new XmlSerializer(typeof(List<Control>));
                XmlTextReader read = new XmlTextReader(stream);
                List<Control> control = (List<Control>)ser.Deserialize(read);
                read.Close();
                return control;
            }
        }
        #endregion

        #region Properties
        private List<Control> controls;
        public IEnumerable<Control> Controls
        {
            get
            {
                if (controls == null)
                    controls = deserializeDefinition();
                foreach (Control c in controls)
                    yield return c;
            }
        }

        public string Name
        {
            get
            {
                return Convert.ToString(GetExtraInfo("Name"));
            }
            set
            {
                SetExtraInfo("Name", value);
            }
        }
        #endregion

        #region CommonObject
        protected override string SelectStatement
        {
            get { return "SELECT * FROM dbFPEDefinition"; }
        }

        protected override string PrimaryTableName
        {
            get { return "Definition"; }
        }

        protected override string DefaultForm
        {
            get { return ""; }
        }

        public override object Parent
        {
            get { return null; }
        }

        public override string FieldPrimaryKey
        {
            get { return "ID"; }
        }

        protected override string SelectExistsStatement
        {
            get
            {
                return "SELECT ID FROM dbFPEDefinition";
            }
        }
        #endregion

        #region Static

        internal static FWBS.OMS.FormsPortalService.FormsPortalService GetService(string url,string user,string password)
        {
            FWBS.OMS.FormsPortalService.FormsPortalService svc = new FWBS.OMS.FormsPortalService.FormsPortalService();
            svc.Url = url;
            svc.Proxy = GetProxy(svc.Url);
            AuthenticationHeader h = new AuthenticationHeader();
            h.UserName = user;
            h.Password = password;
            svc.AuthenticationHeaderValue = h;
            return svc;
        }

        private static IWebProxy GetProxy(string url)
        {
            var proxy = WebRequest.DefaultWebProxy;

            Uri resource = new Uri(url);

            // See what proxy is used for resource.
            Uri resourceProxy = proxy.GetProxy(resource);

            // Test to see whether a proxy was selected.
            if (resourceProxy == resource)
            {
                return null;
            }
            else
            {
                //set proxy to use the users default network credentials
                proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                return proxy;
            }
        }

        #endregion
    }
}
