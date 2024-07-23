namespace Fwbs.Office
{
    partial class OfficeCommandBars 
    {
        public bool GetEnabledMso(string idMso)
        {
            return bars.GetEnabledMso(idMso);
        }
        public stdole.IPictureDisp GetImageMso(string idMso, int Width, int Height)
        {
            return bars.GetImageMso(idMso, Width, Height);
        }

        public string GetLabelMso(string idMso)
        {
            return bars.GetLabelMso(idMso);
        }

        public bool GetPressedMso(string idMso)
        {
            return bars.GetPressedMso(idMso);
        }

        public string GetScreentipMso(string idMso)
        {
            return bars.GetScreentipMso(idMso);
        }

        public string GetSupertipMso(string idMso)
        {
            return bars.GetSupertipMso(idMso);
        }

        public bool GetVisibleMso(string idMso)
        {
            return bars.GetVisibleMso(idMso);
        }

        public void ExecuteMso(string idMso)
        {
            bars.ExecuteMso(idMso);
        }
    }
}
