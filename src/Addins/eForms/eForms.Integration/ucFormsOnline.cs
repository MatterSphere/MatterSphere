using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucFormsOnline : UserControl 
    {
        public ucFormsOnline()
        {
            InitializeComponent();
        }

        public event CancelEventHandler FormRequesting;
        public event EventHandler FormRequested;
        public event CancelEventHandler FormRefreshing;
        public event EventHandler FormRefreshed;

        protected virtual bool OnFormRequesting()
        {
            if (FormRequesting != null)
            {
                CancelEventArgs e = new CancelEventArgs();
                FormRequesting(this, e);
                return e.Cancel;
            }
            else
                return false;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            Font boldFont = new Font(this.Font, FontStyle.Bold);
            labUpdated.Font = boldFont;
            labFormName.Font = boldFont;
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            if (contextCommands.IsHandleCreated)
            {
                contextCommands.Items.Remove(mnuCancelRequest);
                contextCommands.Dispose();
                contextCommands = new ContextMenuStrip(this.components);
                contextCommands.Items.Add(mnuCancelRequest);
            }
        }

        protected virtual void OnFormRequested()
        {
            if (FormRequested != null)
                FormRequested(this, EventArgs.Empty);
        }

        protected virtual bool OnFormRefreshing()
        {
            if (FormRefreshing != null)
            {
                CancelEventArgs e = new CancelEventArgs();
                FormRefreshing(this, e);
                return e.Cancel;
            }
            else
                return false;
        }

        protected virtual void OnFormRefreshed()
        {
            if (FormRefreshed != null)
                FormRefreshed(this, EventArgs.Empty);
        }

        private EnquiryForm parent;
        private FormData obj;
        private FWBS.OMS.Interfaces.IExtendedDataCompatible iext;
        private FormDataEngine engine;

        private void ucFormsOnline_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                parent = this.Parent as EnquiryForm;
                if (parent != null)
                {
                    iext = parent.Enquiry.Object as FWBS.OMS.Interfaces.IExtendedDataCompatible;
                    SetIEnquiryCompatible();
                }
            }
            else
            {
                parent = null;
                iext = null;
                obj = null;
            }
        }

        private string _extdataname;

        public string ExtendedDataName
        {
            get
            {
                return _extdataname;
            }
            set
            {
                _extdataname = value;
                SetIEnquiryCompatible();
            }
        }


        private void SetIEnquiryCompatible()
        {
            if (iext != null && String.IsNullOrEmpty(_extdataname) == false)
            {
                try
                {
                    obj = iext.ExtendedData[_extdataname].Object as FormData;
                    engine = new FormDataEngine(obj,iext,true);
                    engine.FormServiceCalled += new EventHandler<FormServiceEventArgs>(engine_FormServiceCalledInvoke);
                    if (obj != null)
                    {
                        UpdateUI();
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ex);
                    DisableUI();
                }
            }
        }

        private void engine_FormServiceCalledInvoke(object sender, FormServiceEventArgs e)
        {
            EventHandler<FormServiceEventArgs> sch = new EventHandler<FormServiceEventArgs>(this.engine_FormServiceCalled);
            this.Invoke(sch, new object[2] { sender, e });
        }

        private void engine_FormServiceCalled(object sender, FormServiceEventArgs e)
        {
            if (e.Exception != null)
                ErrorBox.Show(e.Exception);
            if (e.Errors != null)
                MessageBox.ShowInformation(String.Join(Environment.NewLine, e.Errors));

            if (e.Type == FormServiceType.Refresh || e.Type == FormServiceType.Cancel)
            {
                OnFormRefreshed();
                UpdateUI();
                if (parent != null)
                    parent.RefreshItem();
            }
            if (e.Type == FormServiceType.Cancel)
            {
                OMSFile file = iext as OMSFile;
                if (file != null)
                {
                    file.AddEvent("EFORMSC", thecancelreason, "");
                    file.Update();
                }
            }

            DisableProcessing();
            UpdateUI();
        }

        private void DisableUI()
        {
            btnRequest.Enabled = false;
            labFormName.Text = "Error";
            labUpdateCaption.Visible = false;
            labUpdated.Visible = false;
        }

        private void UpdateUI()
        {
            if (obj != null)
            {
                labFormName.Text = CodeLookup.GetLookup("EXTENDEDDATA", this.ExtendedDataName);
                if (obj.FormGuid != null)
                {
                    btnRequest.Text = Session.CurrentSession.Resources.GetResource("REFRESH", "Refresh", "").Text;
                    labUpdateCaption.Visible = true;
                    labUpdated.Visible = true;
                    btnDown.Visible = (obj.Completed == null);
                    labUpdated.Text = obj.Refreshed.ToString();
                }
                else
                {
                    btnRequest.Text = Session.CurrentSession.Resources.GetResource("REQUEST", "Request", "").Text;
                    labUpdateCaption.Visible = false;
                    btnDown.Visible = false;
                    labUpdated.Visible = false;
                }
            }
            btnRequest.Enabled = true;
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                ClickRequestRefreshButton();
            }
            catch (Exception ex)
            {
                UpdateUI();
                ErrorBox.Show(ex);
            }
        }

        private void EnableProcessing()
        {
            picProcessing.Visible = true;
            btnRequest.Enabled = false;
        }

        private void DisableProcessing()
        {
            picProcessing.Visible = false;
            btnRequest.Enabled = true;
        }

        public void SetParent(FWBS.OMS.Interfaces.IExtendedDataCompatible obj)
        {
            iext = obj;
            SetIEnquiryCompatible();
        }

        public void ClickRequestRefreshButton()
        {
            if (obj == null)
            {
                throw new OMSException2("ERRNOEXTNM", "The Online Form Control is missing or has an Incorrect ExtendedDataName Property");
            }
            else
            {
                try
                {
                    EnableProcessing();
                    if (obj != null && obj.FormGuid == null)
                    {
                        if (OnFormRequesting())
                            return;
                        engine.RequestForm();
                        OnFormRequested();
                    }
                    else
                    {
                        if (OnFormRefreshing())
                            return;
                        engine.RefreshForm(); 
                    }
                }
                finally
                {
                }
            }
        }

        public string Updated
        {
            get
            {
                return labUpdated.Text;
            }
        }

        public string FormName
        {
            get
            {
                return labFormName.Text;
            }
        }

        public FormData FormData
        {
            get
            {
                return obj;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            contextCommands.Show(btnDown, new Point((btnRequest.Width) * -1, btnDown.Height));
        }

        private void mnuCancelRequest_Click(object sender, EventArgs e)
        {
            EnableProcessing();
            if (OnFormRefreshing())
                return;
            string reason = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetMessage("CANCELREASON","Please enter the reason why you wish to Cancel this request","").Text, "eForms World");
            if (reason == FWBS.OMS.UI.Windows.InputBox.CancelText) return;
            if (String.IsNullOrEmpty(reason))
            {
                MessageBox.ShowInformation("AREASONREQ","A reason is required");
                return;
            }
            thecancelreason = reason;
            engine.CancelRequest(reason);
        }

        private string thecancelreason = "";
    }
}
