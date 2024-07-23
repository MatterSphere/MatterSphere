using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.MatterSphereIntegration.Gateway;

namespace FWBS.MatterSphereIntegration
{
    public partial class ConfigureAccountDetail : Form
    {
        public ConfigureAccountDetail()
        {
            InitializeComponent();
            this.Load += new EventHandler(ConfigureAccountDetail_Load);
        }
        private class KeyValue
        {
            public KeyValue(string key, string value)
            {
                Key = key;
                Value = value;
            }
            public string Key { get; set; }
            public string Value { get; set; }
        }
        void ConfigureAccountDetail_Load(object sender, EventArgs e)
        {
            List<KeyValue> fieldTypes = new List<KeyValue>();
            fieldTypes.Add(new KeyValue("String", "String"));
            fieldTypes.Add(new KeyValue("Telephone", "Phone"));
            fieldTypes.Add(new KeyValue("Email", "Email"));
            fieldTypes.Add(new KeyValue("Address", "Address"));
            fieldTypes.Add(new KeyValue("Heading", "BREAK"));
            fieldTypes.Add(new KeyValue("Custom", string.Empty));
            this.cbHidden.Checked = true;
            this.comboBox1.DataSource = fieldTypes;
            this.comboBox1.DisplayMember = "Key";
            this.comboBox1.ValueMember = "Value";
            this.comboBox1.SelectedValueChanged += new EventHandler(comboBox1_SelectedValueChanged);
            copy = null;
            if (Data == null)
            {
                this.Data = new EntityDetail();
                this.tbType.Text = "String";
            }
            else
            {

                copy = new EntityDetail();
                copy.Display = Data.Display;
                copy.ExtensionData = Data.ExtensionData;
                copy.Name = Data.Name;
                copy.Type = Data.Type;
                copy.Value = Data.Value;

                this.tbName.Text = this.Data.Name;
                this.tbType.Text = this.Data.Type;
                this.tbValue.Text = this.Data.Value;
                this.tbLink.Text = this.Data.Link;
                this.cbHidden.Checked = this.Data.Display;

                

            }

            string type = string.Empty;
            foreach (var field in fieldTypes)
            {
                if (field.Value == this.tbType.Text)
                {
                    type = field.Value;
                    break;
                }
            }

            
            
            comboBox1.SelectedValue = type;
            if(string.IsNullOrWhiteSpace(type))
                this.tbType.Text = this.Data.Type;
        }

        void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedValue = (string)this.comboBox1.SelectedValue;
            this.tbType.Text = selectedValue;

            if (selectedValue == "BREAK")
            {
                this.tbName.Enabled = false;
                this.tbName.Text = "BREAK";
            }
            else
                this.tbName.Enabled = true;
                
            this.tbType.Visible = string.IsNullOrEmpty(selectedValue);
        

        }

        private EntityDetail copy;
        public EntityDetail Data
        {
            get;
            set;
        }
        


        private void button1_Click(object sender, EventArgs e)
        {
            this.Data.Name = this.tbName.Text;
            this.Data.Type = this.tbType.Text;
            this.Data.Value = this.tbValue.Text ;
            this.Data.Display = this.cbHidden.Checked;
            this.Data.Link = this.tbLink.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Data = copy;
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
