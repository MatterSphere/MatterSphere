using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.SearchEngine;

namespace FWBS.OMS.UI.Windows
{
    public class DataGridViewLabelColumn : DataGridViewTextBoxColumn, IBeforeCellDisplayable
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

        /// <summary>
        /// Create a Data List from the DataListName Property
        /// </summary>
        private FWBS.OMS.EnquiryEngine.DataLists _dataList = null;

        /// <summary>
        /// The Name of the DataList
        /// </summary>
        private string _dataListName = "";

        /// <summary>
        /// The Name of the Data Code Lookup Type
        /// </summary>
        private string _dataCodeType = "";

        /// <summary>
        /// The Parent Search List object for the Column
        /// </summary>
        private FWBS.OMS.SearchEngine.SearchList _searchlist = null;

        /// <summary>
        /// Contains the Data Table for the Data List;
        /// </summary>
        private DataTable _run = null;
        private readonly CultureInfo _cultureInfo;
        #endregion

        #region Constructors
        public DataGridViewLabelColumn()
        {
            this._cultureInfo = Session.CurrentSession.DefaultCultureInfo;
            this.CellTemplate = new DataGridViewLabelCell();
            this.HeaderCell = new DataGridViewControls.DataGridViewExColumnHeaderCell();
        }

        #endregion

        #region Interface Implementation

        public event EventHandler<CellDisplayEventArgs> BeforeCellDisplayEvent;

        public CellDisplayEventArgs OnBeforeCellDisplayEvent(int rownum, CurrencyManager source, string text, string columnname)
        {
            EventHandler<CellDisplayEventArgs> ev = BeforeCellDisplayEvent;
            if (ev != null)
            {
                DataView dv = source.List as DataView;
                if (dv != null)
                {
                    CellDisplayEventArgs e = new CellDisplayEventArgs(rownum, dv[rownum], text, columnname);
                    ev(this, e);
                    return e;
                }
            }
            return null;
        }

        #endregion

        #region Override Methods

        public override bool ReadOnly
        {
            get { return true; }
            set { }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                var cell = base.CellTemplate;
                if (!(cell is DataGridViewLabelCell))
                {
                    ThrowInvalidCastException(typeof(DataGridViewLabelCell));
                }
                return cell;
            }
            set
            {
                var type = typeof (DataGridViewLabelCell);
                if (value?.GetType().IsAssignableFrom(type) == false)
                {
                    ThrowInvalidCastException(type);
                }
                base.CellTemplate = value;
            }
        }

        private void ThrowInvalidCastException(Type target)
        {
            throw new OMSException2(
                "COLUMNCELLTYPE", 
                "Cell must be a '%1%' type", 
                new InvalidCastException(), 
                true,
                target.Name);
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
                //UTCFIX: DM - 27/11/06 - Make sure that a UTC date is converted to local time for displaying to user.
                if (value is Boolean)
                {
                    if (Convert.ToBoolean(value))
                        newValue = FWBS.OMS.Session.CurrentSession.Resources.GetResource("SLYES", "Yes", "").Text;
                    else
                        newValue = FWBS.OMS.Session.CurrentSession.Resources.GetResource("SLNO", "No", "").Text;
                }
                if (_dataList != null)
                {
                    string filter = "";
                    try
                    {
                        if (_run == null)
                        {
                            _run = _dataList.Run() as DataTable;
                            if (_run == null) _dataList = null;
                        }
                        if (_run != null)
                        {
                            try
                            {
                                filter = _run.Columns[0].ColumnName + " = " + Convert.ToString(value);
                                _run.DefaultView.RowFilter = filter;
                            }
                            catch
                            {
                                filter = _run.Columns[0].ColumnName + " = '" + Convert.ToString(value) + "'";
                                _run.DefaultView.RowFilter = filter;
                            }
                            if (_run.DefaultView.Count > 0)
                                newValue = _run.DefaultView[0][1];
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(new FWBS.OMS.OMSException2("ERRBADCOLDATA1", "Error using DataList '%1%' on Column '%2%'. Applying Filter '%3%'", ex, true, _dataListName, this.DataPropertyName, filter));
                        _dataList = null;
                    }
                }
                else if (_dataCodeType != "" && _run != null)
                {
                    string filter = "";
                    try
                    {
                        filter = "cdCode = '" + Convert.ToString(value) + "'";
                        _run.DefaultView.RowFilter = filter;
                        if (_run.DefaultView.Count > 0)
                            newValue = _run.DefaultView[0]["cddesc"];
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(new FWBS.OMS.OMSException2("ERRBADCOLDATA2", "Error using Code Lookup Type '%1%' on Column '%2%'. Applying Filter '%3%'", ex, true, _dataCodeType, this.DataPropertyName, filter));
                        _dataList = null;
                    }

                }

                if (DisplayDateAs == SearchColumnsDateIs.Local)
                {
                    if (value is DateTime)
                    {
                        DateTime datedata = (DateTime)value;
                        if (datedata.Kind == DateTimeKind.Unspecified)
                            newValue = datedata;
                        else
                            newValue = datedata.ToLocalTime();
                    }
                    return System.String.Format(_cultureInfo, "{0:" + this.DefaultCellStyle.Format + "}", newValue);
                }
                else if (DisplayDateAs == SearchColumnsDateIs.UTC)
                {
                    if (value is DateTime)
                    {
                        DateTime datedata = (DateTime)value;
                        if (datedata.Kind == DateTimeKind.Unspecified)
                            newValue = datedata;
                        else
                            newValue = datedata.ToUniversalTime();
                    }
                    return System.String.Format(_cultureInfo, "{0:" + this.DefaultCellStyle.Format + "}", newValue) + " (UTC)";
                }
                else if (this.DefaultCellStyle.Format != "")
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

        public FWBS.OMS.EnquiryEngine.DataLists DataLists
        {
            get { return _dataList; }
        }

        public DataTable Run
        {
            get { return _run; }
        }

        public DataTable DataListTable
        {
            get
            {
                if (_dataList != null)
                {
                    if (_run == null)
                    {
                        _run = _dataList.Run() as DataTable;
                        if (_run == null) _dataList = null;
                    }
                }
                return _run;
            }
        }

        public SearchColumnsDateIs SourceDateIs { get; set; } = SearchColumnsDateIs.NotApplicable;
        public SearchColumnsDateIs DisplayDateAs { get; set; } = SearchColumnsDateIs.NotApplicable;
        public IFormatProvider FormatInfo { get; set; } = null;

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
                _imageListScaled = Images.ScaleList(_imageList, DataGridView.LogicalToDeviceUnits(new Size(16, 16)));
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
                    int pad = DataGridView.LogicalToDeviceUnits(20);
                    if (DataGridView.RightToLeft == RightToLeft.Yes)
                        DefaultCellStyle.Padding = new Padding(DefaultCellStyle.Padding.Left, DefaultCellStyle.Padding.Top, pad, DefaultCellStyle.Padding.Bottom);
                    else
                        DefaultCellStyle.Padding = new Padding(pad, DefaultCellStyle.Padding.Top, DefaultCellStyle.Padding.Right, DefaultCellStyle.Padding.Bottom);
                }
            }
        }

        private void DataGridView_RightToLeftChanged(object sender, EventArgs e)
        {
            UpdateCellPadding();
        }

        [Category("Appearance")]
        [DefaultValue(omsImageLists.None)]
        [RefreshProperties(RefreshProperties.All)]
        public omsImageLists Resources
        {
            get { return _omsImageLists; }
            set
            {
                if (_omsImageLists != value)
                {
                    switch (value)
                    {
                        case omsImageLists.AdminMenu16:
                            {
                                ImageList = Windows.Images.AdminMenu16();
                                break;
                            }

                        case omsImageLists.AdminMenu32:
                            {
                                ImageList = Windows.Images.AdminMenu32();
                                break;
                            }

                        case omsImageLists.Arrows:
                            {
                                ImageList = Windows.Images.Arrows;
                                break;
                            }

                        case omsImageLists.PlusMinus:
                            {
                                ImageList = Windows.Images.PlusMinus;
                                break;
                            }

                        case omsImageLists.CoolButtons16:
                            {
                                ImageList = Windows.Images.CoolButtons16();
                                break;
                            }

                        case omsImageLists.CoolButtons24:
                            {
                                ImageList = Windows.Images.GetCoolButtons24();
                                break;
                            }

                        case omsImageLists.Developments16:
                            {
                                ImageList = Windows.Images.Developments();
                                break;
                            }

                        case omsImageLists.Entities16:
                            {
                                ImageList = Windows.Images.Entities();
                                break;
                            }

                        case omsImageLists.Entities32:
                            {
                                ImageList = Windows.Images.Entities32();
                                break;
                            }

                        case omsImageLists.imgFolderForms16:
                            {
                                ImageList = Windows.Images.GetFolderFormsIcons(Windows.Images.IconSize.Size16);
                                break;
                            }

                        case omsImageLists.imgFolderForms32:
                            {
                                ImageList = Windows.Images.GetFolderFormsIcons(Windows.Images.IconSize.Size32);
                                break;
                            }

                        case omsImageLists.None:
                            {
                                ImageList = null;
                                break;
                            }
                    }
                    _omsImageLists = value;
                }
            }
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

        [Category("Data")]
        [DefaultValue("")]
        public string DataListName
        {
            get { return _dataListName; }
            set
            {
                if (_dataListName != value)
                {
                    _dataListName = value;
                    try
                    {
                        _dataCodeType = "";
                        _dataList = new FWBS.OMS.EnquiryEngine.DataLists(_dataListName);
                        _dataList.ChangeParent(_searchlist.Parent);
                    }
                    catch (Exception ex)
                    {
                        _dataListName = "";
                        _dataList = null;
                        _run = null;
                        ErrorBox.Show(new FWBS.OMS.OMSException2("ERRINVDATALS", "Error invalid Data List name '%1%'",
                            ex, true, _dataListName));
                    }
                }
            }
        }

        [Category("Data")]
        [DefaultValue("")]
        public string DataCodeType
        {
            get { return _dataCodeType; }
            set
            {
                try
                {
                    if (_dataCodeType != value)
                    {
                        _dataListName = "";
                        _dataCodeType = value;
                        _run = FWBS.OMS.CodeLookup.GetLookups(_dataCodeType);
                    }
                }
                catch (Exception ex)
                {
                    _dataCodeType = "";
                    _run = null;
                    ErrorBox.Show(new FWBS.OMS.OMSException2("ERRINVCODETYP", "Error invalid code lookup type name '%1%'", ex, true, _dataCodeType));
                }
            }
        }

        [Browsable(false)]
        public FWBS.OMS.SearchEngine.SearchList SearchList
        {
            get { return _searchlist; }
            set { _searchlist = value; }
        }

        #endregion
    }
}
