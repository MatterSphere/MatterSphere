using System;
using System.Text;

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using Document = OMSDocument;


    /// <summary>
    /// An OMSApp which deals with simple OMS document types.
    /// </summary>
    [System.Runtime.InteropServices.Guid("E0A0AB30-DA8A-4CB6-BBCE-513A657EDDC3")]
    public class SimpleOMS : OMSApp
    {
        #region Field Variables

        private frmSimpleDoc _item = null;

        #endregion

        #region Save Methods


        protected override System.IO.FileInfo GetCurrentFileLocation(object obj)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveForm.ActiveDocument.File;
        }

        protected override void InternalDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version)
        {
            CheckObjectIsDoc(ref obj);

            IStorageItem si = version;
            if (si == null)
                si = doc;

            StorageProvider provider = doc.GetStorageProvider();

            System.IO.FileInfo file = provider.GetLocalFile(si);

            ActiveForm.RefreshXML();
            ActiveForm.ActiveDocument.Save(file.FullName);
            provider.Store(si, file, obj, true, this);
        }

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec)
        {
            CheckObjectIsDoc(ref obj);

            IStorageItem si = prec;
            StorageProvider provider = prec.GetStorageProvider();

            System.IO.FileInfo file = provider.GetLocalFile(si);

            ActiveForm.RefreshXML();
            ActiveForm.ActiveDocument.Save(file.FullName);
            provider.Store(prec, file, obj, true);

        }

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec, PrecedentVersion version)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGSOMSDNTSPPR", "SimpleOMS OMSApp does not support storing of precedent versions.", "").Text);
        }


        protected override void EndDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version)
        {
            CheckObjectIsDoc(ref obj);
            if (ActiveForm.ActiveDocument is SMS)
            {
                SMS sms = (SMS)ActiveForm.ActiveDocument;
                sms.Send();
            }
        }



        #endregion

        #region Print Methods

        public override bool CanPrint
        {
            get
            {
                switch (GetActiveDocType(this))
                {
                    case "RECEIPT":
                        return false;
                    case "SMS":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override void InternalPrint(object obj, int copies)
        {
            CheckObjectIsDoc(ref obj);
            frmSimpleDoc activeDoc = (frmSimpleDoc)obj;

            System.Drawing.Font printFont = new System.Drawing.Font("Arial", 12);
            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();

            doc.PrintPage += delegate(object sender, System.Drawing.Printing.PrintPageEventArgs e)
            {
                System.Drawing.Graphics gr = e.Graphics;
                float linesPerPage = 0;
                float yPos = 0;
                int count = 0;
                float height = printFont.GetHeight(gr);
                float leftMargin = e.MarginBounds.Left;
                float topMargin = e.MarginBounds.Top;
                string line = null;
                System.Drawing.Brush brush = System.Drawing.Brushes.Black;
                System.Drawing.StringFormat fmt = new System.Drawing.StringFormat();
                
                // Calculate the number of lines per page.
                linesPerPage = e.MarginBounds.Height / printFont.GetHeight(gr);

                doc.DocumentName = "SMS Message";

                yPos = topMargin + (count * height);

                using (System.IO.StringReader streamToPrint = new System.IO.StringReader(activeDoc.ActiveDocument.Text))
                {
                    gr.DrawString("SMS Message", printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);

                    gr.DrawString("*****************", printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);

                    Client client = Client.GetClient(Convert.ToInt64(activeDoc.ActiveDocument.GetVariable(OMSApp.CLIENT)));
                    OMSFile file = OMSFile.GetFile(Convert.ToInt64(activeDoc.ActiveDocument.GetVariable(OMSApp.FILE)));
                    Associate assoc = Associate.GetAssociate(Convert.ToInt64(activeDoc.ActiveDocument.GetVariable(OMSApp.ASSOCIATE)));

                    gr.DrawString(String.Format("Document Id: {0}", activeDoc.ActiveDocument.GetVariable(OMSApp.DOCUMENT)), printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);
                    gr.DrawString(String.Format("Ref: {0}/{1}", client.ClientNo, file.FileNo), printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);
                    gr.DrawString(Session.CurrentSession.Terminology.Parse(String.Format("%CLIENT% Name: {0}", client.ClientName), false), printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);
                    gr.DrawString(Session.CurrentSession.Terminology.Parse(String.Format("%FILE% Desc: {0}", file.FileDescription), false), printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);
                    gr.DrawString(String.Format("Sent To: {0} ({1})", assoc.Contact.Name, activeDoc.ActiveDocument.GetExtraInfo("_number", "n/a")), printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);
                    //UTCFIX: DM - 30/11/06 - No need to fix local time required, added time zone support.
                    gr.DrawString(String.Format("Printed On {0} UTC{0:zzz} By {1}", System.DateTime.Now, Session.CurrentSession.CurrentUser.FullName), printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);

                    // Print each line of the file.
                    gr.DrawString("*****************", printFont, brush, leftMargin, yPos, fmt);
                    count++;
                    yPos = topMargin + (count * height);

                    while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
                    {
                        int interval = 76;
                        if (line.Length > interval)
                        {
                            yPos = topMargin + (count * height);
                            gr.DrawString(WordWrap(line,interval).ToString(), printFont, brush, leftMargin, yPos, fmt);
                            count++;
                        }
                        else
                        {
                            yPos = topMargin + (count * height);
                            gr.DrawString(line, printFont, brush, leftMargin, yPos, fmt);
                            count++;
                        }
                    }

                    streamToPrint.Close();
                }

                // If more lines exist, print another page.
                if (line != null)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            };

            doc.PrinterSettings.Copies = unchecked((short)copies);
            doc.Print();
        }
        private StringBuilder WordWrap(string message, int length)
        {
            StringBuilder build = new StringBuilder();
            length++;
            while (!string.IsNullOrEmpty(message))
            {
                var lineLength = message.Length > length ? length : message.Length;

                string line = message.Substring(0, lineLength);
                if (!line.EndsWith(" "))
                {
                    var lastSpace = line.LastIndexOf(" ");
                    if (lastSpace != -1)
                        lineLength = lastSpace;

                    line = message.Substring(0, lineLength);

                }
                build.AppendLine(line);
                message = message.Remove(0, lineLength);
                message = message.TrimStart();

            }
            return build;
        }


        #endregion

        #region Field Routines

        public override void CheckFields(object obj)
        {
            CheckObjectIsDoc(ref obj);
            System.Text.RegularExpressions.MatchCollection fieldMatches = GetFieldRegExMatches(obj);
            string text = Convert.ToString(ActiveForm.DisplayText);
            foreach (System.Text.RegularExpressions.Match match in fieldMatches)
            {
                string field = match.Groups["FieldName"].Value;
                text = text.Replace("~" + field + "~", Convert.ToString(GetDocVariable(obj, field)));
            }
            ActiveForm.DisplayText = text;
            fieldMatches = null;
        }

        public override void AddField(object obj, string name)
        {
            base.AddField(obj, name);
            CheckObjectIsDoc(ref obj);
            name = "~" + name + "~";
            string text = ActiveForm.DisplayText + name;
            ActiveForm.DisplayText = text;
        }


        public override void DeleteField(object obj, string name)
        {
            string val = GetDocVariable(obj, name, "");
            base.DeleteField(obj, name);
            CheckObjectIsDoc(ref obj);
            name = "~" + name + "~";
            string text = ActiveForm.DisplayText;
            text = text.Replace(name, "");
            text = text.Replace(val, "");
            ActiveForm.Text = val;
            ScreenRefresh();
        }

        public override int GetFieldCount(object obj)
        {
            CheckObjectIsDoc(ref obj);
            System.Text.RegularExpressions.MatchCollection fieldMatches = GetFieldRegExMatches(obj);
            foreach (System.Text.RegularExpressions.Match match in fieldMatches)
            {
                string field = match.Groups["FieldName"].Value;
                if (HasDocVariable(obj, field) == false)
                    SetDocVariable(obj, field, "");
            }


            return ActiveForm.ActiveDocument.VariableCount;
        }


        public override string GetFieldName(object obj, int index)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveForm.ActiveDocument.GetVariableName(index);
        }

        public override object GetFieldValue(object obj, int index)
        {
            CheckObjectIsDoc(ref obj);
            string field = GetFieldName(obj, index);
            return GetDocVariable(obj, field);
        }

        public override object GetFieldValue(object obj, string name)
        {
            CheckObjectIsDoc(ref obj);
            return GetDocVariable(obj, name);
        }

        public override void SetFieldValue(object obj, int index, object val)
        {
            CheckObjectIsDoc(ref obj);
            string field = GetFieldName(obj, index);
            SetDocVariable(obj, field, val);
        }

        public override void SetFieldValue(object obj, string name, object val)
        {
            CheckObjectIsDoc(ref obj);
            SetDocVariable(obj, name, val);
        }

        private System.Text.RegularExpressions.MatchCollection GetFieldRegExMatches(object obj)
        {
            CheckObjectIsDoc(ref obj);
            string text = Convert.ToString(ActiveForm.GetText("_text"));
            const string fieldregx = "~(?<FieldName>.*?)~";
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline);
            return matches;
        }

        #endregion

        #region Document Variable Routines


        protected override object GetDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);
            if (HasDocVariable(obj, varName))
            {
                return ActiveForm.ActiveDocument.GetVariable(varName);
            }
            return null;
        }

        public override bool HasDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveForm.ActiveDocument.HasVariable(varName);
        }

        public override void RemoveDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);
            ActiveForm.ActiveDocument.RemoveVariable(varName);
        }


        protected override bool SetDocVariable(object obj, string varName, object val)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveForm.ActiveDocument.SetVariable(varName, val);
        }


        #endregion

        #region IOMSApp Implementation

        public override void Close(object obj)
        {
            CheckObjectIsDoc(ref obj);
            ActiveForm.Close();
        }

        public override string DefaultDocType
        {
            get
            {
                return "RECEIPT";
            }
        }


        public override string ExtractPreview(object obj)
        {
            return ActiveForm.DisplayText;
        }

        protected override string GenerateDocDesc(object obj)
        {
            switch (GetActiveDocType(obj))
            {
                case "RECEIPT":
                    return ActiveForm.DisplayText;
                default:
                    return base.GenerateDocDesc(obj);
            }
        }

        protected override DocumentDirection GetActiveDocDirection(object obj, Precedent prec)
        {
            switch (GetActiveDocType(obj))
            {
                case "RECEIPT":
                    return DocumentDirection.In;
                default:
                    return DocumentDirection.Out;
            }
        }


        public override string ModuleName
        {
            get
            {
                return "Simple Document Application";
            }
        }

        public override System.Windows.Forms.IWin32Window ActiveWindow
        {
            get
            {
                return ActiveForm;
            }
        }

        protected override object InternalDocumentOpen(Document document, FetchResults fetchData, OpenSettings settings)
        {
            System.IO.FileInfo file = fetchData.LocalFile;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(file.FullName);

            switch (document.DocumentType)
            {
                case "RECEIPT":
                    {
                        switch (settings.Mode)
                        {
                            case DocOpenMode.Edit:
                                frmSimpleDoc edit = new frmSimpleDoc(new SimpleDoc(doc, file), Session.CurrentSession.DefaultSystemForm(SystemForms.DocReceipt), this, false);
                                edit.Show();
                                return edit;
                            case DocOpenMode.Print:
                                object[] docs = new object[1];
                                docs[0] = new frmSimpleDoc(new SimpleDoc(doc, file), Session.CurrentSession.DefaultSystemForm(SystemForms.DocReceipt), this, true);
                                BeginPrint(docs, settings.Printing);
                                return null;
                            case DocOpenMode.View:
                                frmSimpleDoc view = new frmSimpleDoc(new SimpleDoc(doc, file), Session.CurrentSession.DefaultSystemForm(SystemForms.DocReceipt), this, true);
                                view.Show();
                                return view;
                        }
                    }
                    break;
                case "SMS":
                    {
                        switch (settings.Mode)
                        {
                            case DocOpenMode.Edit:
                                frmSimpleDoc edit = new frmSimpleDoc(new SimpleDoc(doc, file), Session.CurrentSession.DefaultSystemForm(SystemForms.SMSEdit), this, true);
                                edit.Show();
                                return edit;
                            case DocOpenMode.Print:
                                object[] docs = new object[1];
                                docs[0] = new frmSimpleDoc(new SimpleDoc(doc, file), Session.CurrentSession.DefaultSystemForm(SystemForms.SMSEdit), this, true);
                                BeginPrint(docs, settings.Printing);
                                return docs[0];
                            case DocOpenMode.View:
                                goto case DocOpenMode.Edit;
                        }
                    }
                    break;
            }
            return null;

        }

        protected override object InternalPrecedentOpen(Precedent precedent, FetchResults fetchData, OpenSettings settings)
        {
            System.IO.FileInfo file = fetchData.LocalFile;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(file.FullName);
            switch (settings.Mode)
            {
                case DocOpenMode.Edit:
                    return null;
                case DocOpenMode.Print:
                    return null;
                case DocOpenMode.View:
                    return null;
            }
            return null;
        }


        protected override object TemplateStart(object obj, PrecedentLink preclink)
        {
            System.IO.FileInfo tmppath = null;

            //Get the Precedent File to Load...
            FWBS.OMS.DocumentManagement.Storage.FetchResults fetch = preclink.Merge();

            if (fetch == null)
                return null;

            tmppath = fetch.LocalFile;

            if (tmppath != null)
            {
                System.Xml.XmlDocument prec = new System.Xml.XmlDocument();
                prec.Load(tmppath.FullName);
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(prec.OuterXml);

                switch (preclink.Precedent.PrecedentType.ToUpper())
                {
                    case "RECEIPT":
                        {
                            _item = new frmSimpleDoc(new SimpleDoc(doc, tmppath), preclink.Associate, Session.CurrentSession.DefaultSystemForm(SystemForms.DocReceipt), this);
                            _item.Show();
                        }
                        break;
                    case "SMS":
                        {
                            Session.CurrentSession.ValidateLicensedFor("SMS");
                            _item = new frmSimpleDoc(new SMS(doc, tmppath), preclink.Associate, this);
                            _item.Show();
                        }
                        break;
                }

                return ActiveForm;
            }
            else
                return null;
        }

        #endregion

        #region Methods


        protected override void CheckObjectIsDoc(ref object obj)
        {
            if (obj == this)
                obj = ActiveForm;
            else if (obj is frmSimpleDoc)
                ActiveForm = (frmSimpleDoc)obj;
            else
                throw new Exception(Session.CurrentSession.Resources.GetMessage("MSGPRMISNTSDO", "The passed parameter is not a Simple Document object.", "").Text);
        }

        protected override void InsertText(object obj, PrecedentLink precLink)
        {
        }


        protected override void SetWindowCaption(object obj, string caption)
        {
            CheckObjectIsDoc(ref obj);
            ActiveForm.Text = caption;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the active document.
        /// </summary>
        internal frmSimpleDoc ActiveForm
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }


        #endregion

    }
}
