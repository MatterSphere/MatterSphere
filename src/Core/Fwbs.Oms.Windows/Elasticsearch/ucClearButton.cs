using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Elasticsearch
{
    public partial class ucClearButton : Button
    {
        private string _caption;

        public ucClearButton()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get { return base.Text; }
            set { _caption = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Enabled)
            {
                this.FlatAppearance.BorderSize = 1;
                this.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            }
            else
            {
                this.FlatAppearance.BorderSize = 0;
                this.ForeColor = System.Drawing.Color.FromArgb(189, 189, 189);
            }

            base.OnPaint(e);
            
            using (var drawBrush = new SolidBrush(this.ForeColor))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(_caption, this.Font, drawBrush, e.ClipRectangle, sf);
            }
        }
    }
}
