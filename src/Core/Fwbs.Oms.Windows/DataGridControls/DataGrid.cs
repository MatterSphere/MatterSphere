using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class DataGridEx : DataGrid
    {
        private class TruncatedStringConverter : StringConverter
        {
            private readonly int _maxLength;

            public TruncatedStringConverter(int maxLength = 8192)
            {
                _maxLength = maxLength;
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                string str = (string)base.ConvertTo(context, culture, value, destinationType);
                return str.Length <= _maxLength ? str : str.Substring(0, _maxLength);
            }
        }

        private class GridImageTraits
        {
            private class Trait
            {
                private FieldInfo _imageField;
                private Bitmap _scaledBitmap;
                private Bitmap _originalBitmap;

                public Trait(DataGrid dataGrid, object obj, string fieldName, string methodName, params object[] parameters)
                {
                    Type type = obj.GetType();
                    _imageField = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
                    if (_imageField == null)
                        _imageField = type.BaseType.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);

                    if (_imageField != null)
                    {
                        _scaledBitmap = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, parameters) as Bitmap;
                        dataGrid.ScaleBitmapLogicalToDevice(ref _scaledBitmap);
                        _imageField.SetValue(null, null); // clear cached disposed bitmap
                    }
                    else
                    {
                        throw new MissingMemberException(type.Name, fieldName);
                    }
                }

                public void SetBitmap(bool scaled)
                {
                    if (scaled)
                    {
                        _originalBitmap = _imageField.GetValue(null) as Bitmap;
                        _imageField.SetValue(null, _scaledBitmap);
                    }
                    else
                    {
                        _imageField.SetValue(null, _originalBitmap);
                    }
                }
            }

            private List<Trait> _traits = new List<Trait>();

            private GridImageTraits()
            {
            }

            public static GridImageTraits NewRowHeaderImageTraits(DataGrid dataGrid)
            {
                GridImageTraits git = null;
                try
                {
                    Array rows = typeof(DataGrid).GetProperty("DataGridRows", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dataGrid) as Array;
                    if (rows != null && rows.Length > 0)
                    {
                        object row = rows.GetValue(0);
                        git = new GridImageTraits();
                        git._traits.Add(new Trait(dataGrid, row, "leftArrow", "GetLeftArrowBitmap"));
                        git._traits.Add(new Trait(dataGrid, row, "rightArrow", "GetRightArrowBitmap"));
                        git._traits.Add(new Trait(dataGrid, row, "starBmp", "GetStarBitmap"));
                        git._traits.Add(new Trait(dataGrid, row, "pencilBmp", "GetPencilBitmap"));
                        git._traits.Add(new Trait(dataGrid, row, "errorBmp", "GetErrorBitmap"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    git = null;
                }
                return git;
            }

            public static GridImageTraits NewCaptionImageTraits(DataGrid dataGrid)
            {
                GridImageTraits git = null;
                try
                {
                    object caption = typeof(DataGrid).GetProperty("Caption", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dataGrid);
                    if (caption != null)
                    {
                        git = new GridImageTraits();
                        git._traits.Add(new Trait(dataGrid, caption, "leftButtonBitmap", "GetBackButtonBmp", false));
                        git._traits.Add(new Trait(dataGrid, caption, "leftButtonBitmap_bidi", "GetBackButtonBmp", true));
                        git._traits.Add(new Trait(dataGrid, caption, "magnifyingGlassBitmap", "GetDetailsBmp"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    git = null;
                }
                return git;
            }

            public void SetBitmaps(bool scaled)
            {
                foreach (Trait trait in _traits)
                {
                    trait.SetBitmap(scaled);
                }
            }
        }

        private static readonly Dictionary<int, GridImageTraits> _rowHeaderImageTraits = new Dictionary<int, GridImageTraits>();
        private static readonly Dictionary<int, GridImageTraits> _captionImageTraits = new Dictionary<int, GridImageTraits>();

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            if (factor.Width != 1 && (specified & BoundsSpecified.Size) != 0)
            {
                RowHeaderWidth = Convert.ToInt32(RowHeaderWidth * factor.Width);
                PreferredColumnWidth = Convert.ToInt32(PreferredColumnWidth * factor.Width);
                PreferredRowHeight = Convert.ToInt32(PreferredRowHeight * factor.Height);

                foreach (DataGridTableStyle tableStyle in TableStyles)
                {
                    tableStyle.RowHeaderWidth = Convert.ToInt32(tableStyle.RowHeaderWidth * factor.Width);
                    tableStyle.PreferredColumnWidth = Convert.ToInt32(tableStyle.PreferredColumnWidth * factor.Height);
                    tableStyle.PreferredRowHeight = Convert.ToInt32(tableStyle.PreferredRowHeight * factor.Height);

                    foreach (DataGridColumnStyle columnStyle in tableStyle.GridColumnStyles)
                    {
                        columnStyle.Width = Convert.ToInt32(columnStyle.Width * factor.Width);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (RowHeadersVisible || (CaptionVisible && AllowNavigation))
            {
                GridImageTraits headerTraits = null, captionTraits = null;

                if (RowHeadersVisible && !_rowHeaderImageTraits.TryGetValue(DeviceDpi, out headerTraits))
                {
                    headerTraits = GridImageTraits.NewRowHeaderImageTraits(this);
                    if (headerTraits != null)
                        _rowHeaderImageTraits[DeviceDpi] = headerTraits;
                }

                if (CaptionVisible && AllowNavigation && !_captionImageTraits.TryGetValue(DeviceDpi, out captionTraits))
                {
                    captionTraits = GridImageTraits.NewCaptionImageTraits(this);
                    if (captionTraits != null)
                        _captionImageTraits[DeviceDpi] = captionTraits;
                }

                headerTraits?.SetBitmaps(true);
                captionTraits?.SetBitmaps(true);

                base.OnPaint(pe);

                headerTraits?.SetBitmaps(false);
                captionTraits?.SetBitmaps(false);
            }
            else
            {
                base.OnPaint(pe);
            }
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);

            if (!ReadOnly || TableStyles.Count > 0)
                return;

            DataGridTableStyle gridTable = typeof(DataGrid).GetField("myGridTable", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this) as DataGridTableStyle;
            if (gridTable != null && string.IsNullOrEmpty(gridTable.MappingName))
            {
                FieldInfo typeConverterField = null;
                TruncatedStringConverter stringConverter = new TruncatedStringConverter();

                foreach (DataGridColumnStyle column in gridTable.GridColumnStyles)
                {
                    if (column.GetType() == typeof(DataGridTextBoxColumn) && column.PropertyDescriptor.PropertyType == typeof(string))
                    {
                        DataGridTextBoxColumn textColumn = (DataGridTextBoxColumn)column;
                        if (textColumn.FormatInfo == null && textColumn.Format == string.Empty)
                        {
                            if (typeConverterField == null)
                                typeConverterField = typeof(DataGridTextBoxColumn).GetField("typeConverter", BindingFlags.Instance | BindingFlags.NonPublic);

                            TypeConverter typeConverter = typeConverterField?.GetValue(textColumn) as TypeConverter;
                            if (typeConverter != null && typeConverter.GetType() == typeof(StringConverter))
                            {
                                typeConverterField.SetValue(textColumn, stringConverter);
                            }
                        }
                    }
                }
            }
        }

        public ScrollBar VerticalScrollBar
        {
            get { return VertScrollBar; }
        }

        public ScrollBar HorizontalScrollBar
        {
            get { return HorizScrollBar; }
        }
    }
}
