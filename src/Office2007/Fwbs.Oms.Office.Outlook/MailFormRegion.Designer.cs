namespace Fwbs.Oms.Office.Outlook
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class MailFormRegion : Microsoft.Office.Tools.Outlook.FormRegionBase
    {
        public MailFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.InitializeComponent();
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
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
            this.ucOMSTypeEmbeded1 = new FWBS.OMS.UI.Windows.ucOMSTypeEmbeded();
            this.SuspendLayout();
            // 
            // ucOMSTypeEmbeded1
            // 
            this.ucOMSTypeEmbeded1.AlertsVisible = true;
            this.ucOMSTypeEmbeded1.AutoLoad = true;
            this.ucOMSTypeEmbeded1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ucOMSTypeEmbeded1.BackButtonVisible = true;
            this.ucOMSTypeEmbeded1.CommandCentreButtonVisible = true;
            this.ucOMSTypeEmbeded1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucOMSTypeEmbeded1.InfoButtonVisible = true;
            this.ucOMSTypeEmbeded1.InfoPanelVisible = true;
            this.ucOMSTypeEmbeded1.LeftToolBarVisible = true;
            this.ucOMSTypeEmbeded1.Location = new System.Drawing.Point(0, 0);
            this.ucOMSTypeEmbeded1.Name = "ucOMSTypeEmbeded1";
            this.ucOMSTypeEmbeded1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ucOMSTypeEmbeded1.RefreshButtonVisible = true;
            this.ucOMSTypeEmbeded1.RightToolBarVisible = true;
            this.ucOMSTypeEmbeded1.SaveButtonVisible = true;
            this.ucOMSTypeEmbeded1.SearchButtonVisible = true;
            this.ucOMSTypeEmbeded1.Size = new System.Drawing.Size(150, 150);
            this.ucOMSTypeEmbeded1.StatusBarVisible = true;
            this.ucOMSTypeEmbeded1.TabIndex = 0;
            this.ucOMSTypeEmbeded1.TabPosition = System.Windows.Forms.TabAlignment.Top;
            this.ucOMSTypeEmbeded1.ToolBarVisible = true;
            // 
            // MailFormRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.ucOMSTypeEmbeded1);
            this.Name = "MailFormRegion";
            this.FormRegionShowing += new System.EventHandler(this.MailFormRegion_FormRegionShowing);
            this.FormRegionClosed += new System.EventHandler(this.MailFormRegion_FormRegionClosed);
            this.Click += new System.EventHandler(this.MailFormRegion_Click);
            this.ResumeLayout(false);

        }

        #endregion

        #region Form Region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private static void InitializeManifest(Microsoft.Office.Tools.Outlook.FormRegionManifest manifest)
        {
            manifest.FormRegionName = "Matter Information";

        }

        #endregion

        private FWBS.OMS.UI.Windows.ucOMSTypeEmbeded ucOMSTypeEmbeded1;


        public partial class MailFormRegionFactory : Microsoft.Office.Tools.Outlook.IFormRegionFactory
        {
           public event Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler FormRegionInitializing;

            private Microsoft.Office.Tools.Outlook.FormRegionManifest _Manifest;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MailFormRegionFactory()
            {
                this._Manifest = Globals.Factory.CreateFormRegionManifest();
                MailFormRegion.InitializeManifest(this._Manifest);
                this.FormRegionInitializing += new Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler(this.MailFormRegionFactory_FormRegionInitializing);
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public Microsoft.Office.Tools.Outlook.FormRegionManifest Manifest
            {
                get
                {
                    return this._Manifest;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.IFormRegion Microsoft.Office.Tools.Outlook.IFormRegionFactory.CreateFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            {
                MailFormRegion form = new MailFormRegion(formRegion);
                form.Factory = this;
                return form;
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            byte[] Microsoft.Office.Tools.Outlook.IFormRegionFactory.GetFormRegionStorage(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                throw new System.NotSupportedException();
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            bool Microsoft.Office.Tools.Outlook.IFormRegionFactory.IsDisplayedForItem(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                if (this.FormRegionInitializing != null)
                {
                    Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs cancelArgs = Globals.Factory.CreateFormRegionInitializingEventArgs(outlookItem, formRegionMode, formRegionSize, false);
                    this.FormRegionInitializing(this, cancelArgs);
                    return !cancelArgs.Cancel;
                }
                else
                {
                    return true;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.FormRegionKindConstants Microsoft.Office.Tools.Outlook.IFormRegionFactory.Kind
            {
                get
                {
                    return Microsoft.Office.Tools.Outlook.FormRegionKindConstants.WindowsForms;
                }
            }
        
        }
    }

    partial class WindowFormRegionCollection
    {
        internal MailFormRegion MailFormRegion
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.GetType() == typeof(MailFormRegion))
                        return (MailFormRegion)item;
                }
                return null;
            }
        }
    }
}
