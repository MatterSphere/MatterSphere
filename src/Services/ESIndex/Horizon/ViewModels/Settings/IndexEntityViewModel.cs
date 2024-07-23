using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;
using Horizon.Common.Models.Repositories.IndexStructure;

namespace Horizon.ViewModels.Settings
{
    public class IndexEntityViewModel : INotifyPropertyChanged
    {
        public IndexEntityViewModel(IndexEntity entity, bool indexCreated)
        {
            Id = entity.Id;
            Name = entity.Name;
            TableName = entity.TableName;
            Key = entity.Key;
            IndexingEnabled = entity.IndexingEnabled;
            IsDefault = entity.IsDefault;
            _summaryTemplate = entity.SummaryTemplate;
            _indexCreated = indexCreated;
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Key { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDirty { get; set; }

        private bool _indexingEnabled;
        public bool IndexingEnabled
        {
            get { return _indexingEnabled; }
            set
            {
                if (_indexingEnabled != value)
                {
                    _indexingEnabled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ToolTipInfo));
                    OnPropertyChanged(nameof(IndexingEnabledToolTipInfo));
                }
            }
        }

        private bool? _visibleEntity;
        public bool VisibleEntity
        {
            get
            {
                if (!_visibleEntity.HasValue)
                {
                    var name = Name.ToLower();
                    _visibleEntity = name == "associate"
                                          || name == "client"
                                          || name == "contact"
                                          || name == "document"
                                          || name == "precedent"
                                          || name == "file"
                                          || name == "appointment"
                                          || name == "task"
                                          || name == "email"
                                          || name == "note";
                }

                return _visibleEntity.Value;
            }
        }

        private bool? _customizableEntity;
        public bool CustomizableEntity
        {
            get
            {
                if (!_customizableEntity.HasValue)
                {
                    var name = Name.ToLower();
                    _customizableEntity = name != "note";
                }

                return _customizableEntity.Value;
            }
        }

        public string ToolTipInfo
        {
            get
            {
                if (_indexCreated)
                {
                    return $"Customization not available after the first indexing process";
                }

                if (!IndexingEnabled)
                {
                    return $"Customization not available";
                }

                if (!CustomizableEntity)
                {
                    return $"{Name} is not customizable entity";
                }

                return $"Select to customize {Name}";
            }
        }

        public string IndexingEnabledToolTipInfo
        {
            get
            {
                if (IndexingEnabled && !IsDefault)
                {
                    return $"Select to exclude {Name} from indexing";
                }

                if (!IndexingEnabled && !IsDefault)
                    return $"Select to Index {Name}";

                return string.Empty;
            }
        }

        public bool Enabled
        {
            get
            {
                return !_indexCreated && !IsDefault;
            }
        }

        private bool _indexCreated;
        public bool IndexCreated
        {
            get
            {
                return _indexCreated;
            }
            set
            {
                if (_indexCreated != value)
                {
                    _indexCreated = value;
                    OnPropertyChanged(nameof(Enabled));
                    OnPropertyChanged(nameof(ToolTipInfo));
                    OnPropertyChanged(nameof(IndexingEnabledToolTipInfo));
                }
            }
        }

        private string _summaryTemplate;
        public string SummaryTemplate
        {
            get { return _summaryTemplate; }
            set
            {
                if (_summaryTemplate != value)
                {
                    _summaryTemplate = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
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
