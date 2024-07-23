using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Horizon.Common;
using Horizon.DAL;
using Horizon.Models.IndexReports;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IndexReports
{
    public class IndexStatusViewModel : PageViewModel
    {
        private Provider _provider;
        private Timer _timer;
        private int _interval = 5000;
        private string _queue;

        public IndexStatusViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            Entities = new ObservableCollection<EntityProcessItemViewModel>();
            _interval = mainViewModel.Settings.Interval > 0
                ? mainViewModel.Settings.Interval
                : _interval;
            _queue = MainViewModel.Settings.Queue;
            _timer = new Timer(CheckStatus, null, 100, _interval);
        }

        #region Properties

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
                }
            }
        }

        public ObservableCollection<EntityProcessItemViewModel> Entities { get; set; }

        private string _processTime;
        public string ProcessTime
        {
            get { return _processTime; }
            set
            {
                if (_processTime != value)
                {
                    _processTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _estimatedTime;
        public string EstimatedTime
        {
            get { return _estimatedTime; }
            set
            {
                if (_estimatedTime != value)
                {
                    _estimatedTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _queueItemsProcessed;
        public string QueueItemsProcessed
        {
            get { return _queueItemsProcessed; }
            set
            {
                if (_queueItemsProcessed != value)
                {
                    _queueItemsProcessed = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _leftQueueItems;
        public string LeftQueueItems
        {
            get { return _leftQueueItems; }
            set
            {
                if (_leftQueueItems != value)
                {
                    _leftQueueItems = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Private methods

        private async void CheckStatus(Object stateInfo)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            Task<long> queueTask = LoadQueueLength();
            Task<KeyValuePair<DateTime, List<EntityProcessItemViewModel>>> entitiesTask = LoadEntities();
            await Task.WhenAll(queueTask, entitiesTask);

            if (queueTask.Result > 0)
            {
                foreach (var item in entitiesTask.Result.Value)
                {
                    var entity = Entities.FirstOrDefault(ent => ent.Name == item.Name);
                    if (entity != null)
                    {
                        entity.Success = item.Success;
                        entity.Failed = item.Failed;
                        entity.Size = item.Size;
                    }
                    else
                    {
                        entity = new EntityProcessItemViewModel(item.Name, item.StartDate, item.Success, item.Failed, item.Messages, item.Size);
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            Entities.Add(entity);
                        });
                    }
                }

                UpdateStatus(queueTask.Result, entitiesTask.Result.Key,
                    entitiesTask.Result.Value.Sum(entity => entity.Messages));
            }
            else
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Entities.Clear();
                    ResetStatus();
                });
            }

            _timer.Change(_interval, _interval);
        }

        private async Task<long> LoadQueueLength()
        {
            return await Task.Run(() => GetQueueLength());
        }

        private long GetQueueLength()
        {
            var queueLengthResult = _provider.GetQueueLength(_queue);
            if (!queueLengthResult.Key.IsSuccess)
            {
                ShowErrorMessage(queueLengthResult.Key.ErrorMessage);
            }

            return queueLengthResult.Value;
        }

        private async Task<KeyValuePair<DateTime, List<EntityProcessItemViewModel>>> LoadEntities()
        {
            return await Task.Run(() => GetEntities());
        }

        private KeyValuePair<DateTime, List<EntityProcessItemViewModel>> GetEntities()
        {
            var processDetailResult = _provider.GetActualProcessDetail(_interval + 3);
            if (!processDetailResult.Key.IsSuccess)
            {
                ShowErrorMessage(processDetailResult.Key.ErrorMessage);
                return new KeyValuePair<DateTime, List<EntityProcessItemViewModel>>(DateTime.Now, new List<EntityProcessItemViewModel>());
            }

            var date = processDetailResult.Value.Any()
                    ? processDetailResult.Value.Min(entity => entity.StartDate)
                    : DateTime.Now;

            var entities = processDetailResult.Value
                .Select(s => new EntityProcessItemViewModel(s.Entity, s.StartDate, s.Success, s.Failed, s.Messages, s.Size))
                .OrderBy(it => it.Name).ToList();

            return new KeyValuePair<DateTime, List<EntityProcessItemViewModel>>(date, entities);
        }

        private void UpdateStatus(long queueLength, DateTime startDate, long processedNumber)
        {
            StartDate = startDate;

            var processTime = (DateTime.Now - startDate);
            ProcessTime = processTime.ToLabel();

            QueueItemsProcessed = processedNumber.ToLabel();
            LeftQueueItems = queueLength.ToLabel();

            var estimatedTime = queueLength > 0 && processedNumber > 0
                ? queueLength * processTime.Ticks / processedNumber
                : 0;
            EstimatedTime = estimatedTime.ToTime();
        }

        private void ResetStatus()
        {
            var time = new TimeSpan(0, 0, 0);
            long size = 0;
            ProcessTime = time.ToLabel();
            EstimatedTime = time.ToLabel();
            QueueItemsProcessed = size.ToLabel();
            LeftQueueItems = size.ToLabel();
        }

        #endregion
    }
}
