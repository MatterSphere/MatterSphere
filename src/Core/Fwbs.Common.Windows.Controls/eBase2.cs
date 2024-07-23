using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A list selection enquiry control which generally holds a caption and a combo box.  
    /// This particular control can be used for picking items in a combo box style list or using it like a text box.
    /// </summary>
    /// //
    [Designer(typeof(FWBS.Common.UI.Windows.Design.eBaseDesigner))]
    public class eBase2 : System.Windows.Forms.Panel, IBasicEnquiryControl2, ICommandEnquiryControl, ISupportRightToLeft
    {
        #region Events

        /// <summary>
        /// The changed event is used to determine when a major change has happended within the
        /// user control.  This will tend to be used when the internal editing control has changed
        /// in some way or another.
        /// </summary>
        [Category("Action")]
        public virtual event EventHandler Changed;

        /// <summary>
        /// The Active changed event is used to determine when a active change has happended e.g a Text Changed Event
        /// within the user control.
        /// </summary>
        [Category("Action")]
        public virtual event EventHandler ActiveChanged;

        /// <summary>
        /// If the control has been marked with command arguments then this event will be raised
        /// when the command button alongside the control has been clicked, passing the command
        /// parameters to the form that is rendering the control.
        /// </summary>
        [Category("Action")]
        public virtual event EventHandler ExecuteCommand;


        #endregion

        #region Consts/Readonly

        public static readonly int DefaultCaptionWidth = 150;

        #endregion

        #region Fields

        /// <summary>
        /// Marks the editing control as required.
        /// </summary>
        private bool _required = false;

        /// <summary>
        /// Stores how wide the caption should be.
        /// </summary>
        protected int _captionWidth = DefaultCaptionWidth;

        /// <summary>
        /// Internal editing control.  This can be any control which derived controls decide to apply.
        /// </summary>
        protected Control _ctrl = null;

        /// <summary>
        /// Command button variable.
        /// </summary>
        protected Button _cmd = null;

        protected bool _isdirty = false;

        /// <summary>
        /// Display caption on top.
        /// </summary>
        protected bool _captionTop = false;

        /// <summary>
        /// Caption Label Height
        /// </summary>
        private int _captionTopHeight = 0;
        #endregion

        #region Contructors

        public eBase2()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
            this.UpdateStyles();
            InitializeComponent();
            TabIndex = 0;
            Height = 23;
            Width = 300;
        }

        private void InitializeComponent()
        {
            // 
            // eBase2
            //
            this.Click += new System.EventHandler(this.eBase2_Enter);
            this.EnabledChanged += new System.EventHandler(this.eBase2_EnabledChanged);
            this.GotFocus += new System.EventHandler(this.eBase2_Enter);
            this.Enter += new System.EventHandler(this.eBase2_Enter);
            this.Leave += new System.EventHandler(this.eBase2_Leave);
            this.ParentChanged += new System.EventHandler(this.eBase2_ParentChanged);
            this.DoubleBuffered = true;
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
                return false;
            }
        }

        [Browsable(true)]
        public object Control
        {
            get
            {
                return _ctrl;
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (base.Text == "") base.Text = " ";
                this.Invalidate();
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
                if (_ctrl != null)
                    return !_ctrl.Enabled;
                else
                    return !Enabled;
            }
            set
            {
                if (_ctrl != null)
                    _ctrl.Enabled = !value;
                else
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
                return _captionWidth;
            }
            set
            {
                _captionWidth = _captionTop ? 0 : value;
                SetRTL(this,_ctrl,_captionWidth,_cmd, _captionTop, _captionTopHeight);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or Set a bool for the Caption location - on the top or not
        /// </summary>
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
                _captionWidth = value ? 0 : DefaultCaptionWidth;
                _captionTopHeight = value ? CalcCaptionTopHeight(true) : 0;
                SetRTL(this, _ctrl, _captionWidth, _cmd, _captionTop, _captionTopHeight);
                ValidateCtrlSizeOnChildCtrlSize();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or Sets the controls value.  This must be overriden by derived classes to make their
        /// own representation of the value using the internal editing control..
        /// </summary>
        [DefaultValue(null)]
        public virtual object Value
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool IsDirty
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


        /// <summary>
        /// Executes the changed event.
        /// </summary>
        public void OnChanged()
        {
            if (Changed!= null && IsDirty)
            {
                IsDirty = false;
                Changed(this, EventArgs.Empty);
            }
        }

        public void OnActiveChanged()
        {
            IsDirty = true;
            if (ActiveChanged != null)
                ActiveChanged(this,EventArgs.Empty);
        }

        #endregion

        #region ICommandEnquiryControl Implementation

        [Category("Command Button")]
        [DefaultValue(false)]
        public bool Button
        {
            get
            {
                return (_cmd != null);
            }
            set
            {
                SetCommand(value);
            }
        }



        /// <summary>
        /// Gets the command button of the control so that the rendering form can use it to assign
        /// tooltips to it or manipulate it in other ways.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Category("Command")]
        public Button CommandButton
        {
            get
            {
                return _cmd;
            }
        }

        /// <summary>
        /// Assigns a command to the control.
        /// </summary>
        /// <param name="on">Swithches the command button on or off.</param>
        public void SetCommand(bool on)
        {
            if (on == false)
            {
                if (_cmd != null)
                {
                    this.Controls.Remove(_cmd);
                    _cmd.Dispose();
                    _cmd = null;
                    SetRTL(this, _ctrl, _captionWidth, _cmd, _captionTop, _captionTopHeight);
                }
            }
            else
            {
                if (_cmd == null)
                {
                    _cmd = new Button() { Text = "..." };
                    _cmd.FlatStyle = FlatStyle.System;
                    _cmd.AutoEllipsis = false;
                    _cmd.Size = Size.Empty;
                    _cmd.Margin = new Padding(0);
                    _cmd.Click += new EventHandler(this.CommandClick);
                    this.Controls.Add(_cmd);

                    int size = System.Math.Min(_cmd.PreferredSize.Height, (_ctrl ?? this).PreferredSize.Height) * 96 / (omsDesignMode || Font == DefaultFont ? 96 : DeviceDpi);
                    _cmd.Size = new Size(size, size);
                    SetRTL(this, _ctrl, _captionWidth, _cmd, _captionTop, _captionTopHeight);
                }
            }

        }


        /// <summary>
        /// Executes the execute command event.
        /// </summary>
        public void OnExecuteCommand()
        {
            if (ExecuteCommand != null)
                ExecuteCommand(this, EventArgs.Empty);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Raises the changed event within the base control.
        /// </summary>
        protected virtual void RaiseActiveChangedEvent(object sender, System.EventArgs e)
        {
            OnActiveChanged();
        }

        /// <summary>
        /// Raises the leave event within the base control.
        /// </summary>
        protected void RaiseLeaveEvent(object sender, System.EventArgs e)
        {
            fireleave = true;
            OnLeave(EventArgs.Empty);
            OnChanged();
        }

        protected bool fireleave = false;

        protected void RaiseLostFocusEvent(object sender, System.EventArgs e)
        {
            if (fireleave == false)
                RaiseLeaveEvent(sender, e);
            fireleave = false;
        }


        /// <summary>
        /// Captures the got focus event within the base control.
        /// </summary>
        protected void RaiseGotFocusEvent(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// When the user control gets focus then set the main editing control with the focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eBase2_Enter(object sender, System.EventArgs e)
        {
            if (_ctrl != null)
                _ctrl.Focus();
        }

        /// <summary>
        /// When the user control loses focus then remove the focus of the child control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eBase2_Leave(object sender, System.EventArgs e)
        {
            if (_ctrl != null)
                _ctrl.Refresh();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (_captionTop)
            {
                _captionTopHeight = CalcCaptionTopHeight(true);
                SetRTL(this, _ctrl, _captionWidth, _cmd, _captionTop, _captionTopHeight);
            }
            else
            {
                _captionTopHeight = 0;
            }

            ValidateCtrlSizeOnChildCtrlSize();

            if (omsDesignMode && _cmd != null)
            {
                Button = false;
                Button = true;
            }
        }

        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);
            if (this.Controls.Count > 0 && !(this.Controls[0] is Label))
            {
                this.Controls[0].TabStop = this.TabStop;
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            if (_captionTop)
            {
                _captionTopHeight = CalcCaptionTopHeight(false);
                SetRTL(this, _ctrl, _captionWidth, _cmd, _captionTop, _captionTopHeight);
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            try
            {
                MethodInfo shouldSerializeBackColorMethodInfo = GetType().GetMethod("ShouldSerializeBackColor", BindingFlags.Instance | BindingFlags.NonPublic);
                if (Convert.ToBoolean(shouldSerializeBackColorMethodInfo.Invoke(this, null)))
                {
                    Controls[0].BackColor = BackColor;
                }
            }
            catch
            { }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            try
            {
                MethodInfo shouldSerializeForeColorMethodInfo = GetType().GetMethod("ShouldSerializeForeColor", BindingFlags.Instance | BindingFlags.NonPublic);
                if (Convert.ToBoolean(shouldSerializeForeColorMethodInfo.Invoke(this, null)))
                {
                    Controls[0].ForeColor = ForeColor;
                }
            }
            catch
            { }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (factor.Height != 1 && (specified & BoundsSpecified.Height) != 0)
            {
                _captionTopHeight = Convert.ToInt32(_captionTopHeight * factor.Height);
            }
        }
        
        /// <summary>
        /// Capture the click of the command button then raise it in the form of the ExecuteCommand event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CommandClick(object sender, System.EventArgs e)
        {
            OnExecuteCommand();
        }


        /// <summary>
        /// Process the Key Mnenonic
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns></returns>
        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.Visible && System.Windows.Forms.Control.IsMnemonic(charCode, this.Text))
            {
                if (_ctrl != null)
                {
                    _ctrl.Focus();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Design Mode
        private bool _omsdesignmode = false;
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
                if (value)
                {
                    this.Paint += new System.Windows.Forms.PaintEventHandler(this.omsDesignMode_Paint);
                    this.Resize += new EventHandler(this.omsDesignMode_Resize);
                }
            }
        }

        protected virtual void omsDesignMode_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        }

        private void omsDesignMode_Resize(object sender, EventArgs e)
        {
            ValidateCtrlSizeOnChildCtrlSize();
            this.Invalidate();
        }
        #endregion

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintCaption(e, this, _ctrl, _captionWidth, _captionTop, _captionTopHeight);
        }

        public static void PaintCaption(System.Windows.Forms.PaintEventArgs e, Control parent, Control child, int captionWidth, bool captionTop = false, int captionTopHeight = 0)
        {
            if (parent.Text != "")
            {
                //Sets the Text Format Settings
                StringFormat sf = new StringFormat() { FormatFlags = StringFormatFlags.NoClip };
                if (parent.RightToLeft == RightToLeft.Yes)
                {
                    sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                }
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                
				//Calculate rectangle caption
                Rectangle pntarea;
                if (!captionTop)
                {
                    int y = (child == null) ? 0 : (child.Size.Height - child.ClientSize.Height) / 2;
                    int captionHeight = parent.Height - y;
                    captionWidth = parent.LogicalToDeviceUnits(captionWidth);
                    if (parent.RightToLeft == RightToLeft.Yes)
                        pntarea = new Rectangle(parent.Width - captionWidth, y, captionWidth, captionHeight);
                    else
                        pntarea = new Rectangle(0, y, captionWidth, captionHeight);
                }
                else
                {
                    pntarea = new Rectangle(0, 0, parent.Width, captionTopHeight);
                }
                
				//Draw caption string
                using (SolidBrush br = new SolidBrush(parent.Enabled ? parent.ForeColor : SystemColors.GrayText))
                {
                    e.Graphics.DrawString(parent.Text, parent.Font, br, pntarea, sf);
                }
            }
        }

        private void eBase2_ParentChanged(object sender, System.EventArgs e)
        {
            if (omsDesignMode && this.Controls.Count>0 && this.Controls[0] is Label == false)
            {
                this.Controls[0].Enabled = false;
                this.Controls[0].BackColor = Color.White;
                this.Controls[0].ForeColor = Color.Black;
            }
            else if (this.Controls.Count>0 && this.Controls[0] is Label == false)
            {
                this.Controls[0].TabStop = this.TabStop;
            }
        }

        private void eBase2_EnabledChanged(object sender, EventArgs e)
        {
            if (_ctrl != null)
            {
                _ctrl.BackColor = this.Enabled ? SystemColors.Window : SystemColors.ControlLight;
            }
            this.Invalidate();
        }

        /// <summary>
        /// Calculates caption top height in depends of font.
        /// </summary>
        /// <param name="isDeviceDpiRequired">Flag, should include device dpi recalculating</param>
        /// <returns>Caption top height</returns>
        private int CalcCaptionTopHeight(bool isDeviceDpiRequired)
        {
            using (var graphics = this.CreateGraphics())
            {
                return isDeviceDpiRequired
                    ? Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height * 96 / (omsDesignMode || Font == DefaultFont ? 96 : DeviceDpi)))
                    : Convert.ToInt32(System.Math.Ceiling(graphics.MeasureString("GgYy", Font).Height));
            }
        }

        /// <summary>
        /// Re-adjusts the internal controls to be right to left if set to RightToLeft.
        /// </summary>
        public virtual void SetRTL(Form parentform)
        {
            SetRTL(this,_ctrl,_captionWidth,_cmd, _captionTop, _captionTopHeight);
        }

        public static void SetRTL(Control ctrl, Control childCtrl, int captionWidth, Button cmdButton, bool captionTop = false, int captionTopHeight = 0)
        {
            ctrl.SuspendLayout();

            int cmdWidth = (cmdButton != null) ? cmdButton.Width : 0;

            if (cmdWidth != 0)
            {
                cmdButton.Top = captionTop ? captionTopHeight : 0;

                if (ctrl.RightToLeft == RightToLeft.Yes)
                {
                    cmdButton.Left = 0;
                    cmdButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                }
                else if (ctrl.RightToLeft == RightToLeft.No)
                {
                    cmdButton.Left = ctrl.Width - cmdWidth;
                    cmdButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                }
            }

            if (childCtrl != null)
            {
                childCtrl.Anchor = AnchorStyles.None;
                childCtrl.RightToLeft = ctrl.RightToLeft;

                if (captionTop)
                {
                    childCtrl.Left = (ctrl.RightToLeft == RightToLeft.Yes) ? cmdWidth : 0;
                    childCtrl.Top = captionTopHeight;
                    childCtrl.Width = ctrl.Width - cmdWidth;
                    childCtrl.Height = ctrl.Height - captionTopHeight;
                }
                else
                {
                    childCtrl.Left = (ctrl.RightToLeft == RightToLeft.Yes) ? cmdWidth : captionWidth;
                    childCtrl.Top = 0;
                    childCtrl.Width = ctrl.Width - captionWidth - cmdWidth;
                    childCtrl.Height = ctrl.Height;
                }

                childCtrl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            ctrl.ResumeLayout(false);
        }

        /// <summary>
        /// Calculates preffered control height taking into account caption alignment
        /// </summary>
        [Browsable(false)]
        public virtual int PreferredHeight
        {
            get
            {
                return _ctrl != null 
                    ? _ctrl.PreferredSize.Height + (_captionTop ? _captionTopHeight : 0)
                    : 0;
            }
        }

        /// <summary>
        /// Validates this panel-control size on _ctrl size which is the main control of the panel.
        /// </summary>
        protected void ValidateCtrlSizeOnChildCtrlSize()
        {
            if (omsDesignMode)
            {
                if (Height < PreferredHeight)
                {
                    Height = PreferredHeight;
                }
            }
        }
    }
}
