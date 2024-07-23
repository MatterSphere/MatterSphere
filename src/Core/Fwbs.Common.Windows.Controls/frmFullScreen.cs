using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class frmFullScreen : Form
    {
        
        
        public frmFullScreen()
        {
            InitializeComponent();
        }

        #region Properties
        [Obsolete("Use the Display Image method to display an image")]
        public PictureBox Picture
        {
            get
            {
                return pictureBox1;
            }
        }

        /// <summary>
        /// Set the Image to be Displayed
        /// </summary>
        /// <param name="image">Image to be Displayed</param>
        public void DisplayImage(Image image)
        {
            pictureBox1.Image = image;

            

        }
        #endregion

        #region Events
        private void frmFullScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                Print();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CloseScreen();
            }
            else if (e.KeyCode == Keys.Space)
            {
                SetAppearance();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseScreen();
        }

        private void zoomToFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAppearance();
        }

        private void PrinttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 8, 8);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            CloseScreen();
        }
        #endregion

        #region Methods
        private bool fittowindow = false;
        private void SetAppearance()
        {
            pictureBox1.Dock = DockStyle.Fill;

            if (fittowindow)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                zoomToFitToolStripMenuItem.Text = "Actual Size";
            }
            else
            {
                if (pictureBox1.Size.Height <= pictureBox1.Image.Size.Height || pictureBox1.Size.Width <= pictureBox1.Image.Size.Width)
                {
                    pictureBox1.Dock = DockStyle.None;
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

                }
                else
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage ;

                zoomToFitToolStripMenuItem.Text = "Center Image";
            }

            fittowindow = !fittowindow;
        }
       

        private void Print()
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void CloseScreen()
        {
            this.Close();
        }
        #endregion

        private void frmFullScreen_Shown(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (pictureBox1.Size.Height <= pictureBox1.Image.Size.Height || pictureBox1.Size.Width <= pictureBox1.Image.Size.Width)
                    fittowindow = true;
                else
                    fittowindow = false;

                SetAppearance();
            }
        }

    }
}