using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    class TitleBarButtonsControl : UserControl
    {
        enum SystemCommand : uint
        {
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_CLOSE = 0xF060,
            SC_RESTORE = 0xF120
        }

        private static readonly Dictionary<SystemCommand, string> SystemCommands = new Dictionary<SystemCommand, string>();

        private static void FillSystemCommands(IntPtr hWnd)
        {
            if (SystemCommands.Count == 0)
            {
                IntPtr hMenu = GetSystemMenu(hWnd, false);
                if (hMenu != IntPtr.Zero)
                {
                    StringBuilder sb = new StringBuilder(64);
                    foreach (SystemCommand command in Enum.GetValues(typeof(SystemCommand)))
                    {
                        if (GetMenuString(hMenu, (uint)command, sb, sb.Capacity, 0) > 0)
                        {
                            sb.Replace("&", "");
                            SystemCommands.Add(command, sb.ToString().Split('\t')[0]);
                        }
                    }
                }
            }
        }

        #region TitleBarButton

        class TitleBarButton : Button
        {
            private SystemCommand _command;

            [Description("The type of system command requested."), Category("Behavior")]
            [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
            public SystemCommand Command
            {
                get
                {
                    return _command;
                }
                set
                {
                    _command = value;
                    switch (_command)
                    {
                        case SystemCommand.SC_MINIMIZE:
                            base.Text = "0";
                            break;
                        case SystemCommand.SC_MAXIMIZE:
                            base.Text = "1";
                            break;
                        case SystemCommand.SC_RESTORE:
                            base.Text = "2";
                            break;
                        case SystemCommand.SC_CLOSE:
                            base.Text = "r";
                            break;
                    }
                    Update();
                }
            }

            [Browsable(false)]
            public override string Text
            {
                get { return base.Text; }
                set { }
            }

            public TitleBarButton()
            {
                SetStyle(ControlStyles.Selectable, false);
            }
        }

        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlButtons;
        private TitleBarButton btnClose;
        private TitleBarButton btnMaximize;
        private TitleBarButton btnMinimize;
        private ToolTip toolTip;
        private FormWindowState _windowState;

        public event EventHandler FormWindowStateChanged;

        public TitleBarButtonsControl()
        {
            InitializeComponent();
            this.TabStop = false;
            this.BackColor = pnlButtons.BackColor;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnMinimize = new FWBS.OMS.UI.Windows.TitleBarButtonsControl.TitleBarButton();
            this.btnMaximize = new FWBS.OMS.UI.Windows.TitleBarButtonsControl.TitleBarButton();
            this.btnClose = new FWBS.OMS.UI.Windows.TitleBarButtonsControl.TitleBarButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(17)))), ((int)(((byte)(76)))));
            this.pnlButtons.Controls.Add(this.btnMinimize);
            this.pnlButtons.Controls.Add(this.btnMaximize);
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.pnlButtons.ForeColor = System.Drawing.Color.White;
            this.pnlButtons.Location = new System.Drawing.Point(4, 0);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(142, 30);
            this.pnlButtons.TabIndex = 0;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Command = FWBS.OMS.UI.Windows.TitleBarButtonsControl.SystemCommand.SC_MINIMIZE;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(120)))), ((int)(((byte)(193)))));
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(1)))), ((int)(((byte)(198)))));
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(7, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnMinimize.Size = new System.Drawing.Size(45, 30);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Text = "0";
            this.btnMinimize.UseMnemonic = false;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.TitleBarButton_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Command = FWBS.OMS.UI.Windows.TitleBarButtonsControl.SystemCommand.SC_MAXIMIZE;
            this.btnMaximize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(120)))), ((int)(((byte)(193)))));
            this.btnMaximize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(1)))), ((int)(((byte)(198)))));
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(52, 0);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnMaximize.Size = new System.Drawing.Size(45, 30);
            this.btnMaximize.TabIndex = 2;
            this.btnMaximize.TabStop = false;
            this.btnMaximize.Text = "1";
            this.btnMaximize.UseMnemonic = false;
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.TitleBarButton_Click);
            // 
            // btnClose
            // 
            this.btnClose.Command = FWBS.OMS.UI.Windows.TitleBarButtonsControl.SystemCommand.SC_CLOSE;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(112)))), ((int)(((byte)(122)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(97, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnClose.Size = new System.Drawing.Size(45, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "r";
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.UseMnemonic = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.TitleBarButton_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 4000;
            this.toolTip.InitialDelay = 1200;
            this.toolTip.ReshowDelay = 100;
            // 
            // TitleBarButtonsControl
            // 
            this.Controls.Add(this.pnlButtons);
            this.Name = "TitleBarButtonsControl";
            this.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Size = new System.Drawing.Size(146, 48);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public bool SnapToForm(int captionHeight)
        {
            Form form = Parent as Form;
            bool result = (form != null) && form.ControlBox;
            if (result)
            {
                _windowState = form.WindowState;
                form.Resize -= Form_Resize;
                form.StyleChanged -= Form_StyleChanged;
                SuspendLayout();

                bool isMinMaxVisible = (form.MinimizeBox || form.MaximizeBox);
                btnMaximize.Command = (_windowState == FormWindowState.Maximized) ? SystemCommand.SC_RESTORE : SystemCommand.SC_MAXIMIZE;
                btnMaximize.Enabled = form.MaximizeBox;
                btnMinimize.Enabled = form.MinimizeBox;
                btnMinimize.Visible = btnMaximize.Visible = isMinMaxVisible;

                int pnlButtonsWidth = (isMinMaxVisible ? (btnClose.Right - btnMinimize.Left) : btnClose.Width) + LogicalToDeviceUnits(4);

                this.Anchor = AnchorStyles.None;
                this.Bounds = new Rectangle(form.ClientRectangle.Right - pnlButtonsWidth - 1, 1, pnlButtonsWidth, Math.Max(captionHeight, pnlButtons.Height) - 1);
                this.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.Enabled = true;
                this.Visible = true;

                FillSystemCommands(form.Handle);
                foreach (TitleBarButton button in pnlButtons.Controls)
                {
                    if (SystemCommands.ContainsKey(button.Command))
                    {
                        button.Tag = SystemCommands[button.Command];
                        toolTip.SetToolTip(button, (string)button.Tag);
                    }
                }

                ResumeLayout();
                Invalidate(true);
                form.Resize += Form_Resize;
                form.StyleChanged += Form_StyleChanged;
            }
            else
            {
                Enabled = false;
                Visible = false;
            }
            return result;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            FormWindowState windowState = ((Form)sender).WindowState;
            if (windowState != FormWindowState.Minimized && windowState != _windowState)
            {
                _windowState = windowState;
                btnMaximize.Command = (windowState == FormWindowState.Maximized) ? SystemCommand.SC_RESTORE : SystemCommand.SC_MAXIMIZE;
                if (SystemCommands.ContainsKey(btnMaximize.Command))
                {
                    btnMaximize.Tag = SystemCommands[btnMaximize.Command];
                    toolTip.SetToolTip(btnMaximize, (string)btnMaximize.Tag);
                }

                FormWindowStateChanged?.Invoke(sender, EventArgs.Empty);
            }
        }

        private void Form_StyleChanged(object sender, EventArgs e)
        {
            btnMaximize.Enabled = ((Form)sender).MaximizeBox;
            btnMinimize.Enabled = ((Form)sender).MinimizeBox;
        }

        private void TitleBarButton_Click(object sender, EventArgs e)
        {
            Form form = TopLevelControl as Form;
            if (form != null)
            {
                switch (((TitleBarButton)sender).Command)
                {
                    case SystemCommand.SC_MINIMIZE:
                        form.WindowState = FormWindowState.Minimized;
                        break;
                    case SystemCommand.SC_MAXIMIZE:
                        form.WindowState = FormWindowState.Maximized;
                        break;
                    case SystemCommand.SC_RESTORE:
                        form.WindowState = FormWindowState.Normal;
                        break;
                    case SystemCommand.SC_CLOSE:
                        form.Close();
                        break;
                }
            }
        }

        #region Native Methods

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetMenuString(IntPtr hMenu, uint uIDItem, StringBuilder lpString, int nMaxCount, uint uFlag);

        #endregion
    }
}
