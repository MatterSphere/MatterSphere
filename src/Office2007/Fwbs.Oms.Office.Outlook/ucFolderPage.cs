using System;
using System.IO;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.Office;

namespace Fwbs.Oms.Office.Outlook
{
    public partial class ucFolderPage : UserControl
    {
        private Fwbs.Oms.Office.Common.OfficeOMSAddin _addin;
        string _viewmode;
        string _customform;
        FWBS.OMS.OMSFile _file;

        public Fwbs.Oms.Office.Common.OfficeOMSAddin Addin
        {
            get { return _addin; }

        }


        private ucFolderPage()
        {
            InitializeComponent();
        }

        public ucFolderPage(Fwbs.Oms.Office.Common.OfficeOMSAddin addin)
        {
            InitializeComponent();
            _addin = addin;

            var inFolder = ((Microsoft.Office.Interop.Outlook.Application)_addin.Application).ActiveExplorer().CurrentFolder as Microsoft.Office.Interop.Outlook.MAPIFolder;

            if (inFolder == null)
            {
                MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetResource("UNFINDFOLD", "Unable to Find the Folder Definition Information or Error Loading", "").Text);
                inFolder.WebViewOn = false;
                return;
            }

            if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
            {
                

                if (inFolder.WebViewURL.Contains("FWBS!"))
                {
                    string _form = "";
                    try
                    {

                        _form = Path.GetFileNameWithoutExtension(inFolder.WebViewURL).Substring(5);

                        if (FWBS.OMS.CodeLookup.GetLookups(_form).Rows.Count > 0)
                        {
                            this.ucOMSTypeBrowser1.TypeCode = _form;
                        }
                        else
                        {
                            this.ucOMSTypeBrowser1.BrowserVisible = false;
                            this.ucOMSTypeBrowser1.ResetViewVisible = false;
                            this.ucOMSTypeBrowser1.DefaultVisible = false;
                            this.ucOMSTypeBrowser1.Connect(_form, FWBS.OMS.Session.CurrentSession.CurrentUser);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + "Code : " + _form + Environment.NewLine + FWBS.OMS.Session.CurrentSession.Resources.GetResource("UNFINDFOL3", "Unable to Load Custom Form - Error Showing Folder", "").Text);
                        inFolder.WebViewOn = false;
                        inFolder.Application.ActiveExplorer().SelectFolder(inFolder);
                        return;
                    }

                }
                else
                {

                    // Look for Current Folder Message to enable functionality.
                    try
                    {
                        Fwbs.Office.Outlook.OutlookItem oi;
                       
                        this.ucOMSTypeBrowser1.BrowserVisible = true;
                        this.ucOMSTypeBrowser1.ResetViewVisible = true;

                        if (FWBS.OMS.CodeLookup.GetLookups("TYPEVIEWS").Rows.Count > 0)
                        {
                            this.ucOMSTypeBrowser1.TypeCode = "TYPEVIEWS";
                        }


                        var omsapp = (OutlookOMS)addin.OMSApplication;

                        _file = omsapp.GetFolderFile(inFolder);
                        oi = omsapp.GetFolderMessage(inFolder, false);

                        _viewmode = omsapp.GetDocVariable(oi, "VIEWMODE", "FILE");
                        _customform = omsapp.GetDocVariable(oi, "CUSTOMFORM", "");


                        try
                        {
                            this.ucOMSTypeBrowser1.BrowserSelectedValue = _viewmode;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        SetViewMode();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + FWBS.OMS.Session.CurrentSession.Resources.GetResource("UNFINDFOL2", "Unable to Decode File from Folder - Error Showing Folder", "").Text);
                        inFolder.WebViewOn = false;
                        inFolder.Application.ActiveExplorer().SelectFolder(inFolder);
                        return;
                    }
                }
            }
        }

        private bool SetViewMode()
        {

            switch (_viewmode)
            {
                case "FILE":
                    return this.ucOMSTypeBrowser1.Connect(_file);
                case "CLIENT":
                    return this.ucOMSTypeBrowser1.Connect(_file.Client);
                default:
                    {
                        if (String.IsNullOrEmpty(_viewmode))
                            return this.ucOMSTypeBrowser1.Connect(_file);
                        else
                            return this.ucOMSTypeBrowser1.Connect(_viewmode, _file);
                    }
            }

        }

        private void  ucOMSTypeBrowser1_ResetViewClick(object sender, EventArgs e)
        {
            if (_addin == null)
                throw new Exception(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("NOAPPPSDTCTR", "No OMSAPP Passed to Constructor in ucFolderPage", "").Text);

            var exp = ((Microsoft.Office.Interop.Outlook.Application)_addin.Application).ActiveExplorer();

            exp.CurrentFolder.WebViewOn = false;
            exp.CurrentFolder.WebViewURL = null;
            exp.SelectFolder(exp.CurrentFolder);

        }

        private void ucOMSTypeBrowser1_BrowserChanged(object sender, EventArgs e)
        {
            string _oldview = _viewmode;
            _viewmode = this.ucOMSTypeBrowser1.BrowserSelectedValue;
            if (!SetViewMode())
            {
                _viewmode = _oldview;
                this.ucOMSTypeBrowser1.BrowserSelectedValue = _viewmode;
            }
        }


    }
}
