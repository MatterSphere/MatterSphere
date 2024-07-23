using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace IndexingController.Builders
{
    public class RelationBuilder : MultiBuilder
    {
        public override IEnumerable<ExpandoObject> CreateEntities(ExpandoObject model)
        {
            var modelPropertyDictionary = model as IDictionary<String, object>;
            var label = modelPropertyDictionary[_relationFieldName].ToString();
            var entityKeys = label.Split(',').Where(key => !string.IsNullOrWhiteSpace(key)).ToList();

            var entities = new List<ExpandoObject>();
            foreach (var entityKey in entityKeys)
            {
                var entity = new ExpandoObject() as IDictionary<String, object>;
                bool deleted = false;
                entity.Add(_entityDeleted, deleted);
                foreach (var modelProperty in modelPropertyDictionary)
                {
                    if (modelProperty.Key == _relationFieldName || modelProperty.Key == _entityIdFieldName)
                    {
                        continue;
                    }

                    if (modelProperty.Key == _entityKeyFieldName)
                    {
                        entity.Add(modelProperty.Key, entityKey);
                    }
                    else
                    {
                        entity.Add(modelProperty.Key, modelProperty.Value);
                    }
                }

                entities.Add((dynamic)entity);
            }

            return entities;
        }
    }
}
