using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;
using Horizon.Common.Models.Repositories.IndexStructure;

namespace Horizon.ViewModels.Settings
{
    public class FacetableFieldViewModel : INotifyPropertyChanged
    {
        public FacetableFieldViewModel(IndexFieldRow indexField)
        {
            IndexId = indexField.IndexId;
            EntityId = indexField.EntityId;
            Name = indexField.Name;
            FieldType = indexField.FieldType;
            Searchable = indexField.Searchable;
            Facetable = indexField.Facetable;
            _facetOrder = indexField.FacetOrder;
            Suggestable = indexField.Suggestable;
            Analyzer = indexField.Analyzer;
            IsDefault = indexField.IsDefault;
            ExtendedTable = indexField.ExtendedTable;
            FieldCode = indexField.FieldCode;
            FieldCodeLookupGroup = indexField.FieldCodeLookupGroup;
        }

        public short IndexId { get; set; }
        public short EntityId { get; set; }
        public string EntityName { get; set; }
        public string Name { get; set; }
        public string FieldType { get; set; }
        public bool Searchable { get; set; }
        public bool Facetable { get; set; }
        private byte? _facetOrder;
        public byte? FacetOrder
        {
            get { return _facetOrder; }
            set
            {
                if (_facetOrder != value)
                {
                    _facetOrder = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        public byte FacetSort
        {
            get { return _facetOrder.GetValueOrDefault(byte.MaxValue); }
        }
        public string FacetTitle
        {
            get { return $"[{FieldCode}] : {FieldDesc}"; }
        }
        public bool Suggestable { get; set; }
        public string Analyzer { get; set; }
        public bool IsDefault { get; set; }
        public string ExtendedTable { get; set; }
        public string FieldCode { get; set; }
        public string FieldDesc { get; set; }
        public string FieldCodeLookupGroup { get; set; }
        public bool IsDirty { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
