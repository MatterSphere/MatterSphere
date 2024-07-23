using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace FWBS.WF.OMS.ActivityLibrary
{
    /// <summary>
    /// Interaction logic for ParametersEditor.xaml
    /// </summary>
    public partial class ParametersEditor : Window
    {
        public ParametersEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// menuitemAdd Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuitemAdd_Click(object sender, RoutedEventArgs e)
        {
            KeyParameters kp = new KeyParameters();
            IList parameters = null;

            if (lbParameters.Items.Count > 0)
            {
                parameters = (IList)lbParameters.ItemsSource;
            }

            if (parameters == null)
            {
                parameters = new ObservableCollection<KeyParameters>();
                parameters.Add(kp);
                lbParameters.ItemsSource = parameters;
            }
            else
            {
                parameters.Add(kp);
            }
        }

        /// <summary>
        /// menuitemDelete Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuitemDelete_Click(object sender, RoutedEventArgs e)
        {
            IList parameters = null;

            if (lbParameters.Items.Count > 0)
            {
                parameters = (IList)lbParameters.ItemsSource;

                if (lbParameters.SelectedItem != null)
                {
                    int index = lbParameters.Items.IndexOf(lbParameters.SelectedItem);

                    if (index != -1)
                    {
                        if (parameters != null)
                        {
                            parameters.RemoveAt(index);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// menuitemSave Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuitemSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// menuitemCancel Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuitemCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
