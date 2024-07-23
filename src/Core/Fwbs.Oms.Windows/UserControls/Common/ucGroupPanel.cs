using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Common
{
    public partial class ucGroupPanel : UserControl
    {
        public ucGroupPanel()
        {
            InitializeComponent();
        }

        public ucGroupPanel(int width) : this()
        {
            Width = width;
        }

        public UserControl GroupContent { get; private set; }

        public string TitleLabel
        {
            get { return Title.Text; }
            set { Title.Text = value; }
        }

        public int TableHeight
        {
            get { return TableLayoutPanel.Height + Title.Height; }
        }

        public void AddContent(UserControl control)
        {
            TableLayoutPanel.Controls.Add(control, 0, 1);
            control.Dock = DockStyle.Top;
            GroupContent = control;
            this.Height = TableHeight;
        }

        public void AddContent(Control control)
        {
            var panel = new ucPanel
            {
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(control);

            TableLayoutPanel.Controls.Add(panel, 0, 1);
            control.Dock = DockStyle.Fill;
            GroupContent = panel;
            this.Height = TableHeight;
        }

        public void AddContent(string text)
        {
            var richText = new System.Windows.Forms.RichTextBox
            {
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                ReadOnly = true,
                ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None,
                WordWrap = false,
                BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244))))),
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            if (text.ToLower().StartsWith(@"{\\rtf1", StringComparison.OrdinalIgnoreCase)
                || text.ToLower().StartsWith(@"{\rtf1", StringComparison.OrdinalIgnoreCase))
            {
                richText.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(text);
            }
            else
            {
                richText.Text = text;
            }

            SizeF f;
            using (System.Drawing.Graphics g = CreateGraphics())
            {
                try
                {
                    f = g.MeasureString(richText.Text, richText.SelectionFont);
                }
                catch
                {
                    f = g.MeasureString(richText.Text, richText.Font);
                }
            }
            
            Height = Convert.ToInt32(f.Height + Title.Height);
            Width = Convert.ToInt32(f.Width);
            AddContent(richText);
        }
    }
}
