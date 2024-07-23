using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class ucNavigationItem : UserControl
    {
        public event EventHandler onClicked;
        
        private int _imageIndex;
        private Color _borderColor = Color.FromArgb(102, 102, 102);

        private ucNavigationItem(Image icon, string title)
        {
            InitializeComponent();
           
            PictureBox.Image = icon;
            Label.Text = title;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(Label, title);
            toolTip.SetToolTip(PictureBox, title);
        }

        public ucNavigationItem(string title, string code, Image icon, int imageIndex) : this(icon, title)
        {
            _imageIndex = imageIndex;

            Code = code;
            Label.Cursor = Cursors.Hand;
            PictureBox.Cursor = Cursors.Hand;
        }

        public ucNavigationItem(string title, Image icon) : this(icon, title)
        {
            Label.Font = new Font("Segoe UI Semibold", 10.5F);
            Label.ForeColor = Color.FromArgb(51, 51, 51);
            Label.Padding = new Padding(20, 0, 0, 0);
            PictureBox.Visible = false;
        }

        public string Code { get; }

        public void SelectItem()
        {
            this.BackColor = _borderColor;
        }

        public void UnselectItem()
        {
            this.BackColor = Color.FromArgb(244, 244, 244);
        }

        public void ChangeGroupItemState(bool isCompact)
        {
            PictureBox.Visible = isCompact;
            Label.Visible = !isCompact;
        }

        public void ChangePageItemState(bool isCompact)
        {
            Label.Visible = !isCompact;
        }

        public void UpdateImage(ImageList images)
        {
            try
            {
                PictureBox.Image = images.Images[_imageIndex];
            }
            catch
            {
                PictureBox.Image = images.Images[0];
            }
        }

        public void UpdateImage(Image image)
        {
            PictureBox.Image = image;
        }

        private void Label_Click(object sender, System.EventArgs e)
        {
            onClicked?.Invoke(this, e);
        }

        private void PictureBox_Click(object sender, System.EventArgs e)
        {
            onClicked?.Invoke(this, e);
        }
    }
}
