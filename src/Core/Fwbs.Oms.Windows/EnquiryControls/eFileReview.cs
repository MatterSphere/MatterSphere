using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A date time object which takes on some windows UI rules when leached to an OMSFile object.
    /// </summary>
    internal class eFileReview : FWBS.OMS.UI.Windows.ucDateTimePicker
    {
        #region Fields
        /// <summary>
        /// Last Review Date Label
        /// </summary>
        private CustomLabel lblLastReviewDate;

        private class CustomLabel : Label
        {
            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                base.SetBoundsCore(x, y, width, PreferredHeight, specified);
            }

            protected override void OnLayout(LayoutEventArgs e)
            {
                base.OnLayout(e);
                if ((e.AffectedProperty == "Bounds" || e.AffectedProperty == "Font") && e.AffectedControl.Height != e.AffectedControl.PreferredSize.Height)
                {
                    e.AffectedControl.Height = e.AffectedControl.PreferredSize.Height;
                }
            }
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        #endregion

        #region Constructors & Destructors
        public eFileReview()
        {
            disableAutoSize = true;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
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
            this.lblLastReviewDate = new FWBS.OMS.UI.Windows.eFileReview.CustomLabel();
            this.dpDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // dpDate
            // 
            this.dpDate.MinDate = new System.DateTime(1773, 1, 1, 0, 0, 0, 0);
            // 
            // pnlCaptionContainer
            // 
            this.pnlCaptionContainer.Size = new System.Drawing.Size(150, 38);
            // 
            // lblLastReviewDate
            // 
            this.lblLastReviewDate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLastReviewDate.Location = new System.Drawing.Point(150, 25);
            this.lblLastReviewDate.Margin = new System.Windows.Forms.Padding(0);
            this.lblLastReviewDate.Name = "lblLastReviewDate";
            this.lblLastReviewDate.Size = new System.Drawing.Size(242, 13);
            this.lblLastReviewDate.TabIndex = 10;
            this.lblLastReviewDate.Text = "Last : 01/01/2002";
            this.lblLastReviewDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eFileReview
            // 
            this.AllowNull = false;
            this.Controls.Add(this.lblLastReviewDate);
            this.Name = "eFileReview";
            this.Size = new System.Drawing.Size(392, 38);
            this.Controls.SetChildIndex(this.pnlCaptionContainer, 0);
            this.Controls.SetChildIndex(this.dpDate, 0);
            this.Controls.SetChildIndex(this.lblLastReviewDate, 0);
            this.dpDate.ResumeLayout(false);
            this.dpDate.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties
        public override bool LockHeight { get; } = false;

        [Category("OMS Appearance")]
        [Browsable(false)]
        public new DateTimePickerLayout DateTimeLayout
        {
            get
            {
                return DateTimePickerLayout.dtpSameLine;
            }
            set
            {
            }
        }

        [Category("OMS Appearance")]
        [Browsable(false)]
        public new string DefaultTime
        {
            get
            {
                return "09:00:00";
            }
            set
            {
            }
        }

        [Category("OMS Appearance")]
        [Browsable(false)]
        [DefaultValue(false)]
        public new bool TimeEnabled
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Category("OMS Appearance")]
        [Browsable(false)]
        public new  bool TimeLabelVisible
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(TimeLayout.TwentyFourHour)]
        [Browsable(false)]
        public new TimeLayout TimeLayout
        {
            get
            {
                return TimeLayout.TwentyFourHour;
            }
            set
            {

            }
        }

        [Category("OMS Appearance")]
        [Browsable(false)]
        public new bool TimeVisible
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Category("OMS Appearance")]
        [Browsable(false)]
        public new int TimeWidth
        {
            get
            {
                return 43;
            }
            set
            {
            }
        }

        [Browsable(false)]
        public override bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }
        
        [Browsable(false)]
        public override CodeLookupDisplay CueText
        {
            get
            {
                return new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME);
            }
            set { }
        }
        #endregion

        #region Methods
        private void eFileReview_ParentChanged(object sender, System.EventArgs e)
        {
            if (this.Parent is EnquiryForm)
            {
                EnquiryForm enq = (EnquiryForm)Parent;
                OMSDocument doc = enq.Enquiry.Object as OMSDocument;
                if (doc != null)
                {
                    OMSFile file = doc.OMSFile;
                    dpDate.Visible = file.CurrentFileType.EnableFileReview;
                    //UTCFIX: DM - 30/11/06 - Make sure local time is displayed
                    ResourceItem res = Session.CurrentSession.Resources.GetResource("NEXTREV2", "Next: {0}", "");
                    if (file.ReviewDate.IsNull)
                        lblLastReviewDate.Text = String.Format(res.Text, "");
                    else
                        lblLastReviewDate.Text = String.Format(res.Text, (file.ReviewDate.Kind == DateTimeKind.Unspecified ? file.ReviewDate.ToShortDateString() : file.ReviewDate.ToLocalTime().ToShortDateString()));

                    if (file.CurrentFileType.EnableFileReview)
                    {
                        dpDate.MinDate = DateTime.Today;
                        dpDate.Tag = "ON";
                        ValidateDateAndTime(sender, e);
                        dpDate.Tag = null;
                    }
                    else
                    {
                        Value = DBNull.Value;
                        Text = Session.CurrentSession.Resources.GetResource("NOREVIEWDATE", "{Not Available}","").Text;
                    }
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            eFileReview_ParentChanged(this, e);
        }

        /// <summary>
        /// Manages that the control's height is not less than child controls height.
        /// </summary>
        protected override void ValidateCtrlSize()
        {
            if (omsDesignMode && lblLastReviewDate != null)
            {
                var managedHeight = DateVisible
                    ? dpDate.PreferredHeight + lblLastReviewDate.PreferredHeight
                    : lblLastReviewDate.PreferredHeight;
                if (this.Height < managedHeight)
                {
                    this.Height = managedHeight;
                }

                var managedWidth = lblLastReviewDate.PreferredWidth + 5;
                if (this.Width < managedWidth)
                {
                    this.Width = managedWidth;
                }
            }
        }

        protected override void ManageChildLayout()
        {
            base.ManageChildLayout();
            this.lblLastReviewDate.SendToBack();
        }
        #endregion
    }
}
