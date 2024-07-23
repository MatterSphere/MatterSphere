using System.Collections.Generic;
using System.Dynamic;
using Models.ElasticsearchModels;

namespace ElasticsearchProvider
{
    public class MappingBuilder
    {
        private const string _titleNormalizerName = "title_normalizer";
        private dynamic _properties;

        public MappingBuilder()
        {
            _properties = new ExpandoObject();
        }

        public dynamic Properties
        {
            get { return _properties; }
        }

        public void AddFields(IEnumerable<Field> fields)
        {
            var mappingProperties = _properties as IDictionary<string, object>;
            foreach (var field in fields)
            {
                var mappingProperty = CreateField(field);
                if (mappingProperties.ContainsKey(field.Name))
                {
                    mappingProperties[field.Name] = mappingProperty;
                }
                else
                {
                    mappingProperties.Add(field.Name, mappingProperty);
                }
            }
        }

        public void AddSuggestFields(List<string> fieldNames)
        {
            dynamic suggest = new ExpandoObject();
            suggest.type = "completion";
            suggest.analyzer = "standard";
            suggest.search_analyzer = "standard";
            suggest.preserve_separators = true;
            suggest.preserve_position_increments = true;
            var properties = _properties as IDictionary<string, object>;
            if (properties == null)
            {
                return;
            }
            foreach (var fieldName in fieldNames)
            {
                properties.Add($"{fieldName}Suggest", suggest);
            }
        }

        private dynamic CreateField(Field fld)
        {
            dynamic field = new ExpandoObject();
            field.type = fld.Type;

            if (fld.Facetable && fld.Type == "text")
            {
                field.fielddata = true;
            }

            if (fld.Type == "text" && fld.Analyzer == "keyword")
            {
                field.fielddata = true;
            }

            if (fld.Name == "title" || fld.Name == "summary")
            {
                field.fields = new
                {
                    raw = new
                    {
                        type = "keyword",
                        normalizer = _titleNormalizerName
                    }
                };
                field.fielddata = true;
            }

            if (!string.IsNullOrWhiteSpace(fld.Analyzer))
            {
                field.analyzer = fld.Analyzer;
            }

            if (fld.Facetable)
            {
                field.fields = new
                {
                    keyword = new
                    {
                        type = "keyword"
                    }
                };
            }

            return field;
        }
    }
}
