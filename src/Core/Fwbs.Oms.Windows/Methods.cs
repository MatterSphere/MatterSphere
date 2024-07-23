using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    public class Methods
    {
        private static Font NewFont(string fontFamily, float fontSize = 0, FontStyle fontStyle = FontStyle.Regular)
        {
            return new Font(fontFamily ?? CurrentUIVersion.Font, (fontSize == 0) ? CurrentUIVersion.FontSize : fontSize, fontStyle);
        }


        /// <summary>
        /// Sets the font and font size of a given control.
        /// </summary>
        /// <param name="control"></param>
        public static void SetFont(Control control, string font, float fontSize = 0)
        {
            control.Font = NewFont(font, fontSize);
        }


        /// <summary>
        /// Sets the font, colour and font size of a given control.
        /// </summary>
        /// <param name="control"></param>
        public static void SetFont(Control control, string font, System.Drawing.Color colour, float fontSize = 0)
        {
            control.Font = NewFont(font, fontSize);
            control.ForeColor = colour;
        }


        /// <summary>
        /// Sets the font, colour, font size and font style of a given control.
        /// </summary>
        /// <param name="control"></param>
        public static void SetFont(Control control, string font, System.Drawing.Color colour, float fontSize = 0, FontStyle fontStyle = FontStyle.Regular)
        {
            control.Font = NewFont(font, fontSize, fontStyle);
            control.ForeColor = colour;
        }

    }
}
