using System;
using System.Windows.Forms;

namespace Fwbs
{
    namespace WinFinder.Internal
    {
        internal partial class WindowPicker : Form
        {
            #region Fields

            private Window current;
            private Window last;
            private bool dragging;

            #endregion

            #region Constructors

            public WindowPicker()
            {
                InitializeComponent();
            }

            #endregion

            #region Proeprties

            public Window SelectedWindow
            {
                get
                {
                    return last;
                }
            }

            #endregion

            #region Captured Events

            private void button1_Click(object sender, EventArgs e)
            {
                try
                {
                    if (last != null)
                        last.Text = txtWindowText.Text;
                }
                catch (Exception ex)
                {
                    new MessageBoxProxy(this).Show(ex.Message, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void dragPictureBox_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    dragging = true;
                    this.Cursor = Cursors.Cross;
                }
            }

            private void dragPictureBox_MouseUp(object sender, MouseEventArgs e)
            {
                try
                {
                    if (dragging)
                    {
                        dragging = false;
                        this.Cursor = Cursors.Default;
                        if (current != null)
                        {
                            current.UnHighlight();
                            current = null;
                        }
                    }
                }
                catch { }

            }

            private void dragPictureBox_MouseMove(object sender, MouseEventArgs e)
            {
                try
                {
                    if (dragging)
                    {
                        Window newwin = WindowFactory.GetWindowFromMouse();

                        if (newwin.Handle == dragPictureBox.Handle)
                        {
                            // Drawing a border around the dragPictureBox (where we start
                            // dragging) doesn't look nice, so we ignore this window
                            newwin = null;
                        }

                        if (newwin != current)
                        {
                            if (current != null)
                            {
                                current.UnHighlight();
                            }

                            if (newwin != null)
                                newwin.Highlight();

                            current = newwin;
                            last = current;
                        }

                        if (newwin != null)
                        {
                            txtWindowHandle.Text = newwin.Handle.ToString();
                            txtWindowText.Text = newwin.Text;
                            txtClassName.Text = newwin.Class;
                            txtApplication.Text = newwin.FileName;

                            System.Drawing.Icon ico = newwin.LargeIcon;
                            if (ico == null)
                                ico = newwin.Icon;
                            if (ico != null)
                                dragPictureBox.Image = ico.ToBitmap();
                            else
                                dragPictureBox.Image = null;

                            btnPick.Enabled = true;
                        }
                        else
                        {
                            txtWindowHandle.Text = String.Empty;
                            txtWindowText.Text = String.Empty;
                            txtClassName.Text = String.Empty;
                            txtApplication.Text = String.Empty;
                            dragPictureBox.Image = null;

                            btnPick.Enabled = false;
                        }

                    }
                }
                catch { }
            }

            #endregion


        }
    }
}