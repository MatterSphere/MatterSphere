using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Elasticsearch
{
    public partial class ucFilterLabel : Label
    {
        public event EventHandler onClosed;
        private const string _closeSymbol = "✕";

        public ucFilterLabel()
        {
            InitializeComponent();

            closeButton.Text = _closeSymbol;
        }

        public Guid Key { get; set; }
        public bool IsEntityFilter { get; set; }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var size = TextRenderer.MeasureText(this.Text, this.Font);
            var newWidth = size.Width + 4 + closeButton.Width;
            var newHeight = size.Height + 8;

            return new Size(newWidth, newHeight);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            onClosed?.Invoke(this, e);
        }
    }
}
