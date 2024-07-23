using System;
using System.Collections.Generic;
using System.Linq;
using Horizon.Common.Models.Repositories.IndexStructure;
using Horizon.DAL;
using Horizon.Providers;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.Settings
{
    public class IndexFieldFormViewModel : BaseFormViewModel
    {
        private delegate void ChangeProperty();

        private event ChangeProperty onFieldSourceChanged;
        private event ChangeProperty onTableNameChanged;
        private event ChangeProperty onTableEnableChanged;
        private event ChangeProperty onTableFieldsChanged;
        private event ChangeProperty onSelectedTableFieldChanged;
        private event ChangeProperty onSelectedAnalyzerChanged;

        private string _defaultTableName;
        private string _pkFieldName;

        protected Provider _provider;
        protected short _entityId;
        protected string _defaultFieldCode, _defaultFieldDesc;
        protected List<string> _defaultFields;
        protected List<string> _extendedFields;

        protected const string _withoutAnalyzer = "without analyzer";
        protected const string _language = "language";
        
        public enum FieldSource
        {
            DefaultTable,
            ExtendedData
        }

        private IndexFieldFormViewModel()
        {
            onFieldSourceChanged += ChangeTableEnabling;
            onTableNameChanged += CheckTableName;
            onTableEnableChanged += ChangeTableName;
            onTableFieldsChanged += ChangeFieldEnabling;
            onSelectedTableFieldChanged += ChangeFieldPropertyEnabling;
            onSelectedAnalyzerChanged += CheckAnalyzer;
        }

        public IndexFieldFormViewModel(Provider provider, short entityId, string defaultTableName, string pkFieldName, List<string> defaultFields, List<string> extendedFields, Dictionary<string,string> facetableLookups) : this()
        {
            _provider = provider;
            _entityId = entityId;
            _defaultTableName = defaultTableName;
            _pkFieldName = pkFieldName;
            _defaultFields = defaultFields.Select(field => field.ToLower()).ToList();
            _extendedFields = extendedFields.Select(field => field.ToLower()).ToList();
            _facetableLookups = facetableLookups;

            SelectedRadioButton = FieldSource.ExtendedData;
            Searchable = true;

            Analyzers = new string[]
            {
                _withoutAnalyzer,
                "standard",
                "simple",
                "whitespace",
                "stop",
                "keyword",
                "pattern",
                _language,
                "fingerprint"
            };

            SelectedAnalyzer = _withoutAnalyzer;

            string languageAnalyzers = System.Configuration.ConfigurationManager.AppSettings["languageAnalyzers"] ?? string.Empty;
            Languages = languageAnalyzers.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        #region Properties
        private FieldSource _selectedRadioButton;
        public FieldSource SelectedRadioButton
        {
            get { return _selectedRadioButton; }
            set
            {
                if (_selectedRadioButton != value)
                {
                    _selectedRadioButton = value;
                    OnPropertyChanged();

                    onFieldSourceChanged?.Invoke();
                }
            }
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set
            {
                if (_tableName != value)
                {
                    _tableName = value;
                    OnPropertyChanged();

                    onTableNameChanged?.Invoke();
                }
            }
        }

        private bool _tableEnabled;
        public bool TableEnabled
        {
            get { return _tableEnabled; }
            set
            {
                if (_tableEnabled != value)
                {
                    _tableEnabled = value;
                    OnPropertyChanged();

                    onTableEnableChanged?.Invoke();
                }
            }
        }

        private bool _fieldEnabled;
        public bool FieldEnabled
        {
            get { return _fieldEnabled; }
            set
            {
                if (_fieldEnabled != value)
                {
                    _fieldEnabled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AnalyzerFieldEnabled));
                }
            }
        }

        public bool AnalyzerFieldEnabled
        {
            get
            {
                return FieldEnabled && !Facetable && SelectedESType == "text";
            }
        }

        private IEnumerable<TableField> _tableFields;
        public IEnumerable<TableField> TableFields
        {
            get { return _tableFields; }
            set
            {
                if (_tableFields != value)
                {
                    _tableFields = value;
                    OnPropertyChanged();

                    onTableFieldsChanged?.Invoke();
                }
            }
        }

        private TableField _selectedTableField;
        public TableField SelectedTableField
        {
            get { return _selectedTableField; }
            set
            {
                if (_selectedTableField != value)
                {
                    _selectedTableField = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CodeLookupGroupEnabled));

                    onSelectedTableFieldChanged?.Invoke();
                }
            }
        }

        private string _esField;
        public string ESField
        {
            get { return _esField; }
            set
            {
                if (_esField != value)
                {
                    _esField = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _fieldCode;
        public string FieldCode
        {
            get { return _fieldCode; }
            set
            {
                if (_fieldCode != value)
                {
                    _fieldCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _fieldDesc;
        public string FieldDesc
        {
            get { return _fieldDesc; }
            set
            {
                if (_fieldDesc != value)
                {
                    _fieldDesc = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly Dictionary<string, string> _facetableLookups;
        public Dictionary<string, string> FacetableLookups
        {
            get { return _facetableLookups; }
        }

        private bool _fieldPropertyEnabled;
        public virtual bool FieldPropertyEnabled
        {
            get { return _fieldPropertyEnabled; }
            set
            {
                if (_fieldPropertyEnabled != value)
                {
                    _fieldPropertyEnabled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FieldCodeEnabled));
                    OnPropertyChanged(nameof(FacetableEnabled));
                }
            }
        }

        private string _selectedESType;
        public string SelectedESType
        {
            get { return _selectedESType; }
            set
            {
                if (_selectedESType != value)
                {
                    _selectedESType = value;
                    OnPropertyChanged();

                    if (_selectedESType != "text")
                    {
                        Facetable = false;
                        SelectedAnalyzer = _withoutAnalyzer;
                    }

                    OnPropertyChanged(nameof(FacetableEnabled));
                    OnPropertyChanged(nameof(AnalyzerFieldEnabled));
                }
            }
        }

        public IEnumerable<string> Analyzers { get; private set; }

        private string _selectedAnalyzer;
        public string SelectedAnalyzer
        {
            get { return _selectedAnalyzer; }
            set
            {
                if (_selectedAnalyzer != value)
                {
                    _selectedAnalyzer = value;
                    OnPropertyChanged();

                    onSelectedAnalyzerChanged?.Invoke();
                }
            }
        }

        public IEnumerable<string> Languages { get; private set; }

        private string _languageAnalyzer;
        public string LanguageAnalyzer
        {
            get { return _languageAnalyzer; }
            set
            {
                if (_languageAnalyzer != value)
                {
                    _languageAnalyzer = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _languageEnabled;
        public bool LanguageEnabled
        {
            get { return _languageEnabled; }
            set
            {
                if (_languageEnabled != value)
                {
                    _languageEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _searchable;
        public bool Searchable
        {
            get { return _searchable; }
            set
            {
                if (_searchable != value)
                {
                    _searchable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _facetable;
        public bool Facetable
        {
            get { return _facetable; }
            set
            {
                if (_facetable != value)
                {
                    _facetable = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AnalyzerFieldEnabled));
                    OnPropertyChanged(nameof(FieldCodeEnabled));

                    if (value)
                    {
                        FieldCode = _defaultFieldCode;
                        FieldDesc = _defaultFieldDesc;
                        SelectedAnalyzer = Analyzers.First(analyzer => analyzer == "keyword");
                    }
                    else
                    {
                        FacetOrder = null;
                        FieldCode = null;
                        FieldDesc = null;
                        SelectedAnalyzer = _withoutAnalyzer;
                    }
                }
            }
        }

        private byte? _facetOrder;
        public byte? FacetOrder
        {
            get { return _facetOrder; }
            set
            {
                if (_facetOrder != value)
                {
                    _facetOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool FacetableEnabled
        {
            get { return FieldPropertyEnabled && SelectedESType == "text"; }
        }

        public bool FieldCodeEnabled
        {
            get { return Facetable && FieldPropertyEnabled; }
        }

        private bool _suggestable;
        public bool Suggestable
        {
            get { return _suggestable; }
            set
            {
                if (_suggestable != value)
                {
                    _suggestable = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _codeLookupGroups;
        public List<string> CodeLookupGroups
        {
            get
            {
                if (_codeLookupGroups == null)
                {
                    _codeLookupGroups = _provider.GetCodeLookupGroups().Value;
                }
                return _codeLookupGroups;
            }
        }

        private string _selectedCodeLookupGroup;
        public string SelectedCodeLookupGroup
        {
            get { return _selectedCodeLookupGroup; }
            set
            {
                if (_selectedCodeLookupGroup != value)
                {
                    _selectedCodeLookupGroup = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual bool CodeLookupGroupEnabled
        {
            get
            {
                return SelectedTableField != null && SelectedTableField.IsCodeLookup;
            }
        }

        #endregion

        #region Private methids

        private void ChangeTableEnabling()
        {
            TableEnabled = SelectedRadioButton == FieldSource.ExtendedData;
        }

        private void CheckTableName()
        {
            TableFields = new TableField[0];
            if (string.IsNullOrWhiteSpace(TableName))
            {
                return;
            }

            if (TableName.ToLower() == _defaultTableName.ToLower())
            {
                TableFields = GetTableFields(TableName);
            }
            else
            {
                var hasExtendedData = _provider.CheckExtendedData(TableName, _pkFieldName);
                if (hasExtendedData.Key.IsSuccess && hasExtendedData.Value)
                {
                    TableFields = GetTableFields(TableName);
                }
            }
        }

        private List<TableField> GetTableFields(string table)
        {
            var tableFields = _provider.GetTableFields(table);
            return tableFields.Key.IsSuccess
                ? tableFields.Value
                : null;
        }

        private void ChangeTableName()
        {
            SelectedTableField = null;
            SelectedESType = null;
            Searchable = true;
            Facetable = false;
            TableName = TableEnabled ? null : _defaultTableName;
        }

        private void ChangeFieldEnabling()
        {
            FieldEnabled = TableFields != null && TableFields.Any();
            if (!FieldEnabled)
            {
                SelectedTableField = null;
                SelectedESType = null;
                Searchable = true;
                Facetable = false;
            }
        }

        private void ChangeFieldPropertyEnabling()
        {
            FieldPropertyEnabled = SelectedTableField != null;
            if (FieldPropertyEnabled)
            {
                ESField = SelectedTableField.Field.ToLower();
                SelectedESType = SelectedTableField.ToElasticType();
            }
            else
            {
                ESField = null;
                SelectedESType = null;
            }

            if (!CodeLookupGroupEnabled)
                SelectedCodeLookupGroup = null;
        }

        private void CheckAnalyzer()
        {
            if (SelectedAnalyzer == _language)
            {
                LanguageEnabled = true;
            }
            else
            {
                LanguageEnabled = false;
                LanguageAnalyzer = null;
            }
        }
        #endregion
        
        protected string GetAnalyzer()
        {
            switch (SelectedAnalyzer)
            {
                case _withoutAnalyzer:
                    return null;
                case _language:
                    return LanguageAnalyzer;
                default:
                    return SelectedAnalyzer;
            }
        }

        #region Save command
        protected override bool CanSave()
        {
            return SelectedTableField != null
                && SelectedESType != null
                && !string.IsNullOrWhiteSpace(ESField)
                && (!Facetable || !string.IsNullOrWhiteSpace(FieldDesc))
                && (!CodeLookupGroupEnabled || !string.IsNullOrWhiteSpace(SelectedCodeLookupGroup))
                && SelectedAnalyzer != null && (SelectedAnalyzer != _language || !string.IsNullOrWhiteSpace(LanguageAnalyzer))
                && ((SelectedRadioButton == FieldSource.DefaultTable && _defaultFields.All(field => !field.Equals(ESField, StringComparison.OrdinalIgnoreCase)))
                    || _extendedFields.All(field => !field.Equals(ESField, StringComparison.OrdinalIgnoreCase)));
        }

        protected override void Save()
        {
            if (Facetable)
            {
                ValidateFacetableFieldLookup();
            }

            var result = _provider.AddField(new IndexField(_entityId, SelectedTableField.Field, ESField.ToLower(), SelectedESType)
            {
                Searchable = Searchable,
                Facetable = Facetable,
                FacetOrder = FacetOrder,
                FieldCode = FieldCode,
                Suggestable = Suggestable,
                Analyzer = GetAnalyzer(),
                ExtendedData = SelectedRadioButton == FieldSource.ExtendedData
                    ? TableName
                    : null,
                FieldCodeLookupGroup = SelectedTableField.IsCodeLookup
                    ? SelectedCodeLookupGroup
                    : null
            });

            if (result.Key.IsSuccess)
            {
                Result = true;
                this.Close();
            }
            else
            {
                var provider = new ErrorMessageProvider();
                provider.ShowErrorMessage(Window, result.Key.ErrorMessage);
            }
        }
        #endregion

        protected void ValidateFacetableFieldLookup()
        {
            if (_fieldCode == null || !_facetableLookups.ContainsKey(_fieldCode))
            {
                _fieldCode = (_esField.Length <= 15 ? _esField : _esField.Remove(15)).ToUpperInvariant();
                _fieldDesc = _fieldDesc.Trim();
                bool createNew = !_facetableLookups.ContainsKey(_fieldCode);
                var result = _provider.SetFacetableCodeLookup(_fieldCode, _fieldDesc, createNew);
                if (result.Key.IsSuccess)
                {
                    _facetableLookups[_fieldCode] = _fieldDesc;
                }
                else
                {
                    var provider = new ErrorMessageProvider();
                    provider.ShowErrorMessage(Window, result.Key.ErrorMessage);
                }
            }
        }
    }
}
