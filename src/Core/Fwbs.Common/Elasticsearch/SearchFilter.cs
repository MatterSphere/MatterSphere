using System.Collections.Generic;

namespace FWBS.Common.Elasticsearch
{
    public enum ComparisonOperator
    {
        EqualTo = 0,
        GreaterOrEqual = 1,
        LessOrEqual = -1
    }

    public class SearchFilter
    {
        public SearchFilter(string query)
        {
            Query = query;
            PageInfo = new PageData();
            TypesFilter = new List<EntityTypeEnum>();
            FieldsFilter = new List<FieldFilterItem>();
        }
        
        public string Query { get; set; }
        public PageData PageInfo { get; set; }
        public SortData SortInfo { get; set; }
        public EntityFilterData EntityFilter { get; set; }
        public List<EntityFilterData> LinkedEntityFilter { get; set; }
        public List<EntityTypeEnum> TypesFilter { get; set; }
        public List<FieldFilterItem> FieldsFilter { get; set; }
        public bool WithHighlights { get; set; }

        public bool HasEntityFilter
        {
            get { return EntityFilter != null; }
        }

        public bool HasTypesFilter
        {
            get { return TypesFilter.Count > 0; }
        }

        public bool HasFieldsFilter
        {
            get { return FieldsFilter.Count > 0; }
        }

        public class EntityFilterData
        {
            public EntityFilterData(EntityTypeEnum entityType, string value)
            {
                EntityType = entityType;
                Value = value;
                Fields = GetKeyFieldsByEntityType(entityType);
                EntityTypeLabel = Elasticsearch.EntityType.Convert(EntityType);
            }

            public EntityTypeEnum EntityType { get; private set; }
            public string Value { get; private set; }
            public List<string> Fields { get; private set; }
            public string EntityTypeLabel { get; private set; }

            private List<string> GetKeyFieldsByEntityType(EntityTypeEnum entityType)
            {
                switch (entityType)
                {
                    case EntityTypeEnum.Associate:
                        return new List<string> { "associate-id", "associateId" };
                    case EntityTypeEnum.Contact:
                        return new List<string> {"contact-id", "contactId" };
                    case EntityTypeEnum.Client:
                        return new List<string> { "client-id", "clientId" };
                    case EntityTypeEnum.File:
                        return new List<string> { "file-id", "fileId" };
                    default:
                        return null;
                }
            }
        }

        public class FieldFilterItem
        {
            public FieldFilterItem(string field, string value)
            {
                Field = field;
                Value = value;
            }

            public EntityTypeEnum EntityType { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
            public string TargetField { get; set; }
            public ComparisonOperator Operator { get; set; }
        }

        public class PageData
        {
            public PageData()
            {
                Size = 50;
            }

            public PageData(int page, int size)
            {
                Page = page;
                Size = size;
            }

            public int Size { get; set; }
            public int Page { get; set; }
        }
        
        public class SortData
        {
            public string Field { get; set; }
            public string Order { get; set; }
        }
    }
}
