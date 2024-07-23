using System;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eWizardPageChanger.
    /// </summary>
    public class eWizardPageChanger : eXPComboBoxCodeLookup
	{
		#region Fields
		private EnquiryForm _parent = null;
		private string _value = "";
		#endregion

		#region Contructors & Dispose
		public eWizardPageChanger() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// comboBox1
			// 
			this.comboBox1.Name = "comboBox1";
			// 
			// eWizardPageChanger
			// 
			this.Name = "eWizardPageChanger";
			this.VisibleChanged += new System.EventHandler(this.eWizardPageChanger_VisibleChanged);
			this.ParentChanged += new System.EventHandler(this.eWizardPageChanger_ParentChanged);

		}
		#endregion

		private void eWizardPageChanger_ParentChanged(object sender, System.EventArgs e)
		{
			if (Parent is EnquiryForm)
			{
				_parent = Parent as EnquiryForm;
			}
            else if (Parent == null && _parent != null)
            {
                if (_parent.ActionNext != null)
                    _parent.ActionNext.EnabledChanged -= new EventHandler(ActionNext_EnabledChanged);
            }
		}

		#region Properties
		public override object Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_parent != null)
				{
					if (Convert.ToString(value) != _value)
					{
						_value = Convert.ToString(value);
						try
						{
							_parent.SetNextPage(Convert.ToString(value));
						}
						catch
						{
							try
							{
								_parent.SetNextPage(Convert.ToInt16(Convert.ToString(value).Substring(base.Type.Length)));
							}
							catch
							{
								try
								{
									_parent.SetNextPage(Convert.ToInt16(value));
								}
								catch
								{
									MessageBox.Show(Session.CurrentSession.Resources.GetMessage("PAGEERR","Page Failed to Change to '%1%'","",Convert.ToString(value)).Text);
								}
							}
						}
						ActionNext_EnabledChanged(this,EventArgs.Empty);
					}
				}
				base.Value = value;
			}
		}


		#endregion

		private void ActionNext_EnabledChanged(object sender, EventArgs e)
		{
			if (!this.omsDesignMode)
			{
				if (_parent != null && _value == "" && this.Visible)
				{
					if (_parent.ActionNext != null)
						_parent.ActionNext.Enabled = false;
				}
				else
				{
					if (_parent.ActionNext != null)
						_parent.ActionNext.Enabled = true;
				}
			}
		}

		private void eWizardPageChanger_VisibleChanged(object sender, System.EventArgs e)
		{
			
			if (_parent != null && _parent.ActionNext != null)
			{
				_parent.ActionNext.EnabledChanged -= new EventHandler(ActionNext_EnabledChanged);
				if (this.Visible)
					_parent.ActionNext.EnabledChanged += new EventHandler(ActionNext_EnabledChanged);
				ActionNext_EnabledChanged(sender,e);
			}
		}
	}
}
