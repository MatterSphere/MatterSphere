using System;
using System.IO;

namespace Fwbs.Documents
{
    sealed class OfficeWordDocument : FWBS.Common.IPasswordProtected, IRawDocument, ICustomPropertiesDocument, IDisposable
    {
        private readonly OfficeDocPropHandler _handler;
        private Aspose.Words.Document _doc;
        private bool _modified;
        private string _fileName;
        
        public OfficeWordDocument(OfficeDocPropHandler handler)
        {
            var lic = new Aspose.Words.License();
            lic.SetLicense("Aspose.Total.lic");
            _handler = handler;
        }

        private void Initialize()
        {
            Aspose.Words.FileFormatInfo info = Aspose.Words.FileFormatUtil.DetectFileFormat(_fileName);
            HasPassword = info.IsEncrypted;
            string password = HasPassword ? _handler.CachedPasswords[_fileName] as string ?? Environment.NewLine : null;
            _doc = new Aspose.Words.Document(_fileName, new Aspose.Words.Loading.LoadOptions(info.LoadFormat, password, null));
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
                catch (Aspose.Words.IncorrectPasswordException)
                {
                    if (!_handler.PasswordRequest(this))
                        _handler.ThrowPasswordRequestCancelled();
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
                    var saveOptions = Aspose.Words.Saving.SaveOptions.CreateSaveOptions(
                        Aspose.Words.FileFormatUtil.ExtensionToSaveFormat(Path.GetExtension(_fileName)));
                    saveOptions.UpdateFields = false;
                    saveOptions.UpdateSdtContent = false;
                    if (HasPassword)
                    {
                        if (saveOptions is Aspose.Words.Saving.DocSaveOptions)
                            ((Aspose.Words.Saving.DocSaveOptions)saveOptions).Password = (string)_handler.CachedPasswords[_fileName];
                        else if (saveOptions is Aspose.Words.Saving.OoxmlSaveOptions)
                            ((Aspose.Words.Saving.OoxmlSaveOptions)saveOptions).Password = (string)_handler.CachedPasswords[_fileName];
                    }
                    _doc.Save(_fileName, saveOptions);
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
            _doc = null;
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

            WordPropertyConverter conv = new WordPropertyConverter();

            foreach (Aspose.Words.Properties.DocumentProperty docProp in _doc.CustomDocumentProperties)
            {
                CustomProperty prop = properties.Add(docProp.Name);
                prop.Value = conv.FromSource(docProp.Value, docProp.Type);
            }

            foreach (string knownProperty in _handler.KnownProperties)
            {
                if (!properties.Contains(knownProperty))
                {
                    string value = _doc.Variables[knownProperty];
                    if (!string.IsNullOrWhiteSpace(value))
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

            WordPropertyConverter conv = new WordPropertyConverter();
            Aspose.Words.Properties.CustomDocumentProperties docProps = _doc.CustomDocumentProperties;

            foreach (CustomProperty prop in properties)
            {
                if (prop.IsDeleted)
                {
                    if (docProps.Contains(prop.Name))
                    {
                        docProps.Remove(prop.Name);
                        _modified = true;
                    }
                    if (_doc.Variables.Contains(prop.Name))
                    {
                        _doc.Variables.Remove(prop.Name);
                        _modified = true;
                    }
                }
                else//if (prop.HasChanged)
                {
                    object value = conv.ToSource(prop.Value);
                    Aspose.Words.Properties.DocumentProperty docProp = docProps[prop.Name];

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

                    string svalue = Convert.ToString(prop.Value, System.Globalization.CultureInfo.InvariantCulture);
                    if (!_doc.Variables.Contains(prop.Name))
                    {
                        _doc.Variables.Add(prop.Name, svalue);
                        _modified = true;
                    }
                    else if (_doc.Variables[prop.Name] != svalue)
                    {
                        _doc.Variables[prop.Name] = svalue;
                        _modified = true;
                    }
                }
            }
        }

        #endregion

        #region PropertyConverter

        sealed class WordPropertyConverter : IPropertyConverter<Aspose.Words.Properties.PropertyType, object>
        {
            private ConversionMethods methods = new ConversionMethods();

            public object FromSource(object value, Aspose.Words.Properties.PropertyType type)
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

            public Aspose.Words.Properties.PropertyType ToSourceType(Type type)
            {
                if (type == typeof(bool))
                    return Aspose.Words.Properties.PropertyType.Boolean;
                else if (type == typeof(DateTime))
                    return Aspose.Words.Properties.PropertyType.DateTime;
                else if (type == typeof(double))
                    return Aspose.Words.Properties.PropertyType.Double;
                else if (type == typeof(int))
                    return Aspose.Words.Properties.PropertyType.Number;
                else if (type == typeof(string))
                    return Aspose.Words.Properties.PropertyType.String;
                else
                    return Aspose.Words.Properties.PropertyType.String;
            }

            public Type FromSourceType(Aspose.Words.Properties.PropertyType type)
            {
                switch (type)
                {
                    case Aspose.Words.Properties.PropertyType.Boolean:
                        return typeof(bool);
                    case Aspose.Words.Properties.PropertyType.DateTime:
                        return typeof(DateTime);
                    case Aspose.Words.Properties.PropertyType.Double:
                        return typeof(double);
                    case Aspose.Words.Properties.PropertyType.Number:
                        return typeof(int);
                    case Aspose.Words.Properties.PropertyType.String:
                        return typeof(string);
                    default:
                        return typeof(string);
                }
            }
        }

        #endregion
    }
}
