using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Common
{
    public partial class ucLabelField : UserControl
    {
        public ucLabelField()
        {
            InitializeComponent();
        }

        public void SetTextProperty(string label, string value, LabelStyle style = LabelStyle.Text)
        {
            Title.Text = label;
            if (style == LabelStyle.Text)
            {
                Value.Text = value;
                Link.Visible = false;
            }
            else
            {
                Link.Text = value;
                Value.Visible = false;
            }
        }

        public enum LabelStyle
        {
            Text,
            Link
        }
    }
}
