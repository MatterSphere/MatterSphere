using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    using FWBS.OMS.EnquiryEngine;
    using FWBS.OMS.Search;
    using FWBS.OMS.UI.Windows;

    public sealed class SelectAssociate
    {
        #region Fields

        private bool _allowPrivate = Services.AllowPrivateAssociate;
        private bool _showPhases = true;
        private bool _useDefault = false;
        private bool _autoconfirm = false;
        private OMSFile file;

        #endregion

        #region Constructors

        public SelectAssociate()
            : this(Session.CurrentSession.CurrentFile)
        {
        }

        public SelectAssociate(OMSFile file)
        {
            this.file = file;
        }

        #endregion

        #region Properties

        public bool AllowPrivateAssociate
        {
            get
            {
                return _allowPrivate;
            }
            set
            {
                _allowPrivate = value;
            }
        }

        public bool AutoConfirm
        {
            get
            {
                return _autoconfirm;
            }
            set
            {
                _autoconfirm = value;
            }
        }

        public bool ShowPhases
        {
            get
            {
                return _showPhases;
            }
            set
            {
                _showPhases = value;
            }
        }

        public bool UseDefault
        {
            get
            {
                return _useDefault;
            }
            set
            {
                _useDefault = value;
            }
        }

        #endregion

        #region Methods

        private Associate ShowOverride(IWin32Window owner)
        {
            FWBS.Common.KeyValueCollection x = new FWBS.Common.KeyValueCollection();
            x.Add("Private", AllowPrivateAssociate);
            Enquiry e = Enquiry.GetEnquiry(Session.CurrentSession.SelectAssociateEnquiryOverride, null, EnquiryMode.Add, x);
            Services.Screens n = new Services.Screens(e);
            Associate associate = n.Show(owner) as Associate;
            return associate;
        }

        private Associate ShowDefault(IWin32Window owner)
        {
            using (frmSelectClientFile frm = new frmSelectClientFile(file))
            {
                frm.AllowPrivateAssociate = _allowPrivate;
                frm.ShowPhases = _showPhases;
                DialogResult res = frm.ShowDialog(owner);
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    Associate assoc = null;
                    file = frm.SelectedFile;

                    if (file != null)
                        assoc = file.DefaultAssociate;

                    return assoc;
                }
                else if (res == DialogResult.Ignore)
                    return Associate.Private;
                else
                    return null;
            }
        }

        public Associate Show(IWin32Window owner)
        {
            if (!Services.CheckLogin())
                return null;

            if (!string.IsNullOrWhiteSpace(Session.CurrentSession.SelectAssociateEnquiryOverride))
            {
                return ShowOverride(owner);
            }
            else if (_useDefault)
            {
                return ShowDefault(owner);
            }

            var searchScreen = Session.CurrentSession.Container.TryResolve<ISelectEntity>(null);
            if (searchScreen == null)
            {
                using (frmSelectClientFileAssociate frm = new frmSelectClientFileAssociate(file))
                {
                    Associate assoc = null;

                    frm.AllowPrivateAssociate = AllowPrivateAssociate;
                    frm.AutoConfirm = this.AutoConfirm;

                    DialogResult res = frm.ShowDialog(owner);

                    if (res == System.Windows.Forms.DialogResult.OK)
                        assoc = frm.SelectedAssociate;
                    else if (res == DialogResult.Ignore)
                        assoc = Associate.Private;

                    return assoc;
                }

            }

            var entityData = new SelectEntityData();
            entityData.SearchType = EntityType.Associate;
            searchScreen.Initialise(entityData);
            var result = searchScreen.Execute();

            if (result != null)
            {
                if (result is long)
                    return FWBS.OMS.Associate.GetAssociate((long)result);
                else
                    throw new InvalidCastException("Unsupported ID returned from the Command");
            }

            return null;



        }
        #endregion
    }

}
