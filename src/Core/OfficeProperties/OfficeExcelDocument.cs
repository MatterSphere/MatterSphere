using System;
using System.IO;

namespace Fwbs.Documents
{
    sealed class OfficeExcelDocument : FWBS.Common.IPasswordProtected, IRawDocument, ICustomPropertiesDocument, IDisposable
    {
        private readonly OfficeDocPropHandler _handler;
        private Aspose.Cells.Workbook _doc;
        private bool _modified;
        private string _fileName;
        private const string VARIABLESHEETNAME = "_VARIABLES";

        public OfficeExcelDocument(OfficeDocPropHandler handler)
        {
            var lic = new Aspose.Cells.License();
            lic.SetLicense("Aspose.Total.lic");
            _handler = handler;
        }

        private void Initialize()
        {
            Aspose.Cells.FileFormatInfo info = Aspose.Cells.FileFormatUtil.DetectFileFormat(_fileName);
            HasPassword = info.IsEncrypted;
            string password = HasPassword ? _handler.CachedPasswords[_fileName] as string ?? Environment.NewLine : null;
            _doc = new Aspose.Cells.Workbook(_fileName, new Aspose.Cells.LoadOptions(info.LoadFormat) { Password = password });
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        #region IPasswordProtected

        public bool IsInternal { get { return false; } }

        public string PasswordHint { get { return string.Empty; } }

        public bool HasPassword { get; private set; }

        public string CurrentPassword
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    _handler.CachedPasswords.Remove(_fileName);
                else
                    _handler.CachedPasswords.Add(_fileName, value);
            }
        }

        public string ToPasswordString() { return Path.GetFileName(_fileName); }

        public void ValidatePassword() { Initialize(); }

        public void PasswordAuthenticate(string userName, string password) { throw new NotImplementedException(); }

        #endregion

        #region IRawDocument Members

        public void Open(FileInfo file)
        {
            if (!IsOpen)
            {
                if (file == null)
                {
                    throw new ArgumentNullException("file");
                }

                if (!File.Exists(file.FullName))
                {
                    throw new FileNotFoundException("", file.FullName);
                }

                try
                {
                    _fileName = file.FullName;
                    Initialize();
                }
                catch (Aspose.Cells.CellsException ex)
                {
                    if (ex.Code == Aspose.Cells.ExceptionType.IncorrectPassword)
                    {
                        if (!_handler.PasswordRequest(this))
                            _handler.ThrowPasswordRequestCancelled();
                    }
                    else
                    {
                        throw new IOException(ex.Message, ex);
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException(ex.Message, ex);
                }
            }
        }

        public void Save()
        {
            if (!IsOpen)
            {
                throw new FileClosedException();
            }

            if (_modified)
            {
                try
                {
                    _doc.Save(_fileName);
                    _modified = false;
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new IOException(ex.Message, ex);
                }
            }
        }

        public void Close()
        {
            _modified = false;
            if (_doc != null)
            {
                _doc.Dispose();
                _doc = null;
            }
        }

        public bool IsOpen
        {
            get { return _doc != null; }
        }

        #endregion

        #region ICustomPropertiesDocument Members

        public void ReadCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            if (!IsOpen)
            {
                throw new FileClosedException();
            }

            ExcelPropertyConverter conv = new ExcelPropertyConverter();

            foreach (Aspose.Cells.Properties.DocumentProperty docProp in _doc.CustomDocumentProperties)
            {
                CustomProperty prop = properties.Add(docProp.Name);
                prop.Value = conv.FromSource(docProp.Value, docProp.Type);
            }

            foreach (string knownProperty in _handler.KnownProperties)
            {
                if (!properties.Contains(knownProperty))
                {
                    object value = GetDocVariable(knownProperty);
                    if (value != null)
                    {
                        properties.Add(knownProperty).Value = value;
                    }
                }
            }

            properties.Accept();
        }

        public void WriteCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            if (!IsOpen)
            {
                throw new FileClosedException();
            }

            ExcelPropertyConverter conv = new ExcelPropertyConverter();
            Aspose.Cells.Properties.CustomDocumentPropertyCollection docProps = _doc.CustomDocumentProperties;

            foreach (CustomProperty prop in properties)
            {
                if (prop.IsDeleted)
                {
                    if (docProps.Contains(prop.Name))
                    {
                        docProps.Remove(prop.Name);
                        _modified = true;
                    }
                    if (RemoveDocVariable(prop.Name))
                    {
                        _modified = true;
                    }
                }
                else//if (prop.HasChanged)
                {
                    object value = conv.ToSource(prop.Value);
                    Aspose.Cells.Properties.DocumentProperty docProp = docProps[prop.Name];

                    if (docProp != null && conv.ToSourceType(value.GetType()) != docProp.Type)
                    {
                        docProps.Remove(prop.Name);
                        docProp = null;
                    }

                    if (docProp == null)
                    {
                        if (value.GetType() == typeof(string))
                            docProps.Add(prop.Name, (string)value);
                        else if (value.GetType() == typeof(bool))
                            docProps.Add(prop.Name, (bool)value);
                        else if (value.GetType() == typeof(int))
                            docProps.Add(prop.Name, (int)value);
                        else if (value.GetType() == typeof(double))
                            docProps.Add(prop.Name, (double)value);
                        else if (value.GetType() == typeof(DateTime))
                            docProps.Add(prop.Name, (DateTime)value);
                        _modified = true;
                    }
                    else if (!Equals(docProp.Value, value))
                    {
                        docProp.Value = value;
                        _modified = true;
                    }

                    if (SetDocVariable(prop.Name, value))
                    {
                        _modified = true;
                    }
                }
            }
        }

        #endregion

        #region Excel Variables

        private Aspose.Cells.Worksheet GetOrCreateVariableSheet()
        {
            Aspose.Cells.Worksheet varsheet = _doc.Worksheets[VARIABLESHEETNAME];
            if (varsheet == null)
                varsheet = _doc.Worksheets.Add(VARIABLESHEETNAME);

            if (varsheet.VisibilityType != Aspose.Cells.VisibilityType.VeryHidden)
                varsheet.VisibilityType = Aspose.Cells.VisibilityType.VeryHidden;

            return varsheet;
        }

        private Aspose.Cells.Cell GetVarCells(string varName, out Aspose.Cells.Cell cellVal)
        {
            string name;
            Aspose.Cells.Cell cellName = null; cellVal = null;
            Aspose.Cells.Worksheet varsheet = GetOrCreateVariableSheet();

            for (int row = 0; row < 1000; row++)
            {
                cellName = varsheet.Cells[row, 0];
                cellVal = varsheet.Cells[row, 1];

                name = cellName.GetStringValue(Aspose.Cells.CellValueFormatStrategy.None);
                if (string.IsNullOrEmpty(name) || varName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    break;
            }

            return cellName;
        }

        private object GetDocVariable(string varName)
        {
            Aspose.Cells.Cell cellName, cellVal;
            cellName = GetVarCells(varName, out cellVal);

            if (varName.Equals(cellName.GetStringValue(Aspose.Cells.CellValueFormatStrategy.None), StringComparison.InvariantCultureIgnoreCase))
                return cellVal.Value;
            else
                return null;
        }

        private bool SetDocVariable(string varName, object varValue)
        {
            Aspose.Cells.Cell cellName, cellVal;
            cellName = GetVarCells(varName, out cellVal);

            if (cellName.GetStringValue(Aspose.Cells.CellValueFormatStrategy.None) != varName || !Equals(cellVal.Value, varValue))
            {
                cellName.PutValue(varName);
                cellVal.PutValue(varValue);
                return true;
            }

            return false;
        }

        private bool RemoveDocVariable(string varName)
        {
            Aspose.Cells.Cell cellName, cellVal;
            cellName = GetVarCells(varName, out cellVal);

            if (varName.Equals(cellName.GetStringValue(Aspose.Cells.CellValueFormatStrategy.None), StringComparison.InvariantCultureIgnoreCase))
            {
                cellName.Worksheet.Cells.DeleteRow(cellName.Row);
                return true;
            }

            return false;
        }

        #endregion

        #region PropertyConverter

        sealed class ExcelPropertyConverter : IPropertyConverter<Aspose.Cells.Properties.PropertyType, object>
        {
            private ConversionMethods methods = new ConversionMethods();

            public object FromSource(object value, Aspose.Cells.Properties.PropertyType type)
            {
                if (value == null)
                    return null;

                IFormatProvider provider = System.Globalization.CultureInfo.InvariantCulture;
                Type t = FromSourceType(type);
                return methods[t](value, provider);
            }

            public object ToSource(object value)
            {
                if (value == null)
                    return null;

                IFormatProvider provider = System.Globalization.CultureInfo.InvariantCulture;

                Type type = ConvertType(value.GetType());
                value = methods[type](value, provider);

                if (value is DateTime)
                {
                    DateTime date = (DateTime)value;
                    if (date.Kind == DateTimeKind.Unspecified)
                        date = DateTime.SpecifyKind(date, DateTimeKind.Local);

                    return date.ToUniversalTime();
                }

                return value;
            }

            public Type ConvertType(Type type)
            {
                if (type == typeof(int) ||
                   type == typeof(byte) ||
                   type == typeof(short) ||
                   type == typeof(sbyte) ||
                   type == typeof(ushort))
                    return typeof(int);

                if (type == typeof(bool))
                    return typeof(bool);

                if (type == typeof(string) ||
                    type == typeof(char) ||
                    type == typeof(System.Text.StringBuilder) ||
                    type == typeof(Guid))
                    return typeof(string);

                if (type == typeof(float) ||
                    type == typeof(double) ||
                    type == typeof(long) ||
                    type == typeof(uint) ||
                    type == typeof(ulong) ||
                    type == typeof(decimal))
                    return typeof(double);

                if (type == typeof(DateTime))
                    return typeof(DateTime);

                return typeof(string);
            }

            public Aspose.Cells.Properties.PropertyType ToSourceType(Type type)
            {
                if (type == typeof(bool))
                    return Aspose.Cells.Properties.PropertyType.Boolean;
                else if (type == typeof(DateTime))
                    return Aspose.Cells.Properties.PropertyType.DateTime;
                else if (type == typeof(double))
                    return Aspose.Cells.Properties.PropertyType.Double;
                else if (type == typeof(int))
                    return Aspose.Cells.Properties.PropertyType.Number;
                else if (type == typeof(string))
                    return Aspose.Cells.Properties.PropertyType.String;
                else
                    return Aspose.Cells.Properties.PropertyType.String;
            }

            public Type FromSourceType(Aspose.Cells.Properties.PropertyType type)
            {
                switch (type)
                {
                    case Aspose.Cells.Properties.PropertyType.Boolean:
                        return typeof(bool);
                    case Aspose.Cells.Properties.PropertyType.DateTime:
                        return typeof(DateTime);
                    case Aspose.Cells.Properties.PropertyType.Double:
                        return typeof(double);
                    case Aspose.Cells.Properties.PropertyType.Number:
                        return typeof(int);
                    case Aspose.Cells.Properties.PropertyType.String:
                        return typeof(string);
                    default:
                        return typeof(string);
                }
            }
        }

        #endregion
    }
}
