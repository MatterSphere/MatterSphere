using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using FWBS.OMS;
using System.Activities.Presentation.Converters;

namespace FWBS.WF.OMS.ActivityLibrary
{
    // Interaction logic for CreateDefaultPrecedentJobDesigner.xaml
    internal sealed partial class CreateDefaultPrecedentJobDesigner
    {
        #region Constants
        private const string HOST_PROPERTY_NAME_TYPE = "Type";
        #endregion

        #region Properties
        public string InTooltip { get; set; }
        public string InTooltip2 { get; set; }
        public string InTooltip3 { get; set; }
        public string OutTooltip { get; set; }
        #endregion

        public CreateDefaultPrecedentJobDesigner()
        {
            InitializeComponent();
            BuildPrecedentTypeTable();
        }

        /// <summary>
        /// Build Precedent (Default) Type Table
        /// </summary>
        private void BuildPrecedentTypeTable()
        {
            DataTable table = Precedent.GetAllPrecedents();

            foreach (DataRow row in table.Rows)
            {
                if (row["PrecTitle"].ToString().ToUpper().Equals("DEFAULT"))
                {
                    string str = row["PrecType"].ToString();
                    cmbPrecType.Items.Add(str);
                }
            }
        }

        /// <summary>
        /// btnSearchPrecedent Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchPrecedent_Click(object sender, RoutedEventArgs e)
        {
            FWBS.OMS.Precedent precedent = FWBS.OMS.UI.Windows.Services.Searches.FindPrecedent();

            if (precedent != null)
            {
                System.Activities.InArgument<string> litStr = new System.Activities.InArgument<string>();
                litStr.Expression = precedent.PrecedentType;
                this.ModelItem.Properties[HOST_PROPERTY_NAME_TYPE].ComputedValue = litStr;
            }
        }

        /// <summary>
        /// cmbPrecType Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPrecType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPrecType.SelectedItem != null)
            {
                System.Activities.InArgument<string> type = new System.Activities.InArgument<string>();
                type.Expression = Convert.ToString(cmbPrecType.SelectedItem);
                this.ModelItem.Properties[HOST_PROPERTY_NAME_TYPE].SetValue(type);
                cmbPrecType.SelectedItem = null;
            }
        }

        private void CreateDefaultPrecedentDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // set tooltips
                this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCDPJBDTT", "Create a new Default Precedent Job Activity", "").Text;
                this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCDPJINTT", "A required Precedent Type as input", "").Text;
                this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCDPJINTT2", "A required Associate as input", "").Text;
                this.InTooltip3 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCDPJINTT3", "Select a Precedent Type or manually enter a name in the 'Type' field", "").Text;
                this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCDPJOTTT", "A Precedent Job output result", "").Text;
            }
            catch (Exception)
            {
                // error
                ;
            }
        }      
    }
}
