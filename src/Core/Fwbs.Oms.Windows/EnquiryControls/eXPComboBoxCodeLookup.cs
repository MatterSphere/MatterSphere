using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class eXPComboBoxCodeLookup : FWBS.Common.UI.Windows.eXPComboBox
	{
		private bool _allowcreate = false;
		private bool _addnotset = false;
		private string _notsetcode = "NOTSET";
		private string _notsettype = "RESOURCE";
		private bool _teminologyparse = false;

		public eXPComboBoxCodeLookup()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            // eXPComboBoxCodeLookup
            // 
            this.Name = "eXPComboBoxCodeLookup";
            this.DoesNotExist += new System.ComponentModel.CancelEventHandler(this.eXPComboBoxCodeLookup_DoesNotExist);
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

        public override void AddItem(ArrayList dataArrayList, string valueMember, string displayMember)
        {
            if (string.IsNullOrEmpty(Type))
                base.AddItem(dataArrayList, valueMember, displayMember);
        }

        public override void AddItem(DataTable dataTable, string valueMember, string displayMember)
        {
            if (string.IsNullOrEmpty(Type))
                base.AddItem(dataTable, valueMember, displayMember);
        }

        public override void AddItem(DataTable dataTable)
        {
            if (string.IsNullOrEmpty(Type))
                base.AddItem(dataTable);
        }

        public override void AddItem(DataView dataView)
        {
            if (string.IsNullOrEmpty(Type))
                base.AddItem(dataView);
        }

        public override void AddItem(DataView dataView, string valueMember, string displayMember)
        {
            if (string.IsNullOrEmpty(Type))
                base.AddItem(dataView, valueMember, displayMember);
        }

        public override void AddItem(object Value, string displayText)
        {
            if (string.IsNullOrEmpty(Type))
                base.AddItem(Value, displayText);
        }

		/// <summary>
		/// Gets or Set a string for the Code Lookup Type
		/// </summary>
		private string _type = "";

        private void eXPComboBoxCodeLookup_DoesNotExist(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (comboBox1.Text != "" && _allowcreate && _type != "")
                {
                    if (MessageBox.Show(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("RUSURECRT", "Text '%1%' does not exist.    Do you want to Create?", "", false, comboBox1.Text), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string _code;
                        _code = comboBox1.Text.GetHashCode().ToString();
                        string _desc = comboBox1.Text;
                        CodeLookup.Create(_type, _code, comboBox1.Text, "", CodeLookup.DefaultCulture, false, true, true);
                        Session.CurrentSession.ClearCache();
                        Session.CurrentSession.ConfigureCache();
                        this.comboBox1.DataSourceChanged -= new EventHandler(eXPComboBoxCodeLookup_ParentChanged);
                        this.DataSource = null;
                        this.eXPComboBoxCodeLookup_ParentChanged(this, EventArgs.Empty);
                        comboBox1.Text = _desc;
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

		private void eXPComboBoxCodeLookup_ParentChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.comboBox1.DataSourceChanged -= new EventHandler(eXPComboBoxCodeLookup_ParentChanged);
                //
                // Use Codelookup as a Data Source
                //
                if (Parent != null && _type != "" && DataSource == null)
                {
                    if (Session.CurrentSession.IsLoggedIn)
                    {
                        base.BeginUpdate();
                        DataTable dt = CodeLookup.GetLookups(_type);
                        if (_addnotset)
                        {
                            dt.Columns["cdCode"].AllowDBNull = true;
                            DataRow ndr = dt.NewRow();
                            ndr["cdcode"] = DBNull.Value;
                            if (_notsettype == "RESOURCE")
                                ndr["cddesc"] = Session.CurrentSession.Resources.GetResource(_notsetcode, "(not specified)", "").Text;
                            else
                                ndr["cddesc"] = CodeLookup.GetLookup(_notsettype, _notsetcode);
                            dt.Rows.Add(ndr);
                        }

                        if (_teminologyparse)
                        {
                            DataView tp = new DataView(dt, "cddesc like '%[%]%'", "", DataViewRowState.CurrentRows);
                            for (int ctr = tp.Count - 1; ctr >= 0; ctr--)
                                tp[ctr]["cddesc"] = Session.CurrentSession.Terminology.Parse(Convert.ToString(tp[ctr]["cddesc"]), true);
                            dt.AcceptChanges();
                        }

                        dt.DefaultView.Sort = "cddesc";
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            base.DataSource = dt;
                            base.DisplayMember = "cddesc";
                            base.ValueMember = "cdcode";
                            comboBox1.Enabled = !this.ReadOnly;
                        }
                        else
                            ErrorBox.Show(ParentForm, new Exception("Code Lookup Type [" + _type + "] does not exist..."));
                        base.EndUpdate();
                    }
                }
                //
                // Use DataSouce property as the DataSouce and only if AddNotSet is set to True
                //
                else if (Parent != null && _type == "" && DataSource != null && _addnotset)
                {
                    base.BeginUpdate();
                    try
                    {
                        DataTable dt = this.comboBox1.DataSource as DataTable;
                        if (dt == null)
                        {
                            DataView dv = this.comboBox1.DataSource as DataView;
                            if (dv != null) dt = dv.Table;
                        }
                        if (dt != null)
                        {
                            string text = CodeLookup.GetLookup(_notsettype, _notsetcode);
                            DataView filter = new DataView(dt);
                            filter.RowFilter = "[" + this.ValueMember + "] is null AND " + this.DisplayMember + " = '" + text.Replace("'", "''") + "'";
                            if (filter.Count == 0)
                            {
                                dt.Columns[this.ValueMember].AllowDBNull = true;
                                DataRow ndr = dt.NewRow();
                                ndr[0] = DBNull.Value;
                                ndr[this.DisplayMember] = text;
                                dt.Rows.InsertAt(ndr, 0);
                            }
                        }

                        if (_teminologyparse)
                        {
                            DataView tp = new DataView(dt, "cddesc like '%[%]%'", "", DataViewRowState.CurrentRows);
                            for (int ctr = tp.Count - 1; ctr >= 0; ctr--)
                                tp[ctr]["cddesc"] = Session.CurrentSession.Terminology.Parse(Convert.ToString(tp[ctr]["cddesc"]), true);
                            dt.AcceptChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                    }
                    base.EndUpdate();
                }
            }
            finally
            {
                this.comboBox1.DataSourceChanged += new EventHandler(eXPComboBoxCodeLookup_ParentChanged);
            }
		}

		[Category("OMS")]
		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (_type != value)
				{
					_type = value;
                    if (String.IsNullOrEmpty(_type))
                        _allowcreate = false;
					eXPComboBoxCodeLookup_ParentChanged(this,EventArgs.Empty);
				}
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

		[DefaultValue(false)]
		[Category("OMS")]
		[Description("Allow Creation of Code Lookups")]
		public bool AllowCreate
		{
			get
			{
				return _allowcreate;
			}
			set
			{
				_allowcreate = value;
			}
		}
	
		[Category("OMS")]
		[Description("Add a Not Set Row to the Code Lookups")]
		[DefaultValue(false)]
		public bool AddNotSet
		{
			get
			{
				return _addnotset;
			}
			set
			{
				_addnotset = value;
			}
		}

		[Category("OMS")]
		[Description("The Not Set Code Lookup Code")]
		[DefaultValue("NOTSET")]
		public string NotSetCode
		{
			get
			{
				return _notsetcode;
			}
			set
			{
				_notsetcode = value;
			}
		}

		[Category("OMS")]
		[Description("The Not Set Code Lookup Type")]
		[DefaultValue("RESOURCE")]
		public string NotSetType
		{
			get
			{
				return _notsettype;
			}
			set
			{
                if (_type != "")
                {
                    _notsettype = value;
                }
			}
		}

		[Category("OMS")]
		[Description("Parse the Description through the OMS Terminolgy Parser e.g. %FILE% to File")]
		[DefaultValue(false)]
		public bool TerminologyParse
		{
			get
			{
				return _teminologyparse;
			}
			set
			{
				_teminologyparse = value;
			}
		}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_type == "" && this.DataSource == null)
                comboBox1.Enabled = false;
        }
	}
}