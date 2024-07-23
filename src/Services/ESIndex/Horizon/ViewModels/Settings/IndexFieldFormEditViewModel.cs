using System;
using System.Collections.Generic;
using System.Linq;
using IndexField = Horizon.Models.Settings.IndexField;
using IndexFieldData = Horizon.Common.Models.Repositories.IndexStructure.IndexField;
using Horizon.DAL;
using Horizon.Providers;

namespace Horizon.ViewModels.Settings
{
    public class IndexFieldFormEditViewModel : IndexFieldFormViewModel
    {
        public IndexFieldFormEditViewModel(
                Provider provider,
                short entityId, string defaultTableName,
                string pkFieldName,
                List<string> defaultFields,
                List<string> extendedFields,
                Dictionary<string, string> facetableLookups,
                IndexField model) : base(provider, entityId, defaultTableName, pkFieldName, defaultFields, extendedFields, facetableLookups)
        {
            Title = $"Editing selected '{model.Name}' index field";
            if (string.IsNullOrEmpty(model.ExtendedTable))
            {
                TableName = defaultTableName;
                SelectedRadioButton = FieldSource.DefaultTable;
            }
            else
            {
                TableName = model.ExtendedTable;
                SelectedRadioButton = FieldSource.ExtendedData;
                SelectedTableField = TableFields.FirstOrDefault(f => f.Field.Equals(model.Name, StringComparison.OrdinalIgnoreCase));
            }
            ESField = model.Name;
            SelectedESType = model.FieldType;
            FieldCode = _defaultFieldCode = model.FieldCode;
            FieldDesc = _defaultFieldDesc = model.FieldDesc;
            Searchable = model.Searchable;
            Facetable = model.Facetable;
            FacetOrder = model.FacetOrder;
            Suggestable = model.Suggestable;
            if (!string.IsNullOrEmpty(model.FieldCodeLookupGroup))
            {
                SelectedCodeLookupGroup = model.FieldCodeLookupGroup;
                CodeLookupGroupEnabled = true;
            }

            SetAnalyzer(model.Analyzer);
        }

        public override bool CodeLookupGroupEnabled { get; }

        public override bool FieldPropertyEnabled
        {
            get
            {
                return true;
            }
        }

        private void SetAnalyzer(string analyzer)
        {
            analyzer = string.IsNullOrEmpty(analyzer) ? _withoutAnalyzer : analyzer; 
            if (Analyzers.Any(x => x.Equals(analyzer)))
            {
                SelectedAnalyzer = analyzer;
            }
            else
            {
                SelectedAnalyzer = _language;
                LanguageAnalyzer = analyzer;
            }
        }

        protected override bool CanSave()
        {
            return SelectedESType != null
                   && !string.IsNullOrWhiteSpace(ESField)
                   && (!Facetable || !string.IsNullOrWhiteSpace(FieldDesc))
                   && SelectedAnalyzer != null && (SelectedAnalyzer != _language || !string.IsNullOrWhiteSpace(LanguageAnalyzer));
        }

        protected override void Save()
        {
            if (Facetable)
            {
                ValidateFacetableFieldLookup();
            }

            var result = _provider.UpdateIndexField(new IndexFieldData(_entityId, SelectedTableField?.Field, ESField.ToLower(), SelectedESType)
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
                FieldCodeLookupGroup = SelectedCodeLookupGroup
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
    }
}
