using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Horizon.Annotations;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Models.IndexReports;
using Horizon.Providers;

namespace Horizon.ViewModels.IndexReports
{
    public class DocumentBucketViewModel : INotifyPropertyChanged
    {
        public DocumentBucketViewModel(IReportItem item)
        {
            Type = item.Type;
            Success = item.Success;
            Failed = item.Failed;
            EmptyContent = item.EmptyContent;

            Blacklist = new List<BlacklistItem>();
        }

        public string Type { get; set; }
        public long Success { get; set; }
        public long Failed { get; set; }
        public long EmptyContent { get; set; }

        private string _infoMessage;
        public string InfoMessage
        {
            get { return _infoMessage; }
            set
            {
                if (_infoMessage != value)
                {
                    _infoMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _recommendationMessage;
        public string RecommendationMessage
        {
            get { return _recommendationMessage; }
            set
            {
                if (_recommendationMessage != value)
                {
                    _recommendationMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public IFilterInfo IFilterInfo { get; set; }
        public List<BlacklistItem> Blacklist { get; set; }

        public string IFilterName
        {
            get
            {
                return IFilterInfo != null
                    ? IFilterInfo.FileName
                    : "no filter";
            }
        }

        public string IsLocatedInBlacklist
        {
            get
            {
                if (Blacklist.Any())
                {
                    return (Blacklist.Any(it => it.FullExtension))
                        ? "Blacklisted"
                        : "Partially Blacklisted";
                }

                return "Not Blacklisted";
            }
        }

        public void SetBREData(BREData data)
        {
            InfoMessage = data.InfoMessage;
            RecommendationMessage = data.RecommendationMessage;
            Status = data.Status;
            Blacklist = data.Blacklist;
            IFilterInfo = data.IFilterInfo;
            OnPropertyChanged(nameof(IsLocatedInBlacklist));
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
