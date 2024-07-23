using System;
using System.ComponentModel;
using System.Xml;

namespace FWBS.OMS.FileManagement.Configuration
{
    public sealed class ActionConfig : LookupTypeDescriptor, System.ComponentModel.INotifyPropertyChanged
	{
		#region Constants

		public const byte GLOBAL_MILESTONE_STAGE = 0;
		public const string GLOBAL_MILESTONE_PLAN = "";

		#endregion

		#region Fields

		private XmlElement _info;
		private FMApplication _app;
		private bool _isNew = false;
		private CodeLookupDisplay _code = null;
		private CodeLookupDisplayMulti _usrroles = null;
	
		#endregion

		#region Constructors

		private ActionConfig (){}

		internal ActionConfig(FMApplication app)
		{
			if (app == null)
				throw new ArgumentNullException("app");

			_app = app;
			_info = _app._config.CreateElement("Action");
			this.MilestonePlan = app.DefaultMilestonePlan;
			_app.WriteAttribute(_info, "lookup", "");
			_isNew = true;
		}

		internal ActionConfig(FMApplication app, XmlElement info) 
		{
			if (app == null)
				throw new ArgumentNullException("app");

			if (info == null)
				throw new ArgumentNullException("info");

			_app = app;
			_info = info;
			
		}

		#endregion

		#region Properties

		[System.ComponentModel.Browsable(false)]
		public bool IsNew
		{
			get
			{
				return _isNew;
			}
		}

		[System.ComponentModel.Browsable(false)]
		internal XmlElement Element
		{
			get
			{
				return _info;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public string Code
		{
			get
			{
				return _app.ReadAttribute(_info, "lookup", "");
			}
			set
			{
                if (value != Code)
                {
                    _app.WriteAttribute(_info, "lookup", value);
                    OnPropertyChanged("Code");
                }
			}
		}

		[System.ComponentModel.Browsable(false)]
		public string Description
		{
			get
			{
				try
				{
					return LocalizedCode.Description;
				}
				catch
				{
					return _app.ReadAttribute(_info, "description", "");
				}
			}
			set
			{
				LocalizedCode.Description = value;
				_app.WriteAttribute(_info, "description", value);
			}
		}

		[Lookup("Description")]
		[LocCategory("(NAME)")]
		[CodeLookupSelectorTitle("ACTIONNAMES","Action Names")]
		public  CodeLookupDisplay LocalizedCode
		{
			get
			{
				if (_code == null)
				{
					_code = new CodeLookupDisplay("FMACTIONS");
					_code.CodeLookupDisplayChanged+= new EventHandler(this.CodeLookupDisplayChanged);
					_code.Code = this.Code;
				}
				return _code;
			}
			set
			{
				if (value != null)
				{
					_code = value;
					_code.CodeLookupDisplayChanged-= new EventHandler(this.CodeLookupDisplayChanged);
					_code.CodeLookupDisplayChanged+= new EventHandler(this.CodeLookupDisplayChanged);

                    this.Code = value.Code;                    
				}
			}
		}

		[LocCategory("VISIBILITY")]
		public string[] Conditional
		{
			get
			{
				return _app.ReadAttribute(_info, "conditional", "").Split(Environment.NewLine.ToCharArray());
			}
			set
			{
                _app.WriteAttribute(_info,"conditional",String.Join(Environment.NewLine,value));
                OnPropertyChanged("Conditional");
			}
		}

		[Lookup("USERROLES")]
		[LocCategory("VISIBILITY")]
		[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti UserRolesDisplay
		{
			get
			{
				if (_usrroles == null)
				{
					_usrroles = new CodeLookupDisplayMulti("USRROLES");
					_usrroles.Codes = this.UserRoles;
				}
				return _usrroles;
			}
			set
			{
				_usrroles = value;
				this.UserRoles = value.Codes;
			}
		}

		[Browsable(false)]
		public string UserRoles
		{
			get
			{
				return _app.ReadAttribute(_info, "userRoles", "");
			}
			set
			{
                if (value != UserRoles)
                {
                    _app.WriteAttribute(_info, "userRoles", value);
                    OnPropertyChanged("UserRoles");
                }
			}
		}

		[System.ComponentModel.Browsable(false)]
		public FMApplication Application
		{
			get
			{
				return _app;
			}
		}

		/// <summary>
		/// Specifies the milestone plan.
		/// </summary>
		[LocCategory("MILESTONES")]
		[Lookup("PLAN")]
		[RefreshProperties(RefreshProperties.All)]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[FWBS.OMS.Design.DataList("DSMSPLANS", UseNull=true, NullValue="")]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		public string MilestonePlan
		{
			get
			{
				return _app.ReadAttribute(_info, "plan", String.Empty).Trim();
			}
			set
			{
				if (value == null)
					value = String.Empty;
                if (value != MilestonePlan)
                {
                    _app.WriteAttribute(_info, "plan", value);
                    _app.WriteAttribute(_info, "stage", 0);
                    OnPropertyChanged("MilestonePlan");
                }
			}
		}

		/// <summary>
		/// Specifies the milestone stage scope.
		/// </summary>
		[LocCategory("MILESTONES")]
		[Lookup("STAGE")]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[FWBS.OMS.Design.DataList("DSMSPLANSTAGES", UseNull=true, NullValue=0, OrderBy="stage")]
		[Parameter("PLAN", "~OBJ.MilestonePlan", Parse=true)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		public byte MilestoneStage
		{
			get
			{
				try
				{
					return Convert.ToByte(_app.ReadAttribute(_info, "stage", GLOBAL_MILESTONE_STAGE.ToString()));
				}
				catch
				{
					return GLOBAL_MILESTONE_STAGE;
				}
			}
			set
			{
                if (value != MilestoneStage)
                {
                    _app.WriteAttribute(_info, "stage", value);
                    OnPropertyChanged("MilestoneStage");
                }
                
			}
		}

		/// <summary>
		/// Specifies the run command.
		/// </summary>
		[LocCategory("SCRIPTING")]
		[TypeConverter(typeof(Design.MethodLister))]
		public string Method
		{
			get
			{
				return ValidateMethodName(_app.ReadAttribute(_info, "method", ""));
			}
			set
			{
                value = ValidateMethodName(value);

                if (value != Method)
                {
                    _app.WriteAttribute(_info, "method", value);
                    OnPropertyChanged("Method");
                }
			}
		}

		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.TabGlyphDisplayEditor,omsadmin",typeof(System.Drawing.Design.UITypeEditor))]
		[System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.CoolButtonLister,omsadmin")]
		[LocCategory("DESIGN")]
		public int Glyph
		{
			get
			{
				return Common.ConvertDef.ToInt32(_app.ReadAttribute(_info, "glyph", "-1"), -1);
			}
			set
			{
                if (value != Glyph)
                {
                    _app.WriteAttribute(_info, "glyph", value);
                    OnPropertyChanged("Glyph");
                }
			}
		}



		#endregion

		#region Methods

		public override string ToString()
		{
			return Session.CurrentSession.Terminology.Parse(this.LocalizedCode.Description,true);
		}

		private void CodeLookupDisplayChanged (object sender, EventArgs e)
		{
			_app.WriteAttribute(_info, "description", _code.Description);
		}

        private string ValidateMethodName(string name)
        {
            if (name.Length < 1)
                return String.Empty;

            char[] nameChar = name.ToCharArray();

            if (!char.IsLetter(nameChar[0]))
                return String.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < nameChar.Length; i++)
            {
                if (char.IsLetterOrDigit(nameChar[i]) || (nameChar[i] == '_'))
                    sb.Append(nameChar[i]);
            }

            return sb.ToString();
        }

		#endregion



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
