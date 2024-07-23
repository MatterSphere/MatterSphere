using System.Windows.Forms;

namespace RTA_Demo
{
    internal class DataInitializer
    {

        //  Yes/No Types
        private const string COMBO_NO = "No";
        private const string COMBO_YES = "Yes";

        public void InitializeComboBox_YesNo(ComboBox combobox, bool isYes = false)
        {
            combobox.Items.Add(COMBO_NO);
            combobox.Items.Add(COMBO_YES);
            combobox.SelectedIndex = isYes ? 1 : 0;
        }
    }
}
