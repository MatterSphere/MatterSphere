using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Annotations;
using Horizon.Common;
using Horizon.Common.Interfaces;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.ProcessingStatus
{
    public class ProcessHistoryItemViewModel : INotifyPropertyChanged
    {
        public delegate void ErrorDelegate(string errorMessage);
        public event ErrorDelegate onError;
        private IDbProvider _provider;
        private PageViewModel _parrent;

        public ProcessHistoryItemViewModel(int id, DateTime startDate, IDbProvider provider, ProcessingStatusViewModel parrent)
        {
            Id = id;
            StartDate = startDate;
            _provider = provider;
            _parrent = parrent;
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public int ContentErrors { get; set; }

        private List<DetailItem> _detailItems;
        public List<DetailItem> DetailItems
        {
            get
            {
                if (_detailItems == null)
                {
                     GetDetails();
                }

                return _detailItems;
            }
            set
            {
                if (_detailItems != value)
                {
                    _detailItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StartLabel
        {
            get { return StartDate.ToLongTimeString(); }
        }

        public string FinishLabel
        {
            get
            {
                return FinishDate.HasValue
                  ? FinishDate.Value.ToLongTimeString()
                  : "not finished";
            }
        }

        public ProcessStatusEnum Status
        {
            get
            {
                if (FinishDate.HasValue)
                {
                    return Failed > 0
                        ? ProcessStatusEnum.CompletedWithErrors
                        : ProcessStatusEnum.SuccessfullyCompleted;
                }

                return ProcessStatusEnum.NotCompleted;
            }
        }

        private DetailItem _selectedEntityItem;
        public DetailItem SelectedEntityItem
        {
            get { return _selectedEntityItem; }
            set
            {
                if (_selectedEntityItem != value)
                {
                    _selectedEntityItem = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Methods

        public async void GetDetails()
        {
            _detailItems = await Task.Run(() => LoadDetails());
            OnPropertyChanged(nameof(DetailItems));
        }

        private List<DetailItem> LoadDetails()
        {
            var result = _provider.GetProcessHistoryDetail(Id);
            if (result.Key.IsSuccess)
            {
                int index = 1;
                return result.Value.Select(proc => new DetailItem(Id, proc.Entity, proc.StartDate)
                {
                    Index = index++,
                    FinishDate = proc.FinishDate,
                    Successful = proc.Successful,
                    Failed = proc.Failed,
                    ContentErrors = proc.ContentErrors,
                    Size = proc.Size
                }).OrderBy(proc => proc.Index).ToList();
            }

            if (onError != null)
            {
                onError(result.Key.ErrorMessage);
            }

            return new List<DetailItem>();
        }

        #endregion

        #region Commands

        #region OpenDocumentErrorList
        private ICommand _openDocumentErrorGroupsCommand;
        public ICommand OpenDocumentErrorGroupsCommand
        {
            get
            {
                if (_openDocumentErrorGroupsCommand == null)
                {
                    _openDocumentErrorGroupsCommand = new RelayCommand(
                        rc => this.CanOpenDocumentErrorGroups(),
                        rc => this.OpenDocumentErrorGroups());
                }
                return _openDocumentErrorGroupsCommand;
            }
        }

        private bool CanOpenDocumentErrorGroups()
        {
            return SelectedEntityItem != null && SelectedEntityItem.Entity == "DOCUMENT";
        }

        private void OpenDocumentErrorGroups()
        {
            var nextPage = new ErrorCodesViewModel(_parrent.MainViewModel, SelectedEntityItem.ProcessId)
            {
                PreviousPage = _parrent
            };
            _parrent.MainViewModel.CurrentPage = nextPage;
        }
        #endregion

        #endregion

        #region Classes

        public class DetailItem
        {
            public DetailItem(int processId, string entity, DateTime startDate)
            {
                ProcessId = processId;
                Entity = entity;
                StartDate = startDate;
            }

            public int Index { get; set; }
            public int ProcessId { get; set; }
            public string Entity { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? FinishDate { get; set; }
            public int Successful { get; set; }
            public int Failed { get; set; }
            public int ContentErrors { get; set; }
            public long Size { get; set; }

            public string SizeLabel
            {
                get { return Size.ToSize(); }
            }

            public string StartLabel
            {
                get { return StartDate.ToLongTimeString(); }
            }

            public string FinishLabel
            {
                get
                {
                    return FinishDate.HasValue
                        ? FinishDate.Value.ToLongTimeString()
                        : "not finished";
                }
            }
        }

        public enum ProcessStatusEnum
        {
            SuccessfullyCompleted,
            CompletedWithErrors,
            NotCompleted
        }

        #endregion

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
