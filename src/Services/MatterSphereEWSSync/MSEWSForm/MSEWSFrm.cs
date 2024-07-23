using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using MatterSphereEWS;

namespace MSEWSForm
{
    public partial class MSEWSFrm : Form
    {
        public MSEWSFrm()
        {
            InitializeComponent();
            LoadConfigValues();
        }

        private void btnRunProcess_Click(object sender, EventArgs e)
        {
            MatterSphereEWS.MatterSphereEWSFE MSews = new MatterSphereEWS.MatterSphereEWSFE();
            MSews.RunProcess();
            MSews = null;

        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            LoadConfigValues();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfigSettings();
            btnSaveConfig.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AllowControlEditing();
        }

        private void AllowControlEditing()
        {
            btnSaveConfig.Enabled = true;
            foreach (Control c in Controls)
            {
                if (c is GroupBox)
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d is TextBox || d is CheckBox || d is RadioButton || d is DateTimePicker || d is ComboBox)
                        {
                            MakeControlEditable(d);
                        }
                    }
                }
            }
        }

        private void LoadConfigValues()
        {
            btnSaveConfig.Enabled = false;
            btnEditConfig.Enabled = true;
            foreach (Control c in Controls)
            {
                if (c is GroupBox)
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d is TextBox)
                        {
                            LoadControl((TextBox)d);
                        }
                        else if (d is CheckBox)
                        {
                            LoadControlBool((CheckBox)d);
                        }
                        else if (d is RadioButton)
                        {
                            LoadControlBool((RadioButton)d);
                        }
                        else if (d is DateTimePicker)
                        {
                            LoadControlTime((DateTimePicker)d);
                        }
                        else if (d is ComboBox)
                        {
                            LoadControlCombo((ComboBox)d);
                        }
                    }
                }
            }
            if (!rbBasic.Checked && !rbOAuth.Checked)
                rbNtlm.Checked = true;
        }
        private void SaveConfigSettings()
        {
            foreach (Control c in Controls)
            {
                if (c is GroupBox)
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d is TextBox)
                        {
                            SaveControlValue((TextBox)d);
                        }
                        else if (d is CheckBox)
                        {
                            SaveControlValueBool((CheckBox)d);
                        }
                        else if (d is RadioButton)
                        {
                            SaveControlValueBool((RadioButton)d);
                        }
                        else if (d is DateTimePicker)
                        {
                            SaveControlValueTime((DateTimePicker)d);
                        }
                        else if (d is ComboBox)
                        {
                            SaveControlComboxBox((ComboBox)d);
                        }
                    }
                }
            }
        }
        private void LoadControl(TextBox txtBox)
        {
            txtBox.Enabled = false;
            txtBox.Text = Config.GetConfigurationItem(txtBox.Tag.ToString());
        }
        private void LoadControlBool(CheckBox chkBox)
        {
            chkBox.Enabled = false;
            chkBox.Checked = Config.GetConfigurationItemBool(chkBox.Tag.ToString());
        }
        private void LoadControlBool(RadioButton rbBox)
        {
            rbBox.Enabled = false;
            if (rbBox.Tag != null)
                rbBox.Checked = Config.GetConfigurationItemBool(rbBox.Tag.ToString());
        }
        private void LoadControlTime(DateTimePicker dtePicker)
        {
            dtePicker.Enabled = false;
            dtePicker.ShowUpDown = true;
            dtePicker.Value = Convert.ToDateTime(Config.GetConfigurationItem(dtePicker.Tag.ToString()));
        }
        private void LoadControlCombo(ComboBox cmbBox)
        {
            cmbBox.Enabled = false;
            if (cmbBox.Name == "cmbFreeBusy")
            {
                cmbBox.Items.Clear();
                LoadFreeBusyList(cmbBox);
                cmbBox.Text = Config.GetConfigurationItem(cmbBox.Tag.ToString());
            }
            else if (cmbBox.Name == "cmbTimeZone")
            {
                cmbBox.Items.Clear();
                LoadTimeZones(cmbBox);
                cmbBox.Text = Config.GetConfigurationItem(cmbBox.Tag.ToString());
            }
            else if (cmbBox.Name == "cmbLoginType")
            {
                cmbBox.Text = Config.GetConfigurationItem(cmbBox.Tag.ToString());
            }
        }

        private void LoadTimeZones(ComboBox cmbBox)
        {
            ReadOnlyCollection<TimeZoneInfo> tz = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo tzi in tz)
            {
                cmbBox.Items.Add(tzi.Id);
            }
        }
        private void LoadFreeBusyList(ComboBox cmbBox)
        {
            cmbBox.Items.Add("Free");
            cmbBox.Items.Add("Busy");
            cmbBox.Items.Add("NoData");
            cmbBox.Items.Add("OOF");
            cmbBox.Items.Add("Tentative");

        }
        private void MakeControlEditable(Control control)
        {
            control.Enabled = true;
        }
        
        private void SaveControlValue(TextBox txtBox)
        {
            txtBox.Enabled = false;
            Config.SetConfigurationItem(txtBox.Tag.ToString(), txtBox.Text);
        }
        private void SaveControlValueBool(CheckBox chkBox)
        {
            chkBox.Enabled = false;
            Config.SetConfigurationItemBool(chkBox.Tag.ToString(), chkBox.Checked);
        }
        private void SaveControlValueBool(RadioButton rdBox)
        {
            rdBox.Enabled = false;
            if (rdBox.Tag != null)
                Config.SetConfigurationItemBool(rdBox.Tag.ToString(), rdBox.Checked);
        }
        private void SaveControlValueTime(DateTimePicker dtePicker)
        {
            dtePicker.Enabled = false;
            string value = dtePicker.Value.ToString("HH:mm:ss");
            Config.SetConfigurationItem(dtePicker.Tag.ToString(), value);
        }
        private void SaveControlComboxBox(ComboBox cmbBox)
        {
            cmbBox.Enabled = false;
            Config.SetConfigurationItem(cmbBox.Tag.ToString(), cmbBox.Text);
        }

        private void btnRunDeleteProcess_Click(object sender, EventArgs e)
        {
            MatterSphereEWS.MatterSphereDelete MSews = new MatterSphereEWS.MatterSphereDelete();
            MSews.RunProcess();
            MSews = null;
        }

        private void btnCheckCertNumber_Click(object sender, EventArgs e)
        {
            StringBuilder messageBoxString = new StringBuilder();
            try
            {
                using (MatterSphereEWS.CertCheck MSewsCert = new MatterSphereEWS.CertCheck())
                {
                    messageBoxString.AppendLine("Returned Certificate Serial No:");
                    messageBoxString.AppendLine();
                    messageBoxString.AppendLine(MSewsCert.GetCertNumber());
                    messageBoxString.AppendLine();
                    messageBoxString.AppendLine("Use CTRL + C to Copy Message Box Text");
                }
            }
            catch (Exception ex)
            {
                messageBoxString.Append(ex.Message);
            }
            finally
            {
                MessageBox.Show(this, messageBoxString.ToString());
            }
        }

        private void rbAuthType_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls;
            bool visible = ((RadioButton)sender).Checked;
            if (sender == rbBasic)
            {
                controls = new Control[] { lblExcUser, txtExcUser, lblExcPassword, txtExcPassword, lblExcDomain, txtExcDomain };
            }
            else if (sender == rbOAuth)
            {
                controls = new Control[] { lblOAuthApp, txtOAuthApp, lblOAuthClientSecret, txtOAuthClientSecret, lblOAuthTenant, txtOAuthTenant };
            }
            else
            {
                controls = new Control[] { lblExcUser, txtExcUser, lblExcPassword, txtExcPassword, lblExcDomain, txtExcDomain,
                                           lblOAuthApp, txtOAuthApp, lblOAuthClientSecret, txtOAuthClientSecret, lblOAuthTenant, txtOAuthTenant };
                visible = false;
            }

            foreach (Control c in controls)
            {
                c.Visible = visible;
            }
        }
    }
}
