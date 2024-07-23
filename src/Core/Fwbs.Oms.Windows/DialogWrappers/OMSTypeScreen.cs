namespace FWBS.OMS.UI
{
    using System.Windows.Forms;
    using FWBS.OMS.UI.Windows;

    public class OMSTypeScreen
    {
        #region Constructors

        public OMSTypeScreen(FWBS.OMS.Interfaces.IOMSType obj)
        {
            this.obj = obj;
        }

        #endregion

        #region Properties

        private FWBS.OMS.Interfaces.IOMSType obj;

        private OMSType omsType;
        public OMSType OmsType
        {
            set { omsType = value; }
        }

        private string defaultPage;
        public string DefaultPage
        {
            set
            {
                defaultPage = value;

                if (obj != null)
                {
                    obj.DefaultTab = value;
                }
            }
        }

        #endregion

        #region Methods

        public void ShowDialog()
        {
            ShowDialog(null);
        }


        public void ShowDialog(IWin32Window owner)
        {
            Show(owner,true);
        }


        public void Show()
        {
            Show(null);
        }


        public void Show(IWin32Window owner)
        {
            Show(owner, false);

        }


        private void Show(IWin32Window owner, bool modal)
        {
            if (Services.CheckLogin())
            {
                var clientform = new frmOMSTypeV2(obj, omsType, defaultPage);
                var omsTypeViewer = new OMSTypeViewer(clientform, owner, modal, defaultPage);
                omsTypeViewer.ShowClientForm();
            }
        }

        #endregion Methods
    }


    internal class OMSTypeViewer
    {
        #region Members

        bool modal;
        IWin32Window owner;
        string defaultPage = null;
        FWBS.OMS.UI.Windows.Interfaces.IfrmOMSType clientform;

        #endregion Members

        #region Constructors

        public OMSTypeViewer(FWBS.OMS.UI.Windows.Interfaces.IfrmOMSType clientForm, IWin32Window owner, bool modal, string defaultPage)
        {
            this.clientform = clientForm;
            this.defaultPage = defaultPage;
            this.modal = modal;
            this.owner = owner;
        }

        #endregion Constructors

        #region Methods

        public void ShowClientForm()
        {
            clientform.SetTabPage(defaultPage);

            if (modal)
            {
                try
                {
                    clientform.ShowDialog(owner);
                }
                finally
                {
                    clientform.Dispose();
                }
            }
            else
            {
                clientform.Owner = Services.MainWindow;
                if (owner != null)
                {
                    FWBS.Common.Functions.SetParentWindow(owner, Services.MainWindow);
                }
                clientform.Show();
            }
        }

        #endregion Methods
    }
}
