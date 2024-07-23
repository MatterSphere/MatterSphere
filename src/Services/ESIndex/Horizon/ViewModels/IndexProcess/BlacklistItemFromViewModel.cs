using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;

namespace Horizon.ViewModels.IndexProcess
{
    public class BlacklistItemFromViewModel : INotifyPropertyChanged
    {
        public BlacklistItemFromViewModel(string extension)
        {
            Extension = extension;
        }

        private string _extension;
        public string Extension
        {
            get { return _extension; }
            set
            {
                if (_extension != value)
                {
                    _extension = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _metadata;
        public string Metadata
        {
            get { return _metadata; }
            set
            {
                if (_metadata != value)
                {
                    _metadata = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _encoding;
        public string Encoding
        {
            get { return _encoding; }
            set
            {
                if (_encoding != value)
                {
                    _encoding = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maxSize;
        public string MaxSize
        {
            get
            {
                return _maxSize;
            }
            set
            {
                if (_maxSize != value)
                {
                    _maxSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSave
        {
            get
            {
                string extension = (_extension ?? string.Empty).Trim(' ', '.');
                return (extension.Length > 0 && extension.Length <= 15)
                       && (Metadata == null || Metadata.Length <= 1000)
                       && (Encoding == null || Encoding.Length <= 30)
                       && MaxSizeIsValid
                       && (!MaxSizeValidValue.HasValue || MaxSizeValidValue.Value > 0);
            }
        }

        public string ExtensionValidValue { get { return Extension.ToLower().Trim(' ', '.'); } }
        public string MetadataValidValue { get { return Metadata != null ? Metadata.Trim() : string.Empty; } }
        public string EncodingValidValue { get { return Encoding != null ? Encoding.Trim() : string.Empty; } }

        public long? MaxSizeValidValue
        {
            get
            {
                long maxSize;
                return Int64.TryParse(MaxSize, out maxSize)
                    ? maxSize
                    : (long?)null;
            }
        }

        private bool MaxSizeIsValid
        {
            get
            {
                long maxSize;
                return string.IsNullOrEmpty(MaxSize) || Int64.TryParse(MaxSize, out maxSize);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
