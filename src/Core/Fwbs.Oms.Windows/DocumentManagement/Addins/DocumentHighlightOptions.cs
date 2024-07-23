using System.Drawing;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{
    public class DocumentHighlightOptions
    {
        #region constructors
        public DocumentHighlightOptions()
        {
            highlightVisible = Common.ConvertDef.ToBoolean(FWBS.OMS.Session.CurrentSession.GetXmlProperty("HighlightVisible", true), true);
            highlightHidden = Common.ConvertDef.ToBoolean(FWBS.OMS.Session.CurrentSession.GetXmlProperty("HighlightHidden", false), false);

            int color = Common.ConvertDef.ToInt32((FWBS.OMS.Session.CurrentSession.GetXmlProperty("HiddenColor", Color.White.ToArgb())), Color.White.ToArgb());
            hiddenColor = Color.FromArgb(color);

            color = Common.ConvertDef.ToInt32((FWBS.OMS.Session.CurrentSession.GetXmlProperty("VisibleColor", -6420)), -6420);
            visibleColor = Color.FromArgb(color);
        }
        #endregion

        #region fields
        private bool highlightVisible;
        private bool highlightHidden;

        private Color visibleColor;
        private Color hiddenColor;
        #endregion

        #region properties

        public bool Enabled
        {
            get
            {
                return Session.CurrentSession.IsPackageInstalled("MS_CLI_SECURITY") || Session.CurrentSession.IsPackageInstalled("MatterSphere") || Session.CurrentSession.IsPackageInstalled("MatterSphere®");
            }            
        }

        public bool HighlightVisible
        {
            get
            {
                return highlightVisible;
            }
            set
            {
                highlightVisible = value;
            }
        }

        public bool HighlightHidden
        {
            get
            {
                return highlightHidden;
            }
            set
            {
                highlightHidden = value;
            }
        }

        public Color VisibleColor
        {
            get
            {
                return visibleColor;
            }
            set
            {
                visibleColor = value;
            }
        }

        public Color HiddenColor
        {
            get
            {
                return hiddenColor;
            }
            set
            {
                hiddenColor = value;
            }
        }
        #endregion
    }
}
