using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    using FWBS.OMS.EnquiryEngine;
    using FWBS.OMS.Search;
    using FWBS.OMS.UI.Windows;

    public sealed class SelectFile
    {
        #region Fields

        private OMSFile file;
        private bool _showPhases = false;

        #endregion

        #region Constructors

        public SelectFile()
            : this(Session.CurrentSession.CurrentFile)
        {
        }

        public SelectFile(OMSFile file)
        {
            this.file = file;
        }

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        private OMSFile ShowOverride(IWin32Window owner)
        {
            Enquiry e = Enquiry.GetEnquiry(Session.CurrentSession.SelectFileEnquiryOverride, null, EnquiryMode.Add, new FWBS.Common.KeyValueCollection());
            Services.Screens n = new Services.Screens(e);
            OMSFile file = n.Show(owner) as OMSFile;
            return file;
        }

        public OMSFile Show(IWin32Window owner)
        {
            try
            {
                if (!Services.CheckLogin())
                    return null;

                if (!string.IsNullOrWhiteSpace(Session.CurrentSession.SelectFileEnquiryOverride))
                {
                    return ShowOverride(owner);
                }


                var searchScreen = Session.CurrentSession.Container.TryResolve<ISelectEntity>(null);
                if (searchScreen == null)
                {
                    using (frmSelectClientFile frm = new frmSelectClientFile(file))
                    {
                        frm.ShowPhases = _showPhases;
                        if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                        {
                            //Make the newly selected file the session current file.
                            return frm.SelectedFile;
                        }
                        else
                            return null;
                    }

                }

                var entityData = new SelectEntityData();
                entityData.SearchType = EntityType.File;
                if (file != null)
                {

                    entityData.ParentId = file.ClientID;
                    entityData.ParentType = EntityType.Client;
                    entityData.SearchValue = file.Client.ClientNo + "/" + file.FileNo;//POTENTIALLY SLOW!!!
                }
                searchScreen.Initialise(entityData);
                var result = searchScreen.Execute();

                if (result != null)
                {
                    if (result is long)
                        return FWBS.OMS.OMSFile.GetFile((long)result);
                    else
                        throw new InvalidCastException("Unsupported ID returned from the Command");
                }

                return null;

            }
            catch (OMSException ex)
            {
                if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                {
                    throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                }

                return null;
            }
        }

        #endregion
    }
}
