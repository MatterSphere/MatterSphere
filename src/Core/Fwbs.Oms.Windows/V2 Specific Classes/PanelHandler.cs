using System.Collections.Generic;

namespace FWBS.OMS.UI.Windows
{
    public static class PanelHandler
    {

        #region methods

        #region SetPanelVisibility
        /// <summary>
        /// Show/Hide the controls associated with TreeView Navigation based on a supplied
        /// boolean variable.
        /// </summary>
        /// <param name="isVisible"></param>
        public static void SetPanelVisibility(bool showPanels,List<IPanelState> panelStates)
        {
            foreach (IPanelState panelState in panelStates)
            {
                if (!showPanels)
                {
                    panelState.Panel.Close();
                }
                else
                {
                    panelState.Panel.Show();

                    if (!panelState.IsPinned)
                    {
                        panelState.Panel.Unpin();
                    }
                }
            }
        }
        #endregion SetPanelVisibility

        #endregion
    }
}
