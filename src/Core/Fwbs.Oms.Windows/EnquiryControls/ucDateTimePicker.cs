using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Crownwood.Magic.Win32;
using FWBS.Common;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows;
using Math = System.Math;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucDateTimePicker.
    /// </summary>
    [Designer(typeof(DateTimePickerDesigner))]
    public class ucDateTimePicker : System.Windows.Forms.UserControl, IBasicEnquiryControl2, ISupportRightToLeft, IOverrideTooltip
    {
        protected const string CUETEXT_CODELOOKUPGROUPNAME = "ENQQUESTCUETXT";

        protected class ColoredDisabledLabel : Label
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                if (!Enabled && Parent != null && Parent.Enabled && ((ucDateTimePicker)Parent).omsDesignMode)
                {
                    Rectangle face = DeflateRect(ClientRectangle, Padding);

                    // Based on source code this way correct for UseCompatibleTextRendering = false
                    var methodInfo = typeof(Label).GetMethod("CreateTextFormatFlags", BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, Type.EmptyTypes, null);

                    if (methodInfo != null)
                    {
                        TextFormatFlags flags = (TextFormatFlags)methodInfo.Invoke(this, null);
                        TextRenderer.DrawText(e.Graphics, Text, Font, face, ForeColor, flags);
                    }
                    else
                    {
                        base.OnPaint(e);
                    }
                }
                else
                {
                    base.OnPaint(e);
                }
            }

            private Rectangle DeflateRect(Rectangle rect, Padding padding)
            {
                rect.X += padding.Left;
                rect.Y += padding.Top;
                rect.Width -= padding.Horizontal;
                rect.Height -= padding.Vertical;
                return rect;
            }
        }

        #region Events
        [Category("Action")]
        public event EventHandler Changed;

        [Category("Action")]
        public event EventHandler ActiveChanged;

        [Category("Layout")]
        public event EventHandler DateTimePickerLayoutChanged = null;

        public event EventHandler<CancelEventArgs> TimeValidating;

        public void OnDateTimePickerLayoutChanged()
        {
            if (DateTimePickerLayoutChanged != null)
                DateTimePickerLayoutChanged(this,EventArgs.Empty);
        }

        protected virtual void OnTimeValidating(object sender, CancelEventArgs e)
        {
            TimeValidating?.Invoke(sender, e);
        }

        #endregion

        private enum DateTimeMask
        {
            Date,
            Time
        }

        #region Controls
        internal FWBS.Common.UI.Windows.MaskedEdit cmbTime;
        private System.Windows.Forms.Label labTime;
        protected internal System.Windows.Forms.DateTimePicker dpDate;
        private System.Windows.Forms.ContextMenu mnuDateMagic;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem mnuThisWeekend;
        private System.Windows.Forms.MenuItem mnuNextWeekend;
        private System.Windows.Forms.MenuItem mnuNextWorkingDay;
        private System.Windows.Forms.MenuItem mnuFirstofthisMonth;
        private System.Windows.Forms.MenuItem mnuEndofthisMonth;
        private System.Windows.Forms.MenuItem mnuAWeekOn;
        private System.Windows.Forms.MenuItem mnuMondayWO;
        private System.Windows.Forms.MenuItem mnuTuesdayWO;
        private System.Windows.Forms.MenuItem mnuWednesdayWO;
        private System.Windows.Forms.MenuItem mnuThursdayWO;
        private System.Windows.Forms.MenuItem mnuFridayWO;
        private System.Windows.Forms.MenuItem mnuSaturdayWO;
        private System.Windows.Forms.MenuItem mnuSundayWO;
        private System.Windows.Forms.MenuItem mnuThisComming;
        private System.Windows.Forms.MenuItem mnuMondayTC;
        private System.Windows.Forms.MenuItem mnuTuesdayTC;
        private System.Windows.Forms.MenuItem mnuWednesdayTC;
        private System.Windows.Forms.MenuItem mnuThursdayTC;
        private System.Windows.Forms.MenuItem mnuFridayTC;
        private System.Windows.Forms.MenuItem mnuSaturdayTC;
        private System.Windows.Forms.MenuItem mnuSundayTC;
        protected FWBS.Common.UI.Windows.MaskedEdit txtDate;
        private System.Windows.Forms.MenuItem mnuToday;
        private IContainer components = null;
        #endregion

        #region Fields
        internal protected bool disableAutoSize = false;
        private bool _isdirty = false;
        private bool _greaterthantoday = false;
        private bool _lessthantoday = false;
        private bool _optionsvisible = false;
        protected ColoredDisabledLabel pnlCaptionContainer;
        private string rtlResetFormat;
        private CultureInfo cultureInfo;
        private string _inputDateMask;
        private CodeLookupDisplay _cueText;
        private string _cueTextCode;
        #endregion
        
        #region Constructors
        public ucDateTimePicker()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            cultureInfo = Session.CurrentSession.DefaultCultureInfo;

            if (Session.CurrentSession.IsLoggedIn)
                SetupMenuCulture();

            _timsep = cultureInfo.DateTimeFormat.TimeSeparator;
            _inputDateMask = GenerateInputMask(DateTimeMask.Date);
            txtDate.InputMask = _inputDateMask;
            cmbTime.InputMask = GenerateInputMask(DateTimeMask.Time);
            rtlResetFormat = txtDate.Text;
            dpDate.CustomFormat = " ";

            AdjustTxtDateSize();
        }

        private void SetupMenuCulture()
        {
            Res resources = Session.CurrentSession.Resources;
            this.mnuToday.Text = resources.GetResource("TODAY", "Today", "").Text;
            this.mnuThisWeekend.Text = resources.GetResource("THISWKND", "This Weekend", "").Text;
            this.mnuNextWeekend.Text = resources.GetResource("NEXTWKND", "Next Weekend", "").Text;
            this.mnuNextWorkingDay.Text = resources.GetResource("NEXTWKDY", "Next Working Day", "").Text;
            this.mnuFirstofthisMonth.Text = resources.GetResource("FSTMONTH", "First of this Month", "").Text;
            this.mnuEndofthisMonth.Text = resources.GetResource("ENDMONTH", "End of this Month", "").Text;
            this.mnuThisComming.Text = resources.GetResource("THISCOMING", "This Coming", "").Text;
            if (this.mnuAWeekOn.Visible)
            {
                this.mnuAWeekOn.Text = resources.GetResource("AWEEKON", "A Week On", "").Text;
                this.mnuMondayWO.Text = resources.GetResource("MONDAY", "Monday", "").Text;
                this.mnuTuesdayWO.Text = resources.GetResource("TUESDAY", "Tuesday", "").Text;
                this.mnuWednesdayWO.Text = resources.GetResource("WEDNESDAY", "Wednesday", "").Text;
                this.mnuThursdayWO.Text = resources.GetResource("THURSDAY", "Thursday", "").Text;
                this.mnuFridayWO.Text = resources.GetResource("FRIDAY", "Friday", "").Text;
                this.mnuSaturdayWO.Text = resources.GetResource("SATURDAY", "Saturday", "").Text;
                this.mnuSundayWO.Text = resources.GetResource("SUNDAY", "Sunday", "").Text;
                this.mnuMondayTC.Text = resources.GetResource("MONDAY", "Monday", "").Text;
                this.mnuTuesdayTC.Text = resources.GetResource("TUESDAY", "Tuesday", "").Text;
                this.mnuWednesdayTC.Text = resources.GetResource("WEDNESDAY", "Wednesday", "").Text;
                this.mnuThursdayTC.Text = resources.GetResource("THURSDAY", "Thursday", "").Text;
                this.mnuFridayTC.Text = resources.GetResource("FRIDAY", "Friday", "").Text;
                this.mnuSaturdayTC.Text = resources.GetResource("SATURDAY", "Saturday", "").Text;
                this.mnuSundayTC.Text = resources.GetResource("SUNDAY", "Sunday", "").Text;
                this.labTime.Text = resources.GetResource("TIME", "Time : ", "").Text;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labTime = new System.Windows.Forms.Label();
            this.dpDate = new System.Windows.Forms.DateTimePicker();
            this.txtDate = new FWBS.Common.UI.Windows.MaskedEdit();
            this.mnuDateMagic = new System.Windows.Forms.ContextMenu();
            this.mnuToday = new System.Windows.Forms.MenuItem();
            this.mnuThisWeekend = new System.Windows.Forms.MenuItem();
            this.mnuNextWeekend = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuNextWorkingDay = new System.Windows.Forms.MenuItem();
            this.mnuFirstofthisMonth = new System.Windows.Forms.MenuItem();
            this.mnuEndofthisMonth = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.mnuAWeekOn = new System.Windows.Forms.MenuItem();
            this.mnuMondayWO = new System.Windows.Forms.MenuItem();
            this.mnuTuesdayWO = new System.Windows.Forms.MenuItem();
            this.mnuWednesdayWO = new System.Windows.Forms.MenuItem();
            this.mnuThursdayWO = new System.Windows.Forms.MenuItem();
            this.mnuFridayWO = new System.Windows.Forms.MenuItem();
            this.mnuSaturdayWO = new System.Windows.Forms.MenuItem();
            this.mnuSundayWO = new System.Windows.Forms.MenuItem();
            this.mnuThisComming = new System.Windows.Forms.MenuItem();
            this.mnuMondayTC = new System.Windows.Forms.MenuItem();
            this.mnuTuesdayTC = new System.Windows.Forms.MenuItem();
            this.mnuWednesdayTC = new System.Windows.Forms.MenuItem();
            this.mnuThursdayTC = new System.Windows.Forms.MenuItem();
            this.mnuFridayTC = new System.Windows.Forms.MenuItem();
            this.mnuSaturdayTC = new System.Windows.Forms.MenuItem();
            this.mnuSundayTC = new System.Windows.Forms.MenuItem();
            this.pnlCaptionContainer = new FWBS.OMS.UI.Windows.ucDateTimePicker.ColoredDisabledLabel();
            this.cmbTime = new FWBS.Common.UI.Windows.MaskedEdit();
            this.dpDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // labTime
            // 
            this.labTime.AutoSize = true;
            this.labTime.Location = new System.Drawing.Point(150, 20);
            this.labTime.Margin = new System.Windows.Forms.Padding(0);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(45, 13);
            this.labTime.TabIndex = 3;
            this.labTime.Text = " Time : ";
            this.labTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labTime.Visible = false;
            // 
            // dpDate
            // 
            this.dpDate.Controls.Add(this.txtDate);
            this.dpDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.dpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpDate.Location = new System.Drawing.Point(150, 0);
            this.dpDate.Margin = new System.Windows.Forms.Padding(0);
            this.dpDate.Name = "dpDate";
            this.dpDate.Size = new System.Drawing.Size(242, 20);
            this.dpDate.TabIndex = 0;
            this.dpDate.TabStop = false;
            this.dpDate.CloseUp += new System.EventHandler(this.dpDate_CloseUp);
            this.dpDate.DropDown += new System.EventHandler(this.dpDate_DropDown);
            this.dpDate.Enter += new System.EventHandler(this.dpDate_Enter);
            this.dpDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dpDate_MouseDown);
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.SystemColors.Window;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDate.ErrorInvalid = false;
            this.txtDate.InputChar = '_';
            this.txtDate.InputMask = "00/00/0000";
            this.txtDate.Location = new System.Drawing.Point(3, 3);
            this.txtDate.Margin = new System.Windows.Forms.Padding(0);
            this.txtDate.MaxLength = 10;
            this.txtDate.Name = "txtDate";
            this.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDate.Size = new System.Drawing.Size(198, 13);
            this.txtDate.StdInputMask = FWBS.Common.UI.Windows.MaskedEdit.InputMaskType.Custom;
            this.txtDate.TabIndex = 0;
            this.txtDate.Validated += new System.EventHandler(this.ValidateDateAndTime);
            // 
            // mnuDateMagic
            // 
            this.mnuDateMagic.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuToday,
            this.mnuThisWeekend,
            this.mnuNextWeekend,
            this.menuItem5,
            this.mnuNextWorkingDay,
            this.mnuFirstofthisMonth,
            this.mnuEndofthisMonth,
            this.menuItem9,
            this.mnuAWeekOn,
            this.mnuThisComming});
            // 
            // mnuToday
            // 
            this.mnuToday.Index = 0;
            this.mnuToday.Text = "Today";
            this.mnuToday.Click += new System.EventHandler(this.mnuToday_Click);
            // 
            // mnuThisWeekend
            // 
            this.mnuThisWeekend.Index = 1;
            this.mnuThisWeekend.Text = "This Weekend";
            this.mnuThisWeekend.Click += new System.EventHandler(this.mnuThisWeekend_Click);
            // 
            // mnuNextWeekend
            // 
            this.mnuNextWeekend.Index = 2;
            this.mnuNextWeekend.Text = "Next Weekend";
            this.mnuNextWeekend.Click += new System.EventHandler(this.mnuNextWeekend_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "-";
            // 
            // mnuNextWorkingDay
            // 
            this.mnuNextWorkingDay.Index = 4;
            this.mnuNextWorkingDay.Text = "Next Working Day";
            this.mnuNextWorkingDay.Click += new System.EventHandler(this.mnuNextWorkingDay_Click);
            // 
            // mnuFirstofthisMonth
            // 
            this.mnuFirstofthisMonth.Index = 5;
            this.mnuFirstofthisMonth.Text = "First of this Month";
            this.mnuFirstofthisMonth.Click += new System.EventHandler(this.mnuFirstofthisMonth_Click);
            // 
            // mnuEndofthisMonth
            // 
            this.mnuEndofthisMonth.Index = 6;
            this.mnuEndofthisMonth.Text = "End of this Month";
            this.mnuEndofthisMonth.Click += new System.EventHandler(this.mnuEndofthisMonth_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 7;
            this.menuItem9.Text = "-";
            this.menuItem9.Visible = false;
            // 
            // mnuAWeekOn
            // 
            this.mnuAWeekOn.Index = 8;
            this.mnuAWeekOn.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMondayWO,
            this.mnuTuesdayWO,
            this.mnuWednesdayWO,
            this.mnuThursdayWO,
            this.mnuFridayWO,
            this.mnuSaturdayWO,
            this.mnuSundayWO});
            this.mnuAWeekOn.Text = "A Week On";
            this.mnuAWeekOn.Visible = false;
            // 
            // mnuMondayWO
            // 
            this.mnuMondayWO.Index = 0;
            this.mnuMondayWO.Text = "Monday";
            this.mnuMondayWO.Click += new System.EventHandler(this.mnuMondayWO_Click);
            // 
            // mnuTuesdayWO
            // 
            this.mnuTuesdayWO.Index = 1;
            this.mnuTuesdayWO.Text = "Tuesday";
            this.mnuTuesdayWO.Click += new System.EventHandler(this.mnuTuesdayWO_Click);
            // 
            // mnuWednesdayWO
            // 
            this.mnuWednesdayWO.Index = 2;
            this.mnuWednesdayWO.Text = "Wednesday";
            this.mnuWednesdayWO.Click += new System.EventHandler(this.mnuWednesdayWO_Click);
            // 
            // mnuThursdayWO
            // 
            this.mnuThursdayWO.Index = 3;
            this.mnuThursdayWO.Text = "Thursday";
            this.mnuThursdayWO.Click += new System.EventHandler(this.mnuThursdayWO_Click);
            // 
            // mnuFridayWO
            // 
            this.mnuFridayWO.Index = 4;
            this.mnuFridayWO.Text = "Friday";
            this.mnuFridayWO.Click += new System.EventHandler(this.mnuFridayWO_Click);
            // 
            // mnuSaturdayWO
            // 
            this.mnuSaturdayWO.Index = 5;
            this.mnuSaturdayWO.Text = "Saturday";
            this.mnuSaturdayWO.Click += new System.EventHandler(this.mnuSaturdayWO_Click);
            // 
            // mnuSundayWO
            // 
            this.mnuSundayWO.Index = 6;
            this.mnuSundayWO.Text = "Sunday";
            this.mnuSundayWO.Click += new System.EventHandler(this.mnuSundayWO_Click);
            // 
            // mnuThisComming
            // 
            this.mnuThisComming.Index = 9;
            this.mnuThisComming.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMondayTC,
            this.mnuTuesdayTC,
            this.mnuWednesdayTC,
            this.mnuThursdayTC,
            this.mnuFridayTC,
            this.mnuSaturdayTC,
            this.mnuSundayTC});
            this.mnuThisComming.Text = "This Coming";
            this.mnuThisComming.Visible = false;
            // 
            // mnuMondayTC
            // 
            this.mnuMondayTC.Index = 0;
            this.mnuMondayTC.Text = "Monday";
            this.mnuMondayTC.Click += new System.EventHandler(this.mnuMondayWO_Click);
            // 
            // mnuTuesdayTC
            // 
            this.mnuTuesdayTC.Index = 1;
            this.mnuTuesdayTC.Text = "Tuesday";
            this.mnuTuesdayTC.Click += new System.EventHandler(this.mnuTuesdayWO_Click);
            // 
            // mnuWednesdayTC
            // 
            this.mnuWednesdayTC.Index = 2;
            this.mnuWednesdayTC.Text = "Wednesday";
            this.mnuWednesdayTC.Click += new System.EventHandler(this.mnuWednesdayWO_Click);
            // 
            // mnuThursdayTC
            // 
            this.mnuThursdayTC.Index = 3;
            this.mnuThursdayTC.Text = "Thursday";
            this.mnuThursdayTC.Click += new System.EventHandler(this.mnuThursdayWO_Click);
            // 
            // mnuFridayTC
            // 
            this.mnuFridayTC.Index = 4;
            this.mnuFridayTC.Text = "Friday";
            this.mnuFridayTC.Click += new System.EventHandler(this.mnuFridayWO_Click);
            // 
            // mnuSaturdayTC
            // 
            this.mnuSaturdayTC.Index = 5;
            this.mnuSaturdayTC.Text = "Saturday";
            this.mnuSaturdayTC.Click += new System.EventHandler(this.mnuSaturdayWO_Click);
            // 
            // mnuSundayTC
            // 
            this.mnuSundayTC.Index = 6;
            this.mnuSundayTC.Text = "Sunday";
            this.mnuSundayTC.Click += new System.EventHandler(this.mnuSundayWO_Click);
            // 
            // pnlCaptionContainer
            // 
            this.pnlCaptionContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCaptionContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlCaptionContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCaptionContainer.Name = "pnlCaptionContainer";
            this.pnlCaptionContainer.Size = new System.Drawing.Size(150, 23);
            this.pnlCaptionContainer.TabIndex = 8;
            // 
            // cmbTime
            // 
            this.cmbTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbTime.ErrorInvalid = false;
            this.cmbTime.InputChar = '_';
            this.cmbTime.InputMask = "";
            this.cmbTime.Location = new System.Drawing.Point(349, 0);
            this.cmbTime.Margin = new System.Windows.Forms.Padding(0);
            this.cmbTime.MaxLength = 0;
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbTime.Size = new System.Drawing.Size(43, 20);
            this.cmbTime.StdInputMask = FWBS.Common.UI.Windows.MaskedEdit.InputMaskType.None;
            this.cmbTime.TabIndex = 4;
            this.cmbTime.Visible = false;
            this.cmbTime.Enter += new System.EventHandler(this.cmbTime_Enter);
            this.cmbTime.Validating += new System.ComponentModel.CancelEventHandler(this.cmbTime_Validating);
            this.cmbTime.Validated += new System.EventHandler(this.ValidateDateAndTime);
            // 
            // ucDateTimePicker
            // 
            this.Controls.Add(this.dpDate);
            this.Controls.Add(this.pnlCaptionContainer);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucDateTimePicker";
            this.Size = new System.Drawing.Size(392, 23);
            this.Load += new System.EventHandler(this.ucDateTimePicker_Load);
            this.VisibleChanged += new System.EventHandler(this.ucDateTimePicker_VisibleChanged);
            this.Leave += new System.EventHandler(this.RaiseLeaveEvent);
            this.dpDate.ResumeLayout(false);
            this.dpDate.PerformLayout();
            this.ResumeLayout(false);

        }

        protected override void OnFontChanged(EventArgs eventArgs)
        {
            base.OnFontChanged(eventArgs);

            if (CaptionTop)
                pnlCaptionContainer.Height = CalcCaptionHeight(true);

            ValidateCtrlSize();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null)
            {
                ManageChildLayout();
                AdjustTxtDateSize();
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (omsDesignMode)
            {
                pnlCaptionContainer.Refresh();
            }
        }

        #endregion

        #region Fields
        private bool _timevisible = false;
        private int _timeWidth = 43;
        private bool _timeenabled = true;
        private bool _datevisible = true;
        private bool _timelabelvisible = false;

        private DateTimePickerLayout _dtplayout = DateTimePickerLayout.dtpSameLine;
        private DateTimeNULL _value = DBNull.Value;
        private TimeSpan _defaulttime = new TimeSpan(9, 0, 0);
        private bool _required = false;
        private bool _allownull = true;
        private string _timsep;
        private DateTimeType _datetimetype = DateTimeType.DateTime;
        private string _slavename = "";
        private System.DateTimeKind _displkind = DateTimeKind.Local;
        /// <summary>
        /// Display caption on top.
        /// </summary>
        protected bool _captionTop = false;

        private bool ruleapplied;
   
        #endregion

        #region Public
        public void SetRTL(Form parentform)
        {
            foreach (Control item in this.Controls)
            {
                Global.RightToLeftControlConverter(item, parentform);
            }
            dpDate.RightToLeftLayout = true;
            dpDate.TabStop = true;
            txtDate.Visible = false;
            dpDate.ShowCheckBox = true;
            dpDate.ValueChanged += new EventHandler(dpDate_ValueChanged);
            dpDate.Enter -= new System.EventHandler(this.dpDate_Enter);

            if (this.DateTimeNullValue == DBNull.Value)
            {
                dpDate.Checked = false;
                dpDate.CustomFormat = txtDate.Text;
            }
        }

        #endregion

        #region Private

        private void RaiseLeaveEvent(object sender, EventArgs e)
        {
            this.OnChanged();
        }

        private DateTime? maxdate;
        internal DateTime? MaxDate
        {
            get
            {
                return maxdate;
            }
            set
            {
                maxdate = value;
                ManageChildLayout();
            }
        }

        private DateTime? mindate;
        internal DateTime? MinDate
        {
            get
            {
                return mindate;
            }
            set
            {
                mindate = value;
                ManageChildLayout();
            }
        }

        /// <summary>
        /// Manages child layout according to current settings. 
        /// </summary>
        protected virtual void ManageChildLayout()
        {
            SuspendLayout();
            try
            {
                if (this.Parent != null)
                {
                    dpDate.MaxDate = new DateTime(9998, 12, 31);
                    dpDate.MinDate = new DateTime(1773, 1, 1);

                    if (_greaterthantoday)
                    {
                        dpDate.MinDate = DateTime.Today;
                    }
                    else if (_lessthantoday)
                    {
                        dpDate.MaxDate = DateTime.Today;
                    }

                    if (maxdate != null)
                        this.dpDate.MaxDate = maxdate.Value;
                    if (mindate != null)
                        this.dpDate.MinDate = mindate.Value;

                    this.dpDate.Visible = _datevisible;
                    this.cmbTime.Visible = _timevisible;
                    this.cmbTime.Enabled = _timeenabled;
                    this.labTime.Visible = _timelabelvisible;

                    pnlCaptionContainer.Dock = _captionTop ? DockStyle.Top : DockStyle.Left;

                    if (_datevisible)
                    {
                        this.Controls.Add(dpDate);
                        this.dpDate.Dock = DockStyle.Top;
                    }
                    else
                    {
                        this.Controls.Remove(dpDate);
                        this.dpDate.Dock = DockStyle.None;
                    }

                    if (_dtplayout == DateTimePickerLayout.dtpSameLine)
                    {
                        if (_timelabelvisible)
                        {
                            this.Controls.Add(labTime);
                            labTime.Dock = DockStyle.Right;
                        }
                        else
                        {
                            this.Controls.Remove(labTime);
                            labTime.Dock = DockStyle.None;
                        }

                        if (_timevisible)
                        {
                            this.Controls.Add(cmbTime);
                            cmbTime.Dock = DockStyle.Right;
                        }
                        else
                        {
                            this.Controls.Remove(cmbTime);
                            cmbTime.Dock = DockStyle.None;
                        }
                    }
                    else if (_dtplayout == DateTimePickerLayout.dtpTopBottom)
                    {
                        if (_timelabelvisible)
                        {
                            this.Controls.Add(labTime);
                            labTime.Dock = DockStyle.Left;
                        }
                        else
                        {
                            this.Controls.Remove(labTime);
                            labTime.Dock = DockStyle.None;
                        }

                        if (_timevisible)
                        {
                            this.Controls.Add(cmbTime);
                            cmbTime.Dock = DockStyle.Left;
                        }
                        else
                        {
                            this.Controls.Remove(cmbTime);
                            cmbTime.Dock = DockStyle.None;
                        }
                    }

                    if (this.omsDesignMode)
                        ValidateDateAndTime(this, EventArgs.Empty);

                    SetControlsOrder();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            ResumeLayout();
        }

        protected void ValidateDateAndTime(object sender, System.EventArgs e)
        {
            try
            {
                int hour = 0;
                int min = 0;
                int second = 0;

                if (_timevisible)
                {
                    hour = _defaulttime.Hours;
                    min = _defaulttime.Minutes;
                    second = _defaulttime.Seconds;
                    string[] time = cmbTime.Text.Split(_timsep.ToCharArray());
                    try
                    {
                        if (time.Length > 0 && time[0] != "__")
                            hour = Convert.ToInt32(time[0].Substring(0, 2));
                        if (time.Length > 1 && time[1] != "__")
                            min = Convert.ToInt32(time[1].Substring(0,2));
                        if (time.Length > 2 && time[2] != "__")
                            second = Convert.ToInt32(time[2].Substring(0, 2));
                        
                    }
                    catch { }

                    if (TimeLayout == Windows.TimeLayout.CurrentCulture)
                    {
                        System.Globalization.DateTimeFormatInfo objDTFmt = cultureInfo.DateTimeFormat;
                        DateTime ampmdt;
                        if (DateTime.TryParse(cmbTime.Text, out ampmdt))
                        {
                            hour = ampmdt.Hour;
                            min = ampmdt.Minute;
                            second = ampmdt.Second;
                        }
                    }

                    if (hour > 23 || min > 59 || second > 59 || hour < 0 || min < 0 || second < 0)
                    {
                        if (hour > 23) hour = 23;
                        if (min > 59) min = 59;
                        if (second > 59) second = 59;
                        if (hour < 0) hour = 0;
                        if (min < 0) min = 0;
                        if (second < 0) second = 0;
                        switch (TimeLayout)
                        {
                            case TimeLayout.TwentyFourHour:
                                cmbTime.Text = new TimeSpan(hour, min, second).ToString().Substring(0, cmbTime.MaxLength);
                                break;
                            case TimeLayout.CurrentCulture:
                                cmbTime.Text = PadTime(new DateTime(1, 1, 1, hour, min, second,DateTimeKind.Local));
                                break;
                            default:
                                break;
                        }
                    }
                }

                DateTime? dt = ValidateDateBox();

                
                if (dpDate.Visible && cmbTime.Visible)
                {
                    if (dt != null)
                        this.DateTimeNullValue = new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, hour, min, second, _displkind);
                    else
                        this.DateTimeNullValue = DBNull.Value;
                }
                else if (dpDate.Visible && cmbTime.Visible == false)
                {
                    if (dt != null)
                        this.DateTimeNullValue = new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, 0, 0, 0, _displkind);
                    else
                        this.DateTimeNullValue = DBNull.Value;
                }
                else if (dpDate.Visible == false && cmbTime.Visible)
                {
                    if (cmbTime.Value != "" || AllowNull == false)
                    {
                        DateTime dm = new DateTime(dpDate.MinDate.Year, dpDate.MinDate.Month, dpDate.MinDate.Day, hour, min, second, _displkind);
                        this.DateTimeNullValue = dm;
                    }
                    else
                        this.DateTimeNullValue = DBNull.Value;
                }
                
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private DateTime? ValidateDateBox()
        {
            if (txtDate.Value == "")
                return null;

            try
            {
                DateTime dm = Convert.ToDateTime(txtDate.Text, cultureInfo);
                if (dm < dpDate.MinDate || dm > dpDate.MaxDate)
                    throw new ArgumentException();

                return dm;
            }
            catch (System.ArgumentException)
            {
                if (Session.CurrentSession.IsLoggedIn)
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("INVDTGL", "Invalid Date '%1%'. Date should be greater than %2% and less than %3%, reseting to todays date.", txtDate.Text, dpDate.MinDate.ToShortDateString(), dpDate.MaxDate.ToShortDateString());
                else
                    System.Windows.Forms.MessageBox.Show(String.Format("Invalid Date '{0}'. Date should be greater than {1} and less than {2}, reseting to todays date.", txtDate.Text, dpDate.MinDate.ToShortDateString(), dpDate.MaxDate.ToShortDateString()), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DateTime dm = DateTime.Today;
                txtDate.Text = PadDate(dm);
                return dm;
            }
            catch
            {
                if (Session.CurrentSession.IsLoggedIn)
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("INVDATE", "Invalid Date '%1%' reseting to todays date.", txtDate.Text);
                else
                    System.Windows.Forms.MessageBox.Show(String.Format("Invalid Date '{0}' reseting to todays date.", txtDate.Text), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DateTime dm = DateTime.Today;
                txtDate.Text = PadDate(dm);
                return dm;
            }
        }

        private void dpDate_ValueChanged(object sender, EventArgs e)
        {
            if (dpDate.Checked)
            {
                this.DateTimeNullValue = dpDate.Value;
                dpDate.CustomFormat = null;
            }
            else
            {
                this.DateTimeNullValue = DBNull.Value;
                dpDate.CustomFormat = rtlResetFormat;
            }
        }

        private void dpDate_Enter(object sender, EventArgs e)
        {
            try
            {
                if (dpDate.Tag == null)
                    dpDate.Value = Convert.ToDateTime(txtDate.Text, cultureInfo);
            }
            catch { }
            txtDate.Focus();
        }

        private void cmbTime_Enter(object sender, EventArgs e)
        {
            cmbTime.Select(0, 1);
        }

        private void dpDate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_optionsvisible)
                {
                    mnuDateMagic.Show(this, new Point(dpDate.Left, dpDate.Height));
                    txtDate.Focus();
                }
            }
        }

        private void ucDateTimePicker_Load(object sender, EventArgs e)
        {
            if (ruleapplied)
            {
                OnActiveChanged();
                OnChanged();
                ruleapplied = false;
            }
        }

        private void cmbTime_Validating(object sender, CancelEventArgs e)
        {
            OnTimeValidating(sender, e);
        }
        #endregion

        #region Menus

        private void mnuToday_Click(object sender, System.EventArgs e)
        {
            this.DateTimeNullValue = DateTime.Today;
        }
        
        private void mnuThisWeekend_Click(object sender, System.EventArgs e)
        {
            DateTimeNULL tempDate = this.DateTimeNullValue; 

            if (_value.ToObject() == DBNull.Value)
                tempDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, _displkind);

            DateTimeNULL tempValue = tempDate;

            if (tempValue.DateTime.DayOfWeek != DayOfWeek.Saturday)
            {
                short x = 1;
                do
                {
                    tempDate = tempValue.AddDays(x);
                    x++;
                }
                while (tempDate.DateTime.DayOfWeek != DayOfWeek.Saturday);
            }

            this.DateTimeNullValue = tempDate;
        }

        
        private void mnuNextWeekend_Click(object sender, System.EventArgs e)
        {
            DateTimeNULL tempDate = this.DateTimeNullValue; 

            if (_value.ToObject() == DBNull.Value)
                tempDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, _displkind);
            
            DateTimeNULL tempValue = tempDate;

            short x = 1;
            if (tempValue.DateTime.DayOfWeek != DayOfWeek.Saturday)
            {
                do
                {
                    tempDate = tempValue.AddDays(x);
                    x++;
                }
                while (tempDate.DateTime.DayOfWeek != DayOfWeek.Saturday);
            }
            tempDate = tempDate.AddDays(1);
            
            do
            {
                tempDate = tempValue.AddDays(x);
                x++;
            }
            while (tempDate.DateTime.DayOfWeek != DayOfWeek.Saturday);

            this.DateTimeNullValue = tempDate;
        }


        private void mnuNextWorkingDay_Click(object sender, System.EventArgs e)
        {
            DateTimeNULL tempDate = this.DateTimeNullValue;

            if (_value.ToObject() == DBNull.Value)
                tempDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, _displkind);

            DateTimeNULL tempValue = tempDate;

            short x = 1;
            do
            {
                tempDate = tempValue.AddDays(x);
                x++;
            }
            while (tempDate.DateTime.DayOfWeek == DayOfWeek.Saturday || tempDate.DateTime.DayOfWeek == DayOfWeek.Sunday);

            this.DateTimeNullValue = tempDate;
        }

        private void mnuFirstofthisMonth_Click(object sender, System.EventArgs e)
        {
            DateTimeNULL tempDate = this.DateTimeNullValue;

              if (_value.ToObject() == DBNull.Value)
                tempDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);
            
            tempDate = new DateTime(tempDate.Year, tempDate.Month, 1, tempDate.Hour, tempDate.Minute, tempDate.Second, _displkind);
            
            this.DateTimeNullValue = tempDate;
        }

        private void mnuEndofthisMonth_Click(object sender, System.EventArgs e)
        {
            DateTimeNULL tempDate = this.DateTimeNullValue;

            if (_value.ToObject() == DBNull.Value)
                tempDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            tempDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month), tempDate.Hour, tempDate.Minute, tempDate.Second, _displkind);

            this.DateTimeNullValue = tempDate;
        }

        
        #region "NOT IN USE - 'Week On ...'"

        //CM 230715: It appears that these methods are not in use (the menu for these options (mnuAWeekOn) is set as 'visible = false')

        private void mnuMondayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Monday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Monday);
                if (sender == mnuMondayWO) this.DateTimeNullValue = _value.AddDays(7);
            }
        }

        private void mnuTuesdayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Tuesday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Tuesday);
                if (sender == mnuTuesdayWO) this.DateTimeNullValue = _value.AddDays(7);
            }

        }

        private void mnuWednesdayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Wednesday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Wednesday);
                if (sender == mnuWednesdayWO) this.DateTimeNullValue = _value.AddDays(7);
            }
        }

        private void mnuThursdayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Thursday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Thursday);
                if (sender == mnuTuesdayWO) this.DateTimeNullValue = _value.AddDays(7);
            }
        }

        private void mnuFridayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Friday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Friday);
                if (sender == mnuFridayWO) this.DateTimeNullValue = _value.AddDays(7);
            }
        }

        private void mnuSaturdayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Saturday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Saturday);
                if (sender == mnuSaturdayWO) this.DateTimeNullValue = _value.AddDays(7);
            }
        }

        private void mnuSundayWO_Click(object sender, System.EventArgs e)
        {
            if (_value.ToObject() == DBNull.Value)
                this.DateTimeNullValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 08, 0, 0, DateTimeKind.Local);

            if (_value.DateTime.DayOfWeek != DayOfWeek.Sunday)
            {
                do
                    this.DateTimeNullValue = _value.AddDays(1);
                while (_value.DateTime.DayOfWeek != DayOfWeek.Sunday);
                if (sender == mnuSundayWO) this.DateTimeNullValue = _value.AddDays(7);
            }
        }

        #endregion
        
        #endregion
        
        #region Properties
        [Category("UTC")]
        [DefaultValue(DateTimeKind.Local)]
        public DateTimeKind DisplayDateIn
        {
            get
            {
                return _displkind;
            }
            set
            {
                _displkind = value;
            }
        }


        [Browsable(false)]
        public EnquiryForm EnquiryForm
        {
            get
            {
                return Parent as EnquiryForm;
            }
        }


        [TypeConverter(typeof(ContactEnquiryControlLister))]
        public string SlaveDatePicker
        {
            get
            {
                return _slavename;
            }
            set
            {
                if (this.Parent is EnquiryForm)
                {
                    EnquiryForm _parent = this.Parent as EnquiryForm;
                    ucDateTimePicker ctrl = _parent.GetControl(value) as ucDateTimePicker;
                    if (ctrl != null)
                    {
                        if (ctrl != this)
                        {
                            _slavename = value;
                        }
                        else if (ctrl == this)
                            throw new OMSException2("ERRCNTSTSLV", "ERROR cannot set self as Slave...");
                    }
                }
                else
                {
                    _slavename = value;
                }
            }
        }

        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return _isdirty;
            }
            set
            {
                _isdirty = value;
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool SpecialOptionsVisible
        {
            get
            {
                return _optionsvisible;
            }
            set
            {
                _optionsvisible = value;
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool TimeVisible
        {
            get
            {
                return _timevisible;
            }
            set
            {
                _timevisible = value;
                ManageChildLayout();
            }
        }


        [Category("OMS Appearance")]
        [Browsable(true)]
        [DefaultValue(43)]
        public int TimeWidth
        {
            get
            {
                return _timeWidth;
            }
            set
            {
                _timeWidth = Math.Max(value, 43);
                if (cmbTime != null)
                {
                    cmbTime.Width = _timeWidth;
                    ManageChildLayout();
                }
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(true)]
        public bool TimeEnabled
        {
            get
            {
                return _timeenabled;
            }
            set
            {
                _timeenabled = value;
                ManageChildLayout();
            }
        }

        [DefaultValue(DateTimeType.DateTime)]
        [Category("OMS Data")]
        public DateTimeType DateTimeType
        {
            get
            {
                return _datetimetype;
            }
            set
            {
                _datetimetype = value;
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(true)]
        public bool DateVisible
        {
            get
            {
                return _datevisible;
            }
            set
            {
                _datevisible = value;
                ManageChildLayout();
            }
        }

        [Category("OMS Appearance")]
        public string DefaultTime
        {
            get
            {
                return _defaulttime.ToString();
            }
            set
            {
                _defaulttime = TimeSpan.Parse(value);
                ManageChildLayout();
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool TimeLabelVisible
        {
            get
            {
                return _timelabelvisible;
            }
            set
            {
                _timelabelvisible = value;
                ManageChildLayout();
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(DateTimePickerLayout.dtpSameLine)]
        public DateTimePickerLayout DateTimeLayout
        {
            get
            {
                return _dtplayout;
            }
            set
            {
                _dtplayout = value;
                if (omsDesignMode)
                {
                    ValidateCtrlSize();
                }
                ManageChildLayout();
                OnDateTimePickerLayoutChanged();
            }
        }

        [Category("OMS Data")]
        [DefaultValue(true)]
        public bool AllowNull
        {
            get
            {
                return _allownull;
            }
            set
            {
                _allownull = value;
                ManageChildLayout();
            }
        }

        [Category("OMS Data")]
        public DateTimeNULL DateTimeNullValue
        {

            get
            {                       
                return _value;
            }
            set
            {
                SlaveDatePickerValidation();
                
                if (_value.IsNull && value.IsNull)
                    return;

                if (_value == value)
                    return;

                DateTimeNULL _old = _value;
                _value = value;
                try
                {
                    if (_value.ToObject() != DBNull.Value)
                    {
                        if (_value.DateTime > new DateTime(1773, 1, 1))
                        {
                            txtDate.InputMask = GenerateInputMask(DateTimeMask.Date);
                            cmbTime.InputMask = GenerateInputMask(DateTimeMask.Time);
                            DateTime dt = _value.DateTime;
                            if (_displkind == DateTimeKind.Local && dt.Kind != DateTimeKind.Unspecified)
                            {
                                dt = dt.ToLocalTime();
                            }
                            else if (_displkind == DateTimeKind.Utc && dt.Kind != DateTimeKind.Unspecified)
                            {
                                dt = dt.ToUniversalTime();
                            }
                            txtDate.Text = PadDate(dt);
                            cmbTime.Text = PadTime(dt);
                            dpDate.Value = dt;
                        }
                    }
                    else
                    {
                        if (AllowNull)
                        {
                            cmbTime.Text = "";
                            txtDate.Text = "";
                            _value = DBNull.Value;
                            dpDate.Checked = false;
                        }
                        else
                        {
                            if (_old.DateTime >= new DateTime(1773, 1, 1))
                            {
                                txtDate.InputMask = GenerateInputMask(DateTimeMask.Date);
                                cmbTime.InputMask = GenerateInputMask(DateTimeMask.Time);
                                DateTime dt = _old.DateTime;
                                if (_displkind == DateTimeKind.Local && dt.Kind != DateTimeKind.Unspecified)
                                {
                                    dt = dt.ToLocalTime();
                                }
                                else if (_displkind == DateTimeKind.Utc && dt.Kind != DateTimeKind.Unspecified)
                                {
                                    dt = dt.ToUniversalTime();
                                }
                                txtDate.Text = PadDate(dt);
                                cmbTime.Text = PadTime(dt);
                                dpDate.Value = dt;
                            }
                            _value = _old;
                        }
                    }
                    OnActiveChanged();
                    OnChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Generates an input mask based on incoming date value to handle 2 & 4 figure years and single mo0nth and day values
        /// </summary>
        /// <param name="indate">string value of a date</param>
        /// <returns>input mask based on incoming date</returns>
        private string GenerateInputMask(DateTimeMask type)
        {
            switch (type)
            {
                case DateTimeMask.Date:
                    {
                        string mask = "";
                        try //exception will be thrown if if culture is nuetral
                        {
                            DateTimeFormatInfo objDTFmt = cultureInfo.DateTimeFormat;

                            //need to investigate fully the different formats available
                            mask = objDTFmt.ShortDatePattern.Replace("yyyy", "0000").ToLower();
                            mask = mask.Replace("yy", "00");
                            mask = mask.Replace("mmmm", "AAA");
                            mask = mask.Replace("mmm", "AAA");
                            mask = mask.Replace("mm", "00");
                            mask = mask.Replace("m", "90");
                            mask = mask.Replace("dd", "00");
                            mask = mask.Replace("d", "90");
                        }
                        catch
                        {
                            mask = "90/90/0000";
                        }
                        return mask;
                    }
                case DateTimeMask.Time:
                    {
                        string mask = "";
                        try //exception will be thrown if if culture is nuetral
                        {
                            DateTimeFormatInfo objDTFmt = cultureInfo.DateTimeFormat;
                            switch (TimeLayout)
                            {
                                case TimeLayout.TwentyFourHour:
                                    mask = "H0" + objDTFmt.TimeSeparator + "n0";
                                    break;
                                case TimeLayout.CurrentCulture:

                                    //need to investigate fully the different formats available

                                    mask = objDTFmt.ShortTimePattern.ToUpperInvariant().Replace("HH", "ZZ");
                                    mask = mask.Replace("H", "H0");

                                    mask = mask.Replace("MM", "n0");
                                    mask = mask.Replace("SS", "n0");
                                    mask = mask.Replace("ZZ", "H0");
                                    mask = mask.Replace("TT", "AA");
                                    break;
                                default:
                                    break;
                            }

                        }
                        catch
                        {
                            mask = "00:00";
                        }
                        return mask;
                    }
                default:
                    return "";
            }
        }


        /// <summary>
        /// A date of 4/12/2004 will break mask edit control until we find a better control or option
        /// </summary>
        /// <param name="indate">string date value</param>
        /// <returns>date padded with zeros</returns>
        private string PadDate(DateTime date)
        {
            DateTimeFormatInfo objDTFmt = cultureInfo.DateTimeFormat;
            string mask = objDTFmt.ShortDatePattern.ToLower();
            mask = mask.Replace("mmmm", "XXX");
            mask = mask.Replace("mmm", "XXX");
            mask = mask.Replace("mm", "XX");
            mask = mask.Replace("m", "MM");
            mask = mask.Replace("dd", "ZZ");
            mask = mask.Replace("d", "dd");
            mask = mask.Replace("XXX", "MMM");
            mask = mask.Replace("XX", "MM");
            mask = mask.Replace("ZZ", "dd");
            return date.ToString(mask);
        }

        private string PadTime(DateTime date)
        {
            switch (TimeLayout)
            {
                default:
                case TimeLayout.TwentyFourHour:
                    {
                        string mask = "HH:mm";
                        return date.ToString(mask);
                    }
                case TimeLayout.CurrentCulture:
                    {
                        DateTimeFormatInfo objDTFmt = cultureInfo.DateTimeFormat;
                        string mask = objDTFmt.ShortTimePattern;
                        return date.ToString(mask).PadLeft(cmbTime.InputMask.Length);
                    }
            }
       }

        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool GreaterThanToday
        {
            get
            {
                return _greaterthantoday;
            }
            set
            {
                _greaterthantoday = value;
                _lessthantoday = false;
                ManageChildLayout();
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool LessThanToday
        {
            get
            {
                return _lessthantoday;
            }
            set
            {
                _lessthantoday = value;
                _greaterthantoday = false;
                ManageChildLayout();
            }
        }

        private TimeLayout _timelayout;

        [Category("OMS Appearance")]
        [DefaultValue(TimeLayout.TwentyFourHour)]
        public TimeLayout TimeLayout
        {
            get
            {
                return _timelayout;
            }
            set
            {
                _timelayout = value;
                ManageChildLayout();
            }
        }

        [Category("OMS Appearance")]
        [CodeLookupSelectorTitle("CUETEXT", "Cue Text")]
        [DefaultValue(null)]
        [Description("Localised code of the Controls CueText. Use | symbol as separator for Date and Time Cue texts."), LocCategory("Design")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MergableProperty(false)]
        public virtual CodeLookupDisplay CueText
        {
            get { return _cueText ?? (_cueText = new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME)); }

            set
            {
                if (_cueText != value)
                {
                    _cueText = value;

                    if (!string.IsNullOrEmpty(_cueText.Description))
                    {
                        var values = _cueText.Description.Split('|');
                        txtDate.CueText = values[0];
                        if (values.Length > 1)
                        {
                            cmbTime.CueText = values[1];
                        }
                    }
                    IsDirty = true;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string CueTextCode
        {
            get { return _cueTextCode; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals(_cueTextCode))
                {
                    _cueTextCode = value;
                    CueText = new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME)
                    {
                        Code = value,
                        Description = CodeLookup.GetLookup(CUETEXT_CODELOOKUPGROUPNAME, value),
                        UICulture = Thread.CurrentThread.CurrentCulture.Name,
                        Help = CodeLookup.GetLookupHelp(CUETEXT_CODELOOKUPGROUPNAME, value)
                    };
                }
            }
        }
        
        #endregion

        #region IBasicEnquiryControl2 Implementation

        /// <summary>
        /// Gets whether the current control can be stretched by its Y co-ordinate.
        /// This is a design mode property and is set to true.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(true)]
        public virtual bool LockHeight
        {
            get
            {
                return true;
            }
        }

        [Browsable(true)]
        public object Control
        {
            get
            {
                return this;
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return pnlCaptionContainer.Text;
            }
            set
            {
                pnlCaptionContainer.Text = value;
            }
        }

        /// <summary>
        /// Gets or Sets the control as required.  This is then used by the rendering form to display the
        /// control as required by its own definition.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public virtual bool Required
        {
            get
            {
                return _required;
            }
            set
            {
                _required = value;
            }
        }
        
        /// <summary>
        /// Gets or Sets the editable format of the control.  By default the whole control toggles it's enable property.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public virtual bool ReadOnly
        {
            get
            {
                return !Enabled;
            }
            set
            {
                Enabled = !value;
            }
        }

        /// <summary>
        /// Gets or Sets the caption width of a control, leaving the rest of the width of the control
        /// to be the width of the internal editing control.
        /// </summary>
        [Browsable(false)]
        public virtual int CaptionWidth
        {
            get
            {
                return pnlCaptionContainer.Width;
            }
            set
            {
                pnlCaptionContainer.Width = value;
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public virtual bool CaptionTop
        {
            get
            {
                return _captionTop;
            }
            set
            {
                _captionTop = value;
                ValidateCtrlSize();
                ManageChildLayout();
                pnlCaptionContainer.Height = CalcCaptionHeight(true);
                pnlCaptionContainer.Width = eBase2.DefaultCaptionWidth;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or Sets the value of the control.
        /// </summary>
        [DefaultValue(null)]
        [Category("OMS")]
        public object Value
        {
            get
            {
                if (_datetimetype == DateTimeType.DateTime)
                    if (_value == DBNull.Value || DateTime.MinValue.Equals(_value.DateTime))
                        return DBNull.Value;
                    else
                        return _value.ToObject();
                else
                {
                    return _value;
                }
            }
            set
            {
                if (_allownull == false && value == DBNull.Value)
                {
                    DateTime dm = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, _defaulttime.Hours, _defaulttime.Minutes, _defaulttime.Seconds, _displkind);
                    this.DateTimeNullValue = ConvertDef.ToDateTimeNULL(dm, DBNull.Value);
                    ruleapplied = true;
                }
                else
                {
                    this.DateTimeNullValue = ConvertDef.ToDateTimeNULL(value, DBNull.Value);
                }
            }
        }

        private DateTimeNULL previousDateTime = DBNull.Value;

        /// <summary>
        /// Executes the changed event.
        /// </summary>
        public void OnChanged()
        {
            if (Changed != null && IsDirty)
            {
                Changed(this, EventArgs.Empty);
                IsDirty = false;
            }
        }

        public void SlaveDatePickerValidation()
        {
            EnquiryForm _parent = this.Parent as EnquiryForm;
            if (_parent != null && _slavename != "")
            {
                ucDateTimePicker ctrl = _parent.GetControl(_slavename) as ucDateTimePicker;
                if (ctrl != null)
                {
                    if (ctrl.DateTimeNullValue == DBNull.Value)
                        ctrl.Value = this.Value;
                    else if (ctrl.DateTimeNullValue < this.DateTimeNullValue)
                        ctrl.Value = this.Value;
                    else if (this.DateTimeNullValue == DBNull.Value)
                        ctrl.Value = this.Value;

                    if (this.DateTimeNullValue != DBNull.Value)
                        ctrl.MinDate = ((DateTime)this.DateTimeNullValue).Date;
                }
            }
        }
        public void OnActiveChanged()
        {
            SlaveDatePickerValidation();

            IsDirty = true;
            if (ActiveChanged != null)
                ActiveChanged(this, EventArgs.Empty);
        }

        private bool _omsdesignmode = false;

        private void ucDateTimePicker_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.Visible && this.IsDirty)
            {
                OnActiveChanged();
                OnChanged();
            }
        }

        private void dpDate_CloseUp(object sender, System.EventArgs e)
        {
            dpDate.Tag = null;
            txtDate.Text = PadDate(dpDate.Value.Date);
            DateTime time = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, _defaulttime.Hours, _defaulttime.Minutes, _defaulttime.Seconds);
            if (cmbTime.Visible && cmbTime.Value == "")
                cmbTime.Text = PadTime(time);
            ValidateDateAndTime(sender, e);
            txtDate.Focus();
            txtDate.SelectAll();
        }
        private void dpDate_DropDown(object sender, System.EventArgs e)
        {
            dpDate.Tag = "ON";
            txtDate.InputMask = _inputDateMask;
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool omsDesignMode
        {
            get
            {
                return _omsdesignmode;
            }
            set
            {
                _omsdesignmode = value;
                if (_omsdesignmode)
                {
                    this.Resize += OnResize;
                    txtDate.InputMask = _inputDateMask;
                }
            }
        }
        #endregion

        #region Overrides
        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.Visible && System.Windows.Forms.Control.IsMnemonic(charCode, this.Text))
            {
                txtDate.Focus();
                return true;
            }
            else
                return false;

        }

        #endregion

        [Browsable(false)]
        public Control ToolTipControl
        {
            get { return pnlCaptionContainer; }
        }

        protected virtual void ValidateCtrlSize()
        {
            if (omsDesignMode)
            {
                this.cmbTime.Font = this.Font;
                var captionTopHeight = _captionTop ? CalcCaptionHeight(false) : 0;

                switch (_dtplayout)
                {
                    case (DateTimePickerLayout.dtpSameLine):
                        {
                            this.Height = Math.Max(dpDate.PreferredHeight, cmbTime.PreferredHeight) + captionTopHeight;
                            break;
                        }
                    case DateTimePickerLayout.dtpTopBottom:
                        {
                            this.Height = dpDate.PreferredHeight + cmbTime.PreferredHeight + captionTopHeight;
                            break;
                        }
                }
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            ValidateCtrlSize();
        }
        
        private void SetControlsOrder()
        {
            switch (_dtplayout)
            {
                case DateTimePickerLayout.dtpSameLine:
                    {
                        if (_datevisible) this.dpDate.SendToBack();
                        if (_timelabelvisible) this.labTime.SendToBack();
                        if (_timevisible) this.cmbTime.SendToBack();
                        this.pnlCaptionContainer.SendToBack();
                        break;
                    }
                case DateTimePickerLayout.dtpTopBottom:
                    {
                        if (_timevisible) this.cmbTime.SendToBack();
                        if (_timelabelvisible) this.labTime.SendToBack();
                        if (_datevisible) this.dpDate.SendToBack();
                        this.pnlCaptionContainer.SendToBack();
                        break;
                    }
            }
        }

        private int CalcCaptionHeight(bool isDeviceDpiRequired)
        {
            using (var graphics = pnlCaptionContainer.CreateGraphics())
            {
                return isDeviceDpiRequired
                    ? Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height * 96 / (omsDesignMode || Font == DefaultFont ? 96 : DeviceDpi)))
                    : Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height));
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            if (_captionTop)
            {
                pnlCaptionContainer.Height = CalcCaptionHeight(false);
            }

            AdjustTxtDateSize(true);
        }

        #region DATETIMEPICKERINFO
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct DATETIMEPICKERINFO
        {
            public uint cbSize;
            public RECT rcCheck;
            public uint stateCheck;
            public RECT rcButton;
            public uint stateButton;
            public IntPtr hwndEdit;
            public IntPtr hwndUD;
            public IntPtr hwndDropDown;
        }

        private const int DTM_GETDATETIMEPICKERINFO = 0x100E;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref DATETIMEPICKERINFO lParam);

        private void AdjustTxtDateSize(bool isScaled = false)
        {
            DATETIMEPICKERINFO dateTimePickerInfo = new DATETIMEPICKERINFO
            {
                cbSize = (uint) Marshal.SizeOf(typeof(DATETIMEPICKERINFO))
            };

            SendMessage(dpDate.Handle, DTM_GETDATETIMEPICKERINFO, IntPtr.Zero, ref dateTimePickerInfo);

            int offset = isScaled ? LogicalToDeviceUnits(3) : 3;
            int btnWidth = dateTimePickerInfo.rcButton.right - dateTimePickerInfo.rcButton.left;

            if (!isScaled)
            {
                btnWidth = btnWidth * 96 / DeviceDpi;
            }

            txtDate.Anchor = AnchorStyles.None;
            txtDate.Location = new Point(offset, offset);
            txtDate.Width = Math.Max(dpDate.ClientSize.Width - btnWidth - offset * 2, 0);
            txtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }
        #endregion
    }

    #region Designer
    public class DateTimePickerDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        private ucDateTimePicker _ctrl = null;

        public DateTimePickerDesigner()
        {
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = this.Control as ucDateTimePicker;
                    _ctrl.cmbTime.Enabled = false;
                }
                return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable | SelectionRules.Visible;
            }
        }

    }
    #endregion

    #region Public Enums
    public enum DateTimePickerLayout { dtpSameLine, dtpTopBottom }
    public enum DateTimeType { DateTime, DateTimeNULL }
    public enum TimeLayout { TwentyFourHour, CurrentCulture }
    #endregion
}
