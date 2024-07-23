using System;
using System.IO;

namespace Fwbs.Documents
{
    sealed class OfficePowerpointDocument : FWBS.Common.IPasswordProtected, IRawDocument, ICustomPropertiesDocument, IDisposable
    {
        private readonly OfficeDocPropHandler _handler;
        private Aspose.Slides.Presentation _doc;
        private bool _modified;
        private string _fileName;

        public OfficePowerpointDocument(OfficeDocPropHandler handler)
        {
            var lic = new Aspose.Slides.License();
            lic.SetLicense("Aspose.Total.lic");
            _handler = handler;
        }

        private void Initialize()
        {
            Aspose.Slides.IPresentationInfo info = Aspose.Slides.PresentationFactory.Instance.GetPresentationInfo(_fileName);
            HasPassword = info.IsEncrypted;
            string password = HasPassword ? _handler.CachedPasswords[_fileName] as string ?? Environment.NewLine : null;
            _doc = new Aspose.Slides.Presentation(_fileName, new Aspose.Slides.LoadOptions(info.LoadFormat) { Password = password });
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
                catch (Aspose.Slides.InvalidPasswordException)
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
                    var saveFormat = Path.GetExtension(_fileName).Equals(".pptx", StringComparison.InvariantCultureIgnoreCase)
                        ? Aspose.Slides.Export.SaveFormat.Pptx : Aspose.Slides.Export.SaveFormat.Ppt;
                    _doc.Save(_fileName, saveFormat);
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

            Aspose.Slides.IDocumentProperties docProps = _doc.DocumentProperties;

            for (int i = 0; i < docProps.CountOfCustomProperties; i++)
            {
                CustomProperty prop = properties.Add(docProps.GetCustomPropertyName(i));
                prop.Value = docProps[prop.Name];
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

            PowerpointPropertyConverter conv = new PowerpointPropertyConverter();
            Aspose.Slides.IDocumentProperties docProps = _doc.DocumentProperties;

            foreach (CustomProperty prop in properties)
            {
                if (prop.IsDeleted)
                {
                    if (docProps.ContainsCustomProperty(prop.Name))
                    {
                        docProps.RemoveCustomProperty(prop.Name);
                        _modified = true;
                    }
                }
                else//if (prop.HasChanged)
                {
                    object value = conv.ToSource(prop.Value);
                    object docProp = docProps.ContainsCustomProperty(prop.Name) ? docProps[prop.Name] : null;

                    if (docProp != null && docProp.GetType() != value?.GetType())
                    {
                        docProps.RemoveCustomProperty(prop.Name);
                        docProp = null;
                    }

                    if (docProp == null)
                    {
                        if (value == null || value.GetType() == typeof(string))
                            docProps.SetCustomPropertyValue(prop.Name, (string)value);
                        else if (value.GetType() == typeof(bool))
                            docProps.SetCustomPropertyValue(prop.Name, (bool)value);
                        else if (value.GetType() == typeof(int))
                            docProps.SetCustomPropertyValue(prop.Name, (int)value);
                        else if (value.GetType() == typeof(double))
                            docProps.SetCustomPropertyValue(prop.Name, (double)value);
                        else if (value.GetType() == typeof(DateTime))
                            docProps.SetCustomPropertyValue(prop.Name, (DateTime)value);
                        _modified = true;
                    }
                    else if (!Equals(docProp, value))
                    {
                        docProps[prop.Name] = value;
                        _modified = true;
                    }
                }
            }
        }

        #endregion

        #region PropertyConverter

        sealed class PowerpointPropertyConverter : IPropertyConverter<Type, object>
        {
            private ConversionMethods methods = new ConversionMethods();

            public object FromSource(object value, Type type)
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

            public Type ToSourceType(Type type)
            {
                if (type == typeof(bool) || type == typeof(DateTime) || type == typeof(double) || type == typeof(int) || type == typeof(string))
                    return type;
                else
                    return typeof(string);
            }

            public Type FromSourceType(Type type)
            {
                if (type == typeof(bool) || type == typeof(DateTime) || type == typeof(double) || type == typeof(int) || type == typeof(string))
                    return type;
                else
                    return typeof(string);
            }
        }

        #endregion
    }
}
