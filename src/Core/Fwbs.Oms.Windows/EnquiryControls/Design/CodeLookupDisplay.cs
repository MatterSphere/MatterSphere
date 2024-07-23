using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for CodeLookupDisplayEditor.
    /// </summary>
    public class CodeLookupDisplayEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public CodeLookupDisplayEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
		
			string Type = "";
			string Code = "";
			if (value is CodeLookupDisplayReadOnly)
			{
				CodeLookupDisplayReadOnly cld = (CodeLookupDisplayReadOnly)value;
				if (cld.ReadOnly)
				{
                    System.Windows.Forms.MessageBox.Show(cld.ReadOnlyMessage, Session.CurrentSession.Resources.GetResource("OMSADMIN", "OMS Administration", "").Text, MessageBoxButtons.OK,MessageBoxIcon.Stop);
					return value;
				}
				Type = cld.Type;
				Code = cld.Code;
			}
			else if (value is CodeLookupDisplay)
			{
				CodeLookupDisplay cld = (CodeLookupDisplay)value;
				Type = cld.Type;
				Code = cld.Code;
			}
			else
				return value;
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmCodeLookupSelector frmListSelector1 = new frmCodeLookupSelector();
			frmListSelector1.ToolTip.Visible = false;
			frmListSelector1.CodeType = Type;
			string param = "";
			try
			{
				param = Convert.ToString(((ParameterAttribute)context.PropertyDescriptor.Attributes[typeof(ParameterAttribute)]).Value).ToUpper();
			}
			catch
			{
			}
			try
			{
				string title = Convert.ToString(((CodeLookupSelectorTitleAttribute)context.PropertyDescriptor.Attributes[typeof(CodeLookupSelectorTitleAttribute)]).Title);
				frmListSelector1.Text = title;
			}
			catch
			{
			}
			if (param == CodeLookupDisplaySettings.omsObjects.ToString().ToUpper())
			{
				try
				{
					if (context.Instance.GetType().FullName == "FWBS.OMS.UI.Windows.Design.Precedent")
					{
						DataTable dt = OmsObject.GetOMSObjects(null,"ExtData");
						dt.DefaultView.Sort = "ObjDesc";
						frmListSelector1.List.BeginUpdate();
						frmListSelector1.List.DataSource = dt;
						frmListSelector1.List.DisplayMember = "ObjDesc";
						frmListSelector1.List.ValueMember = "ObjCode";
						frmListSelector1.List.EndUpdate();
					}
					else
					{
						Type ct = (Type)TypeDescriptor.GetProperties(context.Instance)["OMSObjectType"].GetValue(context.Instance);
						DataTable dt = OmsObject.GetOMSObjects(ct.FullName);
						dt.DefaultView.Sort = "ObjDesc";
						frmListSelector1.List.BeginUpdate();
						frmListSelector1.List.DataSource = dt;
						frmListSelector1.List.DisplayMember = "ObjDesc";
						frmListSelector1.List.ValueMember = "ObjCode";
						frmListSelector1.List.EndUpdate();
					}
				}
				catch
				{}
			}
			else if (param == CodeLookupDisplaySettings.ExtendedData.ToString().ToUpper())
			{
				string _type = "";
				try
				{
					try
					{
						_type = Convert.ToString(((FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance)).Enquiry.Source.Tables["ENQUIRY"].Rows[0]["enqSource"]);
					}
					catch
					{
						try
						{
							_type = ((Type)TypeDescriptor.GetProperties(context.Instance)["OMSObjectType"].GetValue(context.Instance)).FullName;
						}
						catch
						{}
					}
					string ct = _type;
					frmListSelector1.List.DisplayMember = "cddesc";
					frmListSelector1.List.ValueMember = "extcode";
					if (_type == "")
						frmListSelector1.List.DataSource = ExtendedData.GetExtendedDatas();
					else
						frmListSelector1.List.DataSource = ExtendedData.GetExtendedDatas(ct);
				}
				catch
				{}
			}
			else if (param == CodeLookupDisplaySettings.Commands.ToString().ToUpper())
			{
				DataTable _cmd;
				try
				{
					_cmd = (DataTable)((FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance)).Enquiry.Source.Tables["COMMANDS"];
					frmListSelector1.List.DisplayMember = "cmdCode";
					frmListSelector1.List.ValueMember = "cmdCode";
					frmListSelector1.List.DataSource = _cmd;
				}
				catch
				{}
			}
			else if (param == CodeLookupDisplaySettings.DataList.ToString().ToUpper())
			{
				DataTable _data;
				try
				{
					_data = DataLists.GetDataLists();
					frmListSelector1.List.DisplayMember = "enqTableDesc";
					frmListSelector1.List.ValueMember = "enqTable";
					frmListSelector1.List.DataSource = _data;
				}
				catch
				{}
			}
			else if (param == CodeLookupDisplaySettings.PrecedentAssoc.ToString().ToUpper())
			{
				DataTable _data;
				try
				{
					if (context.Instance is FWBS.OMS.Precedent)
						_data = FWBS.OMS.Associate.GetAssociateTypes(((FWBS.OMS.Precedent)context.Instance).ContactType,false);
					else if (context.Instance is FWBS.OMS.Precedent.MultiPrecedent)
						_data = FWBS.OMS.Associate.GetAssociateTypes(((FWBS.OMS.Precedent.MultiPrecedent)context.Instance).ContactType,false);
					else
						_data = null;
					frmListSelector1.List.DisplayMember = _data.Columns[1].ColumnName;
					frmListSelector1.List.ValueMember = _data.Columns[0].ColumnName;
					frmListSelector1.List.DataSource = _data;
				}
				catch
				{}
			}
			else if (param == CodeLookupDisplaySettings.Dependents.ToString().ToUpper())
			{
				DataTable _data;
				try
				{
					_data = FWBS.OMS.Design.Package.Packages.GetPackageList();
					frmListSelector1.List.DisplayMember = "pkgdesc";
					frmListSelector1.List.ValueMember = "pkgcode";
					frmListSelector1.List.DataSource = _data;
				}
				catch
				{}
			}
			else
				frmListSelector1.LoadCodeTypes();
			if (Convert.ToString(Code) != "")
				frmListSelector1.List.SelectedValue= Code;
			frmListSelector1.ShowHelp = true;
			iWFES.ShowDialog(frmListSelector1);
			if (frmListSelector1.DialogResult == DialogResult.OK)
			{
				if (value is CodeLookupDisplayReadOnly)
				{
					CodeLookupDisplayReadOnly output = new CodeLookupDisplayReadOnly(Type);
					output.Code = frmListSelector1.List.SelectedValue.ToString();
					value = output;
				}
				else
				{
					CodeLookupDisplay output = new CodeLookupDisplay(Type);
					output.Code = Convert.ToString(frmListSelector1.List.SelectedValue);
					value = output;
				}
			}
			return value;
		}
	}

	/// <summary>
	/// Summary description for CodeLookupDisplayEditor.
	/// </summary>
	public class CodeLookupDisplayMultiEditor : UITypeEditor
	{
		public CodeLookupDisplayMultiEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			
			CodeLookupDisplayMulti newval = value as CodeLookupDisplayMulti;
			IWindowsFormsEditorService iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmCodeLookupMulti frmCodeLookupMulti1 = new frmCodeLookupMulti();
			try
			{
				frmCodeLookupMulti1.Text = Convert.ToString(((CodeLookupSelectorTitleAttribute)context.PropertyDescriptor.Attributes[typeof(CodeLookupSelectorTitleAttribute)]).Title);
			}
			catch
			{
			}
			frmCodeLookupMulti1.CollectionSelector.CodeType = "USRROLES";
			frmCodeLookupMulti1.CollectionSelector.ValueMember = "cdcode";
			frmCodeLookupMulti1.CollectionSelector.DisplayMember = "cddesc";
			frmCodeLookupMulti1.CollectionSelector.Value = newval.Codes;

			iWFES.ShowDialog(frmCodeLookupMulti1);
			return new CodeLookupDisplayMulti(Convert.ToString(frmCodeLookupMulti1.CollectionSelector.Value),newval.Type);
		}
	}


	/// <summary>
	/// Summary description for CodeDescriptionEditor.
	/// </summary>
	public class CodeDescriptionEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public CodeDescriptionEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			bool param = false;
			try
			{
				param = (bool)(((ReadOnlyAttribute)context.PropertyDescriptor.Attributes[typeof(ReadOnlyAttribute)]).IsReadOnly);
			}
			catch{}

			bool textonly = false;
			try
			{
				textonly = (bool)(((TextOnlyAttribute)context.PropertyDescriptor.Attributes[typeof(TextOnlyAttribute)]).TextOnly);
			}
			catch{}

			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmTextEditor frmTextEditor1 = new frmTextEditor();
			if (param)
				frmTextEditor1.Text = ResourceLookup.GetLookupText("TextViewer");
			else
				frmTextEditor1.Text = ResourceLookup.GetLookupText("TextEditor");
			frmTextEditor1.ucRichTextEditor1.ReadOnly=param;
			if (textonly == true)
			{
				frmTextEditor1.ucRichTextEditor1.ShowToolBar=false;
				frmTextEditor1.grpOuput.Visible= false;
				frmTextEditor1.ucRichTextEditor1.TextMode = TextMode.Text;
			}

			frmTextEditor1.ucRichTextEditor1.Value = Convert.ToString(value);
			iWFES.ShowDialog(frmTextEditor1);
			if (frmTextEditor1.DialogResult == DialogResult.OK)
			{
				if (textonly == true)
					frmTextEditor1.ucRichTextEditor1.TextMode = TextMode.Text;
				value = frmTextEditor1.ucRichTextEditor1.Value;
			}
			return value;
		}
	}

	/// <summary>
	/// Summary description for CultureUIEditor.
	/// </summary>
	public class CultureUIEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string _type = "";
			string _code = "";
			try
			{
				_type = Convert.ToString(TypeDescriptor.GetProperties(context.Instance)["Type"].GetValue(context.Instance));
				_code = Convert.ToString(TypeDescriptor.GetProperties(context.Instance)["Code"].GetValue(context.Instance));
			}
			catch
			{}
			DataTable dt = FWBS.OMS.CodeLookup.GetCultures(_type,_code);
			ArrayList ar = new ArrayList();
			foreach(DataRow row in dt.Rows)
				ar.Add(row[0]);
			return new StandardValuesCollection(ar);
		}
	}

	/// <summary>
	/// Summary description for DataBuilderEditor.
	/// </summary>
	public class CodeLookupReadOnlyConverter : TypeConverter
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
			if (CodeLookupDisplay.EditorOpen == false && value != null && value.GetType() == typeof(System.String))
			{
				// Parse the string to get the colours.
				string data = value as string ;
				string _type = ((CodeLookupDisplayReadOnly)ctx.PropertyDescriptor.GetValue(ctx.Instance)).Type;
				if (data != "")
				{
					DataTable dt = CodeLookup.GetLookups(_type);

					string searchstring = FWBS.Common.SQLRoutines.RemoveRubbish(data);
					if (searchstring.StartsWith("{\\rtf"))
					{
						dt.DefaultView.RowFilter = "cddesc = '" + searchstring.Replace("'","''") + "'";
						if (dt.DefaultView.Count == 1)
						{
							CodeLookupDisplay output = new CodeLookupDisplay(_type);
							output.Code = Convert.ToString(dt.DefaultView[0]["cdCode"]);
							return output;
						}
						else
						{
							return new CodeLookupDisplay(_type);
						}
					}
					else
					{
						searchstring = searchstring.Replace("[","ÿþýÊ");
						searchstring = searchstring.Replace("]","Êÿýþ");
						searchstring = searchstring.Replace("ÿþýÊ","[[]");
						searchstring = searchstring.Replace("Êÿýþ","[]]");
						searchstring = searchstring.Replace("%","[%]");
						dt.DefaultView.RowFilter = "cddesc Like '" + searchstring.Replace("'","''") + "*'";
					}
					if (dt.DefaultView.Count > 1)
					{
						CodeLookupDisplay.EditorOpen=true;
						frmCodeLookupSelector frmListSelector1 = new frmCodeLookupSelector();
						frmListSelector1.CodeType = _type;
						frmListSelector1.LoadCodeTypes();
						frmListSelector1.txtSearch.Text = data;
						frmListSelector1.ShowHelp = true;
						frmListSelector1.ToolTip.Visible = false;
						frmListSelector1.ShowDialog();
						CodeLookupDisplay.EditorOpen=false;
						if (frmListSelector1.DialogResult == DialogResult.OK)
						{
							CodeLookupDisplayReadOnly output = new CodeLookupDisplayReadOnly(_type);
							output.Code = frmListSelector1.List.SelectedValue.ToString();
							return output;
						}
						else 					
							return base.ConvertFrom ( ctx , culture , value );
					}
					else if (dt.DefaultView.Count == 1)
					{
						CodeLookupDisplayReadOnly output = new CodeLookupDisplayReadOnly(_type);
						output.Code = Convert.ToString(dt.DefaultView[0]["cdCode"]);
						return output;
					}
					else
					{
						return base.ConvertFrom ( ctx , culture , value );
					}
				}
				else if (data == "")
					return new CodeLookupDisplayReadOnly(_type);
				else
					return base.ConvertFrom ( ctx , culture , value );
			}
			else
				return new CodeLookupDisplayReadOnly("");
	}
	}

	public class CodeLookupDisplayMultiConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return value.ToString();
		}


	}
	
	public class CodeLookupDisplayConverter : ExpandableObjectConverter
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
			if (value.GetType() == typeof(System.String))
			{
				// Parse the string to get the colours.
				string data = value as string ;
				string _type = ((CodeLookupDisplay)ctx.PropertyDescriptor.GetValue(ctx.Instance)).Type;
				if (data != "" && CodeLookupDisplay.EditorOpen == false)
				{
					DataTable dt = CodeLookup.GetLookups(_type);

					string searchstring = FWBS.Common.SQLRoutines.RemoveRubbish(data);
					if (searchstring.StartsWith("{\\rtf"))
					{
						dt.DefaultView.RowFilter = "cddesc = '" + searchstring.Replace("'","''") + "'";
						if (dt.DefaultView.Count == 1)
						{
							CodeLookupDisplay output = new CodeLookupDisplay(_type);
							output.Code = Convert.ToString(dt.DefaultView[0]["cdCode"]);
							return output;
						}
						else
						{
							return new CodeLookupDisplay(_type);
						}
					}
					else
					{
						searchstring = searchstring.Replace("[","ÿþýÊ");
						searchstring = searchstring.Replace("]","Êÿýþ");
						searchstring = searchstring.Replace("ÿþýÊ","[[]");
						searchstring = searchstring.Replace("Êÿýþ","[]]");
						searchstring = searchstring.Replace("%","[%]");
						dt.DefaultView.RowFilter = "cddesc Like '" + searchstring.Replace("'","''") + "*'";
					}
					if (dt.DefaultView.Count > 1 || (dt.DefaultView.Count == 1 && data != Convert.ToString(dt.DefaultView[0]["cdDesc"]).Trim()))
					{
						frmCodeLookupSelector frmListSelector1 = null;
						try
						{
							CodeLookupDisplay.EditorOpen=true;
							frmListSelector1 = new frmCodeLookupSelector();
							frmListSelector1.CodeType = _type;
							frmListSelector1.LoadCodeTypes();
							frmListSelector1.txtSearch.Text = data;
							frmListSelector1.ShowHelp = true;
							frmListSelector1.ShowDialog();
						}
						finally
						{
							CodeLookupDisplay.EditorOpen=false;
						}
						if (frmListSelector1.DialogResult == DialogResult.OK)
						{
							CodeLookupDisplay output = new CodeLookupDisplay(_type);
                            output.Code = Convert.ToString(frmListSelector1.List.SelectedValue);
							return output;
						}
						else
						{
							CodeLookupDisplay.EditorOpen=true;
							string _code;
							_code = data.GetHashCode().ToString();
							CodeLookupUIAttribute sw = null;
							try
							{
								sw = ctx.PropertyDescriptor.Attributes[typeof(CodeLookupUIAttribute)] as CodeLookupUIAttribute;
							}
							catch{}
							if (sw == null) sw = new CodeLookupUIAttribute(CodeLookupUIAttributes.Question);

							if (sw.Value == CodeLookupUIAttributes.ChangeCode)
							{
								string question = Session.CurrentSession.Resources.GetMessage("CHOOSECODE","The Description '%1%' does not exist. " + Environment.NewLine + Environment.NewLine + "Please enter a code used to create it...","",data).Text;
								string title = Session.CurrentSession.Resources.GetMessage("CHOOSECODET","Create a %1%","",_type).Text;
								_code = InputBox.Show(question,_code,title,15);
								CodeLookupDisplay.EditorOpen=false;
								if (_code == InputBox.CancelText) return new CodeLookupDisplay(_type);
							}
							else if (sw.Value == CodeLookupUIAttributes.Question)
							{
								if (System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("NEWCODELKUP","The '%1%' does not exist." + Environment.NewLine + Environment.NewLine + "Do you wish to create it?","",false,data),ResourceLookup.GetLookupText("OMSAdmin"),MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
								{
									CodeLookupDisplay.EditorOpen=false;
									return new CodeLookupDisplay(_type);
								}
							}
							CodeLookupDisplay.EditorOpen=false;
							CodeLookup.Create(_type,_code,data,"",CodeLookup.DefaultCulture, true,true,true);
							CodeLookupDisplay output = new CodeLookupDisplay(_type);
							output.Code = _code;
							return output;
						}
					}
					else if (dt.DefaultView.Count == 1 && data == Convert.ToString(dt.DefaultView[0]["cdDesc"]).Trim())
					{
						CodeLookupDisplay output = new CodeLookupDisplay(_type);
						output.Code = Convert.ToString(dt.DefaultView[0]["cdCode"]);
						return output;
					}
					else if (dt.DefaultView.Count == 0)
					{
						CodeLookupDisplay.EditorOpen=true;
						string _code;
						_code = data.GetHashCode().ToString();
						CodeLookupUIAttribute sw = null;
						try
						{
							sw = ctx.PropertyDescriptor.Attributes[typeof(CodeLookupUIAttribute)] as CodeLookupUIAttribute;
						}
						catch{}
						if (sw == null) sw = new CodeLookupUIAttribute(CodeLookupUIAttributes.Question);
						if (sw.Value == CodeLookupUIAttributes.ChangeCode)
						{
							string question = Session.CurrentSession.Resources.GetMessage("CHOOSECODE","The Description '%1%' does not exist. " + Environment.NewLine + Environment.NewLine + "Please enter a code used to create it...","",data).Text;
							string title = Session.CurrentSession.Resources.GetMessage("CHOOSECODET","Create a %1%","",_type).Text;
                            _code = InputBox.Show(question, title, _code, 15);
							CodeLookupDisplay.EditorOpen=false;
							if (_code == InputBox.CancelText) return new CodeLookupDisplay(_type);
						}
						else if (sw.Value == CodeLookupUIAttributes.Question)
						{
							if (System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("NEWCODELKUP","The '%1%' does not exist." + Environment.NewLine + Environment.NewLine + "Do you wish to create it?","",false,data),ResourceLookup.GetLookupText("OMSAdmin"),MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
							{
								CodeLookupDisplay.EditorOpen=false;
								return new CodeLookupDisplay(_type);
							}
						}
						CodeLookupDisplay.EditorOpen=false;
						CodeLookup.Create(_type,_code,data,"",CodeLookup.DefaultCulture, true,true,true);
						CodeLookupDisplay output = new CodeLookupDisplay(_type);
						output.Code = _code;
						return output;
					}
					else 					
						return new CodeLookupDisplay(_type);
				}
				else
					return new CodeLookupDisplay(_type);
			}
			else
				return base.ConvertFrom ( ctx , culture , value ) ;
		}
	}
}
