using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewCustomTextColumn : DataGridViewTextBoxColumn
    {
        #region Fields

        /// <summary>
        /// The image list that the column will look at.
        /// </summary>
        private ImageList _imageList = null;
        private ImageList _imageListScaled = null;

        /// <summary>
        /// The image column name.
        /// </summary>
        private string _imageColumn = "";

        /// <summary>
        /// The image index.
        /// </summary>
        private int _imageIndex = -1;

        /// <summary>
        /// Retains the Resource State
        /// </summary>
        private omsImageLists _omsImageLists = omsImageLists.None;


        private readonly CultureInfo _cultureInfo;
        #endregion

        #region Constructors
        public DataGridViewCustomTextColumn()
        {
            this.CellTemplate = new DataGridViewCustomTextCell();
            this.DefaultCellStyle.ForeColor = Color.FromArgb(21, 101, 192);
            this.DefaultCellStyle.SelectionForeColor = Color.FromArgb(21, 101, 192);
        }

        #endregion

        #region Override Methods

        public override bool ReadOnly
        {
            get { return true; }
            set { }
        }

        protected override void OnDataGridViewChanged()
        {
            base.OnDataGridViewChanged();
            if (DataGridView != null)
            {
                DataGridView.DpiChangedBeforeParent -= ScaleImageList;
                ScaleImageList(DataGridView, EventArgs.Empty);
                DataGridView.DpiChangedBeforeParent += ScaleImageList;

                DataGridView.RightToLeftChanged -= DataGridView_RightToLeftChanged;
                DataGridView.RightToLeftChanged += DataGridView_RightToLeftChanged;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _imageListScaled != null)
            {
                _imageListScaled.Dispose();
                _imageListScaled = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Public Methods
        public string GetText(object value)
        {
            object newValue = value;
            if (value == System.DBNull.Value)
            {
                return this.DefaultCellStyle.NullValue.ToString();
            }
            if (value != null)
            {
                if (this.DefaultCellStyle.Format != "")
                    return System.String.Format(_cultureInfo, "{0:" + this.DefaultCellStyle.Format + "}", newValue);
                else
                    return Convert.ToString(newValue, _cultureInfo);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Properties

        public bool HasImages
        {
            get { return (ImageList != null) && (ImageIndex != -1 || ImageColumn != ""); }
        }

        /// <summary>
        /// Gets or Sets the bound image list associated with the column.
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return _imageListScaled;
            }
            set
            {
                _imageList = value;
                ScaleImageList(DataGridView, EventArgs.Empty);
            }
        }

        private void ScaleImageList(object sender, EventArgs e)
        {
            if (_imageListScaled != null)
            {
                _imageListScaled.Dispose();
                _imageListScaled = null;
            }
            if (_imageList != null && DataGridView != null)
            {
                _imageListScaled = Windows.Images.ScaleList(_imageList, DataGridView.LogicalToDeviceUnits(new Size(16, 16)));
            }
            UpdateCellPadding();
        }

        private void UpdateCellPadding()
        {
            if (DataGridView != null)
            {
                DefaultCellStyle.Padding = DataGridView.DefaultCellStyle.Padding;
                if (HasImages)
                {
                    int pad = DataGridView.LogicalToDeviceUnits(30);
                    DefaultCellStyle.Padding = DataGridView.RightToLeft == RightToLeft.Yes
                        ? new Padding(DefaultCellStyle.Padding.Left, DefaultCellStyle.Padding.Top, pad, DefaultCellStyle.Padding.Bottom)
                        : new Padding(pad, DefaultCellStyle.Padding.Top, DefaultCellStyle.Padding.Right, DefaultCellStyle.Padding.Bottom);
                }
            }
        }

        private void DataGridView_RightToLeftChanged(object sender, EventArgs e)
        {
            UpdateCellPadding();
        }

        [Category("Appearance")]
        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor(typeof(IconDisplayEditor), typeof(UITypeEditor))]
        [DefaultValue(null)]
        public int ImageIndex
        {
            get
            {
                return _imageIndex;
            }
            set
            {
                _imageIndex = value;
                UpdateCellPadding();
            }
        }

        /// <summary>
        /// Gets or Sets the bound image column to map to an image index.
        /// </summary>
        public string ImageColumn
        {
            get
            {
                return _imageColumn;
            }
            set
            {
                _imageColumn = value;
                UpdateCellPadding();
            }
        }
        
        #endregion
    }
}
