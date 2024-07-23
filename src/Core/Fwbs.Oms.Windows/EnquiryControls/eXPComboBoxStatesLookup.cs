using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    public class eXPComboBoxStatesLookup : FWBS.Common.UI.Windows.eXPComboBox
	{
        public eXPComboBoxStatesLookup()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            CountryControlName = "cboCountry";
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // eXPComboBoxStatesLookup
            // 
            this.Name = "eXPComboBoxStatesLookup";
            this.ParentChanged += new System.EventHandler(this.eXPComboBoxCodeLookup_ParentChanged);
            this.ResumeLayout(false);
		}

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            // The other process will work but it does not set it when the enter key is pressed. So 
            // this forces it to Press Enter just once to select. But the tab still works as normal
            StopCancelAndReturn();
        }
    	#endregion

        #region AddItem Stopper
        public override void AddItem(ArrayList dataArrayList, string valueMember, string displayMember)
        {
        }

        public override void AddItem(DataTable dataTable, string valueMember, string displayMember)
        {
        }

        public override void AddItem(DataTable dataTable)
        {
        }

        public override void AddItem(DataView dataView)
        {
        }

        public override void AddItem(DataView dataView, string valueMember, string displayMember)
        {
        }

        public override void AddItem(object Value, string displayText)
        {
        }
        #endregion

        private FormRendererBase parentfrb;
        private IBasicEnquiryControl2 _countrycombo;
        
        private void eXPComboBoxCodeLookup_ParentChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.comboBox1.DataSourceChanged -= new EventHandler(eXPComboBoxCodeLookup_ParentChanged);
                //
                // Use Codelookup as a Data Source
                //
                if (Parent != null && DataSource == null)
                {
                    if (Session.CurrentSession.IsLoggedIn)
                    {
                        parentfrb = Parent as FormRendererBase;
                        if (parentfrb != null)
                        {
                            _countrycombo = parentfrb.GetIBasicEnquiryControl2(CountryControlName, EnquiryControlMissing.None);
                            if (_countrycombo == null)
                            {
                                SetError(Session.CurrentSession.Resources.GetResource(
                                    "ERRMSGCNTFND",
                                    "[%1%] control cannot be found ...",
                                    "",
                                    CountryControlName).Text);
                                return;
                            }

                            UpdateSource();
                            _countrycombo.ActiveChanged += new EventHandler(Country_ActiveChanged);
                        }
                    }
                }
                else if (Parent == null)
                {
                    if (_countrycombo != null)
                    {
                        _countrycombo.ActiveChanged -= new EventHandler(Country_ActiveChanged);
                    }
                }
            }
            finally
            {
                this.comboBox1.DataSourceChanged += new EventHandler(eXPComboBoxCodeLookup_ParentChanged);
            }
		}

        private string countrycontrolname;

        [Category("OMS")]
        [DefaultValue("cboCountry")]
        public string CountryControlName
        {
            get
            {
                return countrycontrolname;
            }
            set
            {
                countrycontrolname = value;
            }
        }


        public override bool omsDesignMode
        {
            get
            {
                return !comboBox1.Enabled;
            }
            set
            {
                comboBox1.Enabled = !value;
                if (value)
                {
                    SetError("");
                }
            }
        }

        private void UpdateSource()
        {
            if (omsDesignMode) 
                return;
            var backupvalue = this.Value;
            base.BeginUpdate();
            DataTable dt = CodeLookup.GetLookups(Convert.ToString(_countrycombo.Value));
            dt.DefaultView.Sort = "cddesc";
            if (dt != null && dt.Rows.Count > 0)
            {
                base.DataSource = dt;
                base.DisplayMember = "cddesc";
                base.ValueMember = "cddesc";
                this.comboBox1.ForeColor = SystemColors.ControlText;
            }
            else
            {
                if (!this.Visible)
                    return;
                SetError(Session.CurrentSession.Resources.GetResource(
                    "ERRMSGCDLMISS",
                    "Country Code [''%1%''] Code Lookup Missing ...",
                    "",
                    Convert.ToString(_countrycombo.Value)).Text);
            }
            base.EndUpdate();
            this.Value = backupvalue;
        }

        private void SetError(string text)
        {
            base.DataSource = null;
            base.DisplayMember = "";
            base.ValueMember = "";
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add(text);
            this.comboBox1.SelectedIndex = 0;
            this.comboBox1.ForeColor = Color.Red;
        }

        void Country_ActiveChanged(object sender, EventArgs e)
        {
            if (parentfrb == null)
                return;

            var backup = this.Value;
            UpdateSource();
            var dt = this.DataSource as DataTable;
            if (dt != null)
            {
                DataView dv = new DataView(dt, "cddesc = '" + Convert.ToString(backup) + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count == 0)
                    comboBox1.SelectedIndex = -1;
            }
        }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new System.Windows.Forms.ComboBox.ObjectCollection Items
		{
			get
			{
				return base.Items;
			}
			set
			{
	
			}
		}
	}
}
