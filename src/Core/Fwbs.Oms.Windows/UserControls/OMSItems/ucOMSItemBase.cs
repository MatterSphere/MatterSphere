using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucOMSItemBase : UserControl, IOMSItem, IOpenOMSType
    {
        public ucOMSItemBase()
        {
            InitializeComponent();
        }

        #region Events

        /// <summary>
        /// This event gets raised when the cancel or OK button get clicked.
        /// </summary>
        public event NewOMSTypeCloseEventHander Close = null;

        /// <summary>
        /// An event that gets raised when a new OMS type object needs to be opened in
        /// a navigational format on the dialog form.
        /// </summary>
        public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;

        /// <summary>
        /// On Dirty Event
        /// </summary>
        public event EventHandler Dirty = null;

        #endregion

        #region Method Events

        protected void OnDirty(object sender, EventArgs e)
        {
            if (Dirty != null)
                Dirty(this, EventArgs.Empty);
        }

        /// <summary>
        /// Forces the close event to be raised.
        /// </summary>
        protected void OnClose(ClosingWhy Why)
        {
            if (Close != null)
                Close(this, new NewOMSTypeCloseEventArgs(Why));
        }

        /// <summary>
        /// Raises the OnNewOMSTypeWindow event with the specified event arguments.
        /// </summary>
        /// <param name="sender">This OMSItem control.</param>
        /// <param name="e">NewOMSTypeWindow Event arguments.</param>
        protected void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            if (NewOMSTypeWindow != null)
                NewOMSTypeWindow(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows a Public Cancel Item with all the UI Accouterments
        /// http://dictionary.reference.com/search?r=2&q=Accouterments
        /// </summary>
        public virtual void CancelUIItem()
        {

        } 

        #endregion

        #region IOMSItem Implementation


        /// <summary>
        /// Refreshes any data change to the underlying enquiry form.
        /// </summary>
        public virtual void RefreshItem()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                enquiryForm1.RefreshItem();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// IOMSItem Member: Updates the data within this object.
        /// </summary>
        public virtual void UpdateItem()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                enquiryForm1.UpdateItem();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Cancels any data change to the underlying enquiry form.
        /// </summary>
        public void CancelItem()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                enquiryForm1.CancelItem();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// IOMSItem Member: Called when the tab that this object sits on is clicked upon 
        /// from the OMS type dialog.
        /// </summary>
        public void SelectItem()
        {
        }

        /// <summary>
        /// IOMSItem Member: Gets a boolean value that indicates whether this class is holding any
        /// unsaved dirty data.
        /// </summary>
        public virtual bool IsDirty
        {
            get
            {
                return (enquiryForm1.IsDirty);
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
                return (enquiryForm1.ToBeRefreshed);
            }
            set
            {
                enquiryForm1.ToBeRefreshed = value;
            }
        }

        #endregion
    }
}
