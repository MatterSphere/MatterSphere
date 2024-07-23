using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Elasticsearch.Interfaces;
using FWBS.Common.Elasticsearch;

namespace Elasticsearch.Provider
{
    public class SearchQueryBuilder : ISearchQueryBuilder
    {
        private const string _objectType = "objecttype";
        private const string _matterSphereKey = "mattersphereid";
        private int _filtersNumber;
        private const string _startTag = "<highlight>";
        private const string _endTag = "</highlight>";
        private readonly string[] _highlights = new []
        {
            "assocSalut",
            "authorType",
            "docDesc",
            "fileDesc",
            "contName",
            "name",
            "title",
            "summary",
            "clientType",
            "clientNum",
            "clientName",
            "address",
            "precCategory",
            "precSubCategory",
            "precMinorCategory",
            "precDesc"
        };

        public dynamic CreateSearchRequestBody(SearchFilter filter, string[] searchableFields, string[] facetableFields, int facetSize, string[] accessList = null)
        {
            dynamic body = new ExpandoObject();
            if (filter != null && filter.SortInfo != null)
            {
                ApplySortInfo(body, filter.SortInfo);
            }

            if (filter != null && filter.PageInfo != null)
            {
                ApplyPaginationInfo(body, filter.PageInfo.Page, filter.PageInfo.Size);
            }

            body._source = CreateSourceSettings();
            body.query = CreateSearchQuery(filter, searchableFields, accessList);

            if (facetSize > 0)
            {
                var aggregations = CreateAggregation(facetableFields, facetSize);

                if (aggregations != null)
                {
                    body.aggs = aggregations;
                }
            }

            if (filter != null && filter.WithHighlights)
            {
                body.highlight = CreateHighlights();
            }

            return body;
        }

        public dynamic CreateSuggestRequestBody(string queryString, string[] suggests, int size)
        {
            var body = new
            {
                suggest = new ExpandoObject() as IDictionary<string, object>
            };

            foreach (var suggest in suggests)
            {
                var parameter = new
                {
                    prefix = queryString,
                    completion = new
                    {
                        field = suggest,
                        skip_duplicates = true,
                        size = size
                    }
                };

                body.suggest.Add(suggest, parameter);
            }

            return body;
        }

        public dynamic CreateUserRequestBody(string adName, string sqlName)
        {
            dynamic body = new ExpandoObject();
            dynamic query = new ExpandoObject();
            query.@bool = new
            {
                should = new object[2]
                {
                    new
                    {
                        term = new
                        {
                            usrad = adName
                        }
                    },
                    new
                    {
                        term = new
                        {
                            usrsql = sqlName
                        }
                    }
                },
                minimum_should_match = 1
            };

            body.query = query;
            return body;
        }

        private void ApplySortInfo(dynamic body, SearchFilter.SortData sortInfo)
        {
            var sortOrder = new Dictionary<string, string>();
            sortOrder[sortInfo.Field] = sortInfo.Order;
            body.sort = new object[]
            {
                sortOrder
            };
        }

        private void ApplyPaginationInfo(dynamic body, int page, int size)
        {
            body.size = size;
            body.from = page * size;
        }

        private dynamic CreateSourceSettings()
        {
            dynamic source = new ExpandoObject();
            var excludeFields = new List<string>
            {
                "docContents",
                "clientNotes",
                "ugdp",
                "*Suggest"
            };

            source.excludes = excludeFields.ToArray();
            return source;
        }

        private int FilterGroupsNumber(SearchFilter filter)
        {
            int number = 0;
            if (filter.HasEntityFilter)
            {
                number++;
            }

            if (filter.HasTypesFilter)
            {
                number++;
            }

            if (filter.FieldsFilter.Any(f => f.EntityType == EntityTypeEnum.Unknown))
            {
                number++;
            }

            return number;
        }

        private dynamic CreateSearchQuery(SearchFilter filter, string[] searchableFields, string[] ugdp = null)
        {
            _filtersNumber = 0;
            dynamic query = new ExpandoObject();
            int mustNumber = FilterGroupsNumber(filter) + (ugdp != null ? 3 : 2);

            query.@bool = new
            {
                must = new object[mustNumber],
                must_not = new object[]
                {
                    new
                    {
                        term = new
                        {
                            objecttype = "CCLINK"
                        }
                    }
                }
            };

            dynamic queryString = new ExpandoObject();
            query.@bool.must[0] = new
            {
                // https://www.elastic.co/guide/en/elasticsearch/reference/7.17/query-dsl-simple-query-string-query.html
                // The simple_query_string query does not return errors for invalid syntax.
                // Instead, it ignores any invalid parts of the query string.
                simple_query_string = queryString
            };
            query.@bool.must[0].simple_query_string.query = filter.Query;

            if (searchableFields != null && searchableFields.Any())
            {
                query.@bool.must[0].simple_query_string.fields = searchableFields.ToArray();
                query.@bool.must[0].simple_query_string.lenient = true;
            }

            _filtersNumber++;

            if (filter.HasTypesFilter)
            {
                AddTypesFilter(query, filter);
            }

            if (filter.HasEntityFilter)
            {
                AddEntityFilter(query, filter.EntityFilter, filter.LinkedEntityFilter);
            }

            if (filter.HasFieldsFilter)
            {
                AddFieldsFilter(query, filter.FieldsFilter.Where(f => f.EntityType == EntityTypeEnum.Unknown).ToList());
            }

            if (ugdp != null)
            {
                var allowList = ugdp.Select(item => $"{item}(ALLOW)").ToArray();
                var denyList = ugdp.Select(item => $"{item}(DENY)").ToArray();

                query.@bool.must[_filtersNumber] = new
                {
                    @bool = new
                    {
                        must_not = new object[]
                        {
                            new
                            {
                                terms = new
                                {
                                    ugdp = denyList
                                }
                            }
                        },
                        should = new object[]
                        {
                            new
                            {
                                terms = new
                                {
                                    ugdp = allowList
                                }
                            }
                        }
                    }
                };
            }

            query.@bool.must[mustNumber-1] = new
            {
                exists = new
                {
                    field = "objecttype"
                }
            };

            return query;
        }

        private dynamic CreateHighlights()
        {
            var fields = new ExpandoObject();
            foreach (var highlight in _highlights)
            {
                AddObjectProperty(fields, highlight);
            }

            return new
            {
                pre_tags = _startTag,
                post_tags = _endTag,
                fields = fields
            };
        }

        private dynamic CreateAggregation(string[] facetableFields, int size)
        {
            if (facetableFields == null || !facetableFields.Any())
            {
                return null;
            }

            dynamic aggregations = new ExpandoObject();
            var dictionary = aggregations as IDictionary<string, object>;

            for (int i = 0; i < facetableFields.Length; i++)
            {
                dictionary[facetableFields[i]] = new
                {
                    terms = new
                    {
                        field = $"{facetableFields[i]}.keyword",
                        size = size
                    }
                };
            }

            return aggregations;
        }

        private List<SearchFilter.FieldFilterItem> TakeConditions(SearchFilter searchFilter, EntityTypeEnum entityType)
        {
            var conditions = new List<SearchFilter.FieldFilterItem>();
            if (entityType != EntityTypeEnum.Unknown)
            {
                conditions.AddRange(searchFilter.FieldsFilter.Where(f => f.EntityType == entityType));
                searchFilter.FieldsFilter.RemoveAll(f => f.EntityType == entityType);
            }
            return conditions;
        }

        private List<SearchFilter.FieldFilterItem> GetRangeLimits(List<SearchFilter.FieldFilterItem> filter, string fieldName)
        {
            return filter.Where(f => f.Operator != ComparisonOperator.EqualTo && fieldName.Equals(f.TargetField ?? f.Field)).ToList();
        }

        private SearchFilter.FieldFilterItem GetTermLimit(List<SearchFilter.FieldFilterItem> filter, string fieldName)
        {
            return filter.FirstOrDefault(f => f.Operator == ComparisonOperator.EqualTo && fieldName.Equals(f.TargetField ?? f.Field));
        }


        private void AddTypesFilter(dynamic query, SearchFilter searchFilter)
        {
            query.@bool.must[_filtersNumber] = new
            {
                @bool = new
                {
                    should = new object[searchFilter.TypesFilter.Count],
                    minimum_should_match = 1
                }
            };

            var subQuery = query.@bool.must[_filtersNumber].@bool;

            var counter = 0;
            foreach (var entityType in searchFilter.TypesFilter)
            {
                dynamic filter = new ExpandoObject();

                List<SearchFilter.FieldFilterItem> conditions = TakeConditions(searchFilter, entityType);
                if (conditions.Count > 0)
                {
                    filter.@bool = new
                    {
                        must = new object[conditions.Select(f => f.TargetField ?? f.Field).Distinct().Count() + 1]
                    };
                    filter.@bool.must[0] = new
                    {
                        term = new
                        {
                            objecttype = new
                            {
                                value = EntityType.Convert(entityType).ToUpper()
                            }
                        }
                    };
                    var conditionCounter = 1;
                    foreach (var fieldName in conditions.Select(f => f.TargetField ?? f.Field).Distinct())
                    {
                        filter.@bool.must[conditionCounter] = CreateCondition(fieldName, conditions);

                        conditionCounter++;
                    }
                }
                else
                {
                    dynamic term = new ExpandoObject();
                    AddProperty(term, _objectType, EntityType.Convert(entityType).ToUpper());
                    filter.term = term;
                }

                subQuery.should[counter] = filter;
                counter++;
            }

            _filtersNumber++;
        }

        private dynamic CreateCondition(string fieldName, List<SearchFilter.FieldFilterItem> searchFilter)
        {
            if (GetTermLimit(searchFilter, fieldName) != null)
            {
                return new
                {
                    term = CreateTermFilter(fieldName, GetTermLimit(searchFilter, fieldName))
                };
            }
            else
            {
                return new
                {
                    range = CreateRangeFilter(fieldName, GetRangeLimits(searchFilter, fieldName))
                };
            }
        }

        private dynamic CreateTermFilter(string fieldName, SearchFilter.FieldFilterItem termLimit)
        {
            dynamic termFilter = new ExpandoObject();

            var termDict = termFilter as IDictionary<string, object>;
            if (termDict != null)
                termDict[fieldName] = new Dictionary<string, object>() { { "value", termLimit.Value } };

            return termFilter;
        }

        private static dynamic CreateRangeFilter(string fieldName, List<SearchFilter.FieldFilterItem> rangeLimits)
        {
            dynamic rangeLimit = new ExpandoObject();

            var lowerLimit = rangeLimits.FirstOrDefault(f => f.Operator == ComparisonOperator.GreaterOrEqual);
            if (lowerLimit != null)
            {
                rangeLimit.gte = lowerLimit.Value;
            }
            var upperLimit = rangeLimits.FirstOrDefault(f => f.Operator == ComparisonOperator.LessOrEqual);
            if (upperLimit != null)
            {
                rangeLimit.lte = upperLimit.Value;
            }

            return new Dictionary<string, object> { { fieldName, rangeLimit } };
        }

        private void AddEntityFilter(dynamic query, SearchFilter.EntityFilterData entityFilter, List<SearchFilter.EntityFilterData> linkedEntities)
        {
            query.@bool.must[_filtersNumber] = new
            {
                @bool = new
                {
                    should = new object[1 + entityFilter.Fields.Count + (linkedEntities == null ? 0 : linkedEntities.Count)]
                }
            };

            var subQuery = query.@bool.must[_filtersNumber].@bool;
            var nextFilterIndex = 0;

            AddFilterExpression(subQuery, entityFilter, nextFilterIndex);
            nextFilterIndex++;

            for (int i = 0; i < entityFilter.Fields.Count; i++)
            {
                dynamic linkFilter = new ExpandoObject();
                dynamic linkTerm = new ExpandoObject();
                AddProperty(linkTerm, entityFilter.Fields[i], entityFilter.Value);
                linkFilter.term = linkTerm;
                subQuery.should[nextFilterIndex] = linkFilter;
                nextFilterIndex++;
            }

            if (linkedEntities != null)
            {
                foreach (var linkedEntity in linkedEntities)
                {
                    AddFilterExpression(subQuery, linkedEntity, nextFilterIndex);
                    nextFilterIndex++;
                }
            }

            _filtersNumber++;
        }

        private void AddFilterExpression(dynamic query, SearchFilter.EntityFilterData filterEntity, int filterIndex)
        {
            query.should[filterIndex] = new
            {
                @bool = new
                {
                    must = new object[2]
                }
            };

            dynamic typeFilter = new ExpandoObject();
            dynamic typeTerm = new ExpandoObject();
            AddProperty(typeTerm, _objectType, EntityType.Convert(filterEntity.EntityType).ToUpper());
            typeFilter.term = typeTerm;

            dynamic idFilter = new ExpandoObject();
            dynamic idTerm = new ExpandoObject();
            AddProperty(idTerm, _matterSphereKey, filterEntity.Value);
            idFilter.term = idTerm;

            query.should[filterIndex].@bool.must[0] = typeFilter;
            query.should[filterIndex].@bool.must[1] = idFilter;
        }

        private void AddFieldsFilter(dynamic query, List<SearchFilter.FieldFilterItem> fields)
        {
            if (fields == null || fields.Count == 0)
                return;

            query.@bool.must[_filtersNumber] = new
            {
                @bool = new
                {
                    must = new object[fields.Count]
                }
            };

            var subQuery = query.@bool.must[_filtersNumber].@bool;

            var counter = 0;
            foreach (var field in fields)
            {
                dynamic filter = new ExpandoObject();
                dynamic term = new ExpandoObject();
                AddProperty(term, field.Field, field.Value);
                filter.term = term;
                subQuery.must[counter] = filter;
                counter++;
            }

            _filtersNumber++;
        }

        private void AddProperty(dynamic expando, string propertyName, string propertyValue)
        {
            var dictionary = expando as IDictionary<string, object>;
            if (dictionary == null)
            {
                return;
            }

            if (dictionary.ContainsKey(propertyName))
            {
                dictionary[propertyName] = new
                {
                    value = propertyValue
                };
            }
            else
            {
                dictionary.Add(propertyName, new
                {
                    value = propertyValue
                });
            }
        }

        private void AddObjectProperty(dynamic expando, string propertyName)
        {
            var dictionary = expando as IDictionary<string, object>;
            if (dictionary == null)
            {
                return;
            }

            if (dictionary.ContainsKey(propertyName))
            {
                dictionary[propertyName] = new {};
            }
            else
            {
                dictionary.Add(propertyName, new {});
            }
        }
    }
}
