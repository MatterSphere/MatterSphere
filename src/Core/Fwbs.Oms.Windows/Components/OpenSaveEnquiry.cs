using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for OpenSaveEnquiry.
    /// </summary>
    public enum OpenSaveModes {OpenForm,SaveForm};
	public class OpenSaveEnquiry : System.ComponentModel.Component, IDisposable
	{
		private frmOpenSaveDialog frmOpenSaveDialog1=null;
		private OpenSaveModes _opensavemode = OpenSaveModes.OpenForm;
		private string _folder = @"\";
		private string _code = "";
		private bool _deletable = false;
		private bool _renamable = false;
		private bool _newfolderable = false;

		public OpenSaveEnquiry()
		{
		}

		#region IDisposable Implementation


		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		protected override void Dispose(bool disposing) 
		{
            try
            {
                if (disposing)
                {
                    frmOpenSaveDialog1.Dispose();
                    frmOpenSaveDialog1 = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

		#endregion

		#region Public Procedures
		public System.Windows.Forms.Form OpenSaveForm()
		{
			frmOpenSaveDialog1 = new frmOpenSaveDialog(_folder);
			frmOpenSaveDialog1.Folder = _folder;
			frmOpenSaveDialog1.Filename.Text = _code;
			SetStyle();
			return frmOpenSaveDialog1;
		}
		
		public System.Windows.Forms.DialogResult Execute()
		{
			return Execute(null);
		}
		
		public System.Windows.Forms.DialogResult Execute(System.Windows.Forms.IWin32Window owner)
		{
			frmOpenSaveDialog1 = new frmOpenSaveDialog(_folder);
			frmOpenSaveDialog1.Folder = _folder;
			frmOpenSaveDialog1.Filename.Text = _code;
			SetStyle();
			if (owner == null)
				frmOpenSaveDialog1.ShowDialog();
			else
				frmOpenSaveDialog1.ShowDialog(owner);

			_folder = frmOpenSaveDialog1.Folder;
			_code = frmOpenSaveDialog1.Filename.Text;
			return frmOpenSaveDialog1.DialogResult;
		}	
		#endregion

		#region Private Procedures
		private void SetStyle()
		{
			if (frmOpenSaveDialog1 != null)
			{
				if (_opensavemode == OpenSaveModes.SaveForm)
				{
					frmOpenSaveDialog1.Text = ResourceLookup.GetLookupText("Save").Replace("&","") + "...";
					frmOpenSaveDialog1.btnOpen.Text = ResourceLookup.GetLookupText("Save","Save","");
					frmOpenSaveDialog1.frmOpenDialogSytle = frmOpenDialogSytles.Save;
				}
				if (_opensavemode == OpenSaveModes.OpenForm)
				{
					frmOpenSaveDialog1.Text = ResourceLookup.GetLookupText("Open").Replace("&","") + "...";
					frmOpenSaveDialog1.btnOpen.Text = ResourceLookup.GetLookupText("Open","Open","");
					frmOpenSaveDialog1.frmOpenDialogSytle = frmOpenDialogSytles.Open;
				}
				frmOpenSaveDialog1.tbDel.Visible = _deletable;
				frmOpenSaveDialog1.tbRename.Visible = _renamable;
				frmOpenSaveDialog1.tbNewFolder.Visible = _newfolderable;
				if (_deletable == false && _renamable == false && _newfolderable == false)
					frmOpenSaveDialog1.tbSp2.Visible = false;
				else
					frmOpenSaveDialog1.tbSp2.Visible = true;
			}
		}
		#endregion

		#region Properties
		[Description("The mode that the Open or Save Dialog is in.")]
		[DefaultValue(OpenSaveModes.OpenForm)]
		[Category("Style")]
		public OpenSaveModes OpenSaveMode
		{
			get
			{
				return _opensavemode;
			}
			set
			{
				if (_opensavemode != value)
				{
					_opensavemode = value;
					SetStyle();
				}
			}
		}

		[Description("Show the Delete Enquiry Form Toolbar Button")]
		[DefaultValue(false)]
		[Category("Allow")]
		public bool AllowDelete
		{
			get
			{
				return _deletable;
			}
			set
			{
				if (_deletable != value)
				{
					_deletable = value;
					SetStyle();
				}
			}
		}

		[Description("Show the Rename Enquiry Form Toolbar Button")]
		[DefaultValue(false)]
		[Category("Allow")]
		public bool AllowRename
		{
			get
			{
				return _renamable;
			}
			set
			{
				if (_renamable != value)
				{
					_renamable = value;
					SetStyle();
				}
			}
		}

		[Description("Show the New Folder Toolbar Button")]
		[DefaultValue(false)]
		[Category("Allow")]
		public bool AllowNewFolder
		{
			get
			{
				return _newfolderable;
			}
			set
			{
				if (_newfolderable != value)
				{
					_newfolderable = value;
					SetStyle();
				}
			}
		}

		[Description("Sets the Folder Path")]
		[DefaultValue(@"\")]
		[Category("Data")]
		public string Folder
		{
			get
			{
				if (frmOpenSaveDialog1 != null)
					return frmOpenSaveDialog1.Folder;
				else
					return _folder;
			}
			set
			{
				if (_folder != value)
				{
					_folder = value;
					if (frmOpenSaveDialog1 != null)
					{
						frmOpenSaveDialog1.Folder = value;
					}
				}
			}
		}

		[Description("Gets the Selected Enquiry Caption")]
		[Browsable(false)]
		[Category("Data")]
		public string Caption
		{
			get
			{
				if (frmOpenSaveDialog1 != null)
					return frmOpenSaveDialog1.Caption;
				else
					return "";
			}
		}
		[Description("Sets or Gets the Enquiry Code Filename")]
		[DefaultValue("")]
		[Category("Data")]
		public string Code
		{
			get
			{
				if (frmOpenSaveDialog1 != null)
					return frmOpenSaveDialog1.Filename.Text;
				else
					return _code;
			}
			set
			{
				if (_code != value)
				{
					_code = value;
					if (frmOpenSaveDialog1 != null)
					{
						frmOpenSaveDialog1.Filename.Text = value;
					}
				}
			}
		}
		#endregion
	}
}
