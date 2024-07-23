using System.Drawing;

namespace FWBS.OMS.UI.Windows
{
    internal static class AlertIconSelector
    {
        internal static Image GetAlertIcon(Alert alert)
        {
            switch (alert.Status)
            {
                case Alert.AlertStatus.Red:
                    return FWBS.OMS.UI.Properties.Resources.Alert_Red;

                case Alert.AlertStatus.Amber:
                    return FWBS.OMS.UI.Properties.Resources.Alert_Amber;

                case Alert.AlertStatus.Green:
                    return FWBS.OMS.UI.Properties.Resources.Alert_Green;

                default:
                    goto case Alert.AlertStatus.Red;
            }
        }
    }
}
