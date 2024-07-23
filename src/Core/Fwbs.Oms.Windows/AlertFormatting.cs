using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    internal interface IAlertFormatting
    {
        Color BackColor { get; }
        Color ForeColor { get; }
        Font Font { get; }
        ContentAlignment TextAlign { get; }
        DockStyle DockStyle { get; }
    }


    internal class Version2UIAlertFormatting : IAlertFormatting
    {
        Alert alert;

        public Version2UIAlertFormatting(Alert alert)
        {
            this.alert = alert;
        }

        public Color BackColor
        {
            get
            {
                return Color.WhiteSmoke;
            }
        }

        public Color ForeColor
        {
            get
            {
                switch (alert.Status)
                {
                    case Alert.AlertStatus.Red:
                        return Color.Red;
                    case Alert.AlertStatus.Amber:
                        return Color.Orange;
                    case Alert.AlertStatus.Green:
                        return Color.Green;
                    default:
                        goto case Alert.AlertStatus.Red;
                }
            }
        }


        public Font Font
        {
            get
            {
                return new System.Drawing.Font(CurrentUIVersion.Font, 10.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            }
        }


        public ContentAlignment TextAlign
        {
            get 
            {
                return ContentAlignment.MiddleCenter;
            }
        }


        public DockStyle DockStyle
        {
            get 
            {
                return DockStyle.Top;
            }
        }
    }

}
