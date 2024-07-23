using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    [System.ComponentModel.DesignerCategory("Code")]
    partial class frmNewBrandIdent
    {
        interface ICustomTitleBarStyle
        {
            int ButtonsAreaHeight { get; }
            
            int CaptionHeight { get; }

            void Draw(TitleBarButtonsControl c, Graphics g, int height, Icon icon);
        }

        class LargeTitleBarStyle : ICustomTitleBarStyle
        {
            private static readonly PrivateFontCollection _logoFonts;

            private static void InitFont(string resourceName)
            {
                byte[] fontData;
                using (var fontStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("FWBS.OMS.UI.Resources.TitleBar." + resourceName))
                {
                    fontData = new byte[fontStream.Length];
                    fontStream.Read(fontData, 0, (int) fontStream.Length);
                }

                IntPtr fontMem = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, fontMem, fontData.Length);
                _logoFonts.AddMemoryFont(fontMem, fontData.Length);
                Marshal.FreeCoTaskMem(fontMem);
            }

            static LargeTitleBarStyle()
            {
                _logoFonts = new PrivateFontCollection();
            }

            int ICustomTitleBarStyle.ButtonsAreaHeight => 40;

            int ICustomTitleBarStyle.CaptionHeight => 48;

            void ICustomTitleBarStyle.Draw(TitleBarButtonsControl c, Graphics g, int height, Icon icon)
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center })
                {
                    Rectangle rect = Rectangle.FromLTRB(c.LogicalToDeviceUnits(8), 0, c.Left, height);
                    using (var logoImage = Properties.Resources.elite_sym_rgb_wht_rev)
                    {
                        var widthRatio = (float)logoImage.Width / (float)logoImage.Height;
                        var logoRect = new Rectangle(rect.Left, rect.Top, (int)Math.Round(height * widthRatio), height);
                        g.DrawImage(logoImage, logoRect);
                        rect.Inflate(-logoRect.Width, 0);
                    }

                    // height of the 3E MATTERSPHERE depends on title height and equals to 1/2 to keep spacing around
                    using (Font regFont = new Font("Verdana", height / 2, FontStyle.Regular, GraphicsUnit.Pixel))
                    {
                        g.DrawString("3E MATTERSPHERE", regFont, Brushes.White, rect, sf);
                    }
                }
            }
        }

        class SmallTitleBarStyle : ICustomTitleBarStyle
        {
            private const int IconMarginDefault = 4;
            private Icon _cachedIcon = null;

            int ICustomTitleBarStyle.ButtonsAreaHeight => 32;

            int ICustomTitleBarStyle.CaptionHeight => 32;

            void ICustomTitleBarStyle.Draw(TitleBarButtonsControl c, Graphics g, int height, Icon icon)
            {
                int leftInitialMargin = (icon == null) ? 4 : 0;
                Rectangle rect = Rectangle.FromLTRB(c.LogicalToDeviceUnits(leftInitialMargin), 0, c.Left, height);

                int fontSize = (height - (c.LogicalToDeviceUnits(leftInitialMargin)*2))/2;
                if (icon != null)
                {
                    int newSize = (int) Images.GetIconSize(c.DeviceDpi, 24);
                    if (_cachedIcon == null)
                    {
                        _cachedIcon = newSize == icon.Height ? icon : new Icon(icon, newSize, newSize);
                    }
                    else
                    {
                        if (newSize != _cachedIcon.Height)
                            _cachedIcon = new Icon(icon, newSize, newSize);
                    }
                    Point iconLocation = rect.Location;
                    int iconMargin = (height - newSize)/2;
                    iconLocation.Offset(iconMargin, iconMargin);
                    Rectangle iconRect = new Rectangle(iconLocation, new Size(height - (iconMargin*2), height - (iconMargin*2)));
                    g.DrawIcon(_cachedIcon, iconRect);
                    // new position for title
                    rect.Inflate(-(iconRect.Width + iconMargin*2), 0);
                    // font size depends on title heght and icon margin
                    fontSize = Math.Max((height - iconMargin)/2, (height - c.LogicalToDeviceUnits(IconMarginDefault))/2);
                }

                string text = c.ParentForm.Text;
                if (!string.IsNullOrWhiteSpace(text))
                {
                    using (Font regFont = new Font("Segoe UI Semibold", fontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                    using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter })
                    {
                        g.TextRenderingHint = TextRenderingHint.SystemDefault;
                        g.DrawString(text.Replace(Environment.NewLine, " "), regFont, Brushes.White, rect, sf);
                    }
                }
            }
        }

        class EmptyTitleBarStyle : ICustomTitleBarStyle
        {
            int ICustomTitleBarStyle.ButtonsAreaHeight => 0;

            int ICustomTitleBarStyle.CaptionHeight => 0;

            void ICustomTitleBarStyle.Draw(TitleBarButtonsControl c, Graphics g, int height, Icon icon)
            {
            }
        }

    }
}