using System.ComponentModel;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.SourceEngine;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmDeleteDesigner.
    /// </summary>
    public class frmDeleteDesigner : FWBS.OMS.UI.Windows.BaseForm
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        protected ResourceLookup resourceLookup1;
        private System.Windows.Forms.Panel pnlButtons;
        private IContainer components;

		public frmDeleteDesigner(ButtonActions Action, string Value, SearchListEditor Parent)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Text = Action.ToString() + " Designer";
			if (Action == ButtonActions.Delete)
				propertyGrid1.SelectedObject = new DeleteBuilder(Value);
			if (Action == ButtonActions.TrashDelete)
				propertyGrid1.SelectedObject = new TrashDeleteBuilder(Value, Parent);
			if (Action == ButtonActions.Restore)
				propertyGrid1.SelectedObject = new RestoreBuilder(Value, Parent);
			if (Action == ButtonActions.ViewActive || Action == ButtonActions.ViewTrash)
				propertyGrid1.SelectedObject = new ActiveTrashBuilder(Value, Parent);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CommandsForeColor = System.Drawing.Color.Black;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(8, 8);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(255, 302);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(8, 0);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(8, 30);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(263, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(84, 302);
            this.pnlButtons.TabIndex = 3;
            // 
            // frmDeleteDesigner
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(355, 318);
            this.ControlBox = false;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.pnlButtons);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmDlteDsignr", "Delete Designer", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeleteDesigner";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Designer";
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public string Output
		{
			get
			{
				if (propertyGrid1.SelectedObject is DeleteBuilder)
				{
					DeleteBuilder del = (DeleteBuilder)propertyGrid1.SelectedObject;
					if (del.DeleteMessage.Code != "" || del.ErrorMessage.Code != "" || del.OverrideSelect.Call != "")
					{
						FWBS.Common.ConfigSetting serial = new FWBS.Common.ConfigSetting("<config></config>");
						if (del.ErrorMessage.Code != "")
							serial.SetSetting("message","errorMessage",del.ErrorMessage.Code);
						if (del.DeleteMessage.Code != "")
							serial.SetSetting("message","deleteMessage",del.DeleteMessage.Code);
						if (del.OverrideSelect.Call != "")
						{
							serial.SetSetting("dataBuilder","call",del.OverrideSelect.Call);
							if (del.OverrideSelect.Source != "")
								serial.SetSetting("dataBuilder","source",del.OverrideSelect.Source);
							serial.SetSetting("dataBuilder","sourceType",del.OverrideSelect.SourceType.ToString());
						}
						return serial.DocObject.OuterXml;
					}
					else
						return "";
				}
				else if (propertyGrid1.SelectedObject is TrashDeleteBuilder)
				{
					TrashDeleteBuilder del = (TrashDeleteBuilder)propertyGrid1.SelectedObject;
					if (del.DeleteMessage.Code != "" || del.ErrorMessage.Code != "" || del.Fieldname != "" )
					{
						FWBS.Common.ConfigSetting serial = new FWBS.Common.ConfigSetting("<config></config>");
						if (del.ErrorMessage.Code != "")
							serial.SetSetting("message","errorMessage",del.ErrorMessage.Code);
						if (del.DeleteMessage.Code != "")
							serial.SetSetting("message","deleteMessage",del.DeleteMessage.Code);
						if (del.Fieldname != "")
							serial.SetSetting("trashCan","fieldname",del.Fieldname);
						if (del.ChangeValue != "")
							serial.SetSetting("trashCan","changeValue",del.ChangeValue);
						if (del.OverrideSelect.Call != "")
						{
							serial.SetSetting("dataBuilder","call",del.OverrideSelect.Call);
							if (del.OverrideSelect.Source != "")
								serial.SetSetting("dataBuilder","source",del.OverrideSelect.Source);
							serial.SetSetting("dataBuilder","sourceType",del.OverrideSelect.SourceType.ToString());
						}
						return serial.DocObject.OuterXml;
					}
					else
						return "";
				}
				else
				{
					ActiveTrashBuilder del = (ActiveTrashBuilder)propertyGrid1.SelectedObject;
					if (del.Fieldname != "" || del.FieldEquals != "")
					{
						FWBS.Common.ConfigSetting serial = new FWBS.Common.ConfigSetting("<config></config>");
						if (del.Fieldname != "") serial.SetSetting("trashCan","fieldname",del.Fieldname);
						if (del.FieldEquals!= "") serial.SetSetting("trashCan","changeValue",del.FieldEquals);
						return serial.DocObject.OuterXml;
					}
					else
						return "";
				}
			}
		}

	}

	public class ActiveTrashBuilder : LookupTypeDescriptor
	{
		private string _trashcanfield = "";
		private string _changevalue = "";
		private SearchListEditor _parent = null;

		public ActiveTrashBuilder(string Value,SearchListEditor Parent)
		{
			FWBS.Common.ConfigSetting serial = new FWBS.Common.ConfigSetting(Value);
			this.Fieldname = serial.GetSetting("trashCan","fieldname",this.Fieldname);
			this.FieldEquals = serial.GetSetting("trashCan","changeValue",this.FieldEquals);
			_parent = Parent;
		}
		
		[LocCategory("Trash")]
		[Lookup("DDFieldName")]
		[TypeConverter(typeof(FieldMappingTypeEditor))]
		public string Fieldname
		{
			get
			{
				return _trashcanfield;
			}
			set
			{
				_trashcanfield = value;
			}
		}

		[LocCategory("Trash")]
		public string FieldEquals
		{
			get
			{
				return _changevalue;
			}
			set
			{
				_changevalue = value;
			}
		}

		[Browsable(false)]
		public SearchListEditor Parent
		{
			get
			{
				return _parent;
			}
		}

	}
	
	public class DeleteBuilder : LookupTypeDescriptor
	{
		private CodeLookupDisplay _deletemessage = new CodeLookupDisplay("RESOURCE");
		private DataBuilder _overrideaselect = new DataBuilder();
		private CodeLookupDisplay _onerrormessage = new CodeLookupDisplay("RESOURCE");
		
		public DeleteBuilder(string Value)
		{
			FWBS.Common.ConfigSetting serial = new FWBS.Common.ConfigSetting(Value);
			this.ErrorMessage.Code = serial.GetSetting("message","errorMessage",this.ErrorMessage.Code);
			this.DeleteMessage.Code = serial.GetSetting("message","deleteMessage",this.DeleteMessage.Code);
			this.OverrideSelect.Call = serial.GetSetting("dataBuilder","call",this.OverrideSelect.Call);
			this.OverrideSelect.Source = serial.GetSetting("dataBuilder","source",this.OverrideSelect.Source);
			this.OverrideSelect.SourceType = (SourceType)FWBS.Common.ConvertDef.ToEnum(serial.GetSetting("dataBuilder","sourceType",this.OverrideSelect.SourceType.ToString()),SourceType.OMS);
		}

		[LocCategory("Messages")]
		[CodeLookupSelectorTitle("DELMSG","Delete Messages")]
		public CodeLookupDisplay DeleteMessage
		{
			get
			{
				return _deletemessage;
			}

			set
			{
				_deletemessage = value;
			}
		}

		[LocCategory("Override")]
		[DataBuilderSourceTypeExclude(SourceType.Class | SourceType.Object | SourceType.Instance)]
		[DataBuilderParametersPanel(false)]
		public DataBuilder OverrideSelect
		{
			get
			{
				return _overrideaselect;
			}
			set
			{
				_overrideaselect = value;
			}
		}

		[LocCategory("Messages")]
		[CodeLookupSelectorTitle("ERRMSGS","Error Messages")]
		public CodeLookupDisplay ErrorMessage
		{
			get
			{
				return _onerrormessage;
			}
			set
			{
				_onerrormessage = value;
			}
		}
	}

	public class TrashDeleteBuilder : LookupTypeDescriptor
	{
		private CodeLookupDisplay _deletemessage = new CodeLookupDisplay("RESOURCE");
		private CodeLookupDisplay _onerrormessage = new CodeLookupDisplay("RESOURCE");
		private DataBuilder _overrideaselect = new DataBuilder();
		private string _trashcanfield = "";
		private string _changevalue = "";
		private SearchListEditor _parent;

		public TrashDeleteBuilder(string Value, SearchListEditor Parent)
		{
			_parent = Parent;
			FWBS.Common.ConfigSetting serial = new FWBS.Common.ConfigSetting(Value);
			this.ErrorMessage.Code = serial.GetSetting("message","errorMessage",this.ErrorMessage.Code);
			this.DeleteMessage.Code = serial.GetSetting("message","deleteMessage",this.DeleteMessage.Code);
			this.Fieldname = serial.GetSetting("trashCan","fieldname",this.Fieldname);
			this.ChangeValue = serial.GetSetting("trashCan","changeValue",this.ChangeValue);
			this.OverrideSelect.Call = serial.GetSetting("dataBuilder","call",this.OverrideSelect.Call);
			this.OverrideSelect.Source = serial.GetSetting("dataBuilder","source",this.OverrideSelect.Source);
			this.OverrideSelect.SourceType = (SourceType)FWBS.Common.ConvertDef.ToEnum(serial.GetSetting("dataBuilder","sourceType",this.OverrideSelect.SourceType.ToString()),SourceType.OMS);

		}

		[LocCategory("Override")]
		[DataBuilderSourceTypeExclude(SourceType.Class | SourceType.Object | SourceType.Instance)]
		[DataBuilderParametersPanel(false)]
		public DataBuilder OverrideSelect
		{
			get
			{
				return _overrideaselect;
			}
			set
			{
				_overrideaselect = value;
			}
		}
		
		[LocCategory("Messages")]
		public virtual CodeLookupDisplay DeleteMessage
		{
			get
			{
				return _deletemessage;
			}

			set
			{
				_deletemessage = value;
			}
		}

		[LocCategory("Messages")]
		public virtual CodeLookupDisplay ErrorMessage
		{
			get
			{
				return _onerrormessage;
			}
			set
			{
				_onerrormessage = value;
			}
		}

		[LocCategory("Trash")]
		[Lookup("DDFieldName")]
		[TypeConverter(typeof(FieldMappingTypeEditor))]
		public string Fieldname
		{
			get
			{
				return _trashcanfield;
			}
			set
			{
				_trashcanfield = value;
			}
		}

		[LocCategory("Trash")]
		public string ChangeValue
		{
			get
			{
				return _changevalue;
			}
			set
			{
				_changevalue = value;
			}
		}

		[Browsable(false)]
		public SearchListEditor Parent
		{
			get
			{
				return _parent;
			}
		}
	}

	public class RestoreBuilder : TrashDeleteBuilder
	{
		public RestoreBuilder(string Value, SearchListEditor Parent) : base(Value, Parent)
		{
		}

		[Browsable(false)]
		public override CodeLookupDisplay DeleteMessage
		{
			get
			{
				return base.DeleteMessage;
			}
			set
			{
				base.DeleteMessage = value;
			}
		}

		[Browsable(false)]
		public override CodeLookupDisplay ErrorMessage
		{
			get
			{
				return base.ErrorMessage;
			}
			set
			{
				base.ErrorMessage = value;
			}
		}



	}

}
