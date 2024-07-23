using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.UI.Elasticsearch
{
    partial class FilterTypesPopup : UserControl
    {
        private const string DateTimeFormat = @"yyyy-MM-ddTHH\:mm\:ss.fff"; // Corresponds to MS-SQL format 126
        private readonly int _minimumHeight;
        private bool _isDirty;
        private DateTimeNULL _documentStartDate = DBNull.Value;
        private DateTimeNULL _documentEndDate = DBNull.Value;

        public FilterTypesPopup()
        {
            InitializeComponent();
            _minimumHeight = CalcMinimumHeight();
            HideExcludedEntities();

            dtpStartDate.MaxDate = dtpEndDate.MaxDate = DateTime.Today.AddDays(1).AddMilliseconds(-1);
        }

        public bool SearchAllowed { get; set; }

        public IEnumerable<EntityTypeEnum> GetSelectedTypes()
        {
            var types = new List<EntityTypeEnum>();
            foreach (CheckBox control in pnlEntities.Controls)
            {
                if (control.Checked)
                    types.Add((EntityTypeEnum)control.Tag);
            }
            return types;
        }

        public Tuple<string, string> GetDocumentsDateRange()
        {
            string docStartDate = null, docEndDate = null;
            if (pnlDocDates.Enabled)
            {
                docStartDate = _documentStartDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
                docEndDate = _documentEndDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
            }
            return new Tuple<string, string>(docStartDate, docEndDate);
        }

        public void LoadSettings()
        {
            Favourites fw = new Favourites("GLOBALSEARCH", "ENTITIES");
            if (fw.Count > 0)
            {
                string[] values = fw.Param1(0).Split('|');
                foreach (string entity in values)
                {
                    EntityTypeEnum tag = EntityType.Convert(entity);
                    CheckBox control = FindEntityCheckBox(tag);
                    if (control != null && control.Visible)
                        control.Checked = true;
                }

                if (fw.Param2(0) == "DOCFILTERING")
                {
                    values = fw.Param3(0).Split('|');
                    if (values.Length == 2)
                    {
                        _documentStartDate = DateTimeNULL.Parse(values[0], CultureInfo.InvariantCulture);
                        if (!_documentStartDate.IsNull)
                            dtpStartDate.Value = _documentStartDate;

                        _documentEndDate = DateTimeNULL.Parse(values[1], CultureInfo.InvariantCulture);
                        if (!_documentEndDate.IsNull)
                            dtpEndDate.Value = _documentEndDate;
                    }
                }
            }
            _isDirty = false;
        }

        public void SaveSettings()
        {
            if (!_isDirty)
                return;

            string param1 = string.Join("|", GetSelectedTypes());
            string param2 = "DOCFILTERING";
            string param3 = string.Concat(_documentStartDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture), "|", _documentEndDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
            if (param3.Length == 1)
                param2 = param3 = null;

            Favourites fw = new Favourites("GLOBALSEARCH", "ENTITIES");
            if (fw.Count == 0)
            {
                fw.AddFavourite("ENTITIES", null, param1, param2, param3);
            }
            else
            {
                fw.Param1(0, param1);
                fw.Param2(0, param2);
                fw.Param3(0, param3);
                fw.Update();
            }
            _isDirty = false;
        }

        #region Private Methods

        private int CalcMinimumHeight()
        {
            int height = pnlDocDates.Top + dtpEndDate.Bottom;
            using (MonthCalendar calendar = new MonthCalendar())
            {   // Force handle creation to obtain the real size including scaling
                if (calendar.Handle != IntPtr.Zero)
                    height += calendar.GetPreferredSize(Size.Empty).Height;
            }
            return height;
        }

        private void HideExcludedEntities()
        {
            var dbProviderFactory = new DbProviderFactory();
            FilterTypeFactory filterTypeFactory = new FilterTypeFactory(dbProviderFactory.CreateDbProvider());

            foreach (EntityTypeEnum hiddenEntity in filterTypeFactory.LoadHiddenEntities())
            {
                CheckBox control = FindEntityCheckBox(hiddenEntity);
                if (control != null)
                {
                    lblDivider.Top = System.Math.Max(lblDivider.Top - control.Height, _minimumHeight - lblDivider.Height);
                    control.Visible = false;
                }
            }
        }

        private CheckBox FindEntityCheckBox(EntityTypeEnum tag)
        {
            foreach (CheckBox control in pnlEntities.Controls)
            {
                if (tag.Equals(control.Tag))
                    return control;
            }
            return null;
        }

        private void CheckAllEntities(bool check)
        {
            foreach (CheckBox control in pnlEntities.Controls)
            {
                if (control.Visible)
                    control.Checked = check;
            }
        }

        private bool IsAnyEntityChecked()
        {
            foreach (CheckBox control in pnlEntities.Controls)
            {
                if (control.Checked)
                    return true;
            }
            return false;
        }

        #endregion Private Methods

        #region Events

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.F4))
            {
                (Parent as ToolStripDropDown)?.Close(ToolStripDropDownCloseReason.Keyboard);
                return true;
            }

            if (keyData == Keys.Enter && (this.ActiveControl is Button))
            {
                ((Button)this.ActiveControl).PerformClick();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                btnSearch.Enabled = SearchAllowed && IsAnyEntityChecked();
                dtpStartDate.Checked = !_documentStartDate.IsNull;
                dtpEndDate.Checked = !_documentEndDate.IsNull;
            }
            base.OnVisibleChanged(e);
        }

        private void chbEntity_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == chbDocument || sender == chbEmail)
                pnlDocDates.Enabled = chbDocument.Checked || chbEmail.Checked;

            btnSearch.Enabled = SearchAllowed && (((CheckBox)sender).Checked || IsAnyEntityChecked());
            _isDirty = true;
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpStartDate.Checked)
                _documentStartDate = DateTime.SpecifyKind(dtpStartDate.Value.Date, DateTimeKind.Local);
            else
                _documentStartDate = DBNull.Value;

            _isDirty = true;
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpEndDate.Checked)
                _documentEndDate = DateTime.SpecifyKind(dtpEndDate.Value.Date.AddDays(1).AddMilliseconds(-1), DateTimeKind.Local);
            else
                _documentEndDate = DBNull.Value;

            _isDirty = true;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            CheckAllEntities(true);
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            CheckAllEntities(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            (Parent as ToolStripDropDown)?.Close(ToolStripDropDownCloseReason.CloseCalled);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            (Parent as ToolStripDropDown)?.Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        #endregion Events
    }
}
