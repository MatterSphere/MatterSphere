using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucAssociateRepeaterContainer.
    /// </summary>
    public class ucAssociateRepeaterContainer : ucSelectorRepeaterContainer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private OMSFile _omsfile;
        private EnquiryForm _parent;

        /// <summary>
        /// A flag to tell the control that the parent enwuiry form has been viewed within the wizard.
        /// </summary>
        private bool _beenSeen = false;

        public ucAssociateRepeaterContainer() : base()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            CaptionTop = false;

        }

        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool CaptionTop
        {
            get
            {
                return this.SelectorRepeaterType == typeof(ucAssociateVertSelector);
            }
            set
            {
                this.SelectorRepeaterType = value ? typeof(ucAssociateVertSelector) : typeof(ucAssociateSelector);
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Name = "ucAssociateRepeaterContainer";
            this.VisibleChanged += new System.EventHandler(this.ucAssociateRepeaterContainer_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.ucAssociateRepeaterContainer_ParentChanged);

        }
        #endregion

        private void ucAssociateRepeaterContainer_ParentChanged(object sender, System.EventArgs e)
        {
            if (Parent is FWBS.OMS.UI.Windows.EnquiryForm)
            {
                _parent = Parent as EnquiryForm;
                _parent.Finishing -=new CancelEventHandler(ucAssociateRepeaterContainer_Finishing);
                if (_parent.Enquiry.Object is OMSFile)
                {
                    _omsfile = _parent.Enquiry.Object as OMSFile;
                    _omsfile.Associates.CopyOnly = true;
                    _parent.Finishing +=new CancelEventHandler(ucAssociateRepeaterContainer_Finishing);

                    this.Clear();
                }
            }
            // If the Control is Remove Unhook
            if (Parent == null && _parent != null)
            {
                _parent.Finishing -=new CancelEventHandler(ucAssociateRepeaterContainer_Finishing);
                _omsfile = null;
                _parent = null;
            }

        }

        private void ucAssociateRepeaterContainer_Finishing(object sender, CancelEventArgs e)
        {
            try
            {
                if (_beenSeen)
                {
                    _omsfile.Associates.Clear();
                    _omsfile.AssociateExtendedData.Clear();
                    var defaultAssociate = (Associate) this.DefaultObject;
                    defaultAssociate.InitializeKey();
                    _omsfile.Associates.Add(defaultAssociate);
                    _omsfile.AssociateExtendedData.Add(defaultAssociate.Key.ToString(), defaultAssociate.ExtendedData);

                    foreach (Associate assoc in this.OtherObjects)
                    {
                        assoc.InitializeKey();
                        _omsfile.Associates.Add(assoc);
                        _omsfile.AssociateExtendedData.Add(assoc.Key.ToString(), assoc.ExtendedData);
                    }
                    this.ValidateObjects();
                }

            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                e.Cancel = true;
            }
        }

        private void ucAssociateRepeaterContainer_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.Visible && _omsfile != null && _parent != null)
            {
                _parent = Parent as EnquiryForm;
                _omsfile = _parent.Enquiry.Object as OMSFile;
                this.SetInfo(_omsfile.Client, _omsfile);
                if (_omsfile.Associates.Count > 0 && this.pnlContainer.Controls.Count == 0)
                {
                    string svalue = Convert.ToString(((FWBS.Common.UI.IBasicEnquiryControl2)_parent.GetControlByProperty("FileDescription")).Value);
                    pnlContainer.SuspendLayout();
                    this.Clear();
                    foreach (Associate assoc in _omsfile.Associates)
                    {
                        if (assoc.AssocHeading == "" && svalue != "" && assoc.AssocType == "CLIENT")
                            assoc.AssocHeading = svalue;
                        this.Add(assoc);
                    }
                    pnlContainer.ResumeLayout();
                }
                _beenSeen = true;
                this.Select(true);
            }
        }

        public override void Clear()
        {
            base.Clear();
            _beenSeen = false;
        }
    }
}
