using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using FWBS.Common.Security;
using FWBS.OMS.FileManagement;
using FWBS.OMS.Teams;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for SearchListButtonEditor
    /// </summary>
    public class PanelParameterEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public PanelParameterEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			// Current Action from the Object
			FWBS.OMS.OMSType.PanelTypes types = (FWBS.OMS.OMSType.PanelTypes)(TypeDescriptor.GetProperties(context.Instance)["PanelType"].GetValue(context.Instance));
			switch (types)
			{
				case FWBS.OMS.OMSType.PanelTypes.Addin:
				{
					iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
					frmListSelector frmListSelector1 = new frmListSelector();
					Type ct = (Type)TypeDescriptor.GetProperties(context.Instance)["OMSObjectType"].GetValue(context.Instance);
					DataTable dt = OmsObject.GetOMSObjects(ct.FullName);
					dt.DefaultView.Sort = "ObjDesc";
					frmListSelector1.List.BeginUpdate();
					frmListSelector1.List.DataSource = dt;
					frmListSelector1.List.DisplayMember = "ObjDesc";
					frmListSelector1.List.ValueMember = "ObjCode";
					frmListSelector1.List.EndUpdate();
					frmListSelector1.List.SelectedValue = Convert.ToString(value);
					iWFES.ShowDialog(frmListSelector1);
					if (frmListSelector1.DialogResult == DialogResult.OK)
						return frmListSelector1.List.SelectedValue;
					else
						return Convert.ToString(value);

				}
				case FWBS.OMS.OMSType.PanelTypes.TimeStatistics:
				{
					return "";
				}
				case FWBS.OMS.OMSType.PanelTypes.DataList:
				{
					iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
					frmListSelector frmListSelector1 = new frmListSelector();
					DataTable _data = FWBS.OMS.EnquiryEngine.DataLists.GetDataLists();
					frmListSelector1.List.DisplayMember = "enqTableDesc";
					frmListSelector1.List.ValueMember = "enqTable";
					frmListSelector1.List.DataSource = _data;
					frmListSelector1.List.SelectedValue = Convert.ToString(value);
					iWFES.ShowDialog(frmListSelector1);
					if (frmListSelector1.DialogResult == DialogResult.OK)
						return frmListSelector1.List.SelectedValue;
					else
						return Convert.ToString(value);
				}
				case FWBS.OMS.OMSType.PanelTypes.Property:
				{

					
					FWBS.OMS.OMSType.Panel pnl = context.Instance as FWBS.OMS.OMSType.Panel;
					EnquiryEngine.EnquiryPropertyCollection props = FWBS.OMS.EnquiryEngine.Enquiry.GetObjectProperties(pnl.OMSObjectType);
                    List<string> vals = new List<string>();
                    foreach (EnquiryEngine.EnquiryProperty prop in props)
                        vals.Add(prop.Name);
					
					iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
					frmListSelector frmListSelector1 = new frmListSelector();
					frmListSelector1.List.Items.AddRange(vals.ToArray());
					frmListSelector1.ShowHelp = false;
					frmListSelector1.List.Text = Convert.ToString(value);
					iWFES.ShowDialog(frmListSelector1);
					if (frmListSelector1.DialogResult == DialogResult.OK)
						return frmListSelector1.List.Text;
					else
						return Convert.ToString(value);
				}
				default:
				{
					return "";
				}

			}

		}
	}

	public class DefaultTemplateEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;

		public DefaultTemplateEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			FWBS.OMS.OMSType.DefaultTemplate parent = (FWBS.OMS.OMSType.DefaultTemplate)context.Instance;
			FWBS.OMS.OMSType _type = parent.OMSType;
			XmlElement _info = parent.Element;
			FWBS.OMS.Precedent p = Services.Searches.FindPrecedent(iWFES);
            if (p != null)
			    value = new FWBS.OMS.OMSType.PickAPrecedent(_type,_info,p.Title,p.Library,p.Category,p.SubCategory, p.MinorCategory);
			return value;
		}
	}

    public class CollectionEditorEx : System.ComponentModel.Design.CollectionEditor
    {
        public CollectionEditorEx(Type type) : base(type)
        {
        }

        /// <summary>
        /// Creates a new form to display and edit the current collection.
        /// Since this form internally has AutoScaleMode.Font and doesn't respect DPI awareness,
        /// we need to change font size in order to scale the form.
        /// </summary>
        /// <returns>
        /// A System.ComponentModel.Design.CollectionEditor.CollectionForm to provide as the user interface for editing the collection.
        /// </returns>
        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            return form;
        }
    }


    public class MultiPrecedentEditor : CollectionEditorEx
	{
		public MultiPrecedentEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.Precedent.MultiPrecedent);
		}

        protected override object CreateInstance(System.Type t)
        {
            FWBS.OMS.Precedent prec = (FWBS.OMS.Precedent)this.Context.Instance;
            FWBS.OMS.Precedent p = Services.Searches.FindPrecedent();
            if (p == null)
                throw new Exception(Session.CurrentSession.Resources.GetResource("NOPRECSEL", "No Precedent Selected", "").Text);

            return prec.MultiPrecedents.New(p);
        }

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.Precedent.MultiPrecedent)};
		}
	}

    public class PrecedentsTeamsAccessEditor : CollectionEditorEx
    {
        public PrecedentsTeamsAccessEditor() : base(typeof(ArrayList))
        {
        }

        protected override System.Type CreateCollectionItemType()
        {
            return typeof(PrecedentsTeamsAccessItem);
        }

        protected override void DestroyInstance(object instance)
        {
            PrecedentsTeamsAccessItem item = instance as PrecedentsTeamsAccessItem;
            if (item != null && CanRemoveInstance(instance))
            {
                FWBS.OMS.Precedent prec = (FWBS.OMS.Precedent)this.Context.Instance;
                prec.TeamsAccess.Remove(item);
            }
        }

        protected override object CreateInstance(System.Type t)
        {
            FWBS.OMS.Precedent prec = (FWBS.OMS.Precedent)this.Context.Instance;
            bool accesDenied;
            Team team = Services.Searches.FindTeam(IsInRole_PrecedentEdit, out accesDenied);
            if (accesDenied)
                throw new UpdatePermissionException();
            
            if (team == null)
                throw new Exception(Session.CurrentSession.Resources.GetResource("NOTEAM", "No Team Selected", "").Text);
            if (prec.TeamsAccess.Contains(PrecedentsTeamsAccessItem.Create(prec, team)))
                throw new Exception(Session.CurrentSession.Resources.GetResource("TEAMACCESEXIST", "The selected Team exists in Teams Access list.", "").Text);
            return prec.TeamsAccess.New(team);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(PrecedentsTeamsAccessItem) };
        }

        protected override bool CanRemoveInstance(object value)
        {
            return IsInRole_PrecedentEdit();
        }

        public static bool IsInRole_PrecedentEdit()
        {
            if (!Session.CurrentSession.CurrentUser.IsInRoles("ADMIN,PRECDEVELOPER,PRECEDIT"))
            {
                if (Session.CurrentSession.CurrentUser.IsInRoles("POWER"))
                {
                    Power power = Session.CurrentSession.CurrentPowerUserSettings;
                    if (power.IsConfigured)
                    {
                        return power.IsInRoles("PRECDEVELOPER", "PRECEDIT");
                    }
                }
                return false;
            }
            return true;
        }
    }


    public class MultiAssociateEditor : CollectionEditorEx
    {
		public MultiAssociateEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.FileType.MultiAssociate);
		}

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            FWBS.OMS.FileType ft = (FWBS.OMS.FileType)context.Instance;
            int count = ft.MultiAssociates.Count;

            object val = base.EditValue(context, provider, value);

            if (ft.MultiAssociates.Count != count)
                ft.OnDirty();

            return val;
        }

		protected override object CreateInstance(System.Type t)
		{
			FWBS.OMS.FileType ft = (FWBS.OMS.FileType)this.Context.Instance;

			Contact cont = Services.Searches.FindContact();
			if (cont == null)
				return null;
			else
				return ft.MultiAssociates.New(cont);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.FileType.MultiAssociate)};
		}
	}

	public class OMSTypeGlyphDisplayEditor : UITypeEditor
	{
        private ImageList imageList = null;
        
        public OMSTypeGlyphDisplayEditor()
		{
            imageList = FWBS.OMS.UI.Windows.Images.Entities();
		}
        
		public override bool GetPaintValueSupported ( ITypeDescriptorContext ctx )
		{
			return true ;
		}

		public override void PaintValue ( PaintValueEventArgs e )
		{
			if (e != null && e.Context != null) 
			{
				try
				{

                    int index = Convert.ToInt32(Convert.ChangeType(e.Value, typeof(System.Int32)));
                    if (index < 0 || index > imageList.Images.Count) 
                        return;
                    Image image = imageList.Images[index];
					e.Graphics.DrawImage(image, e.Bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
				}
				catch
				{
                }
			}
		}
	}

    public class TabGlyphDisplayEditorBase : UITypeEditor
    {
        protected ImageList imageList = null;

        public override bool GetPaintValueSupported(ITypeDescriptorContext ctx)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e != null && e.Context != null)
            {
                try
                {
                    Image image = imageList.Images[Convert.ToInt32(Convert.ChangeType(e.Value, typeof(System.Int32)))];
                    e.Graphics.DrawImage(image, e.Bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
                catch
                { }
            }
        }
    }

    public class TabGlyphDisplayEditor : TabGlyphDisplayEditorBase
    {
        public TabGlyphDisplayEditor()
		{
            imageList = FWBS.OMS.UI.Windows.Images.CoolButtons16();
		}		
	}

    public class NavigationTabGlyphDisplayEditor : TabGlyphDisplayEditorBase
    {
        public NavigationTabGlyphDisplayEditor()
        {
            imageList = FWBS.OMS.UI.Windows.Images.GetNavigationIconsBySize(Images.IconSize.Size16);
        }
    }

    public class DialogGlyphDisplayEditor : TabGlyphDisplayEditorBase
    {
        public DialogGlyphDisplayEditor()
        {
            imageList = FWBS.OMS.UI.Windows.Images.GetDialogIconsBySize(Images.IconSize.Size16);
        }
    }

    public class PanelGlyphDisplayEditor : UITypeEditor
	{
        private ImageList imageList = null;

        public PanelGlyphDisplayEditor()
		{
            imageList = ExpandCollapseIconSelector.GetExpandCollapseIcons();
		}

		public override bool GetPaintValueSupported ( ITypeDescriptorContext ctx )
		{
			return true ;
		}

		public override void PaintValue ( PaintValueEventArgs e )
		{
			if (e != null && e.Context != null) 
			{
				try
				{
					Image image = imageList.Images[Convert.ToInt32(Convert.ChangeType(e.Value,typeof(System.Int32)))];
					e.Graphics.DrawImage(image, e.Bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
				}
				catch
				{}
			}
		}
	}

	public class PanelEditor : CollectionEditorEx
	{
		public PanelEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.OMSType.Panel);
		}

		protected override object CreateInstance(System.Type t)
		{
			FWBS.OMS.OMSType ct = (FWBS.OMS.OMSType)this.Context.Instance;
			return new FWBS.OMS.OMSType.Panel(ct);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.OMSType.Panel)};
		}
	}

	public class DefaultsTemplatesEditor : CollectionEditorEx
	{
		public DefaultsTemplatesEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.OMSType.DefaultTemplate);
		}

		protected override object CreateInstance(System.Type t)
		{
			FWBS.OMS.OMSType ct = (FWBS.OMS.OMSType)this.Context.Instance;
			return new FWBS.OMS.OMSType.DefaultTemplate(ct);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.OMSType.DefaultTemplate)};
		}
	}

	public class JobEditor : CollectionEditorEx
	{
		public JobEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.FileType.Job);
		}

		protected override object CreateInstance(System.Type t)
		{
			FWBS.OMS.FileType ct = (FWBS.OMS.FileType)this.Context.Instance;
			FWBS.OMS.Precedent p = Services.Searches.FindPrecedent();
			if (p == null)
				return null;
			else
			{
				return new FWBS.OMS.FileType.Job(ct,p);
			}
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.FileType.Job)};
		}
	}

	public class TabEditor : CollectionEditorEx
	{
		public TabEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.OMSType.Tab);
		}

		protected override object CreateInstance(System.Type t)
		{
			FWBS.OMS.OMSType ct = (FWBS.OMS.OMSType)this.Context.Instance;
			return new FWBS.OMS.OMSType.Tab(ct);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.OMSType.Tab)};
		}
	}

	public class ExtDataEditor : CollectionEditorEx
	{
		public ExtDataEditor() : base (typeof(ArrayList)) 
		{

		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (FWBS.OMS.OMSType.ExtendedData);
		}

		protected override object CreateInstance(System.Type t)
		{
			FWBS.OMS.OMSType ct = (FWBS.OMS.OMSType)this.Context.Instance;
			return new FWBS.OMS.OMSType.ExtendedData(ct);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(FWBS.OMS.OMSType.ExtendedData)};
		}
	}

    public class Elite3ELookupEditor : UITypeEditor
    {
        private const string EnquiryFormCode = "fdSCR3ETYPECODE";

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.PropertyDescriptor.Attributes[typeof(ParameterAttribute)] == null ||
                !Session.CurrentSession.IsPackageInstalled("ELITE3E"))
                return UITypeEditorEditStyle.None;

            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService iWFES = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            try
            {
                Common.KeyValueCollection param = new Common.KeyValueCollection();
                param.Add("LookupType", Convert.ToString(((ParameterAttribute)context.PropertyDescriptor.Attributes[typeof(ParameterAttribute)]).Value));
                param.Add("CurrentValue", value);

                using (var frm = Factory.frmOMSItemFactory.GetFrmOMSItem(EnquiryFormCode, null, EnquiryEngine.EnquiryMode.Search, false, param))
                {
                    frm.Settings = EnquiryFormSettings.DisableIsDirty;
                    frm.Text = frm.EnquiryForm.Description;
                    frm.FormStorageID = EnquiryFormCode;

                    if (iWFES.ShowDialog((Form)frm) == DialogResult.OK)
                    {
                        DataTable result = frm.EnquiryForm.Enquiry.Object as DataTable;
                        if (result != null)
                        {
                            value = result.Rows[0][0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            return value;
        }
    }
}
