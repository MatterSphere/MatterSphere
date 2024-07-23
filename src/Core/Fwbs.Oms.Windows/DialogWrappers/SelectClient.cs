using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    using FWBS.OMS.EnquiryEngine;
    using FWBS.OMS.Search;
    using FWBS.OMS.UI.Windows;

    public sealed class SelectClient
    {
        #region Fields

        private Client client;

        #endregion

        #region Constructors

        public SelectClient()
            : this(Session.CurrentSession.CurrentClient)
        {
        }

        public SelectClient(Client client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        private Client ShowOverride(IWin32Window owner)
        {
            Enquiry e = Enquiry.GetEnquiry(Session.CurrentSession.SelectClientEnquiryOverride, null, EnquiryMode.Add, new FWBS.Common.KeyValueCollection());
            Services.Screens n = new Services.Screens(e);
            Client client = n.Show(owner) as Client;
            return client;
        }

        public Client Show(IWin32Window owner)
        {
            if (!Services.CheckLogin())
                return null;

            if (!string.IsNullOrWhiteSpace(Session.CurrentSession.SelectClientEnquiryOverride))
            {
                return ShowOverride(owner);
            }

            var searchScreen = Session.CurrentSession.Container.TryResolve<ISelectEntity>(null);
            if (searchScreen == null)
            {
                using (frmSelectClient frm = new frmSelectClient(client))
                {

                    DialogResult res = frm.ShowDialog(owner);
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        //Make the newly selected client the session current client.
                        return frm.SelectedClient;
                    }
                    else
                        return null;
                }

            }

            var entityData = new SelectEntityData();
            entityData.SearchType = EntityType.Client;
            searchScreen.Initialise(entityData);
            var result = searchScreen.Execute();

            if (result != null)
            {
                if (result is long)
                    return FWBS.OMS.Client.GetClient((long)result);
                else if (result is string)
                    return FWBS.OMS.Client.GetClient((string)result);
                else
                    throw new InvalidCastException("Unsupported ID returned from the Command");
            }

            return null;



        }

        #endregion
    }
}
