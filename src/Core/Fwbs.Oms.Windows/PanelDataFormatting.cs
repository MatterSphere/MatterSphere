using System;
using System.Collections.Generic;
using System.Drawing;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI
{
    public interface IPanelDataFormatting
    {
        void Set(ucNavRichText ucNavRichText1, Dictionary<string, string> documentDetailsData);
    }


    public class Version2DocumentDetails : IPanelDataFormatting
    {
        public void Set(ucNavRichText ucNavRichText1, Dictionary<string, string> documentDetailsData)
        {
            foreach (var documentDetail in documentDetailsData)
            {
                AddDocumentDetailsHeaderItem(ucNavRichText1, documentDetail.Key);
                AddDocumentDetailsData(ucNavRichText1, documentDetail.Value);
            }
        }


        private void AddDocumentDetailsHeaderItem(ucNavRichText ucNavRichText1, string header)
        {
            ucNavRichText1.ControlRich.SelectionColor = Color.FromArgb(1, 1, 1);
            ucNavRichText1.ControlRich.AppendText(header + Environment.NewLine);
        }


        private void AddDocumentDetailsData(ucNavRichText ucNavRichText1, string value)
        {
            ucNavRichText1.ControlRich.SelectionColor = Color.FromArgb(102, 102, 102);
            ucNavRichText1.ControlRich.AppendText(value + Environment.NewLine + Environment.NewLine);
        }
    }
}
