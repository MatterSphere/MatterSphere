using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A tick box list enquiry control which populates the list with a bit mask enumeration.
    /// This control uses a CheckedListBox as its base control rather than eBase.
    /// This particular control is a way of displaying bit mask option combinations.
    /// </summary>
    public class eCheckBoxListBoxBitMasks : System.Windows.Forms.CheckedListBox, IBasicEnquiryControl2
	{
		#region Events

		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler Changed;

		[Category("Action")]
		public event EventHandler ActiveChanged;

		#endregion

		#region Design Mode
		private bool _omsdesignmode = false;
		[Browsable(false)]
		public bool omsDesignMode
		{
			get
			{
				return _omsdesignmode;
			}
			set
			{
				_omsdesignmode = value;
			}
		}

		#endregion

		#region Fields

		/// <summary>
		/// Stores how wide the caption should be.
		/// </summary>
		private int _captionWidth = 0;

		/// <summary>
		/// Holds the boolean value for the read only attribute.
		/// </summary>
		private bool _required;

		/// <summary>
		/// Holds the Enumeration Name
		/// </summary>
		private string _enumname;

		private bool _isdirty = false;


		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instanve of this control.
		/// </summary>
		public eCheckBoxListBoxBitMasks()
		{
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
			// eCheckBoxListBoxBitMasks
			// 
			this.ItemCheck +=new ItemCheckEventHandler(eCheckBoxListBoxBitMasks_ItemCheck);
			this.Leave +=new EventHandler(this.RaiseLeaveEvent);

		}
		#endregion

		#region IBasicEnquiryControl2 Implementation

		/// <summary>
		/// Gets whether the current control can be stretched by its Y co-ordinate.
		/// This is a design mode property and is set to true.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		public bool LockHeight 
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets or Sets the value of the control.  In this case it should be True of False.
		/// </summary>
		[DefaultValue(0)]
		public object Value
		{
			get
			{
				return ProcessBitMask();
			}
			set
			{
				if (value != null && value != System.DBNull.Value)
				{
					try
					{
						ProcessBitMask(Convert.ToInt32(value));
					}
					catch
					{
					}
				}
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}

		/// <summary>
		/// Gets or Sets the flag that tells the rendering form that this control is required
		/// to be filled incase the underlying value is DBNull.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Required 
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}

		/// <summary>
		/// Gets or Sets the editable format of the control.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool ReadOnly 
		{
			get
			{
				return !base.Enabled;
			}
			set
			{
				base.Enabled = !value;
			}
		}

		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
			}
		}


		/// <summary>
		/// Gets or Sets the caption width of a control, leaving the rest of the width of the control
		/// </summary>
		[DefaultValue(150)]
		[Category("Layout")]
		public virtual int CaptionWidth
		{
			get
			{
				return _captionWidth;
			}
			set
			{
				_captionWidth = value;
			}
		}

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnChanged()
		{
			if (Changed!= null && IsDirty)
				Changed(this, EventArgs.Empty);
			IsDirty = false;
		}

		public void OnActiveChanged()
		{
			IsDirty=true;
			if (ActiveChanged!= null)
				ActiveChanged(this, EventArgs.Empty);
		}
		#endregion

		#region Methods

		/// <summary>
		/// Processes the items chosen in the list and combining the values into a bitmask value.
		/// </summary>
		/// <returns>The value with the combined values.</returns>
		private int ProcessBitMask()
		{
			int rv = 0;
			int bm = 1;
			for(int t = 0; t <= this.Items.Count-1; t++)
			{
				if (this.GetItemChecked(t))
					rv = rv + bm;
				bm = bm + bm;
			}
			return rv;
		}
		

		/// <summary>
		/// Process the value passed and tick the checkbox items that are involved in the bit mask.
		/// </summary>
		/// <param name="MyValue">The total mask value.</param>
		private void ProcessBitMask(int MyValue)
		{
			int bm = 1;
			for(int t = 0; t <= this.Items.Count-1; t++)
			{
				if ((MyValue & bm) != 0)
					this.SetItemChecked(t,true);
				else
					this.SetItemChecked(t,false);
				bm = bm + bm;
			}
		}

		/// <summary>
		/// Raises the changed event within the base control.
		/// </summary>
		protected void RaiseActiveChangedEvent(object sender, System.EventArgs e)
		{
			OnActiveChanged();
			this.SelectedIndex=-1;
		}

		protected void RaiseChangedEvent(object sender, System.EventArgs e)
		{
			OnChanged();
		}

		/// <summary>
		/// Raises the leave event within the base control.
		/// </summary>
		protected void RaiseLeaveEvent(object sender, System.EventArgs e)
		{
			//OnLeave(e);
			OnChanged();
		}

		#endregion

		#region Properties
		[Browsable(true)]
		public object Control
		{
			get
			{
				return this;
			}
		}

		public string EnumName
		{
			get
			{
				return _enumname;
			}
			set
			{
				_enumname = value;
				this.Items.Clear();
				ArrayList ar = FWBS.Common.EnumListItem.EnumToList(_enumname);
				foreach(FWBS.Common.EnumListItem eli in ar)
					this.Items.Add(eli.Name);
			}
		}
		#endregion

		private void eCheckBoxListBoxBitMasks_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			this.RaiseActiveChangedEvent(sender,EventArgs.Empty);
		}
	}
}
