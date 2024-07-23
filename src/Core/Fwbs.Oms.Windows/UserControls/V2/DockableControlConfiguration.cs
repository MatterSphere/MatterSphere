using System.Drawing;
using Infragistics.Win.UltraWinDock;

namespace FWBS.OMS.UI.Windows
{
    internal class DockableControlConfiguration
    {
        internal DockableControlConfiguration()
        {
        }


        internal void SetDockManagerStyle(UltraDockManager dockManager, DockManagerSettings dockManagerSettings)
        {
            if (dockManager != null)
            {
                Infragistics.Win.Appearance tabAreaAppearance = (!dockManager.Appearances.Exists("unpinnedTabArea"))
                                                                    ? dockManager.Appearances.Add("unpinnedTabArea")
                                                                    : dockManager.Appearances["unpinnedTabArea"];

                tabAreaAppearance.BackColor = dockManagerSettings.BackColor;
                tabAreaAppearance.BorderColor = dockManagerSettings.BorderColor;
                dockManager.UnpinnedTabAreaAppearance = tabAreaAppearance;
                
                dockManager.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
        }


        internal void SetDockPanelStyle(DockablePaneBase dockableControlPane, DockPanelSettings dockPanelSettings)
        {
            if (dockableControlPane != null)
            {
                SetCaptionAreaStyle(dockableControlPane, dockPanelSettings.CaptionSettings);
                SetTabAreaStyle(dockableControlPane, dockPanelSettings.TabSettings);
            }
        }


        private void SetTabAreaStyle(DockablePaneBase dockableControlPane, DockPanelTabSettings tabSettings)
        {
            dockableControlPane.TextTab = tabSettings.TextTab;
            dockableControlPane.Settings.TabAppearance.FontData.Bold = tabSettings.IsBold;
            dockableControlPane.Settings.TabAppearance.ForeColor = tabSettings.ForeColor;
            dockableControlPane.Settings.TabAppearance.BackColor = tabSettings.BackColor;
            dockableControlPane.Settings.TabAppearance.BackColor2 = tabSettings.BackColor2;

            if (tabSettings.Icon != null)
            {
                dockableControlPane.Settings.TabAppearance.Image = tabSettings.Icon;
            }

            dockableControlPane.Settings.TabAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            dockableControlPane.Settings.TabAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
            dockableControlPane.Settings.TabAppearance.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            dockableControlPane.Settings.TabAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            dockableControlPane.Settings.TabAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
        }


        private void SetCaptionAreaStyle(DockablePaneBase dockableControlPane, DockPanelCaptionSettings captionSettings)
        {
            dockableControlPane.Text = string.Concat(" ", dockableControlPane.Text.TrimStart());
            dockableControlPane.Settings.CaptionAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            dockableControlPane.Settings.CaptionAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            dockableControlPane.Settings.CaptionAppearance.BackColor = captionSettings.BackColor;
            dockableControlPane.Settings.CaptionAppearance.ForeColor = captionSettings.ForeColor;
            dockableControlPane.Settings.CaptionAppearance.FontData.Bold = captionSettings.IsBold;

            dockableControlPane.Settings.ActiveCaptionAppearance.BackColor = captionSettings.BackColor;
            dockableControlPane.Settings.ActiveCaptionAppearance.ForeColor = captionSettings.ForeColor;
            dockableControlPane.Settings.ActiveCaptionAppearance.FontData.Bold = captionSettings.IsBold;
        }
    }


    internal class DockManagerSettings
    {
        internal Color BackColor { get; set; }
        internal Color BorderColor { get; set; }

        internal DockManagerSettings()
        {
            BackColor = ColorTranslator.FromHtml("#3d3c3f");
            BorderColor = ColorTranslator.FromHtml("#3d3c3f");
        }
    }


    internal class DockPanelSettings
    {
        internal DockPanelTabSettings TabSettings { get; set; }
        internal DockPanelCaptionSettings CaptionSettings { get; set; }

        internal DockPanelSettings()
        {
            TabSettings = new DockPanelTabSettings();
            CaptionSettings = new DockPanelCaptionSettings();
        }
    }


    internal class DockPanelTabSettings
    {
        internal string TextTab { get; set; }
        internal Infragistics.Win.DefaultableBoolean IsBold { get; set; }
        internal Color ForeColor { get; set; }
        internal Color BackColor { get; set; }
        internal Color BackColor2 { get; set; }
        internal Image Icon { get; set; }
        
        internal DockPanelTabSettings()
        {
            TextTab = "";
            IsBold = Infragistics.Win.DefaultableBoolean.True;
            ForeColor = Color.White;
            BackColor = ColorTranslator.FromHtml("#3d3c3f");
            BackColor2 = ColorTranslator.FromHtml("#3d3c3f");
            Icon = null;
        }
    }


    internal class DockPanelCaptionSettings
    { 
        internal Color BackColor { get; set; }
        internal Color ForeColor { get; set; }
        internal Infragistics.Win.DefaultableBoolean IsBold { get; set; }

        internal DockPanelCaptionSettings()
	    {
            BackColor = System.Drawing.ColorTranslator.FromHtml("#666666");
            ForeColor = Color.White;
            IsBold = Infragistics.Win.DefaultableBoolean.True;
	    }
    }
}
