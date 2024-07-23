using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FWBS.OMS.UI.Windows.Design
{
    public class BranchEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService iWFES;
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmListSelector frmListSelector1 = new frmListSelector();
			frmListSelector1.Text = Session.CurrentSession.Resources.GetResource("BRANCHES","Branches","").Text;
			frmListSelector1.List.BeginUpdate();
			DataTable _data = FWBS.OMS.Branch.GetBranches();
			DataRow dt = _data.NewRow();
			dt["brID"] = DBNull.Value;
			dt["brName"] = FWBS.OMS.Session.CurrentSession.Resources.GetResource("NOTSET","(Not Specified)","").Text;
			_data.Rows.Add(dt);
			_data.DefaultView.Sort = "brID";
			frmListSelector1.List.DataSource = _data;
			frmListSelector1.List.DisplayMember = "brName";
			frmListSelector1.List.ValueMember = "brID";
			if (value is FWBS.OMS.Branch)
				frmListSelector1.List.SelectedValue = ((FWBS.OMS.Branch)value).ID;
			frmListSelector1.List.EndUpdate();
			frmListSelector1.ShowHelp = false;
			iWFES.ShowDialog(frmListSelector1);
			if (frmListSelector1.DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				if (frmListSelector1.List.SelectedValue == DBNull.Value)
					return null;
				else
					return new FWBS.OMS.Branch(Convert.ToInt32(frmListSelector1.List.SelectedValue));
			}
			else
				return value;

		}
	}

}
