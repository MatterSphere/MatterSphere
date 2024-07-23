using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class PermissionItem : UserControl
    {
        public event EventHandler NextPermission;
        public event EventHandler PreviousPermission;
        public event EventHandler AllowOrDenyCheckBoxChanged;
        
        public PermissionItem()
        {
            InitializeComponent();
        }

        public PermissionItem(DataRowView Row, string DescriptionColumnName, string AllowColumnName, string DenyColumnName, string GroupColumnName, string HelpColumnName, string MajorGroupColumnName)
        {
            InitializeComponent();
            _descriptionColumnName = DescriptionColumnName;
            _majorgroupColumnName = MajorGroupColumnName;
            _allowColumnName = AllowColumnName;
            _denyColumnName = DenyColumnName;
            _groupColumnName = GroupColumnName;
            _helpColumnName = HelpColumnName;
            _datarow = Row;
        }

        private DataRowView _datarow;
        private string _descriptionColumnName;
        private string _allowColumnName;
        private string _groupColumnName;
        private string _helpColumnName;
        private string _denyColumnName;
        private string _majorgroupColumnName;
        private bool _selected = false;
        
        public DataRowView DataRowView
        {
            get
            {
                return _datarow;
            }
            set
            {
                if (_datarow != null)
                    _datarow.Row.Table.RowChanged -= new DataRowChangeEventHandler(Table_RowChanged);
                _datarow = value;
                SuspendEvents();
                PermissionItem_ParentChanged(this, EventArgs.Empty);
                ResumeEvents();
            }
        }

        private void Table_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Row == _datarow.Row)
            {
                try
                {
                    SuspendEvents();
                    PermissionItem_ParentChanged(this, EventArgs.Empty);
                    ResumeEvents();
                }
                catch { }
            }
        }

        public string Description
        {
            get
            {
                return  FWBS.OMS.Session.CurrentSession.Terminology.Parse(Convert.ToString(_datarow[_descriptionColumnName]),false);
            }

        }

        public bool Allow
        {
            get
            {
                return ConvertDef.ToBoolean(_datarow[_allowColumnName],false);
            }
            set
            {
                _datarow[_allowColumnName] = value;
            }
        }

        public int DescriptionWidth
        {
            get
            {
                return labPermission.Width;
            }
        }

        public bool Deny
        {
            get
            {
                return ConvertDef.ToBoolean(_datarow[_denyColumnName],false);
            }
            set
            {
                _datarow[_denyColumnName] = value;
            }
        }

        public string Help
        {
            get
            {
                if (_helpColumnName != "")
                    return "";
                else
                    return Convert.ToString(_datarow[_helpColumnName]);
            }
            set
            {
                if (_helpColumnName != "")
                    _datarow[_helpColumnName] = value;
            }
        }

        public string Group
        {
            get
            {
                return Convert.ToString(_datarow[_groupColumnName]);
            }
            set
            {
                _datarow[_groupColumnName] = value;
            }
        }

        public bool MajorGroup
        {
            get
            {
                return ConvertDef.ToBoolean(_datarow[_majorgroupColumnName],false);
            }
            set
            {
                _datarow[_majorgroupColumnName] = value;
            }
        }
        
        public bool IsSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (value)
                {
                    this.BackColor = SystemColors.ActiveCaption;
                    this.ForeColor = SystemColors.ActiveCaptionText;
                }
                else
                {
                    this.BackColor = SystemColors.Window;
                    this.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void PermissionItem_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                SuspendEvents();
                labPermission.Text = this.Description;
                chkAllow.Checked = this.Allow;
                chkDeny.Checked = this.Deny;
                if (this.MajorGroup) labPermission.Font = new Font(labPermission.Font.FontFamily, labPermission.Font.Size, FontStyle.Bold);
                ResumeEvents();
            }
        }

        private void chkAllow_CheckedChanged(object sender, EventArgs e)
        {
            SuspendEvents();
            try
            {
                if (Parent != null && Convert.ToString(Parent.Tag) == "OFF")
                {
                    chkAllow.Checked = !chkAllow.Checked;
                    return;
                }                
                chkDeny.Checked = false;
                _datarow[_allowColumnName] = chkAllow.Checked;
                _datarow[_denyColumnName] = chkDeny.Checked;
                this.OnClick(e);
                OnAllowOrDenyCheckBoxChanged();
            }
            finally
            {
                ResumeEvents();
            }
        }

        private void chkDeny_CheckedChanged(object sender, EventArgs e)
        {
            SuspendEvents();
            try
            {
                if (Parent != null && Convert.ToString(Parent.Tag) == "OFF")
                {
                    chkDeny.Checked = !chkDeny.Checked;
                    return;
                }
                chkAllow.Checked = false;
                _datarow[_allowColumnName] = chkAllow.Checked;
                _datarow[_denyColumnName] = chkDeny.Checked;
                this.OnClick(e);
                OnAllowOrDenyCheckBoxChanged();
            }
            finally
            {
                ResumeEvents();
            }
        }

        private void OnAllowOrDenyCheckBoxChanged()
        {
            if (AllowOrDenyCheckBoxChanged != null)
            {
                AllowOrDenyCheckBoxChanged(this, EventArgs.Empty);
            }
        }

        private void labPermission_Click(object sender, EventArgs e)
        {
            if (Parent != null && Convert.ToString(Parent.Tag) == "OFF") return;
            txtFocus.Focus();
            this.OnClick(e);
        }

        private void SuspendEvents()
        {
            if (_datarow != null)
                _datarow.Row.Table.RowChanged -= new DataRowChangeEventHandler(Table_RowChanged);
            this.chkDeny.CheckedChanged -= new System.EventHandler(this.chkDeny_CheckedChanged);
            this.chkAllow.CheckedChanged -= new System.EventHandler(this.chkAllow_CheckedChanged);
        }

        private void ResumeEvents()
        {
            if (_datarow != null)
            {
                _datarow.Row.Table.RowChanged -= new DataRowChangeEventHandler(Table_RowChanged);
                _datarow.Row.Table.RowChanged += new DataRowChangeEventHandler(Table_RowChanged);
            }
            this.chkDeny.CheckedChanged -= new System.EventHandler(this.chkDeny_CheckedChanged);
            this.chkAllow.CheckedChanged -= new System.EventHandler(this.chkAllow_CheckedChanged);
            this.chkDeny.CheckedChanged += new System.EventHandler(this.chkDeny_CheckedChanged);
            this.chkAllow.CheckedChanged += new System.EventHandler(this.chkAllow_CheckedChanged);
        }

        private void PermissionItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                if (PreviousPermission != null) PreviousPermission(this, EventArgs.Empty);
            if (e.KeyCode == Keys.Down)
                if (NextPermission != null) NextPermission(this, EventArgs.Empty); 
            if (e.KeyCode == Keys.Left)
                chkAllow.Checked = !chkAllow.Checked;
            if (e.KeyCode == Keys.Right)
                chkDeny.Checked = !chkDeny.Checked;
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                e.Handled = true;
        }

        public bool AllowOrDenyChecked
        {
            get
            {
                return chkAllow.Checked || chkDeny.Checked ? true : false;
            }
        }

        public Color DescriptionColour
        {
            set
            {
                labPermission.ForeColor = value;
            }
        }
    }
}