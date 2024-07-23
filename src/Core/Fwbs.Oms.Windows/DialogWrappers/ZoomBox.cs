using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Displays a popup dialog with text to edit.
    /// </summary>
    public sealed class ZoomBox
    {
        private ZoomBox() { }

        /// <summary>
        /// Displays the zoom box.
        /// </summary>
        /// <param name="caption">Caption of the form.</param>
        /// <param name="text">Text to edit.</param>
        /// <returns>The edited text.</returns>
        public static string Show(string caption, string text)
        {
            return Show(null, caption, text);
        }


        /// <summary>
        /// Displays the zoom box.
        /// </summary>
        /// <param name="owner">Owner of the form.</param>
        /// <param name="caption">Caption of the form.</param>
        /// <param name="text">Text to edit.</param>
        /// <returns>The edited text.</returns>
        public static string Show(IWin32Window owner, string caption, string text)
        {
            string ret = text;
            using (frmZoomBox frm = new frmZoomBox())
            {
                frm.Text = caption;
                frm.txtText.Text = text;
                frm.ShowDialog(owner);
                if (frm.DialogResult == DialogResult.OK)
                    ret = frm.txtText.Text;
            }
            return ret;
        }
    }

   

}
