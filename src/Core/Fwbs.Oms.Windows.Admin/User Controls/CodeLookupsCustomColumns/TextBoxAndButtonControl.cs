using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class TextBoxAndButtonControl : UserControl , IDataGridViewEditingControl
    {
        private bool valueChanged;
        public TextBoxAndButtonControl()
        {
            InitializeComponent();
            
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.textBox1.Font = dataGridViewCellStyle.Font;
            this.textBox1.ForeColor = dataGridViewCellStyle.ForeColor;
            this.textBox1.BackColor = dataGridViewCellStyle.BackColor;
        }

        private System.Windows.Forms.DataGridView _dataGrid;
        public System.Windows.Forms.DataGridView EditingControlDataGridView
        {
            get
            {
                return _dataGrid;
            }
            set
            {
                _dataGrid = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                this.textBox1.Text = Convert.ToString(value);
                textBox1.SelectionStart = this.textBox1.Text.Length;
            }
        }

        private int _editingControlRowIndex;
        public int EditingControlRowIndex
        {
            get
            {
                return _editingControlRowIndex;
            }
            set
            {
                _editingControlRowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged; 
            }
            set
            {
                valueChanged = value;
            }
        }

        public string EditBoxTitle
        {
            get;
            set;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return !dataGridViewWantsInputKey;
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
           
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = ZoomBox.Show(this.ParentForm,EditBoxTitle, textBox1.Text);
      
        }

      
      

        
    }
}
