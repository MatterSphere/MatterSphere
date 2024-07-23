using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Horizon.Common.Models.Repositories.IndexStructure;
using Horizon.DAL;
using Horizon.Providers;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.Settings
{
    public class IndexEntityEditViewModel : BaseFormViewModel, IDataErrorInfo
    {
        private readonly Provider _provider;

        public IndexEntityEditViewModel(Provider provider, IndexEntityViewModel entity)
        {
            _provider = provider;

            Title = $"Editing selected '{entity.Name}' index";

            Id = entity.Id;
            Name = entity.Name;
            TableName = entity.TableName;
            Key = entity.Key;
            IndexingEnabled = entity.IndexingEnabled;
            IsDefault = entity.IsDefault;
            SummaryTemplate = entity.SummaryTemplate;
            AllowedFields = _provider.GetIndexFields(Id).Value.Select(x => x.Name).ToList();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Key { get; set; }
        public bool IsDefault { get; set; }
        public List<string> AllowedFields { get; set; }

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
                    OnPropertyChanged();
                }
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "SummaryTemplate")
                {
                    if (SummaryTemplate != null)
                    {
                        var regex = new Regex(@"{((\w|[-])+)}");
                        foreach (Match m in regex.Matches(SummaryTemplate))
                        {
                            var x = m.Groups[1].Value;
                            if (!AllowedFields.Any(f => f.Equals(x)))
                            {
                                return $"Field {m} is not valid!";
                            }
                        }
                    }
                }
                return null;
            }
        }

        protected override bool CanSave()
        {
            return AllFieldsAreAllowed(SummaryTemplate);
        }

        protected override void Save()
        {
            var result = _provider.UpdateSummaryTemplate(new IndexEntity(Id, Name, TableName, Key)
            {
                SummaryTemplate = SummaryTemplate
            });

            if (result.IsSuccess)
            {
                Result = true;
                this.Close();
            }
            else
            {
                var provider = new ErrorMessageProvider();
                provider.ShowErrorMessage(Window, result.ErrorMessage);
            }
        }

        private bool AllFieldsAreAllowed(string summaryTemplate)
        {
            if (summaryTemplate != null)
            {
                var regex = new Regex(@"{((\w|[-])+)}");
                foreach (Match m in regex.Matches(summaryTemplate))
                {
                    var x = m.Groups[1].Value;
                    if (!AllowedFields.Any(f => f.Equals(x)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
