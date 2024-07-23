using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.FileManagement.UI
{
    internal partial class Assignment : FWBS.OMS.UI.Windows.BaseForm
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private const int CS_DROPSHADOW = 0x20000;
        private DataView vwusers;

        public Assignment()
        {
            InitializeComponent();
            SetIcon();
            SetCueText();
        }

        private void Assignment_Load(object sender, EventArgs e)
        {
            DataView vwteams = new DataView(Team.GetTeams());
            vwteams.Sort = "tmname";
            DataRow drw = vwteams.Table.NewRow();
            drw["tmid"] = DBNull.Value;
            drw["tmname"] = Session.CurrentSession.Resources.GetResource("NOTASSIGNED", "(Not Assigned)", "").Text;
            vwteams.Table.Rows.InsertAt(drw, 0);
            FWBS.OMS.EnquiryEngine.DataLists users = new FWBS.OMS.EnquiryEngine.DataLists("DSUSERACT");
            DataTable dt = (DataTable)users.Run();
            DataView vw = new DataView(dt);
            vw.RowFilter = "usrid is null";
            foreach (DataRowView r in vw)
            {
                r.Delete();
            }

            vwusers = new DataView(dt);
            vwusers.Sort = "usrfullname";

            drw = vwusers.Table.NewRow();
            drw["usrid"] = DBNull.Value;
            drw["usrfullname"] = Session.CurrentSession.Resources.GetResource("NOTASSIGNED", "(Not Assigned)", "").Text;
            vwusers.Table.Rows.InsertAt(drw, 0);
            for (int ctr = vwusers.Table.Columns.Count - 1; ctr >= 0; ctr--)
            {
                DataColumn col = vwusers.Table.Columns[ctr];
                var columnName = col.ColumnName.ToLower();
                if (columnName != "usrid" && columnName != "usrfullname")
                {
                    vwusers.Table.Columns.Remove(col);
                }
            }
            cboTeam.DataSource = vwteams;
            cboTeam.ValueMember = vwteams.Table.Columns["tmid"].ColumnName;
            cboTeam.DisplayMember = vwteams.Table.Columns["tmname"].ColumnName;
            cboUser.DataSource = vwusers;
            cboUser.ValueMember = vwusers.Table.Columns["usrid"].ColumnName;
            cboUser.DisplayMember = vwusers.Table.Columns["usrfullname"].ColumnName;
        }

        private int? GetVal(Common.UI.Windows.eXPComboBox cbo)
        {
            DataRowView r = cbo.SelectedItem as DataRowView;
            if (r == null)
                return null;
            else
            {
                if (r[cbo.ValueMember] == DBNull.Value)
                    return null;
                else
                    return (int?)r[cbo.ValueMember];
            }
        }

        private void SetIcon()
        {
            pboxIcon.Image = OMS.UI.Windows.Images.GetNavigationIcons(DeviceDpi).Images[4];
        }

        private void SetCueText()
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            cboTeam.CueText = new CodeLookupDisplay("CHOOSETEAM", "ENQQUESTCUETEXT", "Choose Team", $"{currentCulture}", string.Empty);
            cboUser.CueText = new CodeLookupDisplay("CHOOSE2", "ENQQUESTCUETEXT", "Choose", $"{currentCulture}", string.Empty);
        }

        public int? TeamId
        {
            get
            {
                return GetVal(cboTeam);
            }

        }

        public int? AssignedTo
        {
            get
            {
                return GetVal(cboUser);
            }
        }

        private void PopulateUsers()
        {
            int? tmid = TeamId;
            DataView vw;
            if (tmid.HasValue)
            {
                vw = new DataView(Team.GetTeamMembers(tmid.Value));
                vw.Sort = "usrfullname";
                //How bizzare.  The following two lines were added to stop a null validation error
                //The combobox internally tries to add rows but encounters the constraints.
                vw.Table.Columns["tmid"].AllowDBNull = true;
                vw.Table.Columns["usrid"].AllowDBNull = true;
                DataRow drw = vw.Table.NewRow();
                drw["usrid"] = DBNull.Value;
                drw["usrfullname"] = Session.CurrentSession.Resources.GetResource("NOTASSIGNED", "(Not Assigned)", "").Text;
                vw.Table.Rows.InsertAt(drw, 0);
            }
            else
            {
                vw = vwusers;
            }
            cboUser.DataSource = vw;
            cboUser.ValueMember = vw.Table.Columns["usrid"].ColumnName;
            cboUser.DisplayMember = vw.Table.Columns["usrfullname"].ColumnName;
        }

        private void cboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUser.Items.Count > 0)
            {
                PopulateUsers();
                cboUser.SelectedIndex = -1;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            SetIcon();
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pnlButtons_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(216, 216, 216), pnlButtons.Padding.Top))
            {
                e.Graphics.DrawLine(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right, e.ClipRectangle.Top);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}