using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;
using Horizon.Common;

namespace Horizon.Models.IndexReports
{
    public class EntityProcessItemViewModel : INotifyPropertyChanged
    {
        public EntityProcessItemViewModel(string name, DateTime date, int success, int failed, long messages, long size = 0)
        {
            Name = name;
            StartDate = date;
            Success = success;
            Failed = failed;
            Messages = messages;
            Size = size;
        }

        public string Name { get; set; }
        
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DateLabel));
                }
            }
        }

        private int _success;
        public int Success
        {
            get { return _success; }
            set
            {
                if (_success != value)
                {
                    _success = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Processed));
                }
            }
        }

        private int _failed;
        public int Failed
        {
            get { return _failed; }
            set
            {
                if (_failed != value)
                {
                    _failed = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Processed));
                }
            }
        }
        
        public int Processed
        {
            get { return Success + Failed; }
        }

        private long _size;
        public long Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SizeLabel));
                }
            }
        }

        public long Messages { get; set; }
        
        public string DateLabel
        {
            get { return $"{StartDate.ToShortDateString()} {StartDate.ToShortTimeString()}"; }

        }

        public string SizeLabel
        {
            get { return Size.ToSize(); }
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
