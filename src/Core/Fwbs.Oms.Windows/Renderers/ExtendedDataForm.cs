using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// An extended data form uses the same rendering logic using the FormRenderBase cotnrol
    /// as its base class.  This form is the same eqivalent of the cutom data form in OMS 1.0.
    /// The extended data underlying data source within the object passed will be used.
    /// </summary>
    public class ExtendedDataForm : FormRendererBase, FWBS.OMS.UI.Windows.Interfaces.IOMSItem
	{
		#region Events
		public event EventHandler Dirty = null;
		#endregion

		#region Fields

		/// <summary>
		/// Extended data object to render.
		/// </summary>
		private ExtendedData _ext = null;

		/// <summary>
		/// The underlying source of the extended data object.
		/// </summary>
		private DataSet _source = null;

		/// <summary>
		/// A dirty property that is used to check if there is any unsaved data.
		/// </summary>
		private bool _dirty = false;

		/// <summary>
		/// To Be Refresh when Active
		/// </summary>
		private bool _toberefresh = false;


		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExtendedDataForm()
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
            ((System.ComponentModel.ISupportInitialize)(this.req)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._questions)).BeginInit();
            this.SuspendLayout();
            // 
            // ExtendedDataForm
            // 
            this.AutoScroll = true;
            this.Name = "ExtendedDataForm";
            ((System.ComponentModel.ISupportInitialize)(this.req)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._questions)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the extended data object to the source of this form.  
		/// This object is created by the business layer.
		/// </summary>	
		[Browsable(false)]
		public ExtendedData ExtendedData
		{
			get
			{
				return _ext;
			}
			set
			{
				if (value != null)
				{
					_ext = value;

					//Set up the rendering data source so that the base render form
					//knows how to render the look of the form.					
					_source = _ext.GetSource(new FieldDisplayType(FieldDisplayTypeCallback));
					_questions = _source.Tables["QUESTIONS"];
					
					RenderControls(true);

				}
			}
		}

		#endregion

		#region Rendering Methods

		/// <summary>
		/// Renders a specific control with a matching settings data row.  This overrides certain
		/// functionality that FormRendererBase has.  The control will be created if a null reference 
		/// is given to the function.
		/// </summary>
		/// <param name="ctrl">Reference to a control.</param>
		/// <param name="settings">Settings data row object.</param>
		public override void RenderControl(ref Control ctrl, DataRow settings)
		{
			base.RenderControl(ref ctrl, settings);

			AnchorStyles anchor = ctrl.Anchor;

		
			//*************************************************
			//IBasicEnquiryControl specific property settings.
			//*************************************************
			if (ctrl is IBasicEnquiryControl2)
			{
				IBasicEnquiryControl2 basic = (IBasicEnquiryControl2)ctrl;
				basic.Value = _source.Tables["DATA"].Rows[0][ctrl.Name];
				basic.Changed -= new EventHandler(SourceDataChanged);
				basic.Changed += new EventHandler(SourceDataChanged);
			}	

			ctrl.Anchor = anchor;
			

		}


		#endregion

		#region Methods

		/// <summary>
		/// Captures the data changed event of the controls.
		/// </summary>
		/// <param name="sender">Binding contaxt object.</param>
		/// <param name="e">Empty event arguments.</param>
		private void SourceDataChanged(object sender, System.EventArgs e)
		{
			_dirty = true;
			OnDirty();
		}


		/// <summary>
		/// Used as a callback delegate from the Extended data object.  This method will
		/// determine what display control will be used for each of the field types passed
		/// to the method.
		/// </summary>
		/// <param name="column">Underlying field column.</param>
		/// <returns>Control type that is to be used to display.</returns>
		private Type FieldDisplayTypeCallback (DataColumn column)
		{
			switch (column.DataType.Name)
			{
				case "String":
					return typeof(FWBS.Common.UI.Windows.eTextBox2);
				case "DateTime":
					return typeof(ucDateTimePicker);
				case "Boolean":
					return typeof(FWBS.Common.UI.Windows.eCheckBox2);
				default:
					goto case "String";
			}
			
		}


		#endregion

		#region IOMSItem Implementation

		/// <summary>
		/// IOMSItem Member: Refreshes the data within this object.
		/// </summary>
		public void RefreshItem()
		{
			if (_ext == null) return;

			try
			{
				Cursor = Cursors.WaitCursor;
				_ext.RefreshExtendedData();
				ExtendedData = _ext;
				_dirty = false;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// IOMSItem Member: Updates the data within this object.
		/// </summary>
		public void UpdateItem()
		{
			if (_ext == null) return;
			
			try
			{
				Cursor = Cursors.WaitCursor;
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is IBasicEnquiryControl2)
					{
						IBasicEnquiryControl2 basic = (IBasicEnquiryControl2)ctrl;
						_ext.SetExtendedData(ctrl.Name, basic.Value);
					}
				}
				_ext.UpdateExtendedData();
				_dirty = false;
			}

			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// IOMSItem Member: Cancels the data within this object.
		/// </summary>
		public void CancelItem()
		{
			try
			{
				_dirty = false;
			}
			catch{}
		}

		/// <summary>
		/// IOMSItem Member: Called when the tab that this object sits on is clicked upon 
		/// from the OMS type dialog.
		/// </summary>
		public virtual void SelectItem()
		{
			base.Select();
		}



		/// <summary>
		/// IOMSItem Member: Gets a boolean value that indicates whether this class is holding any
		/// unsaved dirty data.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				if (_ext == null)
					return false;

				return (_dirty);;

			}
		}

		/// <summary>
		/// To Be Refreshed when Active
		/// </summary>
		[Browsable(false)]
		public bool ToBeRefreshed
		{
			get
			{
				return _toberefresh;
			}
			set
			{
				_toberefresh = value;
			}
		}


		#endregion

		#region EventMethods
		protected void OnDirty()
		{
			if (Dirty != null)
				Dirty(this,EventArgs.Empty);
		}
		#endregion
	}
}
