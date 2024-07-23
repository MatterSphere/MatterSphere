using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Static wizard class that runs different types of enquiry form screens.  These wizards include
    /// custom and globally used ones.
    /// </summary>
    public sealed class ExtendedDataScreen
    {
        #region Fields

        private string _extcode = null;
        private FWBS.OMS.Interfaces.IEnquiryCompatible _enq = null;
        #endregion

        #region Constructors

        private ExtendedDataScreen()
        {
        }

        public ExtendedDataScreen(string ExtendedCode, FWBS.OMS.Interfaces.IEnquiryCompatible enq)
        {
            _extcode = ExtendedCode;
            _enq = enq;
        }

        #endregion

        #region Properties

        #endregion

        #region Instance Methods

        public bool Show(IWin32Window owner)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                bool obj = false;

                if (Services.CheckLogin())
                {
                    using (frmExtDataScreen frm = new frmExtDataScreen(_extcode, _enq))
                    {
                        if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                            obj = true;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(owner, ex);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public object Show()
        {
            return Show(null);
        }


        #endregion
    }
}


