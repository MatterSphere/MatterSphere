using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    using FWBS.OMS.UI.Windows;

    /// <summary>
    /// Static wizard class that runs different types of enquiry form screens.  These wizards include
    /// custom and globally used ones.
    /// </summary>
    public class Screen
    {
        #region Fields

        private EnquiryEngine.Enquiry _enq = null;

        #endregion

        #region Constructors

        protected Screen()
        {
        }

        public Screen(EnquiryEngine.Enquiry enq)
        {
            _enq = enq;
        }

        #endregion

        #region Properties

        #endregion

        #region Instance Methods

        public object Show(IWin32Window owner)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                object obj = null;

                if (Services.CheckLogin())
                {
                    using (frmOMSItem frm = new frmOMSItem(_enq))
                    {
                        if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                            obj = frm.EnquiryForm.Enquiry.Object;
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(owner, ex);
                return null;
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


namespace FWBS.OMS.UI.Windows
{
    partial class Services
    {
        [Obsolete("Please use FWBS.OMS.UI.Screen instead.")]
        public sealed class Screens : FWBS.OMS.UI.Screen
        {
            public Screens(EnquiryEngine.Enquiry enq) : base(enq)
            {
            }
        }
    }
}