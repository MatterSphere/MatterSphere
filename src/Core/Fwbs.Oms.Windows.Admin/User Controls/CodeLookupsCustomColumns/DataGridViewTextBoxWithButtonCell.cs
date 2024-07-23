using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public class DataGridViewTextBoxWithButtonColumn : DataGridViewColumn
    {
        public DataGridViewTextBoxWithButtonColumn()
            : base(new DataGridViewTextBoxWithButtonCell())
        {
        }

        

        public override DataGridViewCell CellTemplate
        {
            get
            {
                var cell = base.CellTemplate;

                if (! (cell is DataGridViewTextBoxWithButtonCell))
                {
                    throw new Exception();
                }

                return cell;

                
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewTextBoxWithButtonCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewTextBoxWithButtonCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class DataGridViewTextBoxWithButtonCell : DataGridViewTextBoxCell
    {
        public DataGridViewTextBoxWithButtonCell():base()
        {
           
        }

        public override bool KeyEntersEditMode(KeyEventArgs e)
        {
            
            var val = CalulateStringValue(e);
            if (!string.IsNullOrWhiteSpace(val))
                this.Value = val;

            return base.KeyEntersEditMode(e);
        }

        private string CalulateStringValue(KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.V))
                return string.Empty;

            //handle standard alphabet
            if (e.KeyValue >= (int)Keys.A && e.KeyValue <= (int)Keys.Z)
            {
                var selectedChar = char.ConvertFromUtf32(e.KeyValue);
                string str = selectedChar.ToString();

                if (!e.Shift)
                    return str.ToLowerInvariant();

                return str;
            }
            else if (e.KeyValue >= (int)Keys.D0 && e.KeyValue <= (int)Keys.D9)
            {
                if (e.Shift)
                {
                    return string.Empty;
                }
                else
                {
                    var selectedChar = char.ConvertFromUtf32(e.KeyValue);
                    return selectedChar.ToString();
                }

               
            }

            return string.Empty;

        }
       
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            var ctl = DataGridView.EditingControl as TextBoxAndButtonControl;
           
            ctl.EditingControlFormattedValue = this.Value;
            ctl.EditBoxTitle = this.OwningColumn.HeaderText;
        }

        

        private static Type _editType = typeof(TextBoxAndButtonControl);
        public override Type EditType
        {
            get
            {
                return _editType;
            }
        }
    }
}
