using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for Precedent.
    /// </summary>
    public class Precedent : FWBS.OMS.Precedent
	{
		private Precedent.PrecedentBaseMilestones _premilestones = null;

		public Precedent() : base ("New", "LETTERHEAD", "New Precedent", "doc", -1)
		{
			if (Convert.ToString(base.MilestonePlan) == "")
				this.MilestoneSettings = new PrecedentBaseMilestones();
			else
				this.MilestoneSettings = new PrecedentMilestones(this);
		}

		public Precedent(long id) : base(id, true)
		{
			if (Convert.ToString(base.MilestonePlan) == "")
				this.MilestoneSettings = new PrecedentBaseMilestones();
			else
				this.MilestoneSettings = new PrecedentMilestones(this);
			
		}

		[TypeConverter(typeof(ScriptLister))]
		[ScriptTypeParam("Precedent")]
		public override string ScriptName
		{
			get
			{
				return base.ScriptName;
			}
			set
			{
				base.ScriptName = value;
			}
		}

		[LocCategory("MILESTONES")]
		[Lookup("MSSETTINGS")]
		[RefreshProperties(RefreshProperties.All)]
		public Precedent.PrecedentBaseMilestones MilestoneSettings
		{
			get
			{
				return _premilestones;
			}
			set
			{
				_premilestones = value;
			}
		}

		/// <summary>
		/// Fetches a precedent item in Design Mode based on the unique identifier given.
		/// </summary>
		/// <param name="id">Unique identifier of the item within the data store.</param>
		/// <returns>A precedent item.</returns>
		public static Precedent GetPrecedentInDesignMode(long id)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Precedent(id);
		}

		[TypeConverter(typeof(PrecedentMilestonesConverter))]
		[Editor(typeof(PrecedentMilestonesEditors),typeof(UITypeEditor))]
		public class PrecedentBaseMilestones : LookupTypeDescriptor
		{
			public override string ToString()
			{
				return "(Set Milestone)";
			}
		}

		[TypeConverter(typeof(PrecedentMilestonesConverter))]
		public class PrecedentMilestones : PrecedentBaseMilestones
		{
			private FWBS.OMS.Precedent _parent = null;
			private PrecedentMilestonePlan _milestoneplan = null;
			private CodeLookupDisplay _milestonechangeprompt = new CodeLookupDisplay("PRECMS");
			private PrecedentMileStoneStage _milestonestage = null;

			public PrecedentMilestones(FWBS.OMS.Precedent Parent)
			{
				_parent = Parent;
				_milestoneplan = new PrecedentMilestonePlan(Convert.ToString(_parent.MilestonePlan));
				_milestonestage = new PrecedentMileStoneStage(_parent);
				_milestonechangeprompt.Code = _parent.MilestoneChangePrompt;
			}

			
			[Browsable(false)]
			public FWBS.OMS.Precedent Parent
			{
				get
				{
					return _parent;
				}
			}

			public override string ToString()
			{
				return this.MilestonePlan.ToString();
			}


			[Lookup("MSPLAN")]
			public PrecedentMilestonePlan MilestonePlan
			{
				get
				{
					return _milestoneplan;
				}
				set
				{
					_milestoneplan = value;
					_parent.MilestonePlan = value.Code;
				}
			}

			[Lookup("MSSTAGE")]
			[TypeConverter(typeof(PrecedentMilestoneStageLister))]
			public string MilestoneStage
			{
				get
				{
					return _milestonestage.Text;
				}
				set
				{
					try
					{
						string s = value.Substring(0,value.IndexOf("."));
						_milestonestage.Position = Convert.ToInt32(s);
						_parent.MilestoneStage = _milestonestage.Position;
					}
					catch
					{
						_parent.MilestoneStage = 0;
						_milestonestage.Position = 0;
					}
				}
			}

			[Lookup("MSCHGAUTO")]
			public bool MilestoneChangeAutomatic
			{
				get
				{
					return _parent.MilestoneChangeAutomatic;
				}
				set
				{
					_parent.MilestoneChangeAutomatic = value;
				}
			}

			[Lookup("MSPROMPT")]
			[CodeLookupSelectorTitle("MILECHNGPRMT","Milestone Change Prompts")]
			public CodeLookupDisplay MilestoneChangePrompt
			{
				get
				{
					return _milestonechangeprompt;
				}
				set
				{
					_milestonechangeprompt = value;
					_parent.MilestoneChangePrompt = _milestonechangeprompt.Code;
				}
			}

			[Lookup("MSMVSTAGE")]
			public bool MilestoneConfirmMoveStage
			{
				get
				{
					return _parent.MilestoneConfirmMoveStage;
				}
				set
				{
					_parent.MilestoneConfirmMoveStage = value;
				}
			}
		}

		public class PrecedentMilestonePlan 
		{
			private DataTable _plans = null;
			private string _code = "";

			public PrecedentMilestonePlan(string Code)
			{
				_code = Code;
				_plans = FWBS.OMS.Milestones_OMS2K.GetMilestonePlans(false);
			}

			public override string ToString()
			{
				DataView dv = new DataView(_plans,"[mscode] = '" + _code + "'","",DataViewRowState.CurrentRows);
				if (dv.Count > 0) return Convert.ToString(dv[0]["msdescription"]); else return "";
			}

			public string Code
			{
				get
				{
					return _code;
				}
			}

		}
		public class PrecedentMileStoneStage : LookupTypeDescriptor
		{
			private FWBS.OMS.Precedent _parent = null;
			private DataTable _data;

			public PrecedentMileStoneStage(FWBS.OMS.Precedent Parent)
			{
				_parent = Parent;
				_data = FWBS.OMS.Milestones_OMS2K.GetMilestonePlans(false);
			}

			[Lookup("MSPOSITION")]
			public int Position
			{
				get
				{
					return _parent.MilestoneStage;
				}
				set
				{
					_parent.MilestoneStage = value;
				}
			}

			[Lookup("MSSTAGEDES")]
			public string Text
			{
				get
				{
					if (this.Position > 0) 
					{
						DataView _dv = new DataView(_data,"[MSCode] = '" + _parent.MilestonePlan + "'","",DataViewRowState.CurrentRows);
						return (this.Position).ToString() + ". " + Convert.ToString(_dv[0]["MSStage" + this.Position.ToString() + "Desc"]);
					}
					else 
						return "(Not Set)";
				}
			}

			public override string ToString()
			{
				return this.Text;
			}

		}

		public class PrecedentMilestoneStageLister : StringConverter
		{
			private DataTable _data = null;

			public PrecedentMilestoneStageLister()
			{
				_data = FWBS.OMS.Milestones_OMS2K.GetMilestonePlans(false);	
			}
			
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
			{
				return true;
			}
			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
			{
				string _text = "";
				try{_text = Convert.ToString(((FWBS.OMS.Precedent)TypeDescriptor.GetProperties(context.Instance)["Parent"].GetValue(context.Instance)).MilestonePlan);}
				catch
				{}

				DataView _dv = new DataView(_data,"[MSCode] = '" + _text + "'","",DataViewRowState.CurrentRows);
				System.Collections.ArrayList arr = new System.Collections.ArrayList();
				int i=1;
				arr.Add("(Not Set)");
				foreach(DataColumn cl in _data.Columns)
				{
					if (cl.ColumnName.EndsWith("Desc") && Convert.ToString(_dv[0][cl.ColumnName]) != "")
					{
						arr.Add((i).ToString() + ". " + Convert.ToString(_dv[0][cl.ColumnName]));
						i++;
					}
				}
				return new StandardValuesCollection(arr.ToArray());
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return true;
			}

		}

		public class PrecedentMilestonesConverter : ExpandableObjectConverter
		{
			public override bool CanConvertFrom ( ITypeDescriptorContext ctx , Type sourceType )
			{
				if ( sourceType == typeof ( System.String ) )
					return true ;
				else
					return base.CanConvertFrom ( ctx , sourceType ) ;
			}

			public override object ConvertFrom ( ITypeDescriptorContext ctx , CultureInfo culture , object value )
			{
                if (value != null && value.GetType() == typeof(System.String))
				{
					string data = value as string ;
					if (data == "")
					{
						FWBS.OMS.Precedent _parent = ctx.Instance as FWBS.OMS.Precedent;
						_parent.MilestonePlan = DBNull.Value;
						return new PrecedentBaseMilestones();
					}
					else
						return base.ConvertFrom ( ctx , culture , value );
				}
				else
					return base.ConvertFrom ( ctx , culture , value ) ;
			}
		}


		public class PrecedentMilestonesEditors : UITypeEditor
		{
			public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
			{
				return UITypeEditorEditStyle.Modal; 
			}

			public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				IWindowsFormsEditorService iWFES;
				FWBS.OMS.Precedent _parent = null;
				_parent = context.Instance as FWBS.OMS.Precedent;

				PrecedentMilestones returnvalue = new PrecedentMilestones(_parent);
				
					
				iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				frmListSelector frmListSelector1 = new frmListSelector();
				frmListSelector1.Text = Session.CurrentSession.Resources.GetResource("MSPLANS","Milestone Plans","").Text;
				frmListSelector1.List.DataSource =FWBS.OMS.Milestones_OMS2K.GetMilestonePlans(false);
				frmListSelector1.List.DisplayMember = "MSDescription";
				frmListSelector1.List.ValueMember = "MSCode";
				frmListSelector1.ShowHelp = false;
				iWFES.ShowDialog(frmListSelector1);
				if (frmListSelector1.DialogResult == System.Windows.Forms.DialogResult.OK)
				{
					returnvalue.MilestonePlan = new PrecedentMilestonePlan(Convert.ToString(frmListSelector1.List.SelectedValue));
					FWBS.OMS.Favourites fav = new Favourites("DONOTSHOW");
					if (fav.Count == 0 || fav.Param1(0) != "YES")
					{
						FWBS.OMS.UI.Windows.Admin.frmPropertyDemo frmPropDemo = new FWBS.OMS.UI.Windows.Admin.frmPropertyDemo("Expand Milestones Property");
						frmPropDemo.ShowDialog();
						if (frmPropDemo.chkDoNoShow.Checked)
						{
							fav.Description(0,"Do Not Show Expand Property Demo");
							fav.Param1(0,"YES");
							fav.Update();
						}
					}
					return returnvalue;

				}
				else
					return value;

			}
		}
	}
}
